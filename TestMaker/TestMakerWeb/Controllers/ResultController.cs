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
  }
}
