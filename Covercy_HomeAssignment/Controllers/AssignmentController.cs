﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;


namespace Covercy_HomeAssignment.Controllers
{




    [ApiController]
    public class AssignmentController : Controller
    {

        private IConfiguration Configuration;

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public AssignmentController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ApiTokenDto>>> GetToken()
        {

            int? userIdFromRequest = authenticateRequest(Request.Query);
            if (userIdFromRequest != null)//replace of authenticateRequest(request)
            {
                int userId = (int)userIdFromRequest;
                List<ApiToken> Result;
                using (ApplicationContext db = new ApplicationContext())
                {
                    Result = db.ApiTokens.Where(entity => entity.UserId == userId).ToList();
                }

                return Ok(ApiTokenDto.GetList(Result));
            }
            return BadRequest();
        }

        [HttpPost("")]
        public async Task<ActionResult<ApiTokenDto>> PostToken()
        {
            int? userIdFromRequest = authenticateRequest(Request.Query);
            if (userIdFromRequest != null)//replace of authenticateRequest(request)
            {
                int userId = (int)userIdFromRequest;
                if (Request.Query.ContainsKey("permissions"))
                {
                    string permissions = Request.Query.Where(p => p.Key == "permissions").FirstOrDefault().Value;
                    ApiToken ApiKey = new ApiToken((int)userId, permissions);

                    using (ApplicationContext db = new ApplicationContext())
                    {
                        db.ApiTokens.Add(ApiKey);
                        db.SaveChanges();
                    }

                    return Ok(new ApiTokenDto(ApiKey));
                }
            }
            return BadRequest();
        }

        [HttpDelete("")]
        public async Task<ActionResult<ApiTokenDto>> DeleteToken() //int? userId, string? ApiKey
        {
            int? userIdFromRequest = authenticateRequest(Request.Query);
            if (userIdFromRequest != null)//replace of authenticateRequest(request)
            {
                int userId = (int)userIdFromRequest;

                if (Request.Query.ContainsKey("ApiKey"))
                {
                    string ApiKey = Request.Query.Where(p => p.Key == "ApiKey").FirstOrDefault().Value;
                    ApiToken Key;
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        Key = db.ApiTokens.Where(token => token.ApiKey == ApiKey).FirstOrDefault();
                        if (Key != null)
                        {
                            Key.IsValid = false;
                            db.SaveChanges();
                            return Ok(new ApiTokenDto(Key));
                        }
                    }
                    
                }
            }
            return BadRequest();
        }

        [HttpPost("/authenticate")]
        public async Task<ActionResult<string>> AuthenticateToken()
        {
            if (Request.Query.ContainsKey("ApiKey"))
            {
                string ApiKey = Request.Query.Where(p => p.Key == "ApiKey").FirstOrDefault().Value;
                if (ApiKey != null)
                {
                    ApiToken Key;
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        Key = db.ApiTokens.Where(token => token.ApiKey == ApiKey).FirstOrDefault();
                        if (Key!=null)
                        {
                            if (Key.IsValid)
                            {
                                Key.LastUsage = DateTime.Now;
                                db.SaveChanges();


                                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                                                            Configuration.GetSection("SecretKey").Value));          //generating key for signing
                                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature); //generating sifning credenttials
                                JwtSecurityToken JWTtoken = new JwtSecurityToken(                //generating JWT token
                                            claims: new JWTtokenDto(Key).getClaims(),
                                            signingCredentials: cred
                                            );

                                JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();
                                return Ok(Handler.WriteToken(JWTtoken));
                            }
                        }
                    }
                }
            }

            return BadRequest("Api Key is Revoked or do not exist");
        }



        private int? authenticateRequest(IQueryCollection request)
        {
            if (request.ContainsKey("userId"))
            {
                int ResultID;
                 if(int.TryParse(request.FirstOrDefault(p => p.Key == "userId").Value,out ResultID))
                {
                    return ResultID;
                }
            }
            
            return null;
        }




    }
}
