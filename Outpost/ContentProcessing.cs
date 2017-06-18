using CommonCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Outpost.GameLogic;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Outpost.FileHandling
{
    class ContentProcessing
    {
        public static TileData[] LoadTerrainData(string path, DynamicContentManager content)
        {
            TileDataLoader[] unprocessedTileData;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TileDataCont));
            StreamReader streamReader = new StreamReader(path);
            unprocessedTileData = ((TileDataCont)xmlSerializer.Deserialize(streamReader)).TileDefinitions;
            streamReader.Close();

            TileData[] result = new TileData[unprocessedTileData.Length];
            for (int i = 0; i < result.Length; i++)
            {
                Texture2D tileTexture = content.Load<Texture2D>(unprocessedTileData[i].Spritesheet);
                result[i] = new TileData { ID = unprocessedTileData[i].ID, Name = unprocessedTileData[i].Name, Spritesheet = tileTexture, SampleArea = unprocessedTileData[i].SampleArea };
            }
            return result;
        }
        public static TileData[] LoadBuildingData(string path, DynamicContentManager content)
        {
            BuildingDataLoader[] unprocessedTileData;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BuildingDataCont));
            StreamReader streamReader = new StreamReader(path);
            unprocessedTileData = ((BuildingDataCont)xmlSerializer.Deserialize(streamReader)).TileDefinitions;
            streamReader.Close();

            BuildingData[] result = new BuildingData[unprocessedTileData.Length];
            for (int i = 0; i < result.Length; i++)
            {
                Texture2D tileTexture = content.Load<Texture2D>(unprocessedTileData[i].Spritesheet);
                result[i] = new BuildingData { ID = unprocessedTileData[i].ID, Name = unprocessedTileData[i].Name, Spritesheet = tileTexture, SampleArea = unprocessedTileData[i].SampleArea };
            }
            return result;
        }

        /*static public void SaveTileData(string filePath)
        {
            TileDataLoader[] data = new TileDataLoader[2];
            for (int i = 0; i < 2; i++)
            {
                data[i] = new TileDataLoader { ID = 1, SampleArea = new Rectangle(1,2,3,4), Spritesheet = "bluh"};
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TileDataCont));
            StreamWriter streamWriter = new StreamWriter(File.Create(filePath));
            xmlSerializer.Serialize(streamWriter, new TileDataCont(data));
            streamWriter.Close();
        }*/

        public static void CreateDataSet()
        {
            #region Create Terrain TileData
            TileDataLoader[] data = new TileDataLoader[70];
            for (int y = 0; y < 14; y++)
                for (int x = 0; x < 5; x++)
                {
                    string name = null;
                    if (x == 0 && y % 2 == 0)
                        name = "Dozed";
                    else if (x == 0 && y % 2 == 1)
                        name = "Dug";
                    else if (x == 1 && y % 2 == 0)
                        name = "Smooth";
                    else if (x == 1 && y % 2 == 1)
                        name = "Dozed";
                    else if (x == 2)
                        name = "Rough";
                    else if (x == 3)
                        name = "Hostile";
                    else if (x == 4)
                        name = "Impassible";

                    data[y * 5 + x] = new TileDataLoader { ID = y * 5 + x, Name = name, 
                        SampleArea = new Rectangle(x * 109 + 1, y * 48 + 1, 106, 46), 
                        Spritesheet = ".//Old Content//Terrain//Terrain.png" };
                }
            int prevSet = 70;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TileDataCont));
            StreamWriter streamWriter = new StreamWriter(File.Create(".//Terrain.xml"));
            xmlSerializer.Serialize(streamWriter, new TileDataCont(data));
            streamWriter.Close();
            #endregion
            BuildingDataLoader[] data2 = new BuildingDataLoader[116];

            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    data2[y * 10 + x] = new BuildingDataLoader {ID = y * 10 + x + prevSet, Name = "",
                        SampleArea = new Rectangle(101 * x + 2, 78 * y + 2, 98, 75),
                        Spritesheet = ".//Old Content//Buildings//Buildings 1.png"};
                }
            }
            prevSet += 70;

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    data2[y * 9 + x + prevSet - 70] = new BuildingDataLoader {ID = y * 9 + x + prevSet, Name = "",
                        SampleArea = new Rectangle(113 * x + 549, 119 * y + 2, 110, 116),
                        Spritesheet = ".//Old Content//Buildings//Buildings 1.png"};
                }
            }
            prevSet += 36;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (y * 4 + x == 10)
                        break;
                    data2[y * 4 + x + prevSet - 70] = new BuildingDataLoader {ID = y * 4 + x + prevSet, Name = "",
                        SampleArea = new Rectangle(113 * x + 2, 119 * y + 2, 110, 116),
                        Spritesheet = ".//Old Content//Buildings//Buildings 2.png"};
                }
            }

            xmlSerializer = new XmlSerializer(typeof(BuildingDataCont));
            streamWriter = new StreamWriter(File.Create(".//Buildings.xml"));
            xmlSerializer.Serialize(streamWriter, new BuildingDataCont(data2));
            streamWriter.Close();
        }

        public static Tile[, ,] LoadTileMap(string path, MapToTileData conversions, int levels, DynamicContentManager content)
        {
            if (levels < 1)
                throw new ArgumentException("The map must have at least one level.");
            Texture2D mapImage = content.Load<Texture2D>(path);
            Color[] mapData = new Color[mapImage.Width * mapImage.Height];
            mapImage.GetData<Color>(mapData);
            Tile[,,] map = new Tile[mapImage.Width, mapImage.Height, levels];
            for (int z = 0; z < levels; z++)
            {
                for (int y = 0; y < mapImage.Height; y++)
                {
                    for (int x = 0; x < mapImage.Width; x++)
                    {
                        if (z == 0)
                        {
                            if (mapData[y * mapImage.Width + x] == new Color(128, 128, 128))
                                map[x, y, z] = new Tile(conversions.ADozed, true);
                            else if (mapData[y * mapImage.Width + x] == new Color(128, 128, 0))
                                map[x, y, z] = new Tile(conversions.ASmooth, true);
                            else if (mapData[y * mapImage.Width + x] == new Color(0, 128, 0))
                                map[x, y, z] = new Tile(conversions.ARough, true);
                            else if (mapData[y * mapImage.Width + x] == new Color(128, 0, 0))
                                map[x, y, z] = new Tile(conversions.AHostile, true);
                            else if (mapData[y * mapImage.Width + x] == new Color(0, 0, 0))
                                map[x, y, z] = new Tile(conversions.AImpassible, true);
                        }
                        else
                        {
                            if (mapData[y * mapImage.Width + x] == new Color(128, 128, 128))
                                map[x, y, z] = new Tile(conversions.UDozed, true);
                            else if (mapData[y * mapImage.Width + x] == new Color(128, 128, 0))
                                map[x, y, z] = new Tile(conversions.USmooth, false);
                            else if (mapData[y * mapImage.Width + x] == new Color(0, 128, 0))
                                map[x, y, z] = new Tile(conversions.URough, false);
                            else if (mapData[y * mapImage.Width + x] == new Color(128, 0, 0))
                                map[x, y, z] = new Tile(conversions.UHostile, false);
                            else if (mapData[y * mapImage.Width + x] == new Color(0, 0, 0))
                                map[x, y, z] = new Tile(conversions.UImpassible, false);
                        }
                    }
                }
            }
            return map;
        }

        public struct MapToTileData
        {
            public int ADozed;
            public int ASmooth;
            public int ARough;
            public int AHostile;
            public int AImpassible;
            public int UDozed;
            public int USmooth;
            public int URough;
            public int UHostile;
            public int UImpassible;

            public MapToTileData(int aDozed, int aSmooth, int aRough, int aHostile, int aImpassible, int uDozed, int uSmooth, int uRough, int uHostile, int uImpassible)
            {
                ADozed = aDozed;
                ASmooth = aSmooth;
                ARough = aRough;
                AHostile = aHostile;
                AImpassible = aImpassible;
                UDozed = uDozed;
                USmooth = uSmooth;
                URough = uRough;
                UHostile = uHostile;
                UImpassible = uImpassible;
            }
        }
    }

    public class TileDataLoader
    {
        public int ID;
        public string Name;
        public string Spritesheet;
        public Dimensions SampleArea;
    }

    public class BuildingDataLoader
    {
        public int ID;
        public string Name;
        public string Spritesheet;
        public Dimensions SampleArea;
    }

    public struct Dimensions
    {
        [XmlAttribute]
        public int X;
        [XmlAttribute]
        public int Y;
        [XmlAttribute]
        public int Width;
        [XmlAttribute]
        public int Height;

        public static implicit operator Rectangle(Dimensions a)
        {
            return new Rectangle(a.X, a.Y, a.Width, a.Height);
        }

        public static implicit operator Dimensions(Rectangle a)
        {
            return new Dimensions{X = a.X, Y = a.Y, Width = a.Width, Height = a.Height};
        }
    }

    public class TileDataCont
    {
        public TileDataLoader[] TileDefinitions;

        public TileDataCont() { }

        public TileDataCont(TileDataLoader[] data)
        {
            TileDefinitions = data;
        }
    }

    public class BuildingDataCont
    {
        public BuildingDataLoader[] TileDefinitions;

        public BuildingDataCont() { }

        public BuildingDataCont(BuildingDataLoader[] data)
        {
            TileDefinitions = data;
        }
    }
}
