using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TicketCore.ViewModels.Audit;

namespace TicketCore.Data.Audit.Queries
{
    public interface IAuditQueries
    {
        List<AuditViewModel> GetUserActivity(long? userId);
    }
}