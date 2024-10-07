using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HR_Management_API.EntityObjects
{
    public class StaffCreationRequestObject
    {
        private string temporaryId, surname, othernames, idPicture;
        private DateTime dob;

        
        [Required(ErrorMessage = "Error. TemporaryId is required")]
        [StringLength(10, ErrorMessage = "Invalid TemporaryId")]
        public string TemporaryId
        {
            get
            {
                return temporaryId;
            }
            set
            {
                temporaryId = value;
            }
        }

        public string Surname
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

        public string IDPicture
        {
            get
            {
                return idPicture;
            }
            set
            {
                idPicture = value;
            }
        }
    }
}