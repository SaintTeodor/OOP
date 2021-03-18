namespace GrandPrixTT.Tyres
{

    using System;


    public abstract class Tyre
    {
        private string name;
        private double hardness;
        private double degreadation;

        protected Tyre(string name, double hardness)
        {
            this.Name = name;
            this.Hardness = hardness;
            this.Degradation = 100;

        }


        public virtual double Degradation
        {
            get
            {
                return this.degreadation;
            }
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Blown tire.");
                }
                this.degreadation = value;
            }
        }

        public double Hardness
        {
            get
            {
                return this.hardness;
            }
            private set
            {
                this.hardness = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                this.name = value;
            }
        }

        public virtual void TyreWearState()
        {
            this.Degradation -= this.Hardness;
        }
    }
}

