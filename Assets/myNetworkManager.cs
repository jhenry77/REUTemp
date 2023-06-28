using UnityEngine;
using Mirror;


[AddComponentMenu("")]
    public class myNetworkManager : NetworkManager
    {
        public int numplayer;
        public GameObject serverCamera;
        public Transform player1Spawn;
        public Transform player2Spawn;
        
      public override void Start(){
        //player1Spawn.SetActive(false);
        //player2Spawn.SetActive(false);
        if (SystemInfo.operatingSystemFamily.ToString() != "Windows")
            {
                serverCamera.SetActive(false);
                Debug.Log("On Quest");
                StartClient();

            }else{
                Debug.Log("On PC");
                //serverCamera.SetActive(true);
            }
      }
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            numplayer = numPlayers;
            // add player at correct spawn position
            Debug.Log("Adding a player");
            Transform start = numPlayers == 0 ? player1Spawn : player2Spawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);

            
           
        }

    }
