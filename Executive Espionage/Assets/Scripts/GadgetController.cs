using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GadgetController : MonoBehaviour
{
    protected Camera cam;
    [SerializeField] protected float fireRange;


    private void Start()
    {
        cam = Camera.main;
    }

    public abstract void Activate();
}
