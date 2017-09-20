using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

namespace TasksManager.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            String port = "9000";
            String baseAddress = "http://localhost:" + port + "/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(baseAddress + "api/commands/types").Result;
                //response.Headers.Remove("Server");
                //Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync
                    ().Result);
                Console.ReadLine();
            }
        }
    }
}
