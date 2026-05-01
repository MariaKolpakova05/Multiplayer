using FishNet.Object;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    private float _lastShotTime;
    private PlayerNetwork _playerNetwork;

    private void Awake() => _playerNetwork = GetComponent<PlayerNetwork>();

    void Update()
    {
        if (!base.IsOwner || !_playerNetwork.IsAlive.Value) return;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _lastShotTime + 0.5f)
        {
            ShootServerRpc(_firePoint.position, _firePoint.forward);
            _lastShotTime = Time.time;
        }
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 pos, Vector3 dir)
    {
        GameObject projectile = Instantiate(_projectilePrefab, pos, Quaternion.LookRotation(dir));
        base.Spawn(projectile, base.Owner);
    }
}