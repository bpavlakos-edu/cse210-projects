using Inp = QuickUtils.Inputs; //Import the custom library's input class

class ChecklistGoal : Goal
{
    //Attributes
    private int _bonusCompGoal = 1; //The number the user has to reach to get the bonus
    private int _bonusValue = 0; //The bonus amount
    //Constructors
    public ChecklistGoal() : base()
    {

    }
    public ChecklistGoal(bool triggerBool) : base(triggerBool)
    {
        //For input initalization
        //"How many times does this goal need to be accomplished for a bonus? "
        //"What is the bonus for accomplishing it that many times? "
        _bonusCompGoal = Inp.GetIntInputMin("How many times does this goal need to be accomplished for a bonus? ", 1);
        _bonusValue = Inp.GetIntInput("What is the bonus for accomplishing it that many times? ");
    }
    public ChecklistGoal(string name, string desc, int value, int bonusCompGoal, int bonusValue, int compCount = 0) : base(name, desc, value, compCount)
    {
        _bonusCompGoal = bonusValue;
        _compCount = compCount;
    }
    public ChecklistGoal(List<object> dataList, int offset) : base(dataList, offset)
    {
        _bonusCompGoal = (int)dataList[offset+4];
        _bonusCompGoal = (int)dataList[offset+5];
    }
    //Getters and setters
    public int GetBonusCompGoal()
    {
        return _bonusCompGoal;
    }
    public void SetBonusCompGoal(int bonusCompGoal)
    {
        _bonusCompGoal = bonusCompGoal;
    }
    public int GetBonusValue()
    {
        return _bonusValue;
    }
    public void SetBonusValue(int bonusValue)
    {
        _bonusValue = bonusValue;
    }
    //Methods
    //Override for ToDisplayString, use the original ToDisplayString to generate the first half, then tells the user how many times they've completed the goal, and how many times they need to reach the bonus
    public override string ToDisplayString()
    {
        return $"{base.ToDisplayString()} -- Currently completed: {_compCount}/{_bonusCompGoal}"; //Example: [X] Name (Description text) -- Currently Completed: 1/3
    }
    //Override for Mark, gives a reward when the completion count will equal the bonus completion count goal
    public override int Mark()
    {
        if(!IsCompleted())
        {
            if(_compCount + 1 == _bonusCompGoal) //We will reach our goal, add the bonus points!
            {
                int tempValue = _value; //Store the original value temporarily
                _value += _bonusValue; //Add the bonus value to the value, so that is automatically is displayed in the base.Mark() class
                int returnVal = base.Mark(); //Get the new points
                _value = tempValue; //Restore _value's original value
                return returnVal; //return the result of the basic mark class
            }
            else //We completed it, but we won't get the bonus this time
            {
                return base.Mark(); //Just use the default Mark functionality
            }
        }
        else //It's already completed
        {
            return 0; //Return 0
        }
        
    }
    //Override For IsCompleted(), only returns true when the completion count has met or exceeded the completion count goal
    protected override bool IsCompleted()
    {
        return _compCount >= _bonusCompGoal; //The completion criteria for this is if _compCount == _bonusCompGoal
    }
    //Utility Overrides
    //To List<object>
    public override List<object> ToObjectList()
    {
        List<object> returnList = base.ToObjectList(); //Get the inital list from the base constructor
        returnList.Insert(0, 2); //Insert the Goal List type identifier
        returnList.Add(_bonusCompGoal);
        returnList.Add(_bonusValue);
        return returnList; //Return the return list
    }
}