using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class heart : MonoBehaviour {


    private int hearts;
    //public GameObject[] HeartObj;
	// Use this for initialization
	void Start () {
		
        
	}
	
	// Update is called once per frame
	void Update () {
        hearts = GameObject.FindGameObjectWithTag ("Player").GetComponent<NinjaMovementScript> ().Heath;
        //transform.Find("View/Offest/UI/Panel/Text").gameObject.SetActive(true);
        this.GetComponent<Text>().text = hearts.ToString();
	}
    
   
}
