using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Battle
{
    public struct ActionPreOutcome
    {
        public int damage;
        public int toHitChance;
        public int cost;
    }

    public enum TEAMS
    {
        TEAM1 = 1,
        TEAM2 = -1,
        NONE = 0
    }


    private const int TEAM1 = (int)TEAMS.TEAM1;
    private const int TEAM2 = (int)TEAMS.TEAM2;


    private int tick = 1;
    private const int TICK_CICLE = 6;
    private const int ACTION_POINTS_THRESHOLD = 12;

    private System.Random random = new System.Random();


    public List<Battler> actors = new List<Battler>();

    public Battle (List<CharacterSO> team1, List<CharacterSO> team2)
    {
        Setup(team1, team2);
    }

    public void Setup (List<CharacterSO> team1, List<CharacterSO> team2)
    {
        int i = 0;
        foreach (CharacterSO c in team1)
        {
            Battler b = new Battler(c, TEAM1, i, this);
            actors.Add(b);
            i++;
        }
        foreach (CharacterSO c in team2)
        {
            Battler b = new Battler(c, TEAM2, i, this);
            actors.Add(b);
            i++;
        }
    }

    public void Initialize()
    {
        foreach (Battler b in actors)
        {
            b.actionPoints = b.character.character.attributes.initiative;
        }
    }

    public Battler NextActor()
    {
        List<Battler> readyActors = new List<Battler>();
        foreach (Battler b in actors)
        {
            if (b.character.Hitpoints == 0)
            {
                continue;
            }
            if (b.actionPoints >= ACTION_POINTS_THRESHOLD)
            {
                readyActors.Add(b);
            }
        }
        readyActors = readyActors.OrderBy(b=>b.actionPoints).ToList();
        if (readyActors.Count == 0)
        {
            return null;
        }
        List<Battler> elligibleActors = new List<Battler>();
        foreach (Battler b in readyActors)
        {
            if (b.actionPoints == readyActors[0].actionPoints) {
                elligibleActors.Add(b);
            }
        }
        int rand = random.Next(0, elligibleActors.Count); //Random.Range(0, elligibleActors.Count) //-- not thread safe

        return elligibleActors[rand];
    }

    public bool Tick()
    {
        if (NextActor() != null)
        {
            return false;
        }
        foreach (Battler b in actors)
        {
            int initiative = b.character.character.attributes.initiative;
            //b.actionPoints += initiative >= tick ? 7 : 1; // TODO: adjust the extra action points
            b.actionPoints += initiative + 1;
        }
        tick++;
        if (tick > TICK_CICLE)
        {
            tick = 1;
        }
        return true;
    }

    public bool Update()
    {
        if (CheckWinner() != TEAMS.NONE)
        {
            // Debug.Log("WINNER: " + (CheckWinner() == TEAMS.TEAM1 ? "Team 1" : "Team2") + "!");
            return true;
        }
       

        Battler nextActor = NextActor();
        if (nextActor == null)
        {
            Tick();
        }
        else
        {
            BattlerAction action = nextActor.Act();
            ExecuteAction(action);
        }
        return false;
    }

    public TEAMS CheckWinner()
    {
        int team1Members = 0;
        int team2Members = 0;
        foreach (Battler b in actors)
        {
            if (b.character.Hitpoints > 0)
            {
                if (b.team == TEAM1)
                {
                    team1Members++;
                }
                else // TEAM2
                {
                    team2Members++;
                }
            } 
        }
        if (team1Members == 0)
        {
            return TEAMS.TEAM2;
        }
        else if (team2Members == 0)
        {
            return TEAMS.TEAM1;
        }
        return TEAMS.NONE;
    }

    private void ExecuteAction(BattlerAction action)
    {
        ActionPreOutcome outcome = ResolveAction(action);
        actors[action.actorIndex].actionPoints -= outcome.cost;
        if (random.Next(1, 7) <= outcome.toHitChance) // skill hit
        {
            // Debug.Log(" - " + actors[action.actorIndex].character.name + " hit " + actors[action.targetIndex].character.name + " for " + outcome.damage + " hitpoints.");
            actors[action.targetIndex].character.Hitpoints -= outcome.damage;
            if (actors[action.targetIndex].character.Hitpoints < 0)
            {
                actors[action.targetIndex].character.Hitpoints = 0;
            }
        }
        else
        {
            // Debug.Log(" - " + actors[action.actorIndex].character.name + "'s attack on " + actors[action.targetIndex].character.name + " missed.");
        }
    }

    public ActionPreOutcome ResolveAction(BattlerAction action)
    {
        ActionPreOutcome outcome;
        
        CharacterSO actor = actors[action.actorIndex].character;
        CharacterSO target = actors[action.targetIndex].character;
        List<Battler> tempActors = new List<Battler>();
        foreach (Battler b in tempActors)
        {
            tempActors.Add(new Battler(b.character, b.team, b.index, this, b.position));
        }

        // Damage & Cost
        int cost = 2;
        cost += action.skill.extraCost;
        // cost += distance, if melee
        cost += Mathf.Min(Distance(action.actorIndex, action.targetIndex) - actor.weapon.weapon.range, 0); // TODO: consider path blocking
        int damage = 0;
        damage += action.skill.extraDamage;
        switch (action.skill.baseDamage)
        {
            case Skill.BaseDamage.STRENGTH:
                damage += actor.character.attributes.strength;
                damage += actor.weapon.weapon.value;
                damage -= target.Armor; // .armor.armor.value;
                cost += actor.weapon.weapon.weight;
                break;
            case Skill.BaseDamage.FREE_ENCUMBRANCE:
                damage += actor.character.attributes.strength - actor.Encumbrance;
                damage += actor.weapon.weapon.value;
                damage -= target.Armor; // .armor.armor.value;
                cost += actor.weapon.weapon.weight;
                break;
            case Skill.BaseDamage.INTELLIGENCE:
                damage += actor.character.attributes.inteligence;
                damage -= target.character.attributes.inteligence;
                cost += 3 - Mathf.FloorToInt(actor.FreeEncumbrance/2);
                break;
        }

        // To Hit Chance
        int toHitChance = 6;
        int targetEvasion = target.character.attributes.dexterity + Mathf.FloorToInt(target.FreeEncumbrance / 2);
        //targetEvasion += target.shield.shield.value * (actors[action.targetIndex].defending ? 2 : 1); //FIXME: characterSO must handle null values for equipment
        switch (action.skill.type)
        {
            case Skill.Type.MELEE:
                toHitChance += actor.character.attributes.dexterity + Mathf.FloorToInt(actor.FreeEncumbrance/2);
                toHitChance -= targetEvasion;
                break;
            case Skill.Type.PROJECTILE:
                toHitChance += actor.character.attributes.dexterity + Mathf.FloorToInt(actor.FreeEncumbrance / 2);
                toHitChance -= targetEvasion;
                break;
            case Skill.Type.DIRECT:
                break;
        }
        if (toHitChance < 1)
        {
            toHitChance = 1;
        }

        outcome.cost = cost;
        outcome.toHitChance = toHitChance;
        outcome.damage = damage;

        return outcome;
    }

    public int Distance (int actorIndex, int targetIndex)
    {
        int distance = 1;
        distance += 2 - (actors[actorIndex].position.x + actors[targetIndex].position.x);
        distance += 2 * (actors[actorIndex].position.y + actors[targetIndex].position.y);
        return 0;
    }

    private void EndBattle(TEAMS winner)
    {

    }
}
