using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTestScript : MonoBehaviour
{
    public GameObject WinnerText;
    public GameObject LoserText;
    public enum buttonInfo{
        Correct,Incorrect
    }

    buttonInfo thisButtonInfo =buttonInfo.Incorrect;


    // Start is called before the first frame update
    void Start()
    {
        WinnerText.gameObject.SetActive(false);
        LoserText.gameObject.SetActive(false);
        if(gameObject.name.Contains("Lion")){
            thisButtonInfo = buttonInfo.Correct;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void press(){
        if(thisButtonInfo == buttonInfo.Correct){
            WinnerText.gameObject.SetActive(true);
            LoserText.gameObject.SetActive(false);
        }else{
            LoserText.gameObject.SetActive(true);
            WinnerText.gameObject.SetActive(false);
        }
    }
}
