using UnityEngine;
using System.Collections;

public class SteeringForPursuit : Steering {

	public GameObject target;
	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;


	void Start () {
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
	}
	

	public override Vector3 Force()
	{
		Vector3 toTarget = target.transform.position - transform.position;
       // Debug.Log(toTarget);
       
		float relativeDirection = Vector3.Dot(transform.forward, target.transform.forward);

		if ((Vector3.Dot(toTarget, transform.forward) > 0) && (relativeDirection < -0.95f))
		{
			desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
			return (desiredVelocity - m_vehicle.velocity);
		}
        
		float lookaheadTime = toTarget.magnitude / (maxSpeed + target.GetComponent<Rigidbody2D>().velocity.magnitude);
        //Debug.Log(lookaheadTime);
        Vector2 v =  target.GetComponent<Rigidbody2D>().velocity;
        desiredVelocity = (target.transform.position + new Vector3(v.x,v.y,0) * lookaheadTime 
            - transform.position).normalized * maxSpeed;
		
        
         return (desiredVelocity - m_vehicle.velocity);
	}
}
