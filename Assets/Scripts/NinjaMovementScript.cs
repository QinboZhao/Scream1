using UnityEngine;
using System.Collections;

public class NinjaMovementScript : MonoBehaviour {
	
	//玩家速度
	public float PlayerSpeed;
    //跳跃力
	public float JumpForce;
    //生命
    public int Heath;
	//二级跳开关
	public bool DoubleJump;
	

	
	private MainEventsLog MainEventsLog_script;
	private bool DJ_available;
	private float JumpForceCount;
	private bool IsGrounded;
	private GameObject GroundedToOBJ;

	private float walljump_count;
	private bool WallTouch;
	private bool WallGripJustStarted;
	private GameObject WallOBJ;

	private bool PlayerLooksRight;

	//复活点
	public GameObject ActiveCheckpoint;



	//按键是否按下
	private bool Btn_Left_bool;
	private bool Btn_Right_bool;
	private bool Btn_Jump_bool;

	//动画和精灵
	public Animator AnimatorController;
	public GameObject MySpriteOBJ;
	private Vector3 MySpriteOriginalScale;

	//粒子系统
	public ParticleSystem WallGripParticles;
	private int WallGripEmissionRate;
	public ParticleSystem JumpParticles_floor;
	public ParticleSystem JumpParticles_wall;
	public ParticleSystem JumpParticles_doublejump;
	public ParticleSystem Particles_DeathBoom;


	
	public AudioSource AudioSource_Jump;



	
	
	void Start () {

		//粒子系统发射速率
		WallGripEmissionRate = 10;
		WallGripParticles.emissionRate = 0;

		
		PlayerLooksRight = true;
		MySpriteOriginalScale = MySpriteOBJ.transform.localScale;//原始缩放

	}
	
	
	void Update () {

		//键盘操作
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Button_Left_press();		
		}
		if(Input.GetKeyUp (KeyCode.LeftArrow)) {
			Button_Left_release();		
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Button_Right_press();		
		}
		if(Input.GetKeyUp (KeyCode.RightArrow)) {
			Button_Right_release();		
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			Button_Jump_press();		
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			Button_Jump_release();		
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			Button_Jump_press();		
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			Button_Jump_release();		
		}

		if (walljump_count >= 0) {
			walljump_count -= Time.deltaTime;		
		}

	}


	void FixedUpdate(){

		//实际操作

		//按键检查
		if(Btn_Left_bool == true && Btn_Right_bool == false){
			if(PlayerLooksRight == true && WallTouch == false){
				PlayerLooksRight = false;
				MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x,MySpriteOriginalScale.y,MySpriteOriginalScale.z);
			}
			this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-PlayerSpeed*Time.deltaTime,0f));
		}else if(Btn_Left_bool == false && Btn_Right_bool == true){
			if(PlayerLooksRight == false && WallTouch == false){
				PlayerLooksRight = true;
				MySpriteOBJ.transform.localScale = MySpriteOriginalScale;
			}
			this.GetComponent<Rigidbody2D>().AddForce(new Vector2(PlayerSpeed*Time.deltaTime,0f));
		}


		//如果在墙上，降低玩家速度
		if (IsGrounded == false && WallTouch == true) {
			this.GetComponent<Rigidbody2D>().velocity = new Vector2 (this.GetComponent<Rigidbody2D>().velocity.x, Physics2D.gravity.y * 0.01f);
		}


		//跳跃
		if (Btn_Jump_bool == true && JumpForceCount > 0) {
			this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x,JumpForce);
			JumpForceCount -= 0.1f*Time.deltaTime;			
		}


		//发送数据到动画状态机
		AnimatorController.SetFloat ("HorizontalSpeed", this.GetComponent<Rigidbody2D>().velocity.x*this.GetComponent<Rigidbody2D>().velocity.x);
		AnimatorController.SetFloat ("VerticalSpeed", this.GetComponent<Rigidbody2D>().velocity.y);
		AnimatorController.SetBool ("Grounded", IsGrounded);
		AnimatorController.SetBool ("Walled", WallTouch);

	}


	void OnCollisionEnter2D(Collision2D coll) {

		//在地上
		if (coll.gameObject.tag == "Ground" && IsGrounded == false) {
			DJ_available = false;
			GroundedToOBJ = coll.gameObject;
			this.transform.parent = coll.gameObject.transform;
			IsGrounded = true;
		}

		//在墙上
		if(coll.gameObject.tag == "Wall" && this.GetComponent<Rigidbody2D>().velocity.y < 0f) {

			DJ_available = false;
			WallOBJ = coll.gameObject;
			this.transform.parent = coll.gameObject.transform;

			WallTouch = true;

			//在墙上改变玩家方向
			if(WallOBJ.transform.position.x < this.transform.position.x){
				PlayerLooksRight = true;
				MySpriteOBJ.transform.localScale = MySpriteOriginalScale;
			}else{
				PlayerLooksRight = false;
				MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x,MySpriteOriginalScale.y,MySpriteOriginalScale.z);
			}

			//开启粒子系统
			WallGripParticles.emissionRate = WallGripEmissionRate;
		}

		if (coll.gameObject.tag == "Roof") {
			JumpForceCount = 0f;
		}

	}

	
	void OnCollisionStay2D(Collision2D coll) {
		
		if(coll.gameObject.tag == "Wall" && WallTouch == false && this.GetComponent<Rigidbody2D>().velocity.y < 0f) {
			
			DJ_available = false;
			WallOBJ = coll.gameObject;
			WallTouch = true;
			
			if(WallOBJ.transform.position.x < this.transform.position.x){
				PlayerLooksRight = true;
				MySpriteOBJ.transform.localScale = MySpriteOriginalScale;
			}else{
				PlayerLooksRight = false;
				MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x,MySpriteOriginalScale.y,MySpriteOriginalScale.z);
			}
			
			WallGripParticles.emissionRate = WallGripEmissionRate;
			
		}
		
		if (coll.gameObject.tag == "Ground" && IsGrounded == false) {
			DJ_available = false;
			GroundedToOBJ = coll.gameObject;
			IsGrounded = true;
		}
	}


	//离开墙或地
	void OnCollisionExit2D(Collision2D coll) {

		if (coll.gameObject.tag == "Ground" && coll.gameObject == GroundedToOBJ) {
			DJ_available = true;
			GroundedToOBJ = null;
			this.transform.parent = null;

			IsGrounded = false;
		}

		if (coll.gameObject.tag == "Wall" && coll.gameObject == WallOBJ) {
            //这使得翻墙容易，玩家能在离开墙几毫秒后继续跳跃
			DJ_available = true;
			walljump_count = 0.16f;

			this.transform.parent = null;
			WallOBJ = null;
			WallTouch = false;
			WallGripParticles.emissionRate = 0;
		}
	}


	public void NinjaDies(){
		Particles_DeathBoom.Emit (50);


		this.gameObject.transform.position = ActiveCheckpoint.transform.position;
		this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;


		
		if(MainEventsLog_script == null){
			MainEventsLog_script = GameObject.FindGameObjectWithTag("MainEventLog").GetComponent<MainEventsLog>();
		}
		MainEventsLog_script.PlayerDied();
	}



	//button
	#region ButtonVoids

	public void Button_Left_press(){
		Btn_Left_bool = true;
	}

	public void Button_Left_release(){
		Btn_Left_bool = false;
	}

	public void Button_Right_press(){
		Btn_Right_bool = true;
	}
		
	public void Button_Right_release(){
		Btn_Right_bool = false;
	}


	public void Button_Jump_press(){

		Btn_Jump_bool = true;


		//如果在地上，执行跳跃
		if (IsGrounded == true) {
			DJ_available = true;
			AudioSource_Jump.Play();
			JumpForceCount = 0.02f;
			this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x,JumpForce);
			JumpParticles_floor.Emit(20);

		//如何在天上，开启了二段跳
		}else if(DoubleJump == true && DJ_available == true && WallTouch == false){
			DJ_available = false;
			AudioSource_Jump.Play();
			JumpForceCount = 0.02f;
			this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x,JumpForce);
			JumpParticles_doublejump.Emit(10);
		}


		//在墙上或者刚离开墙，执行墙跳
		if ((WallTouch == true || walljump_count > 0f) && IsGrounded == false) {
			DJ_available = true;
			AudioSource_Jump.Play();
			JumpForceCount = 0.02f;
			JumpParticles_wall.Emit(20);
			if(PlayerLooksRight == false){
				this.GetComponent<Rigidbody2D>().AddForce (new Vector2 (-JumpForce*32f, 0f));
			}else{
				this.GetComponent<Rigidbody2D>().AddForce (new Vector2 (JumpForce*32f, 0f));
			}
		}


	
	}

	public void Button_Jump_release(){
		JumpForceCount = 0f;
		Btn_Jump_bool = false;
	}
	
	#endregion


}
