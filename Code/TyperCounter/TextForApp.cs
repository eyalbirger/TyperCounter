using System;

namespace TyperCounter;

public class TextForApp
{
static private string TC_title = @"
  ______                         ______                  __              
 /_  __/_  ______  ___  _____   / ____/___  __  ______  / /____  _____ 
  / / / / / / __ \/ _ \/ ___/  / /   / __ \/ / / / __ \/ __/ _ \/ ___/ 
 / / / /_/ / /_/ /  __/ /     / /___/ /_/ / /_/ / / / / /_/  __/ /     
/_/  \__, / .___/\___/_/      \____/\____/\__,_/_/ /_/\__/\___/_/      
    /____/_/                                                           ";

  static private string introductionText = 
@"Hello user.
The moment you press 'Space' again the program will start.
that means every time you press on a key on your keyboard,
it will be recorded in a .txt file.
I (the creator) DO NOT have acces to this file (you can't uplaod it to github).

Go to the GitHub page for help.
Press 'Esc' to close this app.
Press the 'Stop' button after you finish recording your key presses.";

  static private string areYouSureText = @"Are You Sure (omni-man meme)? (y/n)";

  static private string textAfterRecording = @"Program stopped.
Press to scan: ";

  public TextForApp()
  {
  }

  public string getText(string Vname)
  {
    switch(Vname)
    {
      case "TC_title":
        return TC_title;
      case "introductionText":
        return introductionText;
      case "areYouSureText":
        return areYouSureText;
      case "textAfterRecording":
        return textAfterRecording;
      
      //i dono i like this to be last i dont know i it does something else if its not last
      default:
        return "text not found";
    }
  }
}
