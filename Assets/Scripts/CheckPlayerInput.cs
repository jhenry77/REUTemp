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
    public override void OnStartClient()
    {
         base.OnStartClient();
        Debug.Log("Starting client");
        gameObject.tag = "NetworkPlayer";
       
        if(!isLocalPlayer){
            Debug.Log("This was a local player");
            return;
        }
            
            myRig.SetActive(true);
            //Instantiate(deviceSim, gameObject.transform, false );
            //Instantiate(deviceSimUi, gameObject.transform, false);
            gameObject.tag = "NetworkPlayer";
            Debug.Log("Instantiating");
            players = GameObject.FindGameObjectsWithTag("NetworkPlayer");
            if(players.Length == 1){
                gameObject.transform.Rotate(new Vector3(0,180,0));
                Debug.Log("Rotated the player");
            }else if(players.Length == 2){
                //gameObject.transform.Rotate(new Vector3(0,-180,0));
            }

    
        
            
    }



    // Update is called once per frame
    void Update()
    {
        
        
    }

    
}
