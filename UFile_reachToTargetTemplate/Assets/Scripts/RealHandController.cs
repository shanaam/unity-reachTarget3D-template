using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealHandController : MonoBehaviour {

    private void Start()
    {
        // disable the whole task initially to give time for the experimenter to use the UI
        gameObject.SetActive(false);
    }

    //Update is called once per frame
    //void Update()
    //{
        //transform.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        //transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        //print(transform.position);
    //}
}
