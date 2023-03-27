/*
Version 3 Changes:
- Added curValue support to all input types
- Removed an overload to GetIntInput() by making bounds nullable
- Changed GetIntInput funnel to FormatException, so that CurValue, which is a nullable parameter, can optionally return the current value if the user input is blank
- Changed the GetIntInput base method to use two nullable integers instead of a collection
- Adjusted logic on GetIntInput to make it more streamlined
- Added GetBoolInput
- Updated large if statement blocks with ternary operators
*/

//To add this static class to a project: "using Inp = QuickUtils.Inputs"
//To use it: "Inp.GetInput()"

namespace QuickUtils /*Library name*/
{
    //User Inputs
    public static class Inputs /*Library static class*/
    {
        //Basic string input
        public static string GetInput(string inMsg, bool toLower = true, bool newLine = false, string curValue = null)
        {
            //Code size has decreased through the use of the ternary operator [ex: dataType myVar = (boolean condition) ? valueIfTrue : valueIfFalse;]
            inMsg = (curValue != null) ? inMsg.Replace(":",$" (currently: "+Misc.QuoteStr(curValue)+" leave blank to cancel):") : inMsg; //When we have a current value, display it
            inMsg = (newLine) ? inMsg + Environment.NewLine : inMsg; //Flag for newline support (off by default)
            Console.Write(inMsg);
            string returnStr = Console.ReadLine() ?? curValue ?? "";//Return the read line, if it's null, return a blank string (or curValue if not empty)
            //Flag for lowercase support (on by default)
            return (toLower) ? returnStr.ToLower() : returnStr; //Return the return string 
        }
        
        //Integer Input
        public static int GetIntInput(string inMsg, int? min = null, int? max = null, bool newLine = false, int? curValue = null)
        {   
            while(true) //Infinite loop
            {
                try //Main exception
                {
                    try //Exception funnel
                    {
                        int returnVal = int.Parse(GetInput(inMsg, true, newLine, (curValue != null) ? curValue + "":null)); //Parse the input (This ternary codition prevents null from being turned into a string, while also letting us turn int into a string)
                        //Determine if the value is in our boundaries
                        if(min == null && max == null) //No boundaries
                        {
                            return returnVal;
                        }
                        else if((returnVal >= (min ?? returnVal)) && (returnVal <= (max ?? returnVal))) //Check boundaries
                        {
                            //If the boundary is "null" it will automatically turn into "returnVal", which will make it's half true! 
                            //But since they are both null in the first if statement, that means one of these two is garunteed to be checked
                            return returnVal;
                        }
                        else //Not within range
                        {
                            //Use the ternary conditional operator to generate most of the strings, this saved us a few lines of code!
                            //See this site for info: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
                            //Syntax: type myVar = (condition) ? valueIftrue : valueIfFalse;
                            string minStr = (min != null) ? $"minimum {min}":"";
                            string maxStr = (max != null) ? $"maximum {max}":"";
                            string toStr = (min != null && max != null) ? " to ":"";
                            string inTheRange = (min != null || max != null) ? $" in the range: {minStr}{toStr}{maxStr}. P":", p";
                            Console.WriteLine($"Sorry that's not a valid number{inTheRange}lease try again!");
                        }
                    }
                    /*Funnel all invalid entries to FormatException*/
                    catch(ArgumentNullException) //Check argument null exceptions
                    {
                        if(curValue != null) //Return curValue when curValue isn't null
                        {
                            return (int) curValue; //Why do I need to type cast this?
                        }
                        throw new FormatException(); //If there's no current value, act like all the other exceptions
                    }
                    catch(OverflowException){throw new FormatException();}
                }
                catch(FormatException)
                {
                    Console.WriteLine("Sorry! That's not a valid number, please try again!");
                }
            }
        }
        //Integer input with a maximum
        public static int GetIntInputMax(string inMsg, int max, bool newLine = false, int? curValue = null)
        {
            return GetIntInput(inMsg, null, max, newLine, curValue);
        }
        //Input with a minimum
        public static int GetIntInputMin(string inMsg, int min, bool newLine = false, int? curValue = null)
        {
            return GetIntInput(inMsg, min, null, newLine, curValue);
        }
        //Int input with minimum and maximum as an array of integers
        public static int GetIntInput(string inMsg, int[] bounds, bool newLine = false, int? curValue = null)
        {
            return GetIntInput(inMsg, bounds[0], bounds[1], newLine, curValue);
        }
        //Int input, but forbid a specific number
        public static int GetIntInputForbid(string inMsg, int forbidNum, bool newLine = false, int? curValue = null)
        {
            while(true)
            {
                int returnNum = GetIntInput(inMsg, null, null, newLine, curValue);
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
        public static float GetFloatInput(string inMsg, float? min = null, float? max = null, bool newLine = false, float? curValue = null)
        {   
            while(true) //Infinite loop
            {
                try //Main exception
                {
                    try //Exception funnel
                    {
                        float returnVal = float.Parse(GetInput(inMsg, true, newLine, (curValue != null) ? curValue + "":null)); //Parse the input (This ternary codition prevents null from being turned into a string, while also letting us turn int into a string)
                        //Determine if the value is in our boundaries
                        if(min == null && max == null) //No boundaries
                        {
                            return returnVal;
                        }
                        else if((returnVal >= (min ?? returnVal)) && (returnVal <= (max ?? returnVal))) //Check boundaries
                        {
                            //If the boundary is "null" it will automatically turn into "returnVal", which will make it's half true! 
                            //But since they are both null in the first if statement, that means one of these two will need to be checked!
                            return returnVal;
                        }
                        else //Not within range
                        {
                            //Use the ternary conditional operator to generate most of the strings, this saved us a few lines of code!
                            //See this site for info: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
                            //Syntax: type myVar = (condition) ? valueIftrue : valueIfFalse;
                            string minStr = (min != null) ? $"minimum {min}":"";
                            string maxStr = (max != null) ? $"maximum {max}":"";
                            string toStr = (min != null && max != null) ? " to ":"";
                            string inTheRange = (min != null || max != null) ? $" in the range: {minStr}{toStr}{maxStr}. P":", p"; //Sorry that's not a valid decimal number in the range min to max, please try again!
                            Console.WriteLine($"Sorry that's not a valid decimal number{inTheRange}lease try again!");
                        }
                    }
                    /*Funnel all invalid entries to FormatException*/
                    catch(ArgumentNullException)
                    {
                        if(curValue != null) //Return curValue when curValue isn't null
                        {
                            return (float) curValue; //Why do I need to type cast this?
                        }
                        throw new FormatException(); //If there's no current value, act like all the other exceptions
                    }
                    catch(OverflowException){throw new FormatException();}
                }
                catch(FormatException)
                {
                    Console.WriteLine("Sorry! That's not a valid decimal number, please try again!");
                }
            }
        }

        //Boolean input
        public static bool GetBoolInput(string inMsg, char yesChar = 'y', char noChar = 'n', bool newLine = false, bool? curValue = null, bool hideCurValue = false)
        {

            while(true) //Infinite loop
            {
                string userInput = GetInput(inMsg.Replace(":",$" [enter {yesChar}/{noChar}]:"), true, newLine, (curValue != null && !hideCurValue) ? ((curValue == true) ? "Enabled" : "Disabled") : null); //Automatically append the  Y/N string (Again, (curVal != null) lets us make sure that the next test (curVal == True), which changes the curVal string to enabled or disabled in GetInput
                if(userInput == "" && curValue != null) //Current value isn't empty, and the user input was blank
                {
                    return (bool)curValue;
                }
                else if(userInput != "" && (userInput[0] == yesChar || userInput[0] == noChar))//User input isn't blank and it's one of our two checking conditions
                {
                    return userInput[0] == yesChar;//Return if it was the yes char or if it wasn't
                }
                else //Unrecognized input
                {
                    Console.WriteLine("Sorry! That's not a valid input, please try again!");
                }
            }
        }

        //List input

        //File Input?

        //DateTime input?

        //Automatic type detection (Should automatically pick based on return type or curValue)
        //String
        public static string GetSmartInput(string inMsg, bool toLower = true, bool newLine = false, string curValue = null)
        {
            return GetInput(inMsg, toLower, newLine, curValue);
        }
        //Int
        public static int GetSmartInput(string inMsg, int? min = null, int? max = null, bool newLine = false, int? curValue = null)
        {
            return GetIntInput(inMsg, min, max, newLine, curValue);
        }
        //Float
        public static float GetSmartInput(string inMsg, float? min = null, float? max = null, bool newLine = false, float? curValue = null)
        {
            return GetFloatInput(inMsg, min, max, newLine, curValue);
        }
        //Bool
        public static bool GetSmartInput(string inMsg, char yesChar = 'y', char noChar = 'n', bool newLine = false, bool? curValue = null)
        {
            return GetBoolInput(inMsg, yesChar, noChar, newLine, curValue);
        }

        public static dynamic GetSettingInput(string inMsg, dynamic curValue, bool toLower = true, bool newLine = false,  int? min = null, int? max = null, char yesChar = 'y', char noChar = 'n')
        {
            if(Type.GetType(curValue) != null)
            {
                if(Type.GetType(curValue) == typeof(string))
                {
                    return GetInput(inMsg, toLower, newLine, curValue);
                }
                else if(Type.GetType(curValue) == typeof(int))
                {
                    return GetIntInput(inMsg, min, max, newLine, curValue);
                }
                else if(Type.GetType(curValue) == typeof(float))
                {
                    return GetFloatInput(inMsg, min, max, newLine, curValue);
                }
                else if(Type.GetType(curValue) == typeof(bool))
                {
                    return GetBoolInput(inMsg, yesChar, noChar, newLine, curValue);
                }
            }
            return  GetInput(inMsg, toLower, newLine, curValue); //Default return value
        }
    }
}


