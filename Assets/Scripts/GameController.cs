using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController :MonoBehaviour
{
    //单例
    
    public CookingStepCollection stepCollection;
    private static GameController instance = new GameController();
    private GameController()
    {
        stepCollection = new CookingStepCollection();
    }
    
    public  static GameController GetInstance()
    {
        return instance;
    }

}
