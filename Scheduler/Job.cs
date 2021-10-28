using System;
using System.Collections.Generic;
using Quartz;
using System.IO;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using SlackBotMessages;
using SlackBotMessages.Models;

namespace Scheduled_pinging_of_IP_address_and_message_on_failure_to_slack
{
    public class Job : IJob //interference
    {
        public void Execute(IJobExecutionContext context)
        {
            //reads the object from the file
            var filePath = GetFilePath();
            var jsonData = File.ReadAllText(filePath);
            var ipListFromFile = JsonConvert.DeserializeObject<List<IpAddressData>>(jsonData); //stores objects of the file

            foreach (var item in ipListFromFile) //for loop which executes each object
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(item.IpAddress); //ip address to ping
                Console.WriteLine("IP Address: " + item.IpAddress);

                Console.WriteLine("Status: " + reply.Status); //status of the ip address
                var ipStatus = Convert.ToInt64(reply.Status);
                Console.WriteLine("Roundtrip Time: " + reply.RoundtripTime);

                if (ipStatus == 11003 || ipStatus == 11010) //condition to post message if the ip address ping fails
                {
                    //call slackmsgbot to send msg 
                    Msg(item.IpAddress, item.Webhook); //the ip address and webhook for each object is sent as arguments
                }
            }
            Console.Read();


        }
        public static string GetFilePath()
        {
            var projectDirectoryPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var fileName = "iPAddressList.json";
            var filePath = Path.Combine(projectDirectoryPath, fileName); //creates the file path to read the file
            return filePath;
        }
        public static void Msg(string ipAddress, string webhook) //uses 3rd party library. using SlackBotMessages;
        {
            var client = new SbmClient(webhook); //posts msg in the respective webhook for that IP address
            var message = new Message
            {
                Text = $"Unsuccessful ping detected! IP address: {ipAddress}" //the message to be posted to the slack channel
            };
            client.Send(message);
        }
    }
}
