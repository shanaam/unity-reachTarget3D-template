using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UXF;
using System;
using TMPro;


public class ExampleController : MonoBehaviour {

    Session session;

    public TextMeshPro instructionTextMesh;

    public GameObject homePositionObject;
    public GameObject handCursorObject;
    public GameObject clampedHandCursorObject;
    public GameObject trackerHolderObject;
    public GameObject rotatorObject;
    public GameObject targetHolder;
    public TargetHolderController targetHolderController;
    public InstructionController instructionController;

    public bool isDoneInstruction = false;
    public string trialType = null;
    //public Vector3 targetPosition = new Vector3();

    List<int> targetList = new List<int>();
    List<int> shuffledTargetList = new List<int>();
    int gradualStep;
    float rotationAngle;                                     //used to set rotation in EACH trial

    //GENERATE TRIALS AND BLOCK!!!
    private void Start()
    {
        //turn the tracker off until it's turned on when you hit the first home position sphere
        trackerHolderObject.GetComponent<PositionRotationTracker>().enabled = false;

        // disable the whole task initially to give time for the experimenter to use the UI
        gameObject.SetActive(false);
    }
    public void GenerateExperiment(Session experimentSession)
    {
        //get a reference to session
        //whatever Session class thing I give this (expSession) will be the session this thing uses as the private reference I made earlier
        session = experimentSession;

        //after I do _________ ExperimentSession.settings will have all the settings in the JSON file

        float rotationSize1 = Convert.ToInt32(session.settings["rotation_1"]);
        float rotationSize2 = Convert.ToInt32(session.settings["rotation_2"]);
        gradualStep = Convert.ToInt32(session.settings["gradual_step"]);

        //makes the blocks and trials!
        //first grab the settings to figure out trial numbers and make the first BLOCK
        int numAlignedTrials1 = Convert.ToInt32(session.settings["num_trials_aligned_reach_1"]);
        Block alignedReachBlock1 = session.CreateBlock(numAlignedTrials1);
        alignedReachBlock1.settings["trial_type"] = "aligned";
        alignedReachBlock1.settings["visible_cursor"] = true;
        alignedReachBlock1.settings["rotation"] = 0;
        alignedReachBlock1.settings["show_instruction"] = true;
        alignedReachBlock1.settings["instruction_text"] = "Reach to the Target";

        //make the first rotated block
        int numRotatedTrials1 = Convert.ToInt32(session.settings["num_trials_rotated_reach_1"]);
        bool isGradual1 = Convert.ToBoolean(session.settings["is_gradual_1"]);
        Block rotatedReachBlock1 = session.CreateBlock(numRotatedTrials1);
        rotatedReachBlock1.settings["trial_type"] = "rotated_1";
        rotatedReachBlock1.settings["visible_cursor"] = true;
        rotatedReachBlock1.settings["rotation"] = rotationSize1;
        rotatedReachBlock1.settings["show_instruction"] = false;
        rotatedReachBlock1.settings["instruction_text"] = "Reach to the Target";
        rotatedReachBlock1.settings["is_gradual"] = isGradual1;


        //make the second rotated block
        int numRotatedTrials2 = Convert.ToInt32(session.settings["num_trials_rotated_reach_2"]);
        bool isGradual2 = Convert.ToBoolean(session.settings["is_gradual_2"]);
        Block rotatedReachBlock2 = session.CreateBlock(numRotatedTrials2);
        rotatedReachBlock2.settings["trial_type"] = "rotated_2";
        rotatedReachBlock2.settings["visible_cursor"] = true;
        rotatedReachBlock2.settings["rotation"] = rotationSize2;
        rotatedReachBlock2.settings["show_instruction"] = false;
        rotatedReachBlock2.settings["instruction_text"] = "Reach to the Target";
        rotatedReachBlock2.settings["is_gradual"] = isGradual2;


        //make the no_cursor blocks (open JSON file to check the correct names)
        int numNoCursorTrials1 = Convert.ToInt32(session.settings["num_trials_noCursor_reach_1"]);
        Block noCursorBlock1 = session.CreateBlock(numNoCursorTrials1);
        noCursorBlock1.settings["trial_type"] = "no_cursor";
        noCursorBlock1.settings["visible_cursor"] = false;
        noCursorBlock1.settings["rotation"] = 0;
        noCursorBlock1.settings["show_instruction"] = true;
        noCursorBlock1.settings["instruction_text"] = "Reach WITHOUT Strategy";

        //make the clamped blocks (open JSON file to check the correct names)
        int numClampedTrials1 = Convert.ToInt32(session.settings["num_trials_clamped_reach_1"]);
        Block clampedBlock1 = session.CreateBlock(numClampedTrials1);
        clampedBlock1.settings["trial_type"] = "clamped";
        clampedBlock1.settings["visible_cursor"] = false;
        clampedBlock1.settings["rotation"] = 0;
        clampedBlock1.settings["show_instruction"] = false;
        clampedBlock1.settings["instruction_text"] = "Reach to the Target";


        //quit the game if any of the trial numbers are not divisible by the number of trials
        int minTarget = Convert.ToInt32(session.settings["min_target"]);
        int maxTarget = Convert.ToInt32(session.settings["max_target"]);
        int numTargets = Convert.ToInt32(session.settings["num_targets"]);

        if(Math.Abs(maxTarget - minTarget) % numTargets != 0)
        {
            Debug.Log("WARNING: Check your trial settings for target positions and numbers");
        }

        int targetStep = Math.Abs(maxTarget - minTarget) / (numTargets - 1);

        for(int i = numTargets; i > 0; i--)
        {
            //add min target to the list
            targetList.Add(minTarget);

            //change min target to next target
            minTarget += targetStep;
        }
    }

    //START A TRIAL!
    //call this next one on the "On Trial Begin" event
    public void StartReachTrial(Trial trial)
    {
        //Debug.Log("starting reach trial!");

        // Show instructions when required
        // If the trial is the first trial in the block
        if (trial.numberInBlock == 1)
        {
            //Set the instruction text to instruction_text
            instructionTextMesh.text = Convert.ToString(trial.settings["instruction_text"]);

            // If showInstruction is true
            if (Convert.ToBoolean(trial.settings["show_instruction"]) == true)
            {
                isDoneInstruction = false;
                Debug.Log("show instruction = true,  expanding");

                // transition to the big instruction, change the text
                instructionController.ExpandInstruction();
                instructionController.IsStill();
            }

            else if (Convert.ToBoolean(trial.settings["show_instruction"]) == false)
            {
                Debug.Log("show instruction = false,  doing nothing");
                isDoneInstruction = true;
            }

        }

        //Pseudorandom target location
        if (shuffledTargetList.Count < 1)
        {
            shuffledTargetList = new List<int>(targetList);
            shuffledTargetList.Shuffle();
        }

        int targetLocation = shuffledTargetList[0];

        //print(targetLocation);
        //remove the used target from the list
        shuffledTargetList.RemoveAt(0);

        //determine Target Position (used by ColliderDetector to instantiate the target)
        //rotate the target holder (this just needs to be done for some reason..)
        targetHolder.transform.rotation = Quaternion.Euler(0, targetLocation - 90, 0);

        //check for clamped
        if (Convert.ToString(trial.settings["trial_type"]).Contains("clamped"))
        {
            handCursorObject.SetActive(false);
            
            clampedHandCursorObject.SetActive(true);
            clampedHandCursorObject.GetComponent<MeshRenderer>().enabled = false;
            //print("setting clamped to active");
        }
        else
        {
            handCursorObject.SetActive(true);
            clampedHandCursorObject.SetActive(false);
            //print("setting clamped to inactive");
        }

        //set the rotation for this trial

        
        if (Convert.ToBoolean(trial.settings["is_gradual"]) && trial.numberInBlock <= Math.Abs((Convert.ToSingle(trial.settings["rotation"]))))
        {
            // add gradualStep if positive, subtract if negative
            rotationAngle = (trial.numberInBlock - 1) * gradualStep * Math.Sign(Convert.ToSingle(trial.settings["rotation"]));
            Debug.Log("if statement works for this trial");
        } 
        else
        {
            rotationAngle = Convert.ToSingle(trial.settings["rotation"]);
        }
        rotatorObject.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        Debug.Log(rotationAngle);


        //Create homeposition
        homePositionObject.SetActive(true);

        // explicitly convert settings["visible_cursor"] to a boolean for if statement
        // this is just to save in the trial by trial csv
        bool visibleCursor = Convert.ToBoolean(trial.settings["visible_cursor"]);

        trialType = Convert.ToString(trial.settings["trial_type"]);

        //Debug.LogFormat("the cursor is {0}", trialType);

        //add these things to the trial_results csv (per trial)
        trial.result["trial_type"] = trial.settings["trial_type"];
        trial.result["cursor_visibility"] = trial.settings["visible_cursor"];
        trial.result["rotation"] = trial.settings["rotation"];
        trial.result["target_angle"] = targetLocation;
    }

    private void Update()
    {
        //bool visibleCursor = Convert.ToBoolean(session.currentTrial.settings["visible_cursor"]);

        //make the Hand Cursor invisible if
        //if (trialType.Contains("no_cursor"))
        //{
        //    GameObject.Find("Hand Cursor").GetComponent<MeshRenderer>().enabled = false;
        //}

        //move to next trial IF handIsPause AND hand is touching target.. (in ColliderDetector)
    }

    // end session or begin next trial (used for an example, find this in Hand Cursor's OnTriggerEnter method
    public void EndAndPrepare()
    {
        //Debug.Log("ending reach trial...");

        //Destroy old target
        targetHolderController.DestroyTarget();

        session.currentTrial.End();

        if (session.currentTrial == session.lastTrial)
        {
            session.End();
        }
        else
        {
            session.BeginNextTrial();
        }
    }

    ////Unused for now but useful
    //IEnumerator WaitAFrame()
    //{
    //    //returning 0 will make it wait 1 frame
    //    yield return 0;
    //}
}
