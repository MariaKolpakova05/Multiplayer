using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ConnectionUI : MonoBehaviour
{
    /*
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _joinButton;

    private void OnEnable()
    {
        _hostButton.OnClick.AddListener(OnHostConnected);
        _joinButton.OnClick.AddListener(OnJoinClient);
    }

    private void OnDisable()
    {
        _hostButton.OnClick.RemoveListener(OnHostConnected);
        _joinButton.OnClick.RemoveListener(OnJoinClient);
    }

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted+=OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted-=OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnServerStarted()
    {
        UpdateStates("Host started");
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsClient && clientId == MetworkManager.Singleton.LocalClientId)
        {
            UpdateStatus("Host connected");
        }
        else if (NetworkManager.Singleton.IsServer)
        {
            UpdateStatus($"client {clientId} connected");
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            UpdateStatus("Disconnected");
        }
    }

    private void OnHostConnected ()
    {
        NetworkManager.Singleton.StartHost();
    }*/

    [SerializeField] private TMP_InputField _nicknameInput;

    // Сохраняем ник локально до появления сетевого объекта игрока.
    public static string PlayerNickname { get; private set; } = "Player";

    public void StartAsHost()
    {
        SaveNickname();
        // Хост одновременно является сервером и клиентом.
        NetworkManager.Singleton.StartHost();
    }

    public void StartAsClient()
    {
        SaveNickname();
        // Клиент только подключается к уже запущенному хосту/серверу.
        NetworkManager.Singleton.StartClient();
    }

    private void SaveNickname()
    {
        // Нормализуем ввод, чтобы сервер не получил пустую строку.
        string rawValue = _nicknameInput != null ? _nicknameInput.text : string.Empty;
        PlayerNickname = string.IsNullOrWhiteSpace(rawValue) ? "Player" : rawValue.Trim();
    }    
}
