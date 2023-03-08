/*Attempted common library:
1. Attempted putting functions in a namespace
2. Attempted putting functions in a namespace class
3. Attempted changing the type of the class to static in the "using" statement
4. Attempted to do "using varName = butil.inputs" and for some reason it didn't work
5. Attempted to set the class itself to static after seeing the following docs: 
https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/namespace
https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/namespaces
https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/namespaces
6. Found Info on how to use "using" properly, and why C# makes librarys harder: https://stackoverflow.com/a/11893468 
7. Tried step 4 again and it worked... I must have mistyped something!*/

//To add this to a project: "using inp = QuickUtils.Inputs"
//To use it: "inp.GetInput()"

namespace QuickUtils /*Library name*/
{
    //User Inputs
    public static class Inputs /*Library static class*/
    {
        //Basic string input
        public static string GetInput(string inMsg, bool toLower = true, bool newLine = false)
        {
            //Flag for newline support (off by default)
            if(newLine)
            {
                inMsg += Environment.NewLine;
            }
            Console.Write(inMsg);
            string returnStr = Console.ReadLine()??"";//Return the read line or nothing if it fails
            //Flag for lowercase support (on by default)
            if(toLower)
            {
                return returnStr.ToLower();
            }
            return returnStr; //Return the return string
        }
        public static int GetIntInput(string inMsg, int[] bounds, bool newLine = false)
        {   
            while(true) //Infinite loop
            {
                try
                {
                    try
                    {
                        int returnVal = int.Parse(GetInput(inMsg,newLine)); //Parse the input
                        //Determine if the value is in our boundaries
                        if(bounds.Length == 0)
                        {
                            return returnVal;
                        }
                        else if(bounds.Length == 1 && returnVal <= bounds[1]) //Max Only
                        {
                            return returnVal;
                        }
                        else if(bounds.Length == 2 && (bounds[0] == bounds[1]) && (returnVal >= bounds[0])) //Minimum only (when bounds are equal)
                        {
                            return returnVal;
                        } 
                        else if(bounds.Length == 2 && (returnVal >= bounds[0] && returnVal <= bounds[1])) //Within minimum and maximum
                        {
                            return returnVal;
                        }
                        else //Not within range
                        {
                            if(bounds.Length > 0)
                            {
                                //Todo: Display min max if it's enabled
                                Console.WriteLine("Sorry that's not a valid number, please try again!");
                            }
                            else //
                            {
                                throw new ArgumentNullException();
                            }
                        } 
                    }
                    /*Funnel all invalid entries to ArgumentNullException*/
                    catch(FormatException){throw new ArgumentNullException();}
                    catch(OverflowException){throw new ArgumentNullException();}
                }
                catch(ArgumentNullException)
                {
                    Console.WriteLine("Sorry! That's not a valid number, please try again!");
                }
            }
        }
        //Integer Input without a minimum or maximum
        public static int GetIntInput(string inMsg, bool newLine = false)
        {
            return GetIntInput(inMsg, new int[]{},newLine);
        }
        //Integer input with a maximum
        public static int GetIntInputMax(string inMsg, int max, bool newLine = false)
        {
            return GetIntInput(inMsg, new int[]{max},newLine);
        }
        //Input with a minimum
        public static int GetIntInputMin(string inMsg, int min, bool newLine = false)
        {
            return GetIntInput(inMsg, new int[]{min,min},newLine);
        }
        //Int input with minimum and maximum as integers and not an array
        public static int GetIntInput(string inMsg, int min, int max, bool newLine = false)
        {
            return GetIntInput(inMsg, new int[]{min,max}, newLine);
        }
        //Int input, but forbid a specific number
        public static int GetIntInputForbid(string inMsg, int forbidNum, bool newLine = false)
        {
            while(true)
            {
                int returnNum = GetIntInput(inMsg);
                if(returnNum != forbidNum)
                {
                    return returnNum;
                }
                else
                {
                    Console.WriteLine("Sorry! That's not a valid number, please try again!");
                }
            }
        }

        //Float input

        //List input

        //File Input?

        //DateTime input?
    }
}



