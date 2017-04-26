using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrack : MonoBehaviour
{
    public GameObject aim;
    public GameObject target;
    public float moveSpeed;//追踪目标移动速度
    public float targetSpeed;//追踪速度
    public float target_x;//追踪移动的单位量
    public float target_y;
    // Use this for initialization
    void Start()
    {
        aim = GameObject.FindGameObjectWithTag("Player").gameObject;
        target = aim;
    }
    // Update is called once per frame
    void Update()
    {
        /*
        moveSpeed = 5.0f;
        targetSpeed =Mathf.Sqrt(target.GetComponent<Rigidbody2D>().velocity.x + target.GetComponent<Rigidbody2D>().velocity.y);
        target_x = target.transform.position.x;
        target_y = target.transform.position.y;
        MoveTarget();
        Track_AI();
         */
    }
    void Track_AI()
    {
        //x方向的追踪
        if (target.transform.position.x > this.transform.position.x)
        {
            this.transform.position += new Vector3(target_x, 0, 0) * targetSpeed;
        }
        else if (target.transform.position.x < this.transform.position.x)
        {
            this.transform.position -= new Vector3(target_x, 0, 0) * targetSpeed;
        }
        //y方向的追踪
        if (target.transform.position.y > this.transform.position.y)
        {
            this.transform.position += new Vector3(0, target_y, 0) * targetSpeed;
        }
        else if (target.transform.position.y < this.transform.position.y)
        {
            this.transform.position -= new Vector3(0, target_y, 0) * targetSpeed;
        }
        
    }
    void MoveTarget()
    {
        float x = aim.transform.position.x;
        float y = aim.transform.position.y;

        target.transform.Translate(x * Time.deltaTime * moveSpeed, y * Time.deltaTime * moveSpeed, 0);
        
    }
}
