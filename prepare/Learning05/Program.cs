//Activity Instructions: https://byui-cse.github.io/cse210-course-2023/unit05/prepare.html
using System;

class Program
{
    static void Main(string[] args)
    {
        //Step 5: Test the square class
        //Square squareTest = new Square("Red",5.5);
        /*
        Console.WriteLine($"Area of a 5 by 5 square: {squareTest.GetArea()}");
        squareTest.SetSide(3.5);
        Console.WriteLine($"Area of a 3.5 by 3.5 square: {squareTest.GetArea()}");
        Console.WriteLine("");
        */
        //Step 7: Test Rectangle and Circle using a list of shape

        List<Shape> shapeList = new List<Shape>{new Square("Red",5.5), new Rectangle("Blue",2.5,11), new Circle("Green",5)};
        //foreach(Shape curShape in shapeList){};//For each loop
        //For loop
        for(int i = 0; i < shapeList.Count; i++)
        {
            Console.WriteLine($"Shape {i+1}, Color: {shapeList[i].GetColor()} Area: {shapeList[i].GetArea()}");
        }
    }
}