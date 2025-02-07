using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IHealth, IDamageAble
{
    public int maxHealthPoint = 10;
    
    private int _healthPoint;
    // Start is called before the first frame update
    void Start()
    {
        _healthPoint = maxHealthPoint;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(int damage = 1){
        _healthPoint -= damage;
        Debug.Log("Crate durability : " + _healthPoint);
        if (_healthPoint <= 0){
            HealthAtZero();
        }
    }

    public void HealthAtZero(){
        Debug.Log("Crate is destroyed");
        Destroy(this.gameObject);
    }
}
