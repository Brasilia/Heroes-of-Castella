using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkillSO))]
public class SkillEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SkillSO myTarget = (SkillSO)target;
        base.OnInspectorGUI();

        EditorGUI.indentLevel++;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Area of Effect", EditorStyles.boldLabel);
         EditorGUI.indentLevel--;

        


        Color colorRed = new Color(1, 0, 0, 0.1f);
        Color colorBlue = new Color(0, 0, 1, 0.1f);
        Color colorGreen = new Color(0, 1, 0, 0.1f);

        Rect rect = EditorGUILayout.BeginVertical(GUILayout.Width(110), GUILayout.Height(50));
        rect.width -= 48;
        rect.x += 26;
        rect.y -= 2;
        // rect.x += EditorGUI.indentLevel * 15;
        EditorGUI.DrawRect(rect, myTarget.skill.targetType == Skill.TargetType.ALLIES ? colorGreen : colorRed);

        for (int i = 0; i < 3; i++)
        {
            rect = EditorGUILayout.BeginHorizontal();
            rect.width -= 44;
            rect.x += 24;
            //rect.x += EditorGUI.indentLevel * 15;
            //EditorGUI.DrawRect(rect, colorBlue);
            EditorGUILayout.LabelField("", GUILayout.Width(20));
            for (int j = 0; j < 5; j++)
            {
                myTarget.skill.targetShape[i * 5 + j] = EditorGUILayout.Toggle(myTarget.skill.targetShape[i * 5 + j]);
            }
            EditorGUILayout.LabelField(i <= 0 ? "" + (i - 1) : (" " + (i - 1)), GUILayout.Width(20));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        

        if (GUILayout.Button("Refresh"))
        {
            myTarget.Refresh();
        }
    }
}
