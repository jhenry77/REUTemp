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
      public override void Start(){
        Debug.Log("Starting in network manager");
        if (SystemInfo.operatingSystemFamily.ToString() != "Windows")
            {
                
                Debug.Log("On Quest");
                StartClient();
    

            }else{
                Debug.Log("On PC");
               serverCamera.SetActive(true);
             }
      }
    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
        serverCamera.SetActive(true);
     }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            numplayer = numPlayers;
            // add player at correct spawn position
            Debug.Log("Adding a player in server add player");
            Transform start = numPlayers == 0 ? player1Spawn : player2Spawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            player.tag = "NetworkPlayer";
            NetworkServer.AddPlayerForConnection(conn, player);
             if(numplayer == 0){
             GameObject controller = Instantiate(spawnPrefabs[0], buttonLocaiton.position, buttonLocaiton.rotation);
             NetworkServer.Spawn(controller);
             }
            

            
           
        }

 


    }
