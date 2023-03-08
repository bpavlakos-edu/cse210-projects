using System.Text.Json; //Thanks Json
using Inp = QuickUtils.Inputs; //Custom library import for user inputs
using UiMenu = QuickUtils.UiMenu; //Custom library import for UI menu
using UiOption = QuickUtils.UiOption; //Custom library import for UI menu option

class Goal
{
    //Attributes
    protected string _name; //The display name of the goal
    protected string _desc; //The short description
    protected int _value; //The point value of the goal
    protected int _compCount; //The completion counter for this goal, used to determine if the goal is completed or not
    //Constructors
    //Blank Constructor
    public Goal()
    {
        
    }
    //Constructor that uses user input to fill the attributes
    /*
    I have this trigger boolean to serve no other purpose than to trigger this constructor, without losing the ability to have a blank constructor
    This constructor could trigger unique 0 parameter constructors with the following changes:
    1. Change trigger bool to int constructorId
    2. Use constructorId in a Switch-Case statement, to call void-type methods that change the value of the attributes just like any other constructor does
    But since there's really only one, I decided to keep it simple by having a boolean input be the trigger for this constructor
    */
    public Goal(bool triggerBool)
    {
        _name = Inp.GetInput("What is the name of your goal? ", false);
        _desc = Inp.GetInput("What is the short description of it? ", false);
        _value = Inp.GetIntInput("What is the amount of points associated with this goal? ");
        _compCount = 0; //It will always be 0 by defualt
    }
    //Fill all attributes
    public Goal(string name, string desc, int value, int compCount = 0)
    {
        _name = name;
        _desc = desc;
        _value = value;
        _compCount = compCount;
    }
    //Fill all attributes using a list of JSON Elements
    public Goal(List<JsonElement> dataList, int offset)
    {
        _name = dataList[offset].GetString();
        _desc = dataList[offset + 1].GetString();
        _value = dataList[offset + 2].GetInt32();
        _compCount = dataList[offset + 3].GetInt32();
    }
    //Fill all attributes from a binary reader (This should never be activated on it's own!!!)
    public Goal(BinaryReader binReader)
    {
        _name = binReader.ReadString();
        _desc = binReader.ReadString();
        _value = binReader.ReadInt32();
        _compCount = binReader.ReadInt32();
    }

    //Getters and Setters
    public string GetName()
    {
        return _name;
    }
    public void SetName(string name)
    {
        _name = name;
    }
    public string GetDesc()
    {
        return _desc;
    }
    public void SetDesc(string desc)
    {
        _desc = desc;
    }
    public int GetValue()
    {
        return _value;
    }
    public void SetValue(int value)
    {
        _value = value;
    }
    public int GetCompCount()
    {
        return _compCount;
    }
    public void SetCompCount(int compCount)
    {
        _compCount = compCount;
    }

    //Methods
    //Main Functionality
    //Return the display string 
    public virtual string ToDisplayString()
    {
        return $"[{GetMarkChar()}] {_name} ({_desc}) [{_value} point reward]"; //Example: "[*] My goal Name (Description)"
    }
    //Automatically append the number in a for loop, to the start of the display string, even if it's overridden by a sub class
    public string ToDisplayString(int index)
    {
        return $"{index+1}. "+ToDisplayString(); //Example: "1. [*] My goal Name (Description)"
    }
    //Mark a goal as finished
    public virtual int Mark()
    {
        _compCount++; //Increment the completion count
        Console.WriteLine($"Congratulations! You have earned {_value} points!");
        return _value; //Return the points the user gained
    }

    //Utility
    protected string GetMarkChar() //The originally planned name was "GetMark"
    {
        if(IsCompleted())
        {
            return "X";
        }
        else
        {
            return " ";
        }
    }
    //Evaluate if this goal has been completed or not
    //Will be overridden by sub-classes to control completion status
    protected virtual bool IsCompleted() //The originally planned name was "GetStatus"
    {
        return _compCount > 0; //By default the completion status will be if _compCount > 0
    }
    //Utility
    //Convert to List<Object>
    public virtual List<object> ToObjectList()
    {
        return new List<object>(){_name, _desc, _value, _compCount};
    }
    //This method should never be run by itself, only overidden by a child class
    public virtual void WriteGoalHex(BinaryWriter binWriter)
    {
        binWriter.Write(_name);
        binWriter.Write(_desc);
        binWriter.Write(_value);
        binWriter.Write(_compCount);
    }
    //Edit the fields in this goal
    public void Edit()
    {
        MakeEditMenu().UiLoop(debugMode:true);
    }
    //Programically generate the UiMenu for editing the attributes of this goal, it can be overidden because the checklist goal has extra settings to change
    public virtual UiMenu MakeEditMenu()
    {
        //Instantrly return the generated Ui Menu
        return new UiMenu(new List<UiOption>()
            {
                //All UiOptions here throw OperationCancelledExceptions to exit the mnu
                new UiOption(new Action(()=>{_name = Inp.GetInput($"What would you like to change the goal name to? (Currently: {_name})"); throw new OperationCanceledException();}),"Goal &Name"),
                new UiOption(new Action(()=>{_desc = Inp.GetInput($"What would you like to change the short description to? (Currently: {_desc})"); throw new OperationCanceledException();}),"Goal &Description"),
                new UiOption(new Action(()=>{_value = Inp.GetIntInput($"What would you like to change the point value to? (Currently: {_value})"); throw new OperationCanceledException();}),"Point &Value"),
                new UiOption(new Action(()=>{_compCount = Inp.GetIntInputMin($"What would you like to change the completion count to? (Currently: {_compCount})",0); throw new OperationCanceledException();}),"Comple&tion Count"),
                new UiOption(new Action(()=>{throw new OperationCanceledException();}),"Go &Back") //Exit
            },
            "Goal Settings:",
            "Select a goal setting or [hotkey] from the menu: ",
            "" //Hide the exit message
            //The rest of the settings can be the default
        );
    }

}