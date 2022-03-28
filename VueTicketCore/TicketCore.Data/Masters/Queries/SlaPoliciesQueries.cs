using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using TicketCore.Models.Masters;
using TicketCore.ViewModels.Masters;

namespace TicketCore.Data.Masters.Queries
{
    public class SlaPoliciesQueries : ISlaPoliciesQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public SlaPoliciesQueries(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public bool CheckPoliciesExists(int? priorityId)
        {
            try
            {
                var result = (from slaPolicy in _vueTicketDbContext.SlaPolicies
                              where slaPolicy.PriorityId == priorityId
                              select slaPolicy).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public IQueryable<SlaPoliciesShowViewModel> ShowAllSLA(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryable = (from slaPoliciese in _vueTicketDbContext.SlaPolicies
                                 join priority in _vueTicketDbContext.Priority on slaPoliciese.PriorityId equals priority.PriorityId
                                 orderby slaPoliciese.CreateDate descending
                                 select new SlaPoliciesShowViewModel()
                                 {
                                     NextResponseHour = slaPoliciese.NextResponseHour ?? 0,
                                     NextResponseMins = slaPoliciese.NextResponseMins ?? 0,
                                     PriorityName = priority.PriorityName,
                                     FirstResponseHour = slaPoliciese.FirstResponseHour ?? 0,
                                     FirstResponseMins = slaPoliciese.FirstResponseMins ?? 0,
                                     ResolutionResponseHour = slaPoliciese.ResolutionResponseHour ?? 0,
                                     ResolutionResponseMins = slaPoliciese.ResolutionResponseMins ?? 0,
                                     SlaPoliciesId = slaPoliciese.SlaPoliciesId,
                                     EscalationStatus = slaPoliciese.EscalationStatus == true ? "Active" : "InActive"

                                 });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryable = queryable.OrderBy(sortColumn + " " + sortColumnDir);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    queryable = queryable.Where(m => m.PriorityName.Contains(search));
                }

                return queryable;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public IQueryable<SlaPoliciesReminderShowViewModel> ShowAllSLAReminder(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryable = (from SlaPoliciesReminder in _vueTicketDbContext.SlaPoliciesReminder
                                 join businessHour in _vueTicketDbContext.BusinessHours on SlaPoliciesReminder.BusinessHoursId equals businessHour.BusinessHoursId
                                 orderby businessHour.CreatedOn descending
                                 select new SlaPoliciesReminderShowViewModel()
                                 {
                                     SlaPoliciesReminderId = SlaPoliciesReminder.SlaPoliciesReminderId,
                                     NextResponseHour = SlaPoliciesReminder.NextResponseHour ?? 0,
                                     NextResponseMins = SlaPoliciesReminder.NextResponseMins ?? 0,
                                     BusinessHours = businessHour.Name,
                                     FirstResponseHour = SlaPoliciesReminder.FirstResponseHour ?? 0,
                                     FirstResponseMins = SlaPoliciesReminder.FirstResponseMins ?? 0,
                                     ResolutionResponseHour = SlaPoliciesReminder.ResolutionResponseHour ?? 0,
                                     ResolutionResponseMins = SlaPoliciesReminder.ResolutionResponseMins ?? 0,
                                 });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryable = queryable.OrderBy(sortColumn + " " + sortColumnDir);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    queryable = queryable.Where(m => m.BusinessHours.Contains(search));
                }

                return queryable;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool CheckSlaPoliciesReminderExists(int BusinessHoursId)
        {
            try
            {
                var result = (from slaPolicyrReminder in _vueTicketDbContext.SlaPoliciesReminder
                              where slaPolicyrReminder.BusinessHoursId == BusinessHoursId
                              select slaPolicyrReminder).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}