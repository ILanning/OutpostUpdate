using System;
using System.Collections.Generic;

namespace Outpost.GameLogic
{
    class BuildingResourceManager
    {
        LinkedList<Dictionary<string, uint>> resourceHistory; //Table of resource count per turn
        LinkedList<Dictionary<int, Dictionary<string, uint>>> buildingExpendatureHistory;  //Table of resources spent per building per turn
        /// <summary>
        /// Alls sets of discounts applied to individual buildings
        /// </summary>
        Dictionary<TileData, ModSet> constructionDiscountSets = new Dictionary<TileData, ModSet>();
        /// <summary>
        /// The total discounts of the sets described in constructionDiscountSets
        /// </summary>
        Dictionary<TileData, float> constructionDiscounts = new Dictionary<TileData, float>();
        Dictionary<string, ModSet> resourceDiscountSets = new Dictionary<string, ModSet>();
        Dictionary<string, float> resourceDiscounts = new Dictionary<string, float>();


        public Dictionary<string, uint> GetResources(SeverityLevel severity)
        {
            Dictionary<string, uint> result = new Dictionary<string, uint>();
            throw new NotImplementedException();
        }

        public Dictionary<string, uint> GetResources(string[] resourceNames, SeverityLevel severity)
        {
            Dictionary<string, uint> result = new Dictionary<string, uint>();
            throw new NotImplementedException();
        }

        public int CountConstructable(TileData building, SeverityLevel severity)
        {
            float bDiscount = constructionDiscounts[building];
            int result = int.MaxValue;
            for(int i = 0; i < building.reqs.Length; i++)
            {
                float cost = building.reqs[i].Count * bDiscount * resourceDiscounts[building.reqs[i].Name];
                int count = (int)(resourceHistory.First.Value[building.reqs[i].Name] / cost);
                if (count < result)
                    result = count;
            }
            return result;
        }

        public void ApplyDiscount(TileData building, string discountName, float percentage)
        {
            
        }
        /*
        Resource system:

            Premise:
                Buildings create, store, and consume resources
                Resources are shared instantly between interconnected buildings, but must be transported between unconnected groups
                Some resources are not consumed by tasks, but are merely "rented out" (e.g. bulldozers, people)
                Resource costs can change due to research and events
                The player needs to be warned abou certain situations arising with the resources in a building group
                Data about resource flow should be gathered and stored

            Goals:
                Track all of the resources available to a building group
                Manage storage and processing of resources in an efficient manner
                Provide an interface for things to ask for/give resources to the building group through
                Provide a means to trade with resource systems that belong to other building groups
                Provide a means to access data about resource flow in the past
                Provide a means to access warnings about potential resource issues, with severity levels
            
            Class Blockout:
                -Table tracking resource counts this turn/past turns   LinkedList of arrays of ResourceDefs + int
                -Table tracking expendatures by building               LinkedList of objects
                -List of resource modifiers                            Dictionary of arrays

                Properties:
                    AvailableResources //Ignores resources needed for maintanence
                    AllResources       //Includes everything the group has
                    //Perhaps make these two into a function that takes a severity level and returns the resources available at that level

                Functions:
                    bool RemoveResources(ResourceList, BuildingDef)
                    bool AddResources(ResourceList, BuildingDef)
                    bool SendResources(ResourceList, BuildingGroup)
                    void SubtractMaintainence()
                    ResourceList GetResources(ResourceList, Severity)
                    void SetResources(ResourceList)
                    void ApplyDiscount(ResourceList, DegreeList, Length)
                    void ApplyDiscount(BuildingList, ResourceList, DegreeList, Length)
                    void ApplyBoost(ResourceList, DegreeList, Length)
                    void ApplyBoost(BuildingList, ResourceList, DegreeList, Length)
                    Dictionary<string, uint> buildBuildingCost(BuildingDef)

         */
        /// <summary>
        /// Defines the degree to which the resources are needed.  At the highest level all buildings will be maintained (on this turn).  On the lowest food, air, and power may fail to get what they need.
        /// </summary>
        public enum SeverityLevel
        {
            FullMaintanence = 0,
            MostMaintanence,
            ModerateMaintanence,
            MinimalMaintanence,
            NoMaintanence
        }

        class ModSet
        {
            /*public string[] ids;
            public float[] percentage;

            public float this[string id]
            {
                get
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (ids[i] == id)
                            return percentage[i];
                    }
                    return 1.0f;
                }
                set
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (ids[i] == id)
                            percentage[i] = value;
                    }
                }
            }*/
            Dictionary<string, float> modifiers;

            public ModSet()
            {
                modifiers = new Dictionary<string, float>();
            }

            public ModSet(string[] names, float[] values)
            {
                if (names.Length != values.Length)
                    throw new ArgumentException("number of elements in names and values must match");
                modifiers = new Dictionary<string, float>(names.Length);
                for(int  i = 0; i < names.Length; i++)
                {
                    modifiers[names[i]] = values[i];
                }
            }

            public float Compile()
            {
                float result = 1.0f;
                //ValuesCollection values = modifiers.Values;
                for (int i = 0; i < modifiers.Values.Count; i++)
                {
                    //result *= modifiers.
                }
                return 0;
            }
        }

        //Just hides a dictionary for now, but may change in the future and keeps things clear in the meantime
        //TODO: Rework or remove this class once the resource manager design settles out
        class ResourceSet
        {
            Dictionary<string, int> storage;

            public int this[string id]
            {
                get { return storage[id]; }
                set { storage[id] = value; }
            }

            public ResourceSet(Dictionary<string, int> set)
            {
                storage = set;
            }
            public ResourceSet(ResourceCount[] set)
            {
                Dictionary<string, int> result = new Dictionary<string, int>(set.Length);
                foreach (ResourceCount item in set)
                    result[item.Name] = item.Count;
            }

            public static implicit operator Dictionary<string, int>(ResourceSet set)
            {
                return set.storage;
            }

            public static implicit operator ResourceSet(Dictionary<string, int> set)
            {
                return new ResourceSet(set);
            }
        }
    }
}
