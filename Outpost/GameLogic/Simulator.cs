using CommonCode;
using Outpost.FileHandling;
using System.Collections.Generic;

namespace Outpost.GameLogic
{
    class Simulator
    {
        public Tile[,,] map;
        public TileData[] TileTypes;
        public Colony[] Factions;
        public int PlayerColony = 0;
        public string[] mapPaths = new string[] { ".//Old Content//Maps//500.png", ".//Old Content//Maps//500A.png" };

        public Simulator()
        {
            StartMap(".//Old Content//Maps//500A.png");
        }

        public void StartMap(string mapPath)
        {
            List<TileData> data = new List<TileData>(ContentProcessing.LoadTerrainData(".//Content//XML//Terrain.xml", ScreenManager.Content));
            data.AddRange(ContentProcessing.LoadBuildingData(".//Content//XML//Buildings.xml", ScreenManager.Content));
            TileTypes = data.ToArray();
            map = ContentProcessing.LoadTileMap(mapPath, new ContentProcessing.MapToTileData(0, 1, 2, 3, 4, 5, 6, 7, 8, 9), 3, ScreenManager.Content);

            map[3, 3, 0].ID = 71;
        }

        public void RunTurn() { }
    }
}
