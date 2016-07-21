using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
        public double RetsVersion { get; set; }

    }

    public class ListingKeys
    {
        public long ListingID;
        public string ListingMLS;
        public string RetsKey;

        public ListingKeys(long listingID, string listingMLS, string retsKey)
        {
            ListingID = listingID;
            ListingMLS = listingMLS;
            RetsKey = retsKey;
        }
    }

    //RETS-Response Code 
    public enum RetsSearchReplyCode
    {
        Success = 0,
        NoResults = 20201,
        Error = 20000
    }

    public class RetsHelper : IDisposable
    {
        private RetsSession session;
        private int totalProcessed = 0;
        private EncodingType encodingType = EncodingType.RETS_XML_DEFAULT_ENCODING;
        private bool logging = false;

        //RETS session connection
        public RetsSession Session
        {
            get { return session; }
            set { session = value; }
        }

        public int TotalProcessed
        {
            get { return totalProcessed; }
            set { totalProcessed = value; }
        }

        public EncodingType EncodingType
        {
            get { return encodingType; }
            set { encodingType = value; }
        }

        public bool Logging
        {
            get { return logging; }
            set { logging = value; }
        }

        
    }

     
    class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText(@"c:\Users\jbodiford\Documents\retsLoginProject\C#RetsVerification\RetsCredentials.json");
            var users = JsonConvert.DeserializeObject<List<RootObject>>(json);
            foreach (var item in users)
            {
                item.RetsLoginURL = new RetsLoginURL:


                public RetsHelper(RetsLoginURL, RetsUserID, RetsPassword)
                    {
                        this.Session = new RetsSession(RetsLoginURL);
                        this.Session.LoggerDelegate = new RetsHttpLogger.Delegate(logRETS)
;
                        try
                        {
                            bool loginResult = this.Session.Login(RetsUserID,RetsPassword);

                            if (!loginResult)
                            {
                                Console.WriteLine("\nLogin to RETS Failed at " + DateTime.Now);
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("\nLogin to RETS Succeeded at " + DateTime.Now);
                                Console.ReadLine();
                            }
                            //switch (users[x].RetsVersion)
                            //{
                            //    case "1.5":
                            //        this.Session.SetRetsVersion(RetsVersion.RETS_1_5);
                            //        break;
                            //    case "1.7":
                            //        this.Session.SetRetsVersion(RetsVersion.RETS_1_7);
                            //        break;
                            //    case "1.72":
                            //        this.Session.SetRetsVersion(RetsVersion.RETS_1_7_2);
                            //        break;
                            //    case "1.8":
                            //        this.Session.SetRetsVersion(RetsVersion.RETS_1_8);
                            //        break;                            
                            //}
                        }
                        catch (Exception ex)
                        {
                           Console.WriteLine(ex.Message);
                        }
                        
                    }
                //Console.WriteLine("{0} {1} {2}\n", item.TenantID, item.BoardID, item.RetsAssociatedUser);
                //Console.ReadLine();
            }
        }
        
                



     }


}
