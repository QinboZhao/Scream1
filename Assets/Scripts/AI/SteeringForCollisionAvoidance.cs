using UnityEngine;
using System.Collections;

public class SteeringForCollisionAvoidance : Steering 
{
	public bool isPlanar;	
	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;
	private float maxForce;
	public float avoidanceForce;
	public float MAX_SEE_AHEAD = 2.0f;
	private GameObject[] allColliders;

	void Start () 
	{
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
		maxForce = m_vehicle.maxForce;
		isPlanar = m_vehicle.isPlanar;
		//avoidanceForce = 20.0f;
		if (avoidanceForce > maxForce)
			avoidanceForce = maxForce;

		//MAX_SEE_AHEAD = 20.0f;
		allColliders = GameObject.FindGameObjectsWithTag("Wall");
	}
    
	public override Vector3 Force()
	{
		RaycastHit2D hit;
		Vector3 force = new Vector3(0,0,0);
		//Debug.DrawLine(transform.position, transform.position + transform.forward * 10);

		Vector3 velocity = m_vehicle.velocity;
		Vector3 normalizedVelocity = velocity.normalized;

        //hit = Physics2D.CircleCastAll(this.transform.position,,);
        hit = Physics2D.BoxCast(this.transform.position, 
            new Vector2(1, 1), 
           // normalizedVelocity,
            10.0f, new Vector2(10, 10),
            MAX_SEE_AHEAD * velocity.magnitude / maxSpeed, 1 << LayerMask.NameToLayer("Wall"));
          //hit = Physics2D.Raycast(this.transform.position, new Vector2(100, 100), MAX_SEE_AHEAD * velocity.magnitude / maxSpeed);
        //hit = Physics2D.Raycast(this.transform.position, new Vector2(100, 100), 
           // MAX_SEE_AHEAD * velocity.magnitude / maxSpeed, 1 << LayerMask.NameToLayer("Wall"));

        if (hit)
        {
            Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.gameObject.name == "Wall"
                || hit.collider.gameObject.name == "Roof" || hit.collider.gameObject.name == "Ground"
                || hit.collider.gameObject.name == "Top")
            {
                Vector3 ahead = transform.position + normalizedVelocity * MAX_SEE_AHEAD * (velocity.magnitude / maxSpeed);
                force = ahead - hit.collider.transform.position;
                force *= avoidanceForce;

                if (isPlanar)
                    force.y = 0;
            }
        }
		//Debug.DrawLine(transform.position, transform.position + normalizedVelocity * MAX_SEE_AHEAD * (velocity.magnitude / maxSpeed));

		//if (Physics.Raycast(transform.position, normalizedVelocity, out hit, MAX_SEE_AHEAD))
       /*
        if (Physics2D.Raycast(new Vector2(),new Vector2(), float(hit), MAX_SEE_AHEAD * velocity.magnitude / maxSpeed))
		{
			//Vector3 ahead = transform.position + normalizedVelocity * MAX_SEE_AHEAD;
			Vector3 ahead = transform.position + normalizedVelocity * MAX_SEE_AHEAD * (velocity.magnitude / maxSpeed);
			force = ahead - hit.collider.transform.position;
			force *= avoidanceForce; 
			
			if (isPlanar)
				force.y = 0;

			//Debug.DrawLine(transform.position, transform.position + force);	
			
		}
        */
        
		return force;
	}
}
