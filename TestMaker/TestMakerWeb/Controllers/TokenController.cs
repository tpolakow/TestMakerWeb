using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestMakerWeb.Data;
using TestMakerWeb.ViewModels;

namespace TestMakerWeb.Controllers
{
  public class TokenController : BaseApiController
  {
    public TokenController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
      IConfiguration configuration)
      : base(context, roleManager, userManager, configuration) { }

    [HttpPost("Auth")]
    public async Task<IActionResult> Auth([FromBody] TokenRequestViewModel model)
    {
      if (model == null) return new StatusCodeResult(500);

      switch (model.grant_type)
      {
        case "password":
          return await GetToken(model);
        default:
          //Nieobslugiwane, zwroc kod statusu 401
          return new UnauthorizedResult();
      }
    }

    private async Task<IActionResult> GetToken(TokenRequestViewModel model)
    {
      try
      {
        //Sprawdz czy istnieje uzytkownik o podanej nazwie
        var user = await UserManager.FindByNameAsync(model.username);
        //dopusc uzycie email zamiast nazwy
        if (user == null && model.username.Contains("@"))
          user = await UserManager.FindByEmailAsync(model.username);
        //nie OK
        if(user==null || !await UserManager.CheckPasswordAsync(user, model.password))
        {
          return new UnauthorizedResult();
        }
        //OK
        DateTime now = DateTime.UtcNow;

        //Dodaj odpowiednie roszczenia do JWT (RFC7519)
        var claims = new[]
        {
          new Claim(JwtRegisteredClaimNames.Sub, user.Id),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
        };

        var tokenExpirationMins = Configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");
        var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:Key"]));

        var token = new JwtSecurityToken(
          issuer: Configuration["Auth:Jwt:Issuer"],
          audience: Configuration["Auth:Jwt:Audience"],
          claims: claims,
          notBefore: now,
          expires: now.Add(TimeSpan.FromMinutes(tokenExpirationMins)),
          signingCredentials: new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256)
        );
        var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

        //Zbuduj i zwroc odpowiedz
        var response = new TokenResponseViewModel()
        {
          token = encodedToken,
          expiration = tokenExpirationMins
        };
        return Json(response);
      }
      catch (Exception ex)
      {
        return new UnauthorizedResult();
      }
    }
  }
}
