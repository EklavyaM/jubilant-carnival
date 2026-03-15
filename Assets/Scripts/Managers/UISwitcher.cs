using CardMatch.Utils;

namespace CardMatch.Managers
{
    public enum ScreenType
    {
        MainMenu,
        Gameplay
    }
    
    public class UISwitcher : Switcher<ScreenType>
    {
    }
}