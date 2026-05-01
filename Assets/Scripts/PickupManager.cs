using FishNet.Object;
using UnityEngine;
using System.Collections;

public class PickupManager : NetworkBehaviour
{
    [SerializeField] private GameObject _healthPrefab;
    [SerializeField] private Transform[] _spawnPoints;

    public override void OnStartNetwork()
    {
        if (!base.IsServerInitialized) return;
        foreach (var p in _spawnPoints) SpawnPickup(p.position);
    }

    public void RespawnHealth(Vector3 pos) => StartCoroutine(WaitAndSpawn(pos));

    private IEnumerator WaitAndSpawn(Vector3 pos)
    {
        yield return new WaitForSeconds(10f);
        SpawnPickup(pos);
    }

    private void SpawnPickup(Vector3 pos)
    {
        GameObject go = Instantiate(_healthPrefab, pos, Quaternion.identity);
        go.GetComponent<HealthPickup>().Init(this);
        base.Spawn(go);
    }
}