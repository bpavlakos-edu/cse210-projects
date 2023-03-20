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
    //Get the currently displayed side
    public string ToDisplayString()
    {
        return (_hidden) ? " " : _curLetter + ""; //Return the current letter or blank for hidden, using the ternary conditional operator ( https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator)
    }
    public void Roll()
    {

    }
    public void ToggleHidden(int randomChance = 1, int randomChanceMax = 2)
    {

    }
    public void OpenSettings()
    {

    }
    //Setting support function, used both internally and externally
    public string LettersToString()
    {
        return "";
    }
}