class Word
{
    //Attributes
    private string _text; //Stores the word's string value
    private bool _hidden; //Controls the hidden state of the word

    //Constructor
    public Word()
    {
        _text = "";
        _hidden = false;
    }
    public Word(string text)
    {
        _text = text;
        _hidden = false;
    }
    
    //Getters and Setters
    //_text
    public string GetText()
    {
        return _text;
    }
    public void SetText(string text)
    {
        _text = text;
    }

    //_hidden
    public bool GetHidden()
    {
        return _hidden;
    }
    public void SetHidden(bool hidden)
    {
        _hidden = hidden;
    }

    //Methods
    //Get the display string for this word
    public string GetDisplayString(){
        if(!_hidden)
        {
            return _text; //Return the text when it's not hidden
        }
        return new string('_',_text.Length); //Use the string constructor that accepts char and length to generate the underscore string (found with intellisense)
    }
}