using System;
using System.Text;
using System.Collections.Generic;
namespace WaveBasedMethodModel
{
    public class Input
    {
        public Input()
        {
            _rooms = new List<Room>();
            _plates = new List<Plate>();
        }
        public void Add(Room room)
        {
            _rooms.Add(room);
        }
        public void Add(Plate plate)
        {
            _plates.Add(plate);
        }

        // 
        public override string ToString()
        {
            var res = new StringBuilder();
            res.Append("Input:\nrooms = ");
            foreach (var room in _rooms)
            {
                res.Append(room.ToString());
            }
            res.Append("\nplates = ");
            foreach (var plate in _plates)
            {
                res.Append(plate.ToString());
            }
            return res.ToString();
        }
        // DATA
        private List<Room> _rooms;
        private List<Plate> _plates;
    }
}
