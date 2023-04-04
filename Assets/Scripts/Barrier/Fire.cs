using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public List<Transform> checkPosition;
    public LayerMask playerLayer;
    public GameObject killFire;

    private Animator animator;
    private bool isPressed;
    private bool overPressed;

    private void Start()
    {
        isPressed = false;
        overPressed = false;
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        RayCheck(); // 射线检测
        PressCheck(); // 按压检测
    }

    private void PressCheck()
    {
        animator.SetBool("isPressed", isPressed);
        animator.SetBool("overPressed", overPressed);
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
            isPressed = true;
        }
        else if (isPressed)
        {
            overPressed = true;
        }

        // 显示射线
        Debug.DrawLine(rayOriginLeft, rayOriginLeft + rayDirection * 0.05f, Color.red);
        Debug.DrawLine(rayOriginRight, rayOriginRight + rayDirection * 0.05f, Color.red);
    }

    public void GenKillFire()
    {
        Invoke("iGenKillFire", 1.0f);
    }

    private void iGenKillFire()
    {
        killFire.SetActive(true);
    }
}
