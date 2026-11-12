using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopforge_backend.generics;
using shopforge_backend.Application.DTO;
using shopforge_backend.Application.iservices;

namespace shopforge_backend.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController : ControllerBase
    {
        private readonly iauthservice _authservice; 
        public authController(iauthservice authservice)
        {
            _authservice=authservice;
        }
        // POST api/auth/Login   body: { "email": "...", "password": "..." }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] logindto dto)
        {
            var result = await _authservice.LoginUser(dto);

            if (result.Item1 == 0)
                return BadRequest(response_model<tokendto>.fail(result.Item2, result.Item2.message));

            // unknown email or wrong password -> 401
            if (result.Item1 == 1 || result.Item1 == 4)
                return Unauthorized(response_model<tokendto>.fail(result.Item2, result.Item2.message));

            return Ok(response_model<tokendto>.success(result.Item2, result.Item2.message));
        }

        // POST api/auth/RegisterUser
        // body: { firstName, lastName, email, password, confirmPassword }
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] registerdto dto)
        {
            var result = await authservice.RegisterUser(dto);

            if (result.Item1 == 0)
                return BadRequest(response_model<string>.fail(null, result.Item2));

            return Ok(response_model<string>.success(null, result.Item2));
        }

    }
}
