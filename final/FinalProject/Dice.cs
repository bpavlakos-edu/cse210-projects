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
    public Dice(List<char> sideList, int side = 0,char curLetter=' ', bool hidden = false)
    {
        _sideList = sideList.ToList<char>();//Copy the input list to break the reference to the original
        _side = side;
        _curLetter = curLetter;
        _hidden = hidden;
    }

    //Getters and Setters (Auto generated using my AutoGetterSetter Python Script in C# configuration)
    public List<char> GetSideList()
    {
        return _sideList.ToList<char>();
    }
    public void SetSideList(List<char> sideList)
    {
        _sideList = sideList.ToList<char>();
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
        return (_hidden) ? "[ ]" : (_curLetter != 'Q') ? "["+_curLetter + " ]" : "["+_curLetter + "u]"; //Return the current letter or blank for hidden, if it's Q add "u" to make "Qu" using the ternary conditional operator (https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator)
    }
    //Randomly pick a side of this dice
    public void Roll()
    {
        try
        {
            _side = new Random().Next(0, _sideList.Count); //Pick a side of the dice, using the char list
            _curLetter = (_sideList[_side] != '?') ? _sideList[_side] : GetRandomLetter(); //Update _curLetter, set using the current side, or by getting a random letter when its set to '?'
        }
        catch(OutOfMemoryException) //Disabled to check for errors//catch(ArgumentOutOfRangeException) //Detect when the user tries to roll 0 length lists
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

    }
    //Setting support function, used both internally and externally
    public string LettersToString()
    {
        return "";
    }
    //Utility
    private bool RandomChance(int rChance, int rChanceMax, bool reverse = false)
    {
        double rThreshold = (double) rChance / (double) rChanceMax; //Calculate the random threshold, "Random" uses a double, so we have to make them doubles too
        return (reverse) ? new Random().NextDouble() <= rThreshold : new Random().NextDouble() >= rThreshold; //Return Random >= rThreshold, or Random <= rThreshold if the reverse flag is activated (using the Ternary Conditional Operator)
    }
    private char GetRandomLetter()
    {
        return IntToChar(new Random().Next(0,26));
    }
    private char IntToChar(int alphabetValue, int asciiStartIndex = 97)
    {
        return (char)((byte)(asciiStartIndex + alphabetValue)); //Convert the letter to it's ascii counterpart, then convert that to hex, then finally to char
    }
}