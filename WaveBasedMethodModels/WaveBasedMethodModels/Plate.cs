using System;
namespace WaveBasedMethodModel
{
    public class Plate
    {
        public double L_x { get; set; }
        public double L_y { get; set; }
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }
        public double Rho { get; set; } // density
        public double H { get; set; } // thickness
        public double B { get; set; } // bending stiffness
        public override string ToString()
        {
            return Utility.ToString<Plate>(this);
        }
    }
}
