using Autodesk.Forge;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using forgeSample.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace forgeSample.Controllers
{
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly IOAuthService _authService;

        public OAuthController(IOAuthService authService)
        {
            _authService = authService;
        }
        
        /// <summary>
        /// Get access token with public (viewables:read) scope
        /// </summary>
        [HttpGet]
        [Route("api/forge/oauth/token")]
        public async Task<dynamic> GetPublicAsync()
        {
            return await _authService.GetPublicAsync();
        }
    }
}