using System;
using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using UiMenuExitException = QuickUtils.UiMenuExitException; //Exit exception for the menu to use
class Program
{
    //Global Variables
    //private List<GameMode> _gameModeList = new List<GameMode>();
    /*private DiceSet _mainDice = new DiceSet(List<Dice>{

    });*/
    //UiMenu setup
    private static UiMenu _mainMenu = new UiMenu(new List<UiOption>{
            new UiOption(TestMode,"&Test Mode"), //For debugging
            new UiOption(()=>{},"&Classic"),
            new UiOption(()=>{},"&Random"),
            new UiOption(()=>{},"&Blink"),
            new UiOption(OptionsMenu,"&Options"),
            new UiOption(()=>{throw new OperationCanceledException();},"&Exit")
        },
        $"Welcome to Doggle!{Environment.NewLine}Menu Options:",
        "Select a Game Mode or [hotkey] from the menu: ",
        "Thank you for playing!"
        //Use default settings for the other parameters
    );
    static void Main(string[] args)
    {
        _mainMenu.UiLoop(); //Open the main menu
    }
    //To help the user pick which game mode
    static void OptionsMenu()
    {
        UiMenu _rootOptionsMenu = new UiMenu(new List<UiOption>{
            new UiOption(GameModeOptionsMenu,"&Game Mode Options"),
            new UiOption(()=>{},"&Dice Set Options"),
            new UiOption(()=>{throw new UiMenuExitException();},"Go &Back"),
        },
        "Options:",
        "Select an option or [hotkey] from the menu: ",
        ""
        );
    }
    //For picking which game mode the user wants to modify, including all game modes
    static void GameModeOptionsMenu()
    {
        
        UiMenu _gmOptionMenu = new UiMenu(new List<UiOption>
        {
            new UiOption(AllGameModeOptionsMenu,"&All Game Modes"),
            new UiOption(()=>{},"&Classic Mode"),
            new UiOption(()=>{},"&Random Mode"),
            new UiOption(()=>{},"&Blink Mode"),
            new UiOption(()=>{throw new UiMenuExitException();},"Go &Back"),
        },
        "Game Mode Options:",
        "Select a game mode or [hotkey] from the menu: ",
        ""
        );
    }
    static void AllGameModeOptionsMenu()
    {
        //Use the base game modes settings class to generate the prompts and properties to apply to the other game modes, 
        //make sure to use default settings and only change when it's changed, 
        //also make sure that individual game modes don't change when different from default settings, or give the user a warning
        //GameMode _mainGm = new GameMode(); //Create a new instance of GameMode with the current global settings
        //_mainGm.OpenSettings(); //Open the settings
    }

    static void TestMode()
    {

    }
}