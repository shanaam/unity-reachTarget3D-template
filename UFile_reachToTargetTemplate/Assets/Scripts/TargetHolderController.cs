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
        target.transform.localPosition = new Vector3(0, 0, 0.3f);
    }
}
