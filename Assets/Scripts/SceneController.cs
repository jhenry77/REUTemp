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
    public ButtonController myButtonController;
   
    public GameObject SpawnPrefab;
    public Transform SpawnLocation;
    private int numWaiting = 0;

    public GameObject player1;
    public GameObject player2;
    public bool InstantiatedController = false;
    public bool clientFindController = false;

    

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

   

    // Update is called once per frame
    void FixedUpdate()
    {
        if(numWaiting == 2){
            Debug.Log("creating controller");
            GameObject controller = Instantiate(SpawnPrefab, SpawnLocation.position, SpawnLocation.rotation);
            NetworkServer.Spawn(controller);
            changeClientFindClient();
            
            numWaiting++;
            
            findTheController();
            myButtonController.setInitialNumbers();
            int player2Pid = myNetworkManager.player2PID;
            myButtonController.orderOfhands = ((player2Pid /2 ) % 6) - 1;
            myNetworkManager.setPlayerWristScales(myButtonController.handSizeOrder[myButtonController.orderOfhands][0]);
            Debug.Log("My inital scale is +  " + myButtonController.handSizeOrder[myButtonController.orderOfhands][0]);
            for(int i = 0; i < 3; i++){
            if(myButtonController.handSizeOrder[myButtonController.orderOfhands][i] == 1f){
                myButtonController.dataHandSize.Add("Fitted");
            }else if(myButtonController.handSizeOrder[myButtonController.orderOfhands][i] == 1.25f){
                myButtonController.dataHandSize.Add("Large");
            }else if(myButtonController.handSizeOrder[myButtonController.orderOfhands][i] == .75f){
                myButtonController.dataHandSize.Add("Small");
            }
            }
            
        }
        if(clientFindController){
            findTheController();
            clientFindController = false;
            
            
        }

       

        
        
    }

    [ClientRpc]
    public void changeClientFindClient(){
        Debug.Log("changing find the controller to true");
        clientFindController = true;
    }
    public void findTheController(){
        Debug.Log("finding the controller");
        GameObject buttonObject = GameObject.FindGameObjectWithTag("ButtonController");
        myButtonController = buttonObject.GetComponent<ButtonController>();
        //myButtonController.setInitialNumbers();
        myButtonController.player1Pid = myNetworkManager.player1PID;
        myButtonController.player2Pid = myNetworkManager.player2PID;
        
    }

    

    
    


    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
       

    }

    [Server]
    public int generateRandomNum(int low, int high){
        int myRand = Random.Range(low,high);
        return myRand;

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
            Debug.Log("itterating num waiting and it is: " + numWaiting);


        }

        [Server]
        public void activatedCalibrationButtonP1(){
            numWaiting++;
            moveOnPlayer1();
        }
        [ClientRpc]
        public void moveOnPlayer1(){
            Debug.Log("activated calibration button p1");
            PlayEnviroment.SetActive(true);
            Player1Enviroment.SetActive(false);
            MoveOnButton.SetActive(false);
            Enviroment.SetActive(false);
            myNetworkManager.movePlayer1();
            ServersetPlayer1SecneOff();
        }
        [Server]
        public void activatedCalibrationButtonP2(){
            numWaiting++;
            moveOnPlayer2();
        }
        [ClientRpc]
        public void moveOnPlayer2(){
            Debug.Log("activated p2 calibration button");
            PlayEnviroment.SetActive(true);
            Player2Enviroment.SetActive(false);
            CombinedEnviorment.SetActive(true);
            MoveOnButtonP2.SetActive(false);
            EnviromentP2.SetActive(false);
            myNetworkManager.movePlayer2();
            ServersetPlayer2SecneOff();
        }
        

}
