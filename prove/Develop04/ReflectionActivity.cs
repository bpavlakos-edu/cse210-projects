class ReflectionActivity : Activity
{
    //New Attributes
    List<string> _refQuestions;

    //Constructor
    public ReflectionActivity():base()
    {

    }
    //Fill All Attributes
    public ReflectionActivity(string name, string description, List<string>messageList, int pauseStyle, List<string> refQuestions):base(name, description, messageList, pauseStyle)
    {
        _refQuestions = refQuestions.ToList<string>(); //Use toList to erase the reference to the original list
    }

    //Getters and setters
    public List<string> GetRefQuestions()
    {
        return _refQuestions.ToList<string>(); //Use toList to erase the reference to the original list
    }
    public void SetRefQuestions(List<string> refQuestions)
    {
        _refQuestions = refQuestions.ToList<string>(); //Use toList to erase the reference to the original list
    }

    public override void Loop(int durationMsec)
    {
        //Pick and display the prompt
        Console.WriteLine("Consider the following prompt:");
        Console.WriteLine("");
        Console.WriteLine($" --- {GetRandomMsg(_messageList)} ---"); //Use GetRandomMessage to pick the prompt
        Console.WriteLine("");
        //Wait for user input
        GetInput("When you have something in mind, press enter to continue.");
        Console.WriteLine("");
        
        Console.WriteLine("Now ponder on each of the following questions as they are related to this experience.");
        Console.Write("You may begin in: ");
        Pause(5000,1); //Countdown timer

        Console.Clear();

    }

}