using CommonCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Outpost.GameLogic;

namespace Outpost
{
    /// <summary>
    /// A set of temporary globals since file loading isn't set up completely yet.  Only for things that will eventually be loaded via XML.
    /// </summary>
    class TempGlobals
    {                                                              //199, 119, 55             43, 144, 0
        public static Color[] BorderColors = new Color[] { new Color(199, 119, 55), new Color(90, 105, 190), new Color(200, 200, 200) };
        public static Coordinate TileSize = new Coordinate(106, 46);
        public static BuildingReqs[] BuildingReqSprites;

    }

    public class DummyData
    {
        public static ResourceCount[] Resources
        {
            get
            {
                return null;
            }
        }
    }

    public class BuildingReqs
    {
        public Texture2D spritesheet;
        public Rectangle[] sheetSources;
        public int[] baseReqs;

        public void init(DynamicContentManager content)
        {
            //content.Load<Texture2D>(".//")
        }
    }

    public struct ResourceCount
    {
        public string Name;
        public int Count;        
    }
}
