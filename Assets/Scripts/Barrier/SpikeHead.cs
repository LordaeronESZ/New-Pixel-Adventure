using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    [Header("移动距离")]
    public float movingSpan;
    [Header("移动速度")]
    public float movingSpeed;

    private float dDistance;
    private float direct;
    private Rigidbody2D rb2d;

    private void Start()
    {
        dDistance = 0;
        direct = 1;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move(); // 自动移动
    }

    private void Move()
    {
        rb2d.velocity = new Vector2(movingSpeed * direct, 0);
        dDistance += movingSpeed * Time.deltaTime;
        if (dDistance >= movingSpan)
        {
            direct *= -1;
            dDistance = 0;
        }
    }
}
