using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [Header("链条")]
    public List<GameObject> chains;
    [Header("齿轮")]
    public GameObject spikeBox;

    private void Update()
    {
        float x1 = spikeBox.transform.position.x;
        float y1 = spikeBox.transform.position.y;
        float x2 = chains[2].transform.position.x;
        float y2 = chains[2].transform.position.y;

        float x3 = (x2 + 2 * x1) / 3, y3 = (y2 + 2 * y1) / 3;
        float x4 = (x1 + 2 * x2) / 3, y4 = (y1 + 2 * y2) / 3;
        chains[0].transform.position = new Vector3(x3, y3, 0);
        chains[1].transform.position = new Vector3(x4, y4, 0);
    }
}
