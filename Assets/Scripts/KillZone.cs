using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour {

	
	public GameObject CheckPoint;//复活点
	public MeshRenderer MyMeshrenderer;
	
	private NinjaMovementScript NinjaScript;
	
	void Start () {
		//隐藏
		MyMeshrenderer.enabled = false;	
	}

	//如果死亡，执行死亡，在复活点复活
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {

			if(NinjaScript == null){
				NinjaScript = GameObject.FindGameObjectWithTag("Player").GetComponent<NinjaMovementScript>();
			}
			NinjaScript.NinjaDies();

		}
	}


}
