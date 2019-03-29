using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    [System.Serializable]
    public struct Attributes
    {
        public int strength;
        public int dexterity;
        public int inteligence;
        public int vitality;
        public int initiative;
    }

    public string name;
    public Attributes attributes;
    //public Equipment armor;
    //public Equipment shield;
    //public Weapon weapon;


    //public int Hitpoints { get; set; }
    //public int Encumbrance { get; set; }
    //public int Armor { get; set; }
    //public int WeaponBaseDamage { get; set; }
    //public int BaseAccuracy { get; set; }
    //public int BaseEvasion { get; set; }

}
