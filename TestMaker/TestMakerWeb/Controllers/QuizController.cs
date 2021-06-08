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
    //GET api/quiz/latest
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
        sampleQuizzes);

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
        sampleQuizzes.OrderBy(t => t.Title));
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
        sampleQuizzes.OrderBy(t => Guid.NewGuid()));
    }
  }
}
