/* 
A note about deep copying / cloning lists:
All lists share reference to the original list's items in memory
A very long time ago I found this solution on stack overflow:
https://stackoverflow.com/questions/15330696/how-to-copy-list-in-c-sharp
Unfortunately it was incorrect, because any modifcations to a mutable object inside the copied list would change objects inside of the original
The only way to avoid this is to deep copy your object manually, or implement a different type of list
Here are some posts about this topic:
https://stackoverflow.com/questions/4226747/deep-copy-of-listt
https://stackoverflow.com/questions/2774099/tolist-does-it-create-a-new-list
https://stackoverflow.com/questions/14007405/how-create-a-new-deep-copy-clone-of-a-listt
So because I don't want to deal with any of this nonsense, I will brute force it into working
*/

using UiMenu = QuickUtils.UiMenu; //Ui Menu Class
using UiOption = QuickUtils.UiOption; //UI Option
using Inp = QuickUtils.Inputs; //User Input Methods
using UiMenuExitException = QuickUtils.UiMenuExitException; //Exit exception for the menu to use
using UiMenuRefreshException = QuickUtils.UiMenuRefreshException; //Refresh exception for the menu to use
using Msc = QuickUtils.Misc; //Misc methods for repetative operations

class DiceSet
{
    //Attributes
    protected List<Dice> _diceList = new List<Dice>();
    protected int _width = 5;
    protected int _height = 5;
    protected bool _allowAutoFill = true; //Enable or disable auto filling of this dice set
    protected bool _allowQu = true; //Enable or disable displaying Q as Qu
    protected char[] _diceBorder = new char[]{'\u0000','\u0000'}; //Warning for settings menu! You must be able to reset this from the console!

    //Threaded flag
    private bool _updating = false; //A flag for threaded functions to check to see if they can print to the console
    
    //Constructors
    
    //No blank constructor, it should never be blank!!!
    
    //Fill all attributes
    public DiceSet(List<Dice> diceList, int width = 5, int height = 5, bool allowAutoFill = true, bool allowQu = true, char[] diceBorder = null)
    {
        _diceList = diceList.ToList<Dice>(); //Copy the list to break the reference to the original
        _width = width;
        _height = height;
        _allowAutoFill = true;
        _allowQu = true;
        _diceBorder = diceBorder ?? new char[]{'\u0000','\u0000'};
    }
    //Fill all attributes but use the default dice list (also doubles as the blank constructor)
    public DiceSet(int width = 5, int height = 5, bool allowAutoFill = true, bool allowQu = true, char[] diceBorder = null)
    {
        _width = width;
        _height = height;
        DiceListToDefault(); //Update the diceList to be the default values
        _allowAutoFill = true;
        _allowQu = true;
        _diceBorder = new char[]{'\u0000','\u0000'};
    }

    //Copy an existing DiceSet so GameModes can modify it without changing the original
    public DiceSet(DiceSet sourceDiceSet) : this(sourceDiceSet.GetDiceList(), sourceDiceSet.GetWidth(), sourceDiceSet.GetHeight(), sourceDiceSet.GetAllowAutoFill(), sourceDiceSet.GetAllowQu(), sourceDiceSet.GetDiceBorder())
    {
        //Use the Fill all fields constructor to re-use code, and make it easier to change
        /*_diceList = sourceDiceSet._diceList;
        _width = sourceDiceSet._height;
        _height = sourceDiceSet._width;*/
    }
    //Special copy constructor, auto fills remaining dice slots when the autoFillFlag constructor is enabled
    public DiceSet(DiceSet sourceDiceSet, bool forceAutoFill) : this(sourceDiceSet) //Use the original copy constructor to create the inital object
    {
        if((_allowAutoFill || forceAutoFill) && CheckSize()) //Only run this check when allow auto fill is true
        {
            FillToCount(_width * _height, sourceDiceSet.GetDiceList(false).ToArray()); //Use the dice list to fill the new dice list
        }
    }

    //Getters and Setters (Normal external access Getters and Setters were auto generated using my AutoGetterSetter Python Script in C# mode)
    public List<Dice> GetDiceList(bool deepCopy = true)
    {
        if(!deepCopy)
        {
            return _diceList.ToList<Dice>();
        }
        else
        {
            return Msc.ListCopy<Dice>(_diceList, (Dice inDice) => {return new Dice(inDice);}); //Use the new ListCopy function
            /* List<Dice> diceListCopy = new List<Dice>();
            foreach(Dice diceItem in _diceList)
            {
                diceListCopy.Add(new Dice(diceItem)); //Clone it with a constructor designed to copy the fields of a dice object
            }
            return diceListCopy; */
        }
    }
    public void SetDiceList(List<Dice> diceList)
    {
        _diceList = Msc.ListCopy<Dice>(diceList, (Dice inDice) => {return new Dice(inDice);});
        //_diceList = diceList.ToList<Dice>();
    }
    public int GetWidth()
    {
        return _width;
    }
    public void SetWidth(int width)
    {
        _width = width;
    }
    public int GetHeight()
    {
        return _height;
    }
    public void SetHeight(int height)
    {
        _height = height;
    }
    //Custom Getters and Setters
    //Check the async updating flag status
    public bool GetUpdating()
    {
        return _updating;
    }
    public void SetUpdating(bool updating) //Please don't set this
    {
        _updating = updating;
    }
    //allowQu option
    public bool GetAllowQu()
    {
        return _allowQu;
    }
    public void SetAllowQu(bool allowQu)
    {
        _allowQu = allowQu;
    }
    //Autofill option
    public bool GetAllowAutoFill()
    {
        return _allowAutoFill;
    }
    public void SetAllowAutoFill(bool allowAutoFill)
    {
        _allowAutoFill = allowAutoFill;
    }
    //Dice wall characters
    public char[] GetDiceBorder()
    {
        return _diceBorder;
    }
    //Automatically handles null arrays, ignores entries in arrays larger than 2, and automatically duplicates arrays with length 1
    public void SetDiceBorder(bool resetOnInvalid = true, params char[] diceBorder)
    {
        if(diceBorder != null && diceBorder.Length > 0) //Only assing
        {
            _diceBorder = (diceBorder.Length > 1) ? new char[]{diceBorder[0],diceBorder[1]} : new char[]{diceBorder[0],diceBorder[0]}; //Let an input of 1 char fill both slots, this will let the user type "|" to fill both, otherwise just fill using the first and second entry
        }
        else if(resetOnInvalid)
        {
            _diceBorder = new char[]{'\u0000','\u0000'};
        }
        //When reseting is disabled, ignore it
    }
    //Function overload to use only the diceBorder, which was the format before adding the resetOnInvalid flag
    public void SetDiceBorder(params char[] diceBorder)
    {
        SetDiceBorder(true, diceBorder);
    }
    //Constructor to automatically handle string input of any kind
    public void SetDiceBorder(string diceBorderString, bool resetOnInvalid = true)
    {
        char[] diceBorder = diceBorderString.Replace(",",null).Replace(" ",null).ToCharArray(); //Yes this is very confusing, but this lets us treat each character as one of the border entries, even if it's entered wrong. Because we have these rules the following is true: "[,]" == "[ ]" == "[]" == '[',']'
        SetDiceBorder(diceBorder);
    }

    //Use a dice set to set the values of this dice set (useful for file loading)
    public void SetDiceSet(DiceSet newDiceSet)
    {
        _diceList = newDiceSet.GetDiceList();
        _width = newDiceSet.GetWidth();
        _height = newDiceSet.GetHeight();
        _allowQu = newDiceSet.GetAllowQu();
        _allowAutoFill = newDiceSet.GetAllowAutoFill();
        _diceBorder = newDiceSet.GetDiceBorder();
    }
    //Get and set using the largest axis of the grid
    public int GetGridSize()
    {
        return (GetWidth() >= GetHeight()) ? GetWidth() : GetHeight(); //Return the largest of the two dimensions
    }
    public void SetGridSize(int newSize)
    {
        SetWidth(newSize);
        SetHeight(newSize);
    }
    //Quickly calculate the grid area
    public int GetGridArea()
    {
        return GetWidth() * GetHeight();
    }

    //Methods

    //Main Functionality

    //Display the letters in a grid
    //Quick Dice Display, uses modulus, char[] buffers to display dice, if you think this is unecessary, try doing 70 x 70 grid with the original display function and compare it to this one, this is much faster!!!
    public void Display()
    {
        _updating = true; //Let asyncronous functions know we are updating
        Console.Clear(); //Clear the console
        char dWallStart = _diceBorder[0]; //Load from the settings
        char dWallEnd = _diceBorder[1]; //Load from the settings
        bool hasQu = _allowQu && _diceList.Exists((Dice inputDice) => {return inputDice.GetCurLetter() == 'Q';}); //"Exists" uses predicates (inline functions that return when a boolean is met) to search a list
        char[] newLineChars = Environment.NewLine.ToCharArray(); //Convert the newline 
        int newLineLength = newLineChars.Length; //Store the new line length so we don't need to repeatedly calculate it
        //Create a display buffer, with it's length specified using the following calculations:
        int cellSize = 2 + ((dWallStart > 0) ? 1 : 0) + ((dWallEnd > 0) ? 1 : 0); //cell size by default is 2, use a ternary operator to add 1 if dWallStart is not 0, do the same if dWallEnd is not 0
        int rowWidth = (cellSize * _width) + newLineLength; //Store the row width so we don't need to repeatedly calculate it
        char[] stringBuffer = new char[rowWidth * _height]; //Initalize the string buffer
        int offset = 0; //Have an offset that is used to access the buffer position
        try 
        {//Write all characters to the buffer
            for(int i = 0; i < _width * _height; i++) //For each item in the array
            {
                (_diceList[i].ToDisplayChars(hasQu, dWallStart, dWallEnd)).CopyTo(stringBuffer, offset); //Copy the display characters to the buffer
                offset += cellSize; //Offset by the cell size
                if((i + 1) % _width == 0) //Write the new line after the last entry
                {
                    newLineChars.CopyTo(stringBuffer, offset); //Copy the newline characters
                    offset += newLineLength; //Update the offset by the length of the new line
                }
            }
        }
        catch(ArgumentOutOfRangeException) //No more dice, write the remaining new lines
        {
            //Get to the nearest new line position
            offset += (rowWidth - (offset % rowWidth)) - newLineLength; //offset % rowWidth gets our current row position, subtract it from the row width to get to the end of the line, subtract the newLine length to get to the new line poistion
            while(offset < rowWidth * _height) //Repeat until all new line chars have been added
            {
                newLineChars.CopyTo(stringBuffer, offset); //Copy the newline characters
                offset += newLineLength; //Update the offset by the length of the new line
            }
        }
        Console.Write(stringBuffer); //Write the whole buffer to the console
        _updating = false; //Let asyncronous functions know we are no longer updating
    }

    //Roll All Dice
    public void RollAll()
    {
        Shuffle(); //Shuffle Dice
        _diceList.ForEach((curDice) => {curDice.Roll();}); //Roll each dice using ForEach, with a lambda (Action with input type dice) call to the current dice's Roll method
        Display(); //Display after rolling
    }

    //Randomly Change a dice's hidden state
    public int RandomHide(int rChance = 1, int rChanceMax = 4, int? rCountMax = null)
    {
        int hiddenCount = 0; //Reset the hidden count tracker (this will be used in the future to control the hidden maximum)
        _diceList.ForEach((curDice) => {
            curDice.ToggleHidden(rChance, rChanceMax);
            if(curDice.GetHidden()) //Increment the hidden counter for each dice that is hidden
            {
                hiddenCount++;
            }
        });
        Display(); //Display the dice after randomly hiding
        return hiddenCount; //Return the number of hidden dice
    }

    //Open the settings menu
    public void OpenSettings()
    {
        UiMenu diceSetSettings = new UiMenu(new List<UiOption>
            {
                new UiOption(OpenDiceListSettingsMenu,"Edit &Dice-List",updateStrFun:()=>{return $" (Number of Dice: {_diceList.Count})";}), //Open Dice Edit Menu
                new UiOption(GetWidth, SetWidth, "Grid &Width", 2), //Set Width
                new UiOption(GetHeight, SetHeight, "Grid &Height", 2), //Set Height
                new UiOption(GetGridSize, SetGridSize, "Grid &Size", 2), //Set Height
                new UiOption(GetAllowAutoFill, SetAllowAutoFill,"&Auto-Fill Dice to Grid Size in Game Modes"), //Allow auto-filling dice set
                new UiOption(GetAllowQu, SetAllowQu,"&Qu Allowed"), //Allow Qu Setting
                new UiOption(()=>{return new string(GetDiceBorder());}, (string inStr)=>{SetDiceBorder(inStr);},"D&ice Border"), //Dice border setting (By using lambdas with an input type of string for the getters and setters, it will use the UiOption constructor that auto generates GetString as the action)
                new UiOption(()=>{throw new UiMenuExitException();},"Go &Back"),
            },
            "Main Menu > Options > Dice-Set Options:"
        );
        diceSetSettings.UiLoop();
    }
    //Dice List Settings Menu
    public void OpenDiceListSettingsMenu()
    {
        bool refreshUi = true;
        do
        {
            UiMenu diceListSettings = new UiMenu(
                _diceList.ToList<object>(),
                (inputDice) => {((Dice)inputDice).OpenSettings();},
                "Open Dice $ Settings (Sides", //$ is replaced by index in loop + 1
                (inputDice) => {return ((Dice)inputDice).LettersToString()+")";}, //Add the parenthesis here to finish the display string
                "Go &Back",
                "Main Menu > Options > Dice-Set Options > Dice-List and Options:",
                "Select a dice number, choice, or [hotkey] from the menu: "
            );
            //Add additional Edit All Options before the exit button:
            //diceListSettings.AddOptionFromEnd(new UiOption(()=>{},"&Export Dice List to File"));
            //diceListSettings.AddOptionFromEnd(new UiOption(()=>{},"&Import Dice List from File")); //Should be included in export settings
            diceListSettings.AddOptionFromEnd(
                new List<UiOption>
                {
                    new UiOption(ShowDiceCode,"&Generate a Dice-List Code for Sharing"),
                    new UiOption(ExportDiceSetCode,"E&xport a Dice-List Code to a File"),
                    new UiOption(()=>{EnterDiceCode(); throw new UiMenuRefreshException();},"&Enter Dice-List Code"), //Needs parenthesis (and a lambda by extension) because it has a parameter
                    new UiOption(()=>{Shuffle(); throw new UiMenuRefreshException();},"&Shuffle Dice-List"),
                    new UiOption(()=>{EnterDiceCode(false); throw new UiMenuRefreshException();},"&Add New Dice Using Dice-List Code"),
                    new UiOption(()=>{RepeatAddDiceCode(); throw new UiMenuRefreshException();},"Re&peatedly Add Dice using Dice-List Code"),
                    new UiOption(()=>{DeleteDiceByUi(); throw new UiMenuRefreshException();},"&Delete Dice From List"),
                    new UiOption(()=>{ReplaceAllRandom(); throw new UiMenuRefreshException();},"Replace all Dice Sides With Ra&ndom Letters"), //Fill With Random Letter
                    new UiOption(()=>{ScrambleAll(); throw new UiMenuRefreshException();},"S&cramble All Dice Letters"), //Scramble
                    new UiOption(()=>{Shuffle(); ScrambleAll(); throw new UiMenuRefreshException();},"Shuff&le and Scramble All Dice Letters"), //Use a lambda function to use shuffle and scramble all in quick succession
                    new UiOption(()=>{MixAllDiceSides(); throw new UiMenuRefreshException();},"&Mix All Dice Sides"), //Mix all dice sides together
                    new UiOption(()=>{ForceDiceSizeUi(); throw new UiMenuRefreshException();},"&Force All Dice Sides to Side Count"), //Force all sides to a specific side count
                    new UiOption(()=>{PrintDiceLetterFrequency(); throw new UiMenuRefreshException();},"Show Dice-List Letter Fre&quency"), //Force all sides to a specific side count
                    new UiOption(()=>{FillToCountUi(); throw new UiMenuRefreshException();},"F&ill to Count"), //Force all sides to a specific side count
                    new UiOption(()=>{DiceListToDefault(); throw new UiMenuRefreshException();},"&Reset Dice-List to Default")
                    /*Additional Option Ideas:
                    Fill all with dice code
                    Fill to requested dice count
                    Fill to dice count with letter pool
                    Fill with letters from set (Use @ to represent dice code letters to pick from)
                    */
                }
            );
            refreshUi = diceListSettings.UiLoop(); //When a UiMenuRefreshException occurs, the list will be refreshed
        }while(refreshUi);
    }
    //Ui Support Functions

    //Set to default dice list
    public void DiceListToDefault()
    {
        //Default dice list was digitized from a real boggle set
        _diceList = new List<Dice>{
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
        };
    }

    //Print a diceListCode to the console
    public void ShowDiceCode()
    {
        Console.WriteLine("Dice-List Code (Highlight and right click to copy):"); //If only this could be used without permissions: https://learn.microsoft.com/en-us/dotnet/api/system.windows.clipboard?view=windowsdesktop-7.0
        Console.WriteLine("");
        Console.WriteLine(GenerateDiceListCode());
        Console.WriteLine("");
        Inp.GetInput("Press enter to continue");
    }

    //Enter a diceList Code into the console
    public void EnterDiceCode(bool clearList = true, int addCodeCount = 1)
    {
        string diceCode = GetDiceCodeInput();
        LoadDiceListCode(diceCode, clearList);
        throw new UiMenuRefreshException();
    }

    //Get dice code from input
    public string GetDiceCodeInput()
    {
        Console.WriteLine("Dice-List Code Rules:");
        Console.WriteLine("Each letter represents 1 side of the dice");
        Console.WriteLine("Each dice can have a unique number of sides");
        Console.WriteLine("Add \",\" to seperate each dice entry");
        Console.WriteLine("\"?\" picks a random letter each time it's rolled in-game");
        Console.WriteLine("\"*\" picks a random letter to save as the side");
        Console.WriteLine("Invalid characters are ignored, letters aren't case-sensitive");
        Console.WriteLine("When the dice list is empty, it will automatically be filled by a single dice");
        return Inp.GetInput("Enter Your Dice-List Code (Leave blank to cancel):", null, true);
    }

    //Get a dice code and count to add the dice code repeatedly
    public void RepeatAddDiceCode()
    {
        int? addCodeCount = Inp.GetIntInput(true, "Enter the number of dice to add using your dice code (leave blank to cancel): ",0);
        if(addCodeCount != null)
        {
            string diceCode = GetDiceCodeInput(); //Get the dice code
            for(int i = ((diceCode.Length > 0 ) ? 0 : (int) addCodeCount); i < addCodeCount; i++) //Skip this for loop if the diceCode has a length of 0
            {
                LoadDiceListCode(diceCode, false); //clearList should never be true on any cycle other than 0 //forces upper case (toLower = null), newLine = true
            }
        }
    }

    //Generate the DiceListCode as a string
    public string GenerateDiceListCode(bool addComma = true)
    {
        int addCommaInt = (addComma) ? 1 : 0; //Create an iteger to multiply i by to hardcode it to 1 when the boolean is enabled and 0
        //Generate the string
        List<char> newDiceCodeBuffer = new List<char>(); //Use a char buffer, it's List<char> because each dice can have a different size (we are also avoiding arithmatic operations using string)
        for(int i = 0; i < _diceList.Count; i++) //Convert each dice to chars
        {
            _diceList[i].AppendToCharList(newDiceCodeBuffer, i * addCommaInt); //Use the dice's method to add them to the buffer, pass the index so it knows when to add a comma
        }
        return new string(newDiceCodeBuffer.ToArray<char>()); //Return it as a string
    }
    
    //Load a DiceListCode from a string, ignore blank codes
    public void LoadDiceListCode(string diceSetCode, bool clearList = true)
    {
        if(diceSetCode != "") //Ignore empty strings
        {
            //Make the new Dice list
            List<Dice> newDiceList = new List<Dice>();
            string[] diceStrArr = diceSetCode.Split(","); //Split the string by using "," This will tell us how many dice there are
            for(int i = 0; i < diceStrArr.Length; i++)
            {
                if(diceStrArr[i] != "") //Ignore empty strings, so we don't accidently fill the actual dice list with an empty list
                {
                    newDiceList.Add(new Dice(diceStrArr[i])); //Automatically create a dice from the string using the constructor made for this
                }
            }
            if(newDiceList.Count > 0) //Update the actual Dice List if the New Dice List isn't empty
            {
                if(clearList) //Clear the list when the flag is active
                {
                    _diceList.Clear(); 
                    _diceList = Msc.ListCopy<Dice>(newDiceList, (Dice inputDice)=>{return new Dice(inputDice);}); //Copy the dice list using the ListCopy Function
                }
                else
                {
                    _diceList.AddRange(Msc.ListCopy<Dice>(newDiceList, (Dice inputDice)=>{return new Dice(inputDice);}));  //Copy the dice list using the ListCopy Function
                }
            }
        }
    }

    //Delete Dice using user input
    public void DeleteDiceByUi()
    {
        PrintDiceList(); //Show the dice list
        List<int[]> deletionIndexes = Inp.GetRangeIntInput("Enter the ranges of sides to delete (\"-\" to make a range, \",\" to seperate numbers, \"!\" to select all): ",1,_diceList.Count,true,subtractNum:1,allowNull:true);
        for(int i = deletionIndexes.Count - 1; i >= 0; i--) //Reverse for loops go backwards so that the greatest item is removed first, to prevent index errors
        {
            for(int j = deletionIndexes[i][deletionIndexes[i].Length - 1]; j >= deletionIndexes[i][0]; j--) //Start at the end range, end when we are below the start range
            {//j = deletionIndexesEnd, j >= deletionIndexesStart, go backwards
                _diceList.RemoveAt(j); //Remove this dice from the dice list (Consider using removeAtRange)
            }
        }
        if(_diceList.Count == 0) //If it's empty by the end
        {
            _diceList.Add(new Dice("?")); //Add ? to fill it
        }
    }

    //Enter Dice Border with user input (Seperate because it needs to call a special method)
    public void EnterDiceBorder()
    {
        string userInput = Inp.GetInput("Enter Dice Border Characters (Leave Blank to Cancel, Invalid values are Ignored): ");
        if(userInput != "")
        {
            SetDiceBorder();
        }
    }

    //Fill to count by user input
    public void FillToCountUi()
    {
        Console.WriteLine("---- Warning! This option makes permanent changes to your dice set! Please use it carefully! ----");
        int? newCount = Inp.GetIntInput(true,"Select a count to set the dice-list to: ", 1, null, false, _diceList.Count);
        if(newCount != null)
        {
            FillToCount(newCount ?? _diceList.Count, Msc.ListCopy<Dice>(_diceList,(Dice inDice)=>{return new Dice(inDice);}).ToArray<Dice>());
        }
    }

    //Add a new dice to the list

    //Mass Dice Modification
    //Set all dice by a copy of a dice object
    public void SetAll(Dice newDice)
    {
        _diceList.ForEach((curDice) => {curDice.SetDice(newDice);}); //Use the SetDice(Dice newDice) setter method
    }
    //Set all to a specific letter, used by GmRandom
    public void SetAll(char fillChar)
    {
        _diceList.ForEach((curDice) => {curDice.SetDice(fillChar);}); //Use the SetDice(char fillChar) setter method
    }

    //Set the visibility of all dice
    public void SetAllVisibility(bool hidden = false)
    {
        _diceList.ForEach((curDice) => {curDice.SetHidden(hidden);}); //Use SetHidden() to set the hidden status of all dice
    }

    //Replace all sides of all dice with random letters
    public void ReplaceAllRandom()
    {
        _diceList.ForEach((curDice) => {curDice.RandomSideLetters();});
    }

    //Scramble all dice
    public void ScrambleAll()
    {
        _diceList.ForEach((curDice) => {curDice.ScrambleSides();});
    }

    //Utility

    //Shuffle Dice
    private void Shuffle(int shuffleCycleCount = 1)
    {
        List<Dice> shuffleDiceList = _diceList.ToList<Dice>(); //Clone the current dice list so we can carry over the changes we made in each shuffle cycle, without changing the real dice list until the end
        for(int i = 0; i < shuffleCycleCount; i++) //Shuffle repeat loop
        {
            //Shuffle the dice by temporarily storing random items from the new list in the dice push list
            List<Dice> storeRandomDiceList = new List<Dice>(); //A place to store the entries we pull from shuffleDiceList
            while(shuffleDiceList.Count > 0) //Repeat until shuffleDiceList is empty
            {
                int randomIdx = Msc.RandomInt(0,shuffleDiceList.Count); //Pick a random index in shuffleDiceList (Todo: consider bit shifting the seed by the capacity of the list)
                storeRandomDiceList.Add(shuffleDiceList[randomIdx]); //Add the item from the source list to the new list
                shuffleDiceList.RemoveAt(randomIdx); //Remove it from the shuffle dice list
            }
            shuffleDiceList = storeRandomDiceList.ToList<Dice>(); //Copy the contents of storeRandomDiceList
        }
        _diceList = shuffleDiceList.ToList<Dice>(); //Store the shuffled Dice
    }
    //Ask if the user wants to fill the remaining slots in the Dice List automatically, this can be called from inside game modes
    //Kind of redundant since we now have the constructor, also, it doesn't 
    public void RequestSizeCheck(bool autoFillEnabled = true)
    {
        if(autoFillEnabled && CheckSize())
        {
            FillToCount(_width * _height, Msc.ListCopy<Dice>(_diceList,(Dice inDice)=>{return new Dice(inDice);}).ToArray<Dice>()); //Use a copy of our own list to fill the array
        }
    }

    //Boolean to quickly check the size, to make sure it's valid
    private bool CheckSize()
    {
        return _diceList.Count < (_width * _height);
    }

    //Fill the dice list to an integer
    //Fill using a list of dice
    public void FillToCount(int newDiceCount, params Dice[] inputDice) //Utilizes the params keyword, which lets us use each item as an individual parameter: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params
    {
        if(_diceList.Count <= newDiceCount)
        {
            for(int i = 0; _diceList.Count < newDiceCount; i++)
            {
                _diceList.Add(new Dice(inputDice[i % inputDice.Length])); //Use modulus to infinitely loop the input array
            }
        }
        else if(_diceList.Count < newDiceCount) //Remove dice to count
        {
            bool userContinue = Inp.GetBoolInput("This action will permanently delete dice, are you sure you want to continue?: ",curValue:false);
            if(userContinue != false)
            {
                while(_diceList.Count > newDiceCount) //Delete dice repeatedly until we reach the count
                {
                    _diceList.RemoveAt(_diceList.Count - 1); //Remove the last dice
                }
            }
        }
        
    }
    //Fill using chars
    public void FillToCount(int newDiceCount, params char[] inputChars)
    {
        FillToCount(newDiceCount, new Dice(inputChars.ToList<char>(), 0, ' ', false)); //Convert input chars to a list to use to create a new dice to send to the original function
    }

    //Randomly swap letters across all dice
    public void MixAllDiceSides()
    {
        //Put all dice sides in one giant list to pull from
        List<char> diceSidePool = GenerateDiceListCode(false).ToList<char>(); //Generate the dice code, Remove all commas, change the string into char[], change char[] into List<char> so the items are removable
        //V2 should just use another diceListCode and replace each letter randomly as it goes through the list
        foreach(Dice diceItem in _diceList)
        {
            char[] newSides = new char[diceItem.GetSideList().Count]; //Initalize a new side list of equal length to the original
            for(int i = 0; i < newSides.Length; i++)
            {
                int randomIdx = Msc.RandomInt(diceSidePool.Count); //Pick a random item from the dice side pool
                newSides[i] = diceSidePool[randomIdx]; //Assign the new letter to the index
                diceSidePool.RemoveAt(i);//Remove the randomly chosen side from the side pool
            }
            diceItem.SetSideList(new string(newSides));//Create a string from the newSides and assign it to the dice
        }
    }
    //Set all dice to a fixed size by user input
    public void ForceDiceSizeUi()
    {
        int? forcedSize = Inp.GetIntInput(true,"Enter the number of sides you want to force each dice to (Leave blank to cancel): ", 1);
        if(forcedSize != null)
        {
            ForceDiceSize((int) forcedSize); //Again, why do I have to type cast it here? The compiler should know it's not null here (nullable types must be seperate types)
        }
    }

    //Set all dice to a fixed size
    public void ForceDiceSize(int forcedSize)
    {
        forcedSize = (forcedSize > 0) ? forcedSize : 1; //Force it to use 1 by default
        List<char> sourceDiceSides = GenerateDiceListCode(false).ToList<char>();
        List<char> newDiceCodeChars = new List<char>();
        int leftoverCount = sourceDiceSides.Count % forcedSize; //Get the leftover from dividing by the forced size
        int newDiceCount = (sourceDiceSides.Count - leftoverCount) / forcedSize;
        for(int i = 0; i < newDiceCount; i++)
        {
            newDiceCodeChars.AddRange(sourceDiceSides.GetRange(i * forcedSize, forcedSize)); //Add the desired size of letters from the sourceDiceSides
            if(i < newDiceCount - 1) //For all entires except the last one, add a comma
            {
                newDiceCodeChars.Add(',');
            }
        }
        if(leftoverCount > 0)//If there are leftovers
        {
            newDiceCodeChars.Add(','); //Add a comma before the next part
            newDiceCodeChars.AddRange(sourceDiceSides.GetRange(newDiceCount * forcedSize, leftoverCount)); //Add the leftover letters, use the leftover count to get the size, and the newDiceCount * forcedSize to calculate the offset
        }
        LoadDiceListCode(new string(newDiceCodeChars.ToArray())); //Load the dice list code
    }

    //Query a number of dice that meet the required predicate, then run the action on them
    public void RandomQueryRun(int maxDiceCount, Predicate<Dice> diceFilter, Action<Dice> diceAction)
    {
        List<Dice> filteredList = _diceList.FindAll(diceFilter); //Filter using the predicate
        //Shuffle the dice list
        List<Dice> shuffledList = new List<Dice>();
        while(filteredList.Count > 0 && shuffledList.Count < maxDiceCount) //Continue until we either meet the max dice count or we run out of items in the filter list
        {
            int randomIdx = Msc.RandomInt(filteredList.Count); //Pick an index from the list at random
            shuffledList.Add(filteredList[randomIdx]); //Add the randomly picked item from the filter list to the shuffle list
            filteredList.RemoveAt(randomIdx); //Delete the randomly picked item from the filtered list
        }
        //Use the dice action on each of the members
        shuffledList.ForEach(diceAction);
    }

    //Display full dice list
    private void PrintDiceList(string displayMsg = "Current Dice:")
    {
        Console.WriteLine(displayMsg);
        for(int i = 0; i < _diceList.Count; i++)
        {
            Console.WriteLine($"{i+1}. Dice Sides: {_diceList[i].LettersToString()}");
        }
    }

    //Display dice letter frequency
    private void PrintDiceLetterFrequency()
    {
        Console.Clear();//Clear the console first
        //Run calculations
        Dictionary<char,int> charCounterDict = MakeCharCounterDictionary(); //Create a dictionary where the key is the ascii character for A to Z (and ?), the value will be incremented by the number of times we find it in a dice code

        //Scan the current dice code to increment each character as we find them
        char[] diceCodeArr = GenerateDiceListCode(false).ToCharArray(); //Get the dice code without commas
        for(int i = 0; i < diceCodeArr.Length; i++) //Check each character in the dice code
        {
            charCounterDict[diceCodeArr[i]]++; //Increment the char in the dictionary
        }

        //Original code used an array of object to count each entry
        //List<object[]> sortedCharCountList = DictTo2dList<char, int>(charCounterDict); //Create a 2d list, index 0 stores the char, index 1 stores the int
        //SortCharCounterList(sortedCharCountList); //Sort the list by number of times we found it. If equal, sort by alphabetical order (with ? being last)
        //List<string> letterFrequencyAsStringList =  Msc.ListMap<object[], string>(sortedCharCountList, CharCounterItemToString); //Use each entry in the 2d list to generate a string representing it's frequency
        
        //Updated code uses the tuple data type instead (Read more about tuples here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples )
        List<(char, int)> sortedCharCountList = DictToTupleList<char, int>(charCounterDict); //Create a 2d list, index 0 stores the char, index 1 stores the int
        sortedCharCountList.Sort(CompareCounterTuples); //Sort the list by number of times we found it. If equal, sort by alphabetical order (with ? being last)
        List<string> letterFrequencyAsStringList =  Msc.ListMap<(char, int), string>(sortedCharCountList, CharCounterTupleToString); //Use each entry in the 2d list to generate a string representing it's frequency

        string letterFrequencyAsDiceCode = string.Join("", letterFrequencyAsStringList); //Merge every item to generate a dice code

        int diceSideCount = letterFrequencyAsDiceCode.Length; //The dice side count will always be equal to the dice code length

        //Print the results
        Console.WriteLine($"Current Dice-List Letter Frequency of {diceSideCount} {Msc.Pluralize("side", diceSideCount)} across {_diceList.Count} dice:");
        for(int i = 0; i < sortedCharCountList.Count; i++)
        {
            Console.WriteLine($"{i}. {(char)sortedCharCountList[i].Item1}: {(int)sortedCharCountList[i].Item2} {(((double) sortedCharCountList[i].Item2) / ((double) diceSideCount)).ToString("P")}"); //Example: "1. A: 34 (30%)" Used this format https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#PFormatString
        }
        //Console.WriteLine($"{i}. {(char)sortedCharCountList[i][0]}: {(int)sortedCharCountList[i][1]} {(((double) ((int) sortedCharCountList[i][1])) / ((double) diceSideCount)).ToString("P")}"); //Example: "1. A: 34 (30%)" Used this format https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#PFormatString
        Console.WriteLine("");
        Console.WriteLine("As a single dice code (scramble this dice, then force the side list size to create a new dice set):");
        Console.WriteLine(letterFrequencyAsDiceCode); //Print as Dice Code
        Console.WriteLine("");
        Inp.GetInput("Press enter to continue");
    }

    //Sort a list of chars and counters //The sorting algorithim for tuples
    private int CompareCounterTuples((char, int) entryA, (char, int) entryB)
    {
        if(entryA.Item2 > entryB.Item2)
        {
            return -1;
        }
        else if(entryA.Item2 < entryB.Item2)
        {
            return 1;
        }
        else  //aVal == bVal (when the int value is equal, use the ascii value to determine what order to use)
        {
            char charA = (entryA.Item1 == '?') ? '`' : entryA.Item1; //Treat '?' as the highest value which will make it be sorted in the last position
            char charB = (entryB.Item1 == '?') ? '`' : entryB.Item1;
            return (charA < charB) ? -1 : 1; //If the value is lower, it should be sorted lower in the list
        }
    }

    //Create the letter dictionary (A to Z and ?) to track letter frequency
    private Dictionary<char,int> MakeCharCounterDictionary()
    {
        Dictionary<char,int> charCounterDict = new Dictionary<char, int>(); //Using a dictionary
        //Fill the dictionary
        for(int i = 0; i < 26; i++) //Use bitwise to generate each capital letter
        {
            charCounterDict.Add((char) (short) (0x40 | (i + 1)), 0); // Bitwise or 0x40 means 010_ ____, which changes 1 through 26 to A through Z, 0x60 or 011_ ____ would change to lowercase //0x40 = 0100 0000, 'A' = 0100 0001, 'Z' = 0101 1010, So: 010_ ____ | (1 to 26) = Ascii Value
        }
        charCounterDict.Add('?',0); //Add ? which isn't a letter and won't be added
        return charCounterDict;
    }

    //Convert a Dictionary to a list of tuple
    private List<(keyType, valType)> DictToTupleList<keyType, valType>(Dictionary<keyType, valType> dictionaryInput)
    {
        List<(keyType, valType)> returnList = new List<(keyType, valType)>();
        foreach(keyType key in dictionaryInput.Keys) //Add all chars from the dictionary to 
        {
            //returnList.Add(new object[]{key,dictionaryInput[key]});
            returnList.Add((key, dictionaryInput[key]));
        }
        return returnList;
    }

    //Convert a letter count tuple into a string
    private string CharCounterTupleToString((char, int) charCountItem)
    {
        return new string(charCountItem.Item1, charCountItem.Item2); //Parse the known indexes into the correct data type, then use them to create a string
    }

    //Bit shifting
    //Compact a byte to 1 bit
    //Will not be implemented

    //File Loading
    public void LoadFromFile(string[] fileLines, ref int offset) //Use the reference type to update the offset ( https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#passing-an-argument-by-reference-an-example )
    {
        //Load attributes
        //Width and height
        string[] gridSize = Msc.ReadFileLine(fileLines, ref offset, "diceSetSize=").Replace(" ","").Split(","); //Remove all spaces and split by "," //Format: "diceGridSize=5,5"
        if(gridSize.Length != 2){throw new IOException($"Error on Line {offset}! Dice Set Size is Invalid: {string.Join(",", gridSize)}");} //Detect invalid width and height, throw an IO error
        _width = int.Parse(gridSize[0]); //Try to parse their entry
        _height = int.Parse(gridSize[1]); //Try to parse their entry
        _allowAutoFill = Msc.ReadFileLine(fileLines, ref offset, "diceSetAllowAutoFill=").ToLower() != "false"; //The flag will be true when true or unrecognized
        _allowQu = Msc.ReadFileLine(fileLines, ref offset, "diceSetAllowQu=").ToLower() != "false"; //The flag will be true when "true" or unrecognized
        SetDiceBorder(Msc.ReadFileLine(fileLines, ref offset, "diceSetBorder=")); //Read the next line, use that string as the argument for the SetDiceBorder() method that is designed for this purpose
        LoadDiceListCode(Msc.ReadFileLine(fileLines, ref offset, "diceSetCode=")); //Load the diceList using the diceSetCode
    }

    //File Writing
    public void WriteToFile(StreamWriter sWriter)
    {
        sWriter.WriteLine($"diceSetSize={_width},{_height}"); ///Write the width and height
        sWriter.WriteLine($"diceSetAllowAutoFill="+((_allowAutoFill) ? "true" : "false")); //Write the status of AllowAutoFill
        sWriter.WriteLine($"diceSetAllowQu="+((_allowAutoFill) ? "true" : "false")); //Write the status of AllowQu
        sWriter.WriteLine($"diceSetBorder="+((_diceBorder.Length == 2 && _diceBorder[0] == '\u0000' && _diceBorder[1] == '\u0000') ? "" : string.Join(',',_diceBorder))); //This ternary operator is confusing to look at, but it just writes "" when both are set to (char) 0, it only does this check when the length is 2 (to prevent index errors). Otherwise it writes the diceBorder seperated by "," [The only reason I have this is because the default value is '\u0000' and not null]
        sWriter.WriteLine($"diceSetCode="+GenerateDiceListCode()); //Write the dice-set code, and the dice by extension
    }

    //Export Dice Code
    public void ExportDiceSetCode()
    {
        string path = Inp.GetInput("Enter the file name you want to save this dice-code to (leave blank to cancel): ",false);
        if(path != "") //Do not accept blank paths
        {
            try
            {
                try
                {
                    using(StreamWriter sWriter = new StreamWriter(File.Open(path,FileMode.Create))) //Open the file and truncate, or create the file
                    {
                        sWriter.Write(GenerateDiceListCode()); //Write the Dice-List code
                    }
                    Inp.GetInput($"File {path} Was Successfully Written");
                }
                catch(ArgumentException e){throw new IOException(e.ToString());}
                catch(UnauthorizedAccessException e){throw new IOException(e.ToString());}
                catch(NotSupportedException e){throw new IOException(e.ToString());}
            }
            catch(IOException e)
            {
                Inp.GetInput($"Error {e.ToString()}! Failed to Write {path}! Press enter to continue");
            }
        }
    }

    //Obsolete Code:

    //Display the Letters in a grid
    /*
    public void DisplayOld(bool clearAll = false)
    {
        char dWallStart = (char) 0;
        char dWallEnd = (char) 0;
        //"Exists" uses predicates (inline functions) to search a list
        //bool hasQu = _allowQu && _diceList.Exists((Dice inputDice) => {return inputDice.GetCurLetter() == 'Q';});
        bool hasQu = _diceList.Exists((Dice inputDice) => {return inputDice.GetCurLetter() == 'Q';});
        Console.Clear(); //Clear the console before starting
        for(int y = 0; y < _height; y++)
        {//Console.Write($"Y{y}");//Debugging
            for(int x = 0; x < _width; x++)
            {//Console.Write($"X{y} ");//Debugging
                if(x != 0)
                {
                    Console.Write(""); //Write a space before entries
                }
                //Console.Write(""); //Write the dice start border
                //consider writing X Y coordinates, make sure it doesn't shift the display
                try
                {
                    Console.Write(_diceList[(y * _width) + x].ToDisplayString(hasQu, dWallStart, dWallEnd)); //Each Y value is equivalent to a full row of X
                }
                catch(Exception) //No dice, literally
                {
                    Console.Write((hasQu) ? "    " : " "); //Write 4 blank spaces "[__]"
                    //Console.Write((hasQu && _allowQu) ? "    " : " "); //Write 4 blank spaces "[__]"
                }
                //Console.Write(""); //Write the dice end border
                if(x == _width - 1)
                {
                    Console.WriteLine(""); //Write a new line
                }
            }
        }
    }*/

    //Sort a list of chars and counters
    //Sort a mixed list of char in index 0 and int in index 1, sort by value first and use characters to determine ties
    /* public void SortCharCounterList(List<object[]> inputList) //Example where it tells you to use 1 and -1 to sort the lists is found here: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.sort?view=net-8.0#system-collections-generic-list-1-sort(system-comparison((-0)))
    {
        inputList.Sort((object[] a, object[] b) => 
        {//1 = move away from index 0, -1 means move closer to index 0 
            char aChar = (char) a[0]; //Load the values into something understandable
            int aVal = (int) a[1]; 
            char bChar = (char) b[0];
            int bVal = (int) b[1];
            if(aVal > bVal)
            {
                return -1; //When the int value is higher it should be sorted lower in the list
            }
            else if(aVal < bVal)
            {
                return 1; //When the int value is lower is should be sorted higher in the list
            }
            else //aVal == bVal (when the int value is equal, use the ascii value to determine what order to use)
            {
                aChar = (aChar == '?') ? '`' : aChar; //Treat '?' as the highest value which will make it be sorted in the last position
                bChar = (bChar == '?') ? '`' : bChar;
                return (aChar < bChar) ? -1 : 1; //If the value is lower, it should be sorted lower in the list
            }
        });
    } */
    /*//Tuple sorting
    private void SortCharCounterList(List<(char, int)> inputCounterList)
    {
        inputCounterList.Sort(CompareCounterTuples);
    }*/

    //Scan a DiceCode for chars using a charCounter dictionary (never made this a method)

    //Convert a dictionary to a list of objects
    /*
    private List<object[]> DictTo2dList<keyType, valType>(Dictionary<keyType, valType> dictionaryInput)
    {
        List<object[]> returnList = new List<object[]>();
        foreach(keyType key in dictionaryInput.Keys) //Add all chars from the dictionary to 
        {
            returnList.Add(new object[]{key,dictionaryInput[key]});
        }
        return returnList;
    }*/

    //Convert a letter count item into a string
    /*private string CharCounterItemToString(object[] charCountItem)
    {
        return new string((char) charCountItem[0], (int)charCountItem[1]); //Parse the known indexes into the correct data type, then use them to create a string
    } */
}