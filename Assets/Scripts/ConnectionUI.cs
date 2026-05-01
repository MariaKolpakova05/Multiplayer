/*using TMPro;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Transporting;
using UnityEngine;

public class ConnectionUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nicknameInput;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _playerPrefab; // <-- ПЕРЕТАЩИ СЮДА PlayerPrefab В ИНСПЕКТОРЕ
    
    public static string PlayerNickname { get; private set; } = "Player";

    private NetworkManager _networkManager;

    private void Start()
    {
        _networkManager = InstanceFinder.NetworkManager;
        
        if (_networkManager != null)
        {
            _networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
        }
        
        ShowMenu();
    }

    private void OnDestroy()
    {
        if (_networkManager != null)
        {
            _networkManager.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }
    }

    public void StartAsHost()
    {
        SaveNickname();
        if (_networkManager != null)
        {
            _networkManager.ServerManager.StartConnection();
            _networkManager.ClientManager.StartConnection();
        }
    }

    public void StartAsClient()
    {
        SaveNickname();
        if (_networkManager != null)
        {
            _networkManager.ClientManager.StartConnection();
        }
    }

    private void SaveNickname()
    {
        string rawValue = _nicknameInput != null ? _nicknameInput.text : string.Empty;
        PlayerNickname = string.IsNullOrWhiteSpace(rawValue) ? "Player" : rawValue.Trim();
    }
    
    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            HideMenu();
            Debug.Log($"[ConnectionUI] Connected as {PlayerNickname}, hiding menu");
            
            // Спавним игрока, если сервер
            if (InstanceFinder.IsServerStarted)
            {
                SpawnPlayerForConnection(InstanceFinder.ClientManager.Connection);
            }
        }
        else if (args.ConnectionState == LocalConnectionState.Stopped)
        {
            ShowMenu();
            Debug.Log("[ConnectionUI] Disconnected, showing menu");
        }
    }

    private void SpawnPlayerForConnection(NetworkConnection conn)
    {
        if (_playerPrefab == null)
        {
            Debug.LogError("[ConnectionUI] Player prefab is not assigned in Inspector!");
            return;
        }
        
        GameObject player = Instantiate(_playerPrefab);
        InstanceFinder.ServerManager.Spawn(player, conn);
        Debug.Log($"[ConnectionUI] Spawned player for connection {conn.ClientId}");
    }

    private void HideMenu()
    {
        if (_menuPanel != null)
        {
            _menuPanel.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    private void ShowMenu()
    {
        if (_menuPanel != null)
        {
            _menuPanel.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}*/
/*
using FishNet;
using FishNet.Transporting;
using TMPro;
using UnityEngine;

public class ConnectionUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nicknameInput;
    [SerializeField] private GameObject _menuPanel;

    public static string PlayerNickname { get; private set; } = "Player";

    private void Start()
    {
        if (InstanceFinder.NetworkManager != null)
        {
            InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;
        }
        ShowMenu();
    }

    private void OnDestroy()
    {
        if (InstanceFinder.NetworkManager != null)
        {
            InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
        }
    }

    public void StartAsHost()
    {
        SaveNickname();
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();
    }

    public void StartAsClient()
    {
        SaveNickname();
        InstanceFinder.ClientManager.StartConnection();
    }

    private void SaveNickname()
    {
        string rawValue = _nicknameInput != null ? _nicknameInput.text : string.Empty;
        PlayerNickname = string.IsNullOrWhiteSpace(rawValue) ? "Player" : rawValue.Trim();
    }

    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        if (args.ConnectionState == LocalConnectionState.Started)
        {
            HideMenu();
            Debug.Log($"[ConnectionUI] Connected as {PlayerNickname}, hiding menu");
        }
        else if (args.ConnectionState == LocalConnectionState.Stopped)
        {
            ShowMenu();
            Debug.Log("[ConnectionUI] Disconnected, showing menu");
        }
    }

    private void HideMenu()
    {
        if (_menuPanel != null) _menuPanel.SetActive(false);
        else gameObject.SetActive(false);
    }

    private void ShowMenu()
    {
        if (_menuPanel != null) _menuPanel.SetActive(true);
        else gameObject.SetActive(true);
    }
}*/
using FishNet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nicknameInput;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    public static string PlayerNickname { get; private set; } = "Player";

    private void Awake()
    {
        hostBtn.onClick.AddListener(() =>
        {
            SaveNickname();
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
            gameObject.SetActive(false);
        });

        clientBtn.onClick.AddListener(() =>
        {
            SaveNickname();
            InstanceFinder.ClientManager.StartConnection();
            gameObject.SetActive(false);
        });
    }

    private void SaveNickname()
    {
        if (_nicknameInput != null && !string.IsNullOrWhiteSpace(_nicknameInput.text))
        {
            PlayerNickname = _nicknameInput.text.Trim();
        }
        else
        {
            PlayerNickname = "Player";
        }
    }
}