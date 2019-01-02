//using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary
{
   public class jpush
    {
        JPushClient client = new JPushClient("867975f8fb6a0933206b6995", "70da29d15e6b2b17908d2b9d");

        //public static void Main(string[] args)
        //{
        //    ExecutePushExample("");
        //    ExecuteDeviceEample();
        //    ExecuteReportExample();
        //    ExecuteScheduleExample();

        //    Console.ReadLine();
        //}

        public void ExecutePushExample(string content,string i_id)
        {
            PushPayload pushPayload = new PushPayload()
            {
                Platform = new List<string> { "android", "ios" },
                Audience = "all",
                Notification = new Notification
                {
                    Alert = "开奖提示",
                    Android = new Android
                    {
                        Alert = "开奖提示",
                        Title = "开奖提示"
                    },
                    IOS = new IOS
                    {
                        Alert = "开奖提示",
                        Badge = "+1"
                    }
                },
                Message = new Message
                {
                    Title = "开奖提示",
                    Content = content,
                    Extras = new Dictionary<string, string>
                    {
                        ["i_id"] = i_id
                    }
                },
                Options = new Options
                {
                    IsApnsProduction = true // 设置 iOS 推送生产环境。不设置默认为开发环境。
                }
            };
            var response = client.SendPush(pushPayload);
            Console.WriteLine(response.Content);
        }

        public void ExecuteDeviceEample()
        {
            var registrationId = "12145125123151";
            var devicePayload = new DevicePayload
            {
                Alias = "alias1",
                Mobile = "12300000000",
                Tags = new Dictionary<string, object>
                {
                    { "add", new List<string>() { "tag1", "tag2" } },
                    { "remove", new List<string>() { "tag3", "tag4" } }
                }
            };
            var response = client.Device.UpdateDeviceInfo(registrationId, devicePayload);
            Console.WriteLine(response.Content);
        }

        public void ExecuteReportExample()
        {
            var response = client.Report.GetMessageReport(new List<string> { "1251231231" });
            Console.WriteLine(response.Content);
        }

        public void ExecuteScheduleExample()
        {
            var pushPayload = new PushPayload
            {
                Platform = "all",
                Notification = new Notification
                {
                    Alert = "Hello JPush"
                }
            };
            var trigger = new Trigger
            {
                StartDate = "2017-08-03 12:00:00",
                EndDate = "2017-12-30 12:00:00",
                TriggerTime = "12:00:00",
                TimeUnit = "week",
                Frequency = 2,
                TimeList = new List<string>
                {
                    "wed", "fri"
                }
            };
            var response = client.Schedule.CreatePeriodicalScheduleTask("task1", pushPayload, trigger);
            Console.WriteLine(response.Content);
        }
    }
}
