using System;
using System.Drawing;
using System.Linq;
using System.IO;


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


  public static List<keyboardKey> record(List<keyboardKey> list, string content)
  {
    if (content.Contains(Environment.NewLine) || content.Contains("\n"))
      {
        string[] lines = content.Split(new[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
          string trimmedLine = line.Trim();

          var existingKey = list.FirstOrDefault(k => k.name == trimmedLine);

          if (existingKey != null)
            existingKey.frequency++;
          else
            list.Add(new keyboardKey(trimmedLine, 1));
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

    string outputPath = Path.Combine(folderName, "layoutOutput.txt");
    
    string content = File.ReadAllText(logsPath);
    List<keyboardKey> recordings = keyboardKey.record(new List<keyboardKey>(), content);
    var proccessedData = calcColor(recordings, layoutPath);

    string layoutContent = File.ReadAllText(layoutPath);

    foreach (var item in proccessedData)
    {
      string replacement = $"{{c: \"{item.hex}\"}}, \"{item.name}\"";

      string targetLower = $"\"{item.name.ToLower()}\"";
      string targetUpper = $"\"{item.name.ToUpper()}\"";

      if(layoutContent.Contains(targetLower))
        layoutContent = layoutContent.Replace(targetLower, replacement);

      if (layoutContent.Contains(targetUpper))
        layoutContent = layoutContent.Replace(targetUpper, replacement);
    }

    File.WriteAllText(outputPath, layoutContent);
  }

}                           
