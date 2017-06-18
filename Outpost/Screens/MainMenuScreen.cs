using CommonCode;
using CommonCode.Collision;
using CommonCode.UI;
using CommonCode.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Outpost
{
    class MainMenuScreen : Screen
    {
        WindowManager windows = new WindowManager("MSSansSerif", "MSSansSerif");
        Texture2D bg;

        public MainMenuScreen() : base() { }

        public override void LoadContent()
        {
            ScreenManager.IsMouseVisible = true;
            bg = ScreenManager.Content.Load<Texture2D>(".//Old Content//Title Screen.png");
            ScreenManager.Content.Load<Texture2D>(".//UI//Exit Button.png");
            windows.AddWindow(CreateMainMenu());
            windows.LoadContent();
        }

        public override void HandleInput(GameTime gameTime)
        {
            windows.HandleInput();

            if (InputManager.IsKeyTriggered(Microsoft.Xna.Framework.Input.Keys.Escape))
                ScreenManager.StaticGame.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            windows.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.Globals.sb.Begin();

            Vector2 bgPos = new Vector2((ScreenManager.StaticGame.Window.ClientBounds.Width - bg.Width) / 2, (ScreenManager.StaticGame.Window.ClientBounds.Height - bg.Height) / 2);
            ScreenManager.Globals.sb.Draw(bg, bgPos, Color.White);

            ScreenManager.Globals.sb.End();

            windows.Draw(ScreenManager.Globals.sb);
        }

        public Window CreateMainMenu()
        {
            Texture2D buttonSheet = ScreenManager.Content.Load<Texture2D>(".//Old Content//UI//Buttons.png");
            AABox buttonBox = new AABox(new Rectangle(0, 0, 87, 23));

            FlatColorElement spacer1 = new FlatColorElement(new Color(255, 255, 255, 0), "Spacer 1");
            ButtonElement newGame = new ButtonElement(new Button(new AABox(buttonBox), buttonSheet, 
                new Rectangle[] { new Rectangle(406, 0, 87, 23), new Rectangle(406, 0, 87, 23), new Rectangle(406, 23, 87, 23), new Rectangle(406, 0, 87, 23) },
                new Coordinate(0, 0)), new Coordinate(87, 23), "New Game", SideTack.Center);
            newGame.Clicked += onClickNewGame;
            ButtonElement intro = new ButtonElement(new Button(new AABox(buttonBox), buttonSheet, 
                new Rectangle[] { new Rectangle(406, 46, 87, 23), new Rectangle(406, 46, 87, 23), new Rectangle(406, 69, 87, 23), new Rectangle(406, 46, 87, 23) },
                new Coordinate(0, 0)), new Coordinate(87, 23), "Introduction", SideTack.Center);
            intro.Clicked += onClickIntroduction;
            ButtonElement loadGame = new ButtonElement(new Button(new AABox(buttonBox), buttonSheet, 
                new Rectangle[] { new Rectangle(406, 92, 87, 23), new Rectangle(406, 92, 87, 23), new Rectangle(406, 115, 87, 23), new Rectangle(406, 92, 87, 23) },
                new Coordinate(0, 0)), new Coordinate(87, 23), "Load Game", SideTack.Center);
            loadGame.Clicked += onClickLoadGame;
            ButtonElement prefs = new ButtonElement(new Button(new AABox(buttonBox), buttonSheet, 
                new Rectangle[] { new Rectangle(406, 138, 87, 23), new Rectangle(406, 138, 87, 23), new Rectangle(406, 161, 87, 23), new Rectangle(406, 138, 87, 23) },
                new Coordinate(0, 0)), new Coordinate(87, 23), "Preferences", SideTack.Center);
            prefs.Clicked += onClickPreferences;
            ButtonElement exit = new ButtonElement(new Button(new AABox(buttonBox), buttonSheet, 
                new Rectangle[] { new Rectangle(406, 184, 87, 23), new Rectangle(406, 184, 87, 23), new Rectangle(406, 207, 87, 23), new Rectangle(406, 184, 87, 23) },
                new Coordinate(0, 0)), new Coordinate(87, 23), "Exit", SideTack.Center);
            exit.Clicked += onClickExit;
            TextElement version = new TextElement("Version 0.01", "MSSansSerif", TempGlobals.BorderColors[1], "Version", ResizeKind.FillRatio);
            TextElement copyright = new TextElement("Copyright let's say Sierra, sure", "MSSansSerif", TempGlobals.BorderColors[1], "Copyright", ResizeKind.FillRatio);
            TextElement arr = new TextElement("Please Don't Sue", "MSSansSerif", TempGlobals.BorderColors[1], "ARR", ResizeKind.FillRatio);

            SpaceFillList mainCont = new SpaceFillList(new float[] {0.2f, 1, 1, 1, 1, 1, 0.5f, 0.5f, 0.5f }, new Element[] { spacer1, newGame, intro, loadGame, prefs, exit, version, copyright, arr }, false, "TopCont");
            Rectangle dimensions = new Rectangle((ScreenManager.StaticGame.Window.ClientBounds.Width-293)/2, (ScreenManager.StaticGame.Window.ClientBounds.Height-244)/2, 293, 244);
            Window mainMenuWindow = new Window(windows, ScreenManager.Content, dimensions, TempGlobals.BorderColors, mainCont, "Program Control", false);

            return mainMenuWindow;
        }

        void onClickNewGame(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameScreen());
        }

        void onClickIntroduction(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new WindowTestScreen());
        }

        void onClickLoadGame(object sender, EventArgs e)
        {
            
        }

        void onClickPreferences(object sender, EventArgs e)
        {
            
        }

        void onClickExit(object sender, EventArgs e)
        {
            ScreenManager.StaticGame.Exit();
        }
    }
}
