using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using TMPro;

public class ButtonTestScript : NetworkBehaviour
{
    
    float increment = 0.001F;
  
    public GameObject WinnerText;
    public GameObject LoserText;
    public enum buttonInfo{
        Correct,Incorrect
    }
    public enum buttonInfoType{
        Answers, Confidence
    }
    bool correctSelection = false;
    

    buttonInfo thisButtonInfo =buttonInfo.Incorrect;
    buttonInfoType thisButtonType;

    public Vector3 initialLocation;
    public Quaternion initialRotation;

    public GameObject myControllerObject;

    private ButtonController myController;

    double min_height;
    double max_height; 
    public int currPhase = 0;
    public int confidence;
    public Vector3 maxHeight;


    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.tag == "ConfidenceButtons"){
            thisButtonType = buttonInfoType.Confidence;
        }else if(gameObject.tag == "Button"){
            thisButtonType = buttonInfoType.Answers;
        }
        myController = myControllerObject.GetComponent<ButtonController>();
        currPhase = myController.currentPhase;
        initialLocation = gameObject.transform.position;
        initialRotation = gameObject.transform.rotation;
        maxHeight = initialLocation;
        if(gameObject.tag == "ConfidenceButtons"){
            max_height = initialLocation.y + .4f;
            min_height = initialLocation.y;
            maxHeight.y = (float)max_height;
        }
        if(gameObject.tag == "Button"){
            max_height = initialLocation.y;
            min_height = initialLocation.y - .4f;
        
        }
        WinnerText.gameObject.SetActive(false);
        LoserText.gameObject.SetActive(false);
        
    }
    [Client]
    // Update is called once per frame
    void FixedUpdate()
    {
        if(myController.hideAnimation && thisButtonType == buttonInfoType.Answers){
            Debug.Log("hidinganswers");
            animateDown();
            animateOnServerDown();
        }
        if(myController.showAnimation && thisButtonType == buttonInfoType.Answers){
             Debug.Log("showinganswers");
            animateUp();
            animateOnServerUp();
        }

        if(myController.showConfidenceButtons && thisButtonType == buttonInfoType.Confidence){
             Debug.Log("showingConfidence");
            showConfidence();
            animateConfidenceServerup();

        }
        if(myController.hideConfidenceButtons && thisButtonType == buttonInfoType.Confidence){
             Debug.Log("hidingConfidence");
            hideConfidence();
            animateConfidenceServerdown();

        }
        
    }

    [Command(requiresAuthority = false)]
    public void animateOnServerUp(){
        Debug.Log("calling animate up on server");
        animateUp();

    }

    [Command(requiresAuthority = false)]
    public void animateOnServerDown(){
        Debug.Log("calling animate down on server");
        animateDown();

    }
    [Command(requiresAuthority = false)]
    public void animateConfidenceServerup(){
        Debug.Log("calling animate up on server");
        showConfidence();

    }
    [Command(requiresAuthority = false)]
    public void animateConfidenceServerdown(){
        Debug.Log("calling animate up on server");
        hideConfidence();

    }

    public void press(){
        
        Debug.Log("calling press");
        if(thisButtonInfo == buttonInfo.Correct && thisButtonType == buttonInfoType.Answers){
            WinnerText.gameObject.SetActive(true);
            LoserText.gameObject.SetActive(false);
            myController.hideAnimation = true;
            
        }else if(thisButtonType == buttonInfoType.Answers){
            LoserText.gameObject.SetActive(true);
            WinnerText.gameObject.SetActive(false);
            myController.hideAnimation = true;
        }
    }

    public void animateDown(){
        if(gameObject.transform.position.y > min_height){
            Debug.Log("animating down;");
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            Debug.Log("animated all the way down");
            // myController.changePhase(currPhase);
            myController.changeButtonName(myController.currentQuestionPhase);
            myController.hideAnimation = false;
            Debug.Log("just set showConfidence to true");
          
            myController.showConfidenceButtons = true;
            currPhase++;
        }
    }

     public void animateUp(){
        if(gameObject.transform.position.y < max_height){
            Debug.Log("animating up");
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y + increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            gameObject.transform.SetPositionAndRotation(initialLocation,initialRotation);
            myController.showAnimation = false;
        }
    }

    public void showConfidence(){
         if(gameObject.transform.position.y < max_height){
            Debug.Log("animating up");
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y + increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            gameObject.transform.SetPositionAndRotation(maxHeight,initialRotation);
            myController.showConfidenceButtons = false;
        }

    }

    public void hideConfidence(){
         if(gameObject.transform.position.y > min_height){
            Debug.Log("animating down;");
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            Debug.Log("animated all the way down");
            myController.hideConfidenceButtons = false;
            myController.showAnimation = true;
            myController.changePhase(currPhase);
            currPhase++;

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

    public void setTextName(string changed){
        TMP_Text myText = gameObject.GetComponentInChildren<TMP_Text>();
        myText.text = changed;
    }


    public void pressConfidence1(){
        confidence = 1;
        myController.setConfidence(confidence.ToString());
        myController.hideConfidenceButtons = true;

    }
    public void pressConfidence2(){
        confidence = 2;
        myController.setConfidence(confidence.ToString());
         myController.hideConfidenceButtons = true;

    }
    public void pressConfidence3(){
        confidence = 3;
        myController.setConfidence(confidence.ToString());
         myController.hideConfidenceButtons = true;
    }
    public void pressConfidence4(){
        confidence = 4;
        myController.setConfidence(confidence.ToString());
        myController.hideConfidenceButtons = true;
    }
    public void pressConfidence5(){
        confidence = 5;
        myController.setConfidence(confidence.ToString());
         myController.hideConfidenceButtons = true;
    }
    public void pressConfidence6(){
        confidence = 6;
        myController.setConfidence(confidence.ToString());
         myController.hideConfidenceButtons = true;
    }
    public void pressConfidence7(){
        confidence = 7;
        myController.setConfidence(confidence.ToString());
         myController.hideConfidenceButtons = true;
    }

   
}
