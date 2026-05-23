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
    public readonly SyncVar<int> Score = new SyncVar<int>(0);

    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _nicknameText;
    [SerializeField] private GameObject _visualModel;
    [SerializeField] private GameObject _uiRoot;
    
    private bool _isRespawning = false;
    private CharacterController _cc;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    public override void OnStartNetwork()
    {
        // Подписываемся на изменения всех SyncVar
        HP.OnChange += OnHpChanged;
        Score.OnChange += (prev, next, asServer) => UpdateUI();
        Nickname.OnChange += (prev, next, asServer) => UpdateUI();
        IsAlive.OnChange += OnIsAliveChanged;

        UpdateUI();
        ApplyVisuals(IsAlive.Value);
    }

    private void OnHpChanged(int prev, int next, bool asServer)
    {
        UpdateUI();
        // Если HP упало до 0, игрок жив и ещё не в процессе респавна — запускаем респавн
        if (base.IsServerInitialized && next <= 0 && IsAlive.Value && !_isRespawning)
        {
            _isRespawning = true;
            IsAlive.Value = false;
            StartCoroutine(RespawnRoutine());
        }
    }

    private void OnIsAliveChanged(bool prev, bool next, bool asServer) => ApplyVisuals(next);

    private void UpdateUI()
    {
        if (_hpText != null) _hpText.text = $"HP: {HP.Value} | Score: {Score.Value}";
        if (_nicknameText != null) _nicknameText.text = Nickname.Value;
    }

    private void ApplyVisuals(bool isVisible)
    {
        if (_visualModel != null) _visualModel.SetActive(isVisible);
        if (_uiRoot != null) _uiRoot.SetActive(isVisible);

        if (_cc != null)
        {
            _cc.enabled = isVisible;
            // Если включили игрока на сервере — сразу синхронизируем его положение в физике
            if (isVisible && base.IsServerInitialized)
            {
                Physics.SyncTransforms();
            }
        }
    }

    private IEnumerator RespawnRoutine()
    {
        // Ждём 3 секунды перед возрождением
        yield return new WaitForSeconds(3f);
        // Отключаем физику для телепортации
        if (_cc != null) _cc.enabled = false;
        // Перемещаем на случайную точку
        transform.position = new Vector3(Random.Range(-7f, 7f), 1f, Random.Range(-7f, 7f));
        // Синхронизируем положение в физическом движке
        Physics.SyncTransforms();
        yield return new WaitForFixedUpdate();

        // Возрождаем только если не идёт показ результатов
        if (GameManager.Instance != null && GameManager.Instance.CurrentState.Value != GameManager.GameState.ShowingResults)
        {
            HP.Value = 100;
            IsAlive.Value = true;
            Debug.Log($"[SERVER] Игрок {Nickname.Value} возродился.");
        }
        _isRespawning = false;
    }

    [ServerRpc]
    public void SetNicknameServerRpc(string name) => Nickname.Value = name;

    public override void OnOwnershipClient(FishNet.Connection.NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        // Когда клиент получает владение объектом — отправляем его ник на сервер
        if (base.IsOwner) SetNicknameServerRpc(ConnectionUI.PlayerNickname);
    }

    [Server]
    public void ResetPlayerStats()
    {
        // Отменяем все корутины (например, респавн)
        StopAllCoroutines();
        _isRespawning = false;

        // 1. Отключаем физику для телепортации
        if (_cc != null) _cc.enabled = false;

        // 2. Случайная точка появления
        transform.position = new Vector3(Random.Range(-7f, 7f), 1f, Random.Range(-7f, 7f));

        // 3. СИНХРОНИЗИРУЕМ ФИЗИКУ (Важно для Linux сервера)
        Physics.SyncTransforms();

        // 4. Сброс статов
        HP.Value = 100;
        Score.Value = 0;
        IsAlive.Value = true;

        // 5. Включаем визуал и коллайдер обратно
        ApplyVisuals(true);

        Debug.Log($"[SERVER] Статистика игрока {Nickname.Value} сброшена.");
    }
}