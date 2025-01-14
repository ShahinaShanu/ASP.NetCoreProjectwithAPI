using CoreAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    //[AllowAnonymous]
    [ApiController]
    public class EmployeeAPIController : ControllerBase
    {
        private IConfiguration _configuration;

        public EmployeeAPIController(IConfiguration config) {
            _configuration=config;
        }
        [HttpPost("GenerateToken")]
        public IActionResult Login([FromBody] Login model)
        {
            if (model.Username == "admin" && model.Password == "abc") // Validate credentials
            {
                var token = GenerateJwtToken(model.Username);
                return Ok(new { token });
            }

            return Unauthorized();
        }
        private string GenerateJwtToken(string username)
        {
            var claims = new[] {
                new Claim("username", username)
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        // GET: api/<EmployeeAPIController>
        [Authorize]
        [HttpGet("getEmployee")]
        public IActionResult Get()
        {
            var employeedto = new EmployeeDto
            { 
             Id=1,
             Name="shahina",
             Email="sheikhshahina@gmail.com",
            };
            return Ok(employeedto);
        }

        // GET api/<EmployeeAPIController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EmployeeAPIController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EmployeeAPIController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeeAPIController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
