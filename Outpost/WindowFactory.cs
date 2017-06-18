using CommonCode;
using CommonCode.Collision;
using CommonCode.UI;
using CommonCode.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Outpost
{
    //Responsible for loading and potentially saving window layouts
    //May be used to save data on the currently open windows
    public class WindowFactory
    {
        //For now, hardcode windows so things elsewhere can be done easily
        public static Window Create(string windowType, Coordinate position, WindowManager manager, params object[] args)
        {
            throw new NotImplementedException();
        }

        public static Window CreateMessageBox(string message, Coordinate position, WindowManager manager)
        {
            ButtonElement element = new ButtonElement(new Button(new AABox(new Rectangle(0, 0, 88, 23)), ScreenManager.Content.Load<Texture2D>(".//UI//Exit Button.png"),
                new Rectangle[] { new Rectangle(0, 0, 88, 23), new Rectangle(0, 23, 88, 23), new Rectangle(0, 46, 88, 23), new Rectangle(0, 69, 88, 23) },
                Coordinate.Zero), new Coordinate(88, 23), "Close", SideTack.Center);
            SpaceFillList container = new SpaceFillList(new float[] { 1.5f, 1 }, new Element[] { new TextElement(message, 
                "MSSansSerif", Color.Green, "Text", ResizeKind.FillRatio), element }, new Rectangle(0, 0, 300, 70), Coordinate.Zero, false, false, Color.Black, "TopCont");
            Window messageBox = new Window(manager, ScreenManager.Content, new Rectangle(position.X, position.Y, 300, 70), TempGlobals.BorderColors, container);
            messageBox.BindInput(element.FullName, "Released", new EventHandler(messageBox.CloseWindow));
            return messageBox;
        }

        /*public static Window CreateTextList(DataProvider provider, Coordinate position, WindowManager manager)
        {
            TextListElement element = new TextListElement("Default", new ColumnOptions[]{}, provider, new Color(), SideTack.Center);
            SpaceFillList main = new SpaceFillList(new float[]{1f}, new Element[]{element},true, "TopCont");
            Window listWindow = new Window(manager, ScreenManager.Content, new Rectangle(position.X, position.Y, element.MinimumSize.X, element.MinimumSize.Y), TempGlobals.BorderColors, main);
            return listWindow;
        }*/

        /*public static Window CreateBuildableList(Coordinate position, WindowManager manager)
        {
            Window result = null;
            Texture2D buttonTexture = ScreenManager.Content.Load<Texture2D>(".//Old Content//UI//Buttons.png");
            ButtonElement backButton = new ButtonElement(new Button(buttonTexture, new Rectangle[] { new Rectangle(0, 109, 10, 17), new Rectangle(0, 109, 10, 17), new Rectangle(0, 125, 10, 17), new Rectangle(0, 125, 10, 17) },
                new AABox(new Rectangle(0, 0, 10, 17)), Coordinate.Zero, true), new Coordinate(10, 17), "backButton", SideTack.Center);
            ButtonElement forwardButton = new ButtonElement(new Button(buttonTexture, new Rectangle[] { new Rectangle(9, 109, 10, 17), new Rectangle(9, 109, 10, 17), new Rectangle(9, 125, 10, 17), new Rectangle(9, 125, 10, 17) },
                new AABox(new Rectangle(0, 0, 10, 17)), Coordinate.Zero, true), new Coordinate(10, 17), "forwardButton", SideTack.Center);
            ButtonElement listButton = new ButtonElement(new Button(buttonTexture, new Rectangle[] { new Rectangle(19, 111, 62, 17), new Rectangle(19, 111, 62, 17), new Rectangle(19, 127, 62, 17), new Rectangle(19, 127, 62, 17) },
                new AABox(new Rectangle(0, 0, 10, 17)), Coordinate.Zero, true), new Coordinate(62, 17), "listButton", SideTack.Center);
            ConstructSelectElement buildList = new ConstructSelectElement();
            backButton.Clicked += buildList.OnBack;
            forwardButton.Clicked += buildList.OnForward;
            listButton.Clicked += buildList.OnList;
            return result;
        }*/
    }
}
