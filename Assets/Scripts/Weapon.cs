using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : Equipment
{
    [System.Serializable]
    public enum Class
    {
        KNIFE,
        SWORD,
        AXE,
        SPEAR,
        BOW,
        STAFF
    }


    public Class weaponClass;
    public bool throwable;
    public bool twoHanded;
    public int range;

}
