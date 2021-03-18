namespace GrandPrixTT.Drivers
{
    
    public class AggressiveDriver : Driver
    {
        private const double aggressiveDriversFuel = 2.7;
        private const double aggressiveDriversSpeed = 1.3;
        public AggressiveDriver(string name, Car car)
            : base(name, car, aggressiveDriversFuel)
        {

        }

        public override double Speed => base.Speed * aggressiveDriversSpeed;
    }
}
