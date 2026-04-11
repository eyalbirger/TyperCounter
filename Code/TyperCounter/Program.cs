using System;
using System.IO;
using System.Linq;
using Terminal.Gui;
using SharpHook;
using SharpHook.Native;

//tmy classes
using TyperCounter;

namespace TyperCounter
{
  internal class Program
  {
    


    static void Main(string[] args)
    {
      List<keyboardKey> recordings  = new List<keyboardKey>();
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

      var programAfterRunning = new Label()
      {
        Text = @"",
        X = Pos.Center(),
        Y = Pos.Center() - 1,
        Width = Dim.Auto(),
        Height = Dim.Auto(),
        TextAlignment = Alignment.Center,
        Visible = false
      };
      
      //sharphook obj
      var hook = new TaskPoolGlobalHook();

      //i just need to add shit here
      win.Add(mainTitle, seconderyTitle, startTitle, secondScreenintroduction,
              areYouSureTitle, programRunningText, stopB, programAfterRunning);
      


      //im starting to actually do something here (like the program does something)
      bool startMenu = true;
      bool secondMenu = false;
      bool programBeforeRunning = false;
      bool accept = false;
      bool programRunning = true;
      bool programAfterRunningSoICouldScanTheTextFile = false;
      win.KeyDown += (sender, args) =>
      {
        if (!accept)
        {
          if (args == Key.Esc)
          {
            Application.RequestStop();
            args.Handled = true;
            return;
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


        else if (programAfterRunningSoICouldScanTheTextFile && args == Key.S)
        {
          if (File.Exists(logsPath))
          {
            string content = File.ReadAllText(logsPath);

            recordings = keyboardKey.record(recordings, content);

            var topkeys = recordings.OrderByDescending(k => k.frequency).ToList();

            var sb = new System.Text.StringBuilder();

            sb.AppendLine("KEY NAME       | FREQUENCY");
            sb.AppendLine("----------------|-----------");

            foreach (var key in topkeys)
            {
              string namePart = key.name.PadRight(15);
              string freqPart = key.frequency.ToString().PadRight(10);
              sb.AppendLine($"{key.name}: {key.frequency}");
            }
            programAfterRunning.Text = sb.ToString();
          }
        }
      };

      hook.KeyPressed += (sender, args) =>
      {
        if (accept)
        {
          FileInfo fileinfo = new FileInfo(@"logs\recording.txt");

          if (fileinfo.Exists && fileinfo.Length > 5 * 1024 * 1024)
            return;

          string keyname = args.Data.KeyCode.ToString().Replace("Vc", "");
          File.AppendAllText(logsPath, keyname + Environment.NewLine);
        }
      };

      stopB.Accepting += (s, e) =>
      {
        if(accept == true)
        {
          accept = false;
          stopB.Visible = false;
          programRunning = false;
          programRunningText.Visible = false;
          programAfterRunningSoICouldScanTheTextFile = true;
          programAfterRunning.Text = text.getText("textAfterRecording");

          programAfterRunning.Visible = true;
          win.SetNeedsLayout();
          programAfterRunning.SetFocus();

          hook.Dispose();
        }
      };
      




      
      Application.Run(win);
      win.SetFocus();
      Application.Shutdown();
      //the end :)
    }
  }
}