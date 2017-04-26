using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDGeek;
public class Enemy_FSM : MonoBehaviour {

    private FSM enemy_fsm = new FSM();
    private NinjaMovementScript PlayerScript;
    private float AwakeDistance = 10f;


	void Start () {
        enemy_fsm.addState("Idle",idelState());
        enemy_fsm.addState("Stroll",strollState()); //闲逛
        enemy_fsm.addState("Search",searchState());
        enemy_fsm.addState("Die", dieState());

        enemy_fsm.init("Idle");

        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<NinjaMovementScript>();
	}
    private State idelState()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.onStart += delegate
        {

        };
        state.onOver += delegate
        {
           
        };

        state.addAction("awake", "Stroll");
        return state;
    }
    private State dieState()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.onStart += delegate
        {

        };
        state.onOver += delegate
        {

        };
        return state;
    }

    private State searchState()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.onStart += delegate
        {

        };
        state.onOver += delegate
        {

        };
        return state;
    }

    private State strollState()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.onStart += delegate
        {

        };
        state.onOver += delegate
        {

        };
        state.addAction("unawake", "Idle");
        return state;
    }

   
	
	// Update is called once per frame
	void Update () {
      
        

	}
    void FixedUpdate()
    {
      InvokeRepeating("CheckPlayerDistance", 0.5f, 0.5f);
    }
    void CheckPlayerDistance()
    {

        if (Vector3.Distance(this.transform.position, PlayerScript.transform.position) <= AwakeDistance)
        {
            Debug.Log("Close enough to wake up");
            //EnemyAwake = true;
            enemy_fsm.post("awake");
           // ParticleTrail.emissionRate = 15;

        }

        if (Vector3.Distance(this.transform.position, PlayerScript.transform.position) > AwakeDistance )
        {
            Debug.Log("Far enough to fall back sleep");
            //EnemyAwake = false;
            enemy_fsm.post("unawake");
            //ParticleTrail.emissionRate = 0;

        }

    }
}
