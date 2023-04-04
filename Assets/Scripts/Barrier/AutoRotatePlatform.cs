using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotatePlatform : MonoBehaviour
{
    [Header("旋转速度")]
    public float rotateSpeed;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, -rotateSpeed * Time.deltaTime));
    }
}
