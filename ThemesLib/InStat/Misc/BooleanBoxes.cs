namespace ThemesLib.InStat.Misc
{
    internal static class BooleanBoxes
    {
        internal static object TrueBox = (object)true;
        internal static object FalseBox = (object)false;

        internal static object Box(bool value)
        {
            if (value)
                return BooleanBoxes.TrueBox;
            return BooleanBoxes.FalseBox;
        }
    }
}