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
        _refQuestions = refQuestions.ToList<string>();
    }

}