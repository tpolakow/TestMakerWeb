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
  public class AnswerController : BaseApiController
  {
    public AnswerController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
      IConfiguration configuration) : base(context, roleManager, userManager, configuration) { }
    #region Metody dostosowujące do konwencji REST
    ///<summary>
    ///Pobiera odpowiedź o podanym {id}
    ///</summary>
    ///<param name="id">identyfikator istniejącej odpowiedzi</param>
    ///<returns>odpowiedź o podanym {id}</returns>
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var answer = DbContext.Answers.Where(q => q.Id == id).FirstOrDefault();
      if (answer == null)
      {
        return NotFound(new
        {
          Error = String.Format("Nie znaleziono odpowiedzi o identyfikatorze {0}", id)
        });
      }
      return new JsonResult(answer.Adapt<AnswerViewModel>(), JsonSettings);
    }

    ///<summary>
    ///Dodaje nową odpowiedź do bazy danych
    ///</summary>
    ///<param name="model">obiekt AnswerViewModel z danymi do wstawienia</param>
    [HttpPost]
    public IActionResult Post([FromBody]AnswerViewModel model)
    {
      if (model == null) return new StatusCodeResult(500);

      var answer = model.Adapt<Answer>();

      answer.CreatedDate = DateTime.Now;
      answer.LastModifiedDate = answer.CreatedDate;

      DbContext.Answers.Add(answer);
      DbContext.SaveChanges();

      return new JsonResult(answer.Adapt<AnswerViewModel>(), JsonSettings);
    }

    ///<summary>
    ///Modyfikuje odpowiedź o podanym {id}
    ///</summary>
    ///<param name="model">obiekt AnswerViewModel z danymi do uaktualnienia</param>
    [HttpPut]
    public IActionResult Put([FromBody]AnswerViewModel model)
    {
      if (model == null) return new StatusCodeResult(500);

      var answer = DbContext.Answers.Where(q => q.Id == model.Id).FirstOrDefault();

      if (answer == null) return NotFound(new
      {
        Error = String.Format("Nie znaleziono odpowiedzi o identyfikatorze {0}", model.Id)
      });

      answer.QuestionId = model.QuestionId;
      answer.Text = model.Text;
      answer.Value = model.Value;
      answer.Notes = model.Notes;
      answer.LastModifiedDate = DateTime.Now;

      DbContext.SaveChanges();

      return new JsonResult(answer.Adapt<AnswerViewModel>(), JsonSettings);
    }

    ///<summary>
    ///Usuwa odpowiedź o podanym {id} z bazy danych
    ///</summary>
    ///<param name="id">id istniejacej odpowiedzi</param>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var answer = DbContext.Answers.Where(q => q.Id == id).FirstOrDefault();

      if (answer == null) return NotFound(new
      {
        Error = String.Format("Nie znaleziono odpowiedzi o identyfikatorze {0}", id)
      });

      DbContext.Answers.Remove(answer);
      DbContext.SaveChanges();

      return new NoContentResult();
    }
    #endregion

    #region Metody routingu bazujące na atrybutach
    //GET api/answer/all
    [HttpGet("All/{questionId}")]
    public IActionResult All(int questionId)
    {
      #region temp
      //var sampleAnswers = new List<AnswerViewModel>();

      ////Dodaj pierwszą przykładową odpowiedź
      //sampleAnswers.Add(new AnswerViewModel()
      //{
      //  Id = 1,
      //  QuestionId = questionId,
      //  Text = "Przyjaciół i rodzinę",
      //  CreatedDate = DateTime.Now,
      //  LastModifiedDate = DateTime.Now
      //});

      ////Dodaj kilka następnych przykładowych odpowiedzi
      //for(int i = 2; i <= 5; i++)
      //{
      //  sampleAnswers.Add(new AnswerViewModel()
      //  {
      //    Id = i,
      //    QuestionId = questionId,
      //    Text = String.Format("Przykładowa odpowiedź {0}", i),
      //    CreatedDate = DateTime.Now,
      //    LastModifiedDate = DateTime.Now
      //  });    
      //}
      #endregion
      var answers = DbContext.Answers.Where(q => q.QuestionId == questionId).ToList();

      //Przekaż wyniki w formacie JSON
      return new JsonResult(answers.Adapt<List<AnswerViewModel>>(), JsonSettings);
    }
    #endregion
  }
}
