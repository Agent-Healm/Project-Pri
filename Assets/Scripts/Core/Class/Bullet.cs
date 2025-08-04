using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Default;
using NaughtyAttributes;
public class Bullet : MonoBehaviour
{
    [Expandable]
    [SerializeField]
    private BulletDataSO baseBullet;

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

    private int _actualUptime;
    public int SetUptime
    {
        set => _actualUptime = value;
    }
    private int _actualDamage;
    public int SetDamage
    {
        set => _actualDamage = value;
    }
    private float _actualSpeed = 0;
    public float SetSpeed
    {
        set => _actualSpeed = value;
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
        for (int i = 0; i < _actualUptime; i++)
        {
            transform.Translate(_direction * 0.08f * _actualSpeed);
            yield return null;
        }
        DestroyBullet();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.TryGetComponent(out IDamageAble damageable))
        {
            int finalDamage = _actualDamage;
            if (Random.Range(0, 100) < _critChance)
            {
                // finalDamage += critDamage;
                finalDamage += _actualDamage;
            }
            if (damageable.InflictDamage(finalDamage))
            {
                DestroyBullet();
            }
        }
        else
        {
            DestroyBullet();
        }
    }
    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

    public void SpawnBullet(Vector2 l_direction, Vector2 l_position, int l_critChance)
    {
        Bullet new_bullet = Instantiate(this, l_position + l_direction.normalized * 0.5f, Quaternion.identity);
        new_bullet._direction = l_direction;
        new_bullet._critChance = l_critChance;

        SetupBullet(new_bullet);
    }

    private void SetupBullet(Bullet bullet)
    {
        foreach (var behaviourSO in baseBullet.Behaviours)
        {
            if (behaviourSO is IBulletBehaviour behaviour)
            {
                behaviour.Apply(bullet);
            }
        }
    }
}