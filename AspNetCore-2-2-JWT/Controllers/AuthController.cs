using AspNetCore_2_2_JWT.Extensions;
using AspNetCore_2_2_JWT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore_2_2_JWT.Controllers
{
    [Route("api")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }
        [HttpPost("nova-conta")]
        public async Task<ActionResult> Register(RegisterUserViewModel register)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser()
            {
                UserName = register.Email,
                Email = register.Email,
                EmailConfirmed = true
            };



            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, Roles.ROLE_API_ADM).Wait();
                _userManager.AddClaimAsync(user, new Claim("Curso", "Get;Add;Upd;Del")).Wait();

                //new Claim(ClaimTypes.Role, "Teste")
                await _signInManager.SignInAsync(user, false);
                //return Ok(register);

                //return Ok(GenerateJWTOnly());
                return Ok(await GenerateJWT(register.Email));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }
        [HttpPost("entrar")]
        public async Task<ActionResult> Login(LoginUserViewModel login)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, true);
            if (result.Succeeded)
            {
                //return Ok(GenerateJWTOnly());
                return Ok(await GenerateJWT(login.Email));
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("E0002", "Usario temporariamente bloqueado por tentativas inválidas");
                return BadRequest(ModelState);
            }

            ModelState.AddModelError("E0001", "Usuário ou senha incorretos.");

            return BadRequest(ModelState);
        }

        private string GenerateJWTOnly()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emitter,
                Audience = _appSettings.ValidIn,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        private async Task<TokenResponseViewModel> GenerateJWT(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            //Adicionamos a coleção de Claims do Usuario, que nos informa o que ele tem permissão depois que ele já está autorizado a entrar no sistema.
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(),ClaimValueTypes.Integer64));

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Issuer = _appSettings.Emitter,
                Audience = _appSettings.ValidIn,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var encodedToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            var response = new TokenResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Claims = claims.Select(c=> new ClaimViewModel { Type = c.Type, Value = c.Value}),
                    Email = user.Email,
                    Id = user.Id,
                    UniqueName = user.Email
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
            =>(long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970,1,1,0,0,0,TimeSpan.Zero)).TotalSeconds);

    }
}
