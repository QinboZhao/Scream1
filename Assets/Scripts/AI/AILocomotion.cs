using UnityEngine;
using System.Collections;
using GDGeek;
public class AILocomotion : Vehicle 
{
    //AI角色控制器
	private CharacterController controller;
	private Rigidbody theRigidbody;
    //每次移动的距离
	private Vector3 moveDistance;
	public bool displayTrack;
     //唤醒敌人的距离
     public float AwakeDistance = 10f;

    public AudioSource EnemyDiesAudio;
    public ParticleSystem ParticleTrail;
    private bool EnemyAwake = false;
    private bool EnemyDead = false; 
    //动画和精灵
    private NinjaMovementScript PlayerScript;
    public Animator AnimatorController;
    public GameObject MySpriteOBJ;
    private Vector3 MySpriteOriginalScale;

    public FSM aiFsm = new FSM();
   
    private string current = "";
	// Use this for initialization
	void Start () 
	{
        
		controller = GetComponent<CharacterController>();
		theRigidbody = GetComponent<Rigidbody>();
		moveDistance = new Vector3(0,0,0);
		base.Start();

        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<NinjaMovementScript>();
        ParticleTrail.emissionRate = 0;

        InvokeRepeating("CheckPlayerDistance", 0.5f, 0.5f);

        MySpriteOriginalScale = MySpriteOBJ.transform.localScale;
        MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 1f);
        
        aiFsm.addState("sleep",Sleep());
        aiFsm.addState("awake", Awake());
        aiFsm.addState("dead", Dead());
        aiFsm.init("awake");
	}

    private State Dead()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.addAction("away", "sleep");
        state.onStart += delegate
        {
            current = "dead";
            AnimatorController.SetBool("Dead", EnemyDead);
            EnemyDead = true;
            Debug.Log("Monster died");

            Invoke("iDied", 0.15f);
        };
        state.onOver += delegate
        {
           
        };

        return state;
    }

    private State Awake()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.addAction("away","sleep");
        state.addAction("die", "dead");
        state.onStart += delegate
        {          
            EnemyAwake = true;
            current = "awake";
            ParticleTrail.emissionRate = 15;
            AnimatorController.SetBool("Awake", EnemyAwake);

        };
        state.onOver += delegate
        {
           EnemyAwake = false;
           AnimatorController.SetBool("Awake", EnemyAwake);
        };
        
        return state;
    }

    private State Sleep()
    {

        StateWithEventMap state = new StateWithEventMap();
        state.addAction("near", "awake");
        state.onStart += delegate
        {
            EnemyAwake = false;
            current = "sleep";
            ParticleTrail.emissionRate = 0;
        };
        state.onOver += delegate
        {
            EnemyAwake = true;
        };
        return state;
    }
	

	void FixedUpdate()
	{
        run();
        
	}
    private void run()
    {
        //计算速度
        velocity += acceleration * Time.fixedDeltaTime;
        //限制速度，要低于最大速度
        if (velocity.sqrMagnitude > sqrMaxSpeed)
            velocity = velocity.normalized * maxSpeed;
        //计算AI角色移动的距离
        moveDistance = velocity * Time.fixedDeltaTime;

        if (isPlanar)
        {
            velocity.y = 0;
            moveDistance.y = 0;
        }

        if (displayTrack)
            //Debug.DrawLine(transform.position, transform.position + moveDistance, Color.red,30.0f);
            Debug.DrawLine(transform.position, transform.position + moveDistance, Color.black, 30.0f);

        if (controller != null)
        {
            //if (displayTrack)
            //Debug.DrawLine(transform.position, transform.position + moveDistance, Color.blue,20.0f);
            controller.SimpleMove(velocity);

        }
        else if (theRigidbody == null || theRigidbody.isKinematic)
        {
            //移动
            if (current=="awake")
            {
                transform.position += moveDistance;
            }
        }
        else
        {
            theRigidbody.MovePosition(theRigidbody.position + moveDistance);
        }

        /*
         * updata facing direction
        if (velocity.sqrMagnitude > 0.00001)
        {
            //Vector3 newForward = Vector3.Slerp(transform.forward, velocity, damping * Time.deltaTime);
            Vector3 newForward = new Vector3(.0f, .0f, damping * Time.deltaTime);
            if (isPlanar)
                newForward.y = 0;
		
         transform.forward = newForward;
        }
        */


        if (MySpriteOBJ.transform.localScale.x > 0 && transform.position.x > PlayerScript.transform.position.x)
        {
            MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 1f);
        }

        if (MySpriteOBJ.transform.localScale.x < 0 && transform.position.x < PlayerScript.transform.position.x)
        {
            MySpriteOBJ.transform.localScale = new Vector3(MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 1f);
        }

   
        //gameObject.animation.Play("walk");
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll.contacts[0].normal.ToString());

        if (coll.gameObject.tag == "Player")
        {

            //Check who killed who. If contact happend from the top player killed the enemy. Else player died.
            if (coll.contacts[0].normal.x > -1f && coll.contacts[0].normal.x < 1f && coll.contacts[0].normal.y < -0.8f && coll.contacts[0].normal.y > -1.8f)
            {
                if (EnemyDiesAudio != null)
                {
                    EnemyDiesAudio.Play();
                }
                ParticleTrail.emissionRate = 0;
                coll.rigidbody.AddForce(new Vector2(0f, 1500f));
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -200f));
                aiFsm.post("die");
            }
            else
            {
                PlayerScript.NinjaDies();
            }

        }
    }

    void iDied()
    {
        Destroy(this.gameObject);
    }


    void CheckPlayerDistance()
    {

        if (Vector3.Distance(this.transform.position, PlayerScript.transform.position) <= AwakeDistance && EnemyAwake == false)
        {
            //			Debug.Log("Close enough to wake up");
           // EnemyAwake = true;
            aiFsm.post("near");
           

        }

        if (Vector3.Distance(this.transform.position, PlayerScript.transform.position) > AwakeDistance && EnemyAwake == true)
        {
            //			Debug.Log("Far enough to fall back sleep");
           // EnemyAwake = false;
            aiFsm.post("away");      
        }

    }
}
