using Inp = QuickUtils.Inputs; //Custom library import for user inputs

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
        _name = Inp.GetInput("What is the name of your goal? ");
        _desc = Inp.GetInput("What is the short description of it? ");
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
        return $"[{GetMarkChar()}] {_name} ({_desc})"; //Example: "[*] My goal Name (Description)"
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
}