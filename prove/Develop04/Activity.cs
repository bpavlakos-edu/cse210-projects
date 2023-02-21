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
    public Activity()
    {
        _name = "";
        _description = "";
        _messageList = new List<string>();
        _pauseStyle = -1;
    }

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
    public void Run(bool skipReady = false)
    {
        int durationMsec = ShowIntro();
        if(!skipReady) //Added a flag for the breathing activity to use
        {
            TransitionLoad("Get ready...");
        }
        Loop(durationMsec); //Start the main loop
        End(durationMsec); //Initalize the end sequence
    }

    //Override documentation found here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override#example
    public virtual void Loop(int durationMsec)
    {
        Console.WriteLine("My extra message will start here.");
        Console.WriteLine("");
        GetInput("Press enter to start");
        
        long[] tickTimes = GetTickStartEnd(durationMsec); //Store the start and end time
        long curTime = tickTimes[0]; //Item 0 is the current time in ticks
        
        int delayDurationMsec = 2000;
        List<string> previousMessageList = new List<string>(); //Keep track of what messages were used
        while(curTime < tickTimes[1])
        {
            Console.WriteLine("My test message");

            //Pick the next item
            string nextItem = GetRandomMsg(_messageList, previousMessageList); //Use the GetRandomMsg method that accepts a removal list as well
            
            Console.Write("Random List Item: "+ nextItem+" ");
            
            Pause(delayDurationMsec,_pauseStyle); //Request a pause using this activity's pause type
            curTime = (DateTime.Now).Ticks; //Update the current time
        }

        //Extra end behavior code can go here
    }

    protected void End(int durationMsec)
    {
        Console.WriteLine("");
        TransitionLoad("Well done!",true,false);
        TransitionLoad($"You have completed another {durationMsec / 1000} seconds of the {_name.ToLower()}.",true,false);
        //Ask if the user wants to restart
    }

    //Intro and outro helpers
    protected int ShowIntro()
    {
        Console.Clear();
        //Messages are matching the syntax as shown in the example here: https://byui-cse.github.io/cse210-course-2023/unit04/develop.html
        //Print Welcome
        Console.WriteLine($"Welcome to the {_name}.");
        Console.WriteLine("");
        
        //Print Description
        Console.WriteLine(_description);
        Console.WriteLine("");
        
        //Get the duration to return it at the end
        int durationMsec = GetIntInput("How long, in seconds, would you like for your session? ") * 1000; //Remember, thread.sleep is in msec, but datetime is in ticks
        return durationMsec;
    }
    protected void TransitionLoad(string inMsg = "Get ready...", bool newLine = true, bool clearAllStart = true, int durationMsec=4000, bool clearAllEnd = true)
    {
        //Flag to clear console at start
        if(clearAllStart)
        {
            Console.Clear();//Clear the console first
        }
        //Newline Flag
        if(newLine)
        {
            inMsg += Environment.NewLine;
        }
        PauseMsg(inMsg,durationMsec,0);//Write the message //Get a spinner
        if(clearAllEnd) //Added a flag for this functionality
        {
            Console.Clear();//Clear the console at the end
        }
    }
    //Pausing
    protected void Pause(int durationMsec, int pauseType)
    {
        //Cancellation token (see https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-cancel-a-task-and-its-children)
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        //How to initalize an async function: https://youtu.be/V2sMXJnDEjM?t=139
        //Intellisense helped me figure out the actual syntax for this:
        Task animTask = new Task(new Action(async ()=>{await RequestAnimation(durationMsec, pauseType);}),token); //Start the async function, using the task data type, action data type, and finally a lambda function to call the animation request function
        //Task animTask = RequestAnimation(durationMsec, pauseType); //An actual async call, since I noticed task was the actual return type
        //var test = new Thread(RequestAnimation(durationMsec, pauseType)); //Threading example: https://learn.microsoft.com/en-us/dotnet/standard/threading/pausing-and-resuming-threads
        //Wait and start were located here: https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming#creating-and-running-tasks-explicitly
        animTask.Start(); //Start the animation task
        //tokenSource.CancelAfter(durationMsec); //Request cancellation after the duration in msec
        Thread.Sleep(durationMsec); //Sleep
        
        /*if(animTask.Status != TaskStatus.RanToCompletion)
        {
            tokenSource.Cancel();
        }*/
        animTask.Wait(); //Wait for the display function to end
    }

    //Combining Pause with a console write
    protected void PauseMsg(string inMsg, int durationMsec, int pauseType)
    {
        Console.Write(inMsg);
        Pause(durationMsec, pauseType);
        Console.WriteLine("");
    }

    //Pick an item from the list at random
    protected string GetRandomMsg(List<string> selectionList)
    {
        if(selectionList.Count > 1)
        {
            return selectionList[new Random().Next(selectionList.Count())];
        }
        else if(selectionList.Count == 1)
        {
            return selectionList[0];
        }
        else //Empty list
        {
            return "";
        }
    }

    //Overload to handle a second list input, which keeps track of which items have been used already
    protected string GetRandomMsg(List<string> selectionList, List<string> removeList)
    {
        if(selectionList.Count > 0) //Accept lists with a item count greater than 0
        {
            List<string> selectionListCopy = selectionList.ToList<string>();//Copy the list so we don't modify the original
            //Remove all instances of the remove list values
            for (int i =0;i<removeList.Count;i++)
            {
                while(selectionListCopy.Contains(removeList[i]))
                {
                    selectionListCopy.Remove(removeList[i]);
                }
            }
            string returnStr = "";
            if(selectionListCopy.Count == 0) //Selection list has no items
            {
                Console.WriteLine("Resetting remove list!"); //Debugging
                removeList = new List<string>(); //Reset the remove list (it's mutable so we can affect it from here)
                returnStr = GetRandomMsg(selectionList); //Use the original list as the call instead 
            }
            else //We can choose a random item
            {
                returnStr = GetRandomMsg(selectionListCopy);//Call the function using
            }
            removeList.Add(returnStr); //Regardless of what happens, add the newly picked string to the removeList, which will change the original list because of mutability
            return returnStr; //Return the return string
        }
        else //Return "" for 0 length strings
        {
            return "";
        }
    }

    //Calcuations
    //Pointlessly created a function to do this, in the hopes that the code that uses it would be one line, instead I just made a way to get a start and end duration from miliseconds
    protected long[] GetTickStartEnd(int durationMsec)
    {
        //Ticks documentation: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.millisecond?source=recommendations&view=net-7.0
        long curTime = (DateTime.Now).Ticks;
        long endTime = curTime + (durationMsec * 10000); //There are 10000 ticks in a milisecond according to the docs: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-7.0
        return new long[]{curTime, endTime};
    }

    //User input
    //Get a generic input from the user
    protected string GetInput(string inMsg)
    {
        Console.Write(inMsg);
        return Console.ReadLine();
    }
    //Get a whole number from a user (supports minimum and maximum)
    protected int GetIntInput(string inMsg, int min = 0, int max = 0)
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

    //Animation helpers
    protected async Task RequestAnimation(int durationMsec, int pauseType, int fps = 60)
    {
        //There is an error here, saying that because I'm not using "await" this is not asyncronous, but I am calling it using an await from a task action
        //Using Await on this method's call would require adding "async" to the Pause() method, which is not practical
        //Console.WriteLine($"Requesting animation type {pauseType}");
        //Switch case in C#: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#the-switch-statement
        switch (pauseType)
        {
            //Found the syntax for task factory on this stack overflow post: https://stackoverflow.com/a/12144426 (Main post: https://stackoverflow.com/questions/12144077/async-await-when-to-return-a-task-vs-void)
            case 0:
                //Spinner
                //await Task.Factory.StartNew(() => {ActivateLoopAnim(durationMsec, new List<string>{"-","\\","|","/"}, 250);});
                await Task.Factory.StartNew(() => {ActivateLoopAnim(durationMsec, new List<string>{"1","2","3","4"}, 250);});//Debugging
                break;
            case 1:
                //Count down timer
                await Task.Factory.StartNew(() => {ActivateAnimTimerMsec(durationMsec);});
                break;
            case 2:
                //Count up timer
                await Task.Factory.StartNew(() => {ActivateAnimTimerMsec(durationMsec, false);});
                break;
            default:
                //No display (such as -1 which means the loop handles the request)
                await Task.Factory.StartNew(() => {}); //Do nothing
                break;
        }
    }

    //Activate a loop animation
    private void ActivateLoopAnim(int durationMsec, List<string> frameChars, int msecPerFrame)
    //Async Documentation: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async
    //Documentation for generic lists (list<object>): https://learn.microsoft.com/en-us/dotnet/api/system.collections.arraylist?view=net-7.0#remarks
    {
        int frameLen = frameChars.Count;
        int maxFrame = (durationMsec / msecPerFrame); //Automatically calculate the maximum frame

        //Console.Write(frameChars[0]);//Add an extra space to overrwrite
        for(int curFrame= 0; curFrame <= maxFrame; curFrame++)
        {
            //Console.Write(" ");//Add an extra space to overrwrite by a frame
            DisplayFrame(frameChars[curFrame % frameLen], msecPerFrame); //Increment current frame, and ensure it doesn't exceed the animation length
            //Console.Write("\b \b");//Erase the final frame
        }
    }
    //Countdown timer or just a regular timer
    private void ActivateAnimTimer(int durationSec, bool countdownFlag = true)
    {
        //Console.Write(" ");//Add an extra space to overrwrite by a frame
        for(int i = 0; i < durationSec; i++)
        {
            //Console.Write(" ");//Add an extra space to overrwrite by a frame
            if(countdownFlag) //Countdown
            {
                int lastLength = ((i-1) + "").Replace("-","").Length; //Get the length of the previous string, ignore the negative sign (for 0)
                DisplayFrame(durationSec - i, 1000, lastLength);
            }
            else //Count up timer
            {
                int lastLength = ((i-1) + "").Replace("-","").Length; //Get the length of the previous string, ignore the negative sign (for 0)
                DisplayFrame(i, 1000, lastLength);
            }
            //Console.Write("\b \b");//Erase the final frame
            
            //3-line version:
            //int boolInt = BitConverter.ToInt32(BitConverter.GetBytes(countdownFlag)); //Convert the boolean to an integer //From: https://learn.microsoft.com/en-us/dotnet/api/system.boolean?view=net-7.0#work-with-booleans-as-binary-values
            //int iValue = (boolInt * durationSec) + (i * (1 - (2 * boolInt))); //Deactivate durationSec when false, flip i negative when true
            //DisplayFrame(iValue, 1000); //Display the result
        }
        //Console.Write("\b \b");//Erase the final frame
        //Console.WriteLine("");
    }
    //Function overload for using MSEC instead of seconds
    private void ActivateAnimTimerMsec(int durationMsec, bool countdownFlag = true)
    {
        ActivateAnimTimer(durationMsec / 1000, countdownFlag);
    }

    //Display a single animation frame, accepts objects so that numbers are handled too
    private void DisplayFrame(object frameObj, int msecPerFrame, int lastFrameLength = 1)
    {
        if(lastFrameLength == 1) //Normal display length 
        {
            //Gained additional understanding of what this is doing by reading this stack overflow post: https://stackoverflow.com/questions/5195692/is-there-a-way-to-delete-a-character-that-has-just-been-written-using-console-wr
            Console.Write("\b \b"); //Backspace to clear
        }
        else //Different length
        {
            //Backspace for each additional length
            for(int i = 0; i < lastFrameLength; i++)
            {
                Console.Write("\b \b");
            }
            //Console.Write("");//Add the extra space
        }
        Console.Write(frameObj); //Write this frame
        Thread.Sleep(msecPerFrame); //Make the thread sleep
        //Console.Write("\b");
    }
    //Function overload for display frame for code completion to recognize
    /*private void DisplayFrame(string frameStr, int msecPerFrame)
    {
        DisplayFrame(frameStr, msecPerFrame);
    }*/
}