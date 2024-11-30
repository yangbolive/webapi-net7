using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Tls;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace webapi.net7.sqlsugar.Controllers
{
    /// <summary>
    /// Token管理
    /// </summary>
    [ApiController]
    [Route("api/[action]")]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.U8移动端接口))]
    public class ApiTokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ApiTokenController(IConfiguration configuration)
        {
            _config = configuration;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public MessageToken GetToken(User user)
        {

            try
            {
                //这里我写死了，后边可以写一个实体，输入用户名和密码放进这里
                if (user.UserName == "admin" && user.Password == "admin")
                {
                    var claims = new[]
                   {
                new Claim(ClaimTypes.Name,$"{user.UserName}"),
                new Claim(ClaimTypes.Upn,$"{user.Password}")
               };


                    //发行者

                    var isyouruser = _config.GetValue<string>("JWT:ISyouuser");
                    //接受者

                    var isAudience = _config.GetValue<string>("JWT:IsAudience");
                    //秘钥key

                    var scKey = _config.GetValue<string>("JWT:SignKey");
                    //设置token的过期时间

                    DateTime timeout = DateTime.Now.AddMinutes(30);
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(scKey));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                    var jwtToken = new JwtSecurityToken(isyouruser, isAudience, claims, expires: timeout, signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                    return new MessageToken { errCode = "", token = "Bearer " + token, errMsg = "成功" };
                }
                else { return new MessageToken { errCode = "1", errMsg = "账号密码错误,获取TOKEN失败。" }; }
            }

            catch (Exception ex)
            {
                return new MessageToken { errCode = "1", errMsg = ex.ToString() };
            }

           
        }




    }
}
