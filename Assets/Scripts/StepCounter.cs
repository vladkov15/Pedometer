using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
#pragma warning disable CS0618

public class StepCounter : MonoBehaviour
{
    public Text statusText, stepsText;
    public float lowLimit = 0.005f;//slow
    public float highLimit = 0.1f;//peaks and valleys when walking
    public float vertHighLimit = 0.25f;//The peak and valley when jumping
    private bool isHigh = false;//status
    private float filterCurrent = 10.0f;//filter parameters to get the fitted value
    private float filterAverage = 0.1f;//Filter parameters to get the average
    private float accelerationCurrent = 0f;//fitting value
    private float accelerationAverage = 0f;//Average
    private int steps = 0;//number of steps
    private int oldSteps;
    private float deltaTime = 0f;//Timer
    private int jumpCount = 0;//jump count
    private int oldjumpCount = 0;

    private bool startTimer = false;//Start timing
    private bool isWalking = false;
    private bool isJumping = false;

    void Awake()
    {
        accelerationAverage = Input.acceleration.magnitude;
        oldSteps = steps;
        oldjumpCount = jumpCount;
    }

    void Update()
    {
        checkWalkingAndJumping();//Check whether to walk

        if (isWalking)
        {
            statusText.text = ("Status: Walking");

        }
        else if (!isWalking)
        {
            statusText.text = ("Status: Not Moving");
        }

        if (isJumping)
        {
            statusText.text = ("Status: Jumping");
        }
    }

    void FixedUpdate()
    {

        //Filter Input.acceleration.magnitude (acceleration scalar sum) through Lerp
        //The linear interpolation formula used here is exactly the EMA one-time exponential filtering y[i]=y[i-1]+(x[i]-y[i])*k=(1-k)*y[i] +kx[i]
        accelerationCurrent = Mathf.Lerp(accelerationCurrent, Input.acceleration.magnitude, Time.deltaTime * filterCurrent);
        accelerationAverage = Mathf.Lerp(accelerationAverage, Input.acceleration.magnitude, Time.deltaTime * filterAverage);
        float delta = accelerationCurrent - accelerationAverage;//Get the difference, that is, the slope

        if (!isHigh)
        {
            if (delta > highLimit)//Go high
            {
                isHigh = true;
                steps++;
                stepsText.text = "Number of steps: " + steps + "\nNumber of jumps: " + jumpCount;
            }
            if (delta > vertHighLimit)
            {
                isHigh = true;
                jumpCount++;
                stepsText.text = "Number of steps: " + steps + "\nNumber of jumps:" + jumpCount;
            }
        }
        else
        {
            if (delta < lowLimit)//lower
            {

                isHigh = false;
            }
        }
    }


    private void checkWalkingAndJumping()
    {
        if ((steps != oldSteps) || (oldjumpCount != jumpCount))
        {
            startTimer = true;
            deltaTime = 0f;
        }

        if (startTimer)//Timer, make it slower to update the UI, because you can’t walk in only one frame QAQ
        {
            deltaTime += Time.deltaTime;

            if (deltaTime != 0)
            {
                if (oldjumpCount != jumpCount)//Check if it is a jump
                    isJumping = true;
                else
                    isWalking = true;

            }
            if (deltaTime > 2)
            {
                deltaTime = 0F;
                startTimer = false;
            }
        }
        else if (!startTimer)
        {
            isWalking = false;
            isJumping = false;
        }
        oldSteps = steps;
        oldjumpCount = jumpCount;
    }
}
