using UnityEngine;
using System.Collections;

//使奖励物品旋转

public class AutoRotator : MonoBehaviour {

	public float RotationSpeed;
	
	void Update () {
		this.transform.Rotate (new Vector3 (0f, 0f, RotationSpeed*Time.deltaTime));
	}
}
