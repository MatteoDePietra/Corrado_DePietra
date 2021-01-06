using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Transform target;
    private float targetSmooth = .125f;

    [SerializeField]
    private Transform[] backgrounds = null;
    private float[] parallaxScale;
    private float parallaxsmooth = 10f;
    private Vector3 previousCamPos;

    [SerializeField]
    private Transform myst = null;
    private float mystSmooth = 5f;
    private Vector3 mystOffset;
    private Vector3 timeOffset;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(target);
        previousCamPos = transform.position;
        parallaxScale = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScale[i] = (backgrounds[i].position.z - 1f)/15;
        }

        mystOffset = new Vector3(-.01f, 0, 0);
        timeOffset = new Vector3(Time.timeScale, 0, 0);
    }
    
    private void LateUpdate()
    {
        FollowTarget();
        ParallaxBackground();
        MystBackground();
    }

    private void FixedUpdate()
    {
    }

    private void FollowTarget()
    {
        Vector3 desiredPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
        Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, targetSmooth);
        transform.position = newPosition;
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
        Vector3 desiredPosition = myst.position + mystOffset*Time.timeScale;
        myst.position = Vector3.Lerp(myst.position, desiredPosition, mystSmooth);
    }
}