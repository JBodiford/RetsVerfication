using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using librets;




namespace RetsVerification
{

    public class RootObject
    {
        public int TenantID { get; set; }
        public string RetsAssociatedUser { get; set; }
        public string RetsLoginURL { get; set; }
        public string RetsUserID { get; set; }
        public string RetsPassword { get; set; }
        public double RetsVersion { get; set; }

    }
     
    class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText(@"c:\Users\jbodiford\Documents\retsLoginProject\C#RetsVerification\RetsCredentials.json");
            var users = JsonConvert.DeserializeObject<List<RootObject>>(json);

            foreach (var item in users)
            {
                Console.WriteLine(users.ToString());
                Console.ReadLine();
            }


       }

     }


}
