//Shape Abstract Super-Class
//abstract class Shape
class Shape
{
    private string _color;

    public string GetColor()
    {
        return _color;
    }
    public void SetColor(string color)
    {
        _color = color;
    }

    //public abstract double GetArea();
    public virtual double GetArea()
    {
        
    }
}