using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute { }

[System.Serializable]
public class YourClass
{
    [ReadOnly]
    public int readOnlyField = 42;

    // Other fields...
}