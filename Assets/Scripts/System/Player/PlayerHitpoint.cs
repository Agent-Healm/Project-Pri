using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHitpoint : MonoBehaviour, IHealth, IArmor, IDamageAble
{

    [field:SerializeField] public float armorRegenStart {get; set;} = 3f;
    [field:SerializeField] public float armorRegenInterval {get; set;} = 1f;
    [field:SerializeField] public int maxArmorPoint {get; set;} = 1;
    [field:SerializeField] public int maxHealthPoint {get; set;} = 1;
    
    private int _armorPoint;
    private int _healthPoint;

    private Coroutine _coroutine;
    void Awake(){
        _armorPoint = maxArmorPoint;
        _healthPoint = maxHealthPoint;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (_coroutine != null){
            StopCoroutine(_coroutine);
        }
    }

    // Update is called once per frame
    // void xUpdate()
    // {
    //     if (_armorPoint >= maxArmorPoint){return;}
    //     if (_healthPoint <= 0){return;}

    // }

    private IEnumerator ArmorSystem(){

        yield return new WaitForSeconds(armorRegenStart);
        WaitForSeconds _wait = new WaitForSeconds(armorRegenInterval);
        while (_armorPoint < maxArmorPoint){
            _armorPoint += 1;
            // print("Player armor : " + _armorPoint);
            yield return _wait;
        }
        // print("coroutine done healing armor");
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

        if (_coroutine != null){
            StopCoroutine(_coroutine);
        }

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
        _coroutine = StartCoroutine(ArmorSystem());
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
