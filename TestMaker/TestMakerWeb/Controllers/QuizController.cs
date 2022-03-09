using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;
using TestMakerWeb.ViewModels;
using System.Collections.Generic;
using System.Linq;
using TestMakerWeb.Data;
using Mapster;

namespace TestMakerWeb.Controllers
{
  [Route("api/[controller]")]
  public class QuizController : Controller
  {
    #region Pola prywatne
    private ApplicationDbContext DbContext;
    #endregion

    #region Konstruktor
    public QuizController(ApplicationDbContext context)
    {
      //Utworzenie ApplicationDbContext poprzez wstrzykiwanie zalezności
      DbContext = context;
    }
    #endregion

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
      #region temp
      ////Tworzy przykładowy quiz pasujący do żądania
      //var v = new QuizViewModel()
      //{
      //  Id = id,
      //  Title = String.Format("Przykładowy quiz o id {0}", id),
      //  Description = "To nie jest prawdziwy quiz - to tylko przykład!",
      //  CreatedDate = DateTime.Now,
      //  LastModifiedDate = DateTime.Now
      //};
      #endregion
      var quiz = DbContext.Quizzes.Where(i => i.Id == id).FirstOrDefault();

      //Przekaż wyniki w formacie JSON
      return new JsonResult(
        //v,
        quiz.Adapt<QuizViewModel>(),
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
      #region temp
      ////Dodaj pierwszy przykładowy quiz
      //sampleQuizzes.Add(new QuizViewModel()
      //{
      //  Id = 1,
      //  Title = "Którą postacią z Shingeki No Kyojin (Atak tytanów) jesteś?",
      //  Description = "Test osobowości bazujący na anime",
      //  CreatedDate = DateTime.Now,
      //  LastModifiedDate = DateTime.Now
      //});

      ////Dodaj kilka nastepnych przykladowych quizow
      //for (int i = 2; i <= num; i++)
      //{
      //  sampleQuizzes.Add(new QuizViewModel()
      //  {
      //    Id = i,
      //    Title = String.Format("Przykładowy quiz {0}", i),
      //    Description = "To jest przykładowy quiz",
      //    CreatedDate = DateTime.Now,
      //    LastModifiedDate = DateTime.Now
      //  });
      //}
      #endregion
      //Przekaż wyniki w formacie JSON
      var latest = DbContext.Quizzes.OrderByDescending(q => q.CreatedDate).Take(num).ToList();

      return new JsonResult(
        //sampleQuizzes,
        latest.Adapt<List<QuizViewModel>>(),
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
      #region temp
      //var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;
      #endregion
      var byTitle = DbContext.Quizzes.OrderBy(q => q.Title).Take(num).ToList();

      return new JsonResult(
        //sampleQuizzes.OrderBy(t => t.Title),
        byTitle.Adapt<List<QuizViewModel>>(),
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
      #region temp
      //var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;
      #endregion
      var random = DbContext.Quizzes.OrderBy(q => Guid.NewGuid()).Take(num).ToList();

      return new JsonResult(
        //sampleQuizzes.OrderBy(t => Guid.NewGuid()),
        random.Adapt<List<QuizViewModel>>(),
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }
    #endregion
  }
}
