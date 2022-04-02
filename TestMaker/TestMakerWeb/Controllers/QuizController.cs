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

namespace TestMakerWeb.Controllers
{
  public class QuizController : BaseApiController
  {
    #region Konstruktor
    public QuizController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
      IConfiguration configuration) : base(context, roleManager, userManager, configuration) { }
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

      //Obsłuż żądania proszące o nieistniejące quizy
      if (quiz == null)
      {
        return NotFound(new
        {
          Error = String.Format("Nie znaleziono quizu o identyfikatorze {0}", id)
        });
      }

      //Przekaż wyniki w formacie JSON
      return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSettings);
    }

    ///<summary>
    ///Dodaje nowy quiz do bazy danych
    ///</summary>
    ///<param name="model">obiekt QuizViewModel z danymi do wstawienia</param>
    [HttpPost]
    public IActionResult Post([FromBody]QuizViewModel model)
    {
      //Zwraca ogólny kod statusu HTTP 500 (Server Error),
      //jeśli dane przesłane przez klienta są niewłaściwe
      if (model == null) return new StatusCodeResult(500);

      //Obsługa wstawienia (bez odwzorowania obiektów)
      var quiz = new Quiz();

      //Właściwości pobierane z żądania
      quiz.Title = model.Title;
      quiz.Description = model.Description;
      quiz.Text = model.Text;
      quiz.Notes = model.Notes;

      //Właściwości ustawiane tylko przez serwer
      quiz.CreatedDate = DateTime.Now;
      quiz.LastModifiedDate = quiz.CreatedDate;

      //Tymczasowo ustaw autora na użytkownika administracyjnego
      quiz.UserId = DbContext.Users.Where(u => u.UserName == "Admin").FirstOrDefault().Id;

      //Dodaj nowy quiz
      DbContext.Quizzes.Add(quiz);
      DbContext.SaveChanges();

      //Zwróć nowo utworzony quiz do klienta
      return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSettings);
    }

    ///<summary>
    ///Modyfikuje quiz o podanym {id}
    ///</summary>
    ///<param name="model">obiekt QuizViewModel z danymi do uaktualnienia</param>
    [HttpPut]
    public IActionResult Put([FromBody]QuizViewModel model)
    {
      //Zwraca ogólny kod statusu HTTP 500 (Server Error),
      //jeśli dane przesłane przez klienta są niewłaściwe
      if (model == null) return new StatusCodeResult(500);

      //Obsługa wstawienia (bez odwzorowania obiektów)
      var quiz = DbContext.Quizzes.Where(q => q.Id == model.Id).FirstOrDefault();

      //Obsłuż żądania proszące o nieistniejące quizy
      if (quiz == null)
      {
        return NotFound(new
        {
          Error = String.Format("Nie znaleziono quizu o identyfikatorze {0}", model.Id)
        });
      }

      //Właściwości pobierane z żądania
      quiz.Title = model.Title;
      quiz.Description = model.Description;
      quiz.Text = model.Text;
      quiz.Notes = model.Notes;

      //Właściwości ustawiane tylko przez serwer
      quiz.LastModifiedDate = DateTime.Now;

      //Zapisz zmiany w bazie
      DbContext.SaveChanges();

      //Zwróć zaktualizowany quiz do klienta
      return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSettings);
    }

    ///<summary>
    ///Usuwa quiz o podanym {id} z bazy danych
    ///</summary>
    ///<param name="id">id istniejacego quizu</param>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var quiz = DbContext.Quizzes.Where(q => q.Id == id).FirstOrDefault();

      if (quiz == null)
      {
        return NotFound(new
        {
          Error = String.Format("Nie znaleziono quizu o identyfikatorze {0}", id)
        });
      }

      DbContext.Quizzes.Remove(quiz);
      DbContext.SaveChanges();

      return new NoContentResult();
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

      return new JsonResult(latest.Adapt<List<QuizViewModel>>(), JsonSettings);

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

      return new JsonResult(byTitle.Adapt<List<QuizViewModel>>(), JsonSettings);
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

      return new JsonResult(random.Adapt<List<QuizViewModel>>(), JsonSettings);
    }
    #endregion
  }
}
