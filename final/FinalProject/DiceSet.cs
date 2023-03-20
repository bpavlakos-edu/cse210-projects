using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
//using Inp = QuickUtils.Inputs;
using UiMenuExitException = QuickUtils.UiMenuExitException; //Exit exception for the menu to use

class DiceSet
{
    //Attributes
    private List<Dice> _diceList = new List<Dice>();
    private int _width = 5;
    private int _height = 5;
    //private bool _allowQu = true; //Enable or disable displaying Q as Qu
    //Char[] _diceBorder = new char[]{'\u0000','u0000'}; //Warning for settings menu! You must be able to reset this from the console!
    
    //Constructors
    
    //No blank constructor, it should never be blank!!!
    
    //Fill all attributes
    public DiceSet(List<Dice> diceList, int width = 5, int height = 5)
    {
        _diceList = diceList.ToList<Dice>(); //Copy the list to break the reference to the original
        _width = width;
        _height = height;
    }

    //Copy an existing DiceSet so GameModes can modify it without changing the original
    public DiceSet(DiceSet sourceDiceSet) : this(sourceDiceSet._diceList, sourceDiceSet._width, sourceDiceSet._height)
    {
        //Use the Fill all fields constructor to re-use code, and make it easier to change
    }

    //Getters and Setters (Normal external access Getters and Setters were auto generated using my AutoGetterSetter Python Script in C# mode)
    public List<Dice> GetDiceList()
    {
        return _diceList.ToList<Dice>();
    }
    public void SetDiceList(List<Dice> diceList)
    {
        _diceList = diceList.ToList<Dice>();
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

    //Methods

    //Main Functionality

    //Display the Letters in a grid
    public void Display(bool clearAll = false)
    {
        char dWallStart = '\u0000';
        char dWallEnd = '\u0000';

        //"Exists" uses predicates (inline functions) to search a list
        bool hasQu = _diceList.Exists((Dice inputDice) => {return inputDice.GetCurLetter() == 'Q';}) /*&& _allowQu*/;
        Console.Clear(); //Clear the console before starting
        for(int y = 0; y < _height; y++)
        {
            for(int x = 0; x < _width; x++)
            {
                if(x != 0)
                {
                    Console.Write(""); //Write a space before entries
                }
                Console.Write(""); //Write the dice start border
                //consider writing X Y coordinates, make sure it doesn't shift the display
                try
                {
                    Console.Write(_diceList[(y * _width) + x].ToDisplayString(hasQu, dWallStart, dWallEnd)); //Each Y value is equivalent to a full row of X
                }
                catch(Exception) //No dice, literally
                {
                    Console.Write((hasQu /*&& _allowQu*/) ? "    " : " "); //Write 4 blank spaces "[__]"
                }
                Console.Write(""); //Write the dice end border
                if(x == _width - 1)
                {
                    Console.WriteLine(""); //Write a new line
                }
            }
        }
    }

    //Smart Dice Display, uses modulus to display dice

    //Roll All Dice
    public void RollAll()
    {
        Shuffle(); //Shuffle Dice
        _diceList.ForEach((curDice) => {curDice.Roll();}); //Roll each dice using ForEach, with a lambda (Action with input type dice) call to the current dice's Roll method
        Display(); //Display after rolling
    }

    //Randomly Change a dice's hidden state
    public int RandomHide(int rChance = 1, int rChanceMax = 2, int? rCountMax = null)
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
            new UiOption(), //Open Dice Edit Menu
            new UiOption(GetWidth, SetWidth, "Grid &Width"), //Set Width
            new UiOption(GetHeight, SetHeight, "Grid &Height"), //Set Height
            new UiOption(()=>{throw new UiMenuExitException();},"&Back"),
        });
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
}