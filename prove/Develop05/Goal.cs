//For Mark()
//$"Congratulations! You have earned {newPoints} points!"
//$"You now have {newPoints} points."

//For ToDisplayString
//$"{i+1} [GetMark()] {_name} ({_desc})" 

//For initalization
//"What is the name of your goal? "
//"What is the short description of it?"
//"What is the amount of points associated with this goal?"

class Goal
{
    //Attributes

    //Constructors
    public Goal()
    {

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