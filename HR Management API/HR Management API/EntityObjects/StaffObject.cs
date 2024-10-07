using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HR_Management_API.EntityObjects
{
    public class StaffObject
    {
        private string staffId, surname, othernames, idphoto;
        private DateTime dob;

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

        
        public string SurName
        {
            get
            {
                return surname;
            }
            set
            {
                surname = value;
            }
        }

        public string OtherNames
        {
            get
            {
                return othernames;
            }
            set
            {
                othernames = value;
            }
        }

        public DateTime DOB
        {
            get
            {
                return dob;
            }
            set
            {
                dob = value;
            }
        }
        public string IDPhoto
        {
            get
            {
                return idphoto;
            }
            set
            {
                idphoto = value;
            }
        }
    }
}