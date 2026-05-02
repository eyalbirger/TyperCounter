using System;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;


//using Terminal.Gui;

namespace TyperCounter;

public class keyboardKey
{
  public string name { get; set; }
  public int frequency { get; set; }

  public keyboardKey(string name, int frequency)
  {
    this.name = name;
    this.frequency = frequency;
  }

  public static string normalizeKeyName(string key)
  {
    string k = key.Trim();
    return k switch
    {
        // System / Meta Keys
        "LWin" or "RWin" or "LeftMeta" or "RightMeta" or "LMeta" or "RMeta" or "Super" or "Command" => "Win",
        "LMenu" or "RMenu" or "LAlt" or "RAlt" or "AltLeft" or "AltRight" => "Alt",
        "LControlKey" or "RControlKey" or "LControl" or "RControl" or "CtrlLeft" or "CtrlRight" => "Ctrl",
        "LShiftKey" or "RShiftKey" or "LShift" or "RShift" or "ShiftLeft" or "ShiftRight" => "Shift",
        
        // Navigation & Editing
        "Back" or "Backspace" or "BackSpace" => "Backspace",
        "Return" or "Enter" => "Enter",
        "Capital" or "CapsLock" => "Caps Lock",
        "Escape" or "Esc" => "Esc",
        "Space" or "Spacebar" or " " => "Space",
        "Tab" => "Tab",
        "Snapshot" or "PrintScreen" or "PrtSc" => "PrtSc",
        "Scroll" or "ScrollLock" => "Scroll Lock",
        "Pause" or "Break" => "Pause",
        "Insert" or "Ins" => "Ins",
        "Delete" or "Del" => "Del",
        "Prior" or "PageUp" or "PgUp" => "PgUp",
        "Next" or "PageDown" or "PgDn" => "PgDn",
        
        // Arrows
        "Left" or "LeftArrow" => "←",
        "Right" or "RightArrow" => "→",
        "Up" or "UpArrow" => "↑",
        "Down" or "DownArrow" => "↓",

        // Top Row Numbers (D1, D2 etc are standard in C#)
        "D1" => "1", "D2" => "2", "D3" => "3", "D4" => "4", "D5" => "5",
        "D6" => "6", "D7" => "7", "D8" => "8", "D9" => "9", "D0" => "0",

        // Numpad
        "NumPad1" => "1", "NumPad2" => "2", "NumPad3" => "3",
        "NumPad4" => "4", "NumPad5" => "5", "NumPad6" => "6",
        "NumPad7" => "7", "NumPad8" => "8", "NumPad9" => "9", "NumPad0" => "0",
        "Decimal" => ".", "Add" => "+", "Subtract" => "-", "Multiply" => "*", "Divide" => "/",

        // Symbols (Depending on your logger, these might come as names)
        "Oemtilde" or "Oem3" or "Grave" => "~",
        "OemMinus" or "OemMinus" => "-",
        "Oemplus" or "OemPlus" => "=",
        "OemOpenBrackets" or "Oem4" => "[",
        "Oem6" or "OemCloseBrackets" => "]",
        "Oem5" or "OemPipe" => "\\",
        "Oem1" or "OemSemicolon" => ";",
        "Oem7" or "OemQuotes" => "'",
        "Oemcomma" or "OemComma" => ",",
        "OemPeriod" or "OemPeriod" => ".",
        "OemQuestion" or "Oem2" => "/",

        // Fallback
        _ => k
    };
  }
 


  public static List<keyboardKey> record(List<keyboardKey> list, string content)
  {
    if (content.Contains(Environment.NewLine) || content.Contains("\n"))
      {
        string[] lines = content.Split(new[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string line in lines)
        {
          string trimmedLine = line.Trim();
          string cleanName = normalizeKeyName(trimmedLine);

          var existingKey = list.FirstOrDefault(k => k.name.Equals(cleanName, StringComparison.OrdinalIgnoreCase));

          if (existingKey != null)
            existingKey.frequency++;

          else
            list.Add(new keyboardKey(cleanName, 1));
        }
      }
      return list;
  }

  public List<(string name, int count, double percent, string hex)> calcColor(List<keyboardKey> list, string layoutPath)
  {
    int totalKeys = list.Sum(k => k.frequency);

    double highestFrequency = list.Max(k => k.frequency);

    var result = new List<(string name, int count, double percent, string hex)>();

    string layoutContent = File.ReadAllText(layoutPath);
    foreach (var key in list)
    {
      double percent = totalKeys > 0 ? (double)key.frequency / totalKeys * 100 : 0;
      string hex;
  

      if (key.frequency <= 0 || !layoutContent.Contains(key.name, StringComparison.OrdinalIgnoreCase))
        hex ="#FFFFFF";
      
      else
      {
        //int colorVeriable = highestFrequency > 0 ?(int)(255 -((255.0*key.frequency)/highestFrequency)) : 255;
        double ratio = (double)key.frequency / highestFrequency;
        int colorVeriable = (int)(255 - (255.0 * Math.Pow(ratio, 0.3)));

        //i think red would be better, i will show the heatmap the best (i think)
        int[] color = {255, colorVeriable, colorVeriable};

        Color colorTransition = Color.FromArgb(color[0], color[1], color[2]);
        hex = ColorTranslator.ToHtml(colorTransition);
      }

      result.Add((key.name, key.frequency, percent, hex));
    }
    return result;
  }

  public void exportLayout(string logsPath, string layoutPath)
  {
    if (!File.Exists(logsPath))
      return;

    string folderName = "layout";

    if (!Directory.Exists(folderName))
      Directory.CreateDirectory(folderName);
    
    if (!File.Exists(layoutPath))
      File.WriteAllText(layoutPath, """ ["A", "S", "D", "F"] """);

    
    string content = File.ReadAllText(logsPath);
    List<keyboardKey> recordings = keyboardKey.record(new List<keyboardKey>(), content);
    var proccessedData = calcColor(recordings, layoutPath);

    string[] lines = File.ReadAllLines(layoutPath);
    List<string> outputLines = new List<string>();

    foreach (string line in lines)
    {
      string processedLine = line;
      processedLine = Regex.Replace(processedLine, "(?<!{c:\"#[A-Fa-f0-9]{6}\"},)\"([^\"]+)\"", "{c:\"#FFFFFF\"},\"$1\"");
      
      foreach (var item in proccessedData.OrderByDescending(k => k.name.Length))
      {
        string replacement = $"{{c:\"{item.hex}\"}},\"{item.name}\"";
        string whiteTarget = $"{{c:\"#FFFFFF\"}},\"{item.name}\"";

        processedLine = Regex.Replace(
          processedLine,
          Regex.Escape(whiteTarget),
          replacement,
          RegexOptions.IgnoreCase
        );
      }
      outputLines.Add(processedLine);
    }

    string outputPath = Path.Combine(folderName, "layoutOutput.txt");
    File.WriteAllLines(outputPath, outputLines);
  }
}                           
