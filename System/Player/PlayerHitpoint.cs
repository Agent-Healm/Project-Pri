using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitpoint : MonoBehaviour, IHealth, IArmor, IDamageAble
{

    // public int armorRegenStart {get; set;}
    // public int armorRegenInterval {get; set;}
    // public int maxArmorPoint {get; set;}
    public int armorRegenStart = 90;
    public int armorRegenInterval = 30;
    public int maxArmorPoint = 1;
    public int maxHealthPoint = 1;

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
        // Debug.Log("Health is zero");
        // Debug.Log("Target is Dead");
        _healthPoint = 0;
            // Destroy(this.gameObject);
    }

    public void ArmorAtZero(){
        // Debug.Log("Armor is zero");
        _armorPoint = 0;
    }

    public void InflictDamage(int damage = 0){
        if (damage < 0){return;}

        if (_armorPoint - damage >= 0){
            _armorPoint -= damage;
            // Debug.Log("Hit, current armor : " + _armorPoint);
        }
        else if(_healthPoint + _armorPoint - damage > 0){
            this.ArmorAtZero();
            _healthPoint += (_armorPoint - damage);
            // Debug.Log("Hit, current health : " + _healthPoint);
        }
        else {
            this.ArmorAtZero();
            this.HealthAtZero();
        }
        _timer = 0;

    }

}
