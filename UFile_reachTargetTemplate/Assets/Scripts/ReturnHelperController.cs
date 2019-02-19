using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnHelperController : MonoBehaviour {

    public ExampleController exampleController;
    public GameObject trackerHolderObject;

    // Use this for initialization
    void Start () {
        // disable initially to give time for the experimenter to use the UI
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        //set radius to... magnitude of cursor-home
        float radius = Vector3.Distance(trackerHolderObject.transform.position, transform.position) * 2;

        // scale to radius
        transform.localScale = new Vector3(radius, radius, radius);
    }
}
