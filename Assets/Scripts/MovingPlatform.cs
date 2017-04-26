using UnityEngine;
using System.Collections;

//移动的平台
public class MovingPlatform : MonoBehaviour {

	//平台移动速度
	public float speed;

	//移动的位置.
	public Transform MovePosition;

	//起始位置
	private Vector3 StartPosition;
	private Vector3 EndPosition;
	private bool OnTheMove;

	
	void Start () {
		
		StartPosition = this.transform.position;
		EndPosition = MovePosition.position;
	}
	
	void FixedUpdate () {
	
		float step = speed * Time.deltaTime;

		if (OnTheMove == false) {
			this.transform.position = 
                Vector3.MoveTowards (this.transform.position, EndPosition, step);
		}else{
			this.transform.position = Vector3.MoveTowards (this.transform.position, StartPosition, step);
		}
		
        //当平台到达终点，开始向反方向移动
		if (this.transform.position.x == EndPosition.x && this.transform.position.y == EndPosition.y && OnTheMove == false) {
			OnTheMove = true;
		}else if (this.transform.position.x == StartPosition.x && this.transform.position.y == StartPosition.y && OnTheMove == true) {
			OnTheMove = false;
		}
	}
	
	

}
