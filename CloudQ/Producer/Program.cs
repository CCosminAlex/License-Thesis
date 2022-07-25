using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Producer
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Reader reader = new Reader();

            List<Day> sensorDatas = reader.Read();
            Producer producer = new Producer(sensorDatas);
            var startTime = TimeSpan.Zero;
            var periodTime = TimeSpan.FromMinutes(1);
            var timer = new System.Threading.Timer((e) =>
            {
                producer.Publish();

            }, null, startTime, periodTime);


            Application.Run();
        }
    }
}
