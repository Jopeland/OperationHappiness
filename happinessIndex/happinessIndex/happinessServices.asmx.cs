using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using Outlook = Microsoft.Office.Interop.Outlook;

// need to talk to mysql
using MySql.Data;
using MySql.Data.MySqlClient;
//and we need this to mainpulate data from a db
using System.Data;
// needed for gmail connectivity
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Gmail.v1;
//using Google.Apis.Gmail.v1.Data;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;
using System.Text;
using System.Reflection;

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
        //// Gmail API scope and application name
        //static string[] Scopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.MailGoogleCom, GmailService.Scope.GmailModify };
        //static string ApplicationName = "Gmail API .NET OperationHappiness";

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

        //public async Task RetrieveEmails()
        //{ 
         
        //    // variables to store email information
        //    string sender = "";
        //    string date = "";
        //    string subject = "";
        //    string body = "";

        //    try
        //    {
        //        UserCredential credential;

        //        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        //        {
        //            // The file token.json stores the user's access and refresh tokens, and is created
        //            // automatically when the authorization flow completes for the first time.
        //            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
        //                GoogleClientSecrets.Load(stream).Secrets,
        //                Scopes,
        //                "user",
        //                CancellationToken.None,
        //                new FileDataStore(this.GetType().ToString()));
        //        }

        //        // Initializes Gmail -- I'm not sure how to get rid of this or if its even possible without this
        //        var gmailService = new GmailService(new BaseClientService.Initializer()
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = ApplicationName
        //        });

        //        // Request the emails that are in the inbox which will be all of the user emails
        //        var emailListRequest = gmailService.Users.Messages.List("scrumdaddiez@gmail.com");
        //        emailListRequest.LabelIds = "INBOX";
        //        emailListRequest.IncludeSpamTrash = false;

        //        // Get the individual emails
        //        var emailListResponse = await emailListRequest.ExecuteAsync();

        //        // if the email list is not empty and there are messages a for loop gets executed that stores each message
        //        if (emailListResponse != null && emailListResponse.Messages != null)
        //        {
        //            // loop through the emails for who its from, when it was sent, the subject, and the body
        //            foreach (var email in emailListResponse.Messages)
        //            {
        //                var emailInfoRequest = gmailService.Users.Messages.Get("scrumdaddiez@gmail.com", email.Id);
        //                var emailInfoResponse = await emailInfoRequest.ExecuteAsync();

        //                if (emailInfoResponse != null)
        //                {
        //                    foreach (var messageParts in emailInfoResponse.Payload.Headers)
        //                    {
        //                        if (messageParts.Name == "Date")
        //                            date = messageParts.Value;
        //                        else if (messageParts.Name == "From")
        //                            sender = messageParts.Value;
        //                        else if (messageParts.Name == "Subject")
        //                            subject = messageParts.Value;

        //                        // if the date and from values are not null the body is read
        //                        if (date != "" && sender != "")
        //                        {
        //                            if (emailInfoResponse.Payload.Parts == null && emailInfoResponse.Payload.Body != null)
        //                            {
        //                                body = emailInfoResponse.Payload.Body.Data;
        //                            }
        //                            else
        //                            {
        //                                body = getNestedParts(emailInfoResponse.Payload.Parts, "");
        //                            }
        //                            // According to stackexchange, need to replace characters since email bodies are encoded in base64 instead of UTF 8
        //                            string codedBody = body.Replace("-", "+");
        //                            codedBody = codedBody.Replace("_", "/");
        //                            byte[] data = Convert.FromBase64String(codedBody);
        //                            body = Encoding.UTF8.GetString(data);
        //                        }

        //                        // if the 4 strings are not empty, the email is added to the emails database (just for testing, this isnt the permanent solution for now)
        //                        if (sender != null && date != null && subject != null && body != null)
        //                        {
        //                            // grabbing connection string from config file
        //                            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

        //                            // set up our connection object to be ready to use our connection string
        //                            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);

        //                            // instantiating new command
        //                            MySqlCommand addEmail = new MySqlCommand("INSERT INTO Emails(Sender,Date,Subject,Body) VALUES(@sender,@date,@subject,@body)", sqlConnection);

        //                            // assigning values
        //                            addEmail.Parameters.AddWithValue("@sender", sender);
        //                            addEmail.Parameters.AddWithValue("@date", date);
        //                            addEmail.Parameters.AddWithValue("@subject", subject);
        //                            addEmail.Parameters.AddWithValue("@body", body);

        //                            // running command
        //                            addEmail.ExecuteNonQuery();

        //                            sqlConnection.Close();
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception)
        //    {

        //    }

        //}

        //public static string getNestedParts(IList<MessagePart> part, string current)
        //{
        //    string str = current;
        //    if (part == null)
        //    {
        //        return str;
        //    }
        //    else
        //    {
        //        foreach (var parts in part)
        //        {
        //            if(parts.Parts == null)
        //            {
        //                if (parts.Body != null && parts.Body.Data != null)
        //                {
        //                    str += parts.Body.Data;
        //                }
        //            }
        //            else
        //            {
        //                return getNestedParts(parts.Parts, str);
        //            }
        //        }

        //        return str;
        //    }
        //}

        [WebMethod]
        public string RetrieveEmails()
        {
            // string variables declared to store parts of the message
            string sender;
            string date;
            string subject;
            string body;

            try
            {
                // Outlook variables instantiated and assigned to outlook folders
                Outlook.Application outlookApp = new Outlook.Application();
                Outlook.NameSpace outlookNS = (Outlook.NameSpace)outlookApp.GetNamespace("MAPI");
                outlookNS.Logon(Missing.Value, Missing.Value, false, true);
                Outlook.MAPIFolder inbox = outlookNS.Folders["scrumdaddiez@gmail.com"].Folders["Inbox"];

                // string variables created to store sender, subject and body
                Outlook.Items messages = inbox.Items.Restrict("[UnRead]=true");

                // a foreach loop goes through all of the messages, adds them to the database, and then marks them as read
                foreach (Outlook.MailItem mail in messages)
                {
                    mail.UnRead = false;

                    // Each part of the message is stored in a variable
                    sender = mail.SenderEmailAddress;
                    date = mail.SentOn.ToString();
                    subject = mail.Subject;
                    body = mail.Body;

                    // if the 4 strings are not empty, the email is added to the emails database(just for testing, this isnt the permanent solution for now)
                    if (sender != null && date != null && subject != null && body != null)
                    {
                        // grabbing connection string from config file
                        string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

                        // set up our connection object to be ready to use our connection string
                        MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
                        sqlConnection.Open();

                        // instantiating new command
                        MySqlCommand addEmail = new MySqlCommand("INSERT INTO Emails(Sender,Date,Subject,Body) VALUES(@sender,@date,@subject,@body)", sqlConnection);

                        // assigning values
                        addEmail.Parameters.AddWithValue("@sender", sender);
                        addEmail.Parameters.AddWithValue("@date", DateTime.Parse(date));
                        addEmail.Parameters.AddWithValue("@subject", subject);
                        addEmail.Parameters.AddWithValue("@body", body);

                        // running command
                        addEmail.ExecuteNonQuery();

                        sqlConnection.Close();
                    }
                }

                return "Worked";
            }
            catch
            {
                return "Failed";
            }
        }

        [WebMethod]
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ViewAccounts(string admin)
        {
            // variable to store HTML string to be appended using JS
            string html = "";
            string email = "";
            int score = 50;

            // mostly the same code as VerifyCredentials
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            // instantiating query
            string sqlSelect = $"Select * FROM employees";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);
            sqlConnection.Open();

            MySqlDataReader reader = sqlCommand.ExecuteReader();

            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        email = (string)reader["Email"];
                        score = (int)reader["Happiness"];

                        int num = 1;
                        html += "<tr id='row" + num +"><td class='email'>" + email + "</td><td class='score'>" + score + "</td></tr>";
                        num++;
                    }

                    return html;
                }

                else
                {
                    html = null;
                    return html;
                }
            }

            finally
            {
                reader.Close();
                sqlConnection.Close();
            }
        }

    }
}

