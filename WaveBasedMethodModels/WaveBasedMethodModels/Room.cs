using System;
using System.ComponentModel;
using System.Text;

namespace WaveBasedMethodModel
{
    public class Room
    {
        public bool IsSource { get; set; } = false;
        public bool IsReceiving { get; set; } = false;
        public double L_x { get; set; }
        public double L_y { get; set; }
        public double L_z { get; set; }
        // X_s, Y_s and Z_s are only applicable to source room
        public double X_s { get; set; } = 0.0;
        public double Y_s { get; set; } = 0.0;
        public double Z_s { get; set; } = 0.0;
        public double T { get; set; }
		public override string ToString()
		{
            return Utility.ToString<Room>(this);
		}
    }
}
