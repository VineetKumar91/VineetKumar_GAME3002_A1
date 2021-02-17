using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeperScript : MonoBehaviour
{
    [SerializeField]
    private GameObject GoalKeeper;

    private Transform m_gkTransform;

    private bool m_bMoveRight = true;
    // Left Position and Right Position Max X value storing, with Y and Z preserved
    private Vector3 RightPos;
    private Vector3 LeftPos;

    // Speed for Lerping
    private float m_fMovingFactor = 5f;

    
    // Start is called before the first frame update
    void Start()
    {
        // Get components and preset the values required
        m_gkTransform = GoalKeeper.GetComponent<Transform>().transform;
        RightPos = m_gkTransform.position;
        LeftPos = m_gkTransform.position;

        // We need only x to be affected NOT Y or Z
        RightPos.x = 5.65f;
        LeftPos.x = -5.65f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bMoveRight)
        {
            // Lerp to Position. With the value that are set, only X-axis Lerping takes place
            m_gkTransform.position = Vector3.Lerp(m_gkTransform.position, RightPos, m_fMovingFactor * Time.deltaTime);
            if (m_gkTransform.position.x >= 5.6f)
            {
                m_bMoveRight = false;
            }
            
        }
        else
        {
            // Lerp to Position. With the value that are set, only X-axis Lerping takes place
            m_gkTransform.position = Vector3.Lerp(m_gkTransform.position, LeftPos, m_fMovingFactor *Time.deltaTime);
            if (m_gkTransform.position.x <= -5.6f)
            {
                m_bMoveRight = true;
            }
        }
    }
}
