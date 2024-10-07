using HR_Management_API.EntityObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace HR_Management_API.ControlObjects
{
    public class BusinessLogic
    {
        private static ResponseObject RespObj = new ResponseObject();

        DatabaseHandler dh = new DatabaseHandler();

        public BusinessLogic()
        {

        }

        public ResponseObject ValidateTemporaryId(string TemporaryId)
        {
            DataTable dt = new DataTable();
            ResponseObject Obj = new ResponseObject();

            try
            {
                ///The TemporaryId must be something like "TEMP0002"
                ///Let's use a regular expression to filterout those that conform to this format.

                if (Regex.IsMatch(TemporaryId, @"^TEMP\d{4}$"))
                {

                    dt = dh.ExecuteDataSet("ValidateTemporaryId", new object[] { TemporaryId }).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        string Status = dt.Rows[0]["Status"].ToString();

                        if (Status.Equals("REGISTERED"))
                        {
                            Obj.StatusCode = "101";
                            Obj.StatusDescription = "TemporaryId was already successfully registered";
                        }
                        else if (Status.Equals("PENDING"))
                        {
                            Obj.StatusCode = "100";
                            Obj.StatusDescription = "PENDING";
                        }
                        else if (Status.Equals("EXPIRED"))
                        {
                            Obj.StatusCode = "101";
                            Obj.StatusDescription = "TemporaryId expired, contact HR";
                        }
                        else
                        {
                            Obj.StatusCode = "101";
                            Obj.StatusDescription = "TemporaryId has an Invalid Status, Contact HR";
                        }
                    }
                    else
                    {
                        Obj.StatusCode = "101";
                        Obj.StatusDescription = "Invalid TemporaryId";
                    }
                }
                else
                {
                    Obj.StatusCode = "101";
                    Obj.StatusDescription = "Invalid TemporaryId";
                }
            }
            catch (Exception ex)
            {
                //log the exception
                LogError(ex.Message, TemporaryId, DateTime.Now, "ValidateTemporaryId");

                throw ex;
            }

            return Obj;
        }

        public string RegisterNewStaff(StaffCreationRequestObject reqobj)
        {
            DataTable dt = new DataTable();
            string StaffId = "";

            try
            {
                dt = dh.ExecuteDataSet("RegisterNewStaff", new object[] { reqobj.TemporaryId, reqobj.Surname, reqobj.OtherNames, reqobj.DOB, reqobj.IDPicture }).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    StaffId = dt.Rows[0]["StaffId"].ToString();
                }
                else
                {
                    //Log error
                    LogError("StaffId Not Returned, error while registering new staff", reqobj.TemporaryId, DateTime.Now, "RegisterNewStaff");

                    throw new Exception("StaffId Not Returned, error while registering new staff");
                }

            }
            catch (Exception ex)
            {
                //Log exception
                LogError(ex.Message, reqobj.TemporaryId, DateTime.Now, "RegisterNewStaff");

                throw ex;
            }
            return StaffId;
        }


        public bool ValidateStaffId(string staffId)
        {
            bool isValid;

            try
            {
                ///The StaffID must be something like "DFCU240002"
                ///i.e. a total of 10 characters beginning with "DFCU" followed by 6 digits and not letters or special characters
                ///Let's write a regex to check for that

                if (Regex.IsMatch(staffId, @"^DFCU\d{6}$"))
                {
                    DataTable dt = dh.ExecuteDataSet("GetStaffDetails", new object[] { staffId }).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                }
                else
                {
                    isValid = false;
                }

            }
            catch (Exception ex)
            {
                //Log exception
                LogError(ex.Message, staffId, DateTime.Now, "ValidateStaffId");

                throw ex;
            }

            return isValid;
        }

        public List<StaffObject> RetrieveStaffDetails(string StaffId)
        {
            DataTable dt = new DataTable();
            List<StaffObject> StaffList = new List<StaffObject>();

            try
            {
                dt = dh.ExecuteDataSet("GetStaffDetails", new object[] { StaffId }).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StaffObject staff = new StaffObject();

                        staff.StaffId = dr["StaffId"].ToString();
                        staff.SurName = dr["SurName"].ToString();
                        staff.OtherNames = dr["OtherNames"].ToString();
                        staff.DOB = Convert.ToDateTime(dr["DOB"].ToString());
                        staff.IDPhoto = dr["IDPhoto"].ToString();

                        StaffList.Add(staff);
                    }
                }
                else
                {
                    //This should not happen, but for some reason if it happens

                    string customexception = "Staff Details Not Returned";
                    LogError(customexception, StaffId, DateTime.Now, "RetrieveStaffDetails");
                    throw new Exception(customexception);
                }
            }
            catch (Exception ex)
            {
                //log the exception
                LogError(ex.Message, StaffId, DateTime.Now, "RetrieveStaffDetails");

                throw ex;
            }

            return StaffList;
        }

        public ResponseObject UpdateStaffDetails(string StaffId, DateTime NewDOB, string NewIDPhoto, string updateby)
        {
            ResponseObject Response = new ResponseObject();

            try
            {
                dh.ExecuteNonQuery("UpdateStaffDetails", new object[] { StaffId, NewDOB, NewIDPhoto, updateby });

                Response.StatusCode = "100";
                Response.StatusDescription = "SUCCESS";

            }
            catch (Exception ex)
            {
                //log the exception
                LogError(ex.Message, StaffId, DateTime.Now, "UpdateStaffDetails");

                throw ex;
            }

            return Response;
        }


        public List<TemporaryIdObject> GetTemporaryIds(int count, int duration)
        {
            DataTable dt = new DataTable();
            List<TemporaryIdObject> TemporaryIds = new List<TemporaryIdObject>();

            try
            {
                dt = dh.ExecuteDataSet("CreateTemporaryIds", new object[] { count, duration }).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        TemporaryIdObject temporayId = new TemporaryIdObject();

                        temporayId.TemporaryId = dr["TemporaryId"].ToString();
                        temporayId.Status = dr["Status"].ToString();
                        temporayId.Creationdate = Convert.ToDateTime(dr["CreationDate"].ToString());
                        temporayId.Expirationdate = Convert.ToDateTime(dr["ExpiryDate"].ToString());

                        TemporaryIds.Add(temporayId);
                    }
                }
                else
                {
                    //This should not happen, but for some reason if it happens

                    string customexception = "Temporary Ids Not Returned";
                    LogError(customexception, "", DateTime.Now, "GetTemporaryIds");
                    throw new Exception(customexception);
                }
            }
            catch (Exception ex)
            {
                //log the exception
                LogError(ex.Message, "", DateTime.Now, "GetTemporaryIds");

                throw ex;
            }

            return TemporaryIds;
        }

        public List<TemporaryIdObject> GetTemporaryIds(string status)
        {
            DataTable dt = new DataTable();
            List<TemporaryIdObject> TemporaryIds = new List<TemporaryIdObject>();

            try
            {
                dt = dh.ExecuteDataSet("GetTemporaryIds", new object[] { status }).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        TemporaryIdObject temporayId = new TemporaryIdObject();

                        temporayId.TemporaryId = dr["TemporaryId"].ToString();
                        temporayId.Status = dr["Status"].ToString();
                        temporayId.Creationdate = Convert.ToDateTime(dr["CreationDate"].ToString());
                        temporayId.Expirationdate = Convert.ToDateTime(dr["ExpiryDate"].ToString());

                        TemporaryIds.Add(temporayId);
                    }
                }
                else
                {
                    //if there are temporary Ids befitting the status chosen.
                }
            }
            catch (Exception ex)
            {
                //log the exception
                LogError(ex.Message, "", DateTime.Now, "GetTemporaryIds");

                throw ex;
            }

            return TemporaryIds;
        }

        public bool ValidateSessionId(string sessionId)
        {
            bool isValid;
            try
            {
                ///Session Id is a 6 digit string, lets check 
                ///that it is actually is before we waste resources going to the DB

                if (Regex.IsMatch(sessionId, @"^\d{6}$"))
                {
                    DataTable dt = dh.ExecuteDataSet("ValidateSessionId", new object[] { sessionId }).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        isValid = true;
                    }
                    else
                    {
                        isValid = false;
                    }
                }
                else
                {
                    isValid = false;
                }

            }
            catch (Exception ex)
            {
                //Log exception
                LogError(ex.Message, sessionId, DateTime.Now, "ValidateSessionId");

                throw ex;
            }

            return isValid;
        }


        public SessionObject Login(string username, string password)
        {
            DataTable dt = new DataTable();
            SessionObject Obj = new SessionObject();

            try
            {
                ///The username should only contain characters and/or a dot
                if (Regex.IsMatch(username, @"^[a-zA-Z.]+$"))
                {
                    dt = dh.ExecuteDataSet("Login", new object[] { username, password }).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];

                        //create a sessionId
                        Random random = new Random();
                        int sessionid = random.Next(100000, 999999);

                        Obj.SessionId = sessionid.ToString();
                        Obj.SessionRole = dr["UserRole"].ToString();

                        //update in the database
                        UpdateSessionId(username, password, sessionid);

                        Obj.SessionName = username;
                        Obj.SessionStatus = "SUCCESS";
                                                
                    }
                }
                else
                {
                    Obj.SessionStatus = "Invalid UserName";
                }
            }
            catch (Exception ex)
            {
                //log the exception
                LogError(ex.Message, "", DateTime.Now, "Login");

                throw ex;
            }

            return Obj;
        }

        public bool UpdateSessionId (string username, string password, int sessionid)
        {
            try
            {
                dh.ExecuteNonQuery("UpdateSessionId", new object[] { username, password, sessionid});

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ResponseObject LogOut(string sessionId)
        {
            ResponseObject Obj = new ResponseObject();

            try
            {
                dh.ExecuteNonQuery("LogOut", new object[] { sessionId });

                Obj.StatusCode = "100";
                Obj.StatusDescription = "SUCCESS";
            }
            catch (Exception ex)
            {
                //log the exception
                LogError(ex.Message, sessionId, DateTime.Now, "Logout");

                throw ex;
            }

            return Obj;
        }


        public void LogRequests (string Request, string method, DateTime requesttime, string ipaddress)
        {
            try
            {
                // Log the request in the requests table
                dh.ExecuteNonQuery("LogRequest", new object[] { Request, method, requesttime, ipaddress });
            }
            catch (Exception ex)
            {
                //Log error on file, including why it failed to be logged in the DB

                ///Let's store them on files per day such that we dont have a file grow too big to be opened, but also,
                ///so that we can easily look through these, should be need to, and that we can delete the old logs easily when creating space.

                //Be sure to replace the path with where you want the logs to be stored.

                string filePath = @"D:\Logs\Requests " + DateTime.Now.ToString("yyyyMMMdd") + ".txt";

                string logentry = "Exception at logging: " + ex.Message + Environment.NewLine +
                    "Request: " + Request + Environment.NewLine +
                    "Method: " + method + Environment.NewLine +
                    "OccurancesTime: " + requesttime + Environment.NewLine +
                    "IpAddress: " + ipaddress + Environment.NewLine +
                    "---------------------------------------------------------------" + Environment.NewLine;

                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine(logentry);
                }

            }
        }


        public void LogResponses(string Response, string method, DateTime responsetime)
        {
            try
            {
                // Log the request in the requests table
                dh.ExecuteNonQuery("LogResponse", new object[] { Response, method, responsetime });
            }
            catch (Exception ex)
            {
                //Log on file, including why it failed to be logged in the DB in the first place.

                ///Let's store them on files per day such that we dont have a file grow too big to be opened, but also,
                ///so that we can easily look through these, should be need to, and that we can delete the old logs easily when creating space.

                //Be sure to replace the path with where you want the logs to be stored.

                string filePath = @"D:\Logs\Responses " + DateTime.Now.ToString("yyyyMMMdd") + ".txt";

                string logentry = "Exception at logging: " + ex.Message + Environment.NewLine +
                    "Request: " + Response + Environment.NewLine +
                    "Method: " + method + Environment.NewLine +
                    "OccuranceTime: " + responsetime + Environment.NewLine +
                    "---------------------------------------------------------------" + Environment.NewLine;

                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine(logentry);
                }

            }
        }

        public void LogError(string error, string processID, DateTime occurancetime, string method)
        {
            try
            {
                // Log the error in the errors table
                dh.ExecuteNonQuery("LogError", new object[] { error, processID, occurancetime, method });
            }
            catch (Exception ex)
            {
                //Log error on file, including why it failed to be logged in the DB

                ///Let's store them on files per day such that we dont have a file grow too big to be opened, but also,
                ///so that we can easily look through these, should be need to, and that we can delete the old logs easily when creating space.

                //Be sure to replace the path with where you want the logs to be stored.

                string filePath = @"D:\Logs\Errors " + DateTime.Now.ToString("yyyyMMMdd") + ".txt";

                string errorentry = "Exception at logging: " + ex.Message + Environment.NewLine +
                    "Error: " + error + Environment.NewLine +
                    "ProcessId: " + processID + Environment.NewLine +
                    "OccuranceTime: " + occurancetime + Environment.NewLine +
                    "Method: " + method + Environment.NewLine +
                    "---------------------------------------------------------------" + Environment.NewLine;

                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine(errorentry);
                }

            }
        }
    }
}