using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using Inp = QuickUtils.Inputs;
using Msc = QuickUtils.Misc;
using UiMenuExitException = QuickUtils.UiMenuExitException; //Exit exception for the menu to use
using UiMenuRefreshException = QuickUtils.UiMenuRefreshException; //Refresh exception for the menu to use
class Dice
{
    //Attributes
    private List<char> _sideList;
    private int _side = 0;
    private char _curLetter = ' ';
    private bool _hidden = false;
    //Constructors
    //Blank
    public Dice()
    {

    }
    //Fill all attributes, most attributes use default values
    public Dice(List<char> sideList, int side = 0, char curLetter=' ', bool hidden = false)
    {
        _sideList = Msc.ListCopy<char>(sideList,(char inObj) => {return inObj;});//Copy the input list to break the reference to the original
        _side = side;
        _curLetter = curLetter;
        _hidden = hidden;
    }

    //Fill attributes with user input
    public Dice(bool triggerFlag) //This boolean is here to activate this constructor
    {
        string newSidesString = Inp.GetInput("Please enter the dice side letters (enter \"?\" for random): ");
        SetSideList(newSidesString);
        //Set everything else to default
        _side = 0;
        _curLetter = ' ';
        _hidden = false;
    }

    //Fill attributes using a string as the char list
    public Dice(string diceCodeString, int side = 0, char curLetter = ' ', bool hidden = false)
    {
        _sideList = new List<char>();
        SetSideList(diceCodeString);
        _side = side;
        _curLetter = curLetter;
        _hidden = hidden;
    }

    //Copy an existing Dice
    public Dice(Dice newDice) : this(newDice.GetSideList(), newDice.GetSide(), newDice.GetCurLetter(), newDice.GetHidden())
    {

    }

    //Getters and Setters (Normal external access Getters and Setters were auto generated using my AutoGetterSetter Python Script in C# mode)
    public List<char> GetSideList()
    {
        return Msc.ListCopy<char>(_sideList,(char inObj) => {return inObj;});
    }
    public void SetSideList(List<char> sideList)
    {
        _sideList = Msc.ListCopy<char>(sideList,(char inObj) => {return inObj;});
    }
    //SetSide list from a string (custom)
    public void SetSideList(string inString, bool clearList = true)
    {
        if(clearList)
        {
            _sideList = new List<char>();
        }
        List<char> charList = inString.ToUpper().ToList<char>();//Captialize the input string before turning it into a list of char
        //Add each side to the list
        charList.ForEach((curChar) => {AddSide(curChar);});//Use AddSide() to filter out invalid characters
        if(_sideList.Count == 0) //No valid characters
        {
            _sideList.Add('?'); //Add '?'
        }
    }
    public int GetSide()
    {
        return _side;
    }
    public void SetSide(int side)
    {
        _side = side;
    }
    public char GetCurLetter()
    {
        return _curLetter;
    }
    public void SetCurLetter(char curLetter)
    {
        _curLetter = curLetter;
    }
    public bool GetHidden()
    {
        return _hidden;
    }
    public void SetHidden(bool hidden)
    {
        _hidden = hidden;
    }
    //Custom Getters and Setters
    public void SetDice(Dice newDice)
    {
        SetSideList(newDice.GetSideList());
        SetSide(newDice.GetSide());
        SetCurLetter(newDice.GetCurLetter());
        SetHidden(newDice.GetHidden());
    }
    public void SetDice(char fillChar)
    {
        SetSideList(new List<char>{fillChar}); //Set the entire side list to just the fill char
    }
    
    
    //Methods
    //Get the currently displayed side of this dice (Todo: Decide if the brackets should be handled by the DiceSet and not here)
    public string ToDisplayString(bool hasQu = false, char dWallStart = (char) 0, char dWallEnd = (char) 0) //Implement allow u
    {
        if(_hidden)
        {
            return dWallStart + ((hasQu) ? "  " : " ") + dWallEnd;
        }
        else //Has a Qu in the display
        {
            return dWallStart + (_curLetter + ((_curLetter != 'Q') ? " " : "u")) + dWallEnd; //Return an extra U when Q and an extra space when not Q
            //return _side+" "; //Debugging
        }
        //ternary conditional operator (https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator)
    }
    //Get the current display side of this dice as chars
    public char[] ToDisplayChars(bool hasQuChar, char dWallStart = (char) 0, char dWallEnd = (char) 0)
    {//Each buffer has a maximum of 4 items: //items: dice wall start char, dice wall char or blank for hidden, space for letters and hidden or u for Qu when option is enabled, dice wall end
        return (new char[] {dWallStart, ((_hidden) ? ' ' : _curLetter), ((hasQuChar && _curLetter == 'Q' && !_hidden) ? 'u':' '), dWallEnd}).Where((char inChar)=>{return inChar != (char) 0;}).ToArray<char>(); //Use ternary operators to change each character slot, use "Where" to get a list of chars that aren't (char) 0, use ToArray to get them as a character array because Where can't be used as an array directly
    }
    //Randomly pick a side of this dice
    public void Roll()
    {
        try
        {
            _side = new Random().Next(0, _sideList.Count); //Pick a random side of the dice, using the char list
            _curLetter = (_sideList[_side] != '?') ? _sideList[_side] : GetRandomLetter(); //Update _curLetter, set using the current side from the list, or by getting a random letter when it's side is set to '?' (Uses ternary operator)
        }
        catch(OperationCanceledException) //Disabled this catch to check for errors //catch(ArgumentOutOfRangeException) //Detect when the user tries to roll 0 length lists
        {
            Console.WriteLine($"Dice is empty! Please add sides before rolling!");
        }
    }
    public void ToggleHidden(int rChance = 1, int rChanceMax = 4, bool showOnFail = true)
    {
        if(RandomChance(rChance, rChanceMax)) //Run the random chance to see if we are swapping this dice's hidden state
        {
            _hidden = !_hidden; //Reverse the state of hidden
        }
        else if(showOnFail) //Flag to let the dice show itself when it fails the random chance
        {
            _hidden = false;
        }
    }
    public void OpenSettings()
    {
        /*
        //Special Updating Menu
        bool exitFlag = false; //Boolean for exiting
        while(!exitFlag)
        {
            UiMenu _diceSideMenu = new UiMenu();
        }*/
        //Sides: A,B,C,D,E,F
        //&Add Side
        //&Remove Side
        //&Edit Side
        //&Set All Sides 
        //Replace Sides With Random
        //Scramble Side Order
        //Go &Back
        //UiMenu diceSettings
        //diceSettings.UiLoop();
        bool refreshUi = true;
        while(refreshUi)
        {
            //Create the UiMenu
            UiMenu diceSettingsMenu = new UiMenu(
                new List<UiOption>
                {
                    new UiOption(()=>{SimpleDiceCode(); throw new UiMenuRefreshException();},"&Edit With Dice-Code"),
                    new UiOption(()=>{SimpleDiceCode(false); throw new UiMenuRefreshException();},"&Add Sides With Dice-Code"),
                    new UiOption(()=>{DeleteSides(); throw new UiMenuRefreshException();},"&Delete Sides"),
                    new UiOption(()=>{ScrambleSides(); throw new UiMenuRefreshException();},"&Scramble Side Order"),
                    new UiOption(()=>{RandomSideLetters(); throw new UiMenuRefreshException();},"&Replace Sides with Random Letters"),
                    new UiOption(()=>{throw new UiMenuExitException();},"Go &Back") //Go back to the previous menu
                },
                "Main Menu > Settings > Dice-Set Options > Dice List and Options > Dice Settings:",
                "Select a setting or [hotkey] from the menu: ",
                "" //Hide the exit message
            );
            refreshUi = diceSettingsMenu.UiLoop(()=>{Console.WriteLine("Current Sides: "+LettersToString());Console.WriteLine($"Side Count: {_sideList.Count}");}); //Refresh the UiMenu
        }
        throw new UiMenuRefreshException(); //Refresh the menu that this menu has been called from (the DiceSetSettings > Edit Dice List menu)
    }
    //Simple DiceCode Method
    public void SimpleDiceCode(bool clearList = true)
    {
        Console.WriteLine("Current Sides: "+LettersToString(','));
        Console.WriteLine("Dice-Code Rules:");
        Console.WriteLine("Each letter represents 1 side of the dice");
        Console.WriteLine("Each dice can have a unique number of sides");
        //Console.WriteLine("Add \",\" to seperate each dice entry");
        Console.WriteLine("\"?\" picks a random letter each time it's rolled in-game");
        Console.WriteLine("\"*\" picks a random letter to save as the side");
        Console.WriteLine("\"@\" picks a random vowel to save as the side");
        Console.WriteLine("\"#\" picks a random non-vowel to save as the side");
        Console.WriteLine("Invalid characters are ignored, letters aren't case-sensitive");
        //Console.WriteLine("When the dice list is empty, it will automatically be filled by a single dice");
        SetSideList(Inp.GetInput("Enter Your Dice-Code (Leave blank to cancel):", null, clearList)); //forces upper case (toLower = null), newLine = true
        //throw new UiMenuRefreshException();
    }
    //Advanced Dice Code
    /*
    public void DiceCodeConsole()
    {
        //Syntax:
        //Commands:
        //A = Set it to 1 side, being A
        //ABCD = Set the sides to ABCD
        //5A = set side 5 to A
        //25A = set side 25 to A
        //+ABCD = Add ABCD as sides to the end
        //-ABCD = Remove all instances of ABCD
        //25+ABCD = At slot 25 add the sides ABCD
        //ABCD,EFGH = Set the sides to ABCDEFGH
        //># = Skip number of sides
        //$ = Reset the side list
        //_ = Keep current side
        //Special Characters:
        //? = Randomly Pick a letter from A to Z
        //* = Randomly Pick a letter to save as the side
        
    }
    //Process the Dice Code
    public void ProcessDiceCode(string diceCodeString)
    {
        List<char> newSideList = Msc.ListCopy<char>(_sideList,(char inChar) => {char returnChar = inChar; return returnChar;}); //Copy the list of chars, just use a simple lambda to copy the value of char to a new char variable to break the reference
        char[] diceCodeBuffer = diceCodeString.ToCharArray(); //Initalize the diceCode buffer
        int offset = 0;
        int sideIndex = 0;
        while(offset < diceCodeBuffer.Length)
        {
            char codeChar = diceCodeBuffer[offset];
            if(char.IsAsciiDigit(codeChar))
            {
                sideIndex = GetNumber(diceCodeBuffer, offset) ?? sideIndex; //Only update when not null
            }
            else if(codeChar == '+') //Add Sides
            {
                //Get new index if it's an integer
            }
            else if(codeChar == '-') //Remove Sides
            {
                //Get new index if it's an integer
            }
            else if(codeChar == ',') //Ingnore command, prepare for next one
            {

            }
        }
    }
    //Get an integer from a dice code buffer
    public int? GetDiceCodeNumber(char[] diceCodeBuffer, int offset)
    {
        List<char> numberCharList = new List<char>();
        while(char.IsNumber(diceCodeBuffer[offset]) == true && offset < diceCodeBuffer.Length)
        {
            numberCharList.Add(diceCodeBuffer[offset]);
            offset++;
        }
        //Try and return the result
        try
        {
            return int.Parse(new string(numberCharList.ToArray()));
        }
        catch(ArgumentNullException)
        {
            return null;
        }
    }
    */
    //Scramble the order of dice sides
    public void ScrambleSides()
    {
        List<char> _newSideList = new List<char>(); //create a temporary list to store the sides we pick from the real list at random
        while(_sideList.Count > 0) //Repeat until the original list is empty
        {
            int nextIndex = new Random().Next(0,_sideList.Count); //Pick the next index
            _newSideList.Add(_sideList[nextIndex]); //Add the picked item
            _sideList.RemoveAt(nextIndex); //Remove the picked index
        }
        _sideList = Msc.ListCopy<char>(_newSideList,(char inputChar)=>{return inputChar;}); //Copy the final list
    }
    //Set the side list using random characters
    public void RandomSideLetters()
    {
        SetSideList(new string('*',_sideList.Count));
    }
    //Delete sides
    public void DeleteSides()
    {
        Console.WriteLine("Current Sides: "+LettersToString(','));
        List<int[]> deletionIndexes = Inp.GetIntRangeInput("Enter the ranges of sides to delete (\"-\" to make a range, \",\" to seperate numbers, \"!\" to select all): ",1,_sideList.Count,subtractNum:1);
        for(int i = deletionIndexes.Count - 1; i >= 0; i--) //Reverse for loops go backwards so that the greatest item is removed first, to prevent index errors
        {
            for(int j = deletionIndexes[i][deletionIndexes[i].Length - 1]; j >= deletionIndexes[i][0]; j--) //Start at the end range, end when we are below the start range
            {//j = deletionIndexesEnd, j >= deletionIndexesStart, go backwards
                RemoveSide(j); //Remove this side from the list
            }
        }
        if(_sideList.Count == 0) //If it's empty by the end
        {
            _sideList.Add('?'); //Add ? to fill it
        }
    }

    //Setting support function, used both internally and externally
    public string LettersToString(char sepChar = '\u0000') //Default char value found here https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char#literals
    {
        return string.Join(sepChar,_sideList); //String.Join puts all chars in a list together to form a string
    }
    //Append this dice as a list of characters to a buffer, for quickly writing dice codes
    public void AppendToCharList(List<char> charListBufferTarget, int index = 0)
    {
        if(charListBufferTarget.Count != 0) //When this isn't the first index
        {
            charListBufferTarget.Add(','); //Add 0
        }
        charListBufferTarget.AddRange(_sideList);
    }
    //Utility
    //Determine if the random chance has been met
    private bool RandomChance(int rChance, int rChanceMax, bool reverseCheck = false)
    {
        double rThreshold = (double) rChance / (double) rChanceMax; //Calculate the random threshold, "Random" uses a double, so we have to make them doubles too
        return (!reverseCheck) ? new Random().NextDouble() >= 1.0 - rThreshold : new Random().NextDouble() <= rThreshold; //Return Random >= rThreshold, or Random <= rThreshold if the reverse flag is activated (using the Ternary Conditional Operator)
    }
    //Get a random letter by calling the IntToChar function after picking a number from 0 to 25 as the offset (Consider merging this function with IntTochar)
    private char GetRandomLetter()
    {
        return IntToChar(new Random().Next(0,26));
    }
    private char IntToChar(int alphabetValue, int asciiStartIndex = 65) //97 is 'a', 65 is 'A'
    {
        return (char)(asciiStartIndex + alphabetValue); //Convert the letter to it's ascii counterpart, then convert that to char
    }
    //Add or remove sides
    private void AddSide(char inputChar, bool strict = false)
    {
        if(inputChar == '?' || inputChar == '*' || char.IsAsciiLetter(inputChar)) //Accept only recognized characters
        {
            _sideList.Add((inputChar == '*') ? GetRandomLetter() : char.ToUpper(inputChar)); //Automatically add upper case letters (which doesn't change '?') to the side list, if the char is '*' pick a random letter to save as the side
        }
        else if(strict) //For debugging
        {
            Console.WriteLine($"Invalid character {inputChar}");
            throw new NotImplementedException();
        }
    }
    //Remove a side by index
    private void RemoveSide(int index)
    {
        try
        {
            _sideList.RemoveAt(index);
        } 
        catch (ArgumentOutOfRangeException){} //Ignore index errors
    }

    //Fill all sides with a character
    /*
    public void FillSides(char fillChar = '*')
    {
        SetSideList(new string(fillChar,_sideList.Count));
    }
    */

    //File Loading Is Handled By Dice-Set Code in config file
}