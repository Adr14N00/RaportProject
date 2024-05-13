namespace SelfOnBoardingManagement.Domain.Entities
{
    public partial class UserRefreshToken
    {
        public long Id { get; set; }
        public long IdUser { get; set; }
        public string Token { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; } = null!;
        public bool IsRevoked { get; set; }
        public DateTime? Revoked { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReasonRevoked { get; set; }
        public string? ReplacedByToken { get; set; }

        //entity FK
        public virtual User IdUserNavigation { get; set; } = null!;

    }
}
