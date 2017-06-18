using System;

namespace Outpost.GameLogic
{
    class Colony
    {
        /// <summary>
        /// Array of all BuildingGroups that this faction is in control of.
        /// </summary>
        public BuildingGroup[] ownedBuildings;
        /// <summary>
        /// Array of the indices of the buildings this faction has unlocked.
        /// </summary>
        public int[] unlockedBuildings;

        public Colony()
        {


        }
    }
}
