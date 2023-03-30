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

using Misc = QuickUtils.Misc;

namespace QuickUtils /*Library name*/
{
    //User Inputs
    public static class Inputs /*Library static class*/
    {
        //Basic string input
        public static string GetInput(string inMsg, bool? toLower = true, bool newLine = false, string curValue = null)
        {
            //Code size has decreased through the use of the ternary operator [ex: dataType myVar = (boolean condition) ? valueIfTrue : valueIfFalse;]
            inMsg = (curValue != null) ? inMsg.Replace(":",$" (currently: "+Misc.QuoteStr(curValue)+" leave blank to cancel):") : inMsg; //When we have a current value, display it
            inMsg = (newLine) ? inMsg + Environment.NewLine : inMsg; //Flag for newline support (off by default)
            Console.Write(inMsg);
            string returnStr = Console.ReadLine() ?? curValue ?? "";//Return the read line, if it's null, return a blank string (or curValue if not empty)
            //Flag for lowercase support (on by default)
            return (toLower == null) ? returnStr.ToUpper() : ((bool)toLower) ? returnStr.ToLower() : returnStr; //Return the return string, false = ignore case, true = ToLower(), null = ToUpper() 
        }
        
        //Integer Input
        public static int? GetIntInput(bool allowEmpty, string inMsg, int? min = null, int? max = null, bool newLine = false, int? curValue = null)
        {   
            while(true) //Infinite loop
            {
                try //Main exception
                {
                    try //Exception funnel
                    {
                        string userInput = GetInput(inMsg, true, newLine, (curValue != null) ? curValue + "":null);
                        if(userInput == ""){throw new ArgumentNullException();} //Detect empty strings
                        int returnVal = int.Parse(userInput); //Parse the input (This ternary codition prevents null from being turned into a string, while also letting us turn int into a string)
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
                            string minStr = (min != null) ? $"minimum {min}" : "";
                            string maxStr = (max != null) ? $"maximum {max}" : "";
                            string toStr = (min != null && max != null) ? " to " : "";
                            string inTheRange = (min != null || max != null) ? $" in the range: {minStr}{toStr}{maxStr}. P" : ", p";
                            Console.WriteLine($"Sorry {returnVal} is not a valid number{inTheRange}lease try again!");
                        }
                    }
                    /*Funnel all invalid entries to FormatException*/
                    catch(ArgumentNullException) //Check argument null exceptions
                    {
                        if(curValue != null || allowEmpty) //Return curValue when curValue isn't null
                        {
                            return curValue; //Why do I need to type cast this?
                        }
                        throw new FormatException(); //If there's no current value, act like all the other exceptions
                    }
                    catch(OverflowException){throw new FormatException();}
                }
                catch(FormatException)
                {
                    Console.WriteLine("Sorry! That's not a valid number, please try again!"); //Figure out why empty strings are going here and not to agument null
                }
            }
        }
        public static int GetIntInput(string inMsg, int? min = null, int? max = null, bool newLine = false, int? curValue = null)
        {
            return (int) GetIntInput(false, inMsg, min, max, newLine, curValue);
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
                            Console.WriteLine($"Sorry {returnVal} is not a valid decimal number{inTheRange}lease try again!");
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
        public static bool? GetBoolInput(bool acceptBlank, string inMsg, char yesChar = 'y', char noChar = 'n', bool newLine = false, bool? curValue = null, bool hideCurValue = false)
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
                    if(acceptBlank && userInput == "")
                    {
                        return null;
                    }
                    else
                    {
                        Console.WriteLine("Sorry! That's not a valid input, please try again!");
                    }
                }
            }
        }
        //Old Method call header / Force Non-Null Return
        public static bool GetBoolInput(string inMsg, char yesChar = 'y', char noChar = 'n', bool newLine = false, bool? curValue = null, bool hideCurValue = false)
        {
            return (bool) GetBoolInput(false, inMsg, yesChar, noChar, newLine, curValue, hideCurValue);
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
        //Get input as a list of ranges
        public static List<int[]> GetRangeIntInput(string inMsg, int? min = null, int? max = null, bool newLine = false, List<int[]> curValue = null, int subtractNum = 0, bool allowNull = false)
        {
            while(true)
            {
                try
                {
                    string userInput = GetInput(inMsg, true, newLine, (curValue != null) ? string.Join(',',curValue) + "":null); //If the current value is not null, send it in the input
                    if(!userInput.Contains('!') && userInput != "")  //This condition is explained on the "else" line
                    {
                        string[] subEntryArray = userInput.Split(","); //Split each entry by a comma
                        List<int[]> returnList = new List<int[]>(); //Initalize the return list
                        //Process every sub entry into an appropriate range
                        for(int i = 0; i < subEntryArray.Length; i++)
                        {
                            int[] subEntryIntRange = RangeFromStringEntry(subEntryArray[i], min, max, subtractNum); //Process the sub entry
                            if(subEntryArray.Length > 0) //Only store it in the return list if it's not empty
                            {
                                returnList.Add(subEntryIntRange); //Add the sub entry after processing it
                            }
                        }
                        Misc.MergeRangeList(returnList);
                        return returnList; //Return the final list
                    }
                    else if(userInput.Contains('!') && min != null && max != null)//When the string contains '!' and the minimum and maximum are not null, select the whole range
                    {
                        return new List<int[]>{new int[]{((int) min) - subtractNum, ((int) max) - subtractNum}};
                    }
                    else if(userInput == "") //Blank input
                    {
                        throw new ArgumentNullException();
                    }
                    else //Any other unrecognized input is ignored
                    {
                        throw new FormatException();
                    }
                }
                catch(ArgumentNullException)
                {
                    if(curValue != null)
                    {
                        return curValue;
                    }
                    else if(allowNull)
                    {
                        new List<int[]>(); //Return a blank list
                    }
                    Console.WriteLine("Sorry! That's not a valid range, please try again!");
                }
                catch(OverflowException)
                {
                    Console.WriteLine("Sorry! That's not a valid range, please try again!");
                }
                catch(FormatException)
                {
                    Console.WriteLine("Sorry! That's not a valid range, please try again!");
                }
            }
            
        }
        //Create a range from a single string entry
        private static int[] RangeFromStringEntry(string stringRange, int? min = null, int? max = null, int subtractNum = 0)
        {
            string[] splitStringArr = stringRange.Split("-");
            List<int> returnIntList = new List<int>();
            for(int i = 0; i < splitStringArr.Length; i++)
            {
                int? entryToInt = ProcessInt(splitStringArr[i], min, max, subtractNum, true); //Turn this string into an integer //Activate strict mode
                if(entryToInt != null)
                {
                    returnIntList.Add((int)entryToInt);//Add this item to the list//Again, why do I need to type cast this??? It's always not null!!!
                }
            }
            return returnIntList.ToArray<int>(); //Convert to an array before returning
        }
        //Process an integer (this is so it can be repeatedly called by a list input function)
        private static int? ProcessInt(string inputVal, int? min = null, int? max = null, int subtractNum = 0, bool strictMode = false)
        {
            int? result = null;
            try //Main exception
                {
                try //Exception funnel
                    {
                        int returnVal = int.Parse(inputVal) - subtractNum; //Parse the input (This ternary codition prevents null from being turned into a string, while also letting us turn int into a string)
                        //Determine if the value is in our boundaries
                        if(min == null && max == null) //No boundaries
                        {
                            return returnVal;
                        }
                        else if((returnVal + subtractNum >= (min ?? (returnVal + subtractNum))) && ((returnVal + subtractNum) <= (max ?? (returnVal + subtractNum)))) //Check boundaries
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
                            string minStr = (min != null) ? $"minimum {min}" : "";
                            string maxStr = (max != null) ? $"maximum {max}" : "";
                            string toStr = (min != null && max != null) ? " to " : "";
                            string inTheRange = (min != null || max != null) ? $" in the range: {minStr}{toStr}{maxStr}. P" : ", p";
                            Console.WriteLine($"Sorry {returnVal} is not a valid number{inTheRange}lease try again!");
                        }
                    }
                    /*Funnel all invalid entries to FormatException*/
                    catch(ArgumentNullException e) //Check argument null exceptions
                    {
                        if(strictMode) //When strict mode is active
                        {
                            throw e; //Throw the error back to the sender
                        }
                    }
                    catch(OverflowException e)
                    {
                        if(strictMode) //When strict mode is active
                        {
                            throw e; //Throw the error back to the sender
                        }
                    }
                }
                catch(FormatException e)
                {
                    if(strictMode) //When strict mode is active
                    {
                        throw e; //Throw the error back to the sender
                    }
                    Console.WriteLine("Sorry! That's not a valid number, please try again!");
                }
            return result;
        }
    }
}



