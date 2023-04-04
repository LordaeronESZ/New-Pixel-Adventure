using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("玩家")]
    public LayerMask playerLayer;
    [Header("检测点")]
    public List<Transform> checkPosition;
    [Header("弹力")]
    public float pushForce;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        RayCheck(); // 射线检测
    }

    private void RayCheck()
    {
        // 计算射线的起始点和方向
        Vector2 rayOriginLeft = checkPosition[0].position;
        Vector2 rayOriginRight = checkPosition[1].position;
        Vector2 rayDirection = Vector2.up;

        // 进行检测
        RaycastHit2D hitLeft = Physics2D.Raycast(rayOriginLeft, rayDirection, 0.05f, playerLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(rayOriginRight, rayDirection, 0.05f, playerLayer);
        if (hitLeft.collider != null || hitRight.collider != null)
        {
            animator.SetBool("isPressed", true);
            if (hitLeft.collider != null)
            {
                Push(hitLeft.rigidbody);
            }
            else
            {
                Push(hitRight.rigidbody);
            }
        }
        else
        {
            animator.SetBool("isPressed", false);
        }

        // 显示射线
        Debug.DrawLine(rayOriginLeft, rayOriginLeft + rayDirection * 0.05f, Color.red);
        Debug.DrawLine(rayOriginRight, rayOriginRight + rayDirection * 0.05f, Color.red);
    }

    private void Push(Rigidbody2D rb2d)
    {
        rb2d.velocity = new Vector2(0, pushForce * Time.deltaTime * 100);
    }
}
