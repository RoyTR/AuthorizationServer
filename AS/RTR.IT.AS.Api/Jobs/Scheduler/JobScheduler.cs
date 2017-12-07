using Quartz;
using Quartz.Impl;

namespace RTR.IT.AS.Api.Jobs.Scheduler
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail expiredRefreshTokensJob = JobBuilder.Create<ExpiredRefreshTokensJob>().Build();

            ITrigger expiredRefreshTokensTrigger = TriggerBuilder.Create()
                .WithIdentity("expiredRefreshTokensTrigger", "GrupoRefreshToken")
                .StartAt(DateBuilder.EvenHourDate(null))//Starts the next even-hour
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(1)//Repeat every 1 hour
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(expiredRefreshTokensJob, expiredRefreshTokensTrigger);
        }
    }
}