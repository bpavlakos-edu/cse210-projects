using System;
using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
class Program
{
    //Global Variables
    //private List<GameMode> _gameModeList = new List<GameMode>();
    //UiMenu setup
    private static UiMenu _mainMenu = new UiMenu(new List<UiOption>{
            new UiOption(()=>{},"&Classic"),
            new UiOption(()=>{},"&Random"),
            new UiOption(()=>{},"&Blink"),
            new UiOption(OptionsMenu,"&Options"),
            new UiOption(()=>{throw new OperationCanceledException();},"&Exit")
        },
        $"Welcome to Doggle!{Environment.NewLine}Menu Options:",
        "Select a Game Mode or [hotkey] from the menu: ",
        "Thank you for playing!"
        //Use default settings for the other 
    );
    static void Main(string[] args)
    {
        _mainMenu.UiLoop(); //Open the main menu
    }
    static void OptionsMenu()
    {
        UiMenu _rootOptionsMenu = new UiMenu(new List<UiOption>{
            new UiOption(()=>{},"&Game Mode Options"),
            new UiOption(()=>{},"&Dice Set Options"),
            new UiOption(()=>{throw new OperationCanceledException();},"Go &Back"),
        },
        "Options:",
        "Select an option or [hotkey] from the menu: "
        );
    }
}