using System;
using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using UiMenuExitException = QuickUtils.UiMenuExitException; //Exit exception for the menu to use
class Program
{
    //Global Variables
    //private List<GameMode> _gameModeList = new List<GameMode>();
    private DiceSet _mainDice = new DiceSet(
        new List<Dice>
        {
            new Dice(new List<char>{'N','G','M','A','N','E'}),
            new Dice(new List<char>{'E','T','I','L','C','I'}),
            new Dice(new List<char>{'N','R','L','D','D','O'}),
            new Dice(new List<char>{'A','I','A','F','R','S'}),
            new Dice(new List<char>{'D','L','O','N','H','R'}),
            new Dice(new List<char>{'E','A','E','A','E','E'}),
            new Dice(new List<char>{'W','Z','C','C','T','S'}),
            new Dice(new List<char>{'D','L','O','H','H','R'}),
            new Dice(new List<char>{'P','L','E','T','C','I'}),
            new Dice(new List<char>{'H','Y','R','R','P','I'}),
            new Dice(new List<char>{'S','C','T','I','P','E'}),
            new Dice(new List<char>{'Z','X','Q','K','B','J'}),
            new Dice(new List<char>{'F','S','A','R','A','A'}),
            new Dice(new List<char>{'S','F','R','Y','I','P'}),
            new Dice(new List<char>{'N','O','W','O','T','U'}),
            new Dice(new List<char>{'N','A','N','D','E','N'}),
            new Dice(new List<char>{'I','I','E','I','T','T'}),
            new Dice(new List<char>{'S','S','S','N','U','E'}),
            new Dice(new List<char>{'T','T','O','T','E','M'}),
            new Dice(new List<char>{'M','E','A','G','E','U'}),
            new Dice(new List<char>{'W','R','O','V','R','G'}),
            new Dice(new List<char>{'O','T','T','U','O','O'}),
            new Dice(new List<char>{'H','H','T','O','D','N'}),
            new Dice(new List<char>{'E','E','E','E','M','A'}),
            new Dice(new List<char>{'S','F','A','Y','I','A'})
        },
        5,
        5);
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