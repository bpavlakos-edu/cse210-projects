//For Mark()
//$"Congratulations! You have earned {newPoints} points!"
//$"You now have {newPoints} points."

//For ToDisplayString
//$"{i+1} [GetMark()] {_name} ({_desc})" 

//For initalization


using inp = QuickUtils.Inputs;

class Goal
{
    //Attributes
    protected string _name;
    protected string _desc;
    protected int _value;
    protected int _compCount;
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
        _name = inp.GetInput("What is the name of your goal? ");
        _desc = inp.GetInput("What is the short description of it? ");
        _value = inp.GetIntInput("What is the amount of points associated with this goal? ");
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

    //Methods

    public virtual string ToDisplayString()
    {
        return ""; //Placeholder
    }
    //Actual method that's not overridden
    //This is so we can automatically append "{index+1}. " to each sub classes ToDisplayString automatically!
    public string ToDisplayString(int index)
    {
        return $"{index+1}. "+ToDisplayString();
    }
    
}