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