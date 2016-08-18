using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json;
using librets;

namespace RetsVerification
{
    //RETS-Response Code 
    public enum RetsSearchReplyCode
    {
        Success = 0,
        NoResults = 20201,
        Error = 20000
    }

    class Program
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var fileName = Path.Combine(directory.FullName, "RetsCredentials.json");
            var retsUsers = DeserializeRetsUsers(fileName);
            var loginConfirmation = GetLoginConfirmation(retsUsers);

            try
            {
                foreach (var retsUser in retsUsers)
                {
                    var session = new RetsSession(retsUser.RetsLoginURL);

                    session.SetRetsVersion(RetsVersion.RETS_1_5);

                    if (retsUser.RetsVersion == 1.7)
                    {
                        session.SetRetsVersion(RetsVersion.RETS_1_7);
                    }
                    else if (retsUser.RetsVersion == 1.72)
                    {
                        session.SetRetsVersion(RetsVersion.RETS_1_7_2);
                    }
                    else if (retsUser.RetsVersion == 1.8)
                    {
                        session.SetRetsVersion(RetsVersion.RETS_1_8);
                    }

                    if (retsUser.RetsUserAgent == null)
                    {
                        session.SetUserAgent("BoomTown/1.1");
                    }
                    else
                    {
                        session.SetUserAgent(retsUser.RetsUserAgent);
                    }

                    if (!String.IsNullOrEmpty(retsUser.RetsUserAgentPassword))
                        session.SetUserAgentPassword(retsUser.RetsUserAgentPassword);

                    bool loginResult = session.Login(retsUser.RetsUserID, retsUser.RetsPassword);
                    if (!loginResult)
                    {
                        //Console.WriteLine(session.GetReplyCode());
                        Console.WriteLine("\nLogin to {0}'s RETS Failed at " + DateTime.Now, retsUser.RetsAssociatedUser);
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("\nLogin to {0}'s RETS Succeeded at " + DateTime.Now, retsUser.RetsAssociatedUser);
                        Console.ReadLine();

                        session.SetDefaultEncoding(EncodingType.RETS_XML_DEFAULT_ENCODING);

                        NameValueCollection queryVals = System.Web.HttpUtility.ParseQueryString(retsUser.QueryString);
                        string query = queryVals["Query"];

                        using (SearchRequest search = session.CreateSearchRequest(queryVals["SearchType"], queryVals["Class"], query))
                        {
                            search.SetQueryType(SearchRequest.QueryType.DMQL2);
                            search.SetStandardNames(false);
                            search.SetLimit(10);
                            search.SetFormatType(SearchRequest.FormatType.COMPACT_DECODED);

                            if (queryVals["Select"] != null)
                                search.SetSelect(queryVals["Select"]);
                            SearchResultSet results = session.Search(search);

                            int replyCode = results.GetReplyCode();
                            //we got records!
                            if (replyCode == (int)RetsSearchReplyCode.Success)
                            {
                                Console.WriteLine("Record count: " + results.GetCount());
                                Console.ReadLine();
                                IEnumerable columns = null;
                                while (results.HasNext())
                                {
                                    if (columns == null)
                                    {
                                        columns = results.GetColumns();
                                    }
                                    foreach (string column in columns)
                                    {
                                        log.Info(retsUser.TenantID + " " + retsUser.RetsAssociatedUser + " " + column + ": " + results.GetString(column));
                                    }
                                    Console.WriteLine();
                                }
                                LogoutResponse logout = session.Logout();
                                Console.WriteLine("Logout message: " + logout.GetLogoutMessage());
                                Console.WriteLine("Connect time: " + logout.GetConnectTime());
                                // no records found
                            }
                            else if (replyCode == (int)RetsSearchReplyCode.NoResults)
                            {
                                Console.WriteLine("{0} - This query returned no results", RetsSearchReplyCode.NoResults);
                                Console.ReadLine();
                                // other rets errors
                            }
                            else if (replyCode > (int)RetsSearchReplyCode.Error)
                            {
                                throw new Exception(String.Format("{0}: {1} {2}",
                                    replyCode,
                                    results.GetReplyText(),
                                    search.GetQueryString()));
                                Console.WriteLine("{0} - Bummer there was a search error.", RetsSearchReplyCode.Error);
                                Console.ReadLine();
                            }
                        }
                        Console.ReadLine();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        public static List<RetsUser> DeserializeRetsUsers(string fileName)
        {
            var retsUsers = new List<RetsUser>();
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(fileName))
            using (var jsonReader = new JsonTextReader(reader))
            {
                retsUsers = serializer.Deserialize<List<RetsUser>>(jsonReader);
            }


            return retsUsers;
        }
        
        public static List<RetsUser> GetLoginConfirmation(List<RetsUser> retsUsers)
        {
            var loginConfirmation = new List<RetsUser>();
            int counter = 0;
            foreach (var retsUser in retsUsers)
            {
                loginConfirmation.Add(retsUser);
                counter++;
                if (counter == 3)
                    break;
            }
            return loginConfirmation;
        }

    }
}