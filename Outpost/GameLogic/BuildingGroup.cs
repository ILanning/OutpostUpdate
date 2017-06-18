using System;
using System.Collections.Generic;

namespace Outpost.GameLogic
{
    /// <summary>
    /// Represents a single grouping of buildings connected by tubes.  May consist of a single building.
    /// </summary>
    class BuildingGroup
    {
        //Contains resources
        //Contains buildings
        //Trades with neighboring BuildingGroups
        //Limited number of trade routes?
        //Trade capacity limited by available trucks
        public Dictionary<ColonyResources, int> AvailableResources;

        public BuildingGroup()
        {
            AvailableResources = new Dictionary<ColonyResources, int>();
            AvailableResources.Add(ColonyResources.MetalA, 1000);
            AvailableResources.Add(ColonyResources.OrganicA, 1000);
            AvailableResources.Add(ColonyResources.OreB, 1000);
        }
    }
}
