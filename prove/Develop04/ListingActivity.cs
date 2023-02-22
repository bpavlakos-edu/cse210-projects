class ListingActivity : Activity
{
    //Hidden attribute
    private bool _timeExpired = false; //A flag to tell the threaded function whether it can display that the current prompt is the last one
    //Constructors
    //Blank
    public ListingActivity() : base()
    {

    }
    //Fill all attributes
    public ListingActivity(string name, string description, List<string> messageList, int pauseStyle) : base(name,description, messageList, pauseStyle)
    {
        //ToList is handled by the base class constructor
    }

    //Methods
    //New loop functionality, overrides the original, so that's it's triggered on "run"
    public override void Loop(int durationMsec)
    {
        _timeExpired = false; //Reset the global variable
        Console.WriteLine("List as many responses as you can to the following prompt:");
        Console.WriteLine($" --- {GetRandomMsg(_messageList)} ---");//Use GetRandomMessage to pick the prompt
        PauseMsg("You may begin in: ",5000,1); //Countdown timer

        long[] tickTimes = GetTickStartEnd(durationMsec); //Get the start and end times
        long curTime = tickTimes[0]; //Initalize curTime to the start time

        //Loop setup
        new Thread(()=>{TimerExpireNotification(durationMsec - 100);}).Start(); //Start the notification thread, give it a 100msec head start
        int enterCounter = 0;
        while(curTime < tickTimes[1])
        {
            if(GetInput("> ") != "")
            {
                enterCounter++; //Count non empty strings
            }
            //Use the spinner animation to generate the new line instead
            curTime = (DateTime.Now).Ticks; //Update the current time
        }
        ToggleLoopExited(); //Trigger the exit status
        //Handle "items" vs "item"
        string s_string = "s";
        if(enterCounter == 1)
        {
            s_string="";
        }
        Console.WriteLine($"You listed {enterCounter} item{s_string}!");
        //if(!_loopExited)
    }

    private bool ToggleLoopExited()
    {
        _timeExpired = !_timeExpired; //Invert the status of LoopExited, true -> false, false -> true
        return _timeExpired; //Return the current status
    }

    private void TimerExpireNotification(int msecDelay)
    {
        Thread.Sleep(msecDelay);
        if(ToggleLoopExited())//_loopExited was false, but is now set to true (We can print the marker)
        {
            int tempLeft = Console.CursorLeft;
            Console.CursorLeft = 0;
            Console.Write("*>");
            Console.CursorLeft = tempLeft;
        }
        //_loopExited was true, but is now set to false (We cannot print the marker the loop has finished)
    }
}