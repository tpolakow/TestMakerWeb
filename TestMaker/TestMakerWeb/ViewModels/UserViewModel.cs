using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TestMakerWeb.ViewModels
{
  [JsonObject (MemberSerialization.OptOut)]
  public class UserViewModel
  {
    #region Konstruktor
    public UserViewModel()
    {

    }
    #endregion

    #region Właściwości
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    #endregion
  }
}
