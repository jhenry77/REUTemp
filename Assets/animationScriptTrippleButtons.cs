using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class animationScriptTrippleButtons : NetworkBehaviour
{
   
    public float increment = 0.003f;
    public ButtonController myButtonController;
    float minHeight;
    float maxHeight;
    // Start is called before the first frame update
    void Start()
    {
        maxHeight = gameObject.transform.position.y;
        minHeight = maxHeight - .4f;
        
    }

    // Update is called once per frame
    [Server]
    void FixedUpdate(){
        if(myButtonController.showAnimation){
            animateUp();
        }
        if(myButtonController.hideAnimation){
            animateDown();
        }
        
    }

    
    public void animateUp(){
        if(gameObject.transform.position.y < maxHeight){
            Vector3 myVector = gameObject.transform.position;
            myVector.y = myVector.y + increment; 
            Quaternion myRotation = gameObject.transform.rotation;
            gameObject.transform.SetPositionAndRotation(myVector,myRotation);
        }else{
            myButtonController.pressed = false;
            myButtonController.showAnimation = false;
            myButtonController.dataStartInterval.Add(Time.time.ToString());
            myButtonController.showWhatToGesture();
            myButtonController.turnOffCorrectText();
            
        }
        

    }
    public void animateDown(){
            if(gameObject.transform.position.y > minHeight){
                Vector3 myVector = gameObject.transform.position;
                myVector.y = myVector.y - increment; 
                Quaternion myRotation = gameObject.transform.rotation;
                gameObject.transform.SetPositionAndRotation(myVector,myRotation);
            }else{
                myButtonController.timeForRandom = true;
                myButtonController.hideAnimation = false;
                myButtonController.showConfidenceButtons = true;
            }

        }


}
