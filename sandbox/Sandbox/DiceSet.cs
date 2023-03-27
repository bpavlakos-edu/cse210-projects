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

using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using Inp = QuickUtils.Inputs;
using UiMenuExitException = QuickUtils.UiMenuExitException; //Exit exception for the menu to use
using Msc = QuickUtils.Misc;
class DiceSet
{
    //Attributes
    protected List<Dice> _diceList = new List<Dice>();
    protected int _width = 5;
    protected int _height = 5;
    //protected bool _autoFillEnabled = true; //Enable or disable auto filling of this dice set
    protected bool _allowQu = true; //Enable or disable displaying Q as Qu
    //protected Char[] _diceBorder = new char[]{'\u0000','u0000'}; //Warning for settings menu! You must be able to reset this from the console!
    protected bool _updating = false; //A flag for threaded functions to check to see if they can print to the console
    
    //Constructors
    
    //No blank constructor, it should never be blank!!!
    
    //Fill all attributes
    public DiceSet(List<Dice> diceList, int width = 5, int height = 5)
    {
        _diceList = diceList.ToList<Dice>(); //Copy the list to break the reference to the original
        _width = width;
        _height = height;
    }
    //Fill all attributes but use the default dice list (also doubles as the blank constructor)
    public DiceSet(int width = 5, int height = 5)
    {
        _width = width;
        _height = height;
        DiceListToDefault(); //Update the diceList to be the default values
    }

    //Copy an existing DiceSet so GameModes can modify it without changing the original
    public DiceSet(DiceSet sourceDiceSet) : this(sourceDiceSet.GetDiceList(), sourceDiceSet.GetWidth(), sourceDiceSet.GetHeight())
    {
        //Use the Fill all fields constructor to re-use code, and make it easier to change
        /*_diceList = sourceDiceSet._diceList;
        _width = sourceDiceSet._height;
        _height = sourceDiceSet._width;*/
    }
    //Special copy constructor, auto fills remaining dice slots when the autoFillFlag constructor is enabled
    public DiceSet(DiceSet sourceDiceSet, bool forceAutoFill) : this(sourceDiceSet) //Use the original copy constructor to create the inital object
    {
        if((/*_autoFillEnabled ||*/ forceAutoFill) && CheckSize()) //Only run this check when allow auto fill is true
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
    //Use a dice set to set the values of this dice set (useful for file loading)
    public void SetDiceSet(DiceSet newDiceSet)
    {
        _diceList = newDiceSet.GetDiceList();
        _width = newDiceSet.GetWidth();
        _height = newDiceSet.GetHeight();
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

    //Methods

    //Main Functionality

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

    //Quick Dice Display, uses modulus, char[] buffers to display dice, if you think this is unecessary, try doing 70 x 70 grid with the original display function and compare it to this one, this is much faster!!!
    public void Display()
    {
        _updating = true; //Let asyncronous functions know we are updating
        Console.Clear(); //Clear the console
        char dWallStart = (char) 0;
        char dWallEnd = (char) 0;
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
        UiMenu diceSetSettings = new UiMenu(new List<UiOption>{
            new UiOption(OpenDiceListSettingsMenu,"Edit &Dice List"), //Open Dice Edit Menu
            new UiOption(GetWidth, SetWidth, "Grid &Width", 2), //Set Width
            new UiOption(GetHeight, SetHeight, "Grid &Height", 2), //Set Height
            new UiOption(GetGridSize, SetGridSize, "Grid &Size", 2), //Set Height
            new UiOption(GetAllowQu, SetAllowQu,"&Qu Allowed"), //Allow Qu Setting
            new UiOption(()=>{throw new UiMenuExitException();},"Go &Back"),
        });
        diceSetSettings.UiLoop();
    }
    //Dice List Settings Menu
    public void OpenDiceListSettingsMenu()
    {
        UiMenu diceListSettings = new UiMenu(
            _diceList.ToList<object>(),
            (inputDice) => {((Dice)inputDice).OpenSettings();},
            "Open Dice $ Settings (Sides", //$ is replaced by index in loop + 1
            (inputDice) => {return ((Dice)inputDice).LettersToString()+")";}, //Add the parenthesis here to finish the display string
            "Go &Back",
            "Dice List and Options:",
            "Select a dice number, choice, or [hotkey] from the menu: "
        );
        //Add additional Edit All Options before the exit button:
        //diceListSettings.AddOptionFromEnd(new UiOption(()=>{},"&Export Dice List to File"));
        //diceListSettings.AddOptionFromEnd(new UiOption(()=>{},"&Import Dice List from File")); //Should be included in export settings
        diceListSettings.AddOptionFromEnd(
            new List<UiOption>
            {
                new UiOption(ShowDiceCode,"&Generate a Dice-List Code for Sharing"),
                new UiOption(()=>{},"&Enter Dice-List Code"),
                new UiOption(()=>{},"&Shuffle Dice Set"),
                new UiOption(()=>{},"&Add New Dice"),
                new UiOption(()=>{},"&Delete Dice"),
                new UiOption(()=>{},"&Set Dice List"),
                new UiOption(DiceListToDefault,"&Reset Dice List to Default"),
            }
        );
        diceListSettings.UiLoop();
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
        Console.WriteLine("Dice List Code:");
        Console.WriteLine("");
        Console.WriteLine(GenerateDiceListCode());
        Console.WriteLine("");
        Inp.GetInput("Press enter to continue");
    }
    //Enter a diceList Code into the console
    public void EnterDiceCode()
    {
        LoadDiceListCode(Inp.GetInput("Enter Your Dice List Code:", null, true));
    }

    //Generate the DiceListCode
    public string GenerateDiceListCode()
    {
        //Generate the string
        List<char> newDiceCodeBuffer = new List<char>(); //Use a char buffer, it's List<char> because each dice can have a different size (we are also avoiding arithmatic operations using string)
        for(int i = 0; i < _diceList.Count; i++) //Convert each dice to chars
        {
            _diceList[i].AppendToCharList(newDiceCodeBuffer, i); //Use the dice's method to add them to the buffer, pass the index so it knows when to add a comma
        }
        return new string(newDiceCodeBuffer.ToArray<char>()); //Return it as a string
    }
    //Load a DiceListCode from a string, ignore blank codes
    public void LoadDiceListCode(string diceSetCode)
    {
        if(diceSetCode != "") //Ignore empty strings
        {
            //Make the new Dice list
            List<Dice> newDiceList = new List<Dice>();
            string[] diceStrArr = diceSetCode.Split(","); //Split the string by using ","
            for(int i = 0; i < diceStrArr.Length; i++)
            {
                if(diceStrArr[i] != "") //Ignore empty strings, so we don't accidently fill the actual dice list with an empty list
                {
                    newDiceList.Add(new Dice(diceStrArr[i])); //Automatically create a dice from the string using the constructor made for this
                }
            }
            if(newDiceList.Count > 0) //Update the actual Dice List if the New Dice List isn't empty
            {
                _diceList.Clear(); //Clear the list
                _diceList = Msc.ListCopy<Dice>(newDiceList, (Dice inputDice)=>{return new Dice(inputDice);}); //Copy the dice list using the ListCopy Function
            }
        }
    }

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
                int randomIdx = new Random().Next(0,shuffleDiceList.Count); //Pick a random index in shuffleDiceList (Todo: consider bit shifting the seed by the capacity of the list)
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

    private bool CheckSize()
    {
        return _diceList.Count < (_width * _height);
    }

    //Fill the dice list to an integer
    //Fill using a list of dice
    public void FillToCount(int newDiceCount, params Dice[] inputDice) //Utilizes the params keyword, which lets us use each item as an individual parameter: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params
    {
        for(int i = 0; _diceList.Count < newDiceCount; i++)
        {
            _diceList.Add(new Dice(inputDice[i % inputDice.Length])); //Use modulus to infinitely loop the input array
        }
    }
    //Fill using chars
    public void FillToCount(int newDiceCount, params char[] inputChars)
    {
        FillToCount(newDiceCount, new Dice(inputChars.ToList<char>(), 0, ' ', false)); //Convert input chars to a list to use to create a new dice to send to the original function
    }
    //Bit shifting
    //Compact a byte to 1 bit
}