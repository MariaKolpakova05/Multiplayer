/*using FishNet.Object;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private Vector3 _offset = new(0f, 8f, -6f);
    private Camera _cam;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        // ИСПРАВЛЕНО: base.IsOwner -> base.Owner.IsLocalClient
        if (!base.Owner.IsLocalClient)
        {
            enabled = false;
            return;
        }
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (_cam == null) return;
        _cam.transform.position = transform.position + _offset;
        _cam.transform.LookAt(transform.position);
    }
}*/
/*
using FishNet.Object;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] private Vector3 _offset = new(0f, 8f, -6f);
    private Camera _cam;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (!base.IsOwner)
        {
            enabled = false;
            return;
        }
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (_cam == null) return;
        _cam.transform.position = transform.position + _offset;
        _cam.transform.LookAt(transform.position);
    }
}*/
using FishNet.Object;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    public override void OnStartNetwork()
    {
        // Èñïîëüçóåì base.Owner.IsLocalClient âìåñòî IsOwner
        if (!base.Owner.IsLocalClient)
        {
            enabled = false;
        }
    }

    void LateUpdate()
    {
        Camera.main.transform.position = transform.position + new Vector3(0, 7, -7);
        Camera.main.transform.LookAt(transform.position);
    }
}