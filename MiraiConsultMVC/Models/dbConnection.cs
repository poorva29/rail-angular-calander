﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiraiConsultMVC.Models
{
    public class dbConnection
    {
        public dbConnection()
        {
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
        }
    }   
}