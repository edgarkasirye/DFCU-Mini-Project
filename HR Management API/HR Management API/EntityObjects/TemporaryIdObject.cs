using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_Management_API.EntityObjects
{
    public class TemporaryIdObject
    {
        private string temporaryId, status;
        private DateTime creationdate, expirationdate;

        public string TemporaryId { get { return temporaryId; } set {  temporaryId = value; } }

        public DateTime Creationdate { get {  return creationdate; } set {  creationdate = value; } }

        public string Status { get { return status; } set {  status = value; } }

        public DateTime Expirationdate { get { return expirationdate; } set {  expirationdate = value; } }
    }
}