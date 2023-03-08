using System.Text.Json; //Thanks Json
class SimpleGoal : Goal
{
    //Attributes: 
    //All attributes are inherited from the base class

    //Constructors:
    //Blank constructor
    public SimpleGoal() : base()
    {

    }
    //User input (the purpose of the bool is to tell the compiler we want to use this constructor, without losing the ability to use the blank constructor)
    public SimpleGoal(bool triggerBool) : base(triggerBool)
    {
        //Empty because the base class handles all input
    }
    //Fill all attributes
    public SimpleGoal(string name, string desc, int value, int compCount = 0) : base(name, desc, value, compCount)
    {
        //Empty because the base class handles filling all attributes
    }
    public SimpleGoal(List<JsonElement> dataList, int offset) : base(dataList, offset)
    {
        //Empty because the base class handles filling all attributes
    }
    public SimpleGoal(BinaryReader binReader) : base(binReader)
    {
        //Empty because the base class handles filling all attributes
    }

    //Getters and Setters: 
    //All getters and setters are inherited from the base class

    //Unique Methods (the rest are inherited from "Goal"):
    //Override Mark() to make it only return points when it hasn't been completed yet
    public override int Mark()
    {
        if(!IsCompleted()) //Only mark it if it's not completed
        {
            return base.Mark(); //Use the base class "Mark()" function to increment compCount, and return (and display) the points we've gained
        }
        else //If it is completeted
        {
            return 0; //Return 0 points, because nothing happened
        }
    }
    //Utility Overrides
    //To List<object>
    public override List<object> ToObjectList()
    {
        List<object> returnList = base.ToObjectList(); //Get the inital list from the base constructor
        returnList.Insert(0, "SimpleGoal"); //Insert the Goal List type identifier
        return returnList; //Return the return list
    }
    //Write to a binary writer
    public override void WriteGoalHex(BinaryWriter binWriter)
    {
        binWriter.Write((byte) 0); //Write this goal's type identifier as a byte
        base.WriteGoalHex(binWriter); //Use the base class function to write the rest of the data
    }
}