using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using Inp = QuickUtils.Inputs;
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
        _sideList = sideList.ToList<char>();//Copy the input list to break the reference to the original
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

    //Copy an existing Dice
    public Dice(Dice newDice) : this(newDice.GetSideList(), newDice.GetSide(), newDice.GetCurLetter(), newDice.GetHidden())
    {

    }

    //Getters and Setters (Normal external access Getters and Setters were auto generated using my AutoGetterSetter Python Script in C# mode)
    public List<char> GetSideList()
    {
        return _sideList.ToList<char>();
    }
    public void SetSideList(List<char> sideList)
    {
        _sideList = sideList.ToList<char>();
    }
    //SetSide list from a string (custom)
    public void SetSideList(string inString)
    {
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
    
    //Methods
    //Get the currently displayed side of this dice (Todo: Decide if the brackets should be handled by the DiceSet and not here)
    public string ToDisplayString()
    {
        return (_hidden) ? "[  ]" : (_curLetter != 'Q') ? "["+_curLetter + " ]" : "["+_curLetter + "u]"; //Return the current letter or blank for hidden, if it's Q add "u" to make "Qu" using the ternary conditional operator (https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator)
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
    public void ToggleHidden(int rChance = 1, int rChanceMax = 2)
    {
        if(RandomChance(rChance, rChanceMax)) //Run the random chance to see if we are swapping this dice's hidden state
        {
            _hidden = !_hidden; //Reverse the state of hidden
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
        //Randomi&ze Side List
        //Go &Back
    }
    //Setting support function, used both internally and externally
    public string LettersToString(char sepChar = '\u0000') //Default char value found here https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char#literals
    {
        return string.Join(sepChar,_sideList); //String.Join puts all chars in a list together to form a string
    }
    //Utility
    //Determine if the random chance has been met
    private bool RandomChance(int rChance, int rChanceMax, bool reverse = false)
    {
        double rThreshold = (double) rChance / (double) rChanceMax; //Calculate the random threshold, "Random" uses a double, so we have to make them doubles too
        return (!reverse) ? new Random().NextDouble() >= rThreshold : new Random().NextDouble() <= rThreshold; //Return Random >= rThreshold, or Random <= rThreshold if the reverse flag is activated (using the Ternary Conditional Operator)
    }
    //Get a random letter by calling the IntToChar function after picking a number from 0 to 25 as the offset (Consider merging this function with IntTochar)
    private char GetRandomLetter()
    {
        return IntToChar(new Random().Next(0,26));
    }
    private char IntToChar(int alphabetValue, int asciiStartIndex = 97)
    {
        return (char)(asciiStartIndex + alphabetValue); //Convert the letter to it's ascii counterpart, then convert that to char
    }
    //Add or remove sides
    private void AddSide(char inputChar, bool strict = false)
    {
        if(inputChar == '?' || char.IsAsciiLetter(inputChar)) 
        {
            _sideList.Add(char.ToLower(inputChar));
            //_sideList.Add(inputChar); //For the if statement version if it's faster
        }

        /*else if(char.IsAsciiLetterLower(inputChar)) //IsAsciiLetterUpper must be added above to use this
        {
            _sideList.Add(char.ToLower(inputChar));
        }*/

        else if(strict) //For debugging
        {
            Console.WriteLine($"Invalid character {inputChar}");
            throw new NotImplementedException();
        }
    }
    private void RemoveSide(int index)
    {
        try
        {
            _sideList.RemoveAt(index);
        } 
        catch (ArgumentOutOfRangeException){} //Ignore index errors
    }
}