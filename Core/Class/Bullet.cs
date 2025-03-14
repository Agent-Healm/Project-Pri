using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int uptime = 60;
    [field:SerializeField] public int damage {get; set;} = 1;
    [SerializeField] private Vector2 direction;
    public int critChance {get; private set;} = 0;

    private Coroutine _coroutine;
    void Awake(){
        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        if (gameObject.layer == 7){
            collider.excludeLayers = 1 << 6 | 1 << 7;
        }
        else if (gameObject.layer == 9){
            collider.excludeLayers = 1 << 8 | 1 << 9;
        }
    }
    void Start(){
        
        if (_coroutine != null){
            StopCoroutine(_coroutine);
        }
        StartCoroutine(BulletMove(uptime));
    }
    void FixedUpdate(){

    }
    private IEnumerator BulletMove(int uptime){
        for (int i = 0 ; i < uptime ; i++){
            transform.Translate(direction * 0.08f);
            yield return null;
        }
        DestroyBullet();

    }
    private void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.TryGetComponent<IDamageAble>(out IDamageAble damageable)){
            int finalDamage = damage;
            if (Random.Range(0, 100) < critChance){
                // finalDamage += critDamage;
                finalDamage += damage;
            }
            if (damageable.InflictDamage(finalDamage)){
                DestroyBullet();
            }
        }
        else {
            if (other.gameObject.layer == 10){
                // Debug.Log("I hit a wall");
                DestroyBullet();
            }
        }
    }

    public void setDirection(Vector2 direction){
        this.direction = direction;
    }
    public void setCritChance(int critChance){
        this.critChance = critChance;
    }
    public void DestroyBullet(){
        Destroy(this.gameObject);
    }
}
