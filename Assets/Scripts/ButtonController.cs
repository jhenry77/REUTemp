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
    public GameObject TFPText;

    public GameObject charadeText;

    public GameObject Confidence;
    public GameObject GuesserText;
    public GameObject CharadeText;
    public GameObject Questionaire;

    [HideInInspector]
    [SyncVar]
    public bool showAnimation = false;
    [HideInInspector]
    [SyncVar]
    public bool hideAnimation = false;
    [HideInInspector]
    [SyncVar]
    public bool showConfidenceButtons = false;
    [HideInInspector]
    [SyncVar]
    public bool hideConfidenceButtons = false;
    [HideInInspector]
    [SyncVar]
    public bool hideConfidenceButtonsP2 = false;
    [HideInInspector]
    [SyncVar]
    public bool showConfidenceButtonsP2 = false;

    public GameObject[] myButtons;
    public GameObject[] confidenceButtons;
    public GameObject[] P2QuestionaireButtons;
    public List<string> myQuestions = new List<string>();

    public List<NumbersClass> myNumberQuestions = new List<NumbersClass>();
    public List<MediumQuestions> myMediumQuestions = new List<MediumQuestions>();
    public List<HardQuestions> myHardQuestions = new List<HardQuestions>();
    [SyncVar(hook = "RandQuestionChanged")]
    public int randQuestion;
    [SyncVar(hook = "randOrderChanged")]
    public int randOrder = 0;


    public enum questionPhase{
        Easy,Medium,Hard,Questionaire
    }
    [SyncVar]
    public questionPhase currentQuestionPhase = questionPhase.Easy;

    public int currentPhase = 0;
    public bool questionaireUp = false;
    public bool questionaireDown = false;
    [SyncVar]
    public bool quesitonairePhase = false;
    public float max_height;
    public float min_height;
    public int questionaireNumberP1 = 0;
    public int questionaireNumberP2 = 0;
    public bool timeToSetScale = false;
    public SceneController mySceneController;
    [SyncVar]
    public int numWaitingInQuestionaire = 0;
    public bool hideP1Confidence = false;
    public bool hideP2Confidence = false;
    [SyncVar(hook = "itterateServerCall")]
    public int onlyCallOnce = 0;
    public int questionaireNumSeen = 0;


    
    public void itterateServerCall(int oldVal, int newVal){
        // Debug.Log("itterating server call on all clients");
        onlyCallOnce = newVal;
    }
    [Command(requiresAuthority = false)]
    public void itterateOnlyCallOnce(){
        // Debug.Log("itterating only call once on server");
        onlyCallOnce++;
    }
    
        
    


    public void RandQuestionChanged(int oldVal, int newVal){
        randQuestion = newVal;
        Debug.Log("Hook called randQuesiton is " + randQuestion);
    }
    public void randOrderChanged(int oldVal, int newVal){
        randOrder = newVal;
        // Debug.Log("Hook called randOrder is:  " + randOrder);
    }

    public void CMDgetRandomQuestion(int low, int high){
        // Debug.Log("setting a new random number for qestions!");
        randQuestion = Random.Range(low,high);
        // Debug.Log("rand Quesiton was: " + randQuestion);

    }
    public void CMDgetRandomOrder(int low, int high){
        // Debug.Log("setting a new random order!");
        randOrder = Random.Range(low,high);
        // Debug.Log("rand Order was: " + randOrder);

    }

    

    void FixedUpdate(){
        if(questionaireUp){
            animateQuestionareUp();
            
        }
        if(questionaireDown){
            animateQuestionareDown();
        }
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
        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "5",
            incorrectAnswer1 = "3",
            incorrectAnswer2 = "2"
        });
        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "7",
            incorrectAnswer1 = "2",
            incorrectAnswer2 = "9"
        });
        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "4",
            incorrectAnswer1 = "1",
            incorrectAnswer2 = "2"
        });
        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "9",
            incorrectAnswer1 = "3",
            incorrectAnswer2 = "5"
        });
        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "6",
            incorrectAnswer1 = "4",
            incorrectAnswer2 = "1"
        });
        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "8",
            incorrectAnswer1 = "7",
            incorrectAnswer2 = "6"
        });
        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "1",
            incorrectAnswer1 = "5",
            incorrectAnswer2 = "3"
        });
        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "9",
            incorrectAnswer1 = "3",
            incorrectAnswer2 = "7"
        });
        myNumberQuestions.Add(new NumbersClass{
            correctAnswer = "7",
            incorrectAnswer1 = "9",
            incorrectAnswer2 = "4"
        });

        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Motorcycle",
            incorrectAnswer1 = "Plane",
            incorrectAnswer2 = "Car"
        });

        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Football",
            incorrectAnswer1 = "Baseball",
            incorrectAnswer2 = "Volleyball"
        });

        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Swimming",
            incorrectAnswer1 = "Kayaking",
            incorrectAnswer2 = "Pitcher"
        });
        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Golf",
            incorrectAnswer1 = "Baseball",
            incorrectAnswer2 = "Basketball"
        });
        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Diving",
            incorrectAnswer1 = "Swimming",
            incorrectAnswer2 = "Kayaking"
        });
        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Ice Skaing",
            incorrectAnswer1 = "Bobsledding",
            incorrectAnswer2 = "Climbing"
        });
        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Baseball",
            incorrectAnswer1 = "Tennis",
            incorrectAnswer2 = "Golf"
        });
        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Boxing",
            incorrectAnswer1 = "Karate",
            incorrectAnswer2 = "Weight lifting"
        });
        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Violin",
            incorrectAnswer1 = "Cello",
            incorrectAnswer2 = "Guitar"
        });
        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Clarinet",
            incorrectAnswer1 = "Trumpet",
            incorrectAnswer2 = "Flute"
        });
        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Ukelele",
            incorrectAnswer1 = "Guitar",
            incorrectAnswer2 = "Triangle"
        });
        myMediumQuestions.Add(new MediumQuestions{
            correctAnswer = "Harmonica",
            incorrectAnswer1 = "Trombone",
            incorrectAnswer2 = "Kazoo"
        });


        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Segway",
            incorrectAnswer1 = "Boat",
            incorrectAnswer2 = "HorseBack\nRiding"
        });

        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Softball",
            incorrectAnswer1 = "Baseball",
            incorrectAnswer2 = "Football"
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
            correctAnswer = "Snowboarding",
            incorrectAnswer1 = "Skiing",
            incorrectAnswer2 = "Gymnastics"
        });
        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Sloth",
            incorrectAnswer1 = "Rhino",
            incorrectAnswer2 = "Swan"
        });
        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Frog",
            incorrectAnswer1 = "Rabbit",
            incorrectAnswer2 = "Mouse"
        });
        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Steak",
            incorrectAnswer1 = "Lasagna",
            incorrectAnswer2 = "Spaghetti"
        });
        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Secretary",
            incorrectAnswer1 = "Truck Driver",
            incorrectAnswer2 = "Judge"
        });
        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Nurse",
            incorrectAnswer1 = "Flight\nAttendant",
            incorrectAnswer2 = "Construction\nWorker"
        });
        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Gambling",
            incorrectAnswer1 = "Winning a Race",
            incorrectAnswer2 = "Monopoly"
        });
        myHardQuestions.Add(new HardQuestions{
            correctAnswer = "Indiana Jones",
            incorrectAnswer1 = "The Office",
            incorrectAnswer2 = "Black Mirror"
        });
        

        




        myQuestions.Add("I liked the physical appearance\n of my virtual hands.");
        myQuestions.Add("My thoughts were cleat to my partner");
        myQuestions.Add("My partner's thoughts were clear to me.");
        myQuestions.Add("It was easy to understand my partner");
        myQuestions.Add("My partner found it easy to understand me");
        myQuestions.Add("Understanding my partner was difficult");
        myQuestions.Add("My partner had diffuclty understanding me");
        
        
        max_height = Questionaire.transform.position.y + .8f;
        min_height = Questionaire.transform.position.y;
        // Debug.Log("just finished start");
        
    }
    
    public void setInitialNumbers(){
        // Debug.Log("setting the button name");
        StartCoroutine(getRandomthenchangeButtonName());
    }
    
    public IEnumerator getRandomthenchangeButtonName(){
        yield return new WaitForSeconds(1);
        // Debug.Log("mynumber qestions length is : " + myNumberQuestions.Count);
        if(currentQuestionPhase == questionPhase.Easy){
            // Debug.Log("Getting random question number for easy");
            CMDgetRandomQuestion(0,myNumberQuestions.Count);
        }else if(currentQuestionPhase == questionPhase.Medium){
            //  Debug.Log("Getting random question number for Medium");
            CMDgetRandomQuestion(0,myMediumQuestions.Count);
        }else if(currentQuestionPhase == questionPhase.Hard){
            //  Debug.Log("Getting random question number for Hard");
            CMDgetRandomQuestion(0,myHardQuestions.Count);
        }
        CMDgetRandomOrder(0,6);
        yield return new WaitForSeconds(1);
        // Debug.Log("calling chagne button name");
        changeButtonName(currentQuestionPhase);
        
    }

    [Server]
    public void changeButtonName(questionPhase currentQuestionPhase){
        if(currentQuestionPhase == questionPhase.Easy){
            // Debug.Log("setting a random easy question and current phase is " +currentPhase );
            //CMDgetRandom(0,myNumberQuestions.Count);
            // Debug.Log("Random num is  " + randQuestion);
            
            // Debug.Log("randQuestion order is " + randOrder);
            switch(randOrder){
                case 0:
                    ButtonTestScript thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(0,myNumberQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    break;
                case 1:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(0,myNumberQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    
                    break;
                case 2:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(1,myNumberQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    break;
                case 3:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(2,myNumberQuestions[randQuestion].correctAnswer, true);

                    break;
                case 4:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    changeButtonNameClient(1,myNumberQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    break;
                case 5:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myNumberQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myNumberQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myNumberQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(2,myNumberQuestions[randQuestion].correctAnswer, true);
                    break;

            }

            changeCharadeText(myNumberQuestions[randQuestion].correctAnswer);
            myNumberQuestions.RemoveAt(randQuestion);
            currentPhase++;



        }

        

        if(currentQuestionPhase == questionPhase.Medium){
            Debug.Log("setting a random medium quesitonand current phase is " +currentPhase );
            Debug.Log("Random num is  " + randQuestion);
            
            Debug.Log("randQuestion order is " + randOrder);
            switch(randOrder){
                case 0:
                    ButtonTestScript thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(0,myMediumQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myMediumQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myMediumQuestions[randQuestion].incorrectAnswer2, false);
                    break;
                case 1:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(0,myMediumQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myMediumQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myMediumQuestions[randQuestion].incorrectAnswer1, false);
                    
                    break;
                case 2:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myMediumQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(1,myMediumQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myMediumQuestions[randQuestion].incorrectAnswer2, false);
                    break;
                case 3:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myMediumQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myMediumQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(2,myMediumQuestions[randQuestion].correctAnswer, true);

                    break;
                case 4:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myMediumQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    changeButtonNameClient(1,myMediumQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myMediumQuestions[randQuestion].incorrectAnswer1, false);
                    break;
                case 5:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myMediumQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myMediumQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myMediumQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(2,myMediumQuestions[randQuestion].correctAnswer, true);
                    break;

            }
            changeCharadeText(myMediumQuestions[randQuestion].correctAnswer);
            myMediumQuestions.RemoveAt(randQuestion);
            currentPhase++;



        }
        if(currentQuestionPhase == questionPhase.Hard){
            Debug.Log("changing buttons to a random hard quesionand current phase is " +currentPhase );
            Debug.Log("Random num is  " + randQuestion);
           
            Debug.Log("randQuestion order is " + randOrder);
            switch(randOrder){
                case 0:
                    ButtonTestScript thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(0,myHardQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myHardQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myHardQuestions[randQuestion].incorrectAnswer2, false);
                    break;
                case 1:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(0,myHardQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myHardQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myHardQuestions[randQuestion].incorrectAnswer1, false);
                    
                    break;
                case 2:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myHardQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(1,myHardQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myHardQuestions[randQuestion].incorrectAnswer2, false);
                    break;
                case 3:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myHardQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myHardQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(2,myHardQuestions[randQuestion].correctAnswer, true);

                    break;
                case 4:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myHardQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    changeButtonNameClient(1,myHardQuestions[randQuestion].correctAnswer, true);
                    thisButtonScript.setCorrect();
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(2,myHardQuestions[randQuestion].incorrectAnswer1, false);
                    break;
                case 5:
                    thisButtonScript = myButtons[0].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer2);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(0,myHardQuestions[randQuestion].incorrectAnswer2, false);
                    thisButtonScript = myButtons[1].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].incorrectAnswer1);
                    thisButtonScript.setIncorrect();
                    changeButtonNameClient(1,myHardQuestions[randQuestion].incorrectAnswer1, false);
                    thisButtonScript = myButtons[2].GetComponent<ButtonTestScript>();
                    thisButtonScript.setTextName(myHardQuestions[randQuestion].correctAnswer);
                    thisButtonScript.setCorrect();
                    changeButtonNameClient(2,myHardQuestions[randQuestion].correctAnswer, true);
                    break;

            }
            changeCharadeText(myHardQuestions[randQuestion].correctAnswer);
            myHardQuestions.RemoveAt(randQuestion);
            currentPhase++;



        }

        

    }

    [ClientRpc]
    public void changeButtonNameClient(int buttonNum, string buttonName, bool correct){
        Debug.Log("chaning the server's button name");
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
            Debug.Log("Phase is 0");
            Debug.Log("setting quesiton phase to easy");
            currentQuestionPhase = questionPhase.Easy;
        }
        if(phase == 1){
            Debug.Log("phase is 1 so starting questionaire phase");
            currentQuestionPhase = questionPhase.Questionaire;
            startQuestionairephase();
             //currentQuestionPhase = questionPhase.Medium;
             //Debug.Log("setting question phase to medium");
        }
        if(phase == 8){
            currentQuestionPhase = questionPhase.Hard;
            Debug.Log("Settin question phase to be hard");
        }
        if(phase == 12){
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
        Debug.Log("Starting the questionaire phase");
        searchForSceneController();
        quesitonairePhase = true;
        showConfidenceButtons = true;
        showConfidenceButtonsP2 = true;
        foreach(var x in myButtons){
            x.SetActive(false);
        }
        foreach(var x in P2QuestionaireButtons){
            x.SetActive(true);
        }
        Questionaire.SetActive(true);
        questionaireUp = true;
        setP2ConfidenceButtonson();
        changeQuestionaireTextPlayer1(myQuestions[questionaireNumberP1]);
        changeQuestionaireTextPlayer2(myQuestions[questionaireNumberP2]);
        changeQuestionaireTextOnClient();

    
    }
    [ClientRpc]
    public void changeQuestionaireTextOnClient(){
        changeQuestionaireTextPlayer1(myQuestions[questionaireNumberP1]);
        changeQuestionaireTextPlayer2(myQuestions[questionaireNumberP2]);
        questionaireNumberP1++;
        questionaireNumberP2++;
    }
    [ClientRpc]
    public void setP2ConfidenceButtonson(){
        Questionaire.SetActive(true);
        foreach(var x in myButtons){
            x.SetActive(false);
        }
        foreach(var x in P2QuestionaireButtons){
            x.SetActive(true);
        }
        
    }
    public void animateQuestionareUp(){
            // Debug.Log("animating the questionaire up");
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
    
    [Command(requiresAuthority = false)]
    public void setServeranimateP2Down(){
        hideP2Confidence = true;
    }
    [Command(requiresAuthority = false)]
    public void setServeranimateP1Down(){
        hideConfidenceButtons = true;
    }
    
    public void changeQuestionairePhasePlayer1(int phase){
        if(questionaireNumberP1 == 2){
            setServeranimateP1Down();
            
            Debug.Log("putting one in numWaiting questionaire");
            incrementNumWaitingInQuestionaire();
            Debug.Log("about to do if statement and numWaiting is" + numWaitingInQuestionaire);
            Debug.Log("Chaning the questionaire text player 1");
            changeQuestionaireTextPlayer1("Please wait for your partner\n to finish answering their questions");
            if(numWaitingInQuestionaire >= 2){
                Debug.Log("resesting to beginning");
                setServerQuestionaireDown();
                hideBothConfidence();
                resetToBeginning();


            }
            return;
        }
        Debug.Log("changin it back to myquestions");
        changeQuestionaireTextPlayer1(myQuestions[phase]);
        questionaireNumberP1++;
        

    }
     public void changeQuestionairePhasePlayer2(int phase){
        if(questionaireNumberP2 == 2){
            setServeranimateP2Down();

            Debug.Log("putting one in numWaiting questionaire");
            incrementNumWaitingInQuestionaire();
            Debug.Log("changing the player 2 text");
            changeQuestionaireTextPlayer2("Please wait for your partner to finish answering their questions");
            if(numWaitingInQuestionaire >= 2){
                Debug.Log("Reseting to begining");
                setServerQuestionaireDown();
                hideBothConfidence();
                resetToBeginning();
                
            }
            
            return;
            
        }
        Debug.Log("changin it back to myquestions and the current phase is " + phase);
        changeQuestionaireTextPlayer2(myQuestions[phase]);
        questionaireNumberP2++;

    }
    [Command(requiresAuthority = false)]
    public void incrementNumWaitingInQuestionaire(){
        Debug.Log("incrementing num waiting in questionaire which is " + numWaitingInQuestionaire);
        numWaitingInQuestionaire++;
    }
    [Command(requiresAuthority = false)]
    public void setServerQuestionaireDown(){
        questionaireDown = true;
    }
    [Command(requiresAuthority = false)]
    public void hideBothConfidence(){
        hideP1Confidence = true;
        hideP2Confidence = true;
    }

    [Command(requiresAuthority = false)]
     public void animateQuestionaiupServer(){
        animateQuestionareUp();
     
    }
    [Command(requiresAuthority = false)]
     public void animateQuestionaiupServerDown(){
        animateQuestionareDown();
     
    }
    [Command(requiresAuthority = false)]
    public void ServerP1QButtonsDown(){
        hideConfidenceButtons = true;
    }
    [Command(requiresAuthority = false)]
    public void ServerP2QButtonsDown(){
        hideP2Confidence = true;
    }

    
    public void searchForSceneController(){
        // Debug.Log("searching for the scene controller");
        GameObject controllerObject = GameObject.FindGameObjectWithTag("SceneController");
        mySceneController = controllerObject.GetComponent<SceneController>();
        //searchForSceneControllerServer();
        
    }

    
    [Command(requiresAuthority = false)]
    public void resetToBeginning(){
        // Debug.Log("seting wrist scales on server");
        
        questionaireNumSeen++;
        if(questionaireNumSeen == 1){
            mySceneController.myNetworkManager.setPlayerWristScales(.5f);
        }else if(questionaireNumSeen == 2){
            mySceneController.myNetworkManager.setPlayerWristScales(1.5f);
        }
        currentPhase = 0;
        numWaitingInQuestionaire = 0;
        questionaireNumberP1 = 0;
        questionaireNumberP2 = 0;
        quesitonairePhase = false;
        currentQuestionPhase = questionPhase.Easy;
        if(questionaireNumSeen == 2){
            TFPText.SetActive(true);
            endGameClient();
            

        }else{
        foreach(var x in myButtons){
            x.SetActive(true);
        }
        foreach(var x in myButtons){
                x.GetComponent<ButtonTestScript>().enabled = true;
            }
        foreach(var x in confidenceButtons){
                x.GetComponent<ButtonTestScript>().enabled = true;
            }
        foreach(var x in P2QuestionaireButtons){
                x.GetComponent<ButtonTestScript>().enabled = true;
            }
        setInitialNumbers();
        waitForEndofanimation();
        showAnimation = true;
        resetToBeginningClient();
        }

    }
     [ClientRpc]
     public void endGameClient(){
        TFPText.SetActive(true);
        foreach(var x in myButtons){
            x.SetActive(false);
        }
        foreach(var x in confidenceButtons){
            x.SetActive(false);
        }
        foreach(var x in P2QuestionaireButtons){
            x.SetActive(false);
        }

     }
    [ClientRpc]
    public void resetToBeginningClient(){
        quesitonairePhase = false;
        currentPhase = 0;
        numWaitingInQuestionaire = 0;
        questionaireNumberP1 = 0;
        questionaireNumberP2 = 0;
        currentQuestionPhase = questionPhase.Easy;
        foreach(var x in myButtons){
            x.SetActive(true);
        }

    }
    public IEnumerator waitForEndofanimation(){
        yield return new WaitForSecondsRealtime(2);

    }
   


    

   
  
    }

   
    
             

   
    
    



    
