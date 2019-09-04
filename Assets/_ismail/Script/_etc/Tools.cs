using System;

public static class Tools
{
    public static int ToInt(this object value)
    {
        if (value.GetType() == typeof(char))
        {
            value = value.ToString();
        }
        int.TryParse((string)value, out int a);
        return a;
    }


}
