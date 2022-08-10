

using APICatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AutorizeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _singInManager;
        private readonly IConfiguration _configuration;

        public AutorizeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration Config)
        {
            _userManager = userManager;
            _singInManager = signInManager;
            _configuration = Config;
        }

        [HttpGet]

        public ActionResult<string> Get()
        {
            return "AutorizeController :: Acessado em : "
                + DateTime.Now.ToLongTimeString();
        }

        [HttpPost("register")]

        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("chego");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.SetLockoutEnabledAsync(user, false);
            await _singInManager.SignInAsync(user, false);
            return Ok(GeraToken(model));
        }

        [HttpPost("login")]

        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(c => c.Errors));
            }

            var result = await _singInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, false, false);

            if (result.Succeeded)
            {
                return Ok(GeraToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido.....");
                return BadRequest(ModelState);
            }
        }
        private UsuarioToken GeraToken(UsuarioDTO userInfo)
        {
            //define declarações do usuário
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                 new Claim("meuPet", "pipoca"),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            //gera uma chave com base em um algoritmo simetrico
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            //gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tempo de expiracão do token.
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            // classe que representa um token JWT e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expiration,
              signingCredentials: credenciais);

            //retorna os dados com o token e informacoes
            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }
    }
}
