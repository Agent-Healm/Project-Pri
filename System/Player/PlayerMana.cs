using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    public int energyPoint {get; set;} = 166;
    public int _energyPoint;
    private void Awake(){
        _energyPoint = energyPoint;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void ManaConsume(int manaCost){
    //     _energyPoint -= manaCost;
    // }
}
