using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Default;
public class Bullet : MonoBehaviour
{
    [SerializeField] private int uptime = 60;
    [SerializeField] private int damage = 1;

    private Vector2 _direction;
    public Vector2 Direction
    {
        get
        {
            return _direction;
        }
        set
        {
            _direction = value.normalized;
        }
    }
    private int _critChance;
    public int CritChance
    {
        private get
        {
            return _critChance;
        }
        set
        {
            _critChance = value;
        }
    }

    private Coroutine _coroutine;
    void Awake()
    {
        BoxCollider2D l_collider = this.GetComponent<BoxCollider2D>();
        l_collider.includeLayers |= GlobalLayerMask.EnvironmentLayer;

        if (this.gameObject.layer == GlobalLayer.EnemyProjectile)
        {
            l_collider.includeLayers |= GlobalLayerMask.PlayerLayer;
        }
        else if (this.gameObject.layer == GlobalLayer.PlayerProjectile)
        {
            l_collider.includeLayers |= GlobalLayerMask.EnemyLayer;
        }
        // else
        // {
        //     Debug.LogError("Bullet must be either Player or Enemy");
        // }
    }
    void Start()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(BulletMove());
    }

    private IEnumerator BulletMove()
    {
        for (int i = 0; i < uptime; i++)
        {
            transform.Translate(_direction * 0.08f);
            yield return null;
        }
        DestroyBullet();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.TryGetComponent(out IDamageAble damageable))
        {
            int finalDamage = damage;
            if (Random.Range(0, 100) < _critChance)
            {
                // finalDamage += critDamage;
                finalDamage += damage;
            }
            if (damageable.InflictDamage(finalDamage))
            {
                DestroyBullet();
            }
        }
        else
        {
            if (other.gameObject.layer == GlobalLayer.Wall)
            {
                // Debug.Log("I hit a wall");
                DestroyBullet();
            }
        }
    }
    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

    public void SpawnBullet(Vector2 l_direction, Vector2 l_position, int l_critChance)
    {
        Bullet new_bullet = Instantiate(this, l_position + l_direction.normalized * 0.5f, Quaternion.identity);
        new_bullet.Direction = l_direction;
        new_bullet.CritChance = l_critChance;
    }

}