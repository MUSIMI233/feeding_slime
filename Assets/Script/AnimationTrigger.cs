using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationTrigger : MonoBehaviour
{
    private HpController hpcontroller;
    private Animator mAnimator;
    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        if (mAnimator == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hpcontroller != null && hpcontroller.currentHealth < 40) {

            TriggerLowHealthAnimation();
        }
    }

    void TriggerLowHealthAnimation ()
    {
        mAnimator.SetTrigger("");
    }
}

