using System.Drawing;

namespace TanksLib
{
    public class Tank
    {
        public string name { get; set; }
        public int hp { get; set; }
        public int speed { get; set; }
        public int id { get; set; }
        public Point cords { get; set; }
        public Rotate rotate { get; set; }
    }
}
