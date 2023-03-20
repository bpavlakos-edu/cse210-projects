using System;
using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
class Program
{
    public static UiMenu _mainMenu = new UiMenu(new List<UiOption>{
            new UiOption(()=>{},"&Classic"),
            new UiOption(()=>{},"&Random"),
            new UiOption(()=>{},"&Blink"),
            new UiOption(()=>{},"&Options"),
            new UiOption(()=>{throw new OperationCanceledException();},"&Exit")
        }
    );
    static void Main(string[] args)
    {
        _mainMenu.UiLoop(); //Open the main menu
    }
}