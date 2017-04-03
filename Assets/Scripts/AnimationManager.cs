using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class AnimationManager : MonoBehaviour, IManager {

    public ManagerState currentState { get; private set; }

    Animator panelAnimator;

    public void BootSequence()
    {
        Debug.Log(string.Format("{0} is booting up", GetType().Name));

        panelAnimator = GameObject.FindWithTag("UI").GetComponent<Animator>();
        currentState = ManagerState.Completed;

        Debug.Log(string.Format("{0} status = {1}", GetType().Name, currentState));
    }

    public IEnumerator IntroAnimation()
    {
        AnimationTuple introAnim = AnimationTuples.introAnimation;
        panelAnimator.SetBool(introAnim.parameter, introAnim.value);
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator ExitAnimation()
    {
        AnimationTuple exitAnim = AnimationTuples.exitAnimation;
        panelAnimator.SetBool(exitAnim.parameter, exitAnim.value);
        yield return new WaitForSeconds(1f);

    }
}
