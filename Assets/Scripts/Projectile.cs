using FishNet.Object;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.forward * 15f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!base.IsServerInitialized) return;

        if (other.TryGetComponent(out PlayerNetwork target))
        {

            if (target.OwnerId == base.OwnerId) return;


            target.HP.Value = Mathf.Max(0, target.HP.Value - 25);

            // Начисляем очко стрелявшему
            if (base.Owner.IsValid)
            {
                foreach (var nob in base.Owner.Objects)
                {
                    if (nob.TryGetComponent<PlayerNetwork>(out var shooterPN))
                    {
                        shooterPN.Score.Value += 1;
                        break;
                    }
                }
            }

            base.Despawn();
        }
    }
}