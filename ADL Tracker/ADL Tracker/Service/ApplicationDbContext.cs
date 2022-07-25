using ADL_Tracker.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADL_Tracker.Service
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

       

        public DbSet<Elder> Elders { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<PatientDisease> PatientDiseases { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<PatientAnswer> PatientAnswers { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<MonitoringData> MonitoringDatas { get; set; }

        public DbSet<TestingDays> TestingDayss { get; set; }
        //entitatile
    }
}
