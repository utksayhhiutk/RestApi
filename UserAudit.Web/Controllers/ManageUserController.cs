using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using UserAudit.Web.Models;

namespace UserAudit.Web.Controllers
{
    public class ManageUserController : ApiController
    {
        String Conn = ConfigurationManager.ConnectionStrings["UserConnectionString"].ConnectionString;
        public SqlConnection GetSqlConnection(string connectionString)
        {
            var con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open)
                con.Open();

            return con;
        }

        // For selecting user record on page load
        [Route("api/user/Get")]
        [HttpGet]
        // GET: api/User
        public IEnumerable<UserModel> Get()
        {
            string query = "Select * FROM [dbo].[User];";
            var con = GetSqlConnection(Conn);
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            List<UserModel> customers = null;
            if (dr.HasRows)
            {
                customers = new List<UserModel>();
                while (dr.Read())
                {
                    UserModel usrdb = new UserModel()
                    {
                        Id = dr["Id"] != DBNull.Value ? (int)dr["Id"] : default(int),
                        FirstName = dr["FirstName"] != DBNull.Value ? dr["FirstName"].ToString() : string.Empty,
                        LastName = dr["LastName"] != DBNull.Value ? dr["LastName"].ToString() : string.Empty,
                        Email = dr["Email"] != DBNull.Value ? dr["Email"].ToString() : string.Empty,
                        Password = dr["Password"] != DBNull.Value ? dr["Password"].ToString() : string.Empty,
                        Status = dr["Status"] != DBNull.Value ? (bool)dr["Status"] : default(bool),
                    };
                    customers.Add(usrdb);
                }
            }
            return customers;
        }

        // For selecting  user Audit report
        [Route("api/user/GetUserLog")]
        [HttpGet]
        // GET: api/User
        public IEnumerable<UserModel> GetUserLog()
        {
            string query = "SELECT *  FROM [dbo].[User] INNER JOIN [dbo].[UserLog] ON [dbo].[User].Email = [dbo].[UserLog].emailid;";
            var con = GetSqlConnection(Conn);
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            List<UserModel> customers = null;
            if (dr.HasRows)
            {
                customers = new List<UserModel>();
                while (dr.Read())
                {
                    UserModel usrdb = new UserModel()
                    {

                        FirstName = dr["FirstName"] != DBNull.Value ? dr["FirstName"].ToString() : string.Empty,
                        Email = dr["Email"] != DBNull.Value ? dr["Email"].ToString() : string.Empty,
                        Intime = dr["Intime"] != DBNull.Value ? Convert.ToDateTime(dr["Intime"]) : default(DateTime),
                        Outtime = dr["Outtime"] != DBNull.Value ? Convert.ToDateTime(dr["Outtime"]) : default(DateTime),
                    };
                    customers.Add(usrdb);
                }
            }
            return customers;
        }
        // For inserting user details
        // POST: api/User  api/user/Post
        [Route("api/user/Post")]
        [HttpPost]
        public void Post(UserModel userobj)
        {
            var con = GetSqlConnection(Conn);

            // SqlCommand cmd = new SqlCommand(query,con);

            using (SqlCommand command = new SqlCommand())
            {
                try
                {
                    if (userobj.Email == "" && userobj.Password == "")
                    {
                        Exception ex;
                    }
                    command.Connection = con;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT into [dbo].[User] (FirstName, LastName, Email,Password,Status) VALUES (@fname, @lname, @email, @passwrd, @status)";
                    command.Parameters.AddWithValue("@fname", userobj.FirstName);
                    command.Parameters.AddWithValue("@lname", userobj.LastName);
                    command.Parameters.AddWithValue("@email", userobj.Email);
                    command.Parameters.AddWithValue("@passwrd", userobj.Password);
                    command.Parameters.AddWithValue("@status", userobj.Status);
                    int recordsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // error here
                }
                finally
                {
                    con.Close();
                }
            }

        }
        // For updating user record
        // PUT: api/User/5
        [Route("api/user/Put")]
        [HttpPost]
        public void Put(UserModel userobj)
        {
            var con = GetSqlConnection(Conn);

            // SqlCommand cmd = new SqlCommand(query,con);

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = con;            // <== lacking
                command.CommandType = CommandType.Text;
                command.CommandText = "Update [dbo].[User] set FirstName=@fname ,LastName=@lname,Email=@email,Password=@password,Status=@status where Id=@id;";
                command.Parameters.AddWithValue("@id", userobj.Id);
                command.Parameters.AddWithValue("@fname", userobj.FirstName);
                command.Parameters.AddWithValue("@lname", userobj.LastName);
                command.Parameters.AddWithValue("@email", userobj.Email);
                command.Parameters.AddWithValue("@password", userobj.Password);
                command.Parameters.AddWithValue("@status", userobj.Status);

                try
                {

                    int recordsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    // error here
                }
                finally
                {
                    con.Close();
                }
            }
        }
        // For deleting user record
        // DELETE: api/User/5
        [Route("api/user/Delete")]
        [HttpPost]
        public void Delete(UserModel usermd)
        {
            string query = "DELETE FROM [dbo].[User] where id=" + usermd.Id;
            var con = GetSqlConnection(Conn);
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr != null)
            {

                dr.Close();
                dr.Dispose();
            }
        }

        // For Selecting user for login and initilizing the session
        // Get: api/User/5
        [Route("api/user/GetUser")]
        [HttpPost]
        public void GetUser(UserModel usermd)
        {
            string query = "select * from [dbo].[User] where Email = '" + usermd.Email + "' and Password='" + usermd.Password + "';";
            var con = GetSqlConnection(Conn);
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr != null)
            {
                while (dr.Read())
                {
                    usermd.FirstName = dr["FirstName"] != DBNull.Value ? dr["FirstName"].ToString() : string.Empty;
                }
                dr.Close();
                dr.Dispose();

                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = con;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "INSERT into [dbo].[UserLog] ( Intime, emailid,username) VALUES (SYSDATETIME(), @email, @fname)";
                        command.Parameters.AddWithValue("@email", usermd.Email);
                        command.Parameters.AddWithValue("@fname", usermd.FirstName);

                        int recordsAffected = command.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        // error here
                    }
                    finally
                    {
                        con.Close();
                    }
                   
                }
            }
        }

        // For updating user session time
        // PUT: api/User/5
        [Route("api/user/PutUserLog")]
        [HttpPost]
        public void PutUserLog(UserModel userobj)
        {
            var con = GetSqlConnection(Conn);

           

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = con;            // <== lacking
                command.CommandType = CommandType.Text;
                command.CommandText = "Update [dbo].[UserLog] set Outtime=SYSDATETIME() where emailid=@email;";
               
                command.Parameters.AddWithValue("@email", userobj.Email);

                try
                {
                    int recordsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    // error here
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
