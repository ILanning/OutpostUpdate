using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outpost.GameLogic
{
    /// <summary>
    /// Represents everything that would belong to a specific group, such as all play controlled forces.
    /// </summary>
    class Faction
    {
        //Contains colonies
        //Contains research

        /// <summary>
        /// Returns an array of index numbers for each buildable tile in the tile list
        /// </summary>
        public int[] UnlockedBuildings
        {
            get { return unlockedBuildings; }
            set { unlockedBuildings = value; }
        }

        int[] unlockedBuildings;
    }
}
