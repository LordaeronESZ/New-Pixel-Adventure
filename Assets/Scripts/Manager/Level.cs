using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public List<GameObject> checkPoints;
    public GameObject edge;

    private void Awake()
    {
        Transform cpTransform = transform.Find("CheckPoint");
        for (int i = 0; i  < cpTransform.childCount; ++i)
        {
            checkPoints.Add(cpTransform.GetChild(i).gameObject);
        }
        Transform edgeTransform = transform.Find("Edge");
        edge = edgeTransform.gameObject;
    }
}
