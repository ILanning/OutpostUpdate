using CommonCode;
using CommonCode.Drawing;
using CommonCode.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Outpost.FileHandling;
using Outpost.GameLogic;
using Outpost.Windows;
using System;
using System.Collections.Generic;

namespace Outpost
{
    class GameScreen : Screen
    {
        WindowManager windows = new WindowManager("MSSansSerif", "MSSansSerif");
        Coordinate cameraPos = Coordinate.Zero;
        int cameraLevel = 0;
        Matrix toTile, toPixel;
        Rectangle pixelArea;
        Vector4 tileArea;
        Coordinate mouseTile = new Coordinate(-1, -1);
        Texture2D cursor;
        TileData aDozed;
        TileData uDozed;
        Simulator sim;

        public GameScreen() : base()
        {
            ScreenManager.IsMouseVisible = true;
        }

        public override void LoadContent()
        {
            sim = new Simulator();
            cursor = ScreenManager.Content.Load<Texture2D>(".//Old Content//UI//Cursor.png");
            aDozed = sim.TileTypes[0];
            uDozed = sim.TileTypes[6];
            windows.LoadContent();
            createMapWindow();
        }

        public override void HandleInput(GameTime gameTime)
        {
            windows.HandleInput();

            mouseTile = new Coordinate(-1, -1);
            if (!windows.InteractedWith)
            {
                if (pixelArea.Contains(InputManager.MousePosition) && !windows.CollidedWith)
                    mouseTile = (Coordinate)(Vector2.Transform(InputManager.MousePosition + cameraPos, toTile) + new Vector2(1));

                if (InputManager.IsKeyReleased(Keys.PageDown) && cameraLevel < sim.map.GetLength(2)-1)
                    cameraLevel++;
                if (InputManager.IsKeyReleased(Keys.PageUp) && cameraLevel > 0)
                    cameraLevel--;

                Vector2 movement = Vector2.Zero;
                if (InputManager.IsKeyDown(Keys.Up))
                    movement.Y--;
                if (InputManager.IsKeyDown(Keys.Down))
                    movement.Y++;
                if (InputManager.IsKeyDown(Keys.Left))
                    movement.X--;
                if (InputManager.IsKeyDown(Keys.Right))
                    movement.X++;
                if (movement != Vector2.Zero)
                {
                    movement.Normalize();
                    movement *= 5;
                    if (InputManager.IsKeyDown(Keys.LeftShift) || InputManager.IsKeyDown(Keys.RightShift))
                        movement *= 4;
                    cameraPos += (Coordinate)movement;
                }

                if (InputManager.IsKeyDown(Keys.Space) && InputManager.IsMouseButtonDown(MouseButtons.LMB) && !windows.InteractedWith)
                    cameraPos -= (Coordinate)InputManager.MouseMovement;

            }
            //Add in camera movement limits

            if (InputManager.IsKeyDown(Keys.Escape))
                RemoveSelf();
        }

        public override void Update(GameTime gameTime)
        {
            updateDrawValues();
            windows.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.Globals.sb;
            drawTiles();
            windows.Draw(sb);

            sb.Begin();

            //sb.DrawString(ScreenManager.Globals.Fonts["MSSansSerif"], mouseTile.ToString(), new Vector2(1000, 0), Color.White);

            sb.End();
        }

        void updateDrawValues()
        {
            Coordinate screenSize = new Coordinate(ScreenManager.Globals.Graphics.Viewport.Width, ScreenManager.Globals.Graphics.Viewport.Height);
            pixelArea = new Rectangle(50, 120, screenSize.X - 100, screenSize.Y - 170);
            Coordinate hTile = TempGlobals.TileSize / 2;
            toPixel = new Matrix(hTile.X, hTile.Y, 0, 0,
                                 -hTile.X, hTile.Y, 0, 0,
                                 0, 0, 1, 0,
                                 0, 0, 0, 1);
            toTile = Matrix.Invert(toPixel);
            tileArea = new Vector4(((int)Vector2.Transform(new Vector2(pixelArea.X, pixelArea.Y), toTile).X) - 2,
                                           ((int)Vector2.Transform(new Vector2(pixelArea.Right, pixelArea.Y), toTile).Y) - 2,
                                           ((int)Vector2.Transform(new Vector2(pixelArea.Right, pixelArea.Bottom), toTile).X) + 2,
                                           ((int)Vector2.Transform(new Vector2(pixelArea.X, pixelArea.Bottom), toTile).Y) + 3);
        }

        void drawTiles()
        {
            SpriteBatch sb = ScreenManager.Globals.sb;

            sb.Begin(rasterizerState: ScreenManager.Globals.ClipState, blendState: BlendState.AlphaBlend);
            sb.GraphicsDevice.ScissorRectangle = pixelArea;

            //Camera is at top left corner of drawArea

            Coordinate tilePos = (Coordinate)Vector2.Transform(cameraPos, toTile);
            Vector4 tileAreaAdj = new Vector4(tileArea.X + tilePos.X, tileArea.Y + tilePos.Y, tileArea.Z + tilePos.X, tileArea.W + tilePos.Y);

            for (int i = tileAreaAdj.X < tileAreaAdj.Y ? (int)tileAreaAdj.X : (int)tileAreaAdj.Y; i < (tileAreaAdj.Z > tileAreaAdj.W ? tileAreaAdj.Z : tileAreaAdj.W); i++)
            {
                Line2D l1 = new Line2D(Vector2.Transform(new Vector2(tileAreaAdj.X, i), toPixel) - cameraPos, Vector2.Transform(new Vector2(tileAreaAdj.Z, i), toPixel) - cameraPos, 1, TempGlobals.BorderColors[1] * 0.5f);
                Line2D l2 = new Line2D(Vector2.Transform(new Vector2(i, tileAreaAdj.Y), toPixel) - cameraPos, Vector2.Transform(new Vector2(i, tileAreaAdj.W), toPixel) - cameraPos, 1, TempGlobals.BorderColors[1] * 0.3f);
                l1.Draw(sb);
                l2.Draw(sb);
            }

            Line2D originX = new Line2D(Vector2.Transform(new Vector2(tileAreaAdj.X, -1), toPixel) - cameraPos - new Vector2(0, 1), Vector2.Transform(new Vector2(tileAreaAdj.Z, -1), toPixel) - cameraPos - new Vector2(0, 1), 1.5f, TempGlobals.BorderColors[1]);
            Line2D originY = new Line2D(Vector2.Transform(new Vector2(-1, tileAreaAdj.Y), toPixel) - cameraPos, Vector2.Transform(new Vector2(-1, tileAreaAdj.W), toPixel) - cameraPos, 1.5f, TempGlobals.BorderColors[1]);
            originX.Draw(sb);
            originY.Draw(sb);


            for (int y = (int)tileAreaAdj.Y; y < tileAreaAdj.W; y++)
                if (y >= 0 && y < sim.map.GetLength(1))
                {
                    for (int x = (int)tileAreaAdj.X; x < tileAreaAdj.Z; x++)
                    {
                        if (x >= 0)
                        {
                            if (x >= sim.map.GetLength(0))
                                break;
                            Tile tile = sim.map[x, y, cameraLevel];
                            TileData data = sim.TileTypes[tile.ID];
                            Coordinate position = (Coordinate)Vector2.Transform(new Vector2(x, y), toPixel) - cameraPos;
                            //sb.Draw(data.Spritesheet, position, data.SampleArea, Color.White);
                            RenderTiles.Draw(position, tile, data, (cameraLevel == 0 ? aDozed : uDozed), sb);
                            if (mouseTile.X != -1 && mouseTile.X == x && mouseTile.Y == y)
                            {
                                sb.Draw(cursor, position - new Coordinate(cursor.Width / 2, cursor.Height), Color.White);
                            }
                        }
                    }
                }
            sb.End();
        }

        public Coordinate[] returnMapPos()
        {
            Coordinate[] result = new Coordinate[4];
            result[0] = (Coordinate)Vector2.Transform(cameraPos + new Coordinate(pixelArea.X, pixelArea.Y), toTile);
            result[1] = (Coordinate)Vector2.Transform(cameraPos + new Coordinate(pixelArea.X, pixelArea.Bottom), toTile);
            result[2] = (Coordinate)Vector2.Transform(cameraPos + new Coordinate(pixelArea.Right, pixelArea.Bottom), toTile);
            result[3] = (Coordinate)Vector2.Transform(cameraPos + new Coordinate(pixelArea.Right, pixelArea.Y), toTile);
            return result;
        }

        void minimapClicked(object sender, MapImageEventArgs e)
        {
            Coordinate areaSize = (Coordinate)Vector2.Transform(new Coordinate(pixelArea.Width, pixelArea.Height), toTile);
            Coordinate correctedPoint = e.Coords - areaSize/2;
            cameraPos = (Coordinate)Vector2.Transform(correctedPoint, toPixel);
        }

        void createMapWindow()
        {
            Texture2D mapA = ScreenManager.Content.Load<Texture2D>(sim.mapPaths[0]);
            Texture2D mapB = ScreenManager.Content.Load<Texture2D>(sim.mapPaths[1]);
            MapElement element = new MapElement(mapA, mapB, SideTack.Center, returnMapPos);
            SpaceFillList mainCont = new SpaceFillList(new float[] { 1 }, new Element[] { element }, false, "MainCont");
            Window mapWindow = new Window(windows, ScreenManager.Content, new Rectangle(500, 0, mapA.Width, mapA.Height), TempGlobals.BorderColors, mainCont, "Map Name", true, true);
            mapWindow.BindInput(element.FullName, "Clicked", new EventHandler<MapImageEventArgs>(minimapClicked));
            windows.AddWindow(mapWindow);
        }
    }
}
