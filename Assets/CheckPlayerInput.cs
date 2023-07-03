using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CheckPlayerInput : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject deviceSim;
    public GameObject deviceSimUi;
    public GameObject myRig;
    public GameObject[] players;


    [Client]
    void Start()
    {
        if(!isLocalPlayer){
            Debug.Log("This was a local player");
            return;
        }

        
        players = GameObject.FindGameObjectsWithTag("NetworkPlayer");


        if(players.Length < 2 ){
            myRig.SetActive(true);
            //Instantiate(deviceSim, gameObject.transform, false );
            //Instantiate(deviceSimUi, gameObject.transform, false);
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
