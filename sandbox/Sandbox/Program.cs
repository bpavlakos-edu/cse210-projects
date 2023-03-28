using System;
using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using UiMenuExitException = QuickUtils.UiMenuExitException; //Exit exception for the menu to use
using Inp = QuickUtils.Inputs; //Added for testing
using Msc = QuickUtils.Misc; //Added to load files
class Program
{
    //Global Variables
    //static bool _autoSave = true;
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
        LoadConfigFile();
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
                new UiOption(SaveConfigOption,"&Save Config File"),
                new UiOption(LoadConfigOption,"&Load Config File"),
                //new UiOption(()=>{return _autoSave;},(bool newVal)=>{_autoSave = newVal;},"&Auto Save"),
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
    //Ui Helper Functions
    static void LoadConfigOption()
    {
        string filePath = Inp.GetInput("Enter the file path to load from (leave blank to cancel, [d] to load default): ", false, false);
        if(filePath != "")
        {
            filePath = (filePath.ToLower() == "d") ? null : filePath; //Replace d with null, so it uses the default path "doggle.cfg"
            LoadConfigFile(filePath, false);
        }
    }
    static void SaveConfigOption()
    {
        string filePath = Inp.GetInput("Enter the file path to save to (leave blank to cancel, [d] to load default): ", false, false);
        if(filePath != "")
        {
            filePath = (filePath.ToLower() == "d") ? null : filePath; //Replace d with null, so it uses the default path "doggle.cfg"
            SaveConfigFile(filePath, false);
        }
    }
    static void LoadConfigFile(string path = "doggle.cfg", bool programStart = true)
    {
        try
        {
            //Load file lines into memory
            string fileLinesRaw = "";
            path ??= "doggle.cfg"; //Use the ternary null setter to set the path to default if it's nulled (by the key 'd')
            using(StreamReader myFile = File.OpenText(path))
            {
                fileLinesRaw = myFile.ReadToEnd(); //read to end to read all lines
            }
            LoadConfigValues(fileLinesRaw);//Load the values from the lines
            if(!programStart)
            {
                Console.WriteLine("File Reading Completed!");
            }
        }
        //Exceptions
        catch(FileNotFoundException) 
        {
            if(programStart)
            {
                SaveConfigFile();
            }
        } //This is not an error, it's intended behavior for when the file is missing
        catch(IOException e){Console.WriteLine($"A file loading Error has occured: {e.ToString()}");}
        catch(UnauthorizedAccessException){Console.WriteLine($"Unable to load file {path}, please try again with permissions");}
        catch(NotSupportedException e){Console.WriteLine($"This OS doesn't support opening files, {e.ToString()}");}
    }
    //Load the values from config lines
    static void LoadConfigValues(string configTextRaw)
    {
        //Process the lines before loading
        configTextRaw.ReplaceLineEndings(";"); //Automatically remove all line endings by replacing them with ";" (Found this with intellisense)
        string[] fileLines = configTextRaw.Split(";",options:StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries); //Split by all instances of new line and remove all whitespace and empty entries //The options use bitwise to merge StringSplitOptions. I have no idea why 3 can't be manually chosen, since 3 == 2 & 1
        //Load each line
        for(int offset = 0; offset < fileLines.Length;) //No increment here! It will be handled inside the loop. This for loop is functioning like a while loop, with the ability to declare the counter at the start
        {
            //use ref to pass offset to classes
            //Load All Game Mode Settings
            List<Type> GmModeTypes = Msc.ListMap<GameMode,Type>(_gameModeList,(GameMode gmItem) => {return gmItem.GetType();}); //Use the ListMap function to get each game mode's type
            Dictionary<string,Type> GmTypesByString = new Dictionary<string, Type>(){{"gmClassic", new GmClassic().GetType()},{"gmRandom", new GmRandom().GetType()},{"gmBlink", new GmBlink().GetType()}/*,{"gmGrow", new GameMode().GetType()},{"gmDecay", new GameMode().GetType()}*/}; //Create a dictionary containing each game modes type, it's accessible by string
            while(Msc.ReadFileLine(fileLines, ref offset).Contains("GmName=")) //Repeat this block until the next line isn't "GmName="
            {
                try
                {
                    offset--; //Go back to the line that the while loop just read
                    string gmName = Msc.ReadFileLine(fileLines, ref offset,"GmName="); //Load the Game Mode's name
                    int gmIndex = GmModeTypes.IndexOf(GmTypesByString[gmName]); //Get the game modes index by looking for the first game mode with a matching "GetType()" result
                    _gameModeList[gmIndex].LoadFromFile(fileLines, ref offset); //Use each game mode's file load functions, by using the index we found, we garuntee that it loads using the correct index, we also ignore game modes that weren't changed
                }
                catch(KeyNotFoundException e) //Key errors are bad news, exit file reading because we're desynced!
                {
                    throw new IOException($"Config File Line: {offset}, Invalid game mode detected: {e.ToString()}"); //We can print the invalid game mode by printing the error as well
                }
            }
            //Load the dice set
            offset--; //Go back to the line that the while loop just read
            _mainDiceSet.LoadFromFile(fileLines, ref offset);
        }
    }
    static void SaveConfigFile(string path = "doggle.cfg", bool silent = true)
    {
        try
        {
            path ??= "doggle.cfg"; //Use the ternary null setter to set the path to default if it's nulled (by the key 'd')
            using(StreamWriter sWriter = new StreamWriter(File.Open(path,FileMode.Create))) //Get a stream writer which is much more useful in this situation
            {
                //Write all game modes to the file
                for(int i = 0; i < _gameModeList.Count; i++)
                {
                    _gameModeList[i].WriteToFile(sWriter); //Write the Current Game Mode Item
                }
                _mainDiceSet.WriteToFile(sWriter); //Write the DiceSet
            }
            if(!silent)
            {
                path = (path.Contains('\\') == false) ? $"{Environment.CurrentDirectory}\\{path}" : path; //If the path doesn't have a backslash, append the local directory
                Inp.GetInput($"File Saved ({path}), Press Enter to Continue");
            }
        }
        catch(IOException e) //Catch all IO Exceptions
        {
            Console.WriteLine($"{e.ToString()}");
        }
        catch(ArgumentException e) //Catch ArgumentNullException, ArgumentOutOfRangeException
        {
            Console.WriteLine($"{e.ToString()}");
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