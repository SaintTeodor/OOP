namespace GrandPrixTT.Factories
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using GrandPrixTT.Tyres;

    public class TyreFactory
    {
        public static Tyre TyresPit(List<string> commandArgs)
        {
            string tyreType = commandArgs[0];
            switch (tyreType)
            {
                case "Hard":
                    return new HardTyre(double.Parse(commandArgs[1]));

                case "Ultrasoft":
                    return new UltrasoftTyre(double.Parse(commandArgs[1]), double.Parse(commandArgs[2]));

                default:
                    throw new ArgumentException("Invalid tyre type.");
            }
        }
    }
}
