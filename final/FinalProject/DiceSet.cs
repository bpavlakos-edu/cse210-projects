class DiceSet
{
    //Attributes
    private List<Dice> _diceList = new List<Dice>();
    private int _width = 5;
    private int _height = 5;
    //Constructors
    
    //No blank constructor, it should never be blank!!!
    
    //Fill all attributes
    public DiceSet(List<Dice> diceList, int width, int height)
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

    //Display the Letter grid
    public void Display(bool clearAll = false)
    {
        Console.Clear();
        for(int y = 0; y < _height; y++)
        {
            for(int x = 0; x < _width; x++)
            {
                if(x != 0)
                {
                    Console.Write(" ");
                }
                try
                {
                    Console.Write(_diceList[(y * _width) + x].ToDisplayString()); //Each Y value is equivalent to a full row of X
                }
                catch(Exception) //No dice, literally
                {
                    Console.Write("    "); //Write 4 blank spaces "[__]"
                }
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
    public int RandomHide(int rChance = 1, int rChanceMax = 2)
    {
        int hiddenCount = 0; //Reset the hidden count tracker
        _diceList.ForEach((curDice) => {
            curDice.ToggleHidden(rChance, rChanceMax);
            if(curDice.GetHidden()) //Increment the hidden counter for each dice that is hidden
            {
                hiddenCount++;
            }
        });
        Display();
        return hiddenCount; //Return the number of hidden dice
    }

    //Open the settings menu
    public void OpenSettings()
    {

    }

    //Mass Dice Modification
    public void SetAll(Dice newDice)
    {
        //_diceList.ForEach((curDice))
        //Old version
        
        int diceCount = _diceList.Count; //Get the dice count before we clear it
        _diceList = new List<Dice>(); //Clear the dice list
        for(int i = 0; i < diceCount; i++)
        {
            _diceList.Add(new Dice(newDice)); //Add a copy of the newDice
        }
        
    }

    public void SetAll(char fillChar)
    {
        
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