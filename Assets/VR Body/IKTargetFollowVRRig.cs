using UnityEngine;
using UnityEngine.XR;
using Mirror;

[System.Serializable]
public class VRMap
{
    public Transform headSavedPosition;
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map()
    {
        
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

    public void storeheadLocation(){
        headSavedPosition =ikTarget;

    }
}

public class IKTargetFollowVRRig : NetworkBehaviour
{
    [Range(0,1)]
    public float turnSmoothness = 1f;
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;

    public bool fixTeleport = false;

    

    // Update is called once per frame
    void LateUpdate()
    {
        if(!isLocalPlayer){return;}
        if(fixTeleport){
            head.storeheadLocation();

        }
        
        transform.position = head.ikTarget.position + headBodyPositionOffset;
        float yaw = head.vrTarget.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z),turnSmoothness);
        

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }


    
}
