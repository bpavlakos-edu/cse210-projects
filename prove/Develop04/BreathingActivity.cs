//Activity Type 1
class BreathingActivity : Activity
{
    //Constructors
    public BreathingActivity() : base()
    {

    }
    //Fill all parameters
    public BreathingActivity(string name, string description, List<string> messageList, int pauseStyle) : base(name, description, messageList, pauseStyle)
    {
        //ToList is handled by the base class constructor
    }
    //Main functionality Loop (attempting to override the original)
    //Override documentation found here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override#example
    //Finally understood it by looking at this page: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#1465-override-methods
    //And also noticing that I had the wrong function name!!!
    public override void Loop(int durationMsec)
    {
        int GapTime = GetIntInput("Enter how long you you will hold a breath (in seconds): ",1,9) * 1000; //Custom input for breathing length
        TransitionLoad("Get ready...");

        long[] tickTimes = GetTickStartEnd(durationMsec);
        long curTime = tickTimes[0]; //Get the start time
        
        //Main sequence loop
        while(curTime < tickTimes[1])
        {
            bool lastCycle = (curTime + ((2 * durationMsec) * 10000) >= tickTimes[1]);
            if(lastCycle) //Check for final set
            {
                Console.WriteLine("Final Set!");
            }
            //Breathe in
            PauseMsg(_messageList[0]+" ",GapTime,_pauseStyle);
            //Breathe out
            PauseMsg(_messageList[1]+" ",GapTime,_pauseStyle);
            if(!lastCycle) //Don't make a new line on the final set
            {
                //Console.WriteLine(""); //Next step
            }
            curTime = (DateTime.Now).Ticks; //Update timer
        }

        //Return overtime
    }
}