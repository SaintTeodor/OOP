namespace GrandPrixTT
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using GrandPrixTT.Drivers;
    using GrandPrixTT.Factories;
    using GrandPrixTT.Tyres;


    public class RaceTower
    {
        private const double TIME_IN_PIT = 20;

        private Dictionary<string, Driver> drivers;
        private Dictionary<Driver, string> didNotFinishDrivers;
        private Track trackInfo;

        public bool isWon;
        public Driver racer;


        public RaceTower()
        {
            drivers = new Dictionary<string, Driver>();
            didNotFinishDrivers = new Dictionary<Driver, string>();
            isWon = false;
        }

        public void SetTrackInfo(int lapsNumber, int trackLength)
        {
            trackInfo = new Track(lapsNumber, trackLength);
        }


        public void RegisterDriver(List<string> commandArgs)
        {
            try
            {
                Tyre tyre = TyreFactory.TyresPit(commandArgs.Skip(4).ToList());
                Car car = CarFactory.AddCar(commandArgs.Skip(2).Take(2).ToList(), tyre);
                Driver driver = DriverFactory.AddDriver(commandArgs.Take(2).ToList(), car);
                drivers.Add(commandArgs[1], driver);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
        }


        public void DriverBoxes(List<string> commandArgs)
        {
            string reasonToBox = commandArgs[0];
            Driver driver = drivers[commandArgs[1]];
            driver.TotalTime += TIME_IN_PIT;
            switch (reasonToBox)
            {
                case "ChangeTyres":
                    Tyre newTyre = TyreFactory.TyresPit(commandArgs.Skip(2).ToList());
                    driver.Car.ChangeTyres(newTyre);
                    break;

                case "Refuel":
                    driver.Car.Refuel(double.Parse(commandArgs[2]));
                    break;
            }
        }

        public string CompleteLaps(List<string> commandArgs)
        {
            int completeLap = int.Parse(commandArgs[0]);
            if (trackInfo.TotalLaps - completeLap < 0)
            {
                return $"There is no time! On lap {trackInfo.CurrentLap}.";


            }
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < completeLap; i++)
                {

                    GetDriversTime();
                    DriversTakeALap();
                    RemoveDidNotFinishDrivers();
                    trackInfo.CurrentLap++;

                    List<Driver> standings = drivers.Values.OrderByDescending(d => d.TotalTime).ToList();
                    Overtaking(standings, sb);
                }

                if (trackInfo.CurrentLap == trackInfo.TotalLaps)
                {
                    isWon = true;
                    racer = drivers.Values.OrderBy(d => d.TotalTime).First();
                }
                return sb.ToString().Trim();
            }
        }


        private void Overtaking(List<Driver> standings, StringBuilder sb)
        {
            for (int i = 0; i < standings.Count - 1; i++)
            {
                Driver frontDriver = standings[i];
                Driver behindDriver = standings[i + 1];
                double gap = Math.Abs(frontDriver.TotalTime - behindDriver.TotalTime);
                int interval = 2;
                bool hasCrashed = CheckTrackConditions(frontDriver, interval);

                if (gap <= interval)
                {
                    if (hasCrashed)
                    {
                        didNotFinishDrivers.Add(frontDriver, "Crashed");
                        drivers.Remove(frontDriver.Name);
                        continue;
                    }
                    frontDriver.TotalTime -= interval;
                    behindDriver.TotalTime += interval;
                    sb.AppendLine($"{frontDriver.Name} has overtaken {behindDriver.Name} on lap {trackInfo.CurrentLap}.");
                }
            }
        }


        private bool CheckTrackConditions(Driver chasingDriver, int interval)
        {
            bool hasCrashed = false;

            if (chasingDriver.GetType().Name == "EnduranceDriver" && chasingDriver.Car.Tyre.GetType().Name == "HardTyre")
            {
                interval = 3;
                if (trackInfo.Weather == "Rainy")
                {
                    hasCrashed = true;
                }
            }

            if (chasingDriver.GetType().Name == "AggressiveDriver" && chasingDriver.Car.Tyre.GetType().Name == "UltrasoftTyre")
            {
                interval = 3;
                if (trackInfo.Weather == "Foggy")
                {
                    hasCrashed = true;
                }
            }

            return hasCrashed;
        }

        private void RemoveDidNotFinishDrivers()
        {
            foreach (Driver retiredDriver in didNotFinishDrivers.Keys)
            {
                if (drivers.ContainsKey(retiredDriver.Name))
                {
                    drivers.Remove(retiredDriver.Name);
                }
            }
        }

        private void DriversTakeALap()
        {
            foreach (Driver driver in drivers.Values)
            {
                try
                {
                    driver.ReduceFuelAmount(trackInfo.Length);
                    driver.Car.Tyre.TyreWearState();
                }
                catch (ArgumentException ae)
                {
                    didNotFinishDrivers.Add(driver, ae.Message);
                }
            }
        }

        private void GetDriversTime()
        {
            foreach (Driver driver in drivers.Values)
            {
                driver.TotalTime += 60 / (trackInfo.Length / driver.Speed);
            }
        }

        public string GetLeaderboard()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Lap {trackInfo.CurrentLap}/{trackInfo.TotalLaps}");

            int position = 1;
            foreach (Driver driver in drivers.Values.OrderBy(d => d.TotalTime))
            {
                sb.AppendLine($"{position} {driver.Name} {driver.TotalTime:F3}");
                position++;
            }
            foreach (KeyValuePair<Driver, string> driver in didNotFinishDrivers.Reverse())
            {
                sb.AppendLine($"{position} {driver.Key.Name} {driver.Value}");
                position++;
            }
            return sb.ToString().Trim();
        }

        public void ChangeWeather(List<string> commandArgs)
        {
            trackInfo.Weather = commandArgs[0];
        }
    }
}
