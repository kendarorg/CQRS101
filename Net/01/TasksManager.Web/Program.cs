﻿using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(baseAddress + "api/Values/TestGet1").Result;
                response.Headers.Remove("Server");
                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync
                    ().Result);
                Console.ReadLine();
            }
        }
    }
}