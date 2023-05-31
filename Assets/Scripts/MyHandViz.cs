using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.XR.CoreUtils;

namespace UnityEngine.XR.Hands
{
    public class MyHandViz : MonoBehaviour
    {
        XRHand hand;

        XRHandSubsystem m_Subsystem;

        public bool jointsAdded = false;

        static readonly List<XRHandSubsystem> s_SubsystemsReuse = new List<XRHandSubsystem>();


        [SerializeField]
        XROrigin m_Origin;

        public Transform myOrigin;

        public List<Transform> left_hand_joints;

        public Transform leftWrist;

        public Transform rightWrist;

        public List<Transform> right_hand_joints;

        public List<Transform> joints;

        

        protected void Awake(){
        #if ENABLE_INPUT_SYSTEM
            InputSystem.InputSystem.settings.SetInternalFeatureFlag("USE_OPTIMIZED_CONTROLS", true);
        #endif // ENABLE_INPUT_SYSTEM
        }


        // Start is called before the first frame update
        void Start()
        {
            

            
        }

        // Update is called once per frame
        void Update()
        {
            if (m_Subsystem != null)
                return;

            SubsystemManager.GetSubsystems(s_SubsystemsReuse);
            if (s_SubsystemsReuse.Count == 0)
                return;


            m_Subsystem = s_SubsystemsReuse[0];
            
            m_Subsystem.updatedHands += OnUpdatedHands;

            //
        }

        void LateUpdate()
        {
            leftWrist.transform.Rotate(90, 0, 0);
        }

        void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
        {
            //Checks to see if the joints are tracked
            bool jointsTracked = (updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints) != 0;
            if(!jointsTracked){
                return;
            }

            if (updateType == XRHandSubsystem.UpdateType.Dynamic)
                return;

            bool leftHandTracked = subsystem.leftHand.isTracked;
            bool rightHandTracked = subsystem.rightHand.isTracked;

            if (leftHandTracked)
            {
                int i = 0;
                var wrist = m_Subsystem.leftHand.GetJoint(XRHandJointID.Wrist);

                for (int fingerIndex = (int)XRHandFingerID.Thumb; fingerIndex <= (int)XRHandFingerID.Little; ++fingerIndex)
                {
                    var fingerId = (XRHandFingerID)fingerIndex;

                    int jointIndexBack = fingerId.GetBackJointID().ToIndex();
                    
                    var parentPose = Pose.identity;
                    wrist.TryGetPose(out Pose wristpose);
                    var localWrist = wristpose.GetTransformedBy(myOrigin);
                    parentPose = wristpose;
                    for (int jointIndex = fingerId.GetFrontJointID().ToIndex(); jointIndex <= jointIndexBack; ++jointIndex)
                    {
                        var otherFingerId = (XRHandJointID)jointIndex;
                        var joint = m_Subsystem.leftHand.GetJoint(otherFingerId);
                        joint.TryGetPose(out var pose);
                        var localPose = pose.GetTransformedBy(myOrigin);
                        
                        
                        Quaternion joint_rotation = Quaternion.Euler(new Vector3(0, 0, 45));
                        //joint_rotation.Set(joint_rotation.x, -joint_rotation.y, -joint_rotation.z, joint_rotation.w);
                        //Quaternion.Euler(new Vector3(pose.rotation.eulerAngles.z, -pose.rotation.eulerAngles.y, pose.rotation.eulerAngles.x));
                       // Quaternion joint_rotation = pose.rotation;
                        joint_rotation = Quaternion.Inverse(parentPose.rotation) * Quaternion.Euler(new Vector3(0, 0, localPose.rotation.eulerAngles.x));
                        switch(i){
                            // case(2):
                            // left_hand_joints[12].localRotation = joint_rotation;
                            // break;
                            // case(3):
                            // left_hand_joints[13].localRotation =  joint_rotation;
                            // break;
                            case(6):
                            left_hand_joints[0].localRotation = joint_rotation;
                            break;
                            case(7):
                            left_hand_joints[1].localRotation = joint_rotation;
                            break;
                            case(8):
                            left_hand_joints[2].localRotation = joint_rotation;
                            break;
                            // case(11):
                            // left_hand_joints[3].localRotation = pose.rotation;
                            // break;
                            // case(12):
                            // left_hand_joints[4].localRotation = pose.rotation;
                            // break;
                            // case(13):
                            // left_hand_joints[5].localRotation = pose.rotation;
                            // break;
                            // case(16):
                            // left_hand_joints[6].localRotation =  pose.rotation;
                            // break;
                            // case(17):
                            // left_hand_joints[7].localRotation = pose.rotation;
                            // break;
                            // case(18):
                            // left_hand_joints[8].localRotation = pose.rotation;
                            // break;
                            // case(21):
                            // left_hand_joints[9].localRotation = pose.rotation;
                            // break;
                            // case(22):
                            // left_hand_joints[10].localRotation = pose.rotation;
                            // break;
                            // case(23):
                            // left_hand_joints[11].localRotation = pose.rotation;
                            // break;
                        }
                       
                        
                        parentPose = pose;
                        print(otherFingerId.ToString() + ", " + i.ToString());
                        i++;
                    }
                }
                
            }

           
        }
        void OnHandUpdate(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
        {
             if (m_Subsystem.leftHand.isTracked)
                print("tracked");

            switch (updateType)
            {
                case XRHandSubsystem.UpdateType.Dynamic:
                    // Update game logic that uses hand data
                    break;
                case XRHandSubsystem.UpdateType.BeforeRender: 
                    // Update visual objects that use hand data
                    break;
            }
        }


    }
}



