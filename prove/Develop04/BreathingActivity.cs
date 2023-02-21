//Activity Type 1
class BreathingActivity : Activity
{
    //Constructors
    public BreathingActivity() : base()
    {

    }
    //Fill all parameters
    public BreathingActivity(string name, string description, List<string> messageList, int pauseStyle) : base(name, description, messageList.ToList<string>(), pauseStyle)
    {

    }
    //Main functionality Loop (attempting to override the original)
    //Override documentation found here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override#example
    //Finally understood it by looking at this page: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#1465-override-methods
    //And also noticing that I had the wrong function name!!!
    public override void Loop(int durationMsec)
    {
        int GapTime = GetIntInput("Enter how long you you will hold a breath (in seconds): ",1,9) * 1000;
        TransitionLoad("Get ready...");

        long[] tickTimes = GetTickStartEnd(durationMsec);
        long curTime = tickTimes[0]; //Get the start time

        while(curTime < tickTimes[1])
        {
            Console.Write(_messageList[0]+" "); //Breathe in
            Pause(GapTime,_pauseStyle);
            Console.Write(_messageList[1]+" "); //Breathe out
            Pause(GapTime,_pauseStyle);
            Console.WriteLine("");
            curTime = (DateTime.Now).Ticks;
        }
        
    }
}