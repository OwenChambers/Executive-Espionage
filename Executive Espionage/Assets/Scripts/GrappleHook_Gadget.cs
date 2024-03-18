using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrappleHook_Gadget : GadgetController
{
    private Spring spring;
    private LineRenderer lr;
    private Vector3 currentGrapplePosition;
    bool isFiring;
    readonly int quality = 500;
    readonly float damper = 8;
    readonly float strength = 800;
    readonly float velocity = 15;
    readonly float waveCount = 2;
    readonly float waveHeight = 10;
    private SpringJoint joint;
    RaycastHit hit;
    Transform gunPoint;
    Transform hook;
    Transform hookPoint;
    [SerializeField] GameObject player;

    [Header("Hook Movement")]
    [SerializeField] float hookSpeed = 1000;
    [SerializeField] float minDistance = 2;

    [Header("Rope Render")]
    [SerializeField] AnimationCurve affectCurve;

    public override void Activate()
    {
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        if(!isFiring)
        {
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, fireRange))
            {
                isFiring = true;
                hook.parent = null;
                hook.position = hit.point;
                hook.rotation = gameObject.transform.rotation;
            }
        }
        else
        {
            Deactivate();
        }
    }

    void Awake()
    {
        gunPoint = transform.Find("Gun Point");
        hookPoint = transform.Find("HookPoint");
        hook = hookPoint.transform.Find("Hook");
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    void LateUpdate()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!isFiring)
        {
            currentGrapplePosition = gunPoint.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        
        var up = Quaternion.LookRotation((hit.point - gunPoint.position).normalized) * Vector3.up; 
        var right = Quaternion.LookRotation((hit.point - gunPoint.position).normalized) * Vector3.right;


        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, hit.point, Time.deltaTime * 12f);
        for (var i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                                 affectCurve.Evaluate(delta) +
                                 right * waveHeight * Mathf.Cos(delta * waveCount * Mathf.PI) * spring.Value *
                                 affectCurve.Evaluate(delta);
            lr.SetPosition(i, Vector3.Lerp(gunPoint.position, currentGrapplePosition, delta) + offset);
        }
    }

    void MovePlayer()
    {
        if(isFiring)
        {
            Vector3 targetDirection = hit.point - transform.position;
            player.GetComponent<Rigidbody>().velocity = targetDirection.normalized * hookSpeed;

            if (Vector3.Distance(player.transform.position, hit.point) < minDistance)
            {
                Deactivate();
            }
        }
    }

    void Deactivate()
    {
        hook.parent = hookPoint.transform;
        hook.localPosition = Vector3.zero;
        hook.localRotation = Quaternion.identity;
        isFiring = false;
    }

    private void OnEnable()
    {
        Application.onBeforeRender += DrawRope;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= DrawRope;
    }
}
