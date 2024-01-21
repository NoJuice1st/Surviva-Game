using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject monster;
    public Terrain terrain;
    public GameObject currentMonster;

    public void SpawnMonster()
    {
        int x;
        int z;
        if (Random.Range(1, 3) == 1)
        {
            x = Random.Range(0, 100);
        }
        else
        {
            x = Random.Range(150, 255);
        }

        if (Random.Range(1, 3) == 1)
        {
            z = Random.Range(0, 100);
        }
        else
        {
            z = Random.Range(150, 255);
        }


        Vector3 pos = new Vector3(x,  terrain.terrainData.GetHeight(x, z) + monster.transform.localScale.y, z);
        currentMonster = Instantiate(monster, pos, new Quaternion());
    }

    public void RemoveMonster()
    {
        Destroy(currentMonster);
    }

    public bool MonsterCheck()
    {
        if (currentMonster != null)
        {
            return true;
        }
        return false;
    }
}
