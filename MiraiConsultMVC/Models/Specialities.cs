using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiraiConsultMVC.Models
{
    public class Specialities : _dbAskMiraiDataContext
    {
      public int  specialityid {get; set;}
      public string name{get; set;}
    }
}