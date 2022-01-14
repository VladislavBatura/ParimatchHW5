namespace Hw5.Exercise0.Core;

public static class ArgumentsHandler
{
    public static bool IsValidArgs(string[] args)
    {
        if (args[0].Length is < 3 or > 3)
        {
            return false;
        }
        else if (args[0][0] == args[0][1])
        {
            return false;
        }
        if (args[1].Length is < 3 or > 3)
        {
            return false;
        }
        else if (args[1][0] == args[1][1])
        {
            return false;
        }

        if (!decimal.TryParse(args[2], out _))
        {
            return false;
        }

        return true;
    }
}
