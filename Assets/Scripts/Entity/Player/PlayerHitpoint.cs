using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHitpoint : MonoBehaviour, IHealth, IArmor, IDamageAble, IHealAble
{

    [SerializeField] private float armorRegenStart = 3f;
    [SerializeField] private float armorRegenInterval = 1f;
    [SerializeField] private int maxArmorPoint = 1;
    public int GetCurrentArmorPoint => _armorPoint;
    [SerializeField] private int maxHealthPoint = 1;
    public int GetCurrentHealthPoint => _healthPoint;
    
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

    public bool InflictDamage(int l_damage = 0){
        if (l_damage < 0){return false;}

        if (_coroutine != null){
            StopCoroutine(_coroutine);
        }

        if (_armorPoint - l_damage >= 0){
            _armorPoint -= l_damage;
            // Debug.Log("Hit, current armor : " + _armorPoint);
        }
        else if(_healthPoint + _armorPoint - l_damage > 0){
            _healthPoint -= (l_damage - _armorPoint);
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

    public void HealHealth(int l_heal = 0){
        if (l_heal <= 0){return;}
        if (_healthPoint + l_heal < maxHealthPoint){
            _healthPoint += l_heal;
        }
        else {
            _healthPoint = maxHealthPoint;
        }
    }
    
}
