namespace GrandPrixTT
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;


    public class Engine
    {
        private RaceTower raceTower;

        public Engine()
        {
            this.raceTower = new RaceTower();
        }

        public void Run()
        {
            int numberOfLaps = int.Parse(Console.ReadLine());
            int trackLength = int.Parse(Console.ReadLine());
            raceTower.SetTrackInfo(numberOfLaps, trackLength);

            string input = Console.ReadLine();
            while (true)
            {
                string result = string.Empty;
                List<string> inputParts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string command = inputParts[0];
                inputParts.Remove(inputParts[0]);
                switch (command)
                {
                    case "RegisterDriver":
                        raceTower.RegisterDriver(inputParts);
                        break;
                    case "Leaderboard":
                        Console.WriteLine(raceTower.GetLeaderboard());
                        break;
                    case "CompleteLaps":
                        result = raceTower.CompleteLaps(inputParts);
                        break;
                    case "Box":
                        raceTower.DriverBoxes(inputParts);
                        break;
                    case "ChangeWeather":
                        raceTower.ChangeWeather(inputParts);
                        break;
                }

                if (result != string.Empty)
                {
                    Console.WriteLine(result);
                }

                if (raceTower.isWon)
                {
                    Console.WriteLine($"{raceTower.racer.Name} wins the race for {raceTower.racer.TotalTime:F3} seconds.");
                    return;
                }
                input = Console.ReadLine();
            }
        }
    }
}

