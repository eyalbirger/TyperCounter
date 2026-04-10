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


}
