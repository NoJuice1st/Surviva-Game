using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public MonsterManager monsterMan;
    public int currentDay;
    public TerrainManager tm;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        tm.GenerateTerrain();
    }

    

    public void SetCurrentDay(int day)
    {
        currentDay = day;
        tm.GenerateTerrain();
    }

    public void NightTime(bool isNight)
    {
        if(!monsterMan.MonsterCheck())
        {
            if(isNight)
            {
                monsterMan.SpawnMonster();
            }
        }
        else
        {
            if(!isNight)
            {
                monsterMan.RemoveMonster();
            }
        }
    }
}
