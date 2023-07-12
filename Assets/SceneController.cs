using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject Enviroment;
    public GameObject UISample;
    public GameObject MoveOnButton;
    public GameObject myMirror;
    public GameObject theTable;
    public GameObject opposingChair;
    public GameObject[] rayInteractors = new GameObject[5];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startCalibartion(){
        rayInteractors = GameObject.FindGameObjectsWithTag("RayInteractor");
        foreach(var x in rayInteractors){x.SetActive(false);}
        UISample.SetActive(false);
        myMirror.SetActive(true);
        Enviroment.SetActive(true);
        theTable.SetActive(false);
        opposingChair.SetActive(false);
        MoveOnButton.SetActive(true);
    }

    public void startPlayPhase(){
        myMirror.SetActive(false);
        theTable.SetActive(true);
        opposingChair.SetActive(true);

    }
}
