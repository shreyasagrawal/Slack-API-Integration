//Scheduler to continuously run the program

using Quartz;
using Quartz.Impl;

namespace Scheduled_pinging_of_IP_address_and_message_on_failure_to_slack
{
    public class Scheduler
        {
            public void Start(int intervalInSeconds)
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();
                IJobDetail job = JobBuilder.Create<Job>().Build();
                ITrigger trigger = TriggerBuilder.Create()
                                .WithIdentity("IP Address", "Slackbot Pinging")
                                .WithSimpleSchedule(x => x
                                    .WithIntervalInSeconds(intervalInSeconds)
                                    .RepeatForever())
                                .Build();


                scheduler.ScheduleJob(job, trigger);
            }
        }
}
