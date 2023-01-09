using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Cart : MonoBehaviour
{
    public SplineContainer spline;
    public float speed = 10f;
    [Range(0f, 1f)] public float ratio = 0f;

    private Rigidbody _rigidbody;


    private void OnValidate()
    {
        if(spline != null)
            EvaluatePosition();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            other.transform.SetParent(transform, true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null, true);
        }
    }


    private void EvaluatePosition()
    {
        transform.position = spline.EvaluatePosition(ratio);
        transform.forward = spline.EvaluateTangent(ratio);
    }

    private void FixedUpdate()
    {
        ratio = Mathf.Repeat(ratio + Time.deltaTime * speed * 0.001f, 1f);
        EvaluatePosition();
    }
}
