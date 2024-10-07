using System.ComponentModel.DataAnnotations;

namespace Covercy_HomeAssignment
{
    public class ApiToken
    {
        public int UserId { get; set; }
        [Key]
        public string ApiKey { get; set; }
        public string Permissions { get; set; }
        public bool IsValid { get; set; }
        public DateTime LastUsage { get; set; }
        public ApiToken(int userId, string permissions)
        {
            ApiKey = Guid.NewGuid().ToString();
            this.UserId = userId;
            Permissions = permissions;
            IsValid = true;
            LastUsage = DateTime.Now;
        }
    }
}
