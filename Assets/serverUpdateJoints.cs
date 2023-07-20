using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class serverUpdateJoints : MonoBehaviour
{
    public List<GameObject> spheres;
    public List<Transform> joints;

    public int index = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //updateJoints();
        
    }
   

    void updateJoints(){
        for(int i = 0; i < joints.Count; i++){
            Debug.Log("Updating the server joints");
            joints[i].position = spheres[i + 2].transform.position;
            joints[i].up = spheres[i + 2].transform.forward;
        }

    }
}
