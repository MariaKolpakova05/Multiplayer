using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public enum GameState { WaitingForPlayers, InProgress, ShowingResults }

    // Правильный синтаксис FishNet 4+
    public readonly SyncVar<GameState> CurrentState = new SyncVar<GameState>(GameState.WaitingForPlayers);
    public readonly SyncVar<int> ConnectedPlayers = new SyncVar<int>(0);
    public readonly SyncVar<float> MatchTimer = new SyncVar<float>(60f);
    public readonly SyncVar<string> WinnerName = new SyncVar<string>("");

    [SerializeField] private int _requiredPlayers = 2; // Нужно 2 игрока для старта

    private void Awake() => Instance = this;

    public override void OnStartServer()
    {
        // Подписываемся на события подключения/отключения игроков
        base.ServerManager.OnRemoteConnectionState += OnPlayerConnection;
    }

    private void OnPlayerConnection(NetworkConnection conn, FishNet.Transporting.RemoteConnectionStateArgs args)
    {
        // Обновляем количество игроков
        ConnectedPlayers.Value = base.ServerManager.Clients.Count;

        // ЛОГИКА СБРОСА: Если матч шёл (или показывались результаты), но последний игрок вышел
        if (CurrentState.Value != GameState.WaitingForPlayers && ConnectedPlayers.Value == 0)
        {
            Debug.Log("Все игроки покинули сервер. Принудительный возврат в лобби.");
            ReturnToLobby(); // Вызываем возврат в лобби
            return; // Выход из метода
        }

        // ЛОГИКА СТАРТА: Если мы в лобби и набралось нужное количество игроков
        if (CurrentState.Value == GameState.WaitingForPlayers && ConnectedPlayers.Value >= _requiredPlayers)
        {
            StartMatch();
        }
    }

    private void StartMatch()
    {
        CurrentState.Value = GameState.InProgress;
        MatchTimer.Value = 60f;
        ResetAllPlayers();
        Debug.Log("Матч начался!");
    }

    private void Update()
    {
        // Таймер работает только если сервер запущен и идёт матч
        if (!IsServerInitialized || CurrentState.Value != GameState.InProgress) return;

        MatchTimer.Value -= Time.deltaTime;
        if (MatchTimer.Value <= 0) EndMatch();
    }

    private void EndMatch()
    {
        CurrentState.Value = GameState.ShowingResults;

        // Определяем победителя (игрок с наибольшим счётом)
        PlayerNetwork winner = FindWinner();
        WinnerName.Value = winner != null ? winner.Nickname.Value : "Ничья";

        Invoke(nameof(ReturnToLobby), 7f); // Через 7 секунд возврат в лобби
    }

    private void ReturnToLobby()
    {
        Debug.Log("[SERVER] Возврат в лобби.");
        CurrentState.Value = GameState.WaitingForPlayers;
        MatchTimer.Value = 60f;

        // Сбрасываем позиции и статы всех игроков ПЕРЕД проверкой старта
        ResetAllPlayers();

        // ВАЖНО: Если после сброса игроков всё ещё достаточно, начинаем новый матч сразу
        if (ConnectedPlayers.Value >= _requiredPlayers)
        {
            Debug.Log("[SERVER] Игроков достаточно. Автоматический старт нового раунда...");
            StartMatch();
        }
    }

    private void ResetAllPlayers()
    {
        // сбрасывает состояние всех игроков перед новым раундом
        // Проходим по всем подключённым клиентам
        foreach (var conn in base.ServerManager.Clients.Values)
        {
            // Проходим по всем сетевым объектам, которыми владеет клиент
            foreach (var nob in conn.Objects)
            {
                // Если на объекте есть компонент PlayerNetwork — сбрасываем его
                if (nob.TryGetComponent(out PlayerNetwork pn))
                {
                    pn.ResetPlayerStats();
                }
            }
        }
    }

    private PlayerNetwork FindWinner()
    {
        // Находим всех игроков на сцене и сортируем по очкам (по убыванию)
        // Берём первого (с наибольшим счётом)
        return FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None)
            .OrderByDescending(p => p.Score.Value)
            .FirstOrDefault();
    }
}