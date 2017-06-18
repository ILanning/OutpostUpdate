using System;
using CommonCode;
using CommonCode.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CommonCode.Drawing;

namespace Outpost.Windows
{
    class MapElement : Element, IInputElement, IDataDisplay
    {
        public static string IntendedType { get { return typeof(Coordinate[]).Name; } }

        DataProvider boundFunction;
        Texture2D mapImage;
        Texture2D tilesImage;
        bool currentMap = true;
        Rectangle croppedArea;
        Coordinate[] viewportPoints;

        public string[] Events { get { return new string[]{"Clicked"}; } }
        public event EventHandler<MapImageEventArgs> Clicked;

        public MapElement(Texture2D map, Texture2D tiles, SideTack attachment = SideTack.Center, DataProvider provider = null)
        {
            mapImage = map;
            tilesImage = tiles;
            if (map.Width != tiles.Width || map.Height != tiles.Height)
                throw new ArgumentException("map and tiles must have matching dimension.");
            MaximumSize = new Coordinate(map.Width, map.Height);
            MinimumSize = new Coordinate(map.Width, map.Height);
            croppedArea = new Rectangle(0, 0, map.Width, map.Height);
            if (provider != null)
                BindData(provider);
        }

        public override void Move(Coordinate movement)
        {
            targetArea.Location += (Point)movement;
        }

        public override void Resize(Rectangle targetSpace)
        {
            Rectangle resultArea = Rectangle.Empty;
            resultArea.Width = targetSpace.Width > MaximumSize.X ? MaximumSize.X : targetSpace.Width;
            resultArea.Height = targetSpace.Height > MaximumSize.Y ? MaximumSize.Y : targetSpace.Height;
            croppedArea.Width = targetSpace.Width < MinimumSize.X ? targetSpace.Width : mapImage.Width;
            croppedArea.Height = targetSpace.Height < MinimumSize.Y ? targetSpace.Height : mapImage.Height;
            resultArea.Location = (Point)SideStick(targetSpace, resultArea);
            targetArea = resultArea;
        }

        public bool BindData(DataProvider provider)
        {
            if (provider.Method.ReturnType.Name != IntendedType)
                return false;
            boundFunction = provider;
            return true;
        }

        public void UpdateData() { }

        public void HandleInput()
        {
            if (targetArea.Contains(InputManager.MousePosition) && InputManager.IsMouseButtonTriggered(MouseButtons.RMB))
                currentMap = !currentMap;
            if (targetArea.Contains(InputManager.MousePosition) && InputManager.IsMouseButtonDown(MouseButtons.LMB) && Clicked != null)
                Clicked(this, new MapImageEventArgs(InputManager.MousePosition - targetArea.Location));
        }

        public override void Update(GameTime gameTime)
        {
            viewportPoints = (Coordinate[])boundFunction.Invoke();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.End();

            sb.Begin(rasterizerState: ScreenManager.Globals.ClipState);
            sb.Draw(currentMap ? mapImage : tilesImage, targetArea, croppedArea, Color.White);
            sb.GraphicsDevice.ScissorRectangle = targetArea;
            for (int i = 0; i < 4; i++)
                Line2D.Draw(viewportPoints[i], viewportPoints[(i + 1) % 4], TempGlobals.BorderColors[1], (Coordinate)targetArea.Location, sb, 1);
            sb.End();

            sb.Begin();
        }
    }

    /// <summary>
    /// Returns the coordinates on the map that the mouse clicked at.
    /// </summary>
    class MapImageEventArgs : EventArgs
    {
        public Coordinate Coords;

        public MapImageEventArgs(Coordinate coords)
        {
            Coords = coords;
        }
    }
}
