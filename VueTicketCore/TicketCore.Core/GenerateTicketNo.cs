using System;
using TicketCore.Common;
using TicketCore.Data.Tickets.Queries;

namespace TicketCore.Core
{
    public class GenerateTicketNo : IGenerateTicketNo
    {
        private readonly ITicketNumberGeneratorQueries ITicketNumberGeneratorQueries;
        public GenerateTicketNo(ITicketNumberGeneratorQueries ticketNumberGeneratorQueries)
        {
            ITicketNumberGeneratorQueries = ticketNumberGeneratorQueries;
        }

        public string ApplicationNo(out int runningTicketno)
        {
            try
            {

                var currentDate = DateTime.Now;
                var lastTwoDigitsOfYear = currentDate.ToString("yy");
                var randomone = GenerateRandomStrings.RandomString(5);
                 runningTicketno = ITicketNumberGeneratorQueries.GetLatestTicketNo();
                return string.Concat("T", "-", lastTwoDigitsOfYear, "-", randomone, "-", runningTicketno);

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
