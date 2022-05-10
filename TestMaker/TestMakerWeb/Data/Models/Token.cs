using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMakerWeb.Data
{
  public class Token
  {
    #region Konstruktor
    public Token()
    {

    }
    #endregion

    #region Właściwości
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string ClientId { get; set; }
    public int Type { get; set; }

    [Required]
    public string Value { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime LastModifiedDate { get; set; }
    #endregion

    #region Właściwości wczytywane leniwie
    ///<summary>
    ///Nadrzędne pytanie
    ///</summary>
    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; }
    #endregion
  }
}
