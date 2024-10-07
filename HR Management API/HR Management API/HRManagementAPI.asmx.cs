using HR_Management_API.ControlObjects;
using HR_Management_API.EntityObjects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;

namespace HR_Management_API
{
    /// <summary>
    /// Summary description for HRManagementAPI
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HRManagementAPI : System.Web.Services.WebService
    {
        BusinessLogic bl = new BusinessLogic();

        [WebMethod]
        public StaffCreationResposeObject RegisterNewStaff(string TemporaryID, string Surname, string Othernames, DateTime DOB, string IDPhoto)
        {
            //No sessionId validation because this can be accessed without logging in.

            StaffCreationResposeObject Response = new StaffCreationResposeObject();

            try
            {
                //Lets get the IP address where the request is coming from - incase we need to do an ip whitelist,
                //or for audit purposes should we need to, in an investigation.

                string ipAddress = HttpContext.Current.Request.UserHostAddress;
                string forwardedIp = HttpContext.Current.Request.Headers["X-Forwarded-For"];

                if (!string.IsNullOrEmpty(forwardedIp))
                {
                    // If there are multiple IPs in the X-Forwarded-For header, get the first one
                    ipAddress = forwardedIp.Split(',')[0];
                }

                //Okay, lets create a string to capture the request as we received it.
                string requeststring = "TemporaryID: " + TemporaryID + "Surname: " + Surname +
                    "Othernames: " + Othernames + "DOB: " + DOB.ToString() + "IDPhoto: " + IDPhoto;

                //log request
                bl.LogRequests(requeststring, "RegisterNewStaff", DateTime.Now, ipAddress);

                //Create the Request Object
                StaffCreationRequestObject obj = new StaffCreationRequestObject();
                obj.TemporaryId = TemporaryID.Trim();
                obj.Surname = Surname.Trim();
                obj.OtherNames = Othernames.Trim();
                obj.DOB = DOB;
                obj.IDPicture = IDPhoto.Trim();

                //Validate the TemporaryId
                ResponseObject validationResponse = bl.ValidateTemporaryId(obj.TemporaryId);

                if (validationResponse.StatusCode != "100")
                {
                    //If the Validation Failed
                    Response.StatusCode = validationResponse.StatusCode;
                    Response.StatusDescription = validationResponse.StatusDescription;
                }
                else
                {
                    //If Validation checks out, then we proceed to create the Staff and return StaffID
                    string staffId = bl.RegisterNewStaff(obj);

                    Response.StaffId = staffId;
                    Response.StatusCode = validationResponse.StatusCode;
                    Response.StatusDescription = "SUCCESS";
                }

            }
            catch (Exception ex)
            {
                Response.StatusCode = "101";
                Response.StatusDescription = "Something went wrong. Contact Support";
            }

            //log response
            string responseString = JsonSerializer.Serialize(Response);
            bl.LogResponses(responseString, "RegisterNewStaff", DateTime.Now);

            return Response;
        }


        [WebMethod]
        public GetStaffDetailsResponseObject GetStaffDetails(string StaffID, string sessionId)
        {
            GetStaffDetailsResponseObject Response = new GetStaffDetailsResponseObject();

            try
            {
                string ipAddress = HttpContext.Current.Request.UserHostAddress;
                string forwardedIp = HttpContext.Current.Request.Headers["X-Forwarded-For"];

                if (!string.IsNullOrEmpty(forwardedIp))
                {
                    // If there are multiple IPs in the X-Forwarded-For header, get the first one
                    ipAddress = forwardedIp.Split(',')[0];
                }

                //Okay, lets create a string to capture the request as we received it.
                string requeststring = "StaffID: " + StaffID + "SessionId: " + sessionId;

                //log request
                bl.LogRequests(requeststring, "GetStaffDetails", DateTime.Now, ipAddress);


                //Check whether this is a valid SessionId
                if (!bl.ValidateSessionId(sessionId))
                {
                    Response.StatusCode = "101";
                    Response.StatusDescription = "Invalid SessionId";
                }
                else
                {

                    string staffId = StaffID.Trim();

                    //Validate the StaffID, if the staffId is an empty string, no need to subject that to the validation check
                    if (!(staffId == "" || bl.ValidateStaffId(staffId)))
                    {
                        Response.StatusCode = "101";
                        Response.StatusDescription = "Invalid Staff ID. Try Again";
                    }
                    else
                    {
                        //Now we can go and retrieve the staff details
                        Response.StaffList = bl.RetrieveStaffDetails(staffId);
                        Response.StatusCode = "100";
                        Response.StatusCode = "SUCCESS";
                    }
                }

            }
            catch (Exception ex)
            {
                Response.StatusCode = "101";
                Response.StatusDescription = "Something went wrong. Contact Support";
            }

            //log response
            string responseString = JsonSerializer.Serialize(Response);
            bl.LogResponses(responseString, "GetStaffDetails", DateTime.Now);

            return Response;
        }

        [WebMethod]
        public ResponseObject UpdateStaffDetails(string StaffId, DateTime NewDOB, string NewIDPhoto, string username, string sessionId)
        {
            ResponseObject Response = new ResponseObject();

            try
            {
                string ipAddress = HttpContext.Current.Request.UserHostAddress;
                string forwardedIp = HttpContext.Current.Request.Headers["X-Forwarded-For"];

                if (!string.IsNullOrEmpty(forwardedIp))
                {
                    // If there are multiple IPs in the X-Forwarded-For header, get the first one
                    ipAddress = forwardedIp.Split(',')[0];
                }

                //Okay, lets create a string to capture the request as we received it.
                string requeststring = "StaffID: " + StaffId + "NewDOB: " + NewDOB + "NewIDPhoto: " + NewIDPhoto + "username: " + username + "SessionId: " + sessionId;

                //log request
                bl.LogRequests(requeststring, "UpdateStaffDetails", DateTime.Now, ipAddress);

                //Check whether this is a valid SessionId
                if (!bl.ValidateSessionId(sessionId))
                {
                    Response.StatusCode = "101";
                    Response.StatusDescription = "Invalid SessionId";
                }
                else
                {

                    string staffId = StaffId.Trim();

                    //Validate the StaffId
                    if (!bl.ValidateStaffId(staffId))
                    {
                        Response.StatusCode = "101";
                        Response.StatusDescription = "Invalid Staff ID. Try Again";
                    }
                    else
                    {
                        //Now we can go and update the staff details
                        Response = bl.UpdateStaffDetails(staffId, NewDOB, NewIDPhoto, username);
                    }
                }

            }
            catch (Exception ex)
            {
                Response.StatusCode = "101";
                Response.StatusDescription = "Something went wrong. Contact Support";
            }

            //log response
            string responseString = JsonSerializer.Serialize(Response);
            bl.LogResponses(responseString, "UpdateStaffDetails", DateTime.Now);

            return Response;
        }

        [WebMethod]
        public LoginResponseObject Login(string username, string password)
        {

            SessionObject obj = new SessionObject();
            LoginResponseObject responseObject = new LoginResponseObject();

            try
            {
                //we are not going to log the password so that we can keep it private to the user,
                //not even to the internal staff that have access to this Data.
                //Infact we could encrypt the password instead, but since this is out of scope for
                //the assignment, we are just going to go the simple non encrypted format

                string ipAddress = HttpContext.Current.Request.UserHostAddress;
                string forwardedIp = HttpContext.Current.Request.Headers["X-Forwarded-For"];

                if (!string.IsNullOrEmpty(forwardedIp))
                {
                    ipAddress = forwardedIp.Split(',')[0];
                }

                //log request
                bl.LogRequests("Username: " + username, "Login", DateTime.Now, ipAddress);

                obj = bl.Login(username, password);

                if (obj.SessionStatus != "Invalid UserName")
                {
                    responseObject.SessionDetails = obj;
                    responseObject.StatusCode = "100";
                    responseObject.StatusDescription = "SUCCESS";
                }
                else
                {
                    responseObject.StatusCode = "101";
                    responseObject.StatusDescription = obj.SessionStatus;
                }
            }
            catch
            {
                responseObject.StatusCode = "101";
                responseObject.StatusDescription = "Something went wrong. Contact Support";
            }

            //log response
            string responseString = JsonSerializer.Serialize(responseObject);

            bl.LogResponses(responseString, "Login", DateTime.Now);

            return responseObject;
        }

        [WebMethod]
        public ResponseObject Logout(string sessionId)
        {
            ResponseObject obj = new ResponseObject();

            try
            {
                string ipAddress = HttpContext.Current.Request.UserHostAddress;
                string forwardedIp = HttpContext.Current.Request.Headers["X-Forwarded-For"];

                if (!string.IsNullOrEmpty(forwardedIp))
                {
                    ipAddress = forwardedIp.Split(',')[0];
                }

                //log request
                bl.LogRequests("SessionId: " + sessionId, "Login", DateTime.Now, ipAddress);

                obj = bl.LogOut(sessionId);
            }
            catch (Exception ex)
            {
                obj.StatusCode = "101";
                obj.StatusDescription = "Something went wrong. Contact Support";
            }

            //log response
            string responseString = JsonSerializer.Serialize(obj);
            bl.LogResponses(responseString, "Logout", DateTime.Now);

            return obj;
        }

        [WebMethod]
        public GetTemporaryIdsResponseObject CreateTemporaryIDs(int count, int durationinseconds, string sessionId)
        {
            GetTemporaryIdsResponseObject Response = new GetTemporaryIdsResponseObject();

            try
            {
                string ipAddress = HttpContext.Current.Request.UserHostAddress;
                string forwardedIp = HttpContext.Current.Request.Headers["X-Forwarded-For"];

                if (!string.IsNullOrEmpty(forwardedIp))
                {
                    ipAddress = forwardedIp.Split(',')[0];
                }

                string requeststring = "Count: " + count + "DurabtionInSeconds: " + durationinseconds +
                    "SessionId: " + sessionId;

                //log request
                bl.LogRequests(requeststring, "CreateTemporaryIDs", DateTime.Now, ipAddress);

                //Check whether this is a valid SessionId
                if (!bl.ValidateSessionId(sessionId))
                {
                    Response.StatusCode = "101";
                    Response.StatusDescription = "Invalid SessionId";
                }
                else
                {
                    Response.TemporaryIds = bl.GetTemporaryIds(count, durationinseconds);
                    Response.StatusCode = "100";
                    Response.StatusDescription = "SUCCESS";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = "101";
                Response.StatusDescription = "Something went wrong. Contact Support";
            }


            string responseString = JsonSerializer.Serialize(Response);
            bl.LogResponses(responseString, "Logout", DateTime.Now);

            return Response;
        }

        [WebMethod]
        public GetTemporaryIdsResponseObject ViewTemporaryIDs(string status, string sessionId)
        {
            GetTemporaryIdsResponseObject Response = new GetTemporaryIdsResponseObject();

            try
            {
                string ipAddress = HttpContext.Current.Request.UserHostAddress;
                string forwardedIp = HttpContext.Current.Request.Headers["X-Forwarded-For"];

                if (!string.IsNullOrEmpty(forwardedIp))
                {
                    ipAddress = forwardedIp.Split(',')[0];
                }

                string requeststring = "status: " + status + "SessionId: " + sessionId;

                //log request
                bl.LogRequests(requeststring, "ViewTemporaryIDs", DateTime.Now, ipAddress);

                //Check whether this is a valid SessionId
                if (!bl.ValidateSessionId(sessionId))
                {
                    Response.StatusCode = "101";
                    Response.StatusDescription = "Invalid SessionId";
                }
                else
                {
                    if (status == "" || status == "ACTIVE" || status == "EXPIRED" || status == "REGISTERED")
                    {
                        //let's get the temporaryids

                        Response.TemporaryIds = bl.GetTemporaryIds(status);

                        if (Response.TemporaryIds.Count == 0)
                        {
                            Response.StatusCode = "100";
                            Response.StatusCode = "No Temporary Ids fit set criteria";
                        }
                        else
                        {
                            Response.StatusCode = "100";
                            Response.StatusCode = "SUCCESS";
                        }
                    }
                    else
                    {
                        Response.StatusCode = "101";
                        Response.StatusDescription = "Invalid Status";
                    }
                }
            }
            catch
            {
                Response.StatusCode = "101";
                Response.StatusDescription = "Something went wrong. Contact Support";
            }


            string responseString = JsonSerializer.Serialize(Response);
            bl.LogResponses(responseString, "Logout", DateTime.Now);

            return Response;
        }

        [WebMethod]
        public string GetAPIStats()
        {
            return "Hello World";
        }
    }
}
