using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SceneController : NetworkBehaviour
{
    
    public GameObject Player1Enviroment;
    public GameObject Player2Enviroment;
    public GameObject PlayEnviroment;
    public GameObject Enviroment;
    public GameObject EnviromentP2;
    public GameObject CombinedEnviorment;
    public GameObject MoveOnButton;
    public GameObject MoveOnButtonP2;
    public myNetworkManager myNetworkManager = new myNetworkManager();
   
    public GameObject SpawnPrefab;
    public Transform SpawnLocation;
    private int numWaiting = 0;

    public GameObject player1;
    public GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if(numWaiting == 1){
            Debug.Log("creating controller");
            GameObject controller = Instantiate(SpawnPrefab, SpawnLocation.position, SpawnLocation.rotation);
            NetworkServer.Spawn(controller);
            numWaiting++;
        }
        
    }

    
    


    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
       

    }



    // [Client]
    // public void startCalibartion(){
    //     Debug.Log("In calibration");
    //     rayInteractors = GameObject.FindGameObjectsWithTag("RayInteractor");
    //     Debug.Log("ray interactors length" + rayInteractors.GetLength(0));
    //     Debug.Log("Body parts length: " + bodyParts.GetLength(0));
    //     Debug.Log("before for loops");
    //     foreach(var x in bodyParts){x.SetActive(true);}
    //     foreach(var x in rayInteractors){x.SetActive(false); }
    //     Debug.Log("Setting things to active");
    //     UISample.SetActive(false);
    //     myMirror.SetActive(true);
    //     Enviroment.SetActive(true);
    //     theTable.SetActive(false);
    //     opposingChair.SetActive(false);
    //     MoveOnButton.SetActive(true);
    // }

    

    
    [Client]
    public void startPlayPhase(){
        startServerPlayPhase();
        CombinedEnviorment.SetActive(true);
        //MovePlayer 1
        //MovePlayer 2
        
    

    }

    [Command(requiresAuthority = false)]
    public void startServerPlayPhase(){
        CombinedEnviorment.SetActive(true);
        Enviroment.SetActive(false);
        EnviromentP2.SetActive(false);

    }

     [Command(requiresAuthority = false)]
        public void startPlayPhase_Client(){
            Debug.Log("Instantiating buttons");
            GameObject controller = Instantiate(SpawnPrefab, SpawnLocation.position, SpawnLocation.rotation);
            NetworkServer.Spawn(controller);

        }

        

        public void setAsplayer1(GameObject player){
            player1 = player;
            Debug.Log("Setting player one as: " + player1.name);
        }

        public void setAsPlayer2(GameObject player){
            player2 = player;
            Debug.Log("Setting player 2 as: " + player2.name);

        }


        [Client]
        public void activedCalibartionButtonP1(){
            PlayEnviroment.SetActive(true);
            Player1Enviroment.SetActive(false);
            Debug.Log("p1 activated calibration button");
            MoveOnButton.SetActive(false);
            Enviroment.SetActive(false);
            myNetworkManager.movePlayer1();
            ServersetPlayer1SecneOff();
            UpdateServerNumWaiting();
        }

         [Client]
        public void activedCalibartionButtonP2(){
            PlayEnviroment.SetActive(true);
            Player2Enviroment.SetActive(false);
            CombinedEnviorment.SetActive(true);
            MoveOnButtonP2.SetActive(false);
            EnviromentP2.SetActive(false);
            myNetworkManager.movePlayer2();
            ServersetPlayer2SecneOff();
            UpdateServerNumWaiting();
            
        }
        [Command(requiresAuthority = false)]
        public void ServersetPlayer1SecneOff(){
            Player1Enviroment.SetActive(false);

        }

        [Command(requiresAuthority = false)]
        public void ServersetPlayer2SecneOff(){
            Player2Enviroment.SetActive(false);

        }
         [Command(requiresAuthority = false)]
        public void UpdateServerNumWaiting(){
            numWaiting++;

        }
}
