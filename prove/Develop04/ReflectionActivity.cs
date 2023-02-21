class ReflectionActivity : Activity
{
    //New Attributes
    List<string> _refQuestions;

    //Constructor
    public ReflectionActivity():base()
    {

    }
    //Fill All Attributes
    public ReflectionActivity(string name, string description, List<string>messageList, int pauseStyle, List<string> refQuestions):base(name, description, messageList, pauseStyle)
    {
        _refQuestions = refQuestions.ToList<string>(); //Use toList to erase the reference to the original list
    }

    //Getters and setters
    public List<string> GetRefQuestions()
    {
        return _refQuestions.ToList<string>(); //Use toList to erase the reference to the original list
    }
    public void SetRefQuestions(List<string> refQuestions)
    {
        _refQuestions = refQuestions.ToList<string>(); //Use toList to erase the reference to the original list
    }

}