using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace MiraiConsultMVC.Models
{
    public partial class _dbAskMiraiDataContext
    {
        [Function(Name = "askmirai_get_alldoctorsdetails")]
        public IMultipleResults getAllDoctorDetails()
        {
            IExecuteResult lstdoctors = this.ExecuteMethodCall(this, (MethodInfo)MethodInfo.GetCurrentMethod());
            return (IMultipleResults)lstdoctors.ReturnValue;

        }
    }
}