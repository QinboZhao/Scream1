using UnityEngine;
using System.Collections;

//MainEvents can keep track of Souls and Player deaths.

public class MainEventsLog : MonoBehaviour {

	public int SoulsCollected;

	public NinjaMovementScript NinjaMovScript;
	public GameObject TouchControls;
    public Ctrl Ctrls;

	public CameraFollowTarget CameraFollowScript;

	public AudioSource AudioSource_Collectible;
	public AudioSource AudioSource_Death;

    //private int heaths;
	
	public void PlayerCollectedSoul(){
		AudioSource_Collectible.Play ();
		Debug.Log ("Soul collected");
        
		SoulsCollected += 1;
	}


	public void PlayerDied(){

		//Camera will stop for a second when the player dies.
		CameraFollowScript.PlayerDied ();
        NinjaMovScript.Heath = NinjaMovScript.Heath - 1;
        Debug.Log(NinjaMovScript.Heath);
        
		AudioSource_Death.Play ();
		Debug.Log ("Player died");
        if (NinjaMovScript.Heath <= 0) { Ctrls.fsmPost("over"); Debug.Log("gameoverpost_______"); }
	}






	//All the Unity UI stuff is here.
	private bool VisibleGUI = false;
	private string GUIText_DoubleJump = "开启二段跳";
	private string GUIText_TouchControls = "隐藏按键";

    public void DoubleJump()
    {
         NinjaMovScript.DoubleJump = true;
    }
	void OnGUI () {
		if(VisibleGUI == true){
			if(GUI.Button(new Rect(10,10,200,40), GUIText_DoubleJump)) {
				if(NinjaMovScript.DoubleJump == true){
					NinjaMovScript.DoubleJump = false;
                    GUIText_DoubleJump = "开启二段跳";
				}else{
                    DoubleJump();
					GUIText_DoubleJump = "取消二段跳";
				}
			}

			if(GUI.Button(new Rect(10,60,200,40), GUIText_TouchControls)) {
				if(TouchControls.activeSelf == true){
					TouchControls.SetActive(false);
					GUIText_TouchControls = "显示按键";
				}else{
					TouchControls.SetActive(true);
					GUIText_TouchControls = "隐藏按键";
				}
			}

         
			if(GUI.Button(new Rect(10,160,100,40), "隐藏")) {
				VisibleGUI = false;
			}
		
		}else{
			if(GUI.Button(new Rect(10,10,150,40), "设置")) {
				VisibleGUI = true;
			}
		}
	}

}
