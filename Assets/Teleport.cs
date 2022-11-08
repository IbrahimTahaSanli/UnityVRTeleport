using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [HideInInspector] public static Teleport Instance;

    [HideInInspector] private bool isTeleporting;

    [SerializeField] private float TeleportDeadzone;
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform rig;
    [SerializeField] private Transform InRigPosition;



    public void Awake()
    {
        Instance = this;
    }


    public void ThumbStickEvent(Vector2 pos)
    {
        if (isTeleporting)
        {
            if (pos.magnitude > TeleportDeadzone)
            {
                RaycastHit hit;
                Physics.Raycast(this.transform.position, -LeftHand.transform.up, out hit, 40.0f);

                Debug.DrawRay(this.transform.position, -LeftHand.transform.up, Color.red);

                line.SetPosition(0, this.transform.InverseTransformPoint( this.transform.position ));
                line.SetPosition(1, (hit.distance > 0 ? hit.distance : 40.0f) * this.transform.InverseTransformVector(-LeftHand.transform.up));
            
            }
            else
            {
                RaycastHit hit;
                Physics.Raycast(this.transform.position, -LeftHand.transform.up, out hit, 40.0f);
                if(hit.collider != null)
                    if (hit.normal == Vector3.up) {
                        Vector3 RealPosition = InRigPosition.localPosition;
                        RealPosition.y = 0;
                        rig.position = hit.point - RealPosition; 
                    }
                

                isTeleporting = false;
                this.transform.parent = null;
                line.enabled = false;
            }
        }
        else
        {
            if (pos.magnitude > TeleportDeadzone)
            {
                isTeleporting = true;
                this.transform.parent = LeftHand.transform;
                this.transform.localRotation = Quaternion.identity;
                this.transform.localPosition = Vector3.zero;
                line.enabled = true;
            }
        }
    }
}
