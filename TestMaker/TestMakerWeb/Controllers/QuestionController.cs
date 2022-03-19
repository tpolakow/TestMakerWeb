using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerWeb.ViewModels;
using System.Collections.Generic;
using System.Linq;
using TestMakerWeb.Data;
using Mapster;

namespace TestMakerWeb.Controllers
{
  public class QuestionController : BaseApiController
  {
    #region Konstruktor
    public QuestionController(ApplicationDbContext context) : base(context) { }
    #endregion

    #region Metody dostosowujące do konwencji REST
    ///<summary>
    ///Pobiera pytanie o podanym {id}
    ///</summary>
    ///<param name="id">identyfikator istniejącego pytania</param>
    ///<returns>pytanie o podanym {id}</returns>
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var question = DbContext.Questions.Where(q => q.Id == id).FirstOrDefault();
      if (question == null)
      {
        return NotFound(new
        {
          Error = String.Format("Nie znaleziono pytania o identyfikatorze {0}", id)
        });
      }
      return new JsonResult(question.Adapt<QuestionViewModel>(), JsonSettings);
    }

    ///<summary>
    ///Dodaje nowe pytanie do bazy danych
    ///</summary>
    ///<param name="model">obiekt QuestionViewModel z danymi do wstawienia</param>
    [HttpPost]
    public IActionResult Post([FromBody]QuestionViewModel model)
    {
      if (model == null) return new StatusCodeResult(500);

      var question = model.Adapt<Question>();

      question.CreatedDate = DateTime.Now;
      question.LastModifiedDate = question.CreatedDate;

      DbContext.Questions.Add(question);
      DbContext.SaveChanges();

      return new JsonResult(question.Adapt<QuestionViewModel>(), JsonSettings);
    }

    ///<summary>
    ///Modyfikuje pytanie o podanym {id}
    ///</summary>
    ///<param name="model">obiekt QuestionViewModel z danymi do uaktualnienia</param>
    [HttpPut]
    public IActionResult Put([FromBody]QuestionViewModel model)
    {
      if (model == null) return new StatusCodeResult(500);

      var question = DbContext.Questions.Where(q => q.Id == model.Id).FirstOrDefault();

      if (question == null) return NotFound(new
      {
        Error = String.Format("Nie znaleziono pytania o identyfikatorze {0}", model.Id)
      });

      question.QuizId = model.QuizId;
      question.Text = model.Text;
      question.Notes = model.Notes;
      question.LastModifiedDate = DateTime.Now;

      DbContext.SaveChanges();

      return new JsonResult(question.Adapt<QuestionViewModel>(), JsonSettings);
    }

    ///<summary>
    ///Usuwa pytanie o podanym {id} z bazy danych
    ///</summary>
    ///<param name="id">id istniejacego pytania</param>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var question = DbContext.Questions.Where(q => q.Id == id).FirstOrDefault();

      if (question == null) return NotFound(new
      {
        Error = String.Format("Nie znaleziono pytania o identyfikatorze {0}", id)
      });

      DbContext.Questions.Remove(question);
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
      //var sampleQuestions = new List<QuestionViewModel>();

      ////Dodaj pierwsze przykładowe pytanie
      //sampleQuestions.Add(new QuestionViewModel()
      //{
      //  Id = 1,
      //  QuizId = quizId,
      //  Text = "Co cenisz w swoim życiu najbradziej?",
      //  CreatedDate = DateTime.Now,
      //  LastModifiedDate = DateTime.Now
      //});

      ////Dodaj kilka innych przykładowych pytań
      //for(int i=2; i <= 5; i++)
      //{
      //  sampleQuestions.Add(new QuestionViewModel()
      //  {
      //    Id = i,
      //    QuizId = quizId,
      //    Text = String.Format("Przykładowe pytanie {0}", i),
      //    CreatedDate = DateTime.Now,
      //    LastModifiedDate = DateTime.Now
      //  });
      //}
      #endregion
      var questions = DbContext.Questions.Where(q => q.QuizId == quizId).ToList();

      //Przekaż wyniki w formacie JSON
      return new JsonResult(questions.Adapt<List<QuestionViewModel>>(), JsonSettings);
    }
    #endregion
  }
}
