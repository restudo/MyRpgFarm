using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SwitchBoundingShape();
    }


    /// Switch the confiner that cinemacine uses to define the edges of the screen
    void SwitchBoundingShape(){
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.boundsConfiner).GetComponent<PolygonCollider2D>();

        CinemachineConfiner2D cinemachineConfiner2D = GetComponent<CinemachineConfiner2D>();

        cinemachineConfiner2D.m_BoundingShape2D = polygonCollider2D;

        cinemachineConfiner2D.InvalidateCache();
    }
}
