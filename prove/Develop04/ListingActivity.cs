class ListingActivity : Activity
{
    //Constructors
    public ListingActivity() : base()
    {

    }
    //Fill all attributes
    public ListingActivity(string name, string description, List<string> messageList, int pauseStyle) : base(name,description, messageList, pauseStyle)
    {
        //ToList is handled by the base class constructor
    }
}