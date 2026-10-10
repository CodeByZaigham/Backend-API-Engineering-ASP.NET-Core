using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopforge_backend.DTO;
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
        public async Task<IActionResult> Login(userdto dto)
        {
            try
            {
                var Result=await _authservice.LoginUser(dto);
                if (Result.Item1 == 1){return NotFound(Result.Item2);}
                if (Result.Item1 == 0){return BadRequest(Result.Item2);}
                return Ok(Result.Item2);

            }
            catch (System.Exception)
            {
                
                throw;
            } 
        }
    }
}
