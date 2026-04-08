using System;
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
      System.Console.WriteLine("This is TyperCounter. its a tool to help you visualize your most used keyboard keys. press");

    }
  }
}