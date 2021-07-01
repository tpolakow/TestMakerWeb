﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMakerWeb.Data
{
  public class Answer
  {
    #region Konstruktor
    public Answer()
    {

    }
    #endregion

    #region Właściwości
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int QuestionId { get; set; }

    [Required]
    public string Text { get; set; }

    [Required]
    public int Value { get; set; }

    public string Notes { get; set; }

    [DefaultValue(0)]
    public int Type { get; set; }

    [DefaultValue(0)]
    public int Flags { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime LastModifiedDate { get; set; }
    #endregion

    #region Właściwości wczytywane leniwie
    ///<summary>
    ///Nadrzędne pytanie
    ///</summary>
    [ForeignKey("QuestionID")]
    public virtual Question Question { get; set; }
    #endregion
  }
}
