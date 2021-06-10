using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerWeb.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace TestMakerWeb.Controllers
{
  [Route("api/[controller]")]
  public class AnswerController : Controller
  {
    #region Metody dostosowujące do konwencji REST
    ///<summary>
    ///Pobiera odpowiedź o podanym {id}
    ///</summary>
    ///<param name="id">identyfikator istniejącej odpowiedzi</param>
    ///<returns>odpowiedź o podanym {id}</returns>
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      return Content("Jeszcze niezaimplementowane");
    }

    ///<summary>
    ///Dodaje nową odpowiedź do bazy danych
    ///</summary>
    ///<param name="model">obiekt AnswerViewModel z danymi do wstawienia</param>
    [HttpPut]
    public IActionResult Put(AnswerViewModel model)
    {
      throw new NotImplementedException();
    }

    ///<summary>
    ///Modyfikuje odpowiedź o podanym {id}
    ///</summary>
    ///<param name="model">obiekt AnswerViewModel z danymi do uaktualnienia</param>
    [HttpPost]
    public IActionResult Post(AnswerViewModel model)
    {
      throw new NotImplementedException();
    }

    ///<summary>
    ///Usuwa odpowiedź o podanym {id} z bazy danych
    ///</summary>
    ///<param name="id">id istniejacej odpowiedzi</param>
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      throw new NotImplementedException();
    }
    #endregion

    #region Metody routingu bazujące na atrybutach
    //GET api/answer/all
    [HttpGet("All/{questionId}")]
    public IActionResult All(int questionId)
    {
      var sampleAnswers = new List<AnswerViewModel>();

      //Dodaj pierwszą przykładową odpowiedź
      sampleAnswers.Add(new AnswerViewModel()
      {
        Id = 1,
        QuestionId = questionId,
        Text = "Przyjaciół i rodzinę",
        CreatedDate = DateTime.Now,
        LastModifiedDate = DateTime.Now
      });

      //Dodaj kilka następnych przykładowych odpowiedzi
      for(int i = 2; i <= 5; i++)
      {
        sampleAnswers.Add(new AnswerViewModel()
        {
          Id = i,
          QuestionId = questionId,
          Text = String.Format("Przykładowa odpowiedź {0}", i),
          CreatedDate = DateTime.Now,
          LastModifiedDate = DateTime.Now
        });    
      }

      //Przekaż wyniki w formacie JSON
      return new JsonResult(
        sampleAnswers,
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }
    #endregion
  }
}
