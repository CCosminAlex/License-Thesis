using ADL_Tracker.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Pre_Processing
{
    class Helper
    {
        public Random rnd = new Random();
        private ActivityProbabilityModel activityProbabilityModel { get; set; }
        private List<Probability> probabilities { get; set; }
        private List<ProbabilityModel> probabiltyStarts { get; set; }
        private List<ProbabilityModel> probabiltyEnds { get; set; }
        private List<ProbabiltyActivitiesLengthDays> probabiltyActivitiesLengthDays { get; set; }

        //HHO

        public void PrepareData(List<Details> days)
        {
            this.activityProbabilityModel = ComputeProbabilityActivityList(days);
            this.probabilities = ComputeTransitionMatrix(days);
            this.probabiltyStarts = ComputePS(activityProbabilityModel);
            this.probabiltyEnds = ComputePE(activityProbabilityModel);
            this.probabiltyActivitiesLengthDays = ComputePL(activityProbabilityModel);
        }

        public Individ HarrisHawksAlgorithm(int T, List<Details> days)
        {
            PrepareData(days);
            List<Individ> individuals = FitnessFunction(days);
            var check = individuals.OrderByDescending(x => x.Fitness);
            List<Individ> XrabbitList = new List<Individ>();
            int t = 1;
            var minus = 0.5;
            Individ xRabbit = null;
            double maxFitness = individuals.Select(x => x.Fitness).Max();
            xRabbit = individuals.FirstOrDefault(x => x.Fitness == maxFitness);
            individuals.OrderBy(x => x.Fitness);
            var middleIndivid = individuals[individuals.Count() / 2];
            while (t <= T)
            {

                foreach (var ind in individuals)
                {

                    var r = rnd.NextDouble();
                    var E = ComputeEnergy(t, T, minus);

                    if (E >= 1)
                    {
                        xRabbit = (ExplorationPhase(ind, xRabbit, middleIndivid));

                    }
                    if (E < 1)
                    {
                        if (r >= 0.5 && E >= 0.5)
                        {
                            xRabbit = (SoftBesiegeStrategy(ind, xRabbit));
                        }
                        else if (r >= 0.5 && E < 0.5)
                        {
                            xRabbit = (HardBesiegeStrategy(ind, xRabbit));
                        }
                        else if (r < 0.5 && E >= 0.5)
                        {
                            xRabbit = (SoftBesiegeWithProgressiveRapidDives(ind, xRabbit, days));
                        }
                        else if (r < 0.5 && E < 0.5)
                        {
                            xRabbit = (HardBesiegeWithProgressiveRapidDives(middleIndivid, xRabbit, days));
                        }


                    }

                }

                t++;

            }
            var r2 = days.ToList();
            r2.AddRange(xRabbit.Activities);
            PrepareData(r2);
            xRabbit.Fitness = ComputeFitness(xRabbit.Activities.ToArray());
            Console.WriteLine("f:" + xRabbit.Fitness);

            return xRabbit;
        }

        public List<ModelForActivityDuration> DurationRoutine(List<Details> routine)
        {
            return ComputeActivityDuration(ComputePartOfTheDay(routine.ToArray()));
        }

        public Recommendation IsNormal(List<Details> routine, List<Details> day, List<Details> days)
        {
            
            var probi1 = ComputeTransitionMatrix(routine);
            var probi2 = ComputeTransitionMatrix(day);
            var activityIntervals = ComputeTimeIntervalAndDuration(probi1.Select(x => x.Activity).ToList(), ComputePartOfTheDay(routine.ToArray()));
            var counterDeviation = 0;
            var modelForActivityDuration = ComputeActivityDuration(ComputePartOfTheDay(day.ToArray()));
            var activityDurationRoutine = ComputeActivityDuration(ComputePartOfTheDay(days.ToArray()));
            var typeofActivity = ComputeTimeIntervalAndDuration(probi2.Select(x => x.Activity).ToList(), ComputePartOfTheDay(day.ToArray()));
            var threshold = 0;
            List<string> PartsofTheDay = new List<string> { "Night_Morning", "Morning", "Noon", "Evening", "Night_Night" };
            foreach (var activityName in modelForActivityDuration)
            {
                foreach (var partoftheday in PartsofTheDay)
                {

                    if (activityName.PartOfTheDay == partoftheday)
                    {
                        double x;
                        var activityInRoutine = activityDurationRoutine.FirstOrDefault(y => y.ActivityName == activityName.ActivityName && y.PartOfTheDay == partoftheday);

                        threshold = 0;
                        double[] array = new double[2];
                        array[0] = activityInRoutine != null ? activityInRoutine.Duration : 0;
                        if (array[0] >= 1800) //long
                        {
                            threshold = 2500;
                        }
                        else
                         if (array[0] >= 100)
                        {
                            threshold = 500;
                        }
                        else
                         if (array[0] > 0)
                        {
                            threshold = 50;
                        }
                        if (activityInRoutine == null)
                        {
                            x = meanAbsDevtion(new double[] { activityName.Duration, 0 }, 2);
                        }
                        else
                        {
                            x = meanAbsDevtion(new double[] { activityName.Duration, activityInRoutine.Duration }, 2);
                        }

                        if (x > threshold && x != -1)
                        {
                            counterDeviation++;
                        }
                        


                    }
                    //array[1] = activityIntervals.listMad.Where(x => x.ActivityName == activityName && x.PartOfTheDay == partoftheday).Select(x => x.Value).ToArray().FirstOrDefault();
                    //var meanDev = meanAbsDevtion(array, array.Length);
                    //Console.WriteLine(partoftheday + ":  " + array[0] + " " + array[1] + " " + meanDev + " thrsh:" + threshold);

                    //if(threshold == 2500 && activityIntervals.listMad.Where(x => x.ActivityName == activityName && x.PartOfTheDay == partoftheday).Select(x => x.TypeOfActivity).FirstOrDefault() != "Long")
                    //{
                    //    counterDeviation++;
                    //}
                    //else
                    //if(threshold == 500 && activityIntervals.listMad.Where(x => x.ActivityName == activityName && x.PartOfTheDay == partoftheday).Select(x => x.TypeOfActivity).FirstOrDefault() != "Medium")
                    //{
                    //    counterDeviation++;
                    //}
                    //else
                    //if(threshold == 50 && activityIntervals.listMad.Where(x => x.ActivityName == activityName && x.PartOfTheDay == partoftheday).Select(x => x.TypeOfActivity).FirstOrDefault() != "Short")
                    //{
                    //    counterDeviation++;
                    //}


                }
            }
            Recommendation recommandation = new Recommendation()
            {
                SleepHours = (int)Math.Round(modelForActivityDuration.Where(y => y.ActivityName == "Sleep").Select(x => x.Duration).Sum() / 3600),
                TakeMedication = modelForActivityDuration.Any(x => x.ActivityName.Contains("Medicine") || x.ActivityName.Contains("Meds")),
                IsDeviated = true,
                Deviation = "Duration"
            };

            if (counterDeviation <= day.Count * 50 / 100)
            {
                recommandation.IsDeviated = false;
            }
            return recommandation;
        }


        #region Duration Detection
        public List<Details> ComputeTypeOfActivity(Details[] results)
        {
            List<Details> activities = new List<Details>();
            TimeSpan ts = new TimeSpan();
            foreach (var a in results)
            {
                ts = a.End_date - a.Start_date;
                if (ts.Minutes > 0 && ts.Minutes <= 120)
                {
                    Details activity = new Details { Activity = a.Activity, Start_date = a.Start_date, End_date = a.End_date, PartOfTheDay = a.PartOfTheDay, TypeOfActivity = "Short" };
                    activities.Add(activity);
                }
                else if (ts.Minutes > 120 && ts.Minutes <= 300)
                {
                    Details activity = new Details { Activity = a.Activity, Start_date = a.Start_date, End_date = a.End_date, PartOfTheDay = a.PartOfTheDay, TypeOfActivity = "Medium" };
                    activities.Add(activity);
                }
                else if (ts.Minutes > 300 && ts.Minutes <= 540)
                {
                    Details activity = new Details { Activity = a.Activity, Start_date = a.Start_date, End_date = a.End_date, PartOfTheDay = a.PartOfTheDay, TypeOfActivity = "Long" };
                    activities.Add(activity);
                }
            }

            return activities;
        }
        public List<Details> ComputePartOfTheDay(Details[] results)
        {
            List<Details> activities = new List<Details>();
            foreach (var r in results)
            {
                if (TimeSpan.Parse(r.Start_date.ToString("HH:mm")) >= TimeSpan.Parse("22:00") && (TimeSpan.Parse(r.Start_date.ToString("HH:mm")) < TimeSpan.Parse("23:59"))) //Night
                {
                    if (TimeSpan.Parse(r.End_date.ToString("HH:mm")) <= TimeSpan.Parse("23:59"))


                    {
                        Details activity = new Details { Activity = r.Activity, Start_date = r.Start_date, End_date = r.End_date, PartOfTheDay = "Night_Night" };
                        activities.Add(activity);
                    }
                    else if (TimeSpan.Parse(r.End_date.ToString("HH:mm")) < TimeSpan.Parse("06:00"))
                    {
                        Details activity = new Details { Activity = r.Activity, Start_date = r.Start_date, End_date = r.End_date.Date + new TimeSpan(23, 59, 0), PartOfTheDay = "Night_Night" };
                        activities.Add(activity);

                        Details activity2 = new Details { Activity = r.Activity, Start_date = r.End_date.Date + new TimeSpan(0, 0, 0), End_date = r.End_date, PartOfTheDay = "Night_Morning" };
                        activities.Add(activity2);

                    }
                }
                else
                 if (TimeSpan.Parse(r.Start_date.ToString("HH:mm")) >= TimeSpan.Parse("00:00") && (TimeSpan.Parse(r.Start_date.ToString("HH:mm")) < TimeSpan.Parse("06:00")))
                {
                    if (TimeSpan.Parse(r.End_date.ToString("HH:mm")) >= TimeSpan.Parse("06:00"))
                    {
                        Details activity = new Details { Activity = r.Activity, Start_date = r.Start_date, End_date = r.End_date.Date + new TimeSpan(6, 0, 0), PartOfTheDay = "Night_Morning" };
                        activities.Add(activity);

                        Details activity2 = new Details { Activity = r.Activity, Start_date = r.Start_date.Date + new TimeSpan(6, 0, 0), End_date = r.End_date, PartOfTheDay = "Morning" };
                        activities.Add(activity2);

                    }
                    else if (TimeSpan.Parse(r.End_date.ToString("HH:mm")) < TimeSpan.Parse("06:00"))
                    {
                        Details activity = new Details { Activity = r.Activity, Start_date = r.Start_date, End_date = r.End_date, PartOfTheDay = "Night_Morning" };
                        activities.Add(activity);



                    }
                }
                else if (TimeSpan.Parse(r.Start_date.ToString("HH:mm")) >= TimeSpan.Parse("06:00") && TimeSpan.Parse(r.Start_date.ToString("HH:mm")) < TimeSpan.Parse("12:00")) //Morning
                {

                    if (TimeSpan.Parse(r.End_date.ToString("HH:mm")) <= TimeSpan.Parse("12:00"))
                    {
                        Details activity = new Details { Activity = r.Activity, Start_date = r.Start_date, End_date = r.End_date, PartOfTheDay = "Morning" };
                        activities.Add(activity);
                    }
                    else
                    {
                        Details activity = new Details { Activity = r.Activity, Start_date = r.Start_date, End_date = r.End_date.Date + new TimeSpan(12, 0, 0), PartOfTheDay = "Morning" };
                        activities.Add(activity);

                        Details activity2 = new Details { Activity = r.Activity, Start_date = r.Start_date.Date + new TimeSpan(12, 0, 0), End_date = r.End_date, PartOfTheDay = "Noon" };
                        activities.Add(activity2);
                    }

                }
                else
                if (TimeSpan.Parse(r.Start_date.ToString("HH:mm")) >= TimeSpan.Parse("12:00") && TimeSpan.Parse(r.Start_date.ToString("HH:mm")) < TimeSpan.Parse("17:00")) //Noon
                {

                    if (TimeSpan.Parse(r.End_date.ToString("HH:mm")) <= TimeSpan.Parse("17:00"))
                    {
                        Details activity = new Details { Activity = r.Activity, Start_date = r.Start_date, End_date = r.End_date, PartOfTheDay = "Noon" };
                        activities.Add(activity);
                    }
                    else
                    {
                        Details activity = new Details { Activity = r.Activity, Start_date = r.Start_date, End_date = r.End_date.Date + new TimeSpan(17, 0, 0), PartOfTheDay = "Noon" };
                        activities.Add(activity);

                        Details activity2 = new Details { Activity = r.Activity, Start_date = r.Start_date.Date + new TimeSpan(17, 0, 0), End_date = r.End_date, PartOfTheDay = "Evening" };
                        activities.Add(activity2);
                    }

                }
                else
                if (TimeSpan.Parse(r.Start_date.ToString("HH:mm")) >= TimeSpan.Parse("17:00") && TimeSpan.Parse(r.Start_date.ToString("HH:mm")) < TimeSpan.Parse("22:00")) //Evening
                {

                    if (TimeSpan.Parse(r.End_date.ToString("HH:mm")) <= TimeSpan.Parse("22:00"))
                    {
                        Details activity = new Details { Activity = r.Activity, Start_date = r.Start_date, End_date = r.End_date, PartOfTheDay = "Evening" };
                        activities.Add(activity);
                    }
                    else
                    {
                        Details activity = new Details { Activity = r.Activity, Start_date = r.Start_date, End_date = r.End_date.Date + new TimeSpan(22, 0, 0), PartOfTheDay = "Evening" };
                        activities.Add(activity);
                        if (TimeSpan.Parse(r.End_date.ToString("HH:mm")) <= TimeSpan.Parse("23:59"))
                        {
                            Details activity2 = new Details { Activity = r.Activity, Start_date = r.Start_date.Date + new TimeSpan(22, 0, 0), End_date = r.End_date, PartOfTheDay = "Night_Night" };
                            activities.Add(activity2);
                        }
                        else if (TimeSpan.Parse(r.End_date.ToString("HH:mm")) > TimeSpan.Parse("00:00") && TimeSpan.Parse(r.End_date.ToString("HH:mm")) < TimeSpan.Parse("06:00"))
                        {
                            Details activity2 = new Details { Activity = r.Activity, Start_date = r.Start_date.Date + new TimeSpan(22, 0, 0), End_date = r.End_date, PartOfTheDay = "Night_Night" };
                            activities.Add(activity2);

                            Details activity3 = new Details { Activity = r.Activity, Start_date = r.End_date.Date + new TimeSpan(0, 0, 0), End_date = r.End_date, PartOfTheDay = "Night_Morning" };
                            activities.Add(activity2);
                        }

                    }
                }

            }
            return activities;
        }
        public List<ModelForActivityDuration> ComputeActivityDuration(List<Details> activities)
        {
            List<ModelForActivityDuration> allModelsForActivityDuration = new List<ModelForActivityDuration>();
            for (int i = 0; i <= activities.Count - 1; i++)
            {
                ModelForActivityDuration currentModelForActivityDuration = new ModelForActivityDuration() { ActivityName = activities[i].Activity, Day = activities[i].Start_date.Date, PartOfTheDay = activities[i].PartOfTheDay, Duration = (activities[i].End_date - activities[i].Start_date).TotalSeconds };
                var currentDay = activities.Where(x => x.Start_date.Date == activities[0].Start_date.Date).ToList();
                foreach (var activity in currentDay)
                {
                    var currentModelForActivityDurationFind = allModelsForActivityDuration.FirstOrDefault(a => a.ActivityName == activities[i].Activity && a.Day == activities[i].Start_date.Date && a.PartOfTheDay == activities[i].PartOfTheDay);
                    if (currentModelForActivityDurationFind != null)
                    {

                        currentModelForActivityDurationFind.Duration += (activity.End_date - activity.Start_date).Minutes;
                    }
                    else
                    {
                        allModelsForActivityDuration.Add(currentModelForActivityDuration);
                    }
                }
            }
            return allModelsForActivityDuration;
        }
        public ModelForMadAndIntervals ComputeTimeIntervalAndDuration(List<string> activityNameList, List<Details> activities)
        {
            var allModelsForActivityDuration = ComputeActivityDuration(activities);


            List<Details> night_morningActivityList = new List<Details>();
            List<Details> morningActivityList = new List<Details>();
            List<Details> noonActivityList = new List<Details>();
            List<Details> eveningActivityList = new List<Details>();
            List<Details> night_nightActivityList = new List<Details>();
            List<ModelForActivityInterval> modelsForActivityInterval = new List<ModelForActivityInterval>();
            List<MAD> mads = new List<MAD>();
            List<string> PartsofTheDay = new List<string> { "Night_Morning", "Morning", "Noon", "Evening", "Night_Night" };
            foreach (var activityName in activityNameList)
            {
                foreach (var partoftheday in PartsofTheDay)
                {
                    var array = allModelsForActivityDuration.Where(x => x.ActivityName == activityName && x.PartOfTheDay == partoftheday).Select(x => x.Duration).ToArray();


                    var madact = new MAD { ActivityName = activityName, Value = array.Length != 0 ? meanAbsDevtion(array, array.Length) : 0, PartOfTheDay = partoftheday };
                    if (madact.Value >= 1000)
                    {
                        madact.TypeOfActivity = "Long";
                    }
                    else
                    if (madact.Value >= 100)
                    {
                        madact.TypeOfActivity = "Medium";
                    }
                    else
                    if (madact.Value > 0)
                    {
                        madact.TypeOfActivity = "Short";
                    }
                    mads.Add(madact);
                }


                night_morningActivityList = activities.Where(x => x.Activity == activityName && x.PartOfTheDay == "Night_Morning").ToList();
                morningActivityList = activities.Where(x => x.Activity == activityName && x.PartOfTheDay == "Morning").ToList();
                noonActivityList = activities.Where(x => x.Activity == activityName && x.PartOfTheDay == "Noon").ToList();
                eveningActivityList = activities.Where(x => x.Activity == activityName && x.PartOfTheDay == "Evening").ToList();
                night_nightActivityList = activities.Where(x => x.Activity == activityName && x.PartOfTheDay == "Night_Night").ToList();


                modelsForActivityInterval.Add(
                    ComputeActivityIntervals(night_nightActivityList, "Night_Morning", activityName));
                modelsForActivityInterval.Add(ComputeActivityIntervals(morningActivityList, "Morning", activityName));
                modelsForActivityInterval.Add(ComputeActivityIntervals(noonActivityList, "Noon", activityName));
                modelsForActivityInterval.Add(ComputeActivityIntervals(eveningActivityList, "Evening", activityName));
                modelsForActivityInterval.Add(ComputeActivityIntervals(night_nightActivityList, "Night_Night", activityName));

            }
            return new ModelForMadAndIntervals { listMad = mads, modelsForActivityInterval = modelsForActivityInterval };

        }



        private static ModelForActivityInterval ComputeActivityIntervals(List<Details> activityList, string partOfTheDay, string activityName)
        {

            TimeSpan minS = new TimeSpan(23, 59, 59);
            TimeSpan maxE = new TimeSpan(0, 0, 0);


            foreach (var activity in activityList)
            {
                if (TimeSpan.Parse(activity.Start_date.ToString("HH:mm:ss")) < minS)
                    minS = TimeSpan.Parse(activity.Start_date.ToString("HH:mm:ss"));
                if (TimeSpan.Parse(activity.End_date.ToString("HH:mm:ss")) > maxE)
                    maxE = TimeSpan.Parse(activity.End_date.ToString("HH:mm:ss"));

            }
            return new ModelForActivityInterval { ActivityName = activityName, PartOfDay = partOfTheDay, Min = minS, Max = maxE };

        }

        private static double Mean(double[] arr, int n)
        {
            // Calculate sum of all elements.
            double sum = 0;

            for (int i = 0; i < n; i++)
                sum = sum + arr[i];

            return sum / n;
        }

        // Function to find mean absolute
        // deviation of given elements.
        private static double meanAbsDevtion(double[] arr, int n)
        {
            // Calculate the sum of absolute
            // deviation about mean.
            double absSum = 0;

            for (int i = 0; i < n; i++)
                absSum = absSum + Math.Abs(arr[i]
                                    - Mean(arr, n));

            // Return mean absolute
            // deviation about mean.
            return absSum / n;
        }

        #endregion

        #region Order Detection
        public ActivityProbabilityModel ComputeProbabilityActivityList(List<Details> results)
        {
            ActivityProbabilityModel activityProbabilityModel = new ActivityProbabilityModel();
            var index = 0;
            var day = results[0];
            activityProbabilityModel.StartActivity.Add(day.Activity);
            activityProbabilityModel.activityPerDays.Add(new ActivityPerDay { Date = day.Start_date.ToString("dd-MM-yyyy"), NrActivityDay = 1 });
            foreach (var r in results)
            {


                if (r.Start_date.ToString("dd-MM-yyyy") != day.Start_date.ToString("dd-MM-yyyy"))
                {

                    activityProbabilityModel.StartActivity.Add(r.Activity);
                    activityProbabilityModel.EndActivity.Add(day.Activity);
                    ActivityPerDay activityPerDay = new ActivityPerDay();
                    activityPerDay.Date = r.Start_date.ToString("dd-MM-yyyy");
                    activityPerDay.NrActivityDay = 1;
                    activityProbabilityModel.activityPerDays.Add(activityPerDay);
                    day = r;


                }
                else if (index == results.Count() - 1)
                {
                    activityProbabilityModel.EndActivity.Add(r.Activity);
                }
                else
                    if (r.Start_date.ToString("dd-MM-yyyy") == day.Start_date.ToString("dd-MM-yyyy"))
                {
                    var activity = activityProbabilityModel.activityPerDays.FirstOrDefault(x => x.Date == day.Start_date.ToString("dd-MM-yyyy"));
                    activity.NrActivityDay++;
                    day = r;
                }
                index++;


            }

            return activityProbabilityModel;
        }

        public List<Probability> ComputeTransitionMatrix(List<Details> results)
        {
            List<Probability> probabilities = new List<Probability>();
            List<string> states = new List<string>();

            foreach (var r in results)
            {
                if (!probabilities.Any(x => x.Activity == r.Activity))
                {
                    var prob = new Probability();
                    prob.Activity = r.Activity;
                    prob.Prob = results.Count(x => x.Activity == r.Activity);
                    prob.Next = new List<Probability>();

                    probabilities.Add(prob);
                }
            }

            foreach (var p in probabilities)
            {
                states.Add(p.Activity);

                for (int i = 0; i < results.Count() - 1; i++)
                {

                    if (p.Activity == results[i].Activity)
                    {

                        var x = results[i + 1];
                        var probabilty = p.Next.FirstOrDefault(q => q.Activity == x.Activity);

                        if (probabilty != null)
                        {
                            probabilty.Prob++;

                        }
                        else
                        {
                            var prob = new Probability();
                            prob.Activity = x.Activity;
                            prob.Prob = 1;
                            p.Next.Add(prob);
                        }

                    }
                    else
                    {
                        var x = results[i + 1];
                        var probabilty = p.Next.FirstOrDefault(q => q.Activity == x.Activity);
                        if (probabilty == null)
                        {
                            var prob = new Probability();
                            prob.Activity = x.Activity;
                            prob.Prob = 0;
                            p.Next.Add(prob);
                        }
                    }

                }

                foreach (var n in p.Next)
                {

                    n.ProbNext = 0;
                    if (p.Prob != 0)
                        n.ProbNext = ((double)n.Prob) / p.Prob;
                }
            }
            return probabilities;
        }

        public List<ProbabilityModel> ComputePS(ActivityProbabilityModel activityProbabilityModel)
        {
            List<ProbabilityModel> probabiltyStarts = new List<ProbabilityModel>();
            foreach (var ps in activityProbabilityModel.StartActivity.Distinct().ToList())
            {

                probabiltyStarts.Add(new ProbabilityModel { ActivityName = ps, Probability = ((double)activityProbabilityModel.StartActivity.Where(x => x == ps).Count()) / activityProbabilityModel.StartActivity.Count() });
            }
            return probabiltyStarts;
        }
        public List<ProbabilityModel> ComputePE(ActivityProbabilityModel activityProbabilityModel)
        {
            List<ProbabilityModel> probabiltyEnds = new List<ProbabilityModel>();
            foreach (var pe in activityProbabilityModel.EndActivity.Distinct().ToList())
            {

                probabiltyEnds.Add(new ProbabilityModel { ActivityName = pe, Probability = ((double)activityProbabilityModel.EndActivity.Where(x => x == pe).Count()) / activityProbabilityModel.EndActivity.Count() });
            }
            return probabiltyEnds;
        }
        public List<ProbabiltyActivitiesLengthDays> ComputePL(ActivityProbabilityModel activityProbabilityModel)
        {
            List<ProbabiltyActivitiesLengthDays> probabiltyActivitiesLengthDays = new List<ProbabiltyActivitiesLengthDays>();
            foreach (var pl in activityProbabilityModel.activityPerDays.Select(x => x.NrActivityDay).Distinct().ToList())
            {

                probabiltyActivitiesLengthDays.Add(new ProbabiltyActivitiesLengthDays { NumberOfActivities = pl, Probability = ((double)activityProbabilityModel.activityPerDays.Where(x => x.NrActivityDay == pl).Count()) / activityProbabilityModel.activityPerDays.Count() });
            }
            return probabiltyActivitiesLengthDays;
        }

        public double ComputeFitness(Details[] results)
        {

            double randP = rnd.NextDouble();
            double randQ = 1.0 - randP;
            List<double> fitnessList = new List<double>();
            int index2 = 0;
            int l = 0;
            Details firstActivity = results.First();
            double fitness = 1.0;
            if (probabiltyStarts.FirstOrDefault(x => x.ActivityName == firstActivity.Activity) != null)
            {
                fitness = probabiltyStarts.FirstOrDefault(x => x.ActivityName == firstActivity.Activity).Probability;
            }
            List<Details> activities = new List<Details>();
            if (probabiltyEnds.FirstOrDefault(x => x.ActivityName == firstActivity.Activity) != null && probabiltyActivitiesLengthDays.FirstOrDefault(x => x.NumberOfActivities == results.Count()) != null)
            {
                fitness *= (randP * probabiltyEnds.FirstOrDefault(x => x.ActivityName == firstActivity.Activity).Probability + (randQ * probabiltyActivitiesLengthDays.FirstOrDefault(x => x.NumberOfActivities == results.Count()).Probability));
            }
            else if (probabiltyActivitiesLengthDays.FirstOrDefault(x => x.NumberOfActivities == results.Count()) != null)
            {
                fitness *= (randP * 1 + (randQ * probabiltyActivitiesLengthDays.FirstOrDefault(x => x.NumberOfActivities == results.Count()).Probability));
            }
            else if (probabiltyEnds.FirstOrDefault(x => x.ActivityName == firstActivity.Activity) != null)
            {
                fitness *= (randP * probabiltyEnds.FirstOrDefault(x => x.ActivityName == firstActivity.Activity).Probability);
            }
            activities = new List<Details>();
            foreach (var act in results)
            {
                // Console.WriteLine(act.Activity);
                firstActivity = act;
                l++;
                activities.Add(act);
                var next = probabilities.FirstOrDefault(x => x.Activity == act.Activity).Next;

                if ((index2 + 1) < results.Count())
                {
                    if (next.FirstOrDefault(x => x.Activity == results[index2 + 1].Activity) != null)
                        fitness *= next.FirstOrDefault(x => x.Activity == results[index2 + 1].Activity).ProbNext;
                }
                index2++;
                //Console.WriteLine(fitness);
            }
            //Console.WriteLine(Math.Pow(fitness, 1.0 / (l + 1)));
            return Math.Pow(fitness, 1.0 / (Math.Pow((l + 1), 2)));

        }

        public List<Individ> FitnessFunction(List<Details> results)
        {
            List<Individ> individs = new List<Individ>();
            double randP = 0.5;//rnd.NextDouble();
            double randQ = 1.0 - randP;
            List<double> fitnessList = new List<double>();
            int index2 = 0;
            int l = 0;
            Details firstActivity = results.First();
            double fitness = 1.0;
            if (probabiltyStarts.FirstOrDefault(x => x.ActivityName == firstActivity.Activity) != null)
            {


                fitness = probabiltyStarts.FirstOrDefault(x => x.ActivityName == firstActivity.Activity).Probability;
            }

            List<Details> activities = new List<Details>();
            foreach (var act in results)
            {

                if (firstActivity.Start_date.ToString("dd-MM-yyyy") != act.Start_date.ToString("dd-MM-yyyy"))
                {
                    if (probabiltyEnds.FirstOrDefault(x => x.ActivityName == firstActivity.Activity) != null && probabiltyActivitiesLengthDays.FirstOrDefault(x => x.NumberOfActivities == l) != null)
                        fitness *= (randP * probabiltyEnds.FirstOrDefault(x => x.ActivityName == firstActivity.Activity).Probability) + (randQ * probabiltyActivitiesLengthDays.FirstOrDefault(x => x.NumberOfActivities == l).Probability);
                    fitness = Math.Pow(fitness, 1.0 / (Math.Pow((l + 1), 2)));
                    fitnessList.Add(fitness);
                    individs.Add(new Individ { Fitness = fitness, NumberOfActivities = l, Activities = activities });
                    if (probabiltyStarts.FirstOrDefault(x => x.ActivityName == act.Activity) != null)
                        fitness = probabiltyStarts.FirstOrDefault(x => x.ActivityName == act.Activity).Probability;
                    l = 0;
                    activities = new List<Details>();
                }

                firstActivity = act;
                l++;
                activities.Add(act);
                var next = probabilities.FirstOrDefault(x => x.Activity == act.Activity).Next;

                if ((index2 + 1) < results.Count())
                {
                    if (next.FirstOrDefault(x => x.Activity == results[index2 + 1].Activity) != null)
                        fitness *= next.FirstOrDefault(x => x.Activity == results[index2 + 1].Activity).ProbNext;
                }

                index2++;
                if (index2 == results.Count())
                {
                    fitness = Math.Pow(fitness, 1.0 / (Math.Pow((l + 1), 2)));
                    fitnessList.Add(fitness);
                    individs.Add(new Individ { Fitness = fitness, NumberOfActivities = l, Activities = activities });
                }
            }

            return individs;
        }

        public Individ ExplorationPhase(Individ xt, Individ xRabbit, Individ middleIndivid)
        {

            var p = rnd.NextDouble(); //Console.WriteLine(p);
            if (p < 0.5)
            {
                var xt1 = ComputeXt1(xt);
                var Xrand = ComputeXRand(xt);
                var xt2 = ComputeXrandMinusXt(xt1, Xrand);
                var xt3 = Do3Opt(xt2); //aplicam 3-opt asupra lui xt’’ si rezulta xt’’’
                var xt4 = ComputeXrandMinusXt(xt3, Xrand);
                xt4.Fitness = ComputeFitness(xt4.Activities.ToArray());
                return xt4;
            }

            if (p >= 0.5)
            {
                var XrabbitXmiddle = ComputeXrabbitMinusXmiddle(xRabbit, middleIndivid);

                return DeltaXt(XrabbitXmiddle, LKH(XrabbitXmiddle));

            }
            return null;

        }

        public Individ SoftBesiegeStrategy(Individ Xt, Individ Xrabbit)
        {
            var deltaXt = DeltaXt(Xrabbit, Xt);
            var JXRabbit = JXrabbit(Xrabbit);
            var Xt2 = DeltaXt(JXRabbit, Xt);
            var EXt2 = JXrabbit(Xt2);


            return DeltaXt(deltaXt, EXt2);

        }

        public Individ HardBesiegeStrategy(Individ Xt, Individ Xrabbit)
        {
            var deltaXt = DeltaXt(Xrabbit, Xt);
            var EXt = JXrabbit(deltaXt);

            return DeltaXt(Xrabbit, EXt);
        }

        public Individ SoftBesiegeWithProgressiveRapidDives(Individ Xt, Individ Xrabbit, List<Details> results)
        {
            var JXRabbit = JXrabbit(Xrabbit);
            var deltaXt = DeltaXt(JXRabbit, Xt);
            var EX = JXrabbit(deltaXt);
            var Y = DeltaXt(Xrabbit, EX);
            var y = results.ToList();
            y.AddRange(Y.Activities);
            var Yprobabilities = ComputeTransitionMatrix(y);
            var Z = YMutation(Y);
            var z = results.ToList();
            z.AddRange(Z.Activities);
            var Zprobabilities = ComputeTransitionMatrix(z);
            if (ComputeFitness(Y.Activities.ToArray()) > Xt.Fitness)
            //FitnessFunction(Xt.Activities.ToArray(), probabiltyStarts, probabiltyEnds, probabiltyActivitiesLengthDays, probabilities)[0].Fitness)
            {
                return Y;
            }
            if (ComputeFitness(Z.Activities.ToArray()) > Xt.Fitness)
            //FitnessFunction(Xt.Activities.ToArray(), probabiltyStarts, probabiltyEnds, probabiltyActivitiesLengthDays, probabilities)[0].Fitness)
            {
                return Z;
            }

            return Xrabbit;


        }

        public Individ HardBesiegeWithProgressiveRapidDives(Individ Xmiddle, Individ Xrabbit, List<Details> results)
        {
            return SoftBesiegeWithProgressiveRapidDives(Xmiddle, Xrabbit, results);
        }

        private Individ ComputeXt1(Individ Xt)
        {
            return Do2Opt(Xt);
        }


        private Individ Do2Opt(Individ Xt)
        {
            int size = Xt.NumberOfActivities;
            int improve = 0;
            var currentDay = Xt.Activities;
            var nextDayFitness = Xt.Fitness;
            double dayFitness = nextDayFitness;
            while (improve < 2)
            {


                for (int i = 1; i < size - 1; i++)
                {
                    for (int k = i + 1; k < size; k++)
                    {



                        var nextDay = TwoOptSwap(i, k, currentDay.ToArray());
                        nextDayFitness = ComputeFitness(nextDay);

                        if (nextDayFitness > dayFitness)
                        {
                            improve = 0;
                            currentDay = nextDay.ToList();
                            dayFitness = nextDayFitness;
                        }
                    }

                }
                improve++;
            }
            return new Individ { Activities = currentDay, NumberOfActivities = currentDay.Count(), Fitness = ComputeFitness(currentDay.ToArray()) };
        }

        private Details[] TwoOptSwap(int i, int k, Details[] day)
        {
            if (i == 0 || day[i] == day[k]) return day;
            Details[] nextDay = new Details[day.Length];
            Array.Copy(day, nextDay, day.Length);
            Array.Reverse(nextDay, i, k - i + 1);
            return nextDay;

        }

        private List<int> ComputeXRand(Individ Xt)
        {
            int maxOnes = (Xt.NumberOfActivities - 1) / 2;
            List<int> Xrand = new List<int>();
            for (var i = 1; i < Xt.NumberOfActivities; ++i)
            {
                if (Xrand.Where(x => x == 1).Count() >= maxOnes)
                {
                    Xrand.Add(0);
                }
                else
                {
                    Xrand.Add(rnd.Next(0, 2));
                }
            }
            return Xrand;
        }

        private Individ ComputeXrandMinusXt(Individ Xt, List<int> Xrand)
        {
            for (int index = 1; index < Xrand.Count() - 2; index++)
            {
                if (Xrand[index] == 1)
                {
                    var aux = Xt.Activities[index];
                    var indexSwap = rnd.Next(index + 1, Xrand.Count() - 1);
                    if (probabilities.FirstOrDefault(x => x.Activity == Xt.Activities[index - 1].Activity).Next.FirstOrDefault(y => y.Activity == Xt.Activities[indexSwap].Activity).ProbNext != 0 &&
                        probabilities.FirstOrDefault(x => x.Activity == Xt.Activities[indexSwap].Activity).Next.FirstOrDefault(y => y.Activity == Xt.Activities[index + 1].Activity).ProbNext != 0 &&
                        probabilities.FirstOrDefault(x => x.Activity == Xt.Activities[index + 1].Activity).Next.FirstOrDefault(y => y.Activity == Xt.Activities[index].Activity).ProbNext != 0 &&
                        probabilities.FirstOrDefault(x => x.Activity == Xt.Activities[index].Activity).Next.FirstOrDefault(y => y.Activity == Xt.Activities[indexSwap + 1].Activity).ProbNext != 0 &&
                        Xt.Activities[index] != Xt.Activities[indexSwap]
                        )
                    {
                        Xt.Activities[index] = Xt.Activities[indexSwap];
                        Xt.Activities[indexSwap] = aux;
                    }
                }
            }
            Xt.Fitness = ComputeFitness(Xt.Activities.ToArray());
            return Xt;
        }
        private Individ Do3Opt(Individ Xt)
        {
            int size = Xt.NumberOfActivities;
            int improve = 0;
            var currentDay = Xt.Activities;
            var currentDayFitness = Xt.Fitness;
            double dayFitness = currentDayFitness;
            while (improve < 2)
            {
                Details[] nextDay;

                for (int i = 0; i < size - 2; i++)
                {

                    for (int k = i + 1; k < size - 1; k++)
                    {

                        for (int j = k + 1; j < size; j++)
                        {


                            nextDay = TwoOptSwap(i, j, currentDay.ToArray());
                            nextDay = TwoOptSwap(k, j, nextDay);
                            var nextDayFitness = ComputeFitness(nextDay);

                            if (nextDayFitness > dayFitness)
                            {
                                improve = 0;
                                currentDay = nextDay.ToList();
                                dayFitness = nextDayFitness;
                            }

                        }
                    }

                }

                improve++;
            }

            return new Individ { Activities = currentDay, NumberOfActivities = currentDay.Count(), Fitness = ComputeFitness(currentDay.ToArray()) };
        }
        private Individ ComputeXrabbitMinusXmiddle(Individ Xrabbit, Individ Xmiddle)
        {
            List<Details> activities = new List<Details>();
            List<int> positions = new List<int>();
            Individ individ = new Individ();
            var len = Math.Max(Xmiddle.NumberOfActivities, Xrabbit.NumberOfActivities);
            individ.Activities = new List<Details>(new Details[len]);
            if (Xrabbit.NumberOfActivities == Xmiddle.NumberOfActivities)
            {
                for (var i = 0; i < Xmiddle.NumberOfActivities; ++i)
                {
                    if (Xrabbit.Activities[i].Activity == Xmiddle.Activities[i].Activity)
                    {
                        individ.Activities[i] = Xrabbit.Activities[i];

                    }
                    else
                    {
                        activities.Add(Xrabbit.Activities[i]);
                        positions.Add(i);
                    }

                }


            }
            else if (Xrabbit.NumberOfActivities > Xmiddle.NumberOfActivities)
            {
                for (int i = 0; i < Xrabbit.NumberOfActivities; i++)
                {
                    if (Xmiddle.Activities.Select(x => x.Activity).Contains(Xrabbit.Activities[i].Activity))

                    {
                        individ.Activities[i] = Xrabbit.Activities[i];
                    }
                    else
                    {
                        activities.Add(Xrabbit.Activities[i]);
                        positions.Add(i);
                    }
                }

            }
            else
            {
                for (int i = 0; i < Xmiddle.NumberOfActivities; i++)
                {
                    if (Xrabbit.Activities.Select(x => x.Activity).Contains(Xmiddle.Activities[i].Activity))

                    {
                        individ.Activities[i] = Xmiddle.Activities[i];
                    }
                    else
                    {
                        activities.Add(Xmiddle.Activities[i]);
                        positions.Add(i);
                    }
                }

            }
            for (int j = 0; j < (positions.Count()) / 2 + 1; j++)
            {
                if (positions.Count() == 1)
                {
                    individ.Activities[positions[j]] = activities[positions.Count() - j - 1];
                }
                else if (positions.Count() > 1)
                {
                    individ.Activities[positions[j]] = activities[positions.Count() - j - 1];
                    individ.Activities[positions[positions.Count() - j - 1]] = activities[j];
                }
                // individ.Activities[positions.Count() - j - 1] = activities[j];
            }
            individ.Fitness = ComputeFitness(individ.Activities.ToArray());
            individ.NumberOfActivities = individ.Activities.Count();
            return individ;
        }






        private Individ LKH(Individ Xt)
        {
            int size = Xt.NumberOfActivities;
            var currentDay = Xt.Activities;

            double dayFitness = Xt.Fitness;
            for (int i = 0; i < size - 1; i++)
            {
                for (int k = i + 1; k < size; k++)
                {
                    if (i == k)
                    {
                        continue;
                    }
                    var nextDay = Swap(i, k, currentDay.ToArray(), probabilities);
                    var nextDayFitness = ComputeFitness(nextDay);

                    if (nextDayFitness > dayFitness)
                    {

                        currentDay = nextDay.ToList();
                        dayFitness = nextDayFitness;
                    }
                }
            }

            return new Individ { Activities = currentDay, Fitness = dayFitness, NumberOfActivities = currentDay.Count() };
        }
        private Details[] Swap(int i, int j, Details[] activities, List<Probability> probabilities)
        {
            if (i <= 0 || j >= activities.Count() - 2 || activities[i].Activity == activities[j].Activity) return activities;
            if (probabilities.FirstOrDefault(x => x.Activity == activities[j].Activity).Next.FirstOrDefault(y => y.Activity == activities[i].Activity).ProbNext != 0 &&
                probabilities.FirstOrDefault(x => x.Activity == activities[i - 1].Activity).Next.FirstOrDefault(y => y.Activity == activities[j].Activity).ProbNext != 0 &&
                probabilities.FirstOrDefault(x => x.Activity == activities[i].Activity).Next.FirstOrDefault(y => y.Activity == activities[j + 1].Activity).ProbNext != 0)
            {
                var aux = activities[i];
                activities[i] = activities[j];
                activities[j] = aux;
            }

            return activities;

        }

        private Individ DeltaXt(Individ Xrabbit, Individ Xt)
        {
            Individ child = new Individ();
            if (Xrabbit.NumberOfActivities > Xt.NumberOfActivities) // Xt - donor, Xrabbit- receiver
            {
                int i = MinProbability(Xt.Activities, probabilities).i + 1;
                child.Activities = Xt.Activities.TakeWhile((act, index) => index < i).ToList();
                for (int j = i; j < Xrabbit.Activities.Skip(i).Count(); j++)
                {
                    if (probabilities.FirstOrDefault(x => x.Activity == child.Activities[j - 1].Activity).Next.FirstOrDefault(y => y.Activity == Xrabbit.Activities[j].Activity).ProbNext != 0 || Xt.Activities.Count() <= j ||
                        Xrabbit.Activities[j].Activity != child.Activities[j - 1].Activity)
                    {
                        child.Activities.Add(Xrabbit.Activities[j]);
                    }
                    else
                    {

                        child.Activities.Add(Xt.Activities[j]);
                    }
                }
            }
            else if (Xrabbit.NumberOfActivities <= Xt.NumberOfActivities) // Xrabbit - donor, Xt- receiver
            {
                int i = MinProbability(Xrabbit.Activities, probabilities).i + 1;
                child.Activities = Xrabbit.Activities.TakeWhile((act, index) => index < i).ToList();
                for (int j = i; j < Xt.Activities.Skip(i).Count(); j++)
                {
                    if (probabilities.FirstOrDefault(x => x.Activity == child.Activities[j - 1].Activity).Next.FirstOrDefault(y => y.Activity == Xt.Activities[j].Activity).ProbNext != 0 || Xrabbit.Activities.Count() <= j || Xrabbit.Activities[j].Activity != child.Activities[j - 1].Activity)
                    {
                        child.Activities.Add(Xt.Activities[j]);
                    }
                    else
                    {
                        child.Activities.Add(Xrabbit.Activities[j]);
                    }
                }


            }
            child.Fitness = ComputeFitness(child.Activities.ToArray());
            child.NumberOfActivities = child.Activities.Count();

            return child;

        }

        private MinimProbabilityModel MinProbability(List<Details> activities, List<Probability> probabilities)
        {
            MinimProbabilityModel minim = new MinimProbabilityModel { Minimum = 1 };

            for (var i = 1; i < activities.Count() - 1; i++)
            {
                double probability = 0;
                List<Probability> nextProbabilities = probabilities.FirstOrDefault(x => x.Activity == activities[i].Activity).Next.Where(y => y.ProbNext != 0).ToList();
                if (probabilities.FirstOrDefault(x => x.Activity == activities[i].Activity).Next.FirstOrDefault(y => y.Activity == activities[i + 1].Activity) != null)
                {

                    var choose = rnd.Next(0, nextProbabilities.Count);
                    probability = probabilities.FirstOrDefault(x => x.Activity == activities[i].Activity).Next[choose].ProbNext;
                }
                if (probability < minim.Minimum && probability != 0)
                {
                    minim.Minimum = probability;
                    minim.i = i;
                    return minim;

                }
            }
            return minim;
        }

        private Individ JXrabbit(Individ Xrabbit)
        {

            var min = MinProbability(Xrabbit.Activities, probabilities).i;
            if (min == 0)
            {
                var x = probabiltyStarts.OrderByDescending(i => i.Probability).First();
                if (x.ActivityName == Xrabbit.Activities[min].Activity)
                {
                    min = MinProbability(Xrabbit.Activities.Skip(min).ToList(), probabilities).i + 1;

                }


            }
            var activities = Swap(min - 1, min, Xrabbit.Activities.ToArray(), probabilities);
            return new Individ { Activities = activities.ToList(), NumberOfActivities = activities.Count(), Fitness = ComputeFitness(activities) };
        }

        private double ComputeEnergy(int t, int T, double minus)
        {
            double E0 = rnd.NextDouble() * 2 - 1;
            var E = 2 * E0 * (1 - (t / (double)T));

            return E;

        }

        private Individ YMutation(Individ Y)
        {
            var min = MinProbability(Y.Activities, probabilities).i;
            if (min == 0)
            {
                var x = probabiltyStarts.OrderByDescending(i => i.Probability).First();
                if (x.ActivityName == Y.Activities[min].Activity)
                {
                    min = MinProbability(Y.Activities.Skip(1).ToList(), probabilities).i + 1;

                }
            }
            else
            {
                if (min == Y.NumberOfActivities - 1)
                {
                    var z = probabiltyEnds.Max();
                    if (z.ActivityName == Y.Activities[min].Activity)
                    {
                        min = MinProbability(Y.Activities.Take(Y.NumberOfActivities - 1).ToList(), probabilities).i;

                    }

                }
            }
            var Act = SwapWithMaxProb(min, Y.Activities.ToArray(), probabilities).ToList();
            return new Individ { Activities = Act, NumberOfActivities = Act.Count(), Fitness = ComputeFitness(Act.ToArray()) };

        }

        private Details[] SwapWithMaxProb(int i, Details[] activities, List<Probability> probabilities)
        {
            var Pa = probabilities.FirstOrDefault(y => y.Activity == activities[i].Activity).Next.FirstOrDefault(l => l.Activity == activities[i + 1].Activity).ProbNext;
            var a1 = probabilities.Select(y => y.Next.FirstOrDefault(s => s.Prob >= Pa)).First();
            activities[i].Activity = a1.Activity;
            return activities;
        }

        double cosine_similarity(int[] A, int[] B, int Vector_Length)
        {
            double dot = 0.0, denom_a = 0.0, denom_b = 0.0;
            for (int i = 0; i < Vector_Length; ++i)
            {
                dot += A[i] * B[i];
                denom_a += A[i] * A[i];
                denom_b += B[i] * B[i];
            }
            return dot / (Math.Sqrt(denom_a) * Math.Sqrt(denom_b));
        }

        public double CosineSimilarity(Individ routine, Details[] newIndivid)
        {
            int index = 0;
            var length = Math.Max(routine.Activities.Count(), newIndivid.Count());
            int[] routineInts = new int[length];
            int[] individInts = new int[length];
            foreach (var r in routine.Activities)
            {
                byte[] asciiBytes = Encoding.ASCII.GetBytes(r.Activity);
                int total = 0;
                Array.ForEach(asciiBytes, delegate (byte i) { total += i; });
                routineInts[index] = total;
                index++;

            }
            index = 0;
            foreach (var re in newIndivid)
            {
                byte[] asciiBytes = Encoding.ASCII.GetBytes(re.Activity);
                int total = 0;
                Array.ForEach(asciiBytes, delegate (byte i) { total += i; });
                individInts[index] = total;
                index++;
            }

            var cosine = cosine_similarity(routineInts, individInts, length);

            return cosine;
        }
        public double CosineSimilarity2(Individ routine, Details[] newIndivid)
        {
            int index = 0;
            var length = Math.Max(routine.Activities.Count(), newIndivid.Count());
            string routineInts = "";
            string individInts = "";
            foreach (var r in routine.Activities)
            {
                routineInts += r.Activity;
            }
            foreach (var r in newIndivid)
            {
                individInts += r.Activity;
            }
            var rx = CalculateSimilarity(individInts, routineInts);
            return rx;
        }

        int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

        double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }




        #endregion
    }


}

