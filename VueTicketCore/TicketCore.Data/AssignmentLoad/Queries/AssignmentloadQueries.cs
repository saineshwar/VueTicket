using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketCore.Models.AssignmentLoad;

namespace TicketCore.Data.AssignmentLoad.Queries
{
    public class AssignmentloadQueries : IAssignmentloadQueries
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public AssignmentloadQueries(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public DefaultTicketSettings GetDefaultTicketCount()
        {
            var isaddedcheck = (from defaultTickets in _vueTicketDbContext.DefaultTicketSettings
                                select defaultTickets).FirstOrDefault();
            return isaddedcheck;
        }

      
    }
}