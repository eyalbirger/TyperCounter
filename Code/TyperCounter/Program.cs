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
      //folder path for the layout
      string layoutPath = Path.Combine("layout", "layout.txt");

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
        Y = Pos.Center() - 2,
        Width = Dim.Fill(),
        Height = Dim.Fill(),
        Visible = false
      };

      var scanB = new Button()
      {
        Text = @"",
        X = Pos.Center(),
        Y = Pos.Center(),
        Visible = false
      };
      
      //sharphook obj
      var hook = new TaskPoolGlobalHook();

      //i just need to add shit here
      win.Add(mainTitle, seconderyTitle, startTitle, secondScreenintroduction,
              areYouSureTitle, programRunningText, stopB, programAfterRunning,
              scanB);
      


      //im starting to actually do something here (like the program does something)
      bool startMenu = true;
      bool secondMenu = false;
      bool programBeforeRunning = false;
      bool accept = false;
      bool programRunning = true;
      bool programAfterRunningSoICouldScanTheTextFile = false;
      bool isProcessingStats = false;
      bool scanPlz = false;
      bool exportToTxt = false;
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
          win.SetNeedsLayout();
          programAfterRunning.SetFocus();

          hook.Dispose();

          programAfterRunning.Visible = true;
          
          scanB.Visible = true;
          scanB.Text = text.getText("textAfterRecording");
          scanB.SetFocus();
        }
      };
      scanB.Accepting += (s, e) =>
      {
        if (programAfterRunningSoICouldScanTheTextFile)
        {
          scanPlz = true;
          if (File.Exists(logsPath))
          {
            string content = File.ReadAllText(logsPath);
            recordings.Clear();
            recordings = keyboardKey.record(recordings, content);

            var topKeys = recordings.OrderByDescending(k => k.frequency);
            int totalKeys = topKeys.Sum(k => k.frequency);

            var table = new System.Text.StringBuilder();
            string header = $" {"Key Name".PadRight(15)} | {"Count".PadRight(12)} | {"%".PadRight(8)}";
            string divider = new string('-', header.Length);

            table.AppendLine(header);
            table.AppendLine(divider);

            foreach(var key in topKeys)
            {
              double percent = totalKeys > 0 ? (double)key.frequency / totalKeys * 100 : 0;
              table.AppendLine($" {key.name.PadRight(15)} | {key.frequency.ToString().PadRight(12)} | {percent:F1}%");
              table.AppendLine(divider);
            }

            programAfterRunning.Text = table.ToString();
            programAfterRunning.Visible = true;

            scanB.Visible = false;

            programAfterRunning.Y = 0;
            programAfterRunning.X = 0;
            programAfterRunning.TextAlignment = Alignment.Start;


            //this is for the txt/json export
            exportToTxt = true;
            
            keyboardKey engine = new keyboardKey("", 0);
            engine.exportLayout(logsPath, layoutPath);
          }
        }
      };
      

      
      
      Application.Run(win);
      win.SetFocus();
      Application.Shutdown();
      //the end :)
    }
  }
}