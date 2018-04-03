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
        public double T { get; set; }
		public override string ToString()
		{
            return Utility.ToString<Room>(this);
		}
    }
}
