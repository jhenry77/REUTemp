using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.XR.CoreUtils.Bindings;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

using UnityEngine;
using Mirror;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using TMPro;

public class ButtonController : NetworkBehaviour
{

    public GameObject charadeText;

    public GameObject Confidence;
    public GameObject Questionaire;

    [HideInInspector]
    public bool showAnimation = false;
    [HideInInspector]
    public bool hideAnimation = false;
    [HideInInspector]
    public bool showConfidenceButtons = false;
    [HideInInspector]
    public bool hideConfidenceButtons = false;

    public GameObject[] myButtons;
    public GameObject[] confidenceButtons;
    public List<string> myQuestions = new List<string>();


    private List<string> myStrings = new List<string>();

    public enum questionPhase{
        Numbers1,Numbers2,Numbers3,Instruments1,Sports1
    }

    public questionPhase currentQuestionPhase;

    public int currentPhase = 0;

    
    



    



    // Start is called before the first frame update
    void Start()
    {
        myQuestions.Add("I liked the physical appearance\n of my virtual hands.");
        myQuestions.Add("My thoughts were cleat to my partner");
        myQuestions.Add("My partner's thoughts were clear to me.");
        myQuestions.Add("It was easy to understand my partner");
        myQuestions.Add("My partner found it easy to understand me");
        myQuestions.Add("Understanding my partner was difficult");
        myQuestions.Add("My partner had diffuclty understanding me");
        myStrings.Add("1");
        myStrings.Add("2");
        myStrings.Add("3");
        myStrings.Add("violin");
        myStrings.Add("guitar");
        myStrings.Add("Harp");
        myStrings.Add("Bowling");
        myStrings.Add("BaseBall");
        myStrings.Add("Fencing");
        ButtonTestScript thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
        thisButtonScript.setCorrect();
        
    }

    public void changeButtonName(questionPhase currentQuestionPhase){
        if(currentQuestionPhase == questionPhase.Numbers1){  
            for(int i = 0; i < 3; i++){
                ButtonTestScript thisButtonScript = myButtons[i].GetComponent<ButtonTestScript>();
                thisButtonScript.setTextName(myStrings[i].ToString());

            }
            changeCharadeText(myStrings[0]);
        }

        if(currentQuestionPhase == questionPhase.Instruments1){ 
            int x = 0; 
            for(int i = 3; i < 6; i++){
                ButtonTestScript thisButtonScript = myButtons[x].GetComponent<ButtonTestScript>();
                thisButtonScript.setTextName(myStrings[i].ToString());
                x++;

            }
            changeCharadeText(myStrings[3]);
        }
        if(currentQuestionPhase == questionPhase.Sports1){  
            int x = 0;
            for(int i = 6; i < 9; i++){
                ButtonTestScript thisButtonScript = myButtons[x].GetComponent<ButtonTestScript>();
                thisButtonScript.setTextName(myStrings[i].ToString());
                x++;

            }
            changeCharadeText(myStrings[6]);
        }

    }

    public void changePhase(int phase){
        if(phase == 0){
            currentQuestionPhase = questionPhase.Numbers1;
        }
        if(phase == 1){
            currentQuestionPhase = questionPhase.Instruments1;
        }
        if(phase == 2){
            currentQuestionPhase = questionPhase.Sports1;
        }
        if(phase == 3){
            startQuestionairephase();

        }
    }

    public void changeCharadeText(string newInput){
        TMP_Text myText = charadeText.GetComponent<TMP_Text>();
        myText.text = "Gesture:\n" + newInput;
    }

    public void setConfidence(string newInput){
        TMP_Text myText = Confidence.GetComponent<TMP_Text>();
        myText.text = "Confidence : " +  newInput;

    }

    public void startQuestionairephase(){
        foreach(var x in myButtons){
            x.SetActive(false);
        }
        Vector3 myPosition = Questionaire.transform.position;
        float bottomY = myPosition.y - .8f;
        myPosition.y = bottomY;
        Questionaire.SetActive(true);
        animateQuestionareUp();
    }

    public void animateQuestionareUp(){
        Debug.Log("animating up");
        Vector3 questionaireOrigLocation = Questionaire.transform.position;
        while(Questionaire.transform.position.y < questionaireOrigLocation.y + .8f){
            Vector3 myPosition = Questionaire.transform.position;
            myPosition.y += .005f;
            Questionaire.transform.position = myPosition;
        }

    }

    

   
  
    }

   
    
             

   
    
    



    
