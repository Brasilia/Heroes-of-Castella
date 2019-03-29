using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterSO))]
public class CharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CharacterSO myTarget = (CharacterSO)target;
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Hitpoints: " + myTarget.Hitpoints);
        EditorGUILayout.LabelField("Encumbrance: " + myTarget.Encumbrance);
        EditorGUILayout.LabelField("Free Encumbrance: " + myTarget.FreeEncumbrance);
        EditorGUILayout.LabelField("Armor: " + myTarget.Armor);
        EditorGUILayout.LabelField("Weapon Base Damage: " + myTarget.WeaponBaseDamage);
        EditorGUILayout.LabelField("Base Accuracy: " + myTarget.BaseAccuracy);
        EditorGUILayout.LabelField("Evasion: " + myTarget.BaseEvasion);

        if (GUILayout.Button("Refresh"))
        {
            myTarget.Refresh();
        }
    }
}
