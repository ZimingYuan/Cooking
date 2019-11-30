using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController :MonoBehaviour
{
    //单例
    private static GameController instance = new GameController();
    
    private GameController()
    {

    }
    
    public  static GameController GetInstance()
    {
        return instance;
    }

    public CookingStepCollection stepCollection;


}
