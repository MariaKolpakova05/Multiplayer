/*using FishNet.Object;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _cooldown = 0.4f;
    [SerializeField] private int _maxAmmo = 10;

    private float _lastShotTime;
    private int _currentAmmo;
    private PlayerNetwork _playerNetwork;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        _currentAmmo = _maxAmmo;
        _playerNetwork = GetComponent<PlayerNetwork>();
    }

    private void Update()
    {
        if (!base.IsOwner) return;
        if (!_playerNetwork.IsAlive.Value) return;  // доступ через .Value
        if (Input.GetKeyDown(KeyCode.Space))
            ShootServer(_firePoint.position, _firePoint.forward);
    }

    [ServerRpc]
    private void ShootServer(Vector3 pos, Vector3 dir)
    {
        if (_playerNetwork.HP.Value <= 0) return;  // доступ через .Value
        if (_currentAmmo <= 0) return;
        if (Time.time < _lastShotTime + _cooldown) return;

        _lastShotTime = Time.time;
        _currentAmmo--;

        GameObject go = Instantiate(_projectilePrefab, pos + dir * 1.2f, Quaternion.LookRotation(dir));
        ServerManager.Spawn(go, base.Owner);
    }
}*/
/*
using FishNet.Object;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _cooldown = 0.4f;
    [SerializeField] private int _maxAmmo = 10;

    private float _lastShotTime;
    private int _currentAmmo;
    private PlayerNetwork _playerNetwork;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        _currentAmmo = _maxAmmo;
        _playerNetwork = GetComponent<PlayerNetwork>();
    }

    private void Update()
    {
        if (!base.IsOwner) return;
        if (!_playerNetwork.IsAlive.Value) return; // Через .Value
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootServer(_firePoint.position, _firePoint.forward);
        }
    }

    [ServerRpc]
    private void ShootServer(Vector3 pos, Vector3 dir)
    {
        if (_playerNetwork.HP.Value <= 0) return; // Через .Value
        if (_currentAmmo <= 0) return;
        if (Time.time < _lastShotTime + _cooldown) return;

        _lastShotTime = Time.time;
        _currentAmmo--;

        GameObject go = Instantiate(_projectilePrefab, pos + dir * 1.2f, Quaternion.LookRotation(dir));
        ServerManager.Spawn(go, base.Owner);
    }
}*/
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