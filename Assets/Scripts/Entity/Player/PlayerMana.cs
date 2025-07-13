using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private int maxEnergyPoint = 166;
    public int CurrentEnergyPoint { get; private set; }
    
    private void Awake()
    {
        CurrentEnergyPoint = maxEnergyPoint;
    }

    /// <summary>
    /// Use player mana if there is sufficient, otherwise return false
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    public bool ConsumeMana(int cost = 0){
        bool isSufficient = CurrentEnergyPoint >= cost;
        if (isSufficient){
            CurrentEnergyPoint -= cost;
        }
        return isSufficient;
    }

    public void RestoreMana(int manaValue){
        if (CurrentEnergyPoint + manaValue > maxEnergyPoint){
            CurrentEnergyPoint = maxEnergyPoint;
        }
        else {
            CurrentEnergyPoint += manaValue;
        }

    }
}
