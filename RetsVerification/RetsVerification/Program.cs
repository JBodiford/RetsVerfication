﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using librets;




namespace RetsVerification
{
    //json object parsing
    public class RootObject
    {
        public int TenantID { get; set; }
        public int BoardID {get; set;}
        public string RetsAssociatedUser { get; set; }
        public string RetsLoginURL { get; set; }
        public string RetsUserID { get; set; }
        public string RetsPassword { get; set; }
        public string RetsUserAgent { get; set; }
        public string RetsUserAgentPassword { get; set; }
        public double RetsVersion { get; set; }
        public string QueryString { get; set; }

    }

    //RETS-Response Code 
    public enum RetsSearchReplyCode
    {
        Success = 0,
        NoResults = 20201,
        Error = 20000
    }

    
     
    class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText(@"c:\Users\jbodiford\Documents\retsLoginProject\C#RetsVerification\RetsCredentials.json");
            var users = JsonConvert.DeserializeObject<List<RootObject>>(json);
           try
           {
                foreach (var user in users)
                {
                   var session = new RetsSession(user.RetsLoginURL);
                      
                   session.SetRetsVersion(RetsVersion.RETS_1_5);
                   
                       if (user.RetsVersion == 1.7)
                       {
                           session.SetRetsVersion(RetsVersion.RETS_1_7);
                       } else if(user.RetsVersion == 1.72)
                       {
                          session.SetRetsVersion(RetsVersion.RETS_1_7_2);
                       } else if(user.RetsVersion == 1.8)
                       {
                          session.SetRetsVersion(RetsVersion.RETS_1_8);
                       }
                       
                    if (user.RetsUserAgent == null)
                       {
                           session.SetUserAgent("BoomTown/1.1");
                       }
                       else
                       {
                           session.SetUserAgent(user.RetsUserAgent);
                       }

                   if (!String.IsNullOrEmpty(user.RetsUserAgentPassword))
                       session.SetUserAgentPassword(user.RetsUserAgentPassword);
                   
                   bool loginResult = session.Login(user.RetsUserID, user.RetsPassword);
                       if (!loginResult)
                       {
                          //Console.WriteLine(session.GetReplyCode());
                           Console.WriteLine("\nLogin to {0}'s RETS Failed at " + DateTime.Now, user.RetsAssociatedUser);
                           Console.ReadLine();
                       }
                       else
                       {
                           Console.WriteLine("\nLogin to {0}'s RETS Succeeded at " + DateTime.Now, user.RetsAssociatedUser);
                           Console.ReadLine();
                           
                           NameValueCollection queryVals = System.Web.HttpUtility.ParseQueryString(user.QueryString);
                           string query = queryVals["Query"];
                           
                           using (SearchRequest search = session.CreateSearchRequest(queryVals["SearchType"], queryVals["Class"], query))
                           {
                               search.SetQueryType(SearchRequest.QueryType.DMQL2);
                               search.SetStandardNames(false);
                               search.SetLimit(10);

                               if (queryVals["Select"] != null)
                                   search.SetSelect(queryVals["Select"]);
                               SearchResultSet results = session.Search(search);
                               int replyCode = results.GetReplyCode();
                               // no records found
                               if (replyCode == (int)RetsSearchReplyCode.NoResults)
                                    Console.WriteLine(results);
                                    Console.ReadLine();

                               // other rets errors
                               if (replyCode > (int)RetsSearchReplyCode.Error)
                                   throw new Exception(String.Format("{0}: {1} {2}",
                                       replyCode,
                                       results.GetReplyText(),
                                       search.GetQueryString()));

                                   Console.WriteLine(results);
                                   Console.ReadLine();
                               
                           }
                           Console.ReadLine();
                       }
                     
                   

                   session.Logout();
                }
           } 
           catch (Exception ex)
	        {
	            Console.WriteLine(ex.Message);
	        }
        }
     }


}
