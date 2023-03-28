using System;
using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using UiMenuExitException = QuickUtils.UiMenuExitException; //Exit exception for the menu to use
using Inp = QuickUtils.Inputs; //Added for testing
class Program
{
    //Global Variables
    static List<GameMode> _gameModeList = new List<GameMode>{new GmClassic(), new GmRandom(), new GmBlink()};
    static DiceSet _mainDiceSet = new DiceSet(new List<Dice>
    {//Default dice list was digitized from a real boggle set
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
    },5,5); //It's a 5x5 grid, a total of 25 letters at once
    //UiMenu setup
    private static UiMenu _mainMenu = new UiMenu(
        new List<UiOption>
        {
            //new UiOption(()=>{TestMode();},"&Test Mode"), //For debugging //Disabled
            new UiOption(()=>{_gameModeList[0].Start(_mainDiceSet);},"Play the &Classic Game Mode"),
            new UiOption(()=>{_gameModeList[1].Start(_mainDiceSet);},"Play the &Random Game Mode"),
            new UiOption(()=>{_gameModeList[2].Start(_mainDiceSet);},"Play the &Blink Game Mode"),
            //new UiOption(()=>{_gameModeList[3].Start(_mainDiceSet);},"Play the &Grow Game Mode"),
            //new UiOption(()=>{_gameModeList[4].Start(_mainDiceSet);},"Play the &Decay Game Mode"),
            new UiOption(ShowGmHelp,"Open the &Help Menu"),
            new UiOption(OptionsMenu,"Open the &Options Menu"),
            new UiOption(()=>{throw new UiMenuExitException();},"&Exit")
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
    static void ShowGmHelp()
    {
        UiMenu _helpMenu = new UiMenu(
            new List<UiOption>
            {
                new UiOption(_gameModeList[0].DisplayHelp,"About the &Classic Game Mode"),
                new UiOption(_gameModeList[1].DisplayHelp,"About the &Random Game Mode"),
                new UiOption(_gameModeList[2].DisplayHelp,"About the Bli&nk Game Mode"),
                //new UiOption(_gameModeList[3].DisplayHelp,"About the &Grow Game Mode"),
                //new UiOption(_gameModeList[4].DisplayHelp,"About the &Decay Game Mode"),
                new UiOption(()=>{throw new UiMenuExitException();},"Go &Back"),
            }
        );
        _helpMenu.UiLoop();
    }
    //Pick the settings menu to open (GameMode or DiceSet)
    static void OptionsMenu()
    {
        UiMenu _rootOptionsMenu = new UiMenu(
            new List<UiOption>
            {
                new UiOption(GameModeOptionsMenu,"Open the &Game Mode Options Menu"),
                new UiOption(_mainDiceSet.OpenSettings,"Open the &Dice-Set Options Menu"),
                new UiOption(()=>{throw new UiMenuExitException();},"Go &Back"),
            },
            "Options:",
            "Select an option or [hotkey] from the menu: ",
            ""
        );
        _rootOptionsMenu.UiLoop();
    }
    //Pick which Game Mode to open the settings menu for (All, Classic, Random, or Blink)
    static void GameModeOptionsMenu()
    {
        UiMenu _gmOptionMenu = new UiMenu(
        new List<UiOption>
            {
                new UiOption(AllGameModeOptionsMenu,"Change &All Game Mode Options"),
                new UiOption(_gameModeList[0].OpenSettings,"&Classic Mode Options"),
                new UiOption(_gameModeList[1].OpenSettings,"&Random Mode Options"),
                new UiOption(_gameModeList[2].OpenSettings,"Bli&nk Mode Options"),
                //new UiOption(_gameModeList[3].OpenSettings,"&Grow Mode Options"),
                //new UiOption(_gameModeList[4].OpenSettings,"&Decay Mode Options"),
                new UiOption(()=>{throw new UiMenuExitException();},"Go &Back"),
            },
            "Game Mode Options:",
            "Select a game mode or [hotkey] from the menu: ",
            ""
        );
        _gmOptionMenu.UiLoop();
    }
    //Set all game mode settings
    static void AllGameModeOptionsMenu()
    {
        //Use the base game modes settings class to generate the prompts and properties to apply to the other game modes, 
        //make sure to use default settings and only change when it's changed, 
        //also make sure that individual game modes don't change when different from default settings, or give the user a warning
        if(_gameModeList.Count > 0)
        {
            GameMode _mainGm = new GameMode(); //Create a new instance of GameMode with the current global settings
            _mainGm.OpenSettings(); //Open the settings
            //Todo: Find a way to only apply settings if they are changed, so default values don't overwrite custom ones
            for(int i = 0; i < _gameModeList.Count; i++)
            {
                _gameModeList[i].SetDurationSec(_mainGm.GetDurationSec());
                _gameModeList[i].SetShowCDown(_mainGm.GetShowCDown());
            }
        }
        //Consider using the input capture menu (options menu from the previous project)
        /*int? newDurationSec = null;
        bool? newShowCDown = null;*/
    }
    //Config file importing / exporting
    static void LoadConfigText(string path = "doggle.cfg", bool programStart = true)
    {
        try
        {
            //Load file lines into memory
            string fileLinesRaw = "";
            using(StreamReader myFile = File.OpenText(path))
            {
                fileLinesRaw = myFile.ReadToEnd();
            }
            LoadConfigValues(fileLinesRaw);//Load the values from the lines
        }
        //Exceptions
        catch(FileNotFoundException) 
        {
            if(programStart)
            {
                SaveConfigStart();
            }
        } //This is not an error, it's intended behavior for when the file is missing
        catch(IOException e){Console.WriteLine($"An IO Error has occured {e.ToString()}");}
        catch(UnauthorizedAccessException){Console.WriteLine($"Unable to load file {path}, please try again with permissions");}
        catch(NotSupportedException e){Console.WriteLine($"This OS doesn't support opening files, {e.ToString()}");}
    }
    //Load the values from config lines
    static void LoadConfigValues(string configTextRaw)
    {
        configTextRaw.ReplaceLineEndings(";"); //Automatically remove all line endings by replacing them with ";" (Found this with intellisense)
        configTextRaw.Split(";",options:StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries); //Split by all instances of new line and remove all  //The options use bitwise to merge StringSplitOptions. I have no idea why 3 can't be manually chosen, since 3 == 2 & 1

    }
    static void SaveConfigStart(string path = "doggle.cfg", bool silent = true)
    {
        if(!silent)
        {
            Inp.GetInput($"File Saved ({Environment.CurrentDirectory}\\doggle.cfg), Press Enter to Continue");
        }
    }
    //For testing basic functionality
    static void TestMode()
    {
        DiceSet diceSetCopy = new DiceSet(_mainDiceSet);
        //diceSetCopy.Display();
        string userInput = "";
        while(userInput != "x")
        {
            userInput = Inp.GetInput("Press Enter to roll (commands: \"g\"-test base game mode class,\"x\"-exit,\"r\"-reset dice,\"h\"-hide random dice, \"?\"-Fill dice With Random char): ").ToLower();
            if(userInput == "r")
            {
                diceSetCopy = new DiceSet(_mainDiceSet);
            }
            else if(userInput == "g")
            {
                GameMode _gmTest = new GameMode(15);
                _gmTest.Start(_mainDiceSet);
            }
            else if(userInput == "h")
            {
                diceSetCopy.RandomHide(1,4);
            }
            else if(userInput == "?")
            {
                diceSetCopy.SetAll('?');
            }
            else if(userInput != "x")
            {
                diceSetCopy.RollAll();
            }
        }
    }
}