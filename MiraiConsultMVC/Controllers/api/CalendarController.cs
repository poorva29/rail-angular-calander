using MiraiConsultMVC.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MiraiConsultMVC.Controllers
{
    public class CalendarController : ApiController
    {
        public static string EVENT_TYPE_BOOKING = "booking";
        public static string EVENT_TYPE_NON_WORKING = "non-working";

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

        // GET api/calendar/calendar_details
        [AcceptVerbs("GET")]
        //[MiraiAuthorize]
        public Object calendar_details(int doclocation_id, DateTime start, DateTime end)
        {
            EFModelContext ctx = new EFModelContext();
            doctorlocation dl = ctx.doctorlocations.Find(doclocation_id);
            if (dl == null)
            {
                return new { error = "No such location!" };
            }
            ICollection<doctorlocationworkinghour> work_hrs = ctx.doctorlocationworkinghours.Where(
                x => x.doclocationid == doclocation_id).ToList();
            if (work_hrs == null || work_hrs.Count < 1)
            {
                return new { error = "No work hours available at this location!" };
            }
            IList<dynamic> appointments = (IList<dynamic>)ctx.appointments.Where(a =>
               a.starttime >= start && a.endtime <= end && a.doclocationid == doclocation_id).
                OrderBy(a => a.starttime).
                Select(a => new
                {
                    a.appointmentid,
                    a.appointmenttype.name,
                    a.starttime,
                    a.endtime,
                    a.subject,
                    a.patientname
                }).ToList().ConvertAll(a => appt_event(a));

            int min = 0;
            int max = 0;
            IDictionary<DayOfWeek, IList<int[]>> nonWorkHrsByDay = non_working_hrs(work_hrs, ref min, ref max);
            IList<dynamic> events = generateEvents(start, end, appointments, nonWorkHrsByDay);
            return new
            {
                calendar = new { slot_duration = "00:" + dl.timeslot + ":00", min = min, max = max },
                work_hrs = work_hrs,
                events = events
            };
        }

        private IList<dynamic> generateEvents(DateTime today, DateTime end, IList<dynamic> appointments, 
            IDictionary<DayOfWeek, IList<int[]>> nonWorkHrsByDay)
        {
            IList<dynamic> events = new List<dynamic>();
            int apptIndex = 0;
            var nextAppt = appointments.Count > apptIndex ? appointments[apptIndex] : null;
            while (today <= end)
            {
                foreach (int[] nwBlock in nonWorkHrsByDay[today.DayOfWeek])
                {
                    events.Insert(events.Count, non_working_event(nwBlock, today));
                }
                if (appointments.Count > apptIndex)
                {
                    nextAppt = appointments[apptIndex];
                    while (nextAppt != null &&
                        (nextAppt.start.Year == today.Year) && (nextAppt.start.DayOfYear == today.DayOfYear))
                    {
                        events.Insert(events.Count, nextAppt);
                        apptIndex += 1;
                        nextAppt = appointments.Count > apptIndex ? appointments[apptIndex] : null;
                    }
                }
                today = today.AddDays(1);
            }
            return events;
        }

        private IDictionary<DayOfWeek, IList<int[]>> non_working_hrs(ICollection<doctorlocationworkinghour> workHrs,
            ref int min, ref int max)
        {
            //Calendar bounds across all days
            min = Int32.Parse(workHrs.First().fromtime);
            max = Int32.Parse(workHrs.Last().totime);
            IDictionary<DayOfWeek, IList<int[]>> workHrsByWeekDay = new Dictionary<DayOfWeek, IList<int[]>>();
            foreach (doctorlocationworkinghour workHr in workHrs)
            {
                int from = Int32.Parse(workHr.fromtime);
                int to = Int32.Parse(workHr.totime);
                min = Math.Min(min, from);
                max = Math.Max(max, to);
                foreach (DayOfWeek weekday in Enum.GetValues(typeof(DayOfWeek)))
                {
                    int fromTime = workHr.fromtimeFor(weekday);
                    int toTime = workHr.totimeFor(weekday);
                    if (fromTime < 0) //non-working day
                        continue;
                    IList<int[]> todaysHrs = workHrsByWeekDay.ContainsKey(weekday) ?
                        workHrsByWeekDay[weekday] : new List<int[]>();
                    todaysHrs.Add(new int[] { from, to });
                    workHrsByWeekDay[weekday] = todaysHrs;
                }
            }
            return calcNonWorkHrs(workHrsByWeekDay, min, max);
        }

        private IDictionary<DayOfWeek, IList<int[]>> calcNonWorkHrs(IDictionary<DayOfWeek, IList<int[]>> workHrsByWeekDay, 
            int min, int max)
        {
            IDictionary<DayOfWeek, IList<int[]>> non_work_hrs = new Dictionary<DayOfWeek, IList<int[]>>();
            foreach(DayOfWeek weekday in Enum.GetValues(typeof(DayOfWeek)))
            {
                IList<int[]> work_hrs = workHrsByWeekDay.ContainsKey(weekday) ?
                    workHrsByWeekDay[weekday] : null;
                if (work_hrs != null)
                { // working day
                    int whCount = 0;
                    IList<int[]> non_work_slots = new List<int[]>();
                    foreach (int[] work_slot in work_hrs)
                    {
                        if (whCount == 0 && min < work_slot[0]) //first working slot
                            non_work_slots.Add(new int[] { min, work_slot[0] });
                        if (whCount == (work_hrs.Count -1) && work_slot[1] < max) //last working slot
                            non_work_slots.Add(new int[] { work_slot[1], max });
                        if (whCount < work_hrs.Count - 1) //there's another working slot after this one.
                            non_work_slots.Add(new int[] { work_slot[1], work_hrs[whCount + 1][0] });
                        whCount++;
                    }
                    non_work_hrs[weekday] = non_work_slots;
                }
                else
                {   //non-working day
                    non_work_hrs[weekday] = new List<int[]>();
                    non_work_hrs[weekday].Add(new int[] { min, max });
                }
            }
            
            return non_work_hrs;
        }

        private object appt_event(dynamic appt)
        {
            return new
            {
                id = appt.appointmentid,
                start = appt.starttime,
                end = appt.endtime,
                event_type = EVENT_TYPE_BOOKING,
                appointment_type = appt.name,
                patient_name = appt.patientname,
                subject = appt.subject
            };
        }

        private dynamic non_working_event(int[] block_times, DateTime today)
        {
            int from_hr = block_times[0] / 100;
            int from_min = block_times[0] % 100;
            int to_hr = block_times[1] / 100;
            int to_min = block_times[1] % 100;
            return new
            {
                id = -1,
                day = today.DayOfWeek,
                start = new DateTime(today.Year, today.Month, today.Day, from_hr, from_min, 0),
                end = new DateTime(today.Year, today.Month, today.Day, to_hr, to_min, 0),
                event_type = EVENT_TYPE_NON_WORKING,
                appointment_type = "",
                patient_name ="",
                subject = ""
            };
        }

    }
}