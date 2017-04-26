using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDGeek;
using UnityEngine.SceneManagement;
public class Ctrl : MonoBehaviour {

    //有限状态机，人工智能基础组件之一
    private FSM fsm_ = new FSM();

    private NinjaMovementScript NinjaMovScript;

    public View _view = null;
    public Model model = null;

    public void fsmPost(string msg)
    {
        fsm_.post(msg);
    }

    public void doubleJumpClickSet()
    {

         GameObject.FindGameObjectWithTag("MainEventLog").GetComponent<MainEventsLog>().DoubleJump();
            
    }
   
	void Start () {
        fsm_.addState("begin",beginState());
        fsm_.addState("play", playState());
        fsm_.addState("end", endState());

        fsm_.init("begin");
	}

    private State beginState()
    {
        //带消息映射表的状态类
        StateWithEventMap state = new StateWithEventMap();
        
        state.onStart += delegate
        {
            
            _view.begin.gameObject.SetActive(true);
           // _view.uiCamera.gameObject.SetActive(false);
            //_view.playCamera.gameObject.SetActive(true);
        };
        state.onOver += delegate
        {
            _view.begin.gameObject.SetActive(false);
           // _view.uiCamera.gameObject.SetActive(true);
           // _view.playCamera.gameObject.SetActive(false);
        };
        
        state.addAction("begin", "play");
        return state;
    }

	private State playState()
    {
        
        StateWithEventMap state = new StateWithEventMap();

        
        //带有任务的状态,异步处理工具
        /*
        StateWithEventMap state = TaskState.Create(delegate
        {
            TaskWait tw = new TaskWait();
            
            tw.setAllTime(10f);
            
            return tw;
        }, fsm_, "end");
        */
        state.onStart += delegate
        {
            _view.play.gameObject.SetActive(true);
           
        };
        state.onOver += delegate
        {
            _view.play.gameObject.SetActive(false);
           // Destroy(GameObject.FindGameObjectWithTag("Player"));
        };
        state.addAction("over", "end");
        return state;
        
    }

    private State endState()
    {
        StateWithEventMap state = new StateWithEventMap(); 
        state.addAction("end", "begin");
        
        state.onStart += delegate
        {
            _view.end.gameObject.SetActive(true);
        };
        state.onOver += delegate
        {
            _view.end.gameObject.SetActive(false);
            SceneManager.LoadScene(0);
        };
             
        return state;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
