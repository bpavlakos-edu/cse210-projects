class EternalGoal : Goal
{
    //Attributes: 
    //All attributes are inherited from the base class

    //Constructors:
    //Blank Constructor
    public EternalGoal() : base()
    {

    }
    //Get from user input
    public EternalGoal(bool triggerBool) : base(triggerBool)
    {
        //The contents of this constructor are handled by the parent class "Goal"
    }
    //Fill all attributes
    public EternalGoal(string name, string desc, int value, int compCount = 0) : base(name,desc,value,compCount)
    {
        //The contents of this constructor are handled by the parent class "Goal"
    }
    public EternalGoal(List<object> dataList, int offset) : base(dataList, offset)
    {
        //Empty because the base class handles filling all attributes
    }

    //Getters and Setters: 
    //All getters and setters are inherited from the base class

    //Methods:
    public override int Mark()
    {
        _compCount--; //Always subtract 1 to prevent _compCount from increasing, ensuring this goal is never "completed"
        return base.Mark(); //Use the existing code from the base class to return the value of this goal, and print the completion message
    }
}