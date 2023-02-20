using System.Collections;
//Super Class - Activity: Contains core functionality for all Activity Sub Classes

class Activity
{
    //Attributes
    protected string _name;
    protected string _description;
    protected List<string> _messageList;
    protected int _pauseStyle;

    //Constructors

    //Fill Attribute Constructor
    public Activity(string name, string description, List<string> messageList, int pauseStyle)
    {
        _name = name;
        _description = description;
        _messageList = messageList.ToList<string>();
        _pauseStyle = pauseStyle;
    }

    //Getters and Setters
    public string GetName()
    {
        return _name;
    }
    public void Set(string name)
    {
        _name = name;
    }
    public string GetDescription()
    {
        return _description;
    }
    public void SetDescription(string description)
    {
        _description = description;
    }
    public List<string> GetMessageList()
    {
        return _messageList.ToList<string>(); //Clone the list so the original isn't changed
    }
    public void SetMessageList(List<string> messageList)
    {
        _messageList = messageList.ToList<string>(); //Clone the list so the input list isn't changed
    }
    public int GetPauseStyle()
    {
        return _pauseStyle;
    }
    public void SetPauseStyle(int pauseStyle)
    {
        _pauseStyle = pauseStyle;
    }

    //Methods

    //Main Functionality Flow (Run and loop are Placeholders/templates for subclass overrides)
    public void Run()
    {
        int durationMsec = ShowIntro();
        Loop(durationMsec);
        End();
    }

    public void Loop(int durationMsec)
    {

    }

    public void End()
    {

    }
    //Intro and outro helpers
    public int ShowIntro()
    {
        //Messages are matching the syntax as shown in the example here: https://byui-cse.github.io/cse210-course-2023/unit04/develop.html
        //Print Welcome
        Console.WriteLine($"Welcome to the {_name}.");
        Console.WriteLine("");
        //Print Description
        Console.WriteLine(_description);
        Console.WriteLine("");
        //Get the duration to return it at the end
        int durationMsec = GetIntInput("How long, in seconds, would you like for your session? ") * 1000; //Remember, thread.sleep is in msec, but datetime is in ticks
        TransitionLoad("Get ready...");
        return durationMsec;
    }
    public void TransitionLoad(string inMsg = "Get ready...", bool newLine = true)
    {
        Console.Clear();//Clear the console first
        //Newline Flag
        if(newLine)
        {
            inMsg += Environment.NewLine;
        }
        Console.Write(inMsg);//Write the message
        Pause(6000,0); //Get a spinner
        Console.Clear();//Clear the console again
    }
    //Pausing and animations
    public void Pause(int durationMsec, int pauseType)
    {
        //Ticks documentation: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.millisecond?source=recommendations&view=net-7.0
        long curTime = (DateTime.Now).Ticks;
        long endTime = curTime + (durationMsec * 10000); //There are 10000 ticks in a milisecond according to the docs: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-7.0
        
        //Task animTask = DisplayAnimation(pauseType); //Start the async function
        RequestAnimation(pauseType, durationMsec); //Start the async function
        while(curTime < endTime)
        {
            //Run animation?
            curTime = (DateTime.Now).Ticks;
        }
    }
    
    private void RequestAnimation(int pauseType, int durationMsec, int fps = 60)
    {
        //Switch case in C#: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#the-switch-statement
        switch (pauseType)
        {
            case 0:
                //Spinner
                ActivateLoopAnim(new List<object>{"-","\\","|","/"},250);
                break;
            case 1:
                //Count down timer
                ActivateAnimTimerMsec(durationMsec);
                break;
            default:
                //No display
                break;
        }
    }

    //Async Documentation: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async
    private void ActivateLoopAnim(List<object> frameChars, int msecPerFrame = 1000)
    {
        int frame = 0;
        int frameLen = frameChars.Count;
        //Console.Write(frameChars[0]);//Add an extra space to overrwrite
        Console.Write(" ");//Add an extra space to overrwrite by a frame
        while(true)
        {
            DisplayFrame(frameChars[frame % frameLen], msecPerFrame);
            frame++; //Increment current frame
        }
    }
    private void ActivateAnimTimer(int durationSec, bool countdownFlag = true)
    {
        for(int i = 0; i < durationSec; i++)
        {
            if(countdownFlag) //Countdown
            {
                DisplayFrame(durationSec - i, 1000);
            }
            else //Count up timer
            {
                DisplayFrame(i, 1000);
            }
            
            //3-line version:
            //int boolInt = BitConverter.ToInt32(BitConverter.GetBytes(countdownFlag)); //Convert the boolean to an integer //From: https://learn.microsoft.com/en-us/dotnet/api/system.boolean?view=net-7.0#work-with-booleans-as-binary-values
            //int iValue = (boolInt * durationSec) + (i * (1 - (2 * boolInt))); //Deactivate durationSec when false, flip i negative when true
            //DisplayFrame(iValue, 1000); //Display the result
        }
    }
    private void ActivateAnimTimerMsec(int durationMsec, bool countdownFlag = true)
    {
        ActivateAnimTimer(durationMsec / 1000, countdownFlag);
    }
    

    private void DisplayFrame(object frameStr, int msecPerFrame)
    {
        Console.Write("\b \b"); //Backspace to clear
        Console.Write(frameStr); //
        Thread.Sleep(msecPerFrame);
    }

    //User input
    //Get a generic input from the user
    public string GetInput(string inMsg)
    {
        Console.Write(inMsg);
        return Console.ReadLine();
    }
    //Get a number from a user
    public int GetIntInput(string inMsg, int min = 0, int max = 0)
    {
        while(true) //Repeat until a valid number is found
        {
            //Catch parsing errors
            try 
            {
                int returnInt = int.Parse(GetInput(inMsg)); //Parse the user input
                //Determine if the current integer is a valid number
                if(min == max) //This means no minimum or maximum was set
                {
                    return returnInt;//Exit the while loop by returning the value
                }
                else if(returnInt <= max && returnInt >= min) //is the number between the minimum and maximum?
                {
                    return returnInt;//Exit the while loop by returning the value
                }
                else //Invalid number
                {
                    Console.WriteLine($"That's not a number between {min} and {max}, please try again!");
                }
            }
            catch(FormatException) //Not a number
            {
                Console.WriteLine($"That's not a valid whole number, please try again!");
            }
            catch(ArgumentNullException) //Empty input
            {
                Console.WriteLine("Please enter a number to continue!");
            }
            catch(OverflowException) //Overflow
            {
                Console.WriteLine("That's not a number the program can process, please try again!");
            }
        }
    }

    

}