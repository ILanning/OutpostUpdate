using System;
using CommonCode.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CommonCode.UI;
using CommonCode;
using Outpost.GameLogic;
using CommonCode.Drawing;

namespace Outpost
{
    class ConstructSelectElement : Element, IDataDisplay, IInputElement
    {

        int selectedBuilding = 0;
        TileData[] tileSet;
        int[] unlockedTiles;
        int[] buildableTiles;

        DisplayMode mode;
        ScrollBar scroll;
        int rowHeight;
        string[,] list;
        Rectangle selectBox;
        Simulator sim;


        /// <summary>
        /// Returns the ID of the currently selected building every time a new selection is made.
        /// </summary>
        public event EventHandler<BuildingSelectedEventArgs> BuildingSelected;

        public ConstructSelectElement(Coordinate minSize, Coordinate maxSize, Vector2 proportions, SideTack attachment, Simulator sim)
        {
            this.IdealDimensions = proportions;
            this.MaximumSize = maxSize;
            MinimumSize = minSize;
            this.SideAttachment = attachment;
            this.sim = sim;
        }

        public override void Move(Coordinate movement)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        void recalculateAvailable(BuildingResourceManager resources)
        {
            //Ask for list of unlocked buildings (buildings include resources to build and discounts)
            TileData[] unlocked = new TileData[sim.Factions[sim.PlayerColony].unlockedBuildings.Length];
            for (int i = 0; i < unlocked.Length; i++)
                unlocked[i] = sim.TileTypes[sim.Factions[sim.PlayerColony].unlockedBuildings[i]];

            //Ask for a list of available resources, decide how many buildings may be made

            //Bind to resources changed event

            //maintain selected building if possible
            //scroll to keep selected building in view if list changes
            int[] result = null;
            //Iterate over list of buildings and figure out how many of each could be built assuming all resources are spent on that one building
            

            buildableTiles = result;
        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }

        public override void Resize(Rectangle targetSpace)
        {
            //if size is greater than
            throw new NotImplementedException();
        }

        public bool BindData(DataProvider provider)
        {
            throw new NotImplementedException();
        }

        public void UpdateData()
        {
            throw new NotImplementedException();
        }


        public string[] Events
        {
            get { throw new NotImplementedException(); }
        }

        public void HandleInput()
        {
            throw new NotImplementedException();
        }

        public void OnForward(object sender, EventArgs e)
        {
            selectedBuilding++;
            if (selectedBuilding >= buildableTiles.Length)
                selectedBuilding = 0;
        }

        public void OnBack(object sender, EventArgs e)
        {
            selectedBuilding--;
            if (selectedBuilding > 0)
                selectedBuilding = buildableTiles.Length - 1;
        }
        public void OnChangeBuildingGroup(object sender, EventArgs e)
        {

        }

        //Has two parts: A list of buildable tiles and a detail pane for the selected item from that list
        private class ConstructListElement
        {
            //Array of key requirement pairs
            //Value one is a string identifying a resource icon/the icon sprite
            //Value two is an integer for the quantity of that icon

            public int Tile;
            
            public ConstructListElement(ResourcesDef[] resources, int[] resourcesCount, Sprite image, int tile)
            {
                Tile = tile;
            }

            public void Update()
            {

            }

            public void Draw(SpriteBatch sb)
            {

            }

            enum CLEState
            {
                Disabled = 0,
                Enabled,
                Hovered,
                Selected,
                Clicked
            }
        }
    }


    public class BuildingSelectedEventArgs : EventArgs
    {
        public int ID;
        
        public BuildingSelectedEventArgs(int id)
        {
            ID = id;
        }
    }
}
