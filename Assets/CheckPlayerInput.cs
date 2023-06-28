using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject deviceSim;
    void Start()
    {


        if (SystemInfo.operatingSystemFamily.ToString() == "Windows"){
            Instantiate(deviceSim);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
}
