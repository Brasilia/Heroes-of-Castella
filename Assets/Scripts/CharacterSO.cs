using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSO : ScriptableObject
{
    //[SerializeField]
    public Character character;
    public WeaponSO weapon;
    public ArmorSO armor;
    public ShieldSO shield;
    public List<SkillSO> skills = new List<SkillSO>();


    //private int hitpoints;



    public int Hitpoints { get; set; }
    public int Encumbrance { get; set; }
    public int FreeEncumbrance { get; set; }
    public int Armor { get; set; }
    public int WeaponBaseDamage { get; set; }
    public int BaseAccuracy { get; set; }
    public int BaseEvasion { get; set; }


    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        Hitpoints = character.attributes.vitality * 10;
        Encumbrance = (weapon == null ? 0 : weapon.weapon.weight) + (armor == null ? 0 : armor.armor.weight) + (shield == null ? 0 : shield.shield.weight);
        FreeEncumbrance = character.attributes.strength - Encumbrance;
        Armor = armor == null ? 0 : armor.armor.value;
        WeaponBaseDamage = weapon == null ? 0 : weapon.weapon.value;
        BaseAccuracy = character.attributes.dexterity + FreeEncumbrance / 2;
        BaseEvasion = BaseAccuracy;
    }

}

