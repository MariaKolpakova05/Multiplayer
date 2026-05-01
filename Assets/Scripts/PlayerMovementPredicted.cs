/*using FishNet.Object;
using FishNet.Object.Prediction;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementPredicted : NetworkBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _gravity = -9.81f;

    private CharacterController _cc;
    private PlayerNetwork _playerNetwork;
    private float _verticalVelocity;

    // Структуры для предсказания
    public struct MoveData : IReplicateData
    {
        public float Horizontal;
        public float Vertical;
        private uint _tick;
        public uint GetTick() => _tick;
        public void SetTick(uint value) => _tick = value;
        public void Dispose() { }
    }

    public struct ReconcileData : IReconcileData
    {
        public Vector3 Position;
        public float VerticalVelocity;
        private uint _tick;
        public uint GetTick() => _tick;
        public void SetTick(uint value) => _tick = value;
        public void Dispose() { }
    }

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _playerNetwork = GetComponent<PlayerNetwork>();
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        base.TimeManager.OnTick += OnTick;
        base.TimeManager.OnPostTick += OnPostTick;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        base.TimeManager.OnTick -= OnTick;
        base.TimeManager.OnPostTick -= OnPostTick;
    }

    private void OnTick()
    {
        if (!_playerNetwork.IsAlive) return;

        if (base.IsOwner)
        {
            MoveData md = new MoveData
            {
                Horizontal = Input.GetAxisRaw("Horizontal"),
                Vertical = Input.GetAxisRaw("Vertical")
            };
            Replicate(md);
        }
        else
        {
            Replicate(default);
        }
    }

    private void OnPostTick()
    {
        if (base.IsServerInitialized)
        {
            ReconcileData rd = new ReconcileData
            {
                Position = transform.position,
                VerticalVelocity = _verticalVelocity
            };
            Reconcile(rd);
        }
    }

    [Replicate]
    private void Replicate(MoveData md, ReplicateState state = ReplicateState.Invalid)
    {
        Vector3 move = new Vector3(md.Horizontal, 0, md.Vertical).normalized * _speed;

        _verticalVelocity += _gravity * (float)base.TimeManager.TickDelta;
        move.y = _verticalVelocity;

        _cc.Move(move * (float)base.TimeManager.TickDelta);

        if (_cc.isGrounded) _verticalVelocity = 0f;
    }

    [Reconcile]
    private void Reconcile(ReconcileData rd)
    {
        transform.position = rd.Position;
        _verticalVelocity = rd.VerticalVelocity;
    }
}*/
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using UnityEngine;

// 1. Ñòðóêòóðà ââîäà
public struct MoveData : IReplicateData
{
    public float Horizontal;
    public float Vertical;
    private uint _tick;
    public void Dispose() { }
    public uint GetTick() => _tick;
    public void SetTick(uint value) => _tick = value;

}

// 2. Ñòðóêòóðà êîððåêöèè
public struct ReconcileData : IReconcileData
{
    public Vector3 Position;
    public Quaternion Rotation;
    private uint _tick;
    public void Dispose() { }
    public uint GetTick() => _tick;
    public void SetTick(uint value) => _tick = value;
}

public class PlayerMovementPredicted : NetworkBehaviour
{
    [SerializeField] private float _speed = 5f;
    private CharacterController _cc;
    private float _verticalVelocity;

    private void Awake() => _cc = GetComponent<CharacterController>();

    public override void OnStartNetwork()
    {

        TimeManager.OnTick += OnTick;
    }

    public override void OnStopNetwork()
    {
        if (TimeManager != null) TimeManager.OnTick -= OnTick;
    }

    private void OnTick()
    {
        if (IsOwner)
        {

            Reconciliation(default);


            MoveData md = new MoveData
            {
                Horizontal = Input.GetAxisRaw("Horizontal"),
                Vertical = Input.GetAxisRaw("Vertical")
            };


            Move(md);
        }

        if (IsServerInitialized)
        {

            Move(default);
        }
    }


    [Replicate]
    private void Move(MoveData md, ReplicateState state = ReplicateState.Invalid, Channel channel = Channel.Unreliable)
    {
        if (_cc == null || !_cc.enabled) return;


        Vector3 move = new Vector3(md.Horizontal, 0f, md.Vertical).normalized * _speed;


        if (_cc.isGrounded)
        {
            _verticalVelocity = -2f; 
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * (float)TimeManager.TickDelta;
        }
        move.y = _verticalVelocity;


        _cc.Move(move * (float)TimeManager.TickDelta);
    }


    public override void CreateReconcile()
    {
        ReconcileData rd = new ReconcileData
        {
            Position = transform.position,
            Rotation = transform.rotation
        };
        Reconciliation(rd);
    }


    [Reconcile]
    private void Reconciliation(ReconcileData rd, Channel channel = Channel.Unreliable)
    {
        transform.position = rd.Position;
        transform.rotation = rd.Rotation;
    }
}