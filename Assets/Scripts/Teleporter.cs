using UnityEngine;
using UnityEngine.Events;

public class Teleporter : MonoBehaviour
{
    public Teleporter TeleportTo;

    public UnityAction<TopDownCharacterController> OnCharacterTeleport;

    public bool isBeingTeleportedTo { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (!isBeingTeleportedTo)
        {
            TopDownCharacterController cc = other.GetComponent<TopDownCharacterController>();
            if (cc)
            {
                cc.Motor.SetPositionAndRotation(TeleportTo.transform.position, TeleportTo.transform.rotation);

                if (OnCharacterTeleport != null)
                {
                    OnCharacterTeleport(cc);
                }
                TeleportTo.isBeingTeleportedTo = true;
            }
        }

        isBeingTeleportedTo = false;
    }
}
