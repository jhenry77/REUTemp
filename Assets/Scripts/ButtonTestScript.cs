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
        if(gameObject.tag == "ConfidenceButtons" || gameObject.tag == "P2QuestionaireButtons"){
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
    [Server]
    // Update is called once per frame
    void FixedUpdate()
    {
        if(myController.hideAnimation && thisButtonType == buttonInfoType.Answers){
            animateDown();
            hideGesture();
        }
        if(myController.showAnimation && thisButtonType == buttonInfoType.Answers){
            hideText();
            animateUp();
        }

        if(myController.showConfidenceButtons && thisButtonType == buttonInfoType.Confidence){
            showConfidence();

        }
        if(myController.hideConfidenceButtons && thisButtonType == buttonInfoType.Confidence){
            hideConfidence();
            showText(myController.gotCorrect);
            

        }
        

        if(myController.showConfidenceButtonsP2 && thisButtonType == buttonInfoType.P2Confidence){
            Debug.Log("calling show confidence P2");
            showConfidenceP2();
        }
        if(timeForNewRandom){
            Debug.Log("In time for new random");
            StartCoroutine(myController.getRandomthenchangeButtonName());
            timeForNewRandom = false;
        }

        if(myController.hideP1Confidence && thisButtonType == buttonInfoType.Confidence){
            hideConfidenceQuestionaireP1();

        }
        if(myController.hideP2Confidence && thisButtonType == buttonInfoType.P2Confidence){
            Debug.Log("hiding p2 Confidence");
            hideConfidenceQuestionaireP2();

        }
        
    }
    
    [ClientRpc]
    public void hideGesture(){
        myController.charadeText.SetActive(false);
        
    }
    [ClientRpc]
    public void showGesture(){
        myController.charadeText.SetActive(true);
        
    }
    
    [ClientRpc]
    public void showText(bool showCorrect){
        if(showCorrect){
            WinnerText.SetActive(true);
        }else if(!showCorrect){
            LoserText.SetActive(true);
        }
        
    }

    [ClientRpc]
    public void hideText(){
            WinnerText.SetActive(false);
            LoserText.SetActive(false);
        
        
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
    [Client]
    public void press(){
        
        Debug.Log("calling press");
        if(thisButtonInfo == buttonInfo.Correct && thisButtonType == buttonInfoType.Answers){
            // Debug.Log("Got it correct! setting thing to active");
            setGotCorrect();
            // myController.hideAnimation = true;
            serverSetAnimateDownTrue();
            
            
        }else if(thisButtonType == buttonInfoType.Answers){
            // Debug.Log("Got it incorrect! setting things active");
            setGotIncorrect();
            // myController.hideAnimation = true;
            serverSetAnimateDownTrue();
        }
    }
    [Server]
    public void pressOnServer(){
        Debug.Log("setting end interavl time now is " + Time.time);
        myController.dataEndInterval.Add((Time.time.ToString()));
        string pressedButton = gameObject.GetComponentInChildren<TMP_Text>().text;
        Debug.Log("adding a button to button chosen name");
        myController.dataButtonChosenName.Add(pressedButton);
        if(thisButtonInfo == buttonInfo.Correct && thisButtonType == buttonInfoType.Answers){
            Debug.Log("got it correct");
            myController.dataChoseCorrect.Add("True");
        }else if(thisButtonType == buttonInfoType.Answers && thisButtonInfo == buttonInfo.Incorrect){
            Debug.Log("got it incorrect");
            myController.dataChoseCorrect.Add("False");
        }
        
        
    }
    [Command(requiresAuthority = false)]
    public void setGotCorrect(){
        myController.gotCorrect = true;

    }
    [Command(requiresAuthority = false)]
    public void setGotIncorrect(){
        myController.gotIncorrect = true;
    }
    [ClientRpc]
    public void setSafteyTextOn(){
        myController.SafetyText.SetActive(true);

    }
    [ClientRpc]
    public void setSafteyTextoff(){
        myController.SafetyText.SetActive(false);
    }



    
    public void animateDown(){
        if(gameObject.transform.position.y > min_height){
            setSafteyTextOn();
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
            // Debug.Log("animating the orig buttons down");
        }else{
            myController.hideAnimation = false;
            // serverSetAnimateDownFalse();
            // Debug.Log("setting myController to show the confidence buttons");
            myController.showConfidenceButtons = true;
            // serverSetAnimateConfidenceUpTrue();
            // Debug.Log("show confidence buttons is : " + myController.showConfidenceButtons.ToString());

        }
    }
     public void animateUp(){
        if(gameObject.transform.position.y < max_height){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y + increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            Debug.Log("setting start interval");
            myController.dataStartInterval.Add((Time.time.ToString()));
            setSafteyTextoff();
            gameObject.transform.SetPositionAndRotation(initialLocation,initialRotation);
            myController.showAnimation = false;
            showGesture();
            // serverSetAnimateUpFalse();
        }
    }
    public void showConfidence(){
         if(gameObject.transform.position.y < max_height){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y + increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            setSafteyTextoff();
            gameObject.transform.SetPositionAndRotation(maxHeight,initialRotation);
            myController.showConfidenceButtons = false;
            // serverSetAnimateConfidenceUpFalse();
        }

    }
    public void hideConfidence(){
         if(gameObject.transform.position.y > min_height){
            setSafteyTextOn();
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{

            

            if(myController.quesitonairePhase == false){
            Debug.Log("calling change phase in hide and it is: " +myController.currentPhase);
            myController.changePhase(myController.currentPhase);
            myController.hideConfidenceButtons = false;
            if(myController.quesitonairePhase == false){
            timeForNewRandom = true;
            myController.showAnimation = true;
            }
            //serverSetAnimateConfidenceDownFalse();
           
            //serverSetAnimateUpTrue();
            
            }else{
                // myController.hideConfidenceButtons = false;
                //serverSetAnimateConfidenceDownFalse();
                // myController.showConfidenceButtons = true;
                //serverSetAnimateConfidenceUpTrue();
            }
            

        }

    }
    
    public void showConfidenceP2(){
         if(gameObject.transform.position.y < max_height){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y + increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            gameObject.transform.SetPositionAndRotation(maxHeight,initialRotation);
            myController.showConfidenceButtonsP2 = false;
            // serverSetAnimateConfidenceUpFalse();
        }

    }
    public void hideConfidenceP2(){
         if(gameObject.transform.position.y > min_height){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            myController.hideConfidenceButtonsP2 = false;

         
            

        }

    }

    public void hideConfidenceP1QuestionairePhase(){
         if(gameObject.transform.position.y > min_height){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y - increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{

        }

    }





    [Command(requiresAuthority = false)]
    public void setServerPhase(int phase){

    }

    [Command(requiresAuthority = false)]
    public void serverSetAnimateDownTrue(){
        myController.hideAnimation = true;

    }

     [Command(requiresAuthority = false)]
    public void serverSetAnimateDownFalse(){
        myController.hideAnimation = false;
        
    }
    [Command(requiresAuthority = false)]
    public void serverSetAnimateUpTrue(){
        myController.showAnimation = true;

    }

     [Command(requiresAuthority = false)]
    public void serverSetAnimateUpFalse(){
        myController.showAnimation = false;
        
    }

    [Command(requiresAuthority = false)]
    public void serverSetAnimateConfidenceDownTrue(){
        myController.hideConfidenceButtons = true;

    }

     [Command(requiresAuthority = false)]
    public void serverSetAnimateConfidenceDownFalse(){
        myController.hideConfidenceButtons = false;
        
    }
    [Command(requiresAuthority = false)]
    public void serverSetAnimateConfidenceUpTrue(){
        myController.showConfidenceButtons = true;

    }

     [Command(requiresAuthority = false)]
    public void serverSetAnimateConfidenceUpFalse(){
        myController.showConfidenceButtons = false;
        
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

    [Client]
    public void pressConfidence1(){
        if(myController.quesitonairePhase == true){
            confidence = 1;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 1;
        myController.setConfidence(confidence.ToString());
        serverSetAnimateConfidenceDownTrue();
        // myController.hideConfidenceButtons = true;
        }

    }

    [Client]
    public void pressConfidence2(){
         if(myController.quesitonairePhase == true){
            confidence = 2;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 2;
        myController.setConfidence(confidence.ToString());
        // myController.hideConfidenceButtons = true;
        serverSetAnimateConfidenceDownTrue();

        }

    }
    [Client]
    public void pressConfidence3(){
         if(myController.quesitonairePhase == true){
            confidence = 3;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 3;
        myController.setConfidence(confidence.ToString());
        // myController.hideConfidenceButtons = true;
        serverSetAnimateConfidenceDownTrue();

        }
    }
    [Client]
    public void pressConfidence4(){
         if(myController.quesitonairePhase == true){
            confidence = 4;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 4;
        myController.setConfidence(confidence.ToString());
        // myController.hideConfidenceButtons = true;
        serverSetAnimateConfidenceDownTrue();

        }
    }
    [Client]
    public void pressConfidence5(){
         if(myController.quesitonairePhase == true){
            confidence = 5;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 5;
        myController.setConfidence(confidence.ToString());
        // myController.hideConfidenceButtons = true;
        serverSetAnimateConfidenceDownTrue();

        }
    }
    [Client]
    public void pressConfidence6(){
         if(myController.quesitonairePhase == true){
            confidence = 6;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 6;
        myController.setConfidence(confidence.ToString());
        // myController.hideConfidenceButtons = true;
        serverSetAnimateConfidenceDownTrue();

        }
    }
    [Client]
    public void pressConfidence7(){
         if(myController.quesitonairePhase == true){
            confidence = 7;
            myController.changeQuestionairePhasePlayer1(myController.questionaireNumberP1);
        }else{
        confidence = 7;
        myController.setConfidence(confidence.ToString());
        // myController.hideConfidenceButtons = true;
        serverSetAnimateConfidenceDownTrue();

        }
    }
     [Server]
    public void pressConfidence1Server(){
       Debug.Log("PressedConfidence1Button");
       myController.dataconfidenceInt.Add("1");

    }

    [Server]
    public void pressConfidence2Server(){
        Debug.Log("PressedConfidence2Button");
        myController.dataconfidenceInt.Add("2");


    }
    [Server]
    public void pressConfidence3Server(){
      Debug.Log("PressedConfidence3Button");
      myController.dataconfidenceInt.Add("3");

    }
    [Server]
    public void pressConfidence4Server(){
      Debug.Log("PressedConfidence4Button");
      myController.dataconfidenceInt.Add("4");

    }
    [Server]
    public void pressConfidence5Server(){
        Debug.Log("PressedConfidence5Button");
        myController.dataconfidenceInt.Add("5");

    }
    [Server]
    public void pressConfidence6Server(){
         Debug.Log("PressedConfidence6Button");
         myController.dataconfidenceInt.Add("6");

    }
    [Server]
    public void pressConfidence7Server(){
        Debug.Log("PressedConfidence7Button");
        myController.dataconfidenceInt.Add("7");

    }
    [Client]
    public void pressConfidenceP21(){
        if(myController.quesitonairePhase == true){
            
            confidence = 1;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 1;
        myController.hideConfidenceButtons = true;
        }

    }
    [Client]
    public void pressConfidenceP22(){
         if(myController.quesitonairePhase == true){
            confidence = 2;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 2;
        myController.hideConfidenceButtons = true;
        }

    }
    [Client]
    public void pressConfidenceP23(){
         if(myController.quesitonairePhase == true){
            confidence = 3;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 3;
        myController.hideConfidenceButtons = true;
        }
    }
    [Client]
    public void pressConfidenceP24(){
         if(myController.quesitonairePhase == true){
            confidence = 4;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 4;
        myController.hideConfidenceButtons = true;
        }
    }
    [Client]
    public void pressConfidenceP25(){
         if(myController.quesitonairePhase == true){
            confidence = 5;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 5;
        myController.hideConfidenceButtons = true;
        }
    }
    [Client]
    public void pressConfidenceP26(){
         if(myController.quesitonairePhase == true){
            confidence = 6;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 6;
        myController.hideConfidenceButtons = true;
        }
    }
    [Client]
    public void pressConfidenceP27(){
         if(myController.quesitonairePhase == true){
            // Debug.Log("set p2 confidence 2 and then setting the phase of player2");
            confidence = 7;
            myController.changeQuestionairePhasePlayer2(myController.questionaireNumberP2);
        }else{
        confidence = 7;
        myController.hideConfidenceButtons = true;
        }
    }
    [Server]
    public void pressConfidenceP21Server(){
       Debug.Log("P2 pressed button1 ");

    }
    [Server]
    public void pressConfidenceP22Server(){
       Debug.Log("P2 pressed button2 ");

    }
    [Server]
    public void pressConfidenceP23Server(){
        Debug.Log("P2 pressed button3");
    }
    [Server]
    public void pressConfidenceP24Server(){
        Debug.Log("P2 pressed button4 ");
    }
    [Server]
    public void pressConfidenceP25Server(){
         Debug.Log("P2 pressed button5 ");
    }
    [Server]
    public void pressConfidenceP26Server(){
         Debug.Log("P2 pressed button6 ");
    }
    [Server]
    public void pressConfidenceP27Server(){
        Debug.Log("P2 pressed button7 ");
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
            myController.hideP2Confidence = false;
    
        }

    }

   
}
