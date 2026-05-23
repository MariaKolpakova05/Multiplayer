using FishNet;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject LobbyPanel;
    public GameObject HUDPanel;
    public GameObject ResultsPanel;

    [Header("Texts")]
    public TMP_Text LobbyText;
    public TMP_Text TimerText;
    public TMP_Text WinnerNameText;

    void Update()
    {
        // 1. ПРОВЕРКА ПОДКЛЮЧЕНИЯ
        // Если сервер и клиент ещё не запущены (мы в главном меню), выключаем игровые панели
        if (!InstanceFinder.ClientManager.Started && !InstanceFinder.ServerManager.Started)
        {
            if (LobbyPanel != null) LobbyPanel.SetActive(false);
            if (HUDPanel != null) HUDPanel.SetActive(false);
            if (ResultsPanel != null) ResultsPanel.SetActive(false);
            return; // Дальше код не выполняем, чтобы не перекрывать кнопки подключения
        }

        // 2. ПРОВЕРКА НАЛИЧИЯ МЕНЕДЖЕРА
        if (GameManager.Instance == null) return;

        // 3. ПОЛУЧЕНИЕ СОСТОЯНИЯ (FishNet 4+ использует .Value)
        var state = GameManager.Instance.CurrentState.Value;

        // 4. УПРАВЛЕНИЕ ПАНЕЛЯМИ
        // Включаем только ту панель, которая соответствует текущему состоянию
        if (LobbyPanel != null) LobbyPanel.SetActive(state == GameManager.GameState.WaitingForPlayers);
        if (HUDPanel != null) HUDPanel.SetActive(state == GameManager.GameState.InProgress);
        if (ResultsPanel != null) ResultsPanel.SetActive(state == GameManager.GameState.ShowingResults);

        // 5. ОБНОВЛЕНИЕ ТЕКСТА
        switch (state)
        {
            case GameManager.GameState.WaitingForPlayers:
                if (LobbyText != null)
                    LobbyText.text = $"Ожидание игроков: {GameManager.Instance.ConnectedPlayers.Value}/2";
                break;

            case GameManager.GameState.InProgress:
                if (TimerText != null)
                    TimerText.text = $"До конца: {(int)GameManager.Instance.MatchTimer.Value} с";
                break;

            case GameManager.GameState.ShowingResults:
                if (WinnerNameText != null)
                    WinnerNameText.text = $"Победитель: {GameManager.Instance.WinnerName.Value}";
                break;
        }
    }
}
