using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_Management_API.EntityObjects
{
    public class SessionObject
    {
        private string sessionId, username, role, status;

        public string SessionId { get { return sessionId; } set { sessionId = value; } }
        public string SessionName { get { return username; } set { username = value; } }
        public string SessionRole { get { return role; } set { role = value; } }
        public string SessionStatus { get { return status; } set { status = value; } }
    }
}