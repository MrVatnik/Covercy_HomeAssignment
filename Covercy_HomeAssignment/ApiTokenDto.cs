using System.ComponentModel.DataAnnotations;

namespace Covercy_HomeAssignment
{
    public class ApiTokenDto
    {
        public int UserId { get; set; }
        public string ApiKey { get; set; }
        public string Status { get; set; }
        public DateTime LastUsage { get; set; }

        public ApiTokenDto(ApiToken apiToken)
        {
            this.UserId = apiToken.UserId;
            this.ApiKey = apiToken.ApiKey;
            this.LastUsage = apiToken.LastUsage;
            if(apiToken.IsValid)
            {
                Status = "Valid";
            }
            else
            {
                {
                    Status = "Revoked";
                }
            }
        }

        public static List<ApiTokenDto> GetList (List<ApiToken> TokensList)
        {
            List <ApiTokenDto> Result = new List <ApiTokenDto> ();
            foreach (ApiToken apiToken in TokensList)
            {
                Result.Add(new ApiTokenDto (apiToken));
            }
            return Result;
        }
    }
}
