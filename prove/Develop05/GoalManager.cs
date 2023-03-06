using System.Text.Json; //Lets us use JSON serialization and deserialization

class GoalManager
{
    //Attributes
    private List<Goal> _goalList = new List<Goal>();
    private long _points = 0;
    private string _userName = "My";
    
    //Constructors
    //Blank
    public GoalManager()
    {
        _goalList = new List<Goal>();
        _points = 0;
        _userName = "My";
    }
    //Fill all attributes
    public GoalManager(List<Goal> goalList, long points)
    {
        _goalList = goalList.ToList<Goal>();//Copy the list to break the reference to the original
        _points = points;
    }
    //Generate from file path
    public GoalManager(string filePath)
    {
        //Not implemented
    }
    //Copy from an existing goal manager
    public GoalManager(GoalManager newGoalManager)
    {
        //Update this goal list using the values of the GoalManager argument
        _goalList = newGoalManager.GetGoalList();
        _points = newGoalManager.GetPoints();
    }

    //Getters and setters
    public List<Goal> GetGoalList()
    {
        return _goalList.ToList<Goal>();//Copy the list to break the reference to the original
    }
    public void SetGoalList(List<Goal> goalList)
    {
        _goalList = goalList.ToList<Goal>();//Copy the list to break the reference to the original
    }
    public long GetPoints()
    {
        return _points;
    }
    public void SetPoints(long points)
    {
        _points = points;
    }
    public string GetUserName()
    {
        return _userName;
    }
    public void SetUserName(string userName)
    {
        _userName = userName;
    }

    //Methods
    //Main Display (for goals)
    public void Display()
    {
        //Consider using UI Menu here (Programically generate options)
        Console.WriteLine("The goals are:");
        for(int i = 0; i < _goalList.Count; i++)
        {
            //Console.WriteLine(_goalList[i].ToDisplayString(i));
        }
    }
    //Goal List Modification
    public void NewGoal()
    {
        //Determine goal Type
        //"The types of Goals are: "
        /*
        "Which type of goal would you like to create? "
        */
        //Add goal to list
        //_goalList.Add(new GoalType(0));
    }
    public void DeleteGoal()
    {
        //Consider using UI Menu Here
        //Int input (index of goal in list), invalid to cancel
        //_goalList.RemoveAt(index);
    }
    public void EditGoal()
    {
        //Consider using UI Menu Here
        //Int input (index of goal in list), invalid to cancel
        //Int input (index of property to change), invalid to cancel
        //input / int input, new value [Write new value]
    }
    //File loading and saving (Method Overloads)
    public void Save()
    {
        string fileName = GetInput("What is the filename for the goal file? ");
        Save(fileName);
    }
    public void Load()
    {
        string fileName = GetInput("What is the filename for the goal file? ");
        Load(fileName);
    }

    //Goal Marking
    public void RecordEvent()
    {
        //Consider using UI Menu Here
        //Int input (index of goal in list), invalid to cancel
        int goalIndex = GetIntInput("Which goal did you accomplish? ");
        //_points += _goalList[goalIndex].Mark();
    }

    //Ported from Journal.cs in Develop 02
    //Change Goal Manager Owner name (obey grammar rules with "'s")
    public void ChangeName()
    {
        string uInput = GetInput("Please Enter Your Name (Leave blank to cancel): ");
        if(uInput != "")
        {
            if(uInput.ToLower().Substring(uInput.Length - 1) == "s") //Check if the last letter is "s"
            { 
                _userName = uInput + "'"; //James -> James' Goals
            }
            else
            {
                _userName = uInput + "'s"; //John -> John's Goals
            }
        }
    }

    //File Management
    public void Save(string filePath)
    {
        try
        {
            //I found File.CreateText() using intellisense, it creates the file if missing, or erases it before writing
            using(StreamWriter sWriter = File.CreateText(filePath))
            {
                string jsonString = ToJson(); //Generate the JSON string if possible
                if(jsonString != "")
                {
                    sWriter.Write(jsonString); //Write the JSON string
                    Console.WriteLine("File successfuly saved");
                }
                else
                {
                    Console.WriteLine("Failed to save file, JSON text was empty!");
                }
            }
        }
        catch(IOException e) //IO Errors
        {
            Console.WriteLine($"Unable to create file, error: {e.ToString()}");
        }
        catch(NotSupportedException){}
        catch(ObjectDisposedException){}
        catch(ArgumentException){}
        //catch(ArgumentNullException){} //Caught by ArgumentException
    }
    public void Load(string filePath)
    {
        try
        {
            string jsonText = "";
            using(StreamReader sReader = File.OpenText(filePath))
            {
                jsonText = sReader.ReadToEnd(); //Read all file lines
            }
            FromJson(jsonText); //Use the FromJson method, which will update the global manager
        }
        catch(IOException e) //IO Errors
        {
            Console.WriteLine($"Unable to load file, error: {e.ToString()}");
        }
        //Other errors
        catch(NotSupportedException){}
        catch(ObjectDisposedException){}
        catch(ArgumentException){}
        //catch(ArgumentNullException){} //Caught by ArgumentException
    }

    //JSON Helpers
    //Json Serialization is based on Develop02 Journal.cs  "ToJson()" method
    private string ToJson()
    {
        try
        {
            //Uses JSON settings to include fields and indentation
            return JsonSerializer.Serialize<GoalManager>(this, new JsonSerializerOptions{IncludeFields = true, WriteIndented = true});
        }
        catch(NotSupportedException e) //This exception has multiple meanings, so we better print it
        {
            Console.WriteLine($"Error {e.ToString()}");
            return "";
        }
        
    }
    //Json Deserialization is based on Develop02 Journal.cs "FromJson()" method
    private void FromJson(string jsonText)
    {
        if(jsonText != "")
        {
            try
            {
                //Uses JSON settings to include fields and indentation
                GoalManager newGoalManager = JsonSerializer.Deserialize<GoalManager>(jsonText, new JsonSerializerOptions{IncludeFields = true, WriteIndented = true});
                //Set current goal manager values to the ones from the deserialized object
                _goalList = newGoalManager.GetGoalList();
                _points = newGoalManager.GetPoints();
                _userName = newGoalManager.GetUserName();
            }
            catch(JsonException e)
            {
                Console.WriteLine($"Error, {e.ToString()}");
            }
            catch(NotSupportedException e) //This exception has multiple meanings, so we better print it
            {
                Console.WriteLine($"Error, {e.ToString()}");
            }
            catch(ArgumentNullException e)
            {
                Console.WriteLine($"Error, file was empty! {e.ToString()}");
            }
        }
        else
        {
            Console.WriteLine("Error, file was empty!");
        }
    }

    //User Input
    private string GetInput(string inMsg)
    {
        Console.Write(inMsg); //Write the message
        return Console.ReadLine(); //Read the input
    }
    private int GetIntInput(string inMsg, int min=0, int max=0)
    {
        //Todo: Add Min Max Support
        //Consider putting ParseInt as it's own function in UiMenu
        while(true)
        {
            try
            {
                try
                {
                    int returnVal = int.Parse(GetInput(inMsg));
                    //Insert min/max control here (throw new ArgumentNullException)
                    if(min == max)
                    {
                        return returnVal;
                    }
                    else if(max < min && returnVal >= min) //When max < min it's Minimum only
                    {
                        return returnVal;
                    }
                    /*else if(maximumOnly)
                    {
                        return returnVal;
                    }*/
                    else if(returnVal >= min && returnVal <= max)//Check if the input number is in bounds
                    {
                        return returnVal;
                    }
                    else //Not in bounds
                    {
                        //Do nothing continue with loop
                        //Print message
                        Console.WriteLine("Sorry, that's not a valid number, please try again!");
                    }
                    
                }
                catch(OverflowException){throw new ArgumentNullException();}
                catch(FormatException){throw new ArgumentNullException();}
            }
            catch(ArgumentNullException)//Throw every exception into this block of code
            {
                Console.WriteLine("Sorry, that's not a valid number, please try again!"); //Fail message
            }
        }
    }

    //Utility
    private Goal GetGoal(string inMsg="Select the goal you want to change: ")
    {
        return _goalList[GetIntInput(inMsg, 1, _goalList.Count) - 1]; //Get the current goal (max is goalList.count because its -1 the user input)
    }

}