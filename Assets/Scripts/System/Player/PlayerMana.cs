using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [field:SerializeField] private int energyPoint {get; set;} = 166;
    private int _energyPoint ;
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

    /// <summary>
    /// Use player mana if there is sufficient, otherwise return false
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    public bool ConsumeMana(int cost = 0){
        bool isSufficient = _energyPoint >= cost;
        if (isSufficient){
            _energyPoint -= cost;
        }
        return isSufficient;
    }

    public void RestoreMana(int manaValue){
        if (_energyPoint + manaValue > energyPoint){
            _energyPoint = energyPoint;
        }
        else {
            _energyPoint += manaValue;
        }

    }
    public int GetEnergyPoint(){
        return _energyPoint;
    }
}
