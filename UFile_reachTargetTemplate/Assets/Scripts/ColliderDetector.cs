using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;


public class ColliderDetector : MonoBehaviour {

    //public bool targetHit = false;

    public ExampleController exampleController;
    public TargetHolderController targetHolderController;
    public InstructionController instructionController;
    public GameObject target;
    public GameObject trackerHolderObject;
    public GameObject homePosition;
    public GameObject returnHelper;

    //for pausing to end trial
    //make list
    List<float> distanceFromLastList = new List<float>();
    Vector3 lastPosition;

    public bool isPaused = false;
    bool isInTarget = false;
    bool isInHome = false;
    bool isInHomeArea = false;
    bool isInAcceptor = false;
    bool targetReached = false;

    float checkForPauseRate = 0.05f;

    private void OnTriggerEnter(Collider other)
    {
        //there should be an option for home too
        if (other.CompareTag("Target"))
        {
            isInTarget = true;
        }

        else if (other.CompareTag("Home"))
        {
            // make the controller vibrate
            exampleController.ShortVibrateController();

            isInHome = true;
        }

        else if (other.CompareTag("HomeArea"))
        {
            isInHomeArea = true;

            lastPosition = transform.position;

            //clear the list
            distanceFromLastList.Clear();


            InvokeRepeating("CheckForPause", 0, checkForPauseRate);
            //Debug.Log("Check For Pause!");

            // Activate return helper for clamped and no cursor trials
            if (exampleController.trialType.Contains("no_cursor") || exampleController.trialType.Contains("clamped"))
            {
                if (targetReached)
                {
                    returnHelper.SetActive(true);
                }
            }
        }

        else if (other.CompareTag("InstructionAcceptor"))
        {
            //Debug.Log("is in Acceptor!");
            isInAcceptor = true;

            InvokeRepeating("CheckForPause", 0, checkForPauseRate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HomeArea"))
        {
            isInHomeArea = false;
            isPaused = false;

            lastPosition = transform.position;

            //clear the list
            distanceFromLastList.Clear();


            InvokeRepeating("CheckForPause", 0, checkForPauseRate);

            //start coroutine???
            //StartCoroutine("StartRecordingDistance");

            // Deactivate return helper after clamped and no cursor trials
            returnHelper.SetActive(false);
        }

        else if (other.CompareTag("Target"))
        {
            isInTarget = false;
            
        }

        else if (other.CompareTag("Home"))
        {
            isInHome = false;
        }

        else if (other.CompareTag("InstructionAcceptor"))
        {
            isInAcceptor = false;
        }
    }
    
    private void LateUpdate()
    {
        if (isPaused && isInAcceptor)
        {
            //ANIMATION
            instructionController.ShrinkInstructions();
            instructionController.IsStill();

            exampleController.isDoneInstruction = true;

            isPaused = false;

            CancelInvoke("CheckForPause");

        }

        else
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

                    targetReached = true;

                    //Destroy old target 
                    targetHolderController.DestroyTarget();

                    //Create homeposition
                    homePosition.SetActive(true);
                }

                if (isPaused && isInHome && exampleController.isDoneInstruction)
                {
                    //Turn off home
                    homePosition.SetActive(false);

                    isInHome = false;
                    //isPaused = false;

                    CancelInvoke("CheckForPause");

                    //Create random target
                    //Vector3 newTargetPosition = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), 0.7f, UnityEngine.Random.Range(0.35f, 0.45f));
                    //Quaternion targetRotation = new Quaternion(0, 0, 0, 0);
                    //Instantiate(target, exampleController.targetPosition, targetRotation);

                    //instantiate target
                    //targetHolderController.InstantiateTarget();

                    if (targetReached)
                    {
                        targetReached = false;
                        //start the next trial
                        exampleController.EndAndPrepare();
                    }
                    else
                    {
                        //Create TARGET
                        targetHolderController.InstantiateTarget();

                        //enable the tracker script (for the reach to target)
                        trackerHolderObject.GetComponent<PositionRotationTracker>().enabled = true;
                    }
                }
            }

            //if cursor is invisible
            else
            {
                //if cursor is paused
                if (isPaused && !isInHomeArea)
                {
                    //disable the tracker script (for the return to home position)
                    trackerHolderObject.GetComponent<PositionRotationTracker>().enabled = false;

                    isPaused = false;
                    isInTarget = false;

                    CancelInvoke("CheckForPause");

                    //Destroy old target 
                    targetHolderController.DestroyTarget();

                    //Create homeposition
                    homePosition.SetActive(true);

                    // set target reached -- so that a trial can end (in non-noCursor tasks, this happens when people actually reach the target)
                    targetReached = true;
                }

                if (isInHome && isPaused && exampleController.isDoneInstruction)
                {
                    //Turn off home
                    homePosition.SetActive(false);

                    isInHome = false;
                    //isPaused = false;

                    CancelInvoke("CheckForPause");

                    //Create random target
                    //randomize location of target
                    //Vector3 newTargetPosition = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), 0.7f, UnityEngine.Random.Range(0.35f, 0.45f));
                    //Quaternion targetRotation = new Quaternion(0, 0, 0, 0);
                    //Instantiate(target, exampleController.targetPosition, targetRotation);
        
                    if (targetReached)
                    {
                        targetReached = false;
                        //start the next trial
                        exampleController.EndAndPrepare();
                    }
                    else
                    {
                        //Create TARGET
                        targetHolderController.InstantiateTarget();

                        //enable the tracker script (for the reach to target)
                        trackerHolderObject.GetComponent<PositionRotationTracker>().enabled = true;
                    }
                }
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

        if(distanceMean < 0.001)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }

    //private void Start()
    //{
    //    Debug.Log(gameObject.name);
    //}

}
