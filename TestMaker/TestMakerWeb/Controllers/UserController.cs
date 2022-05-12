using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;
using TestMakerWeb.ViewModels;
using System.Collections.Generic;
using System.Linq;
using TestMakerWeb.Data;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace TestMakerWeb.Controllers
{
  public class UserController : BaseApiController
  {
    #region Konstruktor
    public UserController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
      IConfiguration configuration) : base(context, roleManager, userManager, configuration) { }
    #endregion

    #region Metody dostosowujące do konwencji REST
    
    ///<summary>
    ///POST: api/user
    ///</summary>
    ///<returns>Tworzy i zwraca nowego użytkownika</returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]UserViewModel model)
    {
      //Zwraca ogólny kod statusu HTTP 500 (Server Error),
      //jeśli dane przesłane przez klienta są niewłaściwe
      if (model == null) return new StatusCodeResult(500);

      //Obsługa wstawienia (bez odwzorowania obiektów)
      ApplicationUser user = await UserManager.FindByNameAsync(model.UserName);
      if (user != null) return BadRequest("Nazwa użytkownika jest już zajęta");

      user = await UserManager.FindByEmailAsync(model.Email);
      if (user != null) return BadRequest("Adres email jest już zajęty");

      var now = DateTime.Now;

      //Utwórz nowego użytkownika na podstawie danych przeslanych przez kklienta
      user = new ApplicationUser()
      {
        SecurityStamp = Guid.NewGuid().ToString(),
        UserName = model.UserName,
        Email = model.Email,
        DisplayName = model.DisplayName,
        CreatedDate = now,
        LastModifiedDate = now
      };

      //Dodaj uzytkownika do bazy z wybranym haslem
      await UserManager.CreateAsync(user, model.Password);

      //Przypisz uzytkownikowi rolę "ZarejestrowanyUżytkownik"
      await UserManager.AddToRoleAsync(user, "ZarejestrowanyUżytkownik");

      //Usuń potwierdzanie emaila i blokadę
      user.EmailConfirmed = true;
      user.LockoutEnabled = false;

      //Zapisz zmiany w bazie
      DbContext.SaveChanges();

      //Przekaż nowo utworzonego uzytkownika klientowi
      return Json(user.Adapt<UserViewModel>(), JsonSettings);
    }
    #endregion
  }
}
