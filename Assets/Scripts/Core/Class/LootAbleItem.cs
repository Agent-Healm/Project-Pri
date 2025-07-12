using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootAbleItem : MonoBehaviour
{
    public virtual void SpawnLoot(Vector3 l_position){
        Instantiate(this, l_position, Quaternion.identity);
        // print("spawned : " + this);
    }
}