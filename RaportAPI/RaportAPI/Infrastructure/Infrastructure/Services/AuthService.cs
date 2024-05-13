using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SelfOnboardingManagement.Application.Auth.Command.DeleteOldRefreshTokens;
using SelfOnboardingManagement.Application.Auth.Command.PartnerLogin;
using SelfOnBoardingManagement.Application.Auth.Command.Login;
using SelfOnBoardingManagement.Application.Auth.Command.RefreshToken;
using SelfOnBoardingManagement.Application.Auth.Command.RevokeToken;
using SelfOnBoardingManagement.Application.Common.Interfaces;
using SelfOnBoardingManagement.Common.Exceptions;
using SelfOnBoardingManagement.Common.Models;
using SelfOnBoardingManagement.Domain.Entities;
using System.Data;


namespace SelfOnBoardingManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly Context _context;
        private readonly IJwtTokenUtils _jwtTokenUtils;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserRoleProvider _userRoleProvider;
        public AuthService(Context context, IJwtTokenUtils jwtTokenGenerator, IDateTimeProvider dateTimeProvider, IUserRoleProvider userRoleProvider)
        {
            _context = context;
            _jwtTokenUtils = jwtTokenGenerator;
            _dateTimeProvider = dateTimeProvider;
            _userRoleProvider = userRoleProvider;
        }

        public virtual async Task<LoginVm> Login(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _context.SbUsers.Where(u => u.Email.ToLower() == command.Email.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (user == null) throw new NotFoundException("Incorrect Email Address");
            if (user.IsActive == false) throw new NotActiveUserException();

            if (!BCrypt.Net.BCrypt.Verify(command.Password, user.Password)) throw new InvalidPasswordException();


            if(user.IsPasswordSet) 
            {
                return new LoginVm()
                {
                    IsLoggedIn = false,
                    Email = user.Email,
                    isPasswordSet = user.IsPasswordSet,
                    Token = null,
                    RefreshToken = null
                };
            }

            UserRole role = await _userRoleProvider.GetUserRoleAsync(user.IdRole);

            var jwtToken = await _jwtTokenUtils.GenerateToken(user.Id ,user.Email, role);
            var newRefreshToken = await _jwtTokenUtils.GenerateRefreshToken(command.IpAddress);
            var refreshToken = new UserRefreshToken()
            {
                IdUser = user.Id,
                Token = newRefreshToken.Token,
                IsActive = true,
                Expires = newRefreshToken.Expires,
                Created = newRefreshToken.Created,
                CreatedByIp = newRefreshToken.CreatedByIp,
                IsRevoked = false,
            };

            user.LastSignInDate = _dateTimeProvider.UtcNow;

            await _context.SbUserRefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new LoginVm()
            {
                IsLoggedIn = true,
                Email = user.Email,
                isPasswordSet = user.IsPasswordSet,
                Role = role,
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }

        public virtual async Task<RefreshTokenVm> GetRefreshToken(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var user = await getUserByRefreshToken(command.RefreshToken);
            if (user.IsActive == false) throw new NotActiveUserException();
            var refreshToken = await _context.SbUserRefreshTokens.FirstOrDefaultAsync(t => t.IdUser.Equals(user.Id) && t.Token.Equals(command.RefreshToken));

            if (refreshToken.IsRevoked)
            {
                 revokeDescendantRefreshTokens(
                    refreshToken,
                    user,
                    command.IpAddress,
                    $"Attempted reuse of revoked ancestor token: {command.RefreshToken}",
                    cancellationToken);
            }

            if (!refreshToken.IsActive)
                throw new InvalidTokenException("Invalid Refresh Token!");

            var newRefreshToken = await rotateRefreshTokenAsync(refreshToken, command.IpAddress, cancellationToken: cancellationToken);
            var newToken = new UserRefreshToken()
            {
                IdUser = user.Id,
                Token = newRefreshToken.Token,
                IsActive = true,
                Expires = newRefreshToken.Expires,
                Created = newRefreshToken.Created,
                CreatedByIp = newRefreshToken.CreatedByIp,
                IsRevoked = false,
            };

            await _context.SbUserRefreshTokens.AddAsync(newToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);


            UserRole role = await _userRoleProvider.GetUserRoleAsync(user.IdRole);

            var jwtToken = await _jwtTokenUtils.GenerateToken(user.Id, user.Email, role);

            return new RefreshTokenVm()
            {
                Email = user.Email,
                Role = role.ToString(),
                Token = jwtToken,
                RefreshToken = newToken.Token
            };
        }

        public virtual async Task RevokeToken(RevokeTokenCommand command, CancellationToken cancellationToken)
        {
            var user = await getUserByRefreshToken(command.RefreshToken);
            var refreshToken = await _context.SbUserRefreshTokens.FirstOrDefaultAsync(t => t.IdUser.Equals(user.Id) && t.Token.Equals(command.RefreshToken));

            if (!refreshToken.IsActive)
                throw new InvalidTokenException("Invalid Refresh Token!");

            await revokeRefreshToken(refreshToken, command.IpAddress, cancellationToken, "Revoked without replacement");
        }

        public virtual async Task<PartnerLoginVm> PartnerLogin(PartnerLoginCommand command, CancellationToken cancellationToken)
        {
            var partnerUser = await _context.SbPartnerUsers.Where(u => u.Email.ToLower() == command.Email.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (partnerUser == null) throw new NotFoundException("Incorrect Email Address");
            if (partnerUser.Password.IsNullOrEmpty()) throw new NotActiveUserException();

            if (!BCrypt.Net.BCrypt.Verify(command.Password, partnerUser.Password)) throw new InvalidPasswordException();

            if (partnerUser.IsPasswordSet == null || partnerUser.IsPasswordSet == true)
            {
                return new PartnerLoginVm()
                {
                    IsLoggedIn = false,
                    isPasswordSet = true,
                };
            }

            if (!partnerUser.IsActive)
            {
                return new PartnerLoginVm()
                {
                    IsLoggedIn = false,
                    IsActive = false,
                };
            }

            var partner = _context.SbPartners.FirstOrDefault(p => p.Id.Equals(partnerUser.IdPartner));
            if (partner == null) throw new NotFoundException("Couldn't find partner data");

            var customer = _context.SbCustomers.FirstOrDefault(p => p.Id.Equals(partner.IdCustomer));
            if (customer == null) throw new NotFoundException("Couldn't find customer data");

            partnerUser.LastSignInDate = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new PartnerLoginVm()
            {
                IsLoggedIn = true,
                IsActive = partnerUser.IsActive,
                CompanyName = partner.CompanyName,
                Email = partnerUser.Email,
                isPasswordSet = partnerUser.IsPasswordSet,
                PartnerId = partnerUser.IdPartner,
                CustomerId = customer.Id,
                CustomerCompanyName = customer.CompanyName,
            };

        }

        private async Task<User> getUserByRefreshToken(string token)
        {
            var tokenUser = await _context.SbUserRefreshTokens.FirstOrDefaultAsync(t => t.Token.Equals(token));
            if (tokenUser == null) throw new InvalidTokenException("Invalid Refresh Token!");
            var user = await _context.SbUsers.FirstOrDefaultAsync(u => u.Id == tokenUser.IdUser);
            if (user == null) throw new InvalidTokenException("Invalid Refresh Token!");

            return user;
        }

        private void revokeDescendantRefreshTokens(UserRefreshToken refreshToken, User user, string ipAddress, string reason, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = _context.SbUserRefreshTokens.FirstOrDefault(t => t.Token.Equals(refreshToken.ReplacedByToken));
                if (childToken.IsActive)
                    revokeRefreshToken(childToken, ipAddress, cancellationToken, reason);
                else
                    revokeDescendantRefreshTokens(childToken, user, ipAddress, reason, cancellationToken: cancellationToken);
            }
        }

        private async Task revokeRefreshToken(UserRefreshToken token, string ipAddress, CancellationToken cancellationToken, string? reason = null, string? replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
            token.IsRevoked = true;
            token.IsActive = false;

            await _context.SaveChangesAsync(cancellationToken);

        }

        private async Task<RefreshToken> rotateRefreshTokenAsync(UserRefreshToken refreshToken, string ipAddress, CancellationToken cancellationToken)
        {
            var newRefreshToken = await _jwtTokenUtils.GenerateRefreshToken(ipAddress);
            await revokeRefreshToken(refreshToken, ipAddress, cancellationToken, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        public virtual async Task DeleteOldRefreshTokens(DeleteOldRefreshTokensCommand command, CancellationToken cancellationToken)
        {
            var deleteTimeRule = _dateTimeProvider.UtcNow.AddDays(-3);
            var tokensToDelete = await _context.SbUserRefreshTokens.Where(t => t.Expires <= deleteTimeRule).ToListAsync();
            if (tokensToDelete == null || tokensToDelete.Count == 0) return;
            foreach (var tokenToDelete in tokensToDelete)
            {
                _context.SbUserRefreshTokens.Remove(tokenToDelete);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
