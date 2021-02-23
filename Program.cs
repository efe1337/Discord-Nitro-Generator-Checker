using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nitro
{
    class Program
    {
        static Random random = new Random();
        public static string generategift(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        } // StackOverFlow
        static void Main(string[] args)
        {
            Console.Write("Enter delay(ms) (1000 = 1 sec):");
            int delay = Convert.ToInt32(Console.ReadLine());
            Console.Write("How many try:");
            List<string> gifts = new List<string>();
            int asd = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i<asd;i++)
            {
                gifts.Add(generategift(16));
            }

            foreach(string gift in gifts)
            {

                try
                {
                    //string[] proxys = File.ReadAllLines("proxy.txt");
                    Random proxySelect = new Random();
                    WebClient wc = new WebClient();
                    //wc.Proxy = new WebProxy(proxys[proxySelect.Next(0,proxys.Length-1)]);
                    wc.Proxy = null;
                    string data = wc.DownloadString("https://discordapp.com/api/v6/entitlements/gift-codes/" + gift);
                    dynamic jo = JObject.Parse(data);
                    Console.WriteLine($"Working code: {gift}", Console.ForegroundColor = ConsoleColor.Green);
                }
                catch (WebException esx)
                {
                    if (esx.Response != null)
                    {
                        string esxK = new StreamReader(esx.Response.GetResponseStream()).ReadToEnd();
                        var res = JsonConvert.DeserializeObject<dynamic>(esxK);
                        if (esxK.ToLower().Contains("rate"))
                        {
                            Console.WriteLine($"Rate Limited: {gift}", Console.ForegroundColor = ConsoleColor.Yellow);
                            Console.WriteLine("Change ur ip or use proxy", Console.ForegroundColor = ConsoleColor.Yellow);
                        }
                        else
                        {
                            Console.WriteLine($"Not working code: {gift} | {res.message}", Console.ForegroundColor = ConsoleColor.Magenta);
                        }
                    }
                }
                System.Threading.Thread.Sleep(delay);
            };  
            Console.WriteLine("\nDone");
            Console.ReadLine();
            }
    }
}
