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