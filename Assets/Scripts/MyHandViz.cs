using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using Mirror;

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

        public List<Transform> right_hand_joints;

        public Transform leftWrist;

        public Transform rightWrist;


        
        Vector3 left_thumb1, left_thumb2, left_thumb3, left_thumb3_end;
        Vector3 right_thumb1, right_thumb2, right_thumb3, right_thumb3_end;
        

        protected void Awake()
        {
        #if ENABLE_INPUT_SYSTEM
            InputSystem.InputSystem.settings.SetInternalFeatureFlag("USE_OPTIMIZED_CONTROLS", true);
        #endif // ENABLE_INPUT_SYSTEM
        }


        // Start is called before the first frame update
        void Start()
        {
            left_thumb1 = left_thumb2 = left_thumb3 = Vector3.zero;
            right_thumb1 = right_thumb2 = right_thumb3  = Vector3.zero;
            
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
        }

        void LateUpdate()
        {
            //leftWrist.transform.Rotate(90, 0, 0);
            //rightWrist.transform.Rotate(90,0,0);
        }

        void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
        {
            //Checks to see if the joints are tracked
          

            if (updateType == XRHandSubsystem.UpdateType.Dynamic)
                return;

            bool leftHandTracked = subsystem.leftHand.isTracked;
            bool rightHandTracked = subsystem.rightHand.isTracked;
            //print(rightHandTracked.ToString() + " , " +  leftHandTracked.ToString());
            
            var originPose = new Pose(myOrigin.position, myOrigin.rotation); // create a pose for XR origin

             if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints) != 0)
             {
                 updateJoints(subsystem.leftHand, originPose, left_hand_joints);
                
             }

            if((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.RightHandJoints) != 0){
                updateJoints(subsystem.rightHand, originPose, right_hand_joints);

           }



           
        }


        void updateJoints(XRHand hand, Pose originPose, List<Transform> joints){
            int i = 0;
            var wrist = hand.GetJoint(XRHandJointID.Wrist);
            float offset = 1;

            
            if(hand.handedness == Handedness.Right){
                offset = -1;
            }

                for (int fingerIndex = (int)XRHandFingerID.Thumb; fingerIndex <= (int)XRHandFingerID.Little; ++fingerIndex)
                {
                    var fingerId = (XRHandFingerID)fingerIndex;

                    int jointIndexBack = fingerId.GetBackJointID().ToIndex();
                    
                    var parentPose = Pose.identity;
                    wrist.TryGetPose(out Pose wristpose);
                    parentPose = wristpose.GetTransformedBy(originPose);

                    for (int jointIndex = fingerId.GetFrontJointID().ToIndex(); jointIndex <= jointIndexBack; jointIndex++)
                    {
                        var otherFingerId = (XRHandJointID)jointIndex;
                        var joint = hand.GetJoint(otherFingerId);
                        joint.TryGetPose(out var pose);
                        
                        Quaternion joint_rotation = Quaternion.Inverse(parentPose.rotation) * pose.rotation; // for default hand
                        Quaternion thumb_rotation = Quaternion.Inverse(parentPose.rotation) * pose.rotation;

                        

                        // We need to swap the XR hand X and Z axes for the robot hand
                        joint_rotation.Set(offset * joint_rotation.x, joint_rotation.z, offset * joint_rotation.y, joint_rotation.w);
                        thumb_rotation.Set(-thumb_rotation.y,  offset * thumb_rotation.z,  thumb_rotation.x, joint_rotation.w);  
                         if(hand.handedness == Handedness.Right)
                            {
                             thumb_rotation.Set(-thumb_rotation.y,  thumb_rotation.z,  thumb_rotation.x, thumb_rotation.w);
                            }
                        print(otherFingerId.ToString() + ", " + i.ToString());

                        switch(i){
                            // the thumb joints require different basis from other fingers
                            case(1):
                            if(hand.handedness == Handedness.Right)
                            {
                                right_thumb1 = pose.position;
                                joints[20].transform.up = (right_thumb2-joints[20].position).normalized;
                            }

                            else
                            {
                                left_thumb1 = pose.position;
                                joints[20].transform.up = (left_thumb2-joints[20].position).normalized;
                            }
                            
                            break;
                            case(2):
                            if(hand.handedness == Handedness.Right)
                            {
                                right_thumb2 = pose.position;
                                joints[21].transform.up = (right_thumb3-joints[21].position).normalized;
                            }

                            else
                            {
                                left_thumb2 = pose.position;
                                joints[21].transform.up = (left_thumb3-joints[21].position).normalized;
                            }
                            break;
                            case(3):
                            if(hand.handedness == Handedness.Right)
                            {
                                right_thumb3 = pose.position;
                                joints[22].transform.up = (right_thumb3_end-joints[22].position).normalized;
                            }

                            else
                            {
                                left_thumb3 = pose.position;
                                joints[22].transform.up = (left_thumb3_end-joints[22].position).normalized;
                            }
                            break;
                            case(4):
                            if(hand.handedness == Handedness.Right)
                            {
                                right_thumb3_end = pose.position;
    
                            }

                            else
                            {
                                left_thumb3_end = pose.position;
                            }
                            break;
                            //Index Finger Rotation
                            case(5):
                            joints[0].localRotation = joint_rotation;
                            break;
                            case(6):
                            joints[1].localRotation = joint_rotation;
                            break;
                            case(7):
                            joints[2].localRotation = joint_rotation;
                            break;
                            case(8):
                            joints[3].localRotation = joint_rotation;
                            break;
                            case(9):
                            joints[4].localRotation = joint_rotation;
                            break;
                            //Middle Finger Rotation
                            case(10):
                            joints[5].localRotation = joint_rotation;
                            break;
                            case(11):
                            joints[6].localRotation = joint_rotation;
                            break;
                            case(12):
                            joints[7].localRotation = joint_rotation;
                            break;
                            case(13):
                            joints[8].localRotation = joint_rotation;
                            break;
                            case(14):
                            joints[9].localRotation = joint_rotation;
                            break;
                            //Ring Finger Rotation
                             case(15):
                            joints[15].localRotation =  joint_rotation;
                            break;
                            case(16):
                            joints[16].localRotation =  joint_rotation;
                            break;
                            case(17):
                            joints[17].localRotation = joint_rotation;
                            break;
                            case(18):
                            joints[18].localRotation = joint_rotation;
                            break;
                            case(19):
                            joints[19].localRotation = joint_rotation;
                            break;
                            //Pinky Finger Rotation
                            case(20):
                            joints[10].localRotation = joint_rotation;
                            break;
                            case(21):
                            joints[11].localRotation = joint_rotation;
                            break;
                            case(22):
                            joints[12].localRotation = joint_rotation;
                            break;
                            case(23):
                            joints[13].localRotation = joint_rotation;
                            break;
                            case(24):
                            joints[14].localRotation = joint_rotation;
                            break;
                        }
                       
                        
                        parentPose = pose;
                        
                        i++;
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



