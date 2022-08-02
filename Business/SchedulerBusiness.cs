using Common;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

namespace Business
{
    public class SchedulerBusiness
    {
        public static async void Start()
        {

            IScheduler scheduler =  await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<DailyStatisticsScheduleMail>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithCronSchedule(Utility.Settings.ScheduleTime)            
            .Build();

            await scheduler.ScheduleJob(job, trigger);
            Utility.Logger.Info("Scheduler Started...");
        }
    }
    public class DailyStatisticsScheduleMail : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            EmailBusiness.SendDailyUsageViaEmail();
            Utility.Logger.Info("Scheduler Executed...");
            return Task.CompletedTask;
        }        
    }
}
