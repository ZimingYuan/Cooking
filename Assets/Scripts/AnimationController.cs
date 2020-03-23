using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    [SerializeField] private List<Animator> animators = null;
    [SerializeField] private List<AudioClip> audios = null;
    [SerializeField] private AudioSource Player = null;

    public void Play(int num) { // 播放编号为num的动画
        animators[num].Play("Play", -1, 0);
        Player.clip = audios[num];
        Player.Play();
    }

}
