using UnityEngine;

[CreateAssetMenu(fileName = "SO_Product", menuName = "Scriptable Objects/SO_Product")]
public class SO_Item : ScriptableObject
{
    public ItemType type;
    public float cost;
}

public enum ItemType
{
    
}
