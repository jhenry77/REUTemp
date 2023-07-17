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
    private GameObject[] bodyParts;
    [SyncVar]
    public int PID;

    public bool canSee = false;

    // public SceneController sceneController;
    


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
            // bodyParts = GameObject.FindGameObjectsWithTag("BodyParts");
            // foreach(var x in bodyParts){x.SetActive(false);}
            if(players.Length == 1){
                gameObject.transform.Rotate(new Vector3(0,180,0));
                Debug.Log("Rotated the player");
            }else if(players.Length == 2){
                //gameObject.transform.Rotate(new Vector3(0,-180,0));
            }
      
            
    }


    [ClientRpc]
    public void setClientBodyOn(){
        if(!isLocalPlayer){
            return;
        }
        foreach(var x in bodyParts){x.SetActive(true);}
    }



    // Update is called once per frame
    [Client]
    void Update()
    {

        
    }
   

    [Client]
    public void movePlayer(Transform movePosition){
        if(!isLocalPlayer){return;}
        transform.position = movePosition.position;

    }

    // [Client]
    // public void setScenePlayer() {
    //     if (!isLocalPlayer) {
    //         return;
    //     }

    //     // sceneController = GameObject.Find("Scene Controller").GetComponent<SceneController>();
    //     if (PID == 1) {
    //         setButtonPlayer
    //     } else if (PID == 2) {
    //         sceneController.activedCalibartionButtonP2();
    //     }

    // }
    
}
