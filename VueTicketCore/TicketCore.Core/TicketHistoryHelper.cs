using System;
using System.Linq;
using TicketCore.Common;
using TicketCore.Data;

namespace TicketCore.Core
{
    public class TicketHistoryHelper : ITicketHistoryHelper
    {
        private readonly VueTicketDbContext _vueTicketDbContext;
        public TicketHistoryHelper(VueTicketDbContext vueTicketDbContext)
        {
            _vueTicketDbContext = vueTicketDbContext;
        }

        public string CreateMessage(int? priorityId, int? departmentId)
        {
            string message = string.Empty;
            string priorityName = GetPriorityNameBypriorityId(priorityId);
            string status = GetStatusBystatusId(Convert.ToInt16(StatusMain.Status.Open));
            string DepartmentName = GetDepartmentNameByDepartmentId(departmentId);

            if (priorityId != null && departmentId != null)
            {
                message = $"Created a new ticket. Set status as {status}, priority as {priorityName}, Department as {DepartmentName}";
            }

            return message;
        }

        public string StatusMessage(int? statusId)
        {
            string message = string.Empty;
            string status = GetStatusBystatusId(statusId);

            // Open | Resolved | InProgress | OnHold | Recently |Edited |Replied
            if (statusId != null &&
                   statusId == Convert.ToInt16(StatusMain.Status.Open)
                || statusId == Convert.ToInt16(StatusMain.Status.Resolved)
                || statusId == Convert.ToInt16(StatusMain.Status.InProgress)
                || statusId == Convert.ToInt16(StatusMain.Status.OnHold)
                || statusId == Convert.ToInt16(StatusMain.Status.RecentlyEdited)
                || statusId == Convert.ToInt16(StatusMain.Status.Replied)
            )
            {
                message = $"Set status as {status}";
            }

            // Deleted
            if (statusId != null && statusId == Convert.ToInt16(StatusMain.Status.Deleted))
            {
                message = $"Deleted ticket";
            }

            // Closed
            if (statusId != null && statusId == Convert.ToInt16(StatusMain.Status.Closed))
            {
                message = $"Closed ticket";
            }

            if (statusId != null && statusId == Convert.ToInt16(StatusMain.Status.ReOpened))
            {
                message = $"ReOpened ticket";
            }


            return message;
        }

        public string PriorityMessage(int? priorityId)
        {
            string message = string.Empty;
            string priorityName = GetPriorityNameBypriorityId(priorityId);

            // Deleted
            if (priorityId != null)
            {
                message = $"Set priority as {priorityName}";
            }

            return message;
        }

        public string DepartmentMessage(int? DepartmentId)
        {
            string message = string.Empty;
            string departmentName = GetDepartmentNameByDepartmentId(DepartmentId);

            // Deleted
            if (DepartmentId != null)
            {
                message = $"Set Department as {departmentName}";
            }

            return message;
        }

        public string ReplyMessage(int? statusId)
        {
            string message = string.Empty;
            string status = GetStatusBystatusId(statusId);

            if (status != null)
            {
                message = $"Replied,a few seconds ago. And Set status as {status}";
            }

            return message;
        }

        public string DeleteTicketReplyMessage()
        {
            var message = $"Deleted Ticket Reply";
            return message;
        }

        public string EditTicket()
        {
            var message = "Edited Ticket";
            return message;
        }

        public string EditReplyTicket()
        {
            var message = "Edited Ticket Reply";
            return message;
        }

        public string DeleteTicketAttachment(string ticketid)
        {
            var message = $"Deleted Ticket #{ticketid} Attachment";
            return message;
        }

        public string DeleteTicketReplyAttachment(string ticketid)
        {
            var message = "Deleted Ticket #{ticketid} Reply Attachment";
            return message;
        }

        public string TicketDelete()
        {
            var message = "Ticket Deleted";
            return message;
        }

        public string TicketRestore()
        {
            var message = "Ticket Restored";
            return message;
        }

        public string AssignTickettoUser()
        {
            var message = "Manually Assign Ticket";
            return message;
        }


        private string GetPriorityNameBypriorityId(int? priorityId)
        {
            var priorityList = (from priority in _vueTicketDbContext.Priority
                                where priority.PriorityId == priorityId
                                select priority.PriorityName).FirstOrDefault();
            return priorityList;
        }

        private string GetStatusBystatusId(int? statusId)
        {
            var statusList = (from status in _vueTicketDbContext.Status
                              where status.StatusId == statusId
                              select status.StatusText).FirstOrDefault();
            return statusList;
        }

        private string GetDepartmentNameByDepartmentId(int? departmentId)
        {
            var departmentname = (from Department in _vueTicketDbContext.Department
                                  where Department.DepartmentId == departmentId
                                  select Department.DepartmentName).FirstOrDefault();
            return departmentname;
        }

        public string TransferMessage(int? fromdepartmentId, int? todepartmentId)
        {
            var fromdepartment = GetDepartmentNameByDepartmentId(fromdepartmentId);
            var todepartment = GetDepartmentNameByDepartmentId(todepartmentId);
            var message = $"Ticket is Transfered from {fromdepartment} to {todepartment}";
            return message;
        }

    }
}