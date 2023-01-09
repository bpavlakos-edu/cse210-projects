//Requirements: https://byui-cse.github.io/cse210-course-2023/unit01/csharp-4.html

using System;

class Program
{
    static void Main(string[] args)
    {
        List<int> numList = new List<int>(); //List initalization
        Console.WriteLine("Enter a list of numbers, type 0 when finished.");
        int sum = 0; //Skip reduntant code usage by updating sum inside this loop
        while(1 == 1)
        {
            int newNumber = getInt("Enter Number: ");
            if(newNumber == 0) //Exit condition met
            {
                break;
            }
            numList.Add(newNumber); //Add to the list
            sum += newNumber; //Update the sum
        }


        //Caclulate Minimum and maximum values
        if(numList.Count > 0)
        {
            Console.WriteLine("The sum is: "+sum); //Print the sum
            int max = numList[0];
            int min = numList[0]; //Not asked for
            int closestToZeroPositive = 0;
            int closestToZeroNegative = 0; //Not asked for
            //Run calculations
            for(int i = 0; i < numList.Count; i++)
            {
                //Avoided computation time of i == 0 
                //By initializing min and max to list item 0 before the loop starts
                //if(i == 0){max = curNum; min = curNum;}
                //Update minimum and maximum values
                int curNum = numList[i];
                if(curNum > max)
                {
                    max = curNum;
                }
                else if(curNum < min)
                {
                    min = curNum;
                }
                //Ignore equal values
                
                //Find positive value closest to zero (Stretch Challenge 1)
                if(curNum > 0) 
                { 
                    if(curNum < closestToZeroPositive || closestToZeroPositive == 0)
                    {
                        closestToZeroPositive = curNum;
                    }
                }
                else if(curNum < 0) //Closest to 0 negative value
                {
                    if(curNum > closestToZeroPositive || closestToZeroNegative == 0)
                    {
                        closestToZeroNegative = curNum;
                    }
                }
            }
            float average = ((float)sum)/((float)numList.Count); //Quickly calculate the average

            //Print results:
            //Required functionality
            Console.WriteLine($"The average is: {average}");
            Console.WriteLine("The largest number is: "+max);
            if(closestToZeroPositive != 0)
            {
                Console.WriteLine("The smallest positive number is: "+closestToZeroPositive);
            }

            //Extra functionality
            Console.WriteLine("The smallest number is: "+min);
            if(closestToZeroNegative != 0)
            {
                Console.WriteLine("The largest negative number is: "+closestToZeroNegative);
            }

            //Sort list (Stretch challenge 2)
            numList.Sort();
            Console.WriteLine("The sorted list is:");
            foreach (int num in numList)
            {
                Console.WriteLine(num);
            }
        }
        else
        {
            Console.WriteLine("The number list is empty, please fill the list next time!");
            Console.WriteLine("Now exiting");
        }
        
    }
    //number input
    static int getInt(string promptMsg = "Please enter a valid whole number: "){
        while(1 == 1)
        {
            Console.Write(promptMsg);
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("That's not a valid number, please try again!");
            }
        }

    }
}