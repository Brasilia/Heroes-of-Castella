using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillSO : ScriptableObject
{
    
    public Skill skill;



    private void OnEnable()
    {
        Debug.Log("Enabling");
        if (skill == null)
        {
            Debug.Log("null skill");
            return;
        }
        skill.targetShape[7] = true;
    }

    public void Refresh()
    {
        OnEnable();
    }

    private void Awake()
    {
        
    }
}
