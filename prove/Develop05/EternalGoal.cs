using System.Text.Json; //Thanks Json
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
    public EternalGoal(List<JsonElement> dataList, int offset) : base(dataList, offset)
    {
        //Empty because the base class handles filling all attributes
    }
    public EternalGoal(BinaryReader binReader) : base(binReader)
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
    //Utility Overrides
    //To List<object>
    public override List<object> ToObjectList()
    {
        List<object> returnList = base.ToObjectList(); //Get the inital list from the base constructor
        returnList.Insert(0, "EternalGoal"); //Insert the Goal List type identifier
        return returnList; //Return the return list
    }
    //Write to a binary writer
    public override void WriteGoalHex(BinaryWriter binWriter)
    {
        binWriter.Write((byte) 1); //Write this goal's type identifier as a byte
        base.WriteGoalHex(binWriter); //Use the base class function to write the rest of the data
    }
}