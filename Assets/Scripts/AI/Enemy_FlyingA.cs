using UnityEngine;
using System.Collections;

public class Enemy_FlyingA : MonoBehaviour {

    /*
     AI CORE
     
    */
    public float moveVx;//x方向的分速度    
    public float moveVy;//y方向的分速度
    float _vHeadingX;
    float _vHeadingY;
    // 2维坐标(x,y)
    public Vector2 Position
    {
        get
        {
            return new Vector2(this.transform.position.x, this.transform.position.y);
        }
    }
    //设置导弹的前进方向的归一化向量m_vHeading
    public Vector2 vHeading
    {
        get
        {
            float length = Mathf.Sqrt(moveVx * moveVx + moveVy * moveVy);
            if (length != 0)
            {
                 _vHeadingX = moveVx / length;
                 _vHeadingY = moveVy / length;
            }
            return new Vector2(_vHeadingX, _vHeadingY);
        }
    }
    // 前进方向的垂直向量
    
    public Vector2 vSide
    {
        get
        {            
            return new Vector2(-vHeading.y,vHeading.x);
        }
    }
    // 速度向量
    public Vector2 Velocity
    {
        get
        {
            return new Vector2(moveVx, moveVy);
        }
    }
    // 速度标量
    public float Speed
    {
        get
        {
            return Mathf.Sqrt(moveVx * moveVx + moveVy * moveVy);
        }
    }
    //public float MaxSpeedRate;
    public float speedRate = 1;//移动速度率
    public void Move(float speedRate)
    {
        this.transform.position += new Vector3(moveVx * Time.deltaTime, moveVy * Time.deltaTime, 0) * speedRate;
    }
    /*
     AI CORE END
    */
    public GameObject target;//目标
    float target_moveSpeed;

    public float speed;

    private NinjaMovementScript PlayerScript;

    private bool EnemyAwake = false;
    private bool EnemyDead = false;

    //Distance who far player has to be to this enemy to wakeup
    private float AwakeDistance = 10f;

    //Here are reference slots for AnimationController and Player Sprite Object.
    public Animator AnimatorController;
    public GameObject MySpriteOBJ;
    private Vector3 MySpriteOriginalScale;

    public ParticleSystem ParticleTrail;

    public AudioSource EnemyDiesAudio;

    void Start()
    {
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<NinjaMovementScript>();

       
        target = GameObject.FindGameObjectWithTag("Player");
        //Start the distance checks. (When player gets close enough, Wake up. When he gets far enough, Go back to sleep.
        InvokeRepeating("CheckPlayerDistance", 0.5f, 0.5f);

        MySpriteOriginalScale = MySpriteOBJ.transform.localScale;
        MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 1f);
        ParticleTrail.emissionRate = 0;

    }
    /*
    Vector2 AI_PredictionPursuit()
    {
        Vector2 ToPursuit = target.transform.position - this.transform.position;  
        return new Vector2();
    }
    */
    Vector2 AI_Seek(Vector2 targetPos)
    {
        Vector2 vSeekPos = targetPos - this.Position;
        vSeekPos = vSeekPos.normalized * speed - this.Velocity;
            //*this.speed - this.Velocity;
        return vSeekPos;
    }  
    void FixedUpdate()
    {
       // AI_PredictionPursuit();
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //If you are awake. Move towards the player
        if (EnemyAwake == true && EnemyDead == false)
        {


            //float step = speed * Time.deltaTime;

            moveVx = this.GetComponent<Rigidbody2D>().velocity.x;
            moveVy = this.GetComponent<Rigidbody2D>().velocity.y;
            //transform.position = Vector3.MoveTowards(this.transform.position, PlayerScript.transform.position, step);
            Vector2 moveVec = AI_Seek(target.transform.position);
            float length = Mathf.Sqrt(moveVec.x * moveVec.x + moveVec.y * moveVec.y);
            if (length != 0)
            {
                //   Debug.Log("x:" + moveVec.x + "y:" + moveVec.y);  
                this.moveVx += speedRate * moveVec.x / length;
                this.moveVy += speedRate * moveVec.y / length;

            }  
            //transform.Translate(target.transform.position.x /100* step * target.GetComponent<Rigidbody2D>().velocity.x,
            //    target.transform.position.y /100* step * target.GetComponent<Rigidbody2D>().velocity.y, 0);
            this.Move(1);

            if (MySpriteOBJ.transform.localScale.x > 0 && transform.position.x > PlayerScript.transform.position.x)
            {
                MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 1f);
            }

            if (MySpriteOBJ.transform.localScale.x < 0 && transform.position.x < PlayerScript.transform.position.x)
            {
                MySpriteOBJ.transform.localScale = new Vector3(MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 1f);
            }
        }


        AnimatorController.SetBool("Awake", EnemyAwake);
        AnimatorController.SetBool("Dead", EnemyDead);
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
                EnemyDead = true;
                Debug.Log("Monster died");

                Invoke("iDied", 0.15f);
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
            EnemyAwake = true;
            ParticleTrail.emissionRate = 15;

        }

        if (Vector3.Distance(this.transform.position, PlayerScript.transform.position) > AwakeDistance && EnemyAwake == true)
        {
            //			Debug.Log("Far enough to fall back sleep");
            EnemyAwake = false;
            ParticleTrail.emissionRate = 0;

        }

    }


}
