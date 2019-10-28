using Cruise;
using Cruise.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;

namespace Cqrs_04.Test
{
    [Binding]
    public class IntegrationTestSteps
    {
        private Process _process;

        public string SendRequest(string url,string json=null)
        {
            string html = string.Empty;
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = json == null ? "GET" : "POST";
            request.ContentType = "application/json";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            if (json != null)
            {
                var dataBytes = Encoding.UTF8.GetBytes(json);
                request.ContentLength = dataBytes.Length;
                using (Stream requestBody = request.GetRequestStream())
                {
                    requestBody.Write(dataBytes, 0, dataBytes.Length);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        [When(@"I add the cruise named '(.*)'")]
        public void WhenIAddTheCruiseNamed(string cruiseName)
        {
            string myJson = JsonConvert.SerializeObject(new CreateCruise(cruiseName));
            SendRequest("http://localhost:50922/api/commands/create/cruise", myJson);
            Thread.Sleep(1000);
        }
        
        [Then(@"i can get the cruise named '(.*)' from projection")]
        public void ThenICanGetTheCruiseNamedFromProjection(string name)
        {
            var allCruises = SendRequest("http://localhost:50922/api/projections/cruises");
            var all = JsonConvert.DeserializeObject<List<CruiseProjectionEntity>>(allCruises);
            
            Assert.AreEqual(1,all.Count(a => a.Name == name));
        }

        [When(@"i assign a '(.*)' class room with number '(.*)' to the cruise named '(.*)'")]
        public void WhenIAssignAClassRoomWithNumberToTheCruiseNamed(int cl, int num, string cruise)
        {
            var allCruises = SendRequest("http://localhost:50922/api/projections/cruises");
            var cruiseObj = JsonConvert.DeserializeObject<List<CruiseProjectionEntity>>(allCruises).First(a => a.Name == cruise);

            string myJson = JsonConvert.SerializeObject(new CreateRoom(cruiseObj.Id,num,1,cl));
            SendRequest("http://localhost:50922/api/commands/create/room", myJson);
            Thread.Sleep(1000);
        }

        [Then(@"i can see '(.*)' '(.*)' class room for the cruise named '(.*)'")]
        public void ThenICanSeeClassRoomForTheCruiseNamed(int cnt, int clzz, string cruise)
        {
            var allCruises = SendRequest("http://localhost:50922/api/projections/cruises");
            var cruiseObj = JsonConvert.DeserializeObject<List<CruiseProjectionEntity>>(allCruises).First(a => a.Name == cruise);

            var allRooms = SendRequest("http://localhost:50922/api/projections/rooms");
            var roomObj = JsonConvert.DeserializeObject<List<RoomsForTripsEntity>>(allRooms)
                .FirstOrDefault(a => a.CruiseId == cruiseObj.Id && a.Class == clzz && a.Count==1);
            Assert.IsNotNull(roomObj);
        }


    }
}
