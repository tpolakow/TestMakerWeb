using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerWeb.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace TestMakerWeb.Controllers
{
  [Route("api/[controller]")]
  public class QuestionController : Controller
  {
    //GET api/question/all
    [HttpGet("All/{quizId}")]
    public IActionResult All(int quizId)
    {
      var sampleQuestions = new List<QuestionViewModel>();

      //Dodaj pierwsze przykładowe pytanie
      sampleQuestions.Add(new QuestionViewModel()
      {
        Id = 1,
        QuizId = quizId,
        Text = "Co cenisz w swoim życiu najbradziej?",
        CreatedDate = DateTime.Now,
        LastModifiedDate = DateTime.Now
      });

      //Dodaj kilka innych przykładowych pytań
      for(int i=2; i <= 5; i++)
      {
        sampleQuestions.Add(new QuestionViewModel()
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
        sampleQuestions,
        new JsonSerializerSettings()
        {
          Formatting = Formatting.Indented
        });
    }
  }
}
