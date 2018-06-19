using UnityEngine;
/// <summary>
/// Class renaming attributes in unity inspector.
/// </summary>
public class RenameAttribute : PropertyAttribute
{
    public string NewName { get; private set; }
    public RenameAttribute(string name)
    {
        NewName = name;
    }
}