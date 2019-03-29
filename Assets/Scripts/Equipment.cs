using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment
{
    [System.Serializable]
    public enum Type
    {
        ARMOR,
        WEAPON,
        SHIELD
    }
    public Type type;
    public int weight;
    public int value;
}
