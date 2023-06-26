using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using Mirror;
namespace UnityEngine.XR.Hands
{
    public class TestingHandViz : NetworkBehaviour
    {
       
        [SerializeField]
        XROrigin m_Origin;

        [SerializeField]
        GameObject m_LeftWristLoc;

        [SerializeField]
        GameObject m_RightWristLoc;

        [SerializeField]
        GameObject m_LeftController;

        [SerializeField]
        GameObject m_RightController;



        XRHandSubsystem m_Subsystem;
        HandGameObjects m_LeftHandGameObjects;
        HandGameObjects m_RightHandGameObjects;

        static readonly List<XRHandSubsystem> s_SubsystemsReuse = new List<XRHandSubsystem>();

        protected void Awake()
        {
        #if ENABLE_INPUT_SYSTEM
            InputSystem.InputSystem.settings.SetInternalFeatureFlag("USE_OPTIMIZED_CONTROLS", true);
        #endif // ENABLE_INPUT_SYSTEM
        }
    

        protected void Update()
        {
            if (m_Subsystem != null)
                return;

            SubsystemManager.GetSubsystems(s_SubsystemsReuse);
            if (s_SubsystemsReuse.Count == 0)
                return;

            m_Subsystem = s_SubsystemsReuse[0];

            if (m_LeftHandGameObjects == null)
            {
                m_LeftHandGameObjects = new HandGameObjects(
                    Handedness.Left,
                    transform,
                    m_LeftWristLoc
                    );
            }

            if (m_RightHandGameObjects == null)
            {
                m_RightHandGameObjects = new HandGameObjects(
                    Handedness.Right,
                    transform,
                    m_RightWristLoc
                    );
            }

            
            
            m_Subsystem.updatedHands += OnUpdatedHands;
        }

        

       


        void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
        {
            // We have no game logic depending on the Transforms, so early out here
            // (add game logic before this return here, directly querying from
            // subsystem.leftHand and subsystem.rightHand using GetJoint on each hand)
            if (updateType == XRHandSubsystem.UpdateType.Dynamic)
                return;

            bool leftHandTracked = subsystem.leftHand.isTracked;
            bool rightHandTracked = subsystem.rightHand.isTracked;

            m_LeftHandGameObjects.UpdateJoints(
                m_Origin,
                subsystem.leftHand,
                m_LeftController,
                leftHandTracked);

            // if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose) != 0)
            //     m_LeftHandGameObjects.UpdateRootPose(subsystem.leftHand);

            m_RightHandGameObjects.UpdateJoints(
                m_Origin,
                subsystem.rightHand,
                m_RightController,
                rightHandTracked);
            
            // if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.RightHandRootPose) != 0)
            //     m_RightHandGameObjects.UpdateRootPose(subsystem.rightHand);
        }

        class HandGameObjects
        {
            GameObject m_HandRoot;
            GameObject m_DrawJointsParent;

            Transform[] m_JointXforms = new Transform[XRHandJointID.EndMarker.ToIndex()];
            GameObject[] m_DrawJoints = new GameObject[XRHandJointID.EndMarker.ToIndex()];
            
            LineRenderer[] m_Lines = new LineRenderer[XRHandJointID.EndMarker.ToIndex()];
            bool m_IsTracked;

            static Vector3[] s_LinePointsReuse = new Vector3[2];
            const float k_LineWidth = 0.005f;

            public HandGameObjects(
                Handedness handedness,
                Transform parent,
                GameObject wristLoc)
            {
                void AssignJoint(
                    XRHandJointID jointId,
                    Transform jointXform,
                    Transform drawJointsParent)
                {
                    int jointIndex = jointId.ToIndex();
                    m_JointXforms[jointIndex] = jointXform;

                   
                }

                

                
               

                Transform wristRootXform = wristLoc.transform;
                

                m_DrawJointsParent = new GameObject();
                m_DrawJointsParent.transform.parent = parent;
                m_DrawJointsParent.transform.localPosition = Vector3.zero;
                m_DrawJointsParent.transform.localRotation = Quaternion.identity;
                m_DrawJointsParent.name = handedness + " Hand Debug Draw Joints";

                if (wristRootXform == null)
                {
                    Debug.LogWarning("Hand transform hierarchy not set correctly - couldn't find Wrist joint!");
                }
                else
                {
                    AssignJoint(XRHandJointID.Wrist, wristRootXform, m_DrawJointsParent.transform);
                    for (int childIndex = 0; childIndex < wristRootXform.childCount; ++childIndex)
                    {
                        var child = wristRootXform.GetChild(childIndex);

                        for (int fingerIndex = (int)XRHandFingerID.Thumb;
                             fingerIndex <= (int)XRHandFingerID.Little;
                             ++fingerIndex)
                        {
                            var fingerId = (XRHandFingerID)fingerIndex;

                            var jointIdFront = fingerId.GetFrontJointID();
                            if (!child.name.EndsWith(jointIdFront.ToString()))
                                continue;

                            AssignJoint(jointIdFront, child, m_DrawJointsParent.transform);
                            var lastChild = child;

                            int jointIndexBack = fingerId.GetBackJointID().ToIndex();
                            for (int jointIndex = jointIdFront.ToIndex() + 1;
                                 jointIndex <= jointIndexBack;
                                 ++jointIndex)
                            {
                                for (int nextChildIndex = 0; nextChildIndex < lastChild.childCount; ++nextChildIndex)
                                {
                                    var nextChild = lastChild.GetChild(nextChildIndex);
                                    if (nextChild.name.EndsWith(XRHandJointIDUtility.FromIndex(jointIndex).ToString()))
                                    {
                                        lastChild = nextChild;
                                        break;
                                    }
                                }

                                 if (!lastChild.name.EndsWith(XRHandJointIDUtility.FromIndex(jointIndex).ToString()))
                                     throw new InvalidOperationException("Hand transform hierarchy not set correctly - couldn't find " + XRHandJointIDUtility.FromIndex(jointIndex) + " joint!");

                                var jointId = XRHandJointIDUtility.FromIndex(jointIndex);
                                AssignJoint(jointId, lastChild, m_DrawJointsParent.transform);
                            }
                        }
                    }
                }

                for (int fingerIndex = (int)XRHandFingerID.Thumb;
                     fingerIndex <= (int)XRHandFingerID.Little;
                     ++fingerIndex)
                {
                    var fingerId = (XRHandFingerID)fingerIndex;

                    var jointId = fingerId.GetFrontJointID();
                    if (m_JointXforms[jointId.ToIndex()] == null)
                        Debug.LogWarning("Hand transform hierarchy not set correctly - couldn't find " + jointId + " joint!");
                }
            }

        


            public void UpdateRootPose(XRHand hand)
            {
                var xform = m_JointXforms[XRHandJointID.Wrist.ToIndex()];
                xform.localPosition = hand.rootPose.position;
                xform.localRotation = hand.rootPose.rotation;
            }

            public void UpdateJoints(
                XROrigin xrOrigin,
                XRHand hand,
                GameObject wristLocation,
                bool areJointsTracked)
            {
                
                
                if (!areJointsTracked)
                    return;

                var originTransform = xrOrigin.Origin.transform;
                var originPose = new Pose(originTransform.position, originTransform.rotation);
                
                



                var wristPose = Pose.identity;
                // var wrist = m_JointXforms[hand.GetJoint(XRHandJointID.Wrist).id.ToIndex()];
                // wrist.rotation = wristLocation.transform.rotation;
                
                                

                UpdateJoint(originPose, hand.GetJoint(XRHandJointID.Wrist), ref wristPose, hand);
                //UpdateJoint(originPose, hand.GetJoint(XRHandJointID.Palm), ref wristPose, hand, false);
                
               
                for (int fingerIndex = (int)XRHandFingerID.Thumb;
                    fingerIndex <= (int)XRHandFingerID.Little;
                    ++fingerIndex)
                {
                    var parentPose = wristPose;
                    var fingerId = (XRHandFingerID)fingerIndex;

                    int jointIndexBack = fingerId.GetBackJointID().ToIndex();
                    for (int jointIndex = fingerId.GetFrontJointID().ToIndex();
                        jointIndex <= jointIndexBack;
                        ++jointIndex)
                    {
                        if (m_JointXforms[jointIndex] != null)
                            UpdateJoint(originPose, hand.GetJoint(XRHandJointIDUtility.FromIndex(jointIndex)), ref parentPose, hand);
                    }
                }
            }

            void UpdateJoint(
                Pose originPose,
                XRHandJoint joint,
                ref Pose parentPose,
                XRHand myHand,
                bool cacheParentPose = true
                )
            {
                float offset = 1;

                if(myHand.handedness == Handedness.Right){
                    offset = -1;
                }

                int jointIndex = joint.id.ToIndex();
                var xform = m_JointXforms[jointIndex];
                if (xform == null || !joint.TryGetPose(out var pose))
                    return;
                

                Quaternion jointRotation; 

                var inverseParentRotation = Quaternion.Inverse(parentPose.rotation);
                //xform.localPosition = inverseParentRotation * (pose.position - parentPose.position);
                jointRotation = inverseParentRotation * pose.rotation;
                jointRotation.Set(offset * jointRotation.y, jointRotation.z, offset * jointRotation.x, jointRotation.w);
                xform.localRotation = jointRotation;
                
                
                if (cacheParentPose)
                    parentPose = pose;


            }

            static void ToggleRenderers<TRenderer>(bool toggle, Transform xform)
                where TRenderer : Renderer
            {
                if (xform.TryGetComponent<TRenderer>(out var renderer))
                    renderer.enabled = toggle;

                for (int childIndex = 0; childIndex < xform.childCount; ++childIndex)
                    ToggleRenderers<TRenderer>(toggle, xform.GetChild(childIndex));
            }
        }
    }
}