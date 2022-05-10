﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestMakerWeb.ViewModels
{
  [JsonObject(MemberSerialization.OptOut)]
  public class TokenResponseViewModel
  {
    public TokenResponseViewModel()
    {

    }

    public string token { get; set; }
    public int expiration { get; set; }
  }
}
