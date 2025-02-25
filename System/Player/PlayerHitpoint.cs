using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHitpoint : MonoBehaviour, IHealth, IArmor, IDamageAble
{

    [field:SerializeField] public int armorRegenStart {get; set;} = 90;
    [field:SerializeField] public int armorRegenInterval {get; set;} = 30;
    [field:SerializeField] public int maxArmorPoint {get; set;} = 1;
    [field:SerializeField] public int maxHealthPoint {get; set;} = 1;
    
    // public int armorRegenStart = 90;
    // public int armorRegenInterval = 30;
    // public int maxArmorPoint = 1;
    // public int maxHealthPoint = 1;

    private int _timer;
    private int _armorPoint;
    private int _healthPoint;
    void Awake(){
        _armorPoint = maxArmorPoint;
        _healthPoint = maxHealthPoint;
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_armorPoint >= maxArmorPoint){return;}
        if (_healthPoint <= 0){return;}
        ArmorRegeneration();
        _timer +=1;
    }

    /// <summary>
    /// Regenerates the armor points of the entity by 1.
    /// </summary>
    /// <remarks>
    /// This method increases the armor points and logs the current armor value.
    /// </remarks>
    public void ArmorRegeneration(){
        if (_timer > armorRegenStart && _armorPoint < maxArmorPoint){
            _armorPoint += 1;
            // Debug.Log("Armor is healing : " + _armorPoint);
            _timer -= armorRegenInterval;
        }
    }

    public void HealthAtZero(){
        _healthPoint = 0;
            // Destroy(this.gameObject);
    }

    public void ArmorAtZero(){
        _armorPoint = 0;
    }

    public bool InflictDamage(int damage = 0){
        if (damage < 0){return false;}

        if (_armorPoint - damage >= 0){
            _armorPoint -= damage;
            // Debug.Log("Hit, current armor : " + _armorPoint);
        }
        else if(_healthPoint + _armorPoint - damage > 0){
            _healthPoint -= (damage - _armorPoint);
            this.ArmorAtZero();
            // Debug.Log("Hit, current health : " + _healthPoint);
        }
        else {
            this.ArmorAtZero();
            this.HealthAtZero();
        }
        _timer = 0;
        return true;
    }
    public int getPlayerStats(int index){
        switch(index){
            case 1 : {return _healthPoint;}
            case 2 : {return _armorPoint;}
        }
        return _healthPoint;
    }

    public void HealHealth(int heal = 0){
        if (heal <= 0){return;}
        if (_healthPoint + heal < maxHealthPoint){
            _healthPoint += heal;
        }
        else {
            _healthPoint = maxHealthPoint;
        }
    }
}
