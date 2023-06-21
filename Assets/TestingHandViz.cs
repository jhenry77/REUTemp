using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using Mirror;
namespace UnityEngine.XR.Hands
{
    public class TestingHandViz : MonoBehaviour
    {
        [SerializeField]
        XROrigin m_Origin;

        XRHandSubsystem m_Subsystem;
        
        HandGameObjects m_LeftHandGameObjects;

        HandGameObjects m_RightHandGameObjects;




        //Class for our hands
        class HandGameObjects{

             GameObject m_HandRoot;
            GameObject m_DrawJointsParent;

            Transform[] m_JointXforms = new Transform[XRHandJointID.EndMarker.ToIndex()];

            bool m_IsTracked;



        }














    }





}