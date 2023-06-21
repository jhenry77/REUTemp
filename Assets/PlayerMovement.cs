using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Vector3 movement = new Vector3();

    [Client]
    private void Update(){

        if(!isOwned){
            return;
        }
        if(!Input.GetKeyDown(KeyCode.Space)){
            return;
        }
        transform.Translate(movement);
    }
}
