class Fraction
{
    private int _top;
    private int _bottom;
    //How to save 18 Lines of code (replaces get, set, and empty constructor contents)
    /* public int _top {get; set;} = 1;
    public int _bottom {get; set;} = 1; */

    //Constructors
    public Fraction()
    {
        _top = 1;
        _bottom = 1;
    }
    public Fraction(int wholeNumber)
    {
        _top = wholeNumber;
        _bottom = 1;
    }
    public Fraction(int top, int bottom)
    {
        _top = top;
        _bottom = bottom;
    }

    //Getters and setters
    //Top
    public int GetTop()
    {
        return _top;
    }
    public void SetTop(int top)
    {
        _top = top;
    }

    //Bottom
    public int GetBottom()
    {
        return _bottom;
    }
    public void SetBottom(int bottom)
    {
        _bottom = bottom;
    }
    

    //Methods
    public string GetFractionString()
    {
        return $"{_top}/{_bottom}";
    }
    public double GetDecimalValue()
    {
        return ((double) _top) / ((double) _bottom);
    }
    //Extra

}