using System.Collections;
using System.Collections.Generic;
// using Unity.Mathematics;
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
    public GameObject GuesserText;
    public GameObject CharadeText;
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
    public GameObject[] P2QuestionaireButtons;
    public List<string> myQuestions = new List<string>();

    public List<NumbersClass> myNumberQuestions = new List<NumbersClass>();
    public List<MediumQuestions> myMediumQuestions = new List<MediumQuestions>();
    public List<HardQuestions> myHardQuestions = new List<HardQuestions>();
    [SyncVar]
    public int randQuestion;
    [SyncVar]
    public int randOrder;


    public enum questionPhase{
        Easy,Medium,Hard,Questionaire
    }

    public questionPhase currentQuestionPhase;

    public int currentPhase = 0;
    public bool questionaireUp = false;
    public bool questionaireDown = false;
    public bool quesitonairePhase = false;
    public float max_height;
    public float min_height;
    public int questionaireNumberP1 = 0;
    public int questionaireNumberP2 = 0;
    public bool timeToSetScale = false;
    public SceneController mySceneController;
    public int numWaitingInQuestionaire = 0;
    public bool hideP1Confidence = false;
    public bool hideP2Confidence = false;
    

    
    [Client]
    void FixedUpdate(){
        if(questionaireUp){
            animateQuestionareUp();
            animateQuestionaiupServer();
            
        }
        if(questionaireDown){
            animateQuestionareDown();
            animateQuestionaiupServerDown();
        }
    }
    void Awake(){
    }

    


    // Start is called before the first frame update
    void Start()
    {
        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "5",
            incorrectAnswer1 = "2",
            incorrectAnswer2 = "9"
        });

        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "7",
            incorrectAnswer1 = "1",
            incorrectAnswer2 = "4"
        });

        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "3",
            incorrectAnswer1 = "8",
            incorrectAnswer2 = "6"
        });

        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Segway",
            incorrectAnswer1 = "Boat",
            incorrectAnswer2 = "HorseBack Riding"
        });

        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Tea",
            incorrectAnswer1 = "Soup",
            incorrectAnswer2 = "Burger"
        });

        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Secretary",
            incorrectAnswer1 = "Judge",
            incorrectAnswer2 = "Truck Driver"
        });

        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Table Tennis",
            incorrectAnswer1 = "Tennis",
            incorrectAnswer2 = "Baseball"
        });

        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Kayaking",
            incorrectAnswer1 = "Rowing",
            incorrectAnswer2 = "Swimming"
        });

        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Gambling",
            incorrectAnswer1 = "Winning a Race",
            incorrectAnswer2 = "Monopoly"
        });

        




        //myQuestions.Add("I liked the physical appearance\n of my virtual hands.");
        myQuestions.Add("My thoughts were cleat to my partner");
        myQuestions.Add("My partner's thoughts were clear to me.");
        myQuestions.Add("It was easy to understand my partner");
        myQuestions.Add("My partner found it easy to understand me");
        myQuestions.Add("Understanding my partner was difficult");
        myQuestions.Add("My partner had diffuclty understanding me");
        
        
        max_height = Questionaire.transform.position.y + .8f;
        min_height = Questionaire.transform.position.y;
        
    }

    public void setInitialNumbers(){
        Debug.Log("setting the button name");
        changeButtonName(questionPhase.Easy);
    }

    [Client]
    public void changeButtonName(questionPhase currentQuestionPhase){
        if(currentQuestionPhase == questionPhase.Easy){
            Debug.Log("setting a random easy question and current phase is " +currentPhase );
            getTwoRandom(currentQuestionPhase);
            Debug.Log("Random num is  " + randQuestion);
            
            Debug.Log("randQuestion order is " + randOrder);
            switch(randOrder){
                case 0:
                    ButtonTestScript thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameServer(0,myNumberQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(1,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(2,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    break;
                case 1:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameServer(0,myNumberQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(1,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(2,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    
                    break;
                case 2:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(0,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameServer(1,myNumberQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(2,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    break;
                case 3:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(0,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(1,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameServer(2,myNumberQuestions[randQuestion].correctAnswer, true);

                    break;
                case 4:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(0,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    changeButtonNameServer(1,myNumberQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(2,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    break;
                case 5:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(0,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameServer(1,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameServer(2,myNumberQuestions[randQuestion].correctAnswer, true);
                    break;

            }

            changeCharadeText(myNumberQuestions[randQuestion].correctAnswer);
            myNumberQuestions.RemoveAt(randQuestion);
            currentPhase++;



        }

        

        if(currentQuestionPhase == questionPhase.Medium){
            Debug.Log("setting a random medium quesitonand current phase is " +currentPhase );
            getTwoRandom(currentQuestionPhase);
            Debug.Log("Random num is  " + randQuestion);
            
            Debug.Log("randQuestion order is " + randOrder);
            switch(randOrder){
                case 0:
                    ButtonTestScript thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    break;
                case 1:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    
                    break;
                case 2:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    thisButtonScript.setIncorrect();
                    break;
                case 3:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    break;
                case 4:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    break;
                case 5:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    break;

            }
            changeCharadeText(myMediumQuestions[randQuestion].correctAnswer);
            myMediumQuestions.RemoveAt(randQuestion);
            currentPhase++;



        }
        if(currentQuestionPhase == questionPhase.Hard){
            Debug.Log("changing buttons to a random hard quesionand current phase is " +currentPhase );
            getTwoRandom(currentQuestionPhase);
            Debug.Log("Random num is  " + randQuestion);
           
            Debug.Log("randQuestion order is " + randOrder);
            switch(randOrder){
                case 0:
                    ButtonTestScript thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    break;
                case 1:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    
                    break;
                case 2:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    break;
                case 3:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    break;
                case 4:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    break;
                case 5:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    break;

            }
            changeCharadeText(myHardQuestions[randQuestion].correctAnswer);
            myHardQuestions.RemoveAt(randQuestion);
            currentPhase++;



        }

        

    }

    [Command(requiresAuthority = false)]
    public void changeButtonNameServer(int buttonNum, string buttonName, bool correct){
        ButtonTestScript myTestScript = myButtons[buttonNum].GetComponent<ButtonTestScript>();
        myTestScript.setTextName(buttonName);
        if(correct){
            myTestScript.setCorrect();
            changeCharadeText(buttonName);
        }else{
            myTestScript.setIncorrect();
        }
        
         
    }
    public void changePhase(int phase){
        if(phase == 0){
            //currentQuestionPhase = questionPhase.Numbers1;
            //startQuestionairephase();
            Debug.Log("setting quesiton phase to easy");
            currentQuestionPhase = questionPhase.Easy;
        }
        if(phase == 1){
            //currentQuestionPhase = questionPhase.Instruments1;
            currentQuestionPhase = questionPhase.Medium;
            Debug.Log("setting question phase to medium");
        }
        if(phase == 2){
            //currentQuestionPhase = questionPhase.Sports1;
            currentQuestionPhase = questionPhase.Hard;
            Debug.Log("Settin question phase to be hard");
        }
        if(phase == 3){
            currentQuestionPhase = questionPhase.Questionaire;
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
        searchForSceneController();
        quesitonairePhase = true;
        showConfidenceButtons = true;
        foreach(var x in myButtons){
            x.SetActive(false);
        }
        foreach(var x in P2QuestionaireButtons){
            x.SetActive(true);
        }
        Questionaire.SetActive(true);
        questionaireUp = true;
    
    }
    public void animateQuestionareUp(){
            Debug.Log("animating the questionaire up");
            if(Questionaire.transform.position.y < max_height){
            Vector3 myPosition = Questionaire.transform.position;
            myPosition.y += .001f;
            Questionaire.transform.position = myPosition;
            }else{
                questionaireUp = false;
            }
        

    }

    public void animateQuestionareDown(){
        Debug.Log("animating the questionaire down");
        if(Questionaire.transform.position.y > min_height){
            Vector3 myPosition = Questionaire.transform.position;
            myPosition.y -= .001f;
            Questionaire.transform.position = myPosition;
        }else{
            questionaireDown = false;
            
        }
    }

    public void changeQuestionaireTextPlayer1(string newInput){
       TMP_Text myText = GuesserText.GetComponent<TMP_Text>();
        myText.text = newInput;

    }
     public void changeQuestionaireTextPlayer2(string newInput){
        Debug.Log("setting player 2 text component");
       TMP_Text myText = CharadeText.GetComponent<TMP_Text>();
        myText.text = newInput;

    }
    
    public void changeQuestionairePhasePlayer1(int phase){
        if(questionaireNumberP1 == 2){
            Debug.Log("putting one in numWaiting questionaire");
            numWaitingInQuestionaire++;
            if(numWaitingInQuestionaire == 2){
                questionaireDown = true;
                hideP2Confidence = true;
                hideP1Confidence = true;
                resetToBeginning();

            }else{
            Debug.Log("Chaning the questionaire text player 1");
            changeQuestionaireTextPlayer1("Please wait for your partner to finish answering their questions");
            return;
            }
        }
        Debug.Log("changin it back to myquestions");
        changeQuestionaireTextPlayer1(myQuestions[phase]);
        questionaireNumberP1++;
        

    }
     public void changeQuestionairePhasePlayer2(int phase){
        if(questionaireNumberP2 == 2){
            numWaitingInQuestionaire++;
            if(numWaitingInQuestionaire == 2){
                questionaireDown = true;
                hideP1Confidence = true;
                hideP2Confidence = true;
                resetToBeginning();
                
            }else{
            Debug.Log("changing the player 2 text");
            changeQuestionaireTextPlayer2("Please wait for your partner to finish answering their questions");
            return;
            }
        }
        Debug.Log("changin it back to myquestions");
        changeQuestionaireTextPlayer2(myQuestions[phase]);
        questionaireNumberP2++;

    }

    [Command(requiresAuthority = false)]
     public void animateQuestionaiupServer(){
        animateQuestionareUp();
     
    }
    [Command(requiresAuthority = false)]
     public void animateQuestionaiupServerDown(){
        animateQuestionareDown();
     
    }

    [Client]
    public void searchForSceneController(){
        Debug.Log("searching for the scene controller");
        GameObject controllerObject = GameObject.FindGameObjectWithTag("SceneController");
        mySceneController = controllerObject.GetComponent<SceneController>();
        //searchForSceneControllerServer();
        
    }

    [Command(requiresAuthority = false)]
    public void searchForSceneControllerServer(){
        searchForSceneController();
    }

    public void resetToBeginning(){
        currentPhase = 0;
        numWaitingInQuestionaire = 0;
        questionaireNumberP1 = 0;
        questionaireNumberP2 = 0;
        foreach(var x in myButtons){
            x.SetActive(true);
        }
        showAnimation = true;
        changeButtonName(questionPhase.Easy);

    }
    [Command(requiresAuthority = false)]
    public void getTwoRandom(questionPhase currentQuestionPhase){
        if(currentQuestionPhase == questionPhase.Easy){
            randQuestion = Random.Range(0,myNumberQuestions.Count);
            randOrder = Random.Range(0,6);

        }
         if(currentQuestionPhase == questionPhase.Medium){
             randQuestion = Random.Range(0,myMediumQuestions.Count);
            randOrder = Random.Range(0,6);
            
        }
         if(currentQuestionPhase == questionPhase.Hard){
             randQuestion = Random.Range(0,myHardQuestions.Count);
            randOrder = Random.Range(0,6);
            
        }
        
    }


    

   
  
    }

   
    
             

   
    
    



    
