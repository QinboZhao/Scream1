using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch_BTN_Sound : MonoBehaviour {

    
    private GameObject[] sounds = null;
   
    bool isMute = false;

    void Start()
    {
        transSoundImage();
    }
    public void OnPress_IE()
    {
        isMute = !isMute;
        sounds = GameObject.FindGameObjectsWithTag("Sound");
        foreach (var gameobject in sounds)
        {
            gameobject.GetComponent<AudioSource>().mute = isMute;
          
        }
        transSoundImage();
      
    }

    private void transSoundImage()
    {
         transform.Find("button_sprite_001").gameObject.SetActive(!isMute);
         transform.Find("button_sprite_002").gameObject.SetActive(isMute);
    }

   
}
