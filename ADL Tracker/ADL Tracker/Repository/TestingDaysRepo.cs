using ADL_Tracker.Entity;
using ADL_Tracker.Service;
using AutoMapper;
using Pre_Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Repository
{
    public class TestingDaysRepo
    {
        private ApplicationDbContext dbContext;

        public TestingDaysRepo(ApplicationDbContext DbContext)
        {
            dbContext = DbContext;
           

        }

        public void saveToDataBase(List<TestingDays> testingDays)
        {
            if (dbContext.TestingDayss.FirstOrDefault(x => x.Start_date.Date == testingDays.FirstOrDefault().Start_date.Date) == null)
            {

                foreach (var tday in testingDays)
                {
                    tday.TestingDaysId = Guid.NewGuid().ToString();
                    dbContext.TestingDayss.Add(tday);
                    dbContext.SaveChanges();
                }
            }

        }

        public List<Details> GetTestingDay(DateTime date)
        {
            var day = dbContext.TestingDayss.Where(x => x.Start_date.Date == date.Date).ToList();
            List<Details> dayDetails = new List<Details>();
            foreach(var d in day)
            {
                dayDetails.Add(new Details { Activity = d.Activity, End_date = d.End_date, Start_date = d.Start_date });
            }
            return dayDetails;
        }
    }
}
