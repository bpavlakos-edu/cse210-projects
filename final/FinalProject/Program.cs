using System;
using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using UiMenuExitException = QuickUtils.UiMenuExitException; //Exit exception for the menu to use
using Inp = QuickUtils.Inputs; //Added for testing
using Msc = QuickUtils.Misc; //Added to load files
class Program
{
    //Global Variables
    static bool _autoSave = true; //Boolean setting for auto saving the config file when you exit settings
    static bool _skipIntro = false; //Boolean setting for displaying "Press Enter to Begin"
    static List<GameMode> _gameModeList = new List<GameMode>{new GmClassic(), new GmRandom(), new GmBlink(), new GmGrow(), new GmDecay()};
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
            new UiOption(()=>{_gameModeList[0].Start(_mainDiceSet, _skipIntro);},"Play the &Classic Game Mode"),
            new UiOption(()=>{_gameModeList[1].Start(_mainDiceSet, _skipIntro);},"Play the &Random Game Mode"),
            new UiOption(()=>{_gameModeList[2].Start(_mainDiceSet, _skipIntro);},"Play the &Blink Game Mode"),
            new UiOption(()=>{_gameModeList[3].Start(_mainDiceSet, _skipIntro);},"Play the &Grow Game Mode"),
            new UiOption(()=>{_gameModeList[4].Start(_mainDiceSet, _skipIntro);},"Play the &Decay Game Mode"),
            new UiOption(ShowGmHelp,"Open the &Help Menu"),
            new UiOption(OptionsMenu,"Open the &Options Menu"),
            new UiOption(()=>{throw new UiMenuExitException();},"&Exit")
        },
        $"Welcome to Doggle!{Environment.NewLine}Main Menu:",
        "Select a Game Mode or [hotkey] from the menu: ",
        "Thank you for playing!"
        //Use default settings for the other parameters
    );
    static void Main(string[] args)
    {
        SaveConfigFile("d_r_temp.cfg"); //Create a temporary file using the default values, to load if the user wants to reset all settings
        LoadConfigFile();
        _mainMenu.UiLoop(); //Open the main menu
        SaveConfigFile();
        DeleteResetConfig(); //Delete the temporary config file
    }
    static void ShowGmHelp()
    {
        UiMenu _helpMenu = new UiMenu(
            new List<UiOption>
            {
                new UiOption(ControlHints,"Controls and &Hints"),
                new UiOption(_gameModeList[0].DisplayHelp,"About the &Classic Game Mode"),
                new UiOption(_gameModeList[1].DisplayHelp,"About the &Random Game Mode"),
                new UiOption(_gameModeList[2].DisplayHelp,"About the Bli&nk Game Mode"),
                new UiOption(_gameModeList[3].DisplayHelp,"About the &Grow Game Mode"),
                new UiOption(_gameModeList[4].DisplayHelp,"About the &Decay Game Mode"),
                new UiOption(()=>{throw new UiMenuExitException();},"Go &Back"),
            },
            "Main Menu > Help Menu:"
        );
        _helpMenu.UiLoop();
    }
    //Hints
    static void ControlHints()
    {
        Console.WriteLine("Controls and Hints:");
        Console.WriteLine("Press \"P\" to pause or unpause the timer, even if it's not visible");
        Console.WriteLine("Press \"X\" to end a game early");
        Console.WriteLine("Change game mode settings to match your preferences");
        Console.WriteLine("You can change both dice grid dimensions at once using the \"Grid Size\" setting in the Dice Menu");
        Console.WriteLine("You can manually save your settings to a file");
        Console.WriteLine("The default configuration file will be loaded when the program starts");
        Console.WriteLine("The default configuration file will be saved when the program ends");
        Console.WriteLine("The default configuration auto saves when exiting the options menu, unless you disable the option for it");
        Console.WriteLine("");
        Inp.GetInput("Press enter to continue");
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
                new UiOption(ResetAllSettings,"&Reset All Settings and Config File"),
                new UiOption(()=>{return _autoSave;},(bool newVal)=>{_autoSave = newVal;},"Allow &Auto Save Config File when Exiting Options"),
                new UiOption(()=>{throw new UiMenuExitException();},"Go &Back")
            },
            "Main Menu > Options:",
            "Select an option or [hotkey] from the menu: ",
            ""
        );
        _rootOptionsMenu.UiLoop();
        if(_autoSave)
        {
            SaveConfigFile();
        }
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
                new UiOption(_gameModeList[3].OpenSettings,"&Grow Mode Options"),
                new UiOption(_gameModeList[4].OpenSettings,"&Decay Mode Options"),
                new UiOption(()=>{return _skipIntro;},(bool enabled)=>{_skipIntro = enabled;},"&Skip Game Mode Intro"),
                new UiOption(()=>{throw new UiMenuExitException();},"Go &Back"),
            },
            "Main Menu > Options > Game Mode Options:",
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
            int? durationSec = null;
            bool? showCDown = null;
            UiMenu _allGmSettingsMenu = new UiMenu(
                new List<UiOption>
                {
                    new UiOption(() => 
                    {
                        durationSec = Inp.GetIntInput(true,"Please enter the new duration (leave blank to cancel): ", 1, null);
                    },
                    "Set &Timer Length in Seconds for All Game Modes"),
                    new UiOption(() => 
                    {
                        showCDown = Inp.GetBoolInput(true,"Would you like to enable the timer across all game modes? (leave blank to cancel): ",curValue:null);
                    },
                    "Enable or Disable Timer &Display for All Game Modes"),
                    new UiOption(() => {throw new UiMenuExitException();},"Go &Back") //Give us the exit option
                },
                "Main Menu > Options > Game Mode Options > All Game Mode Options:",
                exitMsg:"" //Get rid of the exit message
            );
            _allGmSettingsMenu.UiLoop(); //Open the menu
            //Todo: Find a way to only apply settings if they are changed, so default values don't overwrite custom ones
            for(int i = 0; i < _gameModeList.Count; i++)
            {
                _gameModeList[i].SetDurationSec(durationSec ?? _gameModeList[i].GetDurationSec());//Set value if input is not null, use the ternary operator to set it to it's current value if the variable is null
                _gameModeList[i].SetShowCDown(showCDown ?? _gameModeList[i].GetShowCDown());//Set value if input is not null, use the ternary operator to set it to it's current value if the variable is null
            }
        }
        //Consider using the input capture menu (options menu from the previous project)
        /*int? newDurationSec = null;
        bool? newShowCDown = null;*/
    }

    //Config file importing / exporting
    //Ui Helper Functions
    //Load Config from path with user input
    static void LoadConfigOption()
    {
        string filePath = Inp.GetInput("Enter the file path to load from (leave blank to cancel, [d] to load default): ", false, false);
        if(filePath != "")
        {
            filePath = (filePath.ToLower() == "d") ? null : filePath; //Replace d with null, so it uses the default path "doggle.cfg"
            LoadConfigFile(filePath, false);
        }
    }
    //Save config to path with user input
    static void SaveConfigOption()
    {
        string filePath = Inp.GetInput("Enter the file path to save to (leave blank to cancel, [d] to load default): ", false, false);
        if(filePath != "")
        {
            filePath = (filePath.ToLower() == "d") ? null : filePath; //Replace d with null, so it uses the default path "doggle.cfg"
            SaveConfigFile(filePath, false);
        }
    }
    //Actual Config File Loading
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
            if(!programStart) //When this isn't silently loading in the background
            {
                path = (path.Contains('\\') == false) ? $"{Environment.CurrentDirectory}\\{path}" : path; //If the path doesn't have a backslash, append the local directory
                Inp.GetInput($"File Loaded ({path}), Press Enter to Continue");
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
        catch(IOException e){Console.WriteLine($"A file loading Error has occured: {e.ToString()}");Inp.GetInput("Press enter to continue");}
        catch(UnauthorizedAccessException){Console.WriteLine($"Unable to load file {path}, please try again with permissions");Inp.GetInput("Press enter to continue");}
        catch(NotSupportedException e){Console.WriteLine($"This OS doesn't support opening files, {e.ToString()}");Inp.GetInput("Press enter to continue");}
    }
    //Load the values from config text lines
    static void LoadConfigValues(string configTextRaw)
    {
        //Process the lines before loading
        configTextRaw = configTextRaw.ReplaceLineEndings(";"); //Automatically remove all line endings by replacing them with ";" (Found this with intellisense)
        string[] fileLines = configTextRaw.Split(";",options:StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries); //Split by all instances of new line and remove all whitespace and empty entries //The options use bitwise to merge StringSplitOptions. I have no idea why 3 can't be manually chosen, since 3 == 2 & 1
        //Load each line
        for(int offset = 0; offset < fileLines.Length;) //No increment here! It will be handled inside the loop. This for loop is functioning like a while loop, with the ability to declare the counter at the start
        {
            _autoSave = Msc.ReadFileLine(fileLines, ref offset, "autoSaveConfigOnOptionsExit=").ToLower() != "false"; //Load the config file auto save flag //Treat unrecognized input as true
            _skipIntro = Msc.ReadFileLine(fileLines, ref offset, "skipGmIntro=").ToLower() == "true"; //Load the config file auto save flag //Treat unrecognized input as false
            //use ref to pass offset to classes
            //Load All Game Mode Settings
            List<Type> GmModeTypes = Msc.ListMap<GameMode,Type>(_gameModeList,(GameMode gmItem) => {return gmItem.GetType();}); //Use the ListMap function to get each game mode's type
            Dictionary<string,Type> GmTypesByString = new Dictionary<string, Type>(){{"gmClassic", new GmClassic().GetType()},{"gmRandom", new GmRandom().GetType()},{"gmBlink", new GmBlink().GetType()},{"gmGrow", new GmGrow().GetType()},{"gmDecay", new GmDecay().GetType()}}; //Create a dictionary containing each game modes type, it's accessible by string
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
            break; //Exit, our job is done
        }
    }
    static void SaveConfigFile(string path = "doggle.cfg", bool silent = true)
    {
        try
        {
            path ??= "doggle.cfg"; //Use the ternary null setter to set the path to default if it's nulled (by the key 'd')
            using(StreamWriter sWriter = new StreamWriter(File.Open(path,FileMode.Create))) //Get a stream writer which is much more useful in this situation
            {
                sWriter.WriteLine("autoSaveConfigOnOptionsExit=" + ((_autoSave) ? "true" : "false"));//Write the Auto Save Setting //Use the ternary operator to auto write "true" or "false" to represent the status of the boolean
                sWriter.WriteLine("skipGmIntro=" + ((_skipIntro) ? "true" : "false"));//Write the Skip Intro Setting //Use the ternary operator to auto write "true" or "false" to represent the status of the boolean
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
            Inp.GetInput("Press enter to continue");
        }
        catch(ArgumentException e) //Catch ArgumentNullException, ArgumentOutOfRangeException
        {
            Console.WriteLine($"{e.ToString()}");
            Inp.GetInput("Press enter to continue");
        }
    }
    //Delete the temporary config file
    static void DeleteResetConfig()
    {
        try
        {
            File.Delete("d_r_temp.cfg");
        }
        //Silently Catch Exceptions
        catch(IOException){}
        catch(ArgumentException){}
        catch(NotSupportedException){}
        catch(UnauthorizedAccessException){}
    }
    //Load Default settings using the temporary config file, and immediately save them
    static void ResetAllSettings()
    {
        LoadConfigFile("d_r_temp.cfg");
        SaveConfigFile("doggle.cfg", false);
    }

    //For testing basic functionality
    /*static void TestMode()
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
    }*/
/*     static void ResetSettings()
    {
        try
        {
            File.Delete("doggle.cfg");
        }
        catch(UnauthorizedAccessException)
        {
            Console.WriteLine("You do not have permission to delete the config file!");
        }
        catch(FileNotFoundException)
        {

        }
    } */
}