using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using Vector3 = UnityEngine.Vector3;

// Calculate angles for getting the velocity to be set on the ball
public class GetAngle : MonoBehaviour
{

    // Confirmation Material
    public Material DefaultMaterial;
    public Material ConfirmMaterial;

    // Pitch and Yaw
    [SerializeField]
    public float Yaw = 0.0f;

    [SerializeField]
    public float Pitch = 0.0f;

    [SerializeField] 
    public Slider heightAdjusterSlider;

    [SerializeField] 
    private GameObject Football;

    private float m_fHeightAdjusterValue = 0.025f;
    //private bool m_bIncrease = true;

    public float m_fMouseSpeed = 2.0f;

    // Pitch and Yaw to Use
    public float m_fProjectileYaw;
    public float m_fProjectilePitch;

    public bool YawAngleSet = false;
    public bool PitchAngleSet = false;

    // Kick the football
    private bool m_bKickedBall = false;
    private Rigidbody m_footBallRigidbody;

    // A TMP reference to change color in case player fired without adjusting the vertical angle
    // NOT IN USE ANYMORE
    [SerializeField]
    private TextMeshProUGUI verticalAngleText;

    // Slider for adjusting force
    [SerializeField]
    public Slider forceAdjusterSider;

    // Charge Bar Slider
    private float m_fCurrentTime = 0.0f;
    private float m_fTotatlTime = 0.0f;
    private float m_fMaxChargeTime = 2.0f;
    public float MaxKickingForce = 40.0f;
    public float CurrentKickingForce = 0.5f;

    // UI for Aim Hint
    [SerializeField]
    private LineRenderer lineTrajectoryHint;
    [SerializeField]
    private int numOfTrajectoryPoints = 5;

    private Vector3 initialPosition = Vector3.zero;
    private Vector3 BallVelocity = Vector3.zero;
    private float m_fPower = 20f;


    // Start is called before the first frame update
    void Start()
    {
        m_footBallRigidbody = Football.GetComponent<Rigidbody>();
        Assert.IsNotNull(m_footBallRigidbody, "Rigidbody for football not attached.");
        m_footBallRigidbody.velocity = Vector3.zero;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If Yaw is not set and not Kicked the Ball yet
        if (!YawAngleSet && !m_bKickedBall)
        {
            getYawAngle();
        }
        else
        {
            // If Yaw is set but Pitch is not set AND not Kicked the Ball yet
            if (!m_bKickedBall)
            {
                getPitchAngle();
            }
        }

        if (YawAngleSet)
        {
            // Mouse Down, Start Charging
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_fCurrentTime = Time.time;
                //Debug.Log("Charge button Down");
            }

            if (Input.GetKey(KeyCode.Space))
            {
                m_fTotatlTime = Time.time - m_fCurrentTime;
                if (m_fTotatlTime >= m_fMaxChargeTime)
                {
                    m_fTotatlTime = m_fMaxChargeTime;
                }
                // Show the slider bar progress

                forceAdjusterSider.value = (m_fTotatlTime / m_fMaxChargeTime);

               // Debug.Log("Charge button Down");
            }

            // Mouse Up, Done Charging
            if (Input.GetKeyUp(KeyCode.Space))
            {
                m_fTotatlTime = Time.time - m_fCurrentTime;
                if (m_fTotatlTime >= m_fMaxChargeTime)
                {
                    m_fTotatlTime = m_fMaxChargeTime;
                }

                // Calculating max kicking force between 0 and 40N for now.
                CurrentKickingForce = (MaxKickingForce * forceAdjusterSider.value);

                // If player kicks ball by mistake :P without anything set
                if (!m_bKickedBall)
                {
                    // Show the player vertical angle not set
                    if (!PitchAngleSet)
                    {
                        verticalAngleText.faceColor = Color.red;
                    }
                    
                    // KICK AS SOON AS YOU GET SOME FORCE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    KickTheBall();
                }

                //Debug.Log("Charge button Up, Value = " + CurrentKickingForce);
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

       
        DrawTrajectoryLine(Yaw - 90, Pitch);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
    }

    void getYawAngle()
    {
        transform.eulerAngles = new Vector3(0.0f, Yaw, 90.0f);
        Yaw += (m_fMouseSpeed * Input.GetAxis("Mouse X"));
        Yaw = Mathf.Clamp(Yaw, 30.0f, 150.0f);

        if (Input.GetMouseButtonDown(0))
        {
            m_fProjectileYaw = Yaw - 90.0f;
            YawAngleSet = true;
            GetComponent<MeshRenderer>().material = ConfirmMaterial;
            //Debug.Log("Yaw Set!");
        }
    }


    void getPitchAngle()
    {
        // Auto height adjuster functionality
       /*if (m_bIncrease)
        {
            heightAdjusterSlider.value += m_fHeightAdjusterValue;

            if (heightAdjusterSlider.value >= heightAdjusterSlider.maxValue)
            {
                m_bIncrease = false;
            }
        }
        else
        {
            heightAdjusterSlider.value -= m_fHeightAdjusterValue;

            if (heightAdjusterSlider.value <= heightAdjusterSlider.minValue)
            {
                m_bIncrease = true;
            }
        }*/

        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f || Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Debug.Log("Scroll Wheel Up");
            heightAdjusterSlider.value += m_fHeightAdjusterValue;
            if (heightAdjusterSlider.value >= heightAdjusterSlider.maxValue)
            {
                heightAdjusterSlider.value = heightAdjusterSlider.maxValue;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f || Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Debug.Log("Scroll Wheel Down");
            heightAdjusterSlider.value -= m_fHeightAdjusterValue;
            if (heightAdjusterSlider.value <= heightAdjusterSlider.minValue)
            {
                heightAdjusterSlider.value = heightAdjusterSlider.minValue;
            }
        }

      
        Pitch = m_fProjectilePitch = 90 * heightAdjusterSlider.value;
        PitchAngleSet = true;
        //Debug.Log("Pitch SET!");
        //verticalAngleText.faceColor = Color.green;
        

    }

    // Kicking the ball function
    void KickTheBall()
    {
        //Debug.Log("Kicking the Ball now!");
        Vector3 tempVelocity = Vector3.zero;
        
        m_footBallRigidbody.velocity = BallVelocity = calculateVelocityOfBall(CurrentKickingForce, m_fProjectileYaw,m_fProjectilePitch);

        m_bKickedBall = true;
        lineTrajectoryHint.enabled = false;
    }

    // Angular Velocity Calculator
    public Vector3 calculateVelocityOfBall(float kickingForce, float currentYaw, float currentPitch)
    {
        Vector3 tempVelocity = Vector3.zero;

        float calcYaw = currentYaw * Mathf.Deg2Rad;
        float calcPitch = currentPitch * Mathf.Deg2Rad;

        if (kickingForce <= 0)
        {
            kickingForce = 0.5f;
        }

        // OpenGL Formulas changed as per Unity's world space - direction and appropriate coordinate system.
        // front.x = cos(glm::radians(pitch)) * cos(glm::radians(yaw));
        // front.y = sin(glm::radians(pitch));
        // front.z = cos(glm::radians(pitch)) * sin(glm::radians(yaw));


        tempVelocity.x = Mathf.Cos(calcPitch) * Mathf.Sin(calcYaw) * kickingForce;
        tempVelocity.y = Mathf.Sin(calcPitch) * kickingForce;
        tempVelocity.z = Mathf.Cos(calcPitch) * Mathf.Cos(calcYaw) * kickingForce;

        return tempVelocity;
    }

    // Reset the overall elements of the game responsible for giving the ball a projectile motion
    public void Reset()
    {
        Debug.Log("Game Elements Reset!");

        // Change Material of Arrow
        GetComponent<MeshRenderer>().material = DefaultMaterial;

        // Has not kicked ball
        m_bKickedBall = false;
        // Velocity and Position set to origin and zero
        m_footBallRigidbody.velocity = Vector3.zero;
        m_footBallRigidbody.angularVelocity = Vector3.zero;
        m_footBallRigidbody.transform.position = Vector3.zero;


        // Yaw meter and value, Pitch meter and value set to default
        Yaw = 90.0f;
        Pitch = 0.0f;
        m_fProjectileYaw = 0.0f;
        m_fProjectilePitch = 0.0f;

        // Reset the arrow position
        transform.eulerAngles = new Vector3(0.0f, Yaw, 90.0f);

        // Reset Height Slider Position
        heightAdjusterSlider.value = 0.0f;

        // Bool of angle set for functionality reset
        YawAngleSet = false;
        PitchAngleSet = false;

        // Reset Kicking Force
        CurrentKickingForce = 0.5f;

        // Reset Force Slider Position
        forceAdjusterSider.value = 0.0f;

        // Now show Line Trajectory
        lineTrajectoryHint.enabled = true;

        // Time will always be correct, but wt the heck, reset them too
        m_fCurrentTime = 0.0f;
        m_fTotatlTime = 0.0f;
    }

    // Draw Trajectory Line for giving a basic hint to player with Yaw, Pitch and Time
    public void DrawTrajectoryLine(float trajYaw, float trajPitch)
    {
        lineTrajectoryHint.positionCount = numOfTrajectoryPoints;
        // Calculating time factor
        // Calculate time = velocity / gravityAcceleration
        float totalTime = (2 * BallVelocity.y) / Physics.gravity.y;
        float timeFactor = totalTime / numOfTrajectoryPoints;
        float time = 0;

        // Restrict the overall trajectory hint
        if (trajPitch <= 10)
        {
            trajPitch = 15f;
        }
        else if (trajPitch >= 30)
        {
            trajPitch = 30f;
        }

        BallVelocity = calculateVelocityOfBall(m_fPower, trajYaw, trajPitch);

        // Distance Travelled Vertically -
        // DistanceTraveled = 1/2*g*t*t;
        // DistanceTraveled = Vit - 1/2*g*t*t;

        // Distance Travelled Horizontally -
        // DistanceTraveled = v*t + x;

        for (int i = 0; i < numOfTrajectoryPoints; i++, time+=timeFactor)
        {
            //  Vertical Distance
            float distanceTravelledVertical = -((-(Physics.gravity.y) * (time * time) / 2f) + BallVelocity.y * time) + initialPosition.y;

            // Horizontal Distance
            float distanceTravelledHorizontal = BallVelocity.x * time * -1f + initialPosition.x;

            // Z Distance Travelled = velocity * time + initial position of ball
            float distanceTravelledForward = -(BallVelocity.z * time + initialPosition.z);

            // Set the Position
            lineTrajectoryHint.SetPosition(i, new Vector3(distanceTravelledHorizontal, distanceTravelledVertical, distanceTravelledForward));
        }
    }

    
}
