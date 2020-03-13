using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    [SerializeField] private List<Animator> animators;

    public void Play(int num) { // 播放编号为num的动画
        animators[num].Play("Play", -1, 0);
    }

}
