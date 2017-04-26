using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class Vehicle : MonoBehaviour {

    //控制操作列表
	private Steering[] steerings;
    //最大速度
	public float maxSpeed = 10;
    //受到的力的最大值
	public float maxForce = 100;
    //最大速度的平方
	protected float sqrMaxSpeed;
    //质量
	public float mass = 1;
    //速度
	public Vector3 velocity;
    //控制转向时的速度
	public float damping = 0.9f;
    //操作力的计算时间间隔，为了达到更高的帧率，控制力不需要每帧更新
	public float computeInterval = 0.2f;
    //是否在二维平面上，如果是，计算两个gameobject的距离时，忽略y值的不同
	public bool isPlanar = true;
    //计算得到的操作力
	private Vector3 steeringForce;
    //AI角色的加速度
	protected Vector3 acceleration;
	//private CharacterController controller;
	//private Rigidbody theRigidbody;
	//private Vector3 moveDistance;

    //计时器
	private float timer;


	protected void Start () 
	{
		steeringForce = new Vector3(0,0,0);
		sqrMaxSpeed = maxSpeed * maxSpeed;
		//moveDistance = new Vector3(0,0,0);
		timer = 0;
        //获得AI角色所包含的操控行为列表
		steerings = GetComponents<Steering>();

		//controller = GetComponent<CharacterController>();
		//theRigidbody = GetComponent<Rigidbody>();
	}


	void Update () 
	{
		timer += Time.deltaTime;
		steeringForce = new Vector3(0,0,0);  

		//如果距离上次计算操纵力的时间大于设定的时间间隔 再次计算操纵力
		if (timer > computeInterval)
		{
            //将操纵行为列表中的所有操纵行为对应的操纵力进行带权重求和
			foreach (Steering s in steerings)
			{
				if (s.enabled)
					steeringForce += s.Force()*s.weight;
			}
            //使操纵力不大于maxForce
			steeringForce = Vector3.ClampMagnitude(steeringForce,maxForce);
            //计算加速度
			acceleration = steeringForce / mass;

			timer = 0;
		}

	}

	/*
	void FixedUpdate()
	{
		velocity += acceleration * Time.fixedDeltaTime; 
		
		if (velocity.sqrMagnitude > sqrMaxSpeed)
			velocity = velocity.normalized * maxSpeed;
		
		moveDistance = velocity * Time.fixedDeltaTime;
		
		if (isPlanar)
			moveDistance.y = 0;
		
		if (controller != null)
			controller.SimpleMove(velocity);
		else if (theRigidbody == null || theRigidbody.isKinematic)
			transform.position += moveDistance;
		else
			theRigidbody.MovePosition(theRigidbody.position + moveDistance);		
		
		//updata facing direction
		if (velocity.sqrMagnitude > 0.00001)
		{
			Vector3 newForward = Vector3.Slerp(transform.forward, velocity, damping * Time.deltaTime);
			newForward.y = 0;
			transform.forward = newForward;
		}
	}*/
}
