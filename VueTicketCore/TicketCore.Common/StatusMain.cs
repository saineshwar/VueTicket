namespace TicketCore.Common
{
    public static class StatusMain
    {
        public enum Status
        {
            Open = 1,
            Resolved = 2,
            InProgress = 3,
            OnHold = 4,
            RecentlyEdited = 5,
            Replied = 6,
            Deleted = 7,
            First_Response_Overdue = 8,
            Resolution_Overdue = 9,
            Every_Response_Overdue=10,
            Escalation_Stage_1 = 11,
            Escalation_Stage_2 = 12,
            Closed =13,
            ReOpened=14
        }

        public enum Roles
        {
            SuperAdmin = 1,
            User = 2,
            Admin = 3,
            Agent = 4,
            AgentManager = 5,
            Administrator = 6
        }

        public enum Priority
        {
            Urgent = 1,
            High = 2,
            Medium = 3,
            Low = 4
        }

        public enum Activities
        {
            Created = 1,
            AutoAgentAssigned,
            Replied,
            Restored,
            EditedTicket,
            EditedTicketReply,
            Resolved,
            AutoAssigned,
            DeleteTicket,
            DeleteTicketReply,
            PriorityChanged,
            CategoryChanged,
            StatusChanged,
            ManuallyAssigedTicket,
            RepliedandChangedStatus,
            DeletedAttachment,
            DeleteReplyAttachment,
            AutoFirstResponseDue,
            AutoEveryresponseDue,
            AutoResolutionDue,
            Resolved_First_Response_Due,
            Resolved_Every_response_Due,
            Resolved_Resolution_Due,
            Resolved_Escalation_stage_1,
            Resolved_Escalation_stage_2,
            Transferred_Tickets
        }
    }
}