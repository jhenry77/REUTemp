using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTestScript : MonoBehaviour
{
    float increment = 0.001F;
    double min_height = .9;
    double max_height =1.195; 
    public GameObject WinnerText;
    public GameObject LoserText;
    public enum buttonInfo{
        Correct,Incorrect
    }
    bool correctSelection = false;
    
    bool showAnimation = false;

    bool hideAnimation = false;

    buttonInfo thisButtonInfo =buttonInfo.Incorrect;

    public Vector3 initialLocation;
    public Quaternion initialRotation;

    public GameObject myControllerObject;

    private TestController myController;

    


    // Start is called before the first frame update
    void Start()
    {
        myController = myControllerObject.GetComponent<TestController>();
        initialLocation = gameObject.transform.position;
        initialRotation = gameObject.transform.rotation;
        WinnerText.gameObject.SetActive(false);
        LoserText.gameObject.SetActive(false);
        if(gameObject.name.Contains("Lion")){
            thisButtonInfo = buttonInfo.Correct;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(myController.hideAnimation){
            animateDown();
        }
        if(myController.showAnimation){
            animateUp();
        }
        
    }

    public void press(){
        if(thisButtonInfo == buttonInfo.Correct){
            WinnerText.gameObject.SetActive(true);
            LoserText.gameObject.SetActive(false);
            correctSelection = true;
            myController.hideAnimation = true;
            
        }else{
            LoserText.gameObject.SetActive(true);
            WinnerText.gameObject.SetActive(false);
        }
    }

    public void animateDown(){
        if(gameObject.transform.position.y > min_height){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            correctSelection = false;
            myController.hideAnimation = false;
            myController.showAnimation = true;
        }
    }

     public void animateUp(){
        if(gameObject.transform.position.y < max_height){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y + increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            gameObject.transform.SetPositionAndRotation(initialLocation,initialRotation);
            correctSelection = false;
            myController.showAnimation = false;
        }
    }

    public void setActive(){
        gameObject.SetActive(true);
    }

    public void setInactive(){
        gameObject.SetActive(false);
    }

    public void setCorrect(){
        thisButtonInfo = buttonInfo.Correct;
    }

    public void setIncorrect(){
        thisButtonInfo = buttonInfo.Incorrect;
    }
}
