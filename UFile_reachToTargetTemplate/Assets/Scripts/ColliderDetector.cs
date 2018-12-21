using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;


public class ColliderDetector : MonoBehaviour {

    //public bool targetHit = false;

    public ExampleController exampleController;
    public TargetHolderController targetHolderController;
    public GameObject target;
    public GameObject trackerHolderObject;
    public GameObject homePosition;

    //for pausing to end trial
    //make list
    List<float> distanceFromLastList = new List<float>();
    Vector3 lastPosition;

    public bool isPaused = false;
    bool isInTarget = false;
    bool isInHome = false;

    private void OnTriggerEnter(Collider other)
    {
        //there should be an option for home too
        if (other.CompareTag("Target"))
        {
            isInTarget = true;
        }

        else if (other.CompareTag("Home"))
        {
            isInHome = true;
        }

        else if (other.CompareTag("HomeArea"))
        {
            lastPosition = transform.position;

            //clear the list
            distanceFromLastList.Clear();


            InvokeRepeating("CheckForPause", 0, 0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HomeArea"))
        {
            lastPosition = transform.position;

            //clear the list
            distanceFromLastList.Clear();


            InvokeRepeating("CheckForPause", 0, 0.1f);

            //start coroutine???
            //StartCoroutine("StartRecordingDistance");
        }

        else if (other.CompareTag("Target"))
        {
            isInTarget = false;
            
        }

        else if (other.CompareTag("Home"))
        {
            isInHome = false;
        }
    }

    //IEnumerator StartRecordingDistance()
    //{
    //    InvokeRepeating("RecordDistance", 0, 1);
    //    yield return null;
    //}

    

    private void LateUpdate()
    {
        //if cursor is visible..
        if (exampleController.trialType.Contains("rotated") || exampleController.trialType.Contains("clamped") || exampleController.trialType.Contains("aligned"))
        { 
            //if cursor is paused AND in target
            if (isPaused && isInTarget)
            {
                //disable the tracker script (for the return to home position)
                trackerHolderObject.GetComponent<PositionRotationTracker>().enabled = false;

                isPaused = false;
                isInTarget = false;

                CancelInvoke("CheckForPause");

                //make the cursor disappear
                GetComponent<MeshRenderer>().enabled = false;

                //start the next trial
                exampleController.EndAndPrepare();
            }

            if (isPaused && isInHome)
            {
                //Turn off home
                homePosition.SetActive(false);

                isInHome = false;
                isPaused = false;

                CancelInvoke("CheckForPause");

                //make the cursor reappear
                GetComponent<MeshRenderer>().enabled = true;

                //Create target
                //randomize location of target
                //Vector3 newTargetPosition = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), 0.7f, UnityEngine.Random.Range(0.35f, 0.45f));
                //Quaternion targetRotation = new Quaternion(0, 0, 0, 0);
                //Instantiate(target, exampleController.targetPosition, targetRotation);

                targetHolderController.InstantiateTarget();

                //enable the tracker script (for the reach to target)
                trackerHolderObject.GetComponent<PositionRotationTracker>().enabled = true;
            }
        }

        //if cursor is invisible
        else
        {
            //NOT CHECKING FOR PAUSE IN HOME FOR NO-CURSORS! FIX THIS
            //if cursor is paused
            if (isPaused)
            {
                //disable the tracker script (for the return to home position)
                trackerHolderObject.GetComponent<PositionRotationTracker>().enabled = false;

                isPaused = false;
                isInTarget = false;

                CancelInvoke("CheckForPause");

                //make the cursor disappear
                GetComponent<MeshRenderer>().enabled = false;

                //start the next trial
                exampleController.EndAndPrepare();
            }

            if (isInHome)
            {
                //Turn off home
                homePosition.SetActive(false);

                isInHome = false;
                isPaused = false;

                CancelInvoke("CheckForPause");

                //make the cursor reappear
                GetComponent<MeshRenderer>().enabled = true;

                //Create target
                //randomize location of target
                //Vector3 newTargetPosition = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), 0.7f, UnityEngine.Random.Range(0.35f, 0.45f));
                //Quaternion targetRotation = new Quaternion(0, 0, 0, 0);
                //Instantiate(target, exampleController.targetPosition, targetRotation);

                targetHolderController.InstantiateTarget();

                //enable the tracker script (for the reach to target)
                trackerHolderObject.GetComponent<PositionRotationTracker>().enabled = true;
            }
        }
    }


    public void CheckForPause()
    {
        //calculate the distance from last position
        float distance = Vector3.Distance(lastPosition, transform.position);

        float distanceMean = 1000;

        //add the distance to our List
        distanceFromLastList.Add(distance);

        //if List is over a certain length, check some stuff
        if(distanceFromLastList.Count > 8)
        {
            //check and print the average distance
            //float[] distanceArray = distanceFromLastList.ToArray();
            float distanceSum = 0f;

            for (int i = 0; i < distanceFromLastList.Count; i++)
            {
                distanceSum += distanceFromLastList[i];
            }

            distanceMean = distanceSum / distanceFromLastList.Count;

            distanceFromLastList.RemoveAt(0);
        }

        //replace lastPosition withh the current position
        lastPosition = transform.position;

        if(distanceMean < 0.01)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }
}
