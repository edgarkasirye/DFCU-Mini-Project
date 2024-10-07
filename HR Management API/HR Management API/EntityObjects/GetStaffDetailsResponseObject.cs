using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_Management_API.EntityObjects
{
    public class GetStaffDetailsResponseObject
    {
        private string status, statusDescription;
        private List<StaffObject> staffList;

        public List<StaffObject> StaffList
        {
            get
            {
                return staffList;
            }
            set
            {
                staffList = value;
            }
        }

        public string StatusCode
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public string StatusDescription
        {
            get
            {
                return statusDescription;
            }
            set
            {
                statusDescription = value;
            }
        }
    }
}