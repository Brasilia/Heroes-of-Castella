using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class BattleBHV : MonoBehaviour
{
    public float tickTimerSeconds = 1;
    [Header("Team 1")]
    public List<CharacterSO> team1;
    public Vector2Int[] positions1;
    [Header("Team2")]
    public List<CharacterSO> team2;
    public Vector2Int[] positions2;

    [Header("Log")]
    public int repeatTimes = 100;
    private float timer;
    private Battle battle = null;
    private bool battleRunning = true;
    [SerializeField]
    private int matchesCount = 0;
    [SerializeField]
    private int team1Wins = 0;
    [SerializeField]
    private int team2Wins = 0;

    private Thread thread = null;


    // Start is called before the first frame update
    void Start()
    {

        //timer = Time.time;
        //battle = new Battle(team1, team2);
        //int i = 0;
        //int j = 0;
        //foreach(Battler b in battle.actors)
        //{
        //    if (b.team == (int)Battle.TEAMS.TEAM1)
        //    {
        //        b.position = positions1[i];
        //        i++;
        //    }
        //    if (b.team == (int)Battle.TEAMS.TEAM1)
        //    {
        //        b.position = positions2[j];
        //        j++;
        //    }
        //}


        //while (matchesCount < repeatTimes)
        //{
        //    battle = new Battle(team1, team2);
        //    matchesCount++;
        //}


        
        for (int i = 0; i < team1.Count; i++)
        {
            team1[i] = Instantiate(team1[i]);
        }
        for (int i = 0; i < team2.Count; i++)
        {
            team2[i] = Instantiate(team2[i]);
        }
        //battle = new Battle(team1, team2);
        thread = new Thread(() => RunCombatThread());
        thread.Start();
        
    }

    private void OnEnable()
    {
        

        //Refresh();
        //StartCoroutine(RunCombat());

        //thread.Join(15000);
    }

    // Update is called once per frame
    void Update()
    {
        if (matchesCount == repeatTimes)
        {
            //thread.Join();
        }
        return;
        timer += Time.deltaTime;
        if (timer < tickTimerSeconds)
        {
            return;
        }
        timer = 0;

        if (battleRunning)
        {
            // Debug.Log("--------------------- Running Turn ---------------------");
            battleRunning = !battle.Update();
            if (!battleRunning)
            {
                Debug.Log("Battle is over.");
                if (battle.CheckWinner() == Battle.TEAMS.TEAM1)
                {
                    team1Wins++;
                }
                else
                {
                    team2Wins++;
                }
                Refresh();
            }
        }

    }

    private void Refresh()
    {
        //battle = new Battle(team1, team2);
        //int i = 0;
        //int j = 0;
        //foreach (Battler b in battle.actors)
        //{
        //    b.character.Refresh();
        //    if (b.team == (int)Battle.TEAMS.TEAM1)
        //    {
        //        b.position = positions1[i];
        //        i++;
        //    }
        //    if (b.team == (int)Battle.TEAMS.TEAM1)
        //    {
        //        b.position = positions2[j];
        //        j++;
        //    }
        //}
    }

    private bool ExecuteCombat()
    {
        // Debug.Log("ExcecuteCombat()");
        if (battleRunning)
        {
            // Debug.Log("--------------------- Running Turn ---------------------");
            battleRunning = !battle.Update();
            if (!battleRunning)
            {
                Debug.Log("Battle is over.");
                if (battle.CheckWinner() == Battle.TEAMS.TEAM1)
                {
                    team1Wins++;
                }
                else
                {
                    team2Wins++;
                }
                Refresh();
                return false;
            }
        }
        return true;
    }

    private IEnumerator RunCombat()
    {
        Debug.Log("Running Combat Coroutine.");
        matchesCount = 0;
        while (matchesCount < repeatTimes)
        {
            Refresh();
            battleRunning = true;
            while (battleRunning)
            {
                battleRunning = !battle.Update();
            }

            if (battle.CheckWinner() == Battle.TEAMS.TEAM1)
            {
                team1Wins++;
            }
            else
            {
                team2Wins++;
            }
            matchesCount++;
        }
        yield return null;
    }

    private void RunCombatThread()
    {
        
        Debug.Log("Running Combat Coroutine.");
        matchesCount = 0;
        while (matchesCount < repeatTimes)
        {
            //Refresh();

            Battle batt = new Battle(team1, team2);
            //batt = new Battle(team1, team2);
            int i = 0;
            int j = 0;
            foreach (Battler b in batt.actors)
            {
                b.character.Refresh();
                if (b.team == (int)Battle.TEAMS.TEAM1)
                {
                    b.position = positions1[i];
                    i++;
                }
                if (b.team == (int)Battle.TEAMS.TEAM1)
                {
                    b.position = positions2[j];
                    j++;
                }
            }




            //battleRunning = true;
            //while (battleRunning)
            //{
            //    battleRunning = !battle.Update();
            //}

            //if (battle.CheckWinner() == Battle.TEAMS.TEAM1)
            //{
            //    team1Wins++;
            //}
            //else
            //{
            //    team2Wins++;
            //}
            matchesCount++;
        }
        Debug.Log("End of thread.");
    }

}
