using UnityEngine;
using System.Collections;

//玩家复活
public class Checkpoint : MonoBehaviour {

	public MeshRenderer MyMeshrenderer;
	
	void Start () {
		//隐藏绿色的复活点
		MyMeshrenderer.enabled = false;	
	}

	//把玩家经过的复活点设为激活的复活点
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			other.GetComponent<NinjaMovementScript>().ActiveCheckpoint = this.gameObject;	
		}
	}


}
