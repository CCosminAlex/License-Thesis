using ADL_Tracker.Entity;
using ADL_Tracker.Entity.Dto;
using ADL_Tracker.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Repository
{
    public class MonitoringDataRepository
    {
        #region Properties
        private ApplicationDbContext dbContext;

        public MonitoringDataRepository(ApplicationDbContext DbContext)
        {
            dbContext = DbContext;
            

        }
        #endregion

        public void InsertDataFromFile(List<MonitoringDataCSVDto> monitoringDataCSVDtos, string elder)
        {
            foreach(var data in monitoringDataCSVDtos)
            {
                MonitoringData monitoringData = new MonitoringData() { MonitoringDataId = Guid.NewGuid().ToString(), ActivityName = data.Activity_name, StartDate 
                    = data.Start_date, EndDate = data.End_date, ElderId = elder };
                dbContext.MonitoringDatas.Add(monitoringData);
            }
            dbContext.SaveChanges();
        }

        public List<TimelineFormated> GetMonitoringDatas(IntervalMonitoringData data)
        {
            List<TimelineFormated> list = new List<TimelineFormated>();
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
           var r = dbContext.MonitoringDatas.Where(x => x.ElderId == data.elderId && x.StartDate.Date >= DateTime.Parse(data.intervalStart).Date && x.EndDate.Date <= DateTime.Parse(data.intervalEnd).Date).OrderBy(y=>y.StartDate).ToList();
            int idex = 0;
            foreach (var t in r) {
                //if (idex == 20)
                //{
                //    break;
                //}
               // idex++;
                list.Add(new TimelineFormated { ActivityName = t.ActivityName, StartDate = t.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate = t.EndDate.ToString("yyyy-MM-dd HH:mm:ss") });
            }
            return list;
        }

        public List<Pre_Processing.Details> GetAllMonitoringDatas(string elderId, int nrDays)
        {
            DateTime start = new DateTime(2011,6,15);
            return dbContext.MonitoringDatas.Select(m=> new Pre_Processing.Details { Activity = m.ActivityName, Start_date = m.StartDate, End_date = m.EndDate, ElderId = m.ElderId }).Where(x => x.ElderId == elderId && DateTime.Compare(x.Start_date, start.AddDays(nrDays))<0).OrderBy(y=>y.Start_date).ToList();
        }
    }
}
