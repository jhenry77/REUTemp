using UnityEngine;
using Mirror;

[AddComponentMenu("")]
    public class myNetworkManager : NetworkManager
    {
        [SerializeField]
        public Transform buttonLocaiton;
        public int numplayer;
        public GameObject serverCamera;
        public Transform player1Spawn;
        public Transform player2Spawn;
        
        public GameObject[] bodyParts;
        public GameObject player1;
        public GameObject player2;
        public SceneController mySceneController;
        public int player1PID;
        public int player2PID;

        public ParticipantData thisParticipant;
        public Transform CombinedSpawn1;
        public Transform CombinedSpawn2;
        public GameObject enviroment1;
        public GameObject enviroment2;
        public GameObject combinedEnviorment;

        
        

      public override void Start(){
        Debug.Log("Starting in network manager");
        if (SystemInfo.operatingSystemFamily.ToString() != "Windows")
            {
                
                Debug.Log("On Quest");
                StartClient();
    

            }else{
                Debug.Log("On PC");
               //serverCamera.SetActive(true);
             }
      }
    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
        serverCamera.SetActive(true);
        enviroment1.SetActive(false);
        enviroment2.SetActive(false);
        combinedEnviorment.SetActive(true);
     }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        numplayer = numPlayers;
        // add player at correct spawn position
        Debug.Log("Adding a player in server add player");
        // Transform start = player1Spawn;
        Transform start = numPlayers == 0 ? player1Spawn : player2Spawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        if(numPlayers == 0){
            player1 = player;
            setPID(player, player1PID);
            
        }else if(numPlayers == 1){
            player2 = player;
            setPID(player, player2PID);
            
        }
        player.tag = "NetworkPlayer";
        NetworkServer.AddPlayerForConnection(conn, player);
        GameObject.FindGameObjectsWithTag("NetworkPlayer");
        //bodyParts = GameObject.FindGameObjectsWithTag("BodyParts");
        //foreach(var x in bodyParts){x.SetActive(false);Debug.Log("setting a body part to be invisible");}
            if(numplayer == 0){
            //GameObject controller = Instantiate(spawnPrefabs[0], buttonLocaiton.position, buttonLocaiton.rotation);
            //NetworkServer.Spawn(controller);
            }
        

        
        
    }
    public void setPID(GameObject player, int id){
        player.GetComponent<CheckPlayerInput>().PID = id;
    }

   

    public void movePlayer1(){
        Debug.Log("moving the first player");
         GameObject[] players = GameObject.FindGameObjectsWithTag("NetworkPlayer");
        foreach(var x in players){
            int currPlayerPid = x.GetComponent<CheckPlayerInput>().PID;
            if(currPlayerPid % 2 == 0){
            }else{
                x.GetComponent<CheckPlayerInput>().movePlayer(CombinedSpawn1);
            }
        }

    }

    public void movePlayer2(){
         GameObject[] players = GameObject.FindGameObjectsWithTag("NetworkPlayer");
        foreach(var x in players){
            int currPlayerPid = x.GetComponent<CheckPlayerInput>().PID;
            if(currPlayerPid % 2 == 0){
                x.GetComponent<CheckPlayerInput>().movePlayer(CombinedSpawn2);
            }

            
        }
    }

    public void setPlayerWristScales(float percentage){
        Debug.Log("setting player wrist scales");
        GameObject[] players = GameObject.FindGameObjectsWithTag("NetworkPlayer");
        foreach(var x in players){
            int currPlayerPid = x.GetComponent<CheckPlayerInput>().PID;
            if(currPlayerPid % 2 == 0){
                Debug.Log("setting 'X's scale to the percentage");
                x.GetComponent<CheckPlayerInput>().setScale(percentage);
            }

            
        }

    }

       

 


    }
