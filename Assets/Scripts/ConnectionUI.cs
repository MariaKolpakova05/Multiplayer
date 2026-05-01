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