using System;

class Program
{
    static void Main(string[] args)
    {
        //Step 4-2
        Fraction fracDefault = new Fraction();
        Fraction fracWholeNum = new Fraction(6);
        Fraction fracFullParams = new Fraction(6/7);
        //Step 5
        Console.WriteLine($"Fraction Defualt: {fracDefault.GetTop()}/{fracDefault.GetBottom()}");
        Console.WriteLine($"Fraction Whole Number: {fracDefault.GetTop()}/{fracDefault.GetBottom()}");
        Console.WriteLine($"Fraction Full Params: {fracDefault.GetTop()}/{fracDefault.GetBottom()}");
        fracDefault.SetBottom(3);
        fracDefault.SetBottom(10);
        Console.WriteLine($"Fraction {fracDefault.GetTop()}/{fracDefault.GetBottom()}");

        //step 6
        Fraction threeFourth = new Fraction(3/4);
        Console.WriteLine(threeFourth.GetFractionString());
        Console.WriteLine(threeFourth.GetDecimalValue());


        //Step 7
        Fraction testFrac1 = new Fraction();
        Fraction testFrac2 = new Fraction(5);
        Fraction testFrac3 = new Fraction(3,4);
        Fraction testFrac4 = new Fraction(1,3);
        Console.WriteLine($"{testFrac1.GetDecimalValue()}");
        Console.WriteLine($"{testFrac1.GetFractionString()}");
         Console.WriteLine($"{testFrac2.GetDecimalValue()}");
        Console.WriteLine($"{testFrac2.GetFractionString()}");
 Console.WriteLine($"{testFrac3.GetDecimalValue()}");
        Console.WriteLine($"{testFrac3.GetFractionString()}");
 Console.WriteLine($"{testFrac4.GetDecimalValue()}");
        Console.WriteLine($"{testFrac4.GetFractionString()}");


    }
}