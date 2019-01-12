using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionController : MonoBehaviour {

    public Animator animator;

	// Use this for initialization
	void Start () {
		
	}
	
	public void ShrinkInstructions ()
    {
        animator.SetTrigger("Shrink");
    }

    public void IsStill()
    {
        animator.SetTrigger("StayStill");
    }

    public void ExpandInstruction()
    {
        animator.SetTrigger("Expand");
    }
}
