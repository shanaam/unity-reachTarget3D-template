using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHolderController : MonoBehaviour {

    public GameObject targetPrefab;

	// Use this for initialization
	void Start () {
		
	}

    public void InstantiateTarget()
    {
        var target = Instantiate(targetPrefab, transform);
        target.transform.localPosition = new Vector3(0, 0, 0.2f);
    }

    // Method for destroying the target (called at the end of each trial
    public void DestroyTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

        for (var i = 0; i < targets.Length; i++)
        {
            Destroy(targets[i]);
        }
    }
}
