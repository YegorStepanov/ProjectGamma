using UnityEngine;

public static class ObjectExtensions
{
    public static T NotNull<T>(this T obj) where T : class
    {
        bool isNull = Equals(obj, null);
        Debug.Assert(!isNull, "'obj' is null");
        return obj;
    }
}
