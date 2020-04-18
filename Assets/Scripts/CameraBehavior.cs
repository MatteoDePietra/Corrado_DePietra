using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Transform target;
    private float targetSmooth = .1f;
    private Vector3 targetOffset;

    [SerializeField]
    private Transform[] background = null;
    private float[] parallaxScale;
    private float parallaxsmooth = 5f;
    private Vector3 previousCamPos;

    [SerializeField]
    private Transform myst = null;
    private float mystSmooth = .5f;
    private Vector3 mystOffset;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetOffset = new Vector3(0, 1, -10);

        previousCamPos = transform.position;
        parallaxScale = new float[background.Length];
        for (int i = 0; i < background.Length; i++)
        {
            parallaxScale[i] = background[i].position.z - 1;
        }

        mystOffset = new Vector3(.1f, 0, 0);
    }
    
    private void FixedUpdate()
    {
        FollowTarget();
        ParallaxCamera();
        MystMovement();
    }

    private void FollowTarget()
    {
        Vector3 desiredPosition = target.position + targetOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, targetSmooth);
    }

    private void ParallaxCamera()
    {
        for (int i = 0; i < background.Length; i++)
        {
            float parallax = (previousCamPos.x - transform.position.x) * parallaxScale[i];
            float backgroundTargetPosX = background[i].position.x + parallax;
            Vector3 backgroudTargetPos = new Vector3(backgroundTargetPosX, background[i].position.y, background[i].position.z);
            background[i].position = Vector3.Lerp(background[i].position, backgroudTargetPos, parallaxsmooth);
        }
        previousCamPos = transform.position;
    }

    private void MystMovement()
    {
        Vector3 desiredPosition = myst.position + mystOffset;
        myst.position = Vector3.Lerp(myst.position, desiredPosition, mystSmooth);
    }
}