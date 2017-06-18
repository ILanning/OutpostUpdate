using CommonCode;
using CommonCode.Collision;
using CommonCode.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CommonCode.Windows;
using System;

namespace Outpost
{
    class WindowTestScreen : Screen
    {
        WindowManager manager = new WindowManager("MSSansSerif", "MSSansSerif");
        TempThing[] testSquares;

        public WindowTestScreen() : base() { }

        public override void LoadContent()
        {            
            ScreenManager.IsMouseVisible = true;
            testSquares = new TempThing[] { new TempThing(new Rectangle(0, 0, 25, 25)), new TempThing(new Rectangle(25, 0, 25, 25)), 
                                            new TempThing(new Rectangle(0, 25, 25, 25)), new TempThing(new Rectangle(25, 25, 25, 25)) };

            /*
             Need to provide a way for other things to trigger a data update on a display ojbect without having a reference to that object
             Other objects will have an event that triggers at some point determined by that thing
             Display object will take that event as an argument and bind to it that way
             */
        }

        public void test()
        { }

        public void AddThingModWindow(TempThing thing)
        {
            ButtonElement element = new ButtonElement(new Button(new AABox(new Rectangle(0, 0, 88, 23)), ScreenManager.Content.Load<Texture2D>(".//UI//Exit Button.png"),
                new Rectangle[] { new Rectangle(0, 0, 88, 23), new Rectangle(0, 23, 88, 23), new Rectangle(0, 46, 88, 23), new Rectangle(0, 69, 88, 23) },
                Coordinate.Zero), new Coordinate(88, 23), "Rotate Color", SideTack.Center);
            SpaceFillList container = new SpaceFillList(new float[] { 1.5f, 1 }, new Element[] { new TextElement("Click to Change Color", 
                "MSSansSerif", Color.Green, "Text", ResizeKind.FillRatio), element }, new Rectangle(0, 0, 300, 70), Coordinate.Zero, false, false, Color.Black, "TopCont");
            Window messageBox = new Window(manager, ScreenManager.Content, new Rectangle(50, 0, 175, 70), TempGlobals.BorderColors, container);
            messageBox.BindInput(element.FullName, "Released", new EventHandler(thing.Change));
            manager.AddWindow(messageBox);
        }

        public void TestTextListWindow()
        {
            TextListElement element = new TextListElement(new ColumnOptions[] { 
                new ColumnOptions { Justified = Justification.Left, Spacing = 2 }, new ColumnOptions { Justified = Justification.Middle, Spacing = 1 }, 
                new ColumnOptions { Justified = Justification.Middle, Spacing = 1 }, new ColumnOptions { Justified = Justification.Right, Spacing = 1 } },
                Color.White, new Coordinate(400, 230), new Coordinate(400, 230), SideTack.Center, dataTest, "MSSansSerif");
            SpaceFillList container = new SpaceFillList(new float[] { 1 }, new Element[] { element }, new Rectangle(0, 0, (int)element.IdealDimensions.X, (int)element.IdealDimensions.Y), Coordinate.Zero, false, true, "TopCont");
            Window textListBox = new Window(manager, ScreenManager.Content, new Rectangle(75, 75, 500, 250), TempGlobals.BorderColors, container);
            triggerUpdates += textListBox.GetUpdater(element.FullName);
            manager.AddWindow(textListBox);
        }

        string[,] persistentStringArray = new string[4, 10];
        UpdateData triggerUpdates;

        public string[,] dataTest()
        {
            return persistentStringArray;
        }

        public override void HandleInput(GameTime gameTime)
        {
            bool stringsUpdated = false;
            if (InputManager.IsKeyTriggered(Keys.N))
            {
                Window window = WindowFactory.CreateMessageBox("No Message", Coordinate.Zero, manager);
                manager.AddWindow(window);
            }
            if (InputManager.IsKeyTriggered(Keys.P) || InputManager.IsKeyTriggered(Keys.G))
            {
                Random rand = new Random();
                int rows = persistentStringArray.GetLength(0);
                int columns = persistentStringArray.GetLength(1);
                for (int j = 0; j < columns; j++)
                {
                    for (int i = 1; i < rows; i++)
                    {
                        persistentStringArray[i, j] = rand.Next(100000).ToString();
                    }
                }
                stringsUpdated = true;
            }
            if (InputManager.IsKeyTriggered(Keys.G))
            {
                persistentStringArray[0, 0] = "Row 1";
                persistentStringArray[0, 1] = "Row 2";
                persistentStringArray[0, 2] = "Row 3";
                persistentStringArray[0, 3] = "Row 4";
                persistentStringArray[0, 4] = "TestTest";
                persistentStringArray[0, 5] = "Row 6";
                persistentStringArray[0, 6] = "1";
                persistentStringArray[0, 7] = "PrettyLongTestName";
                persistentStringArray[0, 8] = "Row 9";
                persistentStringArray[0, 9] = "Row 10";
                TestTextListWindow();
                stringsUpdated = true;
            }
            if (stringsUpdated)
                triggerUpdates();
            manager.HandleInput();

            if (InputManager.IsKeyDown(Keys.Escape))
                RemoveSelf();
        }

        public override void Update(GameTime gameTime)
        {
            manager.Update(gameTime);
            foreach (TempThing thing in testSquares)
            {
                thing.Update();
                if (thing.Clicked)
                    AddThingModWindow(thing);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.Globals.sb;
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            for (int i = 0; i < testSquares.Length; i++)
                testSquares[i].Draw(sb);

            sb.End();

            manager.Draw(sb);
        }
    }

    class TempThing
    {
        Color[] colors = new Color[] { new Color(0, 0, 255), new Color(0, 255, 0), new Color(255, 0, 0) };
        int state = 0;
        Rectangle dimensions;
        public bool Clicked = false;

        public TempThing(Rectangle size)
        {
            dimensions = size;
        }

        public void Change(object sender, EventArgs e)
        {
            state = (state + 1) % 3;
        }

        public void Update()
        {
            Clicked = false;
            if (dimensions.Contains(InputManager.MousePosition) && InputManager.IsMouseButtonTriggered(MouseButtons.LMB))
                Clicked = true;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(ScreenManager.Globals.White1By1, dimensions, colors[state]);
        }
    }
}
