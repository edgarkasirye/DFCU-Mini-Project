using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace HR_Management_API.EntityObjects
{
    public class LoginResponseObject
    {
        private string statusCode, statusDescription;
        private SessionObject sessionDetails;

        public SessionObject SessionDetails { get { return sessionDetails;} set { sessionDetails = value; } }
        public string StatusCode { get { return statusCode; } set { statusCode = value; } }
        public string StatusDescription { get { return statusDescription; } set { statusDescription = value; } }
    }
}