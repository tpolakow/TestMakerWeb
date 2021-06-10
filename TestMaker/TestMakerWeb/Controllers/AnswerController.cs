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

        //Przekaż wyniki w formacie JSON
        return new JsonResult(
          sampleAnswers,
          new JsonSerializerSettings()
          {
            Formatting = Formatting.Indented
          });
      }
    }
  }
}
