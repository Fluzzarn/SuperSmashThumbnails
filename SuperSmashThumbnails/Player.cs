using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperSmashBrosThumbnails
{

    public enum Direction
    {
        Left,
        Right
    }


    public class Player
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public Direction Facing { get; set; }
        public Player()
        {

        }
    }
}
