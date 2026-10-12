using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopforge_backend.DTO;
using shopforge_backend.generics;
using shopforge_backend.iservices;

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
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]userdto dto)
        {
            try
            {
                var Result=await _authservice.LoginUser(dto);
                if (Result.Item1 == 0)
                {
                    return NotFound(
                        response_model<string>.fail(null,Result.Item2)
                    );
                }
                if (Result.Item1 == 1)
                {
                    return BadRequest(
                        response_model<string>.fail(null,Result.Item2)
                    );
                }
                return Ok(
                    response_model<string>.success(null,Result.Item2)
                );

            }
            catch (System.Exception)
            {
                throw;
            } 
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromBody]userdto dto)
        {
            try
            {
                var Result=await _authservice.RegisterUser(dto);
                if (Result.Item1 == 0)
                {
                    return BadRequest(
                        response_model<string>.fail(null,Result.Item2)
                    );
                }
                return Ok(
                    response_model<string>.success(null,Result.Item2)
                );
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
