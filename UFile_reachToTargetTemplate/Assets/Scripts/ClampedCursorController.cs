using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampedCursorController : MonoBehaviour {

    public GameObject realHand;
    public ExampleController exampleController;

    // Use this for initialization
    void Start()
    {
        // disable the whole task initially to give time for the experimenter to use the UI
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        //Vector3 targetPosition = GameObject.FindGameObjectWithTag("Target").transform.position;

        GameObject target = GameObject.FindGameObjectWithTag("Target");

        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            Vector3 localTargetPosition = targetPosition - transform.parent.transform.position;

            //transform.localPosition = realHand.transform.position - transform.parent.transform.position;
            Vector3 realHandPosition = realHand.transform.position;
            Vector3 rotatorObjectPosition = transform.parent.transform.position;

            //project onto a vector pointing toward target
            //transform.localPosition = Vector3.Project(realHandPosition - rotatorObjectPosition, localTargetPosition);

            //project onto a vertical plane intersecting target and home
            Vector3 vectorForPlane = new Vector3(targetPosition.x, targetPosition.y - 1, targetPosition.z);
            Vector3 normalVector = Vector3.Cross(targetPosition - rotatorObjectPosition, vectorForPlane - rotatorObjectPosition);

            transform.localPosition = Vector3.ProjectOnPlane(realHandPosition - rotatorObjectPosition, normalVector);
        }

        else
        {
            Vector3 realHandPosition = realHand.transform.position;
            Vector3 rotatorObjectPosition = transform.parent.transform.position;

            transform.localPosition = realHandPosition - rotatorObjectPosition;
        }
	}
}
