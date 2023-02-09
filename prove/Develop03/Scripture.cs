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
        List<string> splitTextList = fullScriptureText.Replace(Environment.NewLine," ").Split(" ").ToList<string>(); //Split each individual "word" into a string entry of a list
        if(splitTextList.Count() > 0) //Only fill the list if the result of Split(" ") isn't empty
        {
            _wordList = new List<Word>(); //Clear the word list
            splitTextList.ForEach((string newWordText) => {_wordList.Add(new Word(newWordText));}); //Fill the word list by using the Word constructor that accepts a string input, pass the entry from the .split(" ") list as the string input 
        }
    }

    //Getters and Setters

    //Methods
    //Next turn
    public bool NextTurn()
    {
        return HideWords(3); //Hide 3 words
    }

    //Hide word request
    public bool HideWords(int wordHideMax = 3)
    {
        //Add randomization support
        if(wordHideMax < 0)
        {
            wordHideMax = (-wordHideMax) + 1; //Convert it to a positive number, add 1 to make it the ceiling
            if(wordHideMax > _wordList.Count + 1) //Replace numbers that exceed the list count with the list count to make it a better upper ceiling
            {
                wordHideMax = _wordList.Count + 1;
            }
            wordHideMax = (new Random().Next(1, wordHideMax)); //Pick a random number
        }
        List<int> visibleList = GetAllVisibleIdx(); //Get the latest visible index count
        if(visibleList.Count > 0)
        {
            bool wordHidden = false;//Flag to return if a word was hidden, we could use math to detect this, but having a boolean flag is safer
            while(visibleList.Count > 0 && wordHideMax > 0) //Exit the loop when we've removed all the words or we've met the max hide count
            {
                int hideWordIdx = new Random().Next(0, visibleList.Count); //Pick the next index
                _wordList[visibleList[hideWordIdx]].SetHidden(true); //Hide the word
                visibleList.RemoveAt(hideWordIdx); //Remove it from the visibile list, this allows us to exit the loop
                wordHideMax--;//Decrment wordHideMax so that we have a way to exit the loop when we've met our random number
                wordHidden = true;//We hid a word, so toggle the flag
            }
            return wordHidden; //Return if a word was successfully hidden
        }
        else
        {
            return false;//All words are hidden, so we can't hide any more words, send the result back
        }
    }
    //Get the index of all visible words
    private List<int> GetAllVisibleIdx()
    {
        //After realizing that this task is too complicated for a predicate search, I've decided to do a manual search instead
        if(IsAnyVisible()) //Use the IsAnyVisible Function to check if any are visible
        {
            List<int> returnList = new List<int>(); //Return list
            for(int i = 0; i < _wordList.Count; i++)
            {
                if(!_wordList[i].GetHidden()) //If the current word is not hidden
                {
                    returnList.Add(i); //Add it's index to the return list
                }
            }
            return returnList; //Return the return list
        }
        return new List<int>(0); //Return an empty list
        
        //Old documentation:
        //I wanted to use the FindAll function, but that doesn't let us keep the index
        //Documentation: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.findall?view=net-7.0#examples
        //List<Word> onlyVisible = _wordList.FindAll((Word wordObj)=>{return wordObj.GetHidden() == false;}); //This works but doesn't keep track of the index
        //Here's the solution, use contains instead: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.contains?view=net-7.0
    }
    //Use a "exists" search to determine if there are any words in _wordList that aren't hidden
    private bool IsAnyVisible(){
        //Actually the solution is to use "exists" instead of "contains", since "contains" is used for object values, and "exists" uses predicates (inline functions) to search
        return _wordList.Exists((Word curWord)=>{return curWord.GetHidden()==false;});
    }

    //Reset Words
    public void Reset()
    {
        _wordList.FindAll((Word wordItem) => {return wordItem.GetHidden();}).ForEach((Word HiddenWordItem)=>{HiddenWordItem.SetHidden(false);}); //Reset all words
        /*Explanation:
        list.FindAll returns all words in _wordList that have _hidden == true
        I then use list.ForEach on the result, and it's arrow function calls "SetHidden(false)" on each item, setting their hidden values to false
        Because each item in the FindAll list is still a memory reference to the ones in _wordList, the hidden words in _wordList have their _hidden values set to false!
        */
    }

    //Display this entire scripture reference by writing the appropriate string values to the console
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