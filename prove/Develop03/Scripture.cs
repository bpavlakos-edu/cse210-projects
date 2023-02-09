class Scripture{
    //Attributes
    Reference _scripRef = new Reference();
    List<Word> _wordList = new List<Word>();

    //Constructors
    public Scripture(){

    }
    public Scripture(string refString, string fullScriptureText)
    {
        _scripRef = new Reference(refString);
        StringToWords(fullScriptureText);
    }

    //Constructor Helpter Methods
    private void StringToWords(string fullScriptureText)
    {
        List<string> wordStrList = fullScriptureText.Replace(Environment.NewLine," ").Split(" ").ToList<string>();
        if(wordStrList.Count() > 0)
        {
            //Only fill the list if it's not empty
            _wordList = new List<Word>(); //Clear the word list
            wordStrList.ForEach((string newWordString) => {_wordList.Add(new Word(newWordString));});
        }
    }

    //Getters and Setters

    //Methods
    //Next turn
    public bool NextTurn()
    {
        return HideWords(3);
    }
    private bool HideWords(int wordHideMax)
    {
        List<int> visibleList = GetAllVisibleIdx(); //Get the latest visible index count
        if(visibleList.Count > 0)
        {
            bool wordHidden = false;//Flag to return if a word was hidden, we could use math to detect this, but having a boolean flag is safer
            while(visibleList.Count > 0 && wordHideMax > 0)
            {
                int hideWordIdx = new Random().Next(0, visibleList.Count); //Pick the next index
                _wordList[hideWordIdx].SetHidden(true); //Hide the word
                visibleList.RemoveAt(hideWordIdx); //Remove it from the visibile list, this allows us to exit the loop
                wordHideMax--;//Decrment wordHideMax so that we have a way to exit the loop when we've met our random number
            }
            return wordHidden;
        }else{
            return false;//All words are hidden
        }
    }
    //Get the index of all visible words
    private List<int> GetAllVisibleIdx()
    {
        //After realizing that this task is too complicated for a predicate search, I've decided to do a manual search instead
        if(IsAnyVisible()) //Use the IsAnyVisible Function to check if any are visible
        {
            List<int> returnList = new List<int>();
            for(int i = 0; i < _wordList.Count; i++)
            {
                if(!_wordList[i].GetHidden()) //Use
                {
                    returnList.Add(i);
                }
            }
        }
        return new List<int>(0);
        
        //Old documentation:
        //I wanted to use the FindAll function, but that doesn't let us keep the index
        //Documentation: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.findall?view=net-7.0#examples
        //List<Word> onlyVisible = _wordList.FindAll((Word wordObj)=>{return wordObj.GetHidden() == false;}); //This works but doesn't keep track of the index
        //Here's the solution, use contains instead: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.contains?view=net-7.0
    }

    //Use a 
    private bool IsAnyVisible(){
        //Actually the solution is to use "exists" instead of "contains", since "contains" is used for object values, and "exists" uses predicates (inline functions) to search
        return _wordList.Exists((Word curWord)=>{return curWord.GetHidden()==false;});
    }
    public void Display()
    {
        Console.WriteLine(_scripRef.GetRefText()+" "+WordListToString()); //Write the scripture reference and the word 
    }

    //Convert the word list to a string so we can display it
    private string WordListToString()
    {
        //Yes, this is a very slow way to combine strings, but the alternative is to turn each item into bytes, combine them together, and return them as a string
        string returnString = ""; //Initalize the return string
        for(int i=0;i<_wordList.Count;i++)
        {
            if(i != 0)
            {
                returnString+=" ";//Add a space before each word, except for the first word
            }
            returnString += _wordList[i].GetDisplayString(); //Get each words's display string, add it to the return string
        }
        return returnString; //Return the completed string
    }
}