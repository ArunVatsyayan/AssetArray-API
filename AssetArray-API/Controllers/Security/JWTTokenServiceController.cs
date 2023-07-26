using AssetArray.Core.Logic.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetArray_API.Controllers.Security
{
    [Route("api/Security/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class JWTTokenServiceController : Controller
    {
        private readonly IJWTTokenService _jwtTokenService;

        public JWTTokenServiceController(IJWTTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public IActionResult GenerateToken(string authKey) 
        { 
            string jwtToken = _jwtTokenService.GenerateToken(authKey);

            return Ok(new { token = jwtToken});
        }
    }
}
