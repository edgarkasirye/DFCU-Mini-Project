using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_Management_API.EntityObjects
{
    public class GetTemporaryIdsResponseObject
    {
        private string statuscode, statusDescription;
        List<TemporaryIdObject> temporaryIds;

        public string StatusCode
        {
            get { return statuscode; }
            set { statuscode = value; }
        }
        public string StatusDescription
        {
            get { return statusDescription; }
            set { statusDescription = value; }
        }
        public List<TemporaryIdObject> TemporaryIds
        {
            get { return temporaryIds; }
            set { temporaryIds = value; }

        }
    }
}