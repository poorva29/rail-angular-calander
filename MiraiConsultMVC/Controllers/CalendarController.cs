using MiraiConsultMVC.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MiraiConsultMVC.Controllers
{
    public class CalendarController : ApiController
    {
        // GET api/calendar/doclocations
        [AcceptVerbs("GET")]
        [MiraiAuthorize]
        public IEnumerable<Object> doclocations()
        {
            return (from user in new EFModelContext().users
                    where user.usertype == (int)UserType.Doctor
                    select new
                    {
                        id = user.userid,
                        user.firstname,
                        user.lastname,
                        locations = user.doclocations.Select(loc =>
                         new
                         {
                             id = loc.doctorlocationid,
                             name = loc.clinicname
                         }
                        )
                    }).ToList();
        }

    }
}