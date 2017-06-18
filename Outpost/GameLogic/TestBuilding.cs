using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outpost
{
    class TestBuilding
    {
        //Values to display:
        /*General Building Stats:
         * Name (Displayed in Title)
         * Type
         * Status (Operational, Offline, Damaged, etc.)
         * Powered
         * Staff
         * Age
         * Health
         */

        public virtual string[,] GetBasicData()
        { 
            string[,] result = new string[7,2];

            result[0, 0] = "Name"; result[0, 1] = "Test Building 01";
            result[1, 0] = "Type"; result[1, 1] = "Test Building";
            result[2, 0] = "Status"; result[2, 1] = "Offline";
            result[3, 0] = "Power"; result[3, 1] = "0/2 kWh";
            result[4, 0] = "Staff"; result[4, 1] = "Test Building";
            result[5, 0] = "Age"; result[5, 1] = "Test Building";
            result[6, 0] = "Health"; result[6, 1] = "Test Building";

            return result;
        }
    }

    class TestResidence : TestBuilding
    {
        
    }

    class TestStorage : TestBuilding
    { 
        //Values to display:
        /*Warehouse/Storage Tank Stats:
         *Inventory:
         * Object Type
         * Count
         * Volume
         */
    }

    class TestMine : TestBuilding
    { 
        
    }
}
