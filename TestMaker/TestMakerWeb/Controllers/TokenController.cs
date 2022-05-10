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
        case "refresh_token":
          return await RefreshToken(model);
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
        var rt = CreateRefreshToken(model.client_id, user.Id);

        //Dodaj nowy token odswiezania do bazy
        DbContext.Tokens.Add(rt);
        DbContext.SaveChanges();

        //Utworz i zwroc token dostepowy
        var t = CreateAccessToken(user.Id, rt.Value);
        return Json(t);
      }
      catch (Exception ex)
      {
        return new UnauthorizedResult();
      }
    }

    private async Task<IActionResult> RefreshToken(TokenRequestViewModel model)
    {
      try
      {
        //Sprawdź czy otrzymany token odswiezania istnieje dla danego ClientId
        var rt = DbContext.Tokens.
          FirstOrDefault(t => t.ClientId == model.client_id && t.Value == model.refresh_token);

        if (rt == null)
        {
          //Token nie istnieje lub jest niepoprawny (albo przekazano złe ClientId)
          return new UnauthorizedResult();
        }

        //Sprawdź czy istnieje uzytkownik o UserId z tokena odswiezania
        var user = await UserManager.FindByIdAsync(rt.UserId);

        if (user == null)
        {
          return new UnauthorizedResult();
        }

        //Wygeneruj nowy token odswiezania
        var rtNew = CreateRefreshToken(rt.ClientId, rt.UserId);

        //Unieważnij stary token odswiezania(usun go)
        DbContext.Tokens.Remove(rt);
        //dodaj nowy token odswiezania
        DbContext.Tokens.Add(rtNew);

        DbContext.SaveChanges();

        //Utwórz nowy token dostępowy
        var response = CreateAccessToken(rtNew.UserId, rtNew.Value);
        //..i wyslij go do klienta
        return Json(response);
      }
      catch(Exception ex)
      {
        return new UnauthorizedResult();
      }
    }

    private Token CreateRefreshToken(string clientId, string userId)
    {
      return new Token()
      {
        ClientId = clientId,
        UserId = userId,
        Type = 0,
        Value = Guid.NewGuid().ToString("N"),
        CreatedDate = DateTime.UtcNow
      };
    }

    private TokenResponseViewModel CreateAccessToken(string userId, string refreshToken)
    {
      DateTime now = DateTime.UtcNow;

      //DOdaj odpowiednie roszczenia do JWT
      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
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

      return new TokenResponseViewModel()
      {
        token = encodedToken,
        expiration = tokenExpirationMins,
        refresh_token = refreshToken
      };
    }
  }
}
