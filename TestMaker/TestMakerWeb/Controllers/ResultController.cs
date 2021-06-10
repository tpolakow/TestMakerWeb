using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerWeb.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace TestMakerWeb.Controllers
{
  [Route("api/[controller]")]
  public class ResultController : Controller
  {
    #region Metody dostosowujące do konwencji REST
    ///<summary>
    ///Pobiera wynik o podanym {id}
    ///</summary>
    ///<param name="id">identyfikator istniejącego wyniku</param>
    ///<returns>wynik o podanym {id}</returns>
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      return Content("Jeszcze niezaimplementowane");
    }

    ///<summary>
    ///Dodaje nowy wynik do bazy danych
    ///</summary>
    ///<param name="model">obiekt ResultViewModel z danymi do wstawienia</param>
    [HttpPut]
    public IActionResult Put(ResultViewModel model)
    {
      throw new NotImplementedException();
    }

    ///<summary>
    ///Modyfikuje wynik o podanym {id}
    ///</summary>
    ///<param name="model">obiekt ResultViewModel z danymi do uaktualnienia</param>
    [HttpPost]
    public IActionResult Post(ResultViewModel model)
    {
      throw new NotImplementedException();
    }

    ///<summary>
    ///Usuwa wynik o podanym {id} z bazy danych
    ///</summary>
    ///<param name="id">id istniejacego wyniku</param>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      throw new NotImplementedException();
    }
    #endregion

    #region Metody routingu bazujące na atrybutach
    //GET api/question/all
    [HttpGet("All/{quizId}")]
    public IActionResult All(int quizId)
    {
      var sampleResults = new List<ResultViewModel>();

      //Dodaj pierwszy przykładowy wynik
      sampleResults.Add(new ResultViewModel()
      {
        Id = 1,
        QuizId = quizId,
        Text = "Co cenisz w swoim życiu najbradziej?",
        CreatedDate = DateTime.Now,
        LastModifiedDate = DateTime.Now
      });

      //Dodaj kilka innych przykładowych wyników
      for (int i = 2; i <= 5; i++)
      {
        sampleResults.Add(new ResultViewModel()
        {
          Id = i,
          QuizId = quizId,
          Text = String.Format("Przykładowe pytanie {0}", i),
          CreatedDate = DateTime.Now,
          LastModifiedDate = DateTime.Now
        });
      }

      //Przekaż wyniki w formacie JSON
      return new JsonResult(
        sampleResults,
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }
    #endregion
  }
}
