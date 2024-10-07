using System.Security.Claims;

namespace Covercy_HomeAssignment
{
    public class JWTtokenDto
    {
        public string permissions { get; set; }
        public int UserId { get; set; }

        public JWTtokenDto(ApiToken apiToken)
        {
            permissions=apiToken.Permissions;
            UserId = apiToken.UserId;
        }
        public List<Claim> getClaims()
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("permissions", permissions));
            claims.Add(new Claim("UserId", UserId.ToString()));
            return claims;
        }
    }
}
