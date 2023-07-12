using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.XR.CoreUtils.Bindings;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

using UnityEngine;
using Mirror;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class ButtonController : NetworkBehaviour
{

    public enum ExperimentPhase{
        Practice,Start

    };

    public ExperimentPhase phase;

    [HideInInspector]
    [SyncVar]
    public bool showAnimation = false;
    [HideInInspector]
    [SyncVar]
    public bool hideAnimation = false;
    [HideInInspector]
    public List<ButtonTestScript> myButtons = new List<ButtonTestScript>();
    
    public List<XRPokeFollowAffordance> affordanceScript = new List<XRPokeFollowAffordance>();



    



    // Start is called before the first frame update
    void Start()
    {
        
    }
  
    }
    
             

   
    
    



    
