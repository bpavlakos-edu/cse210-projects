//For display string
//$"{i+1} [GetMark()] {_name} ({_desc}) -- Currently completed: {_compCount}/{_maxCompCount}" 

//For input initalization
//"How many times does this goal need to be accomplished for a bonus? "
//"What is the bonus for accomplishing it that many times? "

using Inp = QuickUtils.Inputs;

class ChecklistGoal : Goal
{
    //Attributes
    private int _bonusCompGoal = 1; //The number the user has to reach to get the bonus
    private int _bonusValue = 0;
    //Constructors
    public ChecklistGoal() : base()
    {

    }
    public ChecklistGoal(bool triggerBool) : base(triggerBool)
    {
        _bonusCompGoal = Inp.GetIntInputMin("How many times does this goal need to be accomplished for a bonus? ", 1);
        _bonusValue = Inp.GetIntInput("What is the bonus for accomplishing it that many times? ");
    }
    public ChecklistGoal(string name, string desc, int value, int bonusCompGoal, int bonusValue, int compCount = 0) : base(name, desc, value, compCount)
    {
        _bonusCompGoal = bonusValue;
        _compCount = compCount;
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
    //Override
    //Override
    protected override bool IsCompleted()
    {
        return _compCount == _bonusCompGoal; //The completion criteria for this is if _compCount == _bonusCompGoal
    }
}