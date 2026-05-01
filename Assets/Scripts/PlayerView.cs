/*using FishNet.Object;
using TMPro;
using UnityEngine;

public class PlayerView : NetworkBehaviour
{
    [SerializeField] private PlayerNetwork _playerNetwork;
    [SerializeField] private TMP_Text _nicknameText;
    [SerializeField] private TMP_Text _hpText;

    private void Awake()
    {
        if (_playerNetwork != null)
        {
            // Подписываемся на изменения SyncVar
            _playerNetwork.Nickname.OnChange += OnNicknameChanged;
            _playerNetwork.HP.OnChange += OnHpChanged;
        }
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        // Сразу показываем текущие значения
        if (_playerNetwork != null)
        {
            _nicknameText.text = _playerNetwork.Nickname.Value;
            _hpText.text = $"HP: {_playerNetwork.HP.Value}";
        }
    }

    private void OnDestroy()
    {
        // Обязательно отписываемся, чтобы избежать утечек памяти
        if (_playerNetwork != null)
        {
            _playerNetwork.Nickname.OnChange -= OnNicknameChanged;
            _playerNetwork.HP.OnChange -= OnHpChanged;
        }
    }

    private void OnNicknameChanged(string oldValue, string newValue, bool asServer)
    {
        _nicknameText.text = newValue;
    }

    private void OnHpChanged(int oldValue, int newValue, bool asServer)
    {
        _hpText.text = $"HP: {newValue}";
    }
}*/
