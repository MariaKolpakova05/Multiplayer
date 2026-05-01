/*using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;
using System.Linq;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnpoints;

    private void OnEnable()
    {
        if (InstanceFinder.NetworkManager != null)
        {
            InstanceFinder.NetworkManager.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
            InstanceFinder.NetworkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
        }
    }

    private void OnDisable()
    {
        if (InstanceFinder.NetworkManager != null)
        {
            InstanceFinder.NetworkManager.ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
            InstanceFinder.NetworkManager.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }
    }

    // Для удалённых клиентов
    private void OnRemoteConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState != RemoteConnectionState.Started) return;
        if (!InstanceFinder.IsServerStarted) return;
        
        SpawnPlayer(args.ConnectionId);
    }

    // Для локального клиента (хост)
    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState != LocalConnectionState.Started) return;
        if (!InstanceFinder.IsServerStarted) return;
        
        // Спавним игрока для хоста
        SpawnPlayer(InstanceFinder.ClientManager.Connection.ClientId);
    }

    private void SpawnPlayer(int clientId)
    {
        if (_spawnpoints == null || _spawnpoints.Length == 0) return;

        int spawnPointIndex = GetSpawnIndex(clientId);
        Transform spawnPoint = _spawnpoints[spawnPointIndex];

        // Ищем уже заспавненного игрока
        NetworkObject playerObject = InstanceFinder.ServerManager.Objects.Spawned
            .FirstOrDefault(x => x.Value.OwnerId == clientId)
            .Value;

        if (playerObject != null)
        {
            playerObject.transform.position = spawnPoint.position;
            playerObject.transform.rotation = spawnPoint.rotation;
        }
    }

    private int GetSpawnIndex(int clientId)
    {
        if (_spawnpoints == null || _spawnpoints.Length == 0) return 0;
        return Mathf.Clamp(clientId, 0, _spawnpoints.Length - 1);
    }
}*/
/*
using FishNet;
using FishNet.Transporting;
using UnityEngine;
using System.Linq;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnpoints;

    private void OnEnable()
    {
        if (InstanceFinder.NetworkManager != null)
        {
            InstanceFinder.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
        }
    }

    private void OnDisable()
    {
        if (InstanceFinder.NetworkManager != null)
        {
            InstanceFinder.ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
        }
    }

    private void OnRemoteConnectionState(RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Started)
        {
            if (!InstanceFinder.NetworkManager.IsServerInitialized) return;
            if (_spawnpoints == null || _spawnpoints.Length == 0) return;

            // Получаем ID подключившегося клиента
            int clientId = args.ConnectionId;
            
            // Находим объект игрока по ID клиента
            var clients = InstanceFinder.ServerManager.Clients;
            if (!clients.TryGetValue(clientId, out var client)) return;
            
            NetworkObject playerObj = client.FirstObject;
            if (playerObj == null) return;

            int index = Mathf.Clamp(clientId, 0, _spawnpoints.Length - 1);
            playerObj.transform.position = _spawnpoints[index].position;
            playerObj.transform.rotation = _spawnpoints[index].rotation;
        }
    }
}*/
