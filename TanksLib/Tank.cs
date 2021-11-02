using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
        public int bulletref { get; set; }
        public static bool CheckCollision(List<Tank> tanks, Point pos, int size)
        {
            bool res = false;
            size = size + 1;
            if(tanks != null && tanks.Count > 0)
            {
                Rectangle position = new Rectangle(pos, new Size(size, size));
                res = tanks.Where(x => {
                    if (x != null && x.cords != pos)
                    { 
                        return position.IntersectsWith(new Rectangle(x.cords, new Size(size, size))); 
                    } 
                    else return false; 
                }).Count() > 0;
            }   
            return !res;
        }
    }
}
