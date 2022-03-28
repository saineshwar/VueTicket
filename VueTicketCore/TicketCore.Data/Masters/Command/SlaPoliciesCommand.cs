using System;
using TicketCore.Models.Masters;

namespace TicketCore.Data.Masters.Command
{
    public class SlaPoliciesCommand : ISlaPoliciesCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public SlaPoliciesCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }
        public int? Add(SlaPolicies slaPolicies)
        {
            try
            {
                _vueTicketDbContext.SlaPolicies.Add(slaPolicies);
                _vueTicketDbContext.SaveChanges();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int Delete(int? slaPoliciesId)
        {
            try
            {
                var slaPolicies = _vueTicketDbContext.SlaPolicies.Find(slaPoliciesId);
                if (slaPolicies != null)
                    _vueTicketDbContext.SlaPolicies.Remove(slaPolicies);
                _vueTicketDbContext.SaveChanges();

                return 1;
            }
            catch (Exception)
            {
                return -1;
            }

        }

        public int? AddReminder(SlaPoliciesReminder slaPoliciesReminder)
        {
            try
            {
                _vueTicketDbContext.SlaPoliciesReminder.Add(slaPoliciesReminder);
                _vueTicketDbContext.SaveChanges();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}