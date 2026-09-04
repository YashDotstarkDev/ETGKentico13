using System.Collections.Generic;

namespace ETG.Module.Data
{
    public class ETGUserRole
    {
        public const string ENQUIRY_MANAGEMENT_USER = "EnquiryManagementUser";
        public const string CONSULTANT = "Consultant";
        public const string GLOBAL_CONSULTANT = "GlobalConsultant";
        public const string TEAM_LEADER = "TeamLeader";
        public const string MANAGER = "Manager";
        public const string MARKETING = "Marketing";
    }

    public class ETGEnquireStatus
    {
        public static List<string> Statuses = new List<string>
        {
            "New",
            "1st contact",
            "2nd contact",
            "WIP",
            "Cancelled",
            "Booked"
        };
    }

    public class ETGStatusConfirmed
    {
        public static string[] FieldNames = { "DepartureDate", "ReturnDate", "BookingReference" };
    }
}