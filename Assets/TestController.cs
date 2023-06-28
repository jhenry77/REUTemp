using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{

    public enum ExperimentPhase{
        Practice,Start

    };

    public ExperimentPhase phase;

    [HideInInspector]
    public bool showAnimation = false;
    [HideInInspector]
    public bool hideAnimation = false;
    [HideInInspector]
    public List<ButtonTestScript> myButtons = new List<ButtonTestScript>();



    



    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform buttons in GameObject.Find("/ButtonParent").transform){
            if(buttons.ToString().EndsWith("Button")){
                myButtons.Add(buttons.GetComponent<ButtonTestScript>());
            }

        }
        
    }

    // Update is called once per frame
   
}
