﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMakerWeb.Data
{
  public class Question
  {
    #region Konstruktor
    public Question()
    {

    }
    #endregion

    #region Właściwości
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public int QuizId { get; set; }

    [Required]
    public string Text { get; set; }

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
    ///Nadrzędny quiz
    ///</summary>
    [ForeignKey("QuizId")]
    public virtual Quiz Quiz { get; set; }

    ///<summary>
    ///Lista zawierająca odpowiedzi powiązane z pytaniem.
    ///</summary>
    public virtual List<Answer> Answers { get; set; }
    #endregion
  }
}
