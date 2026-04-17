using System;
using System.Linq;
using System.IO;
using Terminal.Gui;

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

  public List<(string name, int count, double percent, int[] color)> calcColor(List<keyboardKey> list)
  {
    int totalKeys = list.Sum(k => k.frequency);

    double highestFrequency = list.Max(k => k.frequency);

    var result = new List<(string name, int count, double percent, int[] color)>();

    foreach (var key in list)
    {
      double percent = totalKeys > 0 ? (double)key.frequency / totalKeys * 100 : 0;

      int colorVeriable = highestFrequency > 0 ?(int)((255.0*key.frequency)/highestFrequency) : 0;

      //i think red would be better, i will show the heatmap the best (i think)
      int[] color = {255, colorVeriable, colorVeriable};

      result.Add((key.name, key.frequency, percent, color));
    }

    return result;
  }


}                           
