class Reference{
    //Attributes
    private string _refText;

    //Constructor
    public Reference()
    {
        _refText = "";
    }

    public Reference(string refText)
    {
        _refText = refText;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse = -1)
    {

    }

    public Reference(string book, int chapter, int[][] verseMatrix)
    {
        _refText = book+" "+chapter+":"+VersesStringFromMatrix(verseMatrix);
    }

    //Single Verse Array
    public Reference(string book, int chapter, int[] singleVerseArr)
    {
        //Found documentation on the .Join function here: https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualbasic.strings.join?view=net-7.0
        //Intellisense let me know that the parameters I want are in reverse order
        //I initalized a new string object to use this function inline
        _refText = book+" "+chapter+":"+string.Join(",",singleVerseArr);
    }

    //Getters and setters
    //refText
    public string GetRefText()
    {
        return _refText;
    }
    public void SetRefText(string refText)
    {
        _refText = refText;
    }

    //Constructor Helper Methods
    private string VersesStringFromMatrix(int[][] versesArr){
        //Process the list
        string returnString = ""; //Initalize a string to store the return string in
        for(int i = 0; i < versesArr.Length; i++)
        {
            if(i != 0)
            {
                returnString += ","; //Only add this string when we aren't the first element
            }
            returnString += VerseStringFromArr(versesArr[i]); //Get this verse
        }
        return returnString;
    }

    //Convert a single verse set into a verse string
    //Ex: "1-2" "1" etc
    private string VerseStringFromArr(int[] verseSet){
        if(verseSet.Length == 1)
        {
            return verseSet[0]+""; //Single verse entry, simply return the verse by itself
        }
        else if(verseSet.Length > 1)
        {
            return verseSet[0]+"-"+verseSet[verseSet.Length]; //Return the scripture as "Start-Finish"
        }
        else
        {
            //Error, return nothing
            return "";
        }
    }

    //Methods

}