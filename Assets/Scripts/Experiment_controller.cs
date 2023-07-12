using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using TMPro;

public class Experiment_controller : MonoBehaviour
{
    

    public GameObject pidGO;
    public ParticipantData thisParticipant;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkPID(){
        int pid;
        string pidText = pidGO.GetComponent<TMP_InputField>().text;
         if (int.TryParse(pidText, out pid)) {
            thisParticipant.PID = pid;
            Debug.Log("our particpants PID is : " + pid);
                 
            SceneManager.LoadScene(1);
         }else{
            pidGO.GetComponent<TMP_InputField>().Select();
            pidGO.GetComponent<TMP_InputField>().text = "";
         }
    }

    public void loadMultiplayerScene(){
        var activeScene = SceneManager.GetActiveScene();
        
        GameObject myCurrPlayer;
        myCurrPlayer = GameObject.FindGameObjectWithTag("Player");
        //myCurrPlayer.SetActive(false);
        SceneManager.LoadScene(2);
    }
}
