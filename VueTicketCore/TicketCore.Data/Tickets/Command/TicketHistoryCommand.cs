using System;
using TicketCore.Models.Tickets;

namespace TicketCore.Data.Tickets.Command
{
    public class TicketHistoryCommand : ITicketHistoryCommand
    {
        private readonly VueTicketDbContext _context;

        public TicketHistoryCommand(VueTicketDbContext context)
        {
            _context = context;
        }

        public void TicketHistory(TicketHistoryModel ticketHistory)
        {
            try
            {
                ticketHistory.TicketHistoryId = 0;
                _context.TicketHistory.Add(ticketHistory);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}