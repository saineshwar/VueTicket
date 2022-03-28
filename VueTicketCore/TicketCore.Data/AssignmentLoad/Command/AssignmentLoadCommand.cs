using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketCore.Models.AssignmentLoad;

namespace TicketCore.Data.AssignmentLoad.Command
{
    public class AssignmentLoadCommand : IAssignmentLoadCommand
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public AssignmentLoadCommand(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public int? AddDefaultTicketCount(DefaultTicketSettings defaultTicket)
        {
            if (defaultTicket.DefaultTicketId != 0)
            {
                try
                {

                    if (defaultTicket.DefaultTicketId > 0)
                    {
                        defaultTicket.UpdatedDate = DateTime.Now;
                        _vueTicketDbContext.Entry(defaultTicket).State = EntityState.Modified;
                    }

                    var result = _vueTicketDbContext.SaveChanges();
                    return result;
                }
                catch (Exception)
                {

                    return 0;
                }

            }
            else
            {

                try
                {
                    defaultTicket.CreateDate = DateTime.Now;
                    _vueTicketDbContext.DefaultTicketSettings.Add(defaultTicket);
                    _vueTicketDbContext.SaveChanges();

                    return 1;
                }
                catch (Exception ex)
                {

                    return 0;
                }

            }
        }


    }
}