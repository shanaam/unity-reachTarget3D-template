using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCursorContoller : MonoBehaviour {

    //link to the actual hand position object
    public GameObject realHand;

    // Use this for initialization
    void Start () {
        // disable the whole task initially to give time for the experimenter to use the UI
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = realHand.transform.position - transform.parent.transform.position;

        Vector3 realHandPosition = realHand.transform.position;
        Vector3 rotatorObjectPosition = transform.parent.transform.position;

        transform.localPosition = realHandPosition - rotatorObjectPosition;
    }
}
