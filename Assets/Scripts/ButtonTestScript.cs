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
        Answers, Confidence, P2Confidence
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
    public bool timeForNewRandom = false;


    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.tag == "ConfidenceButtons"){
            thisButtonType = buttonInfoType.Confidence;
        }else if(gameObject.tag == "Button"){
            thisButtonType = buttonInfoType.Answers;
        }else if(gameObject.tag == "P2QuestionaireButtons"){
            thisButtonType = buttonInfoType.P2Confidence;
        }
        myController = myControllerObject.GetComponent<ButtonController>();
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
            animateDown();
            animateOnServerDown();
        }
        if(myController.showAnimation && thisButtonType == buttonInfoType.Answers){
            animateUp();
            animateOnServerUp();
        }

        if(myController.showConfidenceButtons && thisButtonType == buttonInfoType.Confidence){
            showConfidence();
            animateConfidenceServerup();

        }
        if(myController.hideConfidenceButtons && thisButtonType == buttonInfoType.Confidence){
            hideConfidence();
            animateConfidenceServerdown();

        }
        if(timeForNewRandom){
            StartCoroutine(myController.getRandomthenchangeButtonName());
            timeForNewRandom = false;
        }

        if(myController.hideP1Confidence && thisButtonType == buttonInfoType.Confidence){
            hideConfidenceQuestionaireP1();
            animateConfidenceQuestionaireServerP1();

        }
        if(myController.hideP2Confidence && thisButtonType == buttonInfoType.P2Confidence){
            hideConfidenceQuestionaireP2();
            animateConfidenceQuestionaireServerP2();

        }
        
    }

    [Command(requiresAuthority = false)]
    public void animateOnServerUp(){
        animateUp();

    }

    [Command(requiresAuthority = false)]
    public void animateOnServerDown(){
        animateDown();

    }
    [Command(requiresAuthority = false)]
    public void animateConfidenceServerup(){
        showConfidence();

    }
    [Command(requiresAuthority = false)]
    public void animateConfidenceServerdown(){
        hideConfidence();

    }
     [Command(requiresAuthority = false)]
    public void animateConfidenceQuestionaireServerP1(){
        hideConfidenceQuestionaireP1();

    }
    [Command(requiresAuthority = false)]
    public void animateConfidenceQuestionaireServerP2(){
        hideConfidenceQuestionaireP2();

    }

    public void press(){
        
        Debug.Log("calling press");
        if(thisButtonInfo == buttonInfo.Correct && thisButtonType == buttonInfoType.Answers){
            Debug.Log("Got it correct! setting thing to active");
            WinnerText.gameObject.SetActive(true);
            LoserText.gameObject.SetActive(false);
            myController.hideAnimation = true;
            
        }else if(thisButtonType == buttonInfoType.Answers){
            Debug.Log("Got it incorrect! setting things active");
            LoserText.gameObject.SetActive(true);
            WinnerText.gameObject.SetActive(false);
            myController.hideAnimation = true;
        }
    }
    public void animateDown(){
        if(gameObject.transform.position.y > min_height){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            myController.hideAnimation = false;
          
            myController.showConfidenceButtons = true;

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
            myController.showAnimation = false;
        }
    }

    public void showConfidence(){
         if(gameObject.transform.position.y < max_height){
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
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            

            if(myController.quesitonairePhase == false){
            Debug.Log("callingChangeButtonName");
            myController.changePhase(myController.currentPhase);
            myController.hideConfidenceButtons = false;
            myController.showAnimation = true;
            timeForNewRandom = true;
            }else{
                myController.hideConfidenceButtons = false;
                myController.showConfidenceButtons = true;
            }
            

        }

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
        if(myController.quesitonairePhase == true){
            confidence = 1;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 1;
        myController.setConfidence(confidence.ToString());
        myController.hideConfidenceButtons = true;
        }

    }
    public void pressConfidence2(){
         if(myController.quesitonairePhase == true){
            confidence = 2;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 2;
        myController.setConfidence(confidence.ToString());
        myController.hideConfidenceButtons = true;
        }

    }
    public void pressConfidence3(){
         if(myController.quesitonairePhase == true){
            confidence = 3;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 3;
        myController.setConfidence(confidence.ToString());
        myController.hideConfidenceButtons = true;
        }
    }
    public void pressConfidence4(){
         if(myController.quesitonairePhase == true){
            confidence = 4;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 4;
        myController.setConfidence(confidence.ToString());
        myController.hideConfidenceButtons = true;
        }
    }
    public void pressConfidence5(){
         if(myController.quesitonairePhase == true){
            confidence = 5;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 5;
        myController.setConfidence(confidence.ToString());
        myController.hideConfidenceButtons = true;
        }
    }
    public void pressConfidence6(){
         if(myController.quesitonairePhase == true){
            confidence = 6;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 6;
        myController.setConfidence(confidence.ToString());
        myController.hideConfidenceButtons = true;
        }
    }
    public void pressConfidence7(){
         if(myController.quesitonairePhase == true){
            confidence = 7;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 7;
        myController.setConfidence(confidence.ToString());
        myController.hideConfidenceButtons = true;
        }
    }
    public void pressConfidenceP21(){
        if(myController.quesitonairePhase == true){
            
            confidence = 1;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 1;
        myController.hideConfidenceButtons = true;
        }

    }
    public void pressConfidenceP22(){
         if(myController.quesitonairePhase == true){
            confidence = 2;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 2;
        myController.hideConfidenceButtons = true;
        }

    }
    public void pressConfidenceP23(){
         if(myController.quesitonairePhase == true){
            confidence = 3;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 3;
        myController.hideConfidenceButtons = true;
        }
    }
    public void pressConfidenceP24(){
         if(myController.quesitonairePhase == true){
            confidence = 4;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 4;
        myController.hideConfidenceButtons = true;
        }
    }
    public void pressConfidenceP25(){
         if(myController.quesitonairePhase == true){
            confidence = 5;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 5;
        myController.hideConfidenceButtons = true;
        }
    }
    public void pressConfidenceP26(){
         if(myController.quesitonairePhase == true){
            confidence = 6;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 6;
        myController.hideConfidenceButtons = true;
        }
    }
    public void pressConfidenceP27(){
         if(myController.quesitonairePhase == true){
            Debug.Log("set p2 confidence 2 and then setting the phase of player2");
            confidence = 7;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 7;
        myController.hideConfidenceButtons = true;
        }
    }

    

    public void hideConfidenceQuestionaireP1(){
         if(gameObject.transform.position.y > min_height){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            myController.hideP1Confidence = false;
    
        }

    }
    public void hideConfidenceQuestionaireP2(){
         if(gameObject.transform.position.y > min_height){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            myController.hideP1Confidence = false;
    
        }

    }

   
}
