using System;
using System.IO;
using Terminal.Gui;
using SharpHook;
using SharpHook.Native;

//the other text file
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
      //folder path for the logs :)
      string logsPath = Path.Combine("logs", "recording.txt");

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
  
      var secondScreenintroduction = new Label()
      {
        Text = @"",
        X = Pos.Center(),
        Y = Pos.Center(),
        Width = Dim.Auto(),
        Height = Dim.Auto(),
        TextAlignment = Alignment.Center
      };
      var areYouSureTitle = new Label()
      {
        Text = @"",
        X = Pos.Center(),
        Y = Pos.Center() - 4
      };

      var programRunningText = new Label()
      {
        Text = @"",
        X = Pos.Center(),
        Y = Pos.Center() - 4,
        Width = Dim.Auto(),
        Height = Dim.Auto(),
        TextAlignment = Alignment.Center
      };

      var stopB = new Button()
      {
        Text = "stop",
        X = Pos.Center(),
        Y = Pos.Center() - 1,
        Visible = false
      };
      
      //sharphook obj
      var hook = new TaskPoolGlobalHook();

      //i just need to add shit here
      win.Add(mainTitle, seconderyTitle, startTitle, secondScreenintroduction, areYouSureTitle, programRunningText, stopB);
      


      //im starting to actually do something here (like the program does something)
      bool startMenu = true;
      bool secondMenu = false;
      bool programBeforeRunning = false;
      bool accept = false;
      bool programRunning = true;
      win.KeyDown += (sender, args) =>
      {
        if (startMenu == true || secondMenu == true || programBeforeRunning == true)
        {
          if (args == Key.Esc)
          {
            Application.RequestStop();
          }
        }
        if (startMenu == true)
        {
          if (args == Key.Space)
          {
            startMenu = false;
            secondMenu = true;
            win.Remove(startTitle);
            win.Remove(seconderyTitle);
            win.Remove(mainTitle);

            secondScreenintroduction.Text = text.getText("introductionText");
          }
        }
        //are you sure? (omni-man meme)
        else if (secondMenu && args == Key.Space)
        {
          secondMenu = false;
          programBeforeRunning = true;
          win.Remove(secondScreenintroduction);

          areYouSureTitle.Text = text.getText("areYouSureText");
          args.Handled = true;
        }
        //accept
        else if (programBeforeRunning)
        {
          if (args == Key.Y)
          {
          accept = true;
          programBeforeRunning = false;
          win.Title = "TyperCounter";
          programRunningText.Text = "Program Running";
          stopB.Visible = true;
          programRunning = true;
          win.Remove(areYouSureTitle);
          args.Handled = true;

          //sharphook hook
          Task.Run(() => hook.Run());
          }
        else if (args == Key.N)
          Application.RequestStop();
        }
      };

      hook.KeyPressed += (sender, args) =>
      {
        if (accept)
        {
          string keyname = args.Data.KeyCode.ToString().Replace("Vc", "");
          File.AppendAllText(logsPath, keyname + Environment.NewLine);
        }
      };

      stopB.Accepting += (s, e) =>
      {
        if(accept == true)
        {
          stopB.Visible = false;
          programRunning = false;
          programRunningText.Text = text.getText("textAfterRecording");
          hook.Dispose();
        }
      };
      
      
      Application.Run(win);
      win.SetFocus();
      Application.Shutdown();
      
      //System.Console.WriteLine("This is TyperCounter. its a tool to help you visualize your most used keyboard keys press");
    }
  }
}