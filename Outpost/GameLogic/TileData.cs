using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Outpost.GameLogic
{
    /// <summary>
    /// Physical and graphical data about a tile type.
    /// </summary>
    public class TileData
    {
        public int ID;
        public string Name;
        public Texture2D Spritesheet;
        public Rectangle SampleArea;
        public ResourceCount[] reqs;
    }

    public class BuildingData : TileData
    {
        public short Lifespan;
    }
}
