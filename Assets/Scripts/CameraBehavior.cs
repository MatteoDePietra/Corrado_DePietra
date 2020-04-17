using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Transform target;
    private float smoothSpeed = .1f;
    private Vector3 offset;

    [SerializeField]
    private Transform[] background = null;
    private float[] parallaxScale;
    private float smoothParallax = 5f;

    private Vector3 previousCamPos;


    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        offset = new Vector3(0, 1, -10);

        previousCamPos = transform.position;
        parallaxScale = new float[background.Length];
        for (int i = 0; i < background.Length; i++)
        {
            parallaxScale[i] = background[i].position.z - 1;
        }
    }
    
    private void FixedUpdate()
    {
        FollowTarget();
        ParallaxCamera();
    }

    private void FollowTarget()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void ParallaxCamera()
    {
        for (int i = 0; i < background.Length; i++)
        {
            float parallax = (previousCamPos.x - transform.position.x) * parallaxScale[i];
            float backgroundTargetPosX = background[i].position.x + parallax;
            Vector3 backgroudTargetPos = new Vector3(backgroundTargetPosX, background[i].position.y, background[i].position.z);
            background[i].position = Vector3.Lerp(background[i].position, backgroudTargetPos, smoothParallax);
        }
        previousCamPos = transform.position;
    }
}