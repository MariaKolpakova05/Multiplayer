/*using FishNet.Object;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] private PlayerNetwork _playerNetwork;
    [SerializeField] private int _damage = 10;

    void Update()
    {
        if (!base.IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);
            foreach (var player in allPlayers)
            {
                if (player != _playerNetwork)
                {
                    TryAttack(player);
                    break;
                }
            }
        }
    }

    public void TryAttack(PlayerNetwork target)
    {
        if (!base.IsOwner || target == null)
            return;

        DealDamageServer(target.NetworkObject.ObjectId, _damage);
    }

    [ServerRpc]
    private void DealDamageServer(int targetObjectId, int damage)
    {
        if (!ServerManager.Objects.Spawned.TryGetValue(targetObjectId, out NetworkObject targetObject))
            return;

        PlayerNetwork targetPlayer = targetObject.GetComponent<PlayerNetwork>();
        if (targetPlayer == null || targetPlayer == _playerNetwork)
            return;

        int nextHp = Mathf.Max(0, targetPlayer.HP.Value - damage);  // доступ через .Value
        targetPlayer.HP.Value = nextHp;  // присваивание в .Value
    }
}*/
/*
using FishNet.Object;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] private PlayerNetwork _playerNetwork;
    [SerializeField] private int _damage = 10;

    private void Update()
    {
        if (!base.IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerNetwork[] allPlayers = FindObjectsByType<PlayerNetwork>(FindObjectsSortMode.None);
            foreach (var player in allPlayers)
            {
                if (player != _playerNetwork)
                {
                    TryAttack(player);
                    break;
                }
            }
        }
    }

    public void TryAttack(PlayerNetwork target)
    {
        if (!base.IsOwner || target == null) return;
        DealDamageServer(target.NetworkObjectId, _damage);
    }

    [ServerRpc]
    private void DealDamageServer(int targetObjectId, int damage)
    {
        if (!base.ServerManager.Objects.Spawned.TryGetValue(targetObjectId, out NetworkObject targetObject))
            return;

        PlayerNetwork targetPlayer = targetObject.GetComponent<PlayerNetwork>();
        if (targetPlayer == null || targetPlayer == _playerNetwork) return;

        targetPlayer.TakeDamage(damage);
    }
}*/