using System.Text.Json; //Lets us use JSON serialization and deserialization
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using UiMenu = QuickUtils.UiMenu; //Importing the custom UI menu class
using UiOption = QuickUtils.UiOption; //Importing the custom Ui Option class
using System.Runtime.Serialization;

[DataContract]
class GoalManager
{
    //How to serialize private members: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/required-properties
    //Note that the import in the example I used (example 2) is incorrect! It should be "using System.Text.Json.Serialization", not "using System.Text.Json"!
    //Also you need a seperate tag for serialzation: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/immutability?source=recommendations&pivots=dotnet-7-0#non-public-property-accessors
    //Attributes
    //[JsonPropertyName("goalList")]
    //[JsonRequired]
    //[JsonInclude]
    private List<Goal> _goalList = new List<Goal>() ;
    //[JsonPropertyName("points")]
    //[JsonRequired]
    //[JsonInclude]
    private long _points = 0;
    //[JsonPropertyName("userName")]
    //[JsonRequired]
    //[JsonInclude]
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
    public GoalManager(List<Goal> goalList, long points, string userName = "My")
    {
        _goalList = goalList.ToList<Goal>();//Copy the list to break the reference to the original
        _points = points;
        _userName = userName;
    }
    //Generate from file path
    public GoalManager(string filePath)
    {
        Load(filePath); //Use existing functionality to load the goal manager to the attributes
    }
    //Copy from an existing goal manager
    public GoalManager(GoalManager newGoalManager)
    {
        //Update this goal list using the values of the GoalManager argument
        _goalList = newGoalManager.GetGoalList();
        _points = newGoalManager.GetPoints();
        _userName = newGoalManager.GetUserName();
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
    //Copy the values of another goal manager to this goal manager
    public void SetGoalManager(GoalManager newGoalManager)
    {
        _goalList = newGoalManager.GetGoalList();
        _points = newGoalManager.GetPoints();
        _userName = newGoalManager.GetUserName();
    }

    //Methods
    //Main Display (for goals)
    public void Display()
    {
        Console.WriteLine($"{_userName} goals are:");
        for(int i = 0; i < _goalList.Count; i++)
        {
            Console.WriteLine(_goalList[i].ToDisplayString(i));
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
        //Add to list actions are stored in lambda functions, to be called by the Action that the menu will use with each item
        List<Action> addToListActions = new List<Action>
        {
            new Action(()=>{_goalList.Add(new SimpleGoal(true));}), //Remember, having true in the constructor will trigger the user input version!
            new Action(()=>{_goalList.Add(new EternalGoal(true));}),
            new Action(()=>{_goalList.Add(new ChecklistGoal(true));})
        };
        //Create a UiMenu that will add a new Child of the Goal class to the Goal list
        UiMenu addGoalMenu = new UiMenu(
            new List<object>(){0,1,2}, //Each of these numbers corresponds to an index in the addToListActions
            new Action<object>((pickedOptionIdx) => {addToListActions[(int)pickedOptionIdx].Invoke();}), //Directly use the result to call the appropriate _goalList.add() action
            new List<string>{"&Simple Goal","&Eternal Goal","&Checklist Goal"}, //Use the new UiOption constructor to auto generate the hotkeys using the index of "&"
            true, //Yes this menu can cancel and return to the previous menu
            "The types of Goals are:", //The menu message
            "Which type of goal would you like to create? " //The input prompt
            //All the other attributes should be set to default, which hides the exit/cancel message by default when this style of constructor is used
        );
        addGoalMenu.UiLoop(); //Start the add goal menu
        //The alternative is to use a default UiMenu, but every action needs a "throw new OperationCancelledError();" at the end
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
        //string fileName = GetInput("What is the filename for the goal file? "); //Original
        string fileName = GetInput("What is the filename you would like to save the goal file as? ");
        //Save(fileName); //Save the file by calling the actual save file method
        SaveBinaryFile(fileName); //Save the file in binary form
    }
    public void Load()
    {
        //string fileName = GetInput("What is the filename for the goal file? "); //Original
        string fileName = GetInput("What is the filename of the goal file you want to load? ");
        //Load(fileName); //Load the file by calling the actual load file method
        LoadBinaryFile(fileName);
    }

    //Goal Marking
    public void RecordEvent()
    {
        //Consider using UI Menu Here
        //Int input (index of goal in list), invalid to cancel
        Display(); //Display the goals before asking which one the user completed
        int goalIndex = GetIntInput("Which goal did you accomplish? ",1,_goalList.Count);
        int newPoints = _goalList[goalIndex-1].Mark(); //Mark the goal as completed
        if(newPoints != 0)
        {
            _points += newPoints;
            Console.WriteLine($"You now have {newPoints} points.");
        }
        else
        {
            Console.WriteLine("That goal has been completed already!");
        }
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
    //Binary Writing
    private void SaveBinaryFile(string filePath)
    {
        try
        {
            using (FileStream binaryStream = File.Open(filePath, FileMode.Create))
            {
                BinaryWriter binWriter = new BinaryWriter(binaryStream);//Initalize the binary writer, which is what can actually write bytes
                binWriter.Write(new char[]{'G','O','A','L'}); //Write a 4 byte header

                binWriter.Write(_goalList.Count); //Write the goal list count

                //Make a list to convert each custom type to a corresponding index
                List<Type> typesToIndex = new List<Type>{new SimpleGoal().GetType(),new EternalGoal().GetType(),new ChecklistGoal().GetType()}; 
                for(int i=0;i<_goalList.Count;i++) //Write every goal in binary form
                {
                    int goalType = typesToIndex.IndexOf(_goalList[i].GetType()); //Figure out what type the goal is and get it's index in teh typesToIndex list
                    binWriter.Write((byte)goalType); //Write a single byte representing the type of goal
                    // WriteString(_goalList[i].GetName(), binWriter);
                    // WriteString(_goalList[i].GetDesc(), binWriter);
                    //Write the fields of the goal
                    binWriter.Write(_goalList[i].GetName());
                    binWriter.Write(_goalList[i].GetDesc());
                    binWriter.Write(_goalList[i].GetValue());
                    binWriter.Write(_goalList[i].GetCompCount());
                    if(goalType == 2) //Goal type 2 (Checklist Goal) is the only type that has additional parameters
                    {
                        ChecklistGoal goalItem = (ChecklistGoal) _goalList[i]; //Store the current item as a checklist goal (because it is one!!!)
                        //Write it's fields to the file
                        binWriter.Write(goalItem.GetBonusCompGoal());
                        binWriter.Write(goalItem.GetBonusValue());
                    }
                }
                //Write the remaining attributes of GoalManager
                binWriter.Write(_points);
                binWriter.Write(_userName);
            }
        }
        catch(IOException e)
        {
            Console.WriteLine($"File writing failed, IO Error: {e.ToString()}");
        }
        catch(ArgumentException e)
        {
            Console.WriteLine($"File writing failed Argument Error: {e.ToString()}");
        }

    }

    private void LoadBinaryFile(string filePath)
    {
        using(FileStream binaryStream = File.OpenRead(filePath))
        {
            BinaryReader binReader = new BinaryReader(binaryStream); //Turn the binary stream into a binary reader so we can use it

            //Read the header to make sure we are reading a file with the correct format
            char[] headerByte = binReader.ReadChars(4);
            if(string.Concat(headerByte) != "GOAL")
            {
                Console.WriteLine("Error! The GOAL file header is missing, this is not a valid file!");
                return; //Return early
            }

            //Start reading the data

            //Read the goal list goal entries
            int goalListCount = binReader.ReadInt32(); //Get the number of goals in the goal list
            List<Goal> newGoalList = new List<Goal>(); //Initalize a blank goal list
            for (int i = 0; i < goalListCount; i++)
            {
                int goalType = (int)binReader.ReadByte(); //Identify the goal type of this entry
                //All goals need these attributes read from the file
                string name = binReader.ReadString(); //Name
                string desc = binReader.ReadString(); //Description
                int value = binReader.ReadInt32(); //Value
                int compCount = binReader.ReadInt32(); //Completion count
                //Add the goal based on the type read from the goalType byte
                switch(goalType)
                {
                    case(0): //Simple Goal
                        newGoalList.Add(new SimpleGoal(name, desc, value, compCount));
                        break;
                    case(1): //Eternal Goal
                        newGoalList.Add(new EternalGoal(name, desc, value, compCount));
                        break;
                    case(2): //Checklist goal
                        //Read additional attributes
                        int bonusCompGoal = binReader.ReadInt32(); //Bonus Completion Goal
                        int bonusValue = binReader.ReadInt32(); //Bonus Value
                        newGoalList.Add(new ChecklistGoal(name, desc, value, bonusCompGoal, bonusValue, compCount));
                        break;
                    default: //Invalid goal type
                        //Do nothing
                        Console.WriteLine($"Error! Invalid goal type: {goalType}"); //Okay, fine, alert the user!
                        break;
                }
            }

            //Read all additional GoalManager parameters
            long points = binReader.ReadInt64(); //Points, it's a long, so it's 64 bytes long!!!
            string userName = binReader.ReadString(); //User name

            //Now finally update the properties of this GoalManager
            _goalList = newGoalList.ToList<Goal>();
            _points = points;
            _userName = userName;
        }
    }

    //Method to quickly write a string and its string length
    //Obsolete because binary Writers write the string length by default
    /*private void WriteString(string inputStr, BinaryWriter binWriter)
    {
        binWriter.Write((short)inputStr.Length); //Always write string length as a short, it takes less space
        binWriter.Write(inputStr);
    } */
    /*private string ReadString()
    {
        string returnStr = "";
        return returnStr;
    }*/
}