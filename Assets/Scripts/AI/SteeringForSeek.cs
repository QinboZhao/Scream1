using UnityEngine;
using System.Collections;

public class SteeringForSeek : Steering {
	//需要寻找的目标物体
	public GameObject target;
    //预期速度
	private Vector3 desiredVelocity;
    //获得被操纵AI角色，查询AI的最大速度信息
	private Vehicle m_vehicle;
    //最大速度
	private float maxSpeed;

	private bool isPlanar;
	
	void Start () {
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
		isPlanar = m_vehicle.isPlanar;
	}
	
	
	public override Vector3 Force()
	{
        //计算预期速度
		desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
		if (isPlanar)
			desiredVelocity.y = 0;
		return (desiredVelocity - m_vehicle.velocity);
	}
}

