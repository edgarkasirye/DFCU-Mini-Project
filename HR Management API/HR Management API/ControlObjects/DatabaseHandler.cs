using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace HR_Management_API.ControlObjects
{
    public class DatabaseHandler
    {
        private Database StaffDb;
        private DbCommand command;

        public DatabaseHandler()
        {
            try
            {
                DatabaseProviderFactory factory = new DatabaseProviderFactory();
                StaffDb = factory.Create("MyStaffDb");
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet ExecuteDataSet(string procedure, params object[] parameters)
        {
            try
            {
                command = StaffDb.GetStoredProcCommand(procedure, parameters);
                return StaffDb.ExecuteDataSet(command);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExecuteNonQuery(string procedure, params object[] parameters)
        {
            try
            {
                command = StaffDb.GetStoredProcCommand(procedure, parameters);
                StaffDb.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}