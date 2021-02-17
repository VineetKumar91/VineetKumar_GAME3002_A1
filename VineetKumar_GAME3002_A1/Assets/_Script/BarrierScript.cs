using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    private GetAngle ArrowReference;

    private GoalScript GoalReference;

    private bool ready = true;
    // Start is called before the first frame update
    void Start()
    {
        ArrowReference = GameObject.FindGameObjectWithTag("Arrow").GetComponent<GetAngle>();
        GoalReference = GameObject.FindGameObjectWithTag("Goal").GetComponent<GoalScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision ballCollisionWithBarrier)
    {
        if (ready)
        {
            ready = false;
            // Reset if ball Collides with this barrier
            GoalReference.NoGoal();
            StartCoroutine(ResetCallMethod());
        }
        
    }

    // Delay needed before ball returns to original position
    IEnumerator ResetCallMethod()
    {
        //Debug.Log("Before Reset");
        yield return new WaitForSeconds(2);
        ArrowReference.Reset();
        GoalReference.setReady();
        ready = true;
        //Debug.Log("After Waiting 2 Seconds");
    }
}
