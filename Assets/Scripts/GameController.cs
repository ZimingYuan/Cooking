using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController :MonoBehaviour
{
    //单例
    
    public CookingStepCollection stepCollection;
    private static GameController instance = new GameController();
    //Holder
    private TimeHolder timeHolder1;
    private TimeHolder timeHolder2;
    private MenuHolder MenuHolder;
    //
    public int maxTime = 0;

    private GameController()
    {
        stepCollection = new CookingStepCollection();
    }
    
    public  static GameController GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        MenuHolder = GameObject.FindWithTag("MenuHolder").GetComponent<MenuHolder>();
        timeHolder1 = GameObject.FindWithTag("TimeHolder1").GetComponent<TimeHolder>();
        timeHolder2 = GameObject.FindWithTag("TimeHolder2").GetComponent<TimeHolder>();
    }

    public void Finish()
    {
        if(MenuHolder.transform.childCount==0)
        {
            instance.maxTime = timeHolder1.maxTime > timeHolder2.maxTime ? timeHolder1.maxTime : timeHolder2.maxTime;
            Debug.Log(instance.maxTime);
        }
    }

  

}
