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

    List<int> targetList_1 = new List<int>();
    List<int> targetList_2 = new List<int>();
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

        float rotationSize1_1 = Convert.ToInt32(session.settings["rotation_1_1"]);
        float rotationSize1_2 = Convert.ToInt32(session.settings["rotation_1_2"]);
        float rotationSize2_1 = Convert.ToInt32(session.settings["rotation_2_1"]);
        float rotationSize2_2 = Convert.ToInt32(session.settings["rotation_2_2"]);

        gradualStep = Convert.ToInt32(session.settings["gradual_step"]);

        //makes the blocks and trials!
        //first grab the settings to figure out trial numbers and make the first BLOCK
        int numAlignedTrials1 = Convert.ToInt32(session.settings["num_trials_aligned_reach_1_1"]);
        Block alignedReachBlock1_1 = session.CreateBlock(numAlignedTrials1);
        alignedReachBlock1_1.settings["trial_type"] = "aligned_1";
        alignedReachBlock1_1.settings["visible_cursor"] = true;
        alignedReachBlock1_1.settings["rotation"] = 0;
        alignedReachBlock1_1.settings["show_instruction"] = true;
        alignedReachBlock1_1.settings["instruction_text"] = "Reach to the Target";
        alignedReachBlock1_1.settings["target_list_to_use"] = 1;


        //make the first rotated block
        int numRotatedTrials1_1 = Convert.ToInt32(session.settings["num_trials_rotated_reach_1_1"]);
        bool isGradual1_1 = Convert.ToBoolean(session.settings["is_gradual_1_1"]);
        Block rotatedReachBlock1_1 = session.CreateBlock(numRotatedTrials1_1);
        rotatedReachBlock1_1.settings["trial_type"] = "rotated_1_1";
        rotatedReachBlock1_1.settings["visible_cursor"] = true;
        rotatedReachBlock1_1.settings["rotation"] = rotationSize1_2;
        rotatedReachBlock1_1.settings["show_instruction"] = false;
        rotatedReachBlock1_1.settings["instruction_text"] = "Reach to the Target";
        rotatedReachBlock1_1.settings["is_gradual"] = isGradual1_1;
        rotatedReachBlock1_1.settings["target_list_to_use"] = 1;


        //make the second rotated block
        int numRotatedTrials1_2 = Convert.ToInt32(session.settings["num_trials_rotated_reach_1_2"]);
        bool isGradual1_2 = Convert.ToBoolean(session.settings["is_gradual_1_2"]);
        Block rotatedReachBlock1_2 = session.CreateBlock(numRotatedTrials1_2);
        rotatedReachBlock1_2.settings["trial_type"] = "rotated_1_2";
        rotatedReachBlock1_2.settings["visible_cursor"] = true;
        rotatedReachBlock1_2.settings["rotation"] = rotationSize1_2;
        rotatedReachBlock1_2.settings["show_instruction"] = false;
        rotatedReachBlock1_2.settings["instruction_text"] = "Reach to the Target";
        rotatedReachBlock1_2.settings["is_gradual"] = isGradual1_2;
        rotatedReachBlock1_2.settings["target_list_to_use"] = 1;


        ////make the no_cursor blocks (open JSON file to check the correct names)
        //int numNoCursorTrials1 = Convert.ToInt32(session.settings["num_trials_noCursor_reach_1"]);
        //Block noCursorBlock1 = session.CreateBlock(numNoCursorTrials1);
        //noCursorBlock1.settings["trial_type"] = "no_cursor";
        //noCursorBlock1.settings["visible_cursor"] = false;
        //noCursorBlock1.settings["rotation"] = 0;
        //noCursorBlock1.settings["show_instruction"] = true;
        //noCursorBlock1.settings["instruction_text"] = "Reach WITHOUT Strategy";

        //make the clamped blocks (open JSON file to check the correct names)
        int numClampedTrials1 = Convert.ToInt32(session.settings["num_trials_clamped_reach_1_1"]);
        Block clampedBlock1_1 = session.CreateBlock(numClampedTrials1);
        clampedBlock1_1.settings["trial_type"] = "clamped_1";
        clampedBlock1_1.settings["visible_cursor"] = false;
        clampedBlock1_1.settings["rotation"] = 0;
        clampedBlock1_1.settings["show_instruction"] = false;
        clampedBlock1_1.settings["instruction_text"] = "Reach to the Target";
        clampedBlock1_1.settings["target_list_to_use"] = 1;



        //Session2
        //aligned block
        int numAlignedTrials2 = Convert.ToInt32(session.settings["num_trials_aligned_reach_2_1"]);
        Block alignedReachBlock2_1 = session.CreateBlock(numAlignedTrials2);
        alignedReachBlock2_1.settings["trial_type"] = "aligned_2";
        alignedReachBlock2_1.settings["visible_cursor"] = true;
        alignedReachBlock2_1.settings["rotation"] = 0;
        alignedReachBlock2_1.settings["show_instruction"] = true;
        alignedReachBlock2_1.settings["instruction_text"] = "Reach to the Target";
        alignedReachBlock2_1.settings["target_list_to_use"] = 2;


        //make the first rotated block
        int numRotatedTrials2_1 = Convert.ToInt32(session.settings["num_trials_rotated_reach_2_1"]);
        bool isGradual2_1 = Convert.ToBoolean(session.settings["is_gradual_2_1"]);
        Block rotatedReachBlock2_1 = session.CreateBlock(numRotatedTrials2_1);
        rotatedReachBlock2_1.settings["trial_type"] = "rotated_2_1";
        rotatedReachBlock2_1.settings["visible_cursor"] = true;
        rotatedReachBlock2_1.settings["rotation"] = rotationSize2_1;
        rotatedReachBlock2_1.settings["show_instruction"] = false;
        rotatedReachBlock2_1.settings["instruction_text"] = "Reach to the Target";
        rotatedReachBlock2_1.settings["is_gradual"] = isGradual2_1;
        rotatedReachBlock2_1.settings["target_list_to_use"] = 2;


        //make the second rotated block
        int numRotatedTrials2_2 = Convert.ToInt32(session.settings["num_trials_rotated_reach_2_2"]);
        bool isGradual2_2 = Convert.ToBoolean(session.settings["is_gradual_2_2"]);
        Block rotatedReachBlock2_2 = session.CreateBlock(numRotatedTrials2_2);
        rotatedReachBlock2_2.settings["trial_type"] = "rotated_2_2";
        rotatedReachBlock2_2.settings["visible_cursor"] = true;
        rotatedReachBlock2_2.settings["rotation"] = rotationSize2_2;
        rotatedReachBlock2_2.settings["show_instruction"] = false;
        rotatedReachBlock2_2.settings["instruction_text"] = "Reach to the Target";
        rotatedReachBlock2_2.settings["is_gradual"] = isGradual2_2;
        rotatedReachBlock2_2.settings["target_list_to_use"] = 2;


        //make the clamped blocks (open JSON file to check the correct names)
        int numClampedTrials2 = Convert.ToInt32(session.settings["num_trials_clamped_reach_2_1"]);
        Block clampedBlock2_1 = session.CreateBlock(numClampedTrials2);
        clampedBlock2_1.settings["trial_type"] = "clamped_2";
        clampedBlock2_1.settings["visible_cursor"] = false;
        clampedBlock2_1.settings["rotation"] = 0;
        clampedBlock2_1.settings["show_instruction"] = false;
        clampedBlock2_1.settings["instruction_text"] = "Reach to the Target";
        clampedBlock2_1.settings["target_list_to_use"] = 2;



        //quit the game if any of the trial numbers are not divisible by the number of trials
        int minTarget = Convert.ToInt32(session.settings["min_target_1"]);
        int maxTarget = Convert.ToInt32(session.settings["max_target_1"]);
        int numTargets = Convert.ToInt32(session.settings["num_targets_1"]);

        if(Math.Abs(maxTarget - minTarget) % numTargets != 0)
        {
            Debug.Log("WARNING: Check your trial settings for target positions and numbers");
        }

        int targetStep = Math.Abs(maxTarget - minTarget) / (numTargets - 1);

        for(int i = numTargets; i > 0; i--)
        {
            //add min target to the list
            targetList_1.Add(minTarget);

            //change min target to next target
            minTarget += targetStep;
        }

        //quit the game if any of the trial numbers are not divisible by the number of trials
        minTarget = Convert.ToInt32(session.settings["min_target_2"]);
        maxTarget = Convert.ToInt32(session.settings["max_target_2"]);
        numTargets = Convert.ToInt32(session.settings["num_targets_2"]);

        if (Math.Abs(maxTarget - minTarget) % numTargets != 0)
        {
            Debug.Log("WARNING: Check your trial settings for target positions and numbers");
        }

        targetStep = Math.Abs(maxTarget - minTarget) / (numTargets - 1);

        for (int i = numTargets; i > 0; i--)
        {
            //add min target to the list
            targetList_2.Add(minTarget);

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
            if (Convert.ToInt32(trial.settings["target_list_to_use"]) == 1)
            {
                shuffledTargetList = new List<int>(targetList_1);
            }
            else if (Convert.ToInt32(trial.settings["target_list_to_use"]) == 2)
            {
                shuffledTargetList = new List<int>(targetList_2);
            }
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
            //clampedHandCursorObject.GetComponent<MeshRenderer>().enabled = false;
            //print("setting clamped to active");
        }
        else
        {
            handCursorObject.SetActive(true);
            clampedHandCursorObject.SetActive(false);
            //print("setting clamped to inactive");
        }

        //set the rotation for this trial

        // first check if this is a gradual rotation block
        if (Convert.ToBoolean(trial.settings["is_gradual"]) && trial.numberInBlock <= Math.Abs((Convert.ToSingle(trial.settings["rotation"]))))
        {
            // add gradualStep if positive, subtract if negative
            rotationAngle = (trial.numberInBlock - 1) * gradualStep * Math.Sign(Convert.ToSingle(trial.settings["rotation"]));
        } 
        else
        {
            rotationAngle = Convert.ToSingle(trial.settings["rotation"]);
        }
        rotatorObject.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        Debug.Log(rotationAngle);

        // explicitly convert settings["visible_cursor"] to a boolean for if statement
        // this is just to save in the trial by trial csv
        bool visibleCursor = Convert.ToBoolean(trial.settings["visible_cursor"]);

        trialType = Convert.ToString(trial.settings["trial_type"]);

        //Debug.LogFormat("the cursor is {0}", trialType);

        //add these things to the trial_results csv (per trial)
        trial.result["trial_type"] = trial.settings["trial_type"];
        trial.result["cursor_visibility"] = trial.settings["visible_cursor"];
        trial.result["rotation"] = rotationAngle;
        trial.result["target_angle"] = targetLocation;

        //Create homeposition
        homePositionObject.SetActive(true);
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
