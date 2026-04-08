using System;
using System.IO;
using Terminal.Gui;

using TyperCounter;

namespace TyperCounter
{
  public class keyboardKey
  {
    public string name;
    public int frequency;

    public keyboardKey(string name, int frequency)
    {
      this.name = name;
      this.frequency = frequency;
    }
  }

  internal class Program
  {
    

    static void Main(string[] args)
    {
      Application.Init ();
      var top = Application.Top;

      //colorssss yay
      var grayOnBlack = new ColorScheme()
      {
        Normal = new Terminal.Gui.Attribute(Color.Gray, Color.Black),
        Focus = new Terminal.Gui.Attribute(Color.Black, Color.Gray),
        HotNormal = new Terminal.Gui.Attribute(Color.White, Color.Blue)
      };
      var whiteOnBlackText = new ColorScheme()
      {
        Normal = new Terminal.Gui.Attribute(Color.White, Color.Black),
      };

      TextForApp text = new TextForApp() {};
      var win = new Window()
      {
        
        Title = "TyperCounter - press 'Esc' to exit",
        X = 0,
        Y = 1,
        Width = Dim.Fill(),
        Height = Dim.Fill(),
        ColorScheme = grayOnBlack,
        CanFocus = true
      };
      
      var mainTitle = new Label()
      {
        Text = text.getText("TC_title"),
        X = Pos.Center(),
        Y = Pos.Center() - 4,
        ColorScheme = whiteOnBlackText
      };
      var seconderyTitle = new Label()
      {
        Text = "This is TyperCounter. A tool to help you visualize your most used keyboard keys.",
        X = Pos.Center(),
        Y = mainTitle.Y + 6,
        ColorScheme = whiteOnBlackText
      };
      var startTitle = new Label()
      {
        Text = "press 'Space' to start",
        X = Pos.Center(),
        Y = seconderyTitle.Y + 1
      };
      
      var secondScreenIntrodaction = new Label()
      {
        Text = @"",
        X = Pos.Center(),
        Y = Pos.Center()//,
        //Width = Dim.Fill(),
        //Height = Dim.Fill()
      };
      


      win.Add(mainTitle, seconderyTitle, startTitle, secondScreenIntrodaction);
      
      bool startMenu = true;
      win.KeyDown += (sender, args) =>
      {
        if (startMenu == true)
        {
          if (args == Key.Space)
          {
            startMenu = false;

            win.Remove(startTitle);
            win.Remove(seconderyTitle);
            win.Remove(mainTitle);

            secondScreenIntrodaction.Text = text.getText("introdactionText");
          }
        }
      };




      
      Application.Run(win);
      win.SetFocus();
      Application.Shutdown();
      
      //System.Console.WriteLine("This is TyperCounter. its a tool to help you visualize your most used keyboard keys press");
    }
  }
}