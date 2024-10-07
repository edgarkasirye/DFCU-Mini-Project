using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.HR_Mgt_API;

namespace WebApplication1
{
    public partial class pagesregstar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
 

        protected void btn_login_Click(object sender, EventArgs e)
        {
            string temporaryid = tempNo.Text;


            if (string.IsNullOrEmpty(temporaryid))
            {
                ShowMessage("Provide you password");
            }
            else
            {
                //Lets submit to the API


            }

        }
        private void ShowMessage(string v)
        {
            throw new NotImplementedException();
        }

    }
}