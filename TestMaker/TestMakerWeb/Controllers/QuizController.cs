using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;
using TestMakerWeb.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace TestMakerWeb.Controllers
{
  [Route("api/[controller]")]
  public class QuizController : Controller
  {
    #region Metody dostosowujące do konwencji REST
    ///<summary>
    ///GET: api/quiz/{id}
    ///Pobiera quiz o podanym {id}
    ///</summary>
    ///<param name="id">identyfikator istniejącego quizu</param>
    ///<returns>quiz o podanym {id}</returns>
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      //Tworzy przykładowy quiz pasujący do żądania
      var v = new QuizViewModel()
      {
        Id = id,
        Title = String.Format("Przykładowy quiz o id {0}", id),
        Description = "To nie jest prawdziwy quiz - to tylko przykład!",
        CreatedDate = DateTime.Now,
        LastModifiedDate = DateTime.Now
      };

      //Przekaż wyniki w formacie JSON
      return new JsonResult(
        v,
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }

    ///<summary>
    ///Dodaje nowy quiz do bazy danych
    ///</summary>
    ///<param name="model">obiekt QuizViewModel z danymi do wstawienia</param>
    [HttpPut]
    public IActionResult Put(QuizViewModel model)
    {
      throw new NotImplementedException();
    }

    ///<summary>
    ///Modyfikuje quiz o podanym {id}
    ///</summary>
    ///<param name="model">obiekt QuizViewModel z danymi do uaktualnienia</param>
    [HttpPost]
    public IActionResult Post(QuizViewModel model)
    {
      throw new NotImplementedException();
    }

    ///<summary>
    ///Usuwa quiz o podanym {id} z bazy danych
    ///</summary>
    ///<param name="id">id istniejacego quizu</param>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      throw new NotImplementedException();
    }
    #endregion

    #region Metody routingu bazujące na atrybutach
    ///<summary>
    ///GET: api/quiz/latest
    ///Pobiera {num} najnowszych quizów
    ///</summary>
    ///<param name="num">liczba quizów do pobrania</param>
    ///<returns>{num} najnowszych quizów</returns>
    [HttpGet("Latest/{num?}")]
    public IActionResult Latest(int num = 10)
    {
      var sampleQuizzes = new List<QuizViewModel>();

      //Dodaj pierwszy przykładowy quiz
      sampleQuizzes.Add(new QuizViewModel()
      {
        Id = 1,
        Title = "Którą postacią z Shingeki No Kyojin (Atak tytanów) jesteś?",
        Description = "Test osobowości bazujący na anime",
        CreatedDate = DateTime.Now,
        LastModifiedDate = DateTime.Now
      });

      //Dodaj kilka nastepnych przykladowych quizow
      for (int i = 2; i <= num; i++)
      {
        sampleQuizzes.Add(new QuizViewModel()
        {
          Id = i,
          Title = String.Format("Przykładowy quiz {0}", i),
          Description = "To jest przykładowy quiz",
          CreatedDate = DateTime.Now,
          LastModifiedDate = DateTime.Now
        });
      }
      //Przekaż wyniki w formacie JSON
      return new JsonResult(
        sampleQuizzes,
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });

    }

    ///<summary>
    ///GET: api/quiz/ByTitle
    ///Pobiera {num} quizów posortowanych po tytule (od A do Z)
    ///</summary>
    ///<param name="num">liczba quizów do pobrania</param>
    ///<returns>{num} quizów posortowanych po tytule</returns>
    [HttpGet("ByTitle/{num:int?}")]
    public IActionResult ByTitle(int num = 10)
    {
      var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;

      return new JsonResult(
        sampleQuizzes.OrderBy(t => t.Title),
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }

    ///<summary>
    ///GET: api/quiz/mostViewed
    ///Pobiera {num} losowych quizów
    ///</summary>
    ///<param name="num">liczba quizów do pobrania</param>
    ///<returns>{num} losowych quizów</returns>
    [HttpGet("Random/{num:int?}")]
    public IActionResult Random(int num = 10)
    {
      var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;

      return new JsonResult(
        sampleQuizzes.OrderBy(t => Guid.NewGuid()),
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }
    #endregion
  }
}
