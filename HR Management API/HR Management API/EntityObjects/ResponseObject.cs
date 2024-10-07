using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_Management_API.EntityObjects
{
    public class ResponseObject
    {
        private string statusCode, statusDescription;

        public string StatusCode
        {
            get
            {
                return statusCode;
            }
            set
            {
                statusCode = value;
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