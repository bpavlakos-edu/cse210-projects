//Project Requirements: https://byui-cse.github.io/cse210-course-2023/unit01/csharp-3.html
using System;

class Program
{
    static void Main(string[] args)
    {
        //"Play again?" loop
        do
        {
            //Game setup
            int magicNumber = 0;//The magic number is not nullable

            bool getRandom = true; //Flag for telling the program to pick a random number, later this could be used to allow the user to type "random" to choose a random number instead of typing it in
            if (getRandom)
            {
                int[] randomBounds = {1,100};
                //I forgot that C# and Java random need +1 to pick the desired upper number, until I looked at the solution: https://github.com/byui-cse/cse210-student-sample-solutions/blob/main/csharp-prep/Prep3/Program.cs
                magicNumber = (new Random()).Next(randomBounds[0],randomBounds[1]+1); //Create a new random object and generate the next integer from 1 to 100
                //magicNumber = (new Random()).Next(); //Are you crazy??? Good luck guessing a number from 0 to the integer ceiling!
                Console.WriteLine($"Randomly chose a number from {randomBounds[0]} to {randomBounds[1]}, good luck!"); //Let the user know the range
            }
            else
            {
                magicNumber = getInt("What is the magic number? "); //Manual User Input
            }
            Console.WriteLine("");//Blank Line

            //Gameplay loop
            //I originally was planning to use a do-while loop (which I've never used before but wanted to try in this project), but I've missed the C#/Java style for-loop so much during python that I'm going to force it to work for this situation

            //V1 Combined guessCount with the user guess so both are contained in the for loop (which is not a good idea now that I thought about it's impact on using the value)
            //for (int[] guessData = {0, magicNumber + 1}; guessData[1] != magicNumber; guessData[0] = guessData[0] + 1) 

            //V2: Initalize the user guess counter outside the for loop
            int userGuess = magicNumber + 1; //Initalize it to the magic number + 1 so it's always wrong when initalized
            for(int guessCount = 0; userGuess != magicNumber; guessCount++)
            {
                userGuess = getInt("What is your guess? ");
                Console.Write($"Guess {guessCount + 1}: ");
                if(userGuess == magicNumber)
                {
                    //Correct number
                    Console.WriteLine("You guessed it!");
                    Console.WriteLine($"Total Guesses: {guessCount + 1}");
                    Console.WriteLine("");//Extra padding
                }
                else if (userGuess < magicNumber)
                {
                    Console.WriteLine("Higher");
                }
                else if (userGuess > magicNumber)
                {
                    Console.WriteLine("Lower");
                }
                else //Impossible condition
                {
                    Console.WriteLine("How did you get here? Impressive!");
                }
            }
        } while (getInput("Would you like to keep playing? (enter \"Yes\" to continue) ").ToLower() == "yes"); //Keep looping until the user wants to exit
        Console.WriteLine("Thank you for playing!");
        //Console.Beep(); //Does this actually work???
        //Yes, in a matter of fact, it does
    }

    //Functions for getting user input and parsing them safely
    static int getInt(string promptMsg = "Please enter a whole number: "){
        while (1 == 1)
        {
            try
            {
                //int.TryParse might be an interesting alternative to try catch
                return int.Parse(getInput(promptMsg));
            }
            catch
            {
                Console.WriteLine("Sorry that's not a valid whole number, please try again!");
            }
        }

    }
    static string getInput(string promptMsg)
    {
        Console.Write(promptMsg);
        return Console.ReadLine();
    }
}