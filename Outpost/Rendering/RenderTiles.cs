using CommonCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Outpost.GameLogic;

namespace Outpost
{
    class RenderTiles
    {
        static TileData CurrentDozed;
        /// <summary>
        /// Draws the given tile at the given location.
        /// </summary>
        /// <param name="position">Where the bottom-center point of the tile will be.</param>
        /// <param name="tile">The specific tile to draw.</param>
        /// <param name="data">The general data about the sort of tile being drawn.</param>
        /// <param name="dozed">The dozed tile type to use.</param>
        public static void Draw(Coordinate position, Tile tile, TileData data, TileData dozed, SpriteBatch sb)
        {
            if (tile.Revealed)
            {
                if (!(data is BuildingData))
                {
                    Coordinate origin = new Coordinate(data.SampleArea.Width / 2, data.SampleArea.Height);
                    sb.Draw(data.Spritesheet, position - origin, data.SampleArea, Color.White);
                }
                else
                {
                    Coordinate origin = new Coordinate(dozed.SampleArea.Width / 2, dozed.SampleArea.Height);
                    sb.Draw(dozed.Spritesheet, position - origin, dozed.SampleArea, Color.White);
                    origin = new Coordinate(data.SampleArea.Width/2, data.SampleArea.Height);
                    sb.Draw(data.Spritesheet, position - origin, data.SampleArea, Color.White);
                }
            }
        }

        /// <summary>
        /// Draws a generic version of the given tile type using the currently set base dozed tile.
        /// </summary>
        /// <param name="position">Where the bottom-center point of the tile will be.</param>
        /// <param name="data"></param>
        /// <param name="sb"></param>
        public static void Draw(Coordinate position, TileData data, SpriteBatch sb)
        {
            if (!(data is BuildingData))
            {
                Coordinate origin = new Coordinate(data.SampleArea.Width / 2, data.SampleArea.Height);
                sb.Draw(data.Spritesheet, position - origin, data.SampleArea, Color.White);
            }
            else
            {
                Coordinate origin = new Coordinate(CurrentDozed.SampleArea.Width / 2, CurrentDozed.SampleArea.Height);
                sb.Draw(CurrentDozed.Spritesheet, position - origin, CurrentDozed.SampleArea, Color.White);
                origin = new Coordinate(data.SampleArea.Width / 2, data.SampleArea.Height);
                sb.Draw(data.Spritesheet, position - origin, data.SampleArea, Color.White);
            }
        }
    }
}
