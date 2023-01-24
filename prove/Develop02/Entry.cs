class Entry
{
    //Attributes
    public string _textEntry = "";
    public string _prompt = "";
    public string _timestamp = "";
    //Constructors
    //Blank Constructor
    public Entry()
    {

    }
    //User input constructor (requires prompt paramter)
    public Entry(string prompt)
    {
        _timestamp = GetTimestamp(); // This function could update the parameter directly instead of returning it
        _prompt = prompt;
        _textEntry = GetTextEntry();

    }
    //Entry creation functions
    public string GetTextEntry()
    {
        Console.Write(_prompt+Environment.NewLine+"> ");//Display the prompt and the "> " on the next line
        return Console.ReadLine();
    }
    public string GetTimestamp()
    {
        //Formatting Picked by using intellisense and the following documentation:
        //https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tolongdatestring?view=net-7.0
        return (DateTime.Now).ToShortDateString();
    }

    //Display
    public void Display()
    {
        Console.WriteLine(this.ToString());
    }

    //ToString override for file writing and display
    public override string ToString()
    {
        string returnStr = $"Date: {_timestamp} - Prompt: {_prompt}{Environment.NewLine}{_textEntry}";
        return returnStr;
    }

}