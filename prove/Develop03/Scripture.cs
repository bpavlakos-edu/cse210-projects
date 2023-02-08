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

    //Methods
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
}