/*using FishNet;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;
using System.Collections;

public class PickupManager : MonoBehaviour
{
    [SerializeField] private GameObject _healthPickupPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _respawnDelay = 10f;
    
    private bool _isInitialized = false;
    private NetworkManager _networkManager;

    private void Start()
    {
        _networkManager = InstanceFinder.NetworkManager;
        
        if (_networkManager == null)
        {
            _networkManager = FindObjectOfType<NetworkManager>();
        }
        
        if (_networkManager != null)
        {
            _networkManager.ServerManager.OnServerConnectionState += OnServerConnectionState;
            
            if (InstanceFinder.IsServerStarted)
            {
                OnServerStarted();
            }
        }
        else
        {
            Debug.LogError("[PickupManager] NetworkManager not found!");
        }
    }

    private void OnServerConnectionState(ServerConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            OnServerStarted();
        }
    }

    private void OnServerStarted()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        
        Debug.Log("[PickupManager] Server started event received, spawning pickups...");
        SpawnAll();
    }

    private void SpawnAll()
    {
        foreach (var point in _spawnPoints)
        {
            if (point != null)
            {
                SpawnPickup(point.position);
            }
        }
    }

    public void OnPickedUp(Vector3 position)
    {
        if (!InstanceFinder.IsServerStarted) return;
        StartCoroutine(RespawnAfterDelay(position));
    }

    private IEnumerator RespawnAfterDelay(Vector3 position)
    {
        yield return new WaitForSeconds(_respawnDelay);
        
        if (InstanceFinder.IsServerStarted)
        {
            SpawnPickup(position);
        }
    }

    private void SpawnPickup(Vector3 position)
    {
        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogError("[PickupManager] Cannot spawn - not server");
            return;
        }
        
        var go = Instantiate(_healthPickupPrefab, position, Quaternion.identity);
        var pickup = go.GetComponent<HealthPickup>();
        
        if (pickup != null)
        {
            pickup.Init(this);
            InstanceFinder.ServerManager.Spawn(go);
            Debug.Log($"[PickupManager] Spawned pickup at {position}");
        }
        else
        {
            Debug.LogError("[PickupManager] Failed to spawn - missing HealthPickup component");
            Destroy(go);
        }
    }
    
    private void OnDestroy()
    {
        if (_networkManager != null)
        {
            _networkManager.ServerManager.OnServerConnectionState -= OnServerConnectionState;
        }
    }
}*/
/*
using FishNet.Object;
using UnityEngine;
using System.Collections;

public class PickupManager : MonoBehaviour
{
    [SerializeField] private GameObject _healthPickupPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _respawnDelay = 10f;

    private bool _isInitialized = false;

    private void Start()
    {
        if (InstanceFinder.NetworkManager != null)
        {
            InstanceFinder.NetworkManager.ServerManager.OnServerStarted += OnServerStarted;
            
            if (InstanceFinder.NetworkManager.IsServerInitialized)
            {
                OnServerStarted();
            }
        }
    }

    private void OnServerStarted()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        SpawnAll();
    }

    private void SpawnAll()
    {
        foreach (var point in _spawnPoints)
        {
            if (point != null) SpawnPickup(point.position);
        }
    }

    public void OnPickedUp(Vector3 position)
    {
        if (!InstanceFinder.NetworkManager.IsServerInitialized) return;
        StartCoroutine(RespawnAfterDelay(position));
    }

    private IEnumerator RespawnAfterDelay(Vector3 position)
    {
        yield return new WaitForSeconds(_respawnDelay);
        if (InstanceFinder.NetworkManager.IsServerInitialized)
        {
            SpawnPickup(position);
        }
    }

    private void SpawnPickup(Vector3 position)
    {
        if (!InstanceFinder.NetworkManager.IsServerInitialized) return;

        GameObject go = Instantiate(_healthPickupPrefab, position, Quaternion.identity);
        HealthPickup pickup = go.GetComponent<HealthPickup>();
        NetworkObject netObj = go.GetComponent<NetworkObject>();

        if (pickup != null && netObj != null)
        {
            pickup.Init(this);
            InstanceFinder.ServerManager.Spawn(netObj);
        }
        else
        {
            Destroy(go);
        }
    }

    private void OnDestroy()
    {
        if (InstanceFinder.NetworkManager != null)
        {
            InstanceFinder.NetworkManager.ServerManager.OnServerStarted -= OnServerStarted;
        }
    }
}*/
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