﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TestMakerWeb.ViewModels
{
  [JsonObject(MemberSerialization.OptOut)]
  public class ResultViewModel
  {
    #region Konstruktor
    public ResultViewModel()
    {

    }
    #endregion

    #region Właściwości
    public int Id { get; set; }
    public int QuizId { get; set; }
    public string Text { get; set; }
    public int? MinValue { get; set; }
    public int? MaxValue { get; set; }
    public string Notes { get; set; }
    [DefaultValue(0)]
    public int Type { get; set; }
    [DefaultValue(0)]
    public int Flags { get; set; }
    [JsonIgnore]
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    #endregion
  }
}
