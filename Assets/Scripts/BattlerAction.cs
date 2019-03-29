using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlerAction
{
    public Skill skill;
    public int actorIndex;
    public int targetIndex;

    public BattlerAction(Skill skill, int actorIndex, int targetIndex)
    {
        this.skill = skill;
        this.actorIndex = actorIndex;
        this.targetIndex = targetIndex;
    }
}
