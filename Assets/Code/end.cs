using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class end : MonoBehaviour {

    private MainEventsLog MainEventsLog_script;
    int score = 0;
    string endString;
	// Use this for initialization
	void Start () {
		 if (MainEventsLog_script == null)
        {
            MainEventsLog_script = GameObject.FindGameObjectWithTag("MainEventLog").GetComponent<MainEventsLog>();
        }
        score = MainEventsLog_script.SoulsCollected;
        if (score <= 5)
        {
           endString = "太菜了！你获得了"  + score.ToString() + "分";
        }
        else if (score >= 5 && score <= 10)
        {
          endString = "再接再厉！你获得了" + score.ToString() + "分";
        }
        else if (score >= 10)
        {
          endString = "恭喜你！你获得了" + score.ToString() + "分";
        }
       
        this.GetComponent<Text>().text = endString;
	}
	
	// Update is called once per frame
	void Update () {
       
	}
}
