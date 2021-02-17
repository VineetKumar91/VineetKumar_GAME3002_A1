using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GoalTextAdjustScriptForPrefab : MonoBehaviour
{
    // TMP object
    private TextMeshPro textMesh;

    // How long the text stays on screen
    private float disappearTimer;

    // Color reference to adjust the alpha for transparency effect
    private Color textColor;
    
    // Const for max time to disappear
    private const float DISAPPEAR_TIMER_MAX = 1f;

    // Create the prefab using GameAssets and Instantiate it
    public static GoalTextAdjustScriptForPrefab Create(bool goal)
    {
        // Instantiate with position and angle set to quaternion euler angle for x axis to match with camera angle
        Transform referenceToPopUpTransform;

        // If it's for GOAL OR NO GOAL
        if (goal)
        {
            referenceToPopUpTransform = Instantiate(GameAssets.i.pfGTASFP, new Vector3(2.0f, 1f, 4.5f), Quaternion.Euler(new Vector3(30.0f, 0f, 0f)));
        }
        else
        {
            referenceToPopUpTransform = Instantiate(GameAssets.i.pfGTASFP, new Vector3(2.0f, 1f, 4.5f), Quaternion.Euler(new Vector3(30.0f, 0f, 0f)));
        }
        
        // We get the reference for that prefab able to call the functions on it
        GoalTextAdjustScriptForPrefab gtasfp = referenceToPopUpTransform.GetComponent<GoalTextAdjustScriptForPrefab>();
        gtasfp.SetGoalNoGoal(goal);

        return gtasfp;
    }
    private void Awake()
    {
        // Assigns the transform of the tmp prefab to our variable
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    // Sets the text and color whether the ball collision that happened was in the goal or not
    // cyan color for GOAL, red color for NO GOAL
    public void SetGoalNoGoal(bool goal)
    {
        if (goal)
        {
            textMesh.SetText("GOAL");
            textMesh.color = Color.cyan;
            textColor = textMesh.color;
        }
        else
        {
            textMesh.SetText("NO GOAL");
            textMesh.color = Color.red;
            textColor = textMesh.color;
        }

        // set a disappear timer so it will do whatever
        // has to be done and disappear and destroy
        disappearTimer = DISAPPEAR_TIMER_MAX;
    }

    private void Update()
    {
        disappearTimer -= Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
        {
            // increase
            float increaseScaleAmount = 10f;
            textMesh.fontSize += increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // decrease
            float decreaseScaleAmount = 10f;
            textMesh.fontSize -= decreaseScaleAmount * Time.deltaTime;
        }
        

        if (disappearTimer < 0)
        {
            // Ready to disappear with reducing alpha to 0
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            // Alpha is now 0, destroy that object
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
