class GmClassic : GameMode
{
    //No class specific attributes
    //Constructors
    //Blank Constructor
    public GmClassic() : base()
    {
        _desc = "Classic Mode:_Rolls all dice and starts a timer_Write down as many words as you can find_The player with the most unique words wins!";
        //All other attributes are filled by the basic constructor
    }
    //Fill Attributes Constructor
    public GmClassic(int durationSec, bool? showCDown = null, string desc = "Classic Mode:_Rolls all dice and starts a timer_Write down as many words as you can find_The player with the most unique words wins!") : base(durationSec, showCDown, desc)
    {
        //All attributes are filled by the basic constructor
    }
}