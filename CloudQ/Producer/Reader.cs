using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    class Reader
    {
        public List<Day> Read()
        {
            List<Details> data = new List<Details>();
            List<Day> days = new List<Day>();
            using (var streamReader = new StreamReader(@"C:\Users\cuibu\Desktop\Licenta cod\Producer\Producer\ResourceForDays.csv"))
            {
                var index = 0;

                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var values = line.Split('\t');
                    if (index == 0)
                    {
                        index++;
                        continue;
                    }

                    data.Add(new Details { Start_date = DateTime.Parse(values[0]), End_date = DateTime.Parse(values[1]), Activity = values[2] });
                }

            }
            var dates = data.Select(x => x.Start_date.Date).Distinct().ToList();
            foreach(var date in dates)
            {
                days.Add(new Day { DayDetails = data.Where(x => x.Start_date.Date == date).ToList() });
            }
            return days;

        }
    }
}
