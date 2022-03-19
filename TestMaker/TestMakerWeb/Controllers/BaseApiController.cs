using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestMakerWeb.Data;
using Newtonsoft.Json;

namespace TestMakerWeb.Controllers
{
  [Route("api/[controller]")]
  public class BaseApiController : Controller
  {
    public BaseApiController(ApplicationDbContext context)
    {
      DbContext = context;
      JsonSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
    }

    protected ApplicationDbContext DbContext { get; private set; }
    protected JsonSerializerSettings JsonSettings { get; private set; }
  }
}
