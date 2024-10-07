using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_Management_API.EntityObjects
{
    public class StaffCreationResposeObject
    {
        private string staffId, status, statusDescription;
        public string StaffId
        {
            get
            {
                return staffId;
            }
            set
            {
                staffId = value;
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