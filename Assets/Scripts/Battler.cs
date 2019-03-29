using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler
{
    public CharacterSO character;
    public int actionPoints;
    public int team;
    public Battle battle;
    public int index;
    public Vector2Int position;

    public bool defending = false;

    public Battler (CharacterSO character, int team, int index, Battle battle, Vector2Int pos)
    {
        this.character = character;
        this.team = team;
        this.index = index;
        this.battle = battle;
        this.position = pos;
    }

    public Battler(CharacterSO character, int team, int index, Battle battle)
    {
        this.character = character;
        this.team = team;
        this.index = index;
        this.battle = battle;
    }

    public BattlerAction Act ()
    {
        // Debug.Log(character.character.name + " choosing action.");
        Skill skill = character.skills[0].skill;
        List<Battler> possibleTargets = new List<Battler>();
        int targetIndex;
        if (skill.targetType == Skill.TargetType.ALLIES)
        {
            foreach (Battler b in battle.actors)
            {
                if (b.team == this.team)
                {
                    possibleTargets.Add(b);
                }
            }
        }
        else // ENEMIES
        {
            foreach (Battler b in battle.actors)
            {
                if (b.team != this.team)
                {
                    possibleTargets.Add(b);
                }
            }
        }

        float bestEfficiency = 0;
        BattlerAction bestAction = null;
        foreach (Battler b in possibleTargets)
        {
            targetIndex = b.index;
            BattlerAction action = new BattlerAction(skill, index, targetIndex);
            if (bestAction == null && false)
            {
                bestAction = action; //TODO: find better place for code
            }
            Battle.ActionPreOutcome outcome = battle.ResolveAction(action);
            //Debug.Log("Expected damage: " + outcome.damage);
            float efficiency = (float)outcome.damage / (float)battle.actors[targetIndex].character.Hitpoints;
            if (efficiency >= 1)
            {
                efficiency = 1;
                efficiency *= 2; // addition to efficiency for killing blows. TODO: should take into account some threat level of the target
            }
            //Debug.Log("Efficiency: " + efficiency);

            efficiency *=  outcome.toHitChance >= 6 ? 1 : (float)outcome.toHitChance / 6;
            //Debug.Log("Efficiency: " + efficiency);

            if (skill.targetType == Skill.TargetType.ALLIES)
            {
                //TODO: code healing and status effects
            }
            //Debug.Log("Cost/ActionPoints: " + outcome.cost + "/" + actionPoints); 
            if (outcome.cost > actionPoints)
            {
                //Debug.Log("Not enough action points.");
                efficiency = 0; // TODO: refactor with code above
            }
            //Debug.Log("Efficiency: " + efficiency + " / Best efficiency: " + bestEfficiency);
            // Found better move?
            if (efficiency > bestEfficiency)
            {
                bestEfficiency = efficiency;
                bestAction = action;
                //Debug.Log("Current best action: " + bestAction);
            }
        }

        return bestAction;
    }
}
