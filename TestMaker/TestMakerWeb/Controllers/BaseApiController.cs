﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestMakerWeb.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace TestMakerWeb.Controllers
{
  [Route("api/[controller]")]
  public class BaseApiController : Controller
  {
    public BaseApiController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
      IConfiguration configuration)
    {
      DbContext = context;
      RoleManager = roleManager;
      UserManager = userManager;
      Configuration = configuration;

      JsonSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
    }

    protected ApplicationDbContext DbContext { get; private set; }
    protected RoleManager<IdentityRole> RoleManager { get; private set; }
    protected UserManager<ApplicationUser> UserManager { get; private set; }
    protected IConfiguration Configuration { get; private set; }
    protected JsonSerializerSettings JsonSettings { get; private set; }
  }
}
