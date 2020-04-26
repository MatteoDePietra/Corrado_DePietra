using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Transform target;
    private float targetSmooth = .1f;
    private Vector3 targetOffset;

    [SerializeField]
    private Transform[] backgrounds = null;
    private float[] parallaxScale;
    private float parallaxsmooth = 2f;
    private Vector3 previousCamPos;

    [SerializeField]
    private Transform myst = null;
    private float mystSmooth = .5f;
    private Vector3 mystOffset;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetOffset = new Vector3(0, .5f, -10);

        previousCamPos = transform.position;
        parallaxScale = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScale[i] = (backgrounds[i].position.z - 1f)/5;
            Debug.Log(i + ": " + parallaxScale[i]);
        }

        mystOffset = new Vector3(.1f, 0, 0);
    }
    
    private void FixedUpdate()
    {
        FollowTarget();
        ParallaxBackground();
        MystBackground();
    }

    private void FollowTarget()
    {
        Vector3 desiredPosition = target.position + targetOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, targetSmooth);
    }

    private void ParallaxBackground()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - transform.position.x) * parallaxScale[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            Vector3 backgroudTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroudTargetPos, parallaxsmooth);
        }
        previousCamPos = transform.position;
    }

    private void MystBackground()
    {
        Vector3 desiredPosition = myst.position + mystOffset;
        myst.position = Vector3.Lerp(myst.position, desiredPosition, mystSmooth);
    }
}