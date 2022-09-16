using UnityEngine;

public static class ObjectExtensions
{
    public static T NotNull<T>(this T obj) where T : class
    {
        bool isNull = Equals(obj, null);

        // Reduce allocations due to string concatenations
        if (isNull)
            Debug.Assert(!isNull, nameof(obj) + " is null");
        return obj;
    }
}
