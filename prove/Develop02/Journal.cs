using System.Text.Json;
class Journal
{
    //Attributes
    public string _ownerName = "My"; //The journal owner name
    public List<Entry> _entryList = new List<Entry>(); //Journal entries
    //Prompt list
    public List<String> _promptList = new List<String>{
        //Suggested Prompts (from: https://byui-cse.github.io/cse210-course-2023/unit02/develop.html)
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?",
        //New prompts
        "What was the biggest challenge for me today?",
        "What is something I was glad I did today?",
        "What is one thing I did today to help others?",
        "What was the funniest thing I heard today?",
        "What is a specific detail of today that I won't remember in a month?"
    };
    //Blank Constructor
    public Journal(){

    }
    /*"Deep" cloning constructor
    In an old C# project of mine, I needed to clone a custom class that contained a lot of lists, 
    The solution to this problem was to use the ".ToList()" function on every list, which returns a new list, not a reference to the original
    The original cited solution is here: https://stackoverflow.com/questions/15330696/how-to-copy-list-in-c-sharp*/
    public Journal(Journal newJournal){
        _ownerName = newJournal._ownerName;
        _entryList = newJournal._entryList.ToList<Entry>();
        _promptList = newJournal._promptList.ToList<String>();
    }

    public Journal(string ownerName, List<Entry> entryList, List<String> promptList)
    {
        _ownerName = ownerName;
        _entryList = entryList.ToList<Entry>();
        _promptList = promptList.ToList<String>();
    }

    //Functions

    //Add a new entry
    public void WriteNewEntry()
    {
        _entryList.Add(new Entry(PickPrompt()));
    }
    //Randomly pick a prompt (probably should be private)
    public string PickPrompt()
    {
        Random randomGen = new Random();
        return _promptList[randomGen.Next(_promptList.Count())];//Pick a random prompt
    }

    //Display
    public void Display()
    {
        Console.Write(this.ToString());
    }

    //ToString override (for both display and text output)
    override public string ToString()
    {
        string returnStr = GetOwnerName()+Environment.NewLine; //[OwnerName] Journal
        //Write all entries to the return string, in the format specified by the instructions
        //I usually use a length count for loop, but foreach is something I want to challenge myself to use in C# (especially since the index isn't being used here)
        foreach(Entry jEntry in _entryList)
        {
            returnStr += jEntry.ToString()+Environment.NewLine+Environment.NewLine; //By adding two newlines we add a blank line between each entry, this isn't part of the entry class, because it's "toString" is only supposed to display itself, not a collection
        }
        return returnStr;
    }

    //Get the owner name + "Journal"
    public string GetOwnerName()
    {
        return _ownerName+" Journal";
    }

    //Set the new owner name (obey grammar rules with "'s")
    public void SetOwnerName(){
        string uInput = Program.GetStrInput("Enter the new Journal Owner's Name (leave blank to cancel): "); //Call the root program for it's GetStrInput utility function
        if(uInput != "")
        {
            if(uInput.ToLower().Substring(uInput.Length - 1) == "s") //Check if the last letter is "s"
            { 
                _ownerName = uInput + "'"; //James -> James' Journal
            }
            else
            {
                _ownerName = uInput + "'s"; //John -> John's Journal
            }
        }
    }

    public string ToJson()
    {
        //Solution to serialzation issue found in the following stack overflow chain:
        //1. C# Doesn't support serializing fields: http://stackoverflow.com/questions/62717934/ddg#62718200
        //2. C# has field serialization disabled by default: https://stackoverflow.com/questions/58784499/system-text-json-jsonserializer-serialize-returns-empty-json-object#comment103854049_58784566
        //3. How to enable serialization of fields in C#: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-7-0#include-fields
        //So the solution is to add a JsonSerializerOptions object to the serialize function call with the IncludeFields option set to true
        //The docs suggested writing it indented for files the user accesses
        return JsonSerializer.Serialize<Journal>(this, new JsonSerializerOptions{IncludeFields = true, WriteIndented = true});
    }
    public void FromJson(string jsonText)
    {
        try
        {
            Journal journalFromJSON = JsonSerializer.Deserialize<Journal>(jsonText, new JsonSerializerOptions{IncludeFields = true, WriteIndented = true});
            _ownerName = journalFromJSON._ownerName;
            _entryList = journalFromJSON._entryList.ToList<Entry>();
            _promptList = journalFromJSON._promptList.ToList<String>();
        }
        catch(ArgumentNullException)
        {
            Console.WriteLine("Failed to decode JSON, check the file for errors");
            return; //Ignore failed serializations
        }
        
    }
}