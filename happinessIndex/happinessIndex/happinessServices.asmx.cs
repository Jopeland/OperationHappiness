using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Text.RegularExpressions;
using java.util;
using java.io;
using edu.stanford.nlp.pipeline;
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
using System.Web.Script.Services;
using System.Web.Script.Serialization;

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
            string notice = "";

            // string variables declared to store parts of the message
            string sender;
            string date;
            string subject;
            string body;

            // list of mailitems
            List<Outlook.MailItem> emails = new List<Outlook.MailItem>();

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
                    emails.Add(mail);
                }

                notice += $"Email count: {emails.Count}. ";

                for (int i = 0; i < emails.Count; i++)
                {
                    emails[i].UnRead = false;

                    // Each part of the message is stored in a variable
                    sender = emails[i].SenderEmailAddress;
                    date = emails[i].SentOn.ToString();
                    subject = emails[i].Subject;
                    body = emails[i].Body;

                    notice += $"Email collected: {sender}, {date}, {subject}, {body}. ";

                    // if the 4 strings are not empty, the email is added to the emails database(just for testing, this isnt the permanent solution for now)
                    if (sender != null && date != null && subject != null && body != null)
                    {

                        notice += $"Email matches parameters. ";
                        // passing email body into sentiment analysis
                        int sentiment = SentimentAnalysis(body);

                        notice += $"Sentmient Analysis run {sentiment}.  ";

                        if (sentiment >= 6)
                            sentiment = 2;
                        else if (sentiment >= 1)
                            sentiment = 1;
                        else if (sentiment == 0)
                            sentiment = 0;
                        else if (sentiment <= -6)
                            sentiment = -2;
                        else
                            sentiment = -1;

                        notice += $"Sentiment level altered: {sentiment}. ";

                        // grabbing connection string from config file
                        string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

                        string sqlSelect = $"SELECT * FROM employees where Email = '{sender}'";

                        // set up our connection object to be ready to use our connection string
                        MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
                        MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);
                        sqlConnection.Open();
                        MySqlDataReader reader = sqlCommand.ExecuteReader();

                        notice += $"Reader initiated. ";


                        int score = 0;
                        try
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    notice += $"Sentiment = {sentiment} and Current = {(int)reader["Happiness"]}. ";
                                    score = sentiment + (int)reader["Happiness"];
                                    notice += $"Score analyzed = {score}. ";
                                }
                            }
                            else
                            {
                                score = 0;
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }


                        // instantiating new command
                        MySqlCommand addScore = new MySqlCommand("INSERT INTO Scores(UserEmail,Date,Score) VALUES(@sender,@date,@score)", sqlConnection);

                        // assigning values
                        addScore.Parameters.AddWithValue("@sender", sender);
                        addScore.Parameters.AddWithValue("@date", DateTime.Parse(date));
                        addScore.Parameters.AddWithValue("@score", score);

                        // running command
                        addScore.ExecuteNonQuery();

                        notice += "Inserted into scores DB.";

                        sqlConnection.Close();
                    }

                }
                return notice;
            }
            catch
            {
                return notice;
            }
        }

        [WebMethod]
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ViewAccounts(string admin)
        {
            // variable to store HTML string to be appended using JS
            string html = "";
            string email = "";
            string department = "";
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
                        department = (string)reader["Department"];

                        int num = 1;
                        html += "<tr><td class='email'>" + email + "</td><td class='score'>" + score + "</td><td class='dept'>" + department + "</td></tr>";
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

        [WebMethod]
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ViewFeedback(string type, string choice = "")
        {
            // variable to store HTML string to be appended using JS
            string html = "";
            string csv = "User ID, Department, Score Change, Approved processes, Recommended Changes, Fixes Recommended, Communication, Additional Comments, Date";

            string email = "";
            string department = "";
            int scoreChange = 0;
            string approved = "";
            string changes = "";
            string fixes = "";
            string communication = "";
            string comments = "";
            string date = "";

            if (type == "load")
            {
                // mostly the same code as VerifyCredentials
                string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

                // instantiating query
                string sqlSelect = $"Select * FROM responses";

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
                            email = (string)reader["userEmail"];
                            department = (string)reader["Department"];
                            scoreChange = (int)reader["scoreChange"];
                            approved = (string)reader["mRecs"];
                            changes = (string)reader["mChange"];
                            fixes = (string)reader["dFixes"];
                            communication = (string)reader["hComms"];
                            comments = (string)reader["comments"];
                            date = (string)reader["mRecs"];


                            int num = 1;
                            html += "<tr><td class='email'>" + email + "</td><td class='department'>" + department + "</td><td class='scoreChange'>" + scoreChange + "</td><td class='approved'>" + approved + "</td><td class='changes'>" + changes + "</td><td class='fixes'>" + fixes + "</td><td class='communication'>" + communication + "</td><td class='comments'>" + comments + "</td><td class='date'>" + date + "</td></tr>";
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
            else
            {
                List<string> UIDs = new List<string>();
                List<string> emails = new List<string>();

                // mostly the same code as VerifyCredentials
                string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

                // instantiating query
                string sqlSelect = $"Select * FROM employees WHERE Department = '{choice}'";

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
                            string UID = (string)reader["UUID"];
                            email = (string)reader["Email"];

                            string tmpString = UID + "," + email;

                            emails.Add(email);
                            UIDs.Add(tmpString);
                        }

                        sqlConnection.Close();
                        reader.Close();

                        sqlSelect = $"Select * FROM responses WHERE ";
                        for (int i = 0; i<emails.Count; i++)
                        {
                            if (i == 0)
                                sqlSelect += $"userEmail = '{emails[i]}' ";
                            else
                                sqlSelect += $"OR userEmail = '{emails[i]}' ";
                        }

                        sqlConnection = new MySqlConnection(sqlConnectString);
                        sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);
                        sqlConnection.Open();

                        reader = sqlCommand.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string UID = "";
                                email = (string)reader["userEmail"];

                                for (int i = 0; i < UIDs.Count; i++)
                                {
                                    if (UIDs[i].Contains(email))
                                    {
                                        List<string> tmpList = UIDs[i].Split(',').ToList<string>();
                                        UID = tmpList[0];
                                    }
                                }
                                department = (string)reader["Department"];
                                scoreChange = (int)reader["scoreChange"];
                                approved = (string)reader["mRecs"];
                                changes = (string)reader["mChange"];
                                fixes = (string)reader["dFixes"];
                                communication = (string)reader["hComms"];
                                comments = (string)reader["comments"];
                                DateTime dateTime = (DateTime)reader["date"];
                                date = dateTime.ToString();

                                string myChar = ",";
                                if (approved.Contains(','))
                                    approved = approved.Replace(myChar, "");
                                if (changes.Contains(','))
                                    changes = changes.Replace(myChar, "");
                                if (fixes.Contains(','))
                                    fixes = fixes.Replace(myChar, "");
                                if (communication.Contains(','))
                                    communication = communication.Replace(myChar, "");
                                if (comments.Contains(','))
                                    comments = comments.Replace(myChar, "");


                                csv += $"\n={UID},{department},{scoreChange},{approved},{changes},{fixes},{communication},{comments},{date}";
                            }

                            return csv;
                        }

                        else
                        {
                            return csv;
                        }
                    }

                    else
                    {
                        return csv;
                    }
                }

                finally
                {
                    reader.Close();
                    sqlConnection.Close();
                }
            }
            
        }

        [WebMethod]
        public void SendFeedbackEmail()
        {
            // Establish Outlook connection
            // Outlook variables instantiated and assigned to outlook folders
            Outlook.Application outlookApp = new Outlook.Application();
            Outlook.NameSpace outlookNS = (Outlook.NameSpace)outlookApp.GetNamespace("MAPI");
            outlookNS.Logon(Missing.Value, Missing.Value, false, true);

            // strings to contain the parts of the message
            string subject = "Feedback Survey Request";
            string body = "Hi.  We would like to hear about how work has been for you recently. Please click this link to take the survey: http://www.usaa.com";

            // integer for score change
            int change;

            // declaring lists to store users
            List<string> emailList = new List<string>();
            List<string> feedbackList = new List<string>();

            // Same SQL stuff as everything else with a select
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            // instantiating query that takes all of the individual emails in the scores table and puts them into the list
            string sqlSelect = $"Select DISTINCT UserEmail FROM scores";

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
                        emailList.Add((string)reader["UserEmail"]);
                    }

                    // close the first reader
                    reader.Close();
                }

                // if the list has values another sql statement to add each user with a large score difference to the feedback email list
                if (emailList.Count > 0)
                {
                    // A for loop loops through the list and passes it as an argument for a sql select
                    for (int i = 0; i < emailList.Count; i++)
                    {
                        change = 0;

                        // SQL command takes only the top 2 results that the database contains
                        MySqlCommand feedbackUsers = new MySqlCommand($"Select ScoreID, Score FROM scores WHERE UserEmail = '{emailList[i]}' ORDER BY ScoreID DESC LIMIT 2", sqlConnection);
                        reader = feedbackUsers.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // if the change variable is 0, change is set equal to score
                                if (change == 0)
                                {
                                    change = reader.GetInt32(reader.GetOrdinal("Score"));
                                }
                                else // else it finds the difference between the first score and the second
                                {
                                    change = change - reader.GetInt32(reader.GetOrdinal("Score"));
                                }
                            }

                            // if the change is greater than +5 or less than -5, they are added to the feedbackList
                            if (change > 5 || change < -5)
                            {
                                feedbackList.Add(emailList[i]);
                            }
                        }

                        // reader closed
                        reader.Close();
                    }
                }
            }

            finally
            {
                sqlConnection.Close();
            }

            // Finding the right email account ("scrumdaddiez@gmail.com") to use to send the email
            Outlook.Accounts accounts = outlookApp.Session.Accounts;

            // foreach loops through and looks for scrumdaddiez@gmail.com and sets it to the sender account
            foreach (Outlook.Account account in accounts)
            {
                if (account.SmtpAddress == "scrumdaddiez@gmail.com")
                {
                    // for every email address in the feedbackList, an email is sent
                    for (int i = 0; i < feedbackList.Count; i++)
                    {
                        // New Mail Item is created and sent
                        Outlook.MailItem email = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
                        email.Subject = subject;
                        email.HTMLBody = body;
                        email.To = feedbackList[i];
                        email.Importance = Outlook.OlImportance.olImportanceHigh;
                        email.SendUsingAccount = account;
                        email.Send();
                    }
                }
            }
        }

        [WebMethod]
        public string SearchEmployees(string input, int minScore, int maxScore, string order)
        {
            // variable to store HTML string to be appended using JS
            string html = "";
            string email = "";
            string department = "";
            int score = 0;
            string sqlSelect;

            // mostly the same code as VerifyCredentials
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            // two different queries depending on if there is an input searched for or not (value of #)
            if (input != "#")
            {
                sqlSelect = $"Select * FROM employees WHERE (Email LIKE'%{input}%' OR Department='{input}') AND Happiness BETWEEN {minScore} AND {maxScore} ORDER BY {order}";
            }
            else
            {
                sqlSelect = $"Select * FROM employees WHERE Happiness BETWEEN {minScore} AND {maxScore} ORDER BY {order}";
            }

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
                        department = (string)reader["Department"];

                        int num = 1;
                        html += "<tr><td class='email'>" + email + "</td><td class='score'>" + score + "</td><td class='dept'>" + department + "</td></tr>";
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDepartmentAverage()
        {
            // list to store departments
            List<string> departments = new List<string>();

            // SQL query to get all of the unique departments stored in the database
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            string sqlSelect = $"Select DISTINCT Department FROM employees ORDER BY Department DESC";

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
                        departments.Add((string)reader["Department"]);
                    }
                }
            }

            finally
            {
                reader.Close();
            }

            // scores list declared
            List<int> averageScores = new List<int>();

            // a for loop executes multiple queries to get the average score of each department
            for (int i = 0; i < departments.Count; i++)
            {
                int total = 0;
                int count = 0;

                MySqlCommand getAverages = new MySqlCommand($"Select Happiness FROM employees WHERE department = '{departments[i]}'", sqlConnection);
                reader = getAverages.ExecuteReader();

                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            total = total + (int)reader["Happiness"];
                            count++;
                        }
                    }
                }

                finally
                {
                    reader.Close();

                    // averaged score is added to scores list
                    averageScores.Add(total / count);
                }
            }

            sqlConnection.Close();

            // list for all values values 
            List<string[,]> values = new List<string[,]>();

            // for loop adds everything into the array
            for (int i = 0; i < departments.Count; i++)
            {
                // array for individual values
                string[,] iValues = new string[1, 2];

                iValues[0, 0] = departments[i];
                iValues[0, 1] = averageScores[i].ToString();

                values.Add(iValues);
            }

            //JSON serializer
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(values);

            // json is returned
            return json;

        }

        [WebMethod]
        public int SentimentAnalysis(string entry)
        {
            // Path to the folder with models extracted from `stanford-corenlp-3.7.0-models.jar`
            var jarRoot = @"C:\Program Files (x86)\stanford-corenlp-full-2018-02-27\stanford-corenlp-3.9.1-models";

            // Text for processing
            var text = entry;

            // Annotation pipeline configuration
            var props = new Properties();
            props.setProperty("annotators", "tokenize,ssplit,pos,parse,sentiment");
            props.setProperty("ner.useSUTime", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            var pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            var stream = new ByteArrayOutputStream();

            // Result - Pretty Print
            using (stream)
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                System.Console.WriteLine(stream.toString());
                stream.close();
            }

            Regex total = new Regex(@"(sentiment:)");
            Regex pos = new Regex(@"(sentiment: Positive)");
            Regex neg = new Regex(@"(sentiment: Negative)");
            Regex neu = new Regex(@"(sentiment: Neutral)");

            string final = stream.ToString();

            MatchCollection totalMatch = total.Matches(final);
            MatchCollection posMatch = pos.Matches(final);
            MatchCollection negMatch = neg.Matches(final);
            MatchCollection neuMatch = neu.Matches(final);

            int sentiment = posMatch.Count + (-1 * negMatch.Count);


            return sentiment;
        }

        [WebMethod]
        public int GetOverallHappiness()
        {
            // Query gets the average of all the scores in the employees table and returns an integer so its rounded and nice
            int average = 0;

            // SQL connections and query
            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;

            string sqlSelect = $"Select AVG(Happiness) FROM employees";

            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);
            sqlConnection.Open();
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    average = Convert.ToInt32(reader["AVG(Happiness)"]);
                }
            }

            // close connection
            reader.Close();
            sqlConnection.Close();

            return average;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetHappinessOverTime()
        {
            // List of ArrayLists is taken to be converted to a json string is created
            List<System.Collections.ArrayList> values = new List<System.Collections.ArrayList>();

            string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            MySqlConnection sqlConnection = new MySqlConnection(sqlConnectString);

            // A query similar to that one in GetDepartmentHealth gets each department to serve as the header row of the datatable
            // First a list is declared to take these values, starting with date
            System.Collections.ArrayList header = new System.Collections.ArrayList();
            header.Add("Date");

            string selectDepartments = $"Select DISTINCT Department FROM employees ORDER BY Department DESC";
            MySqlCommand getDepartments = new MySqlCommand(selectDepartments, sqlConnection);
            sqlConnection.Open();
            MySqlDataReader deptReader = getDepartments.ExecuteReader();

            try
            {
                if (deptReader.HasRows)
                {
                    while (deptReader.Read())
                    {
                        // Each department is added to the list of departments for the header
                        header.Add((string)deptReader["Department"]);
                    }

                    // Lastly, All is added to values as the final header
                    header.Add("All");

                    // the header is added to values
                    values.Add(header);
                }
            }
            finally
            {
                deptReader.Close();
                sqlConnection.Close();
            }

            // A query will loop through and perform 14 queries -- each of the last 14 days
            for (int i = 14; i > 0; i--)
            {
                string sqlSelect = $"SELECT e.Department, ROUND(avg(s.Score)) AS avgscore FROM Employees e, Scores s Where e.Email = s.UserEmail AND DATE(s.Date) = DATE_SUB(CURRENT_DATE(), INTERVAL {i} DAY) GROUP BY e.Department Order BY e.Department DESC";
                MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);
                sqlConnection.Open();
                MySqlDataReader reader = sqlCommand.ExecuteReader();

                // a list of strings is declared to take in all of the values
                System.Collections.ArrayList queryOutput = new System.Collections.ArrayList();

                // The first value added into the list is a string of the date from however many days ago is being take
                queryOutput.Add(DateTime.Today.AddDays(-i).Date.ToString("MM-dd-yy"));

                int total = 0; // takes running total for overall average

                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // the average scores are added to the queryOutput list
                            queryOutput.Add(reader["avgscore"]);

                            // running total is kept
                            total = total + Convert.ToInt32(reader["avgscore"]);
                        }

                        // total is added to the queryOutput list as a string
                        queryOutput.Add((total / (queryOutput.Count - 1)));

                        // queryOutput is put into values
                        values.Add(queryOutput);
                    }
                }

                finally
                {
                    reader.Close();
                    sqlConnection.Close();
                }
            }

            // Next a list of the values for today are queried for and added to their own list which is added to values
            string getToday = $"SELECT e.Department, ROUND(avg(s.Score)) AS avgscore FROM Employees e, Scores s Where e.Email = s.UserEmail AND DATE(s.Date) = CURRENT_DATE() GROUP BY e.Department Order BY e.Department DESC";
            MySqlCommand todayAverage = new MySqlCommand(getToday, sqlConnection);
            sqlConnection.Open();
            MySqlDataReader todayReader = todayAverage.ExecuteReader();

            // List for today's values, first the date as usual
            System.Collections.ArrayList today = new System.Collections.ArrayList();
            today.Add(DateTime.Today.Date.ToString("MM-dd-yy"));
            int todayTotal = 0;

            try
            {
                if (todayReader.HasRows)
                {
                    while (todayReader.Read())
                    {
                        // the average scores are added to the queryOutput list
                        today.Add(todayReader["avgscore"]);

                        // running total is kept
                        todayTotal = todayTotal + Convert.ToInt32(todayReader["avgscore"]);
                    }

                    // total is added to the queryOutput list as a string
                    today.Add((todayTotal / (today.Count - 1)));

                    // queryOutput is put into values
                    values.Add(today);
                }
            }
            finally
            {
                todayReader.Close();
                sqlConnection.Close();
            }

            // Values is converted to a json string
            //JSON serializer
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(values);

            // json is returned
            return json;
        }
    }
}