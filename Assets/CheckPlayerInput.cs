using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject deviceSim;
    public GameObject deviceSimUi;
    public GameObject[] players;
    
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("NetworkPlayer");


        if(players.Length == 0 ){
            Instantiate(deviceSim, gameObject.transform, false );
            Instantiate(deviceSimUi, gameObject.transform, false);
            gameObject.tag = "NetworkPlayer";
            Debug.Log("Instantiating");
        }else{
            Debug.Log("Found another player");
            
        }
            
    


        
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
}
