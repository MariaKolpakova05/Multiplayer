/*using FishNet.Object;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float _speed = 18f;
    [SerializeField] private int _damage = 20;
    [SerializeField] private float _lifetime = 5f;

    private float _spawnTime;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        _spawnTime = Time.time;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        if (base.IsServerInitialized && Time.time > _spawnTime + _lifetime)
        {
            ServerManager.Despawn(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!base.IsServerInitialized) return;
        if (!base.IsSpawned) return;

        PlayerNetwork target = other.GetComponent<PlayerNetwork>();
        if (target == null) return;
        if (target.OwnerId == base.OwnerId) return;

        target.TakeDamage(_damage);
        ServerManager.Despawn(gameObject);
    }
}*/
using FishNet.Object;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.forward * 15f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!base.IsServerInitialized) return;

        if (other.TryGetComponent(out PlayerNetwork target))
        {

            if (target.OwnerId == base.OwnerId) return;


            target.HP.Value = Mathf.Max(0, target.HP.Value - 25);
            base.Despawn();
        }
    }
}