/*using FishNet.Object;
using UnityEngine;

public class HealthPickup : NetworkBehaviour
{
    [SerializeField] private int _healAmount = 40;

    private PickupManager _manager;
    private Vector3 _spawnPosition;

    public void Init(PickupManager manager)
    {
        _manager = manager;
        _spawnPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!base.IsServerInitialized) return;

        var player = other.GetComponent<PlayerNetwork>();
        if (player == null) return;

        // Доступ через .Value
        if (!player.IsAlive.Value) return;
        if (player.HP.Value >= 100) return;

        player.HP.Value = Mathf.Min(100, player.HP.Value + _healAmount);

        _manager.OnPickedUp(_spawnPosition);
        ServerManager.Despawn(gameObject);
    }
}*/
/*
using FishNet.Object;
using UnityEngine;

public class HealthPickup : NetworkBehaviour
{
    [SerializeField] private int _healAmount = 40;

    private PickupManager _manager;
    private Vector3 _spawnPosition;

    public void Init(PickupManager manager)
    {
        _manager = manager;
        _spawnPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!base.IsServerInitialized) return;

        PlayerNetwork player = other.GetComponent<PlayerNetwork>();
        if (player == null) return;
        if (!player.IsAlive.Value) return;  // Через .Value
        if (player.HP.Value >= 100) return; // Через .Value

        player.HP.Value = Mathf.Min(100, player.HP.Value + _healAmount); // Через .Value
        _manager.OnPickedUp(_spawnPosition);
        ServerManager.Despawn(gameObject);
    }
}*/
using FishNet.Object;
using UnityEngine;

public class HealthPickup : NetworkBehaviour
{
    private PickupManager _manager;
    private Vector3 _spawnPos;

    public void Init(PickupManager manager)
    {
        _manager = manager;
        _spawnPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!base.IsServerInitialized) return;

        if (other.TryGetComponent(out PlayerNetwork player))
        {

            if (player.IsAlive.Value && player.HP.Value < 100)
            {
                player.HP.Value = Mathf.Min(100, player.HP.Value + 30);
                _manager.RespawnHealth(_spawnPos);
                base.Despawn();
            }
        }
    }
}