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
//using Inp = QuickUtils.Inputs;
using UiMenuExitException = QuickUtils.UiMenuExitException; //Exit exception for the menu to use
using Msc = QuickUtils.Misc;
class DiceSet
{
    //Attributes
    protected List<Dice> _diceList = new List<Dice>();
    protected int _width = 5;
    protected int _height = 5;
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
    public DiceSet(DiceSet sourceDiceSet) : this(sourceDiceSet.GetDiceList(), sourceDiceSet.GetWidth(), sourceDiceSet.GetHeight())
    {
        //Use the Fill all fields constructor to re-use code, and make it easier to change
        /*_diceList = sourceDiceSet._diceList;
        _width = sourceDiceSet._height;
        _height = sourceDiceSet._width;*/
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
    public void SetDiceSet(DiceSet newDiceSet)
    {
        _diceList = newDiceSet.GetDiceList();
        _width = newDiceSet.GetWidth();
        _height = newDiceSet.GetHeight();
    }

    //Methods

    //Main Functionality

    //Display the Letters in a grid
    public void DisplayOld(bool clearAll = false)
    {
        char dWallStart = (char) 0;
        char dWallEnd = (char) 0;
        //"Exists" uses predicates (inline functions) to search a list
        bool hasQu = /*_allowQu && */_diceList.Exists((Dice inputDice) => {return inputDice.GetCurLetter() == 'Q';});
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
                    Console.Write((hasQu /*&& _allowQu*/) ? "    " : " "); //Write 4 blank spaces "[__]"
                }
                //Console.Write(""); //Write the dice end border
                if(x == _width - 1)
                {
                    Console.WriteLine(""); //Write a new line
                }
            }
        }
    }

    //Smart Dice Display, uses modulus to display dice
    public void Display()
    {
        char dWallStart = (char) 0;
        char dWallEnd = (char) 0;
        //"Exists" uses predicates (inline functions) to search a list
        bool hasQu = /*_allowQu && */_diceList.Exists((Dice inputDice) => {return inputDice.GetCurLetter() == 'Q';});
        /*Create a display buffer, with it's length specified using the following calculations:
        ~ 1 for each character in the list
        ~ Use bitwise to detect ascii values that are non zero:
            Any ascii char:
            0x00 = 0000 0000 = 0  = null = U+0000
            0x01 = 0000 0001 = 1  = U+0001
            0x41 = 0100 0001 = 41 = 'A'
            (Any byte >> 4) | Any byte = compact to 4 bits (compact4)
            (Compact4 >> 2) | Compact4 = compacted to 8 bits (compact 2)
            (Compact2 >> 1) | Compact2 = Compacted to 1 bit
            Compact1 & 1 = 1 for non 0
            So the full formula is way to complicated to do on one line
            Alternate solution ((AnyByte >> 7) | (AnyByte >> 6) | (AnyByte >> 5) | (AnyByte >> 4) | (AnyByte >> 3) | (AnyByte >> 2) | (AnyByte >> 1) | (AnyByte)) & 1
            Alternate Solution: ternary operator: (AnyChar !=0) ? 1 : 0
        ~ Multiply by height to get the row height
        */
        char[] displayBuffer = new char[(((1 + ((dWallStart != 0) ? 1 : 0) + ((dWallEnd != 0) ? 1 : 0) + ((hasQu) ? 1 : 0)) * _width) + 1) * _height];
        
    }

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
            new UiOption(()=>{},"Edit &Dice"), //Open Dice Edit Menu
            new UiOption(GetWidth, SetWidth, "Grid &Width", 2), //Set Width
            new UiOption(GetHeight, SetHeight, "Grid &Height", 2), //Set Height
            new UiOption(()=>{throw new UiMenuExitException();},"Go &Back"),
        });
        diceSetSettings.UiLoop();
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
    //Ask if the user wants to fill the remaining slots in the Dice List automatically
    public void CheckSize(bool allowCheck = true)
    {
        if(allowCheck && (_width * _height) != _diceList.Count)
        {

        }
    }

    //Fill the dice list to an integer
    //Fill using a list of dice
    public void FillToCount(int newDiceCount, params Dice[] inputDice) //Utilizes the params keyword, which lets us use each item as an individual parameter: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params
    {
        for(int i = 0; _diceList.Count < newDiceCount; i++)
        {
            _diceList.Add(inputDice[i % inputDice.Length]); //Use modulus to infinitely loop the input array
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