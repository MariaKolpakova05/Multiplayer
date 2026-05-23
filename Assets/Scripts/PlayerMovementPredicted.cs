using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using UnityEngine;

public struct MoveData : IReplicateData
{
    public float Horizontal;
    public float Vertical;
    private uint _tick;
    public void Dispose() { }
    public uint GetTick() => _tick;
    public void SetTick(uint value) => _tick = value;

}


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