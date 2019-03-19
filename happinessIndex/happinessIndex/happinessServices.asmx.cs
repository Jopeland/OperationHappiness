using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

// need to talk to mysql
using MySql.Data;
using MySql.Data.MySqlClient;
//and we need this to mainpulate data from a db
using System.Data;

namespace happinessIndex.App_Start
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string VerifyCredentials(string username, string password)
        {
            string dbPass = "";

            // grabbing connection string from config file
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            // instantiating query
            string sqlSelect = $"Select * FROM users WHERE username='{username}' OR email='{username}'";

            // set up our connectino object to be ready to use our connection string
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            // set up command object to use our connection, and our query
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);
            sqlConnection.Open();

            // creating a reader that will parse data returned and allow for storage into a variable
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    // reading value in specific column "Password"
                    dbPass = (string)reader["Password"];
                }

                // checking to see if password in DB matches password provided
                if (dbPass == password)
                {
                    return reader.GetValue(3).ToString();
                }
                else
                {
                    string var = "false";
                    return var;
                }
            }
            finally
            {
                // terminating reader and DB connections
                reader.Close();
                sqlConnection.Close();
            }
        }
    }
}
