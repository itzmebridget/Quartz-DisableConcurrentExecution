using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace Quartz
{
    class Program
    {
        static void Main(string[] args)
        {
            Schedule();
        }

        private static async void Schedule()
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = await schedFact.GetScheduler();
            await sched.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            // Trigger the job to run now, and then every 2 seconds
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("myTrigger", "group1")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(2) 
                  .RepeatForever())
              .Build();

            await sched.ScheduleJob(job, trigger);
            Console.ReadLine();
        }

        [DisallowConcurrentExecution]
        [PersistJobDataAfterExecution]
        internal class HelloJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                // DisallowConcurrentExecution and PersistJobDataAfterExecution makes sure that the job instance being called is synchronous.
                await Console.Out.WriteLineAsync("Job Instance created.");

                JobDataMap dataMap = context.JobDetail.JobDataMap;
                await Console.Out.WriteLineAsync("Task a execution time : 10");

                System.Threading.Thread.Sleep(10000);
                await Console.Out.WriteLineAsync("Task b execution time : 20");
                System.Threading.Thread.Sleep(20000);
                await Console.Out.WriteLineAsync("Task c execution time : 30");
                System.Threading.Thread.Sleep(30000);
                await Console.Out.WriteLineAsync("Task d execution time : 40");
                System.Threading.Thread.Sleep(40000);
                await Console.Out.WriteLineAsync("Task e execution time : 50");
                System.Threading.Thread.Sleep(50000);
                await Console.Out.WriteLineAsync("Task f execution time : 60");
                System.Threading.Thread.Sleep(60000);
                await Console.Out.WriteLineAsync("End of the task. New Job instance should be created.");

            }
        }
    }
}
