namespace RaportAPI.Core.Domain.Entities
{
    public class User
    {

        public long Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public long IdRole { get; set; }
        public bool IsActive { get; set; }
        public bool IsPasswordSet { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastSignInDate { get; set; }
    }
}
