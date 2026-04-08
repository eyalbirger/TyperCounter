using System;
using Terminal.Gui;

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
    
    static string TC_title = @"
  _______                        ______                                  
 /_  __/_  ______  ___  _____   / ____/___  __  ______  / /____  _____ 
  / / / / / / __ \/ _ \/ ___/  / /   / __ \/ / / / __ \/ __/ _ \/ ___/ 
 / / / /_/ / /_/ /  __/ /     / /___/ /_/ / /_/ / / / / /_/  __/ /     
/_/  \__, / .___/\___/_/      \____/\____/\__,_/_/ /_/\__/\___/_/      
    /____/_/                                                           ";
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

      var win = new Window()
      {
        Title = "TyperCounter",
        X = 0,
        Y = 1,
        Width = Dim.Fill(),
        Height = Dim.Fill(),
        ColorScheme = grayOnBlack
      };
      
      var mainTitle = new Label()
      {
        Text = TC_title,
        X = Pos.Center(),
        Y = Pos.Center() - 2,
        ColorScheme = whiteOnBlackText
      };
      var seconderyTitle = new Label()
      {
        Text = "This is TyperCounter. A tool to help you visualize your most used keyboard keys.",
        X = Pos.Center(),
        Y = mainTitle.Y + 6,
        ColorScheme = whiteOnBlackText
      };
      
      var startB = new Button()
      {
        Text = "start",
        X = Pos.Center(),
        Y = 5
      };
      
      win.Add(mainTitle, seconderyTitle);
      
      Application.Run(win);

      Application.Shutdown();
      
      //System.Console.WriteLine("This is TyperCounter. its a tool to help you visualize your most used keyboard keys press");
    }
  }
}