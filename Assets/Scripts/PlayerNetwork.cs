using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    public readonly SyncVar<int> HP = new SyncVar<int>(100);
    public readonly SyncVar<string> Nickname = new SyncVar<string>("Player");
    public readonly SyncVar<bool> IsAlive = new SyncVar<bool>(true);

    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _nicknameText;
    [SerializeField] private GameObject _visualModel;
    
    private bool _isRespawning = false;
    private CharacterController _cc;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        // Подписываемся на изменения SyncVar
        HP.OnChange += OnHpChanged;
        Nickname.OnChange += OnNicknameChanged;
        IsAlive.OnChange += OnIsAliveChanged;

        // Начальное состояние UI
        UpdateUI();
        ApplyVisuals(IsAlive.Value);

        // Отправляем ник на сервер, если мы владелец
        if (base.Owner.IsLocalClient)
        {
            // Небольшая задержка, чтобы ConnectionUI точно установил ник
            Invoke(nameof(SendNickname), 0.5f);
        }
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        
        HP.OnChange -= OnHpChanged;
        Nickname.OnChange -= OnNicknameChanged;
        IsAlive.OnChange -= OnIsAliveChanged;
        
        // Отменяем все запланированные вызовы
        CancelInvoke();
    }

    private void SendNickname()
    {
        SetNicknameServerRpc(ConnectionUI.PlayerNickname);
    }

    [ServerRpc]
    public void SetNicknameServerRpc(string name)
    {
        Nickname.Value = string.IsNullOrWhiteSpace(name) 
            ? $"Player_{base.OwnerId}" 
            : name;
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
        HP.Value = Mathf.Max(0, HP.Value - damage);
    }

    private void OnHpChanged(int prev, int next, bool asServer)
    {
        UpdateUI();

        // Только сервер обрабатывает смерть
        if (base.IsServerInitialized && next <= 0 && IsAlive.Value && !_isRespawning)
        {
            _isRespawning = true;
            IsAlive.Value = false; // Это вызовет OnIsAliveChanged на всех клиентах
            
            // Используем InvokeRepeating для надежности
            // Ждем 3 секунды и выполняем респавн
            Invoke(nameof(PerformRespawn), 3f);
            
            Debug.Log($"[PlayerNetwork] Player {OwnerId} died, respawning in 3 seconds...");
        }
    }

    private void OnIsAliveChanged(bool prev, bool next, bool asServer)
    {
        ApplyVisuals(next);
        Debug.Log($"[PlayerNetwork] Player {OwnerId} IsAlive changed to {next}");
    }

    private void OnNicknameChanged(string prev, string next, bool asServer)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_hpText != null) _hpText.text = $"HP: {HP.Value}";
        if (_nicknameText != null) _nicknameText.text = Nickname.Value;
    }

    private void ApplyVisuals(bool isVisible)
    {
        // Визуальная модель
        if (_visualModel != null) 
            _visualModel.SetActive(isVisible);

        // CharacterController
        if (_cc != null) 
            _cc.enabled = isVisible;

        // Коллайдер
        if (TryGetComponent<Collider>(out var col))
            col.enabled = isVisible;
        
        // ВАЖНО: НЕ деактивируем сам GameObject!
        // gameObject.SetActive(isVisible); // ЗАКОММЕНТИРОВАНО
    }

    private void PerformRespawn()
    {
        if (!base.IsServerInitialized) return;

        // Случайная позиция для респавна
        Vector3 respawnPos = new Vector3(
            Random.Range(-7f, 7f), 
            2f, 
            Random.Range(-7f, 7f)
        );

        // Отключаем CC для телепортации
        if (_cc != null)
        {
            _cc.enabled = false;
            transform.position = respawnPos;
        }
        else
        {
            transform.position = respawnPos;
        }

        // Восстанавливаем HP и оживляем
        HP.Value = 100;
        IsAlive.Value = true;
        _isRespawning = false;

        // Включаем CC обратно
        if (_cc != null)
        {
            _cc.enabled = true;
        }

        Debug.Log($"[PlayerNetwork] Player {OwnerId} respawned at {respawnPos}");
    }
}