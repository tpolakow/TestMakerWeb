using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerWeb.ViewModels;
using System.Collections.Generic;
using System.Linq;
using TestMakerWeb.Data;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace TestMakerWeb.Controllers
{
  public class ResultController : BaseApiController
  {
    public ResultController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
      IConfiguration configuration) : base(context, roleManager, userManager, configuration) { }

    #region Metody dostosowujące do konwencji REST
    ///<summary>
    ///Pobiera wynik o podanym {id}
    ///</summary>
    ///<param name="id">identyfikator istniejącego wyniku</param>
    ///<returns>wynik o podanym {id}</returns>
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var result = DbContext.Results.Where(q => q.Id == id).FirstOrDefault();
      if (result == null)
      {
        return NotFound(new
        {
          Error = String.Format("Nie znaleziono wyniku o identyfikatorze {0}", id)
        });
      }
      return new JsonResult(
        result.Adapt<ResultViewModel>(),
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }

    ///<summary>
    ///Dodaje nowy wynik do bazy danych
    ///</summary>
    ///<param name="model">obiekt ResultViewModel z danymi do wstawienia</param>
    [HttpPost]
    public IActionResult Post([FromBody]ResultViewModel model)
    {
      if (model == null) return new StatusCodeResult(500);

      var result = model.Adapt<Result>();

      result.CreatedDate = DateTime.Now;
      result.LastModifiedDate = result.CreatedDate;

      DbContext.Results.Add(result);
      DbContext.SaveChanges();

      return new JsonResult(
        result.Adapt<ResultViewModel>(),
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }

    ///<summary>
    ///Modyfikuje wynik o podanym {id}
    ///</summary>
    ///<param name="model">obiekt ResultViewModel z danymi do uaktualnienia</param>
    [HttpPut]
    public IActionResult Put([FromBody]ResultViewModel model)
    {
      if (model == null) return new StatusCodeResult(500);

      var result = DbContext.Results.Where(q => q.Id == model.Id).FirstOrDefault();

      if (result == null) return NotFound(new
      {
        Error = String.Format("Nie znaleziono wyniku o identyfikatorze {0}", model.Id)
      });

      result.QuizId = model.QuizId;
      result.Text = model.Text;
      result.MinValue = model.MinValue;
      result.MaxValue = model.MaxValue;
      result.Notes = model.Notes;
      result.LastModifiedDate = DateTime.Now;

      DbContext.SaveChanges();

      return new JsonResult(
        result.Adapt<ResultViewModel>(),
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }

    ///<summary>
    ///Usuwa wynik o podanym {id} z bazy danych
    ///</summary>
    ///<param name="id">id istniejacego wyniku</param>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var result = DbContext.Results.Where(q => q.Id == id).FirstOrDefault();

      if (result == null) return NotFound(new
      {
        Error = String.Format("Nie znaleziono wyniku o identyfikatorze {0}", id)
      });

      DbContext.Results.Remove(result);
      DbContext.SaveChanges();

      return new NoContentResult();
    }
    #endregion

    #region Metody routingu bazujące na atrybutach
    //GET api/question/all
    [HttpGet("All/{quizId}")]
    public IActionResult All(int quizId)
    {
      #region temp
      //var sampleResults = new List<ResultViewModel>();

      ////Dodaj pierwszy przykładowy wynik
      //sampleResults.Add(new ResultViewModel()
      //{
      //  Id = 1,
      //  QuizId = quizId,
      //  Text = "Co cenisz w swoim życiu najbradziej?",
      //  CreatedDate = DateTime.Now,
      //  LastModifiedDate = DateTime.Now
      //});

      ////Dodaj kilka innych przykładowych wyników
      //for (int i = 2; i <= 5; i++)
      //{
      //  sampleResults.Add(new ResultViewModel()
      //  {
      //    Id = i,
      //    QuizId = quizId,
      //    Text = String.Format("Przykładowe pytanie {0}", i),
      //    CreatedDate = DateTime.Now,
      //    LastModifiedDate = DateTime.Now
      //  });
      //}
      #endregion
      var results = DbContext.Results.Where(q => q.QuizId == quizId).ToList();

      //Przekaż wyniki w formacie JSON
      return new JsonResult(
        //sampleResults,
        results.Adapt<List<ResultViewModel>>(),
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }
    #endregion
  }
}
