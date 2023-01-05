using System;

class Program
{
    static void Main(string[] args)
    {
        float grade = getFloat("What is your grade percentage? ");

        string letter = "Null";
        bool passed = false;
        if (grade >= 90)
        {
            letter = "A";
            passed = true;
        }
        else if (grade >= 80)
        {
            letter = "B";
            passed = true;
        }
        else if (grade >= 70)
        {
            letter = "C";
            passed = true;
        }
        else if (grade >= 60)
        {
            letter = "D";
        }
        else
        {
            letter = "F";
        }

        //Stretch Challenges 1 to 3
        string sign = "";
        if(letter != "A" && letter != "F")
        {
            float lastDigit = grade % 10;
            if (lastDigit >= 7)
            {
                sign = "+";
            }
            else if (lastDigit < 3)
            {
                sign = "-";
            }
        }

        Console.WriteLine($"Your Grade: {letter}{sign}");
        if (passed)
        {
            Console.WriteLine("Congratulations you passed!");
        }
        else
        {
            Console.WriteLine("You did not pass, study more and try again!");
        }
    }
    static float getFloat(string inMsg){
        while(1 == 1) //The only way to escape this function is to give a valid number
        {
            Console.Write(inMsg);
            string consoleStr = Console.ReadLine();
            try
            {
                return float.Parse(consoleStr);
            }
            catch
            {
                Console.WriteLine("Sorry, that is not a valid number, please try again!");
            }
        }
    }
}