using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskDude : MonoBehaviour
{
    [Header("速度")]
    public float moveSpeed;
    [Header("跳跃")]
    public float jumpForce;
    [Header("二段跳")]
    public float doubleJumpForce;
    [Header("冲刺")]
    public float dashSpeed;
    [Header("地面")]
    public LayerMask groundLayer;
    [Header("左右脚")]
    public Transform leftFoot;
    public Transform rightFoot;
    [Header("子弹")]
    public GameObject bulletPrefab;
    [Header("子弹速度")]
    public float shootSpeed;
    [Header("射击时间")]
    public float shootTimeSpan;
    [Header("掉落死亡线")]
    public float deadLine;

    private Animator animator;
    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;
    private bool isGrounded;
    private bool KPressed;
    private bool LPressed;
    private bool JPressed;
    private float horizontal;
    private int jumpCnt;
    private int dashCnt;
    private bool isDead;
    private float dTime;
    private float addSpeed;

    private void Start()
    {
        isDead = false;
        jumpCnt = 2;
        dashCnt = 1;
        dTime = 0;
        addSpeed = 0;
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.K))
        {
            KPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            JPressed = true;
        }
    }

    private void FixedUpdate()
    {
        MyDebug(); // 调试
        DeadCheck(); // 死亡检测
        Move(); // 移动
        RayCheck(); // 射线检测
        Jump(); // 跳跃
        DoubleJump(); // 二段跳
        FallCheck(); // 下落
        FallDeadCheck(); // 掉落死亡
        // Dash(); // 冲刺
        // Attack(); // 攻击
    }

    private void Move()
    {
        rb2d.velocity = new Vector2(moveSpeed * horizontal, rb2d.velocity.y);
        rb2d.velocity += new Vector2(addSpeed, 0);
        if (horizontal != 0)
        {
            animator.SetBool("isMoving", true);
            if (horizontal > 0)
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (horizontal < 0)
            {
                this.transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void Jump()
    {
        animator.SetBool("isGrounded", isGrounded);
        if (KPressed && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce * Time.deltaTime * 100);
            --jumpCnt;
            KPressed = false;
        }
    }

    private void DoubleJump()
    {
        if (KPressed && jumpCnt == 1)
        {
            rb2d.velocity += new Vector2(0, doubleJumpForce * Time.deltaTime * 100);
            animator.SetTrigger("Double");
            --jumpCnt;
        }
        KPressed = false;
    }

    private void Dash()
    {
        if (LPressed && dashCnt > 0)
        {
            if (transform.rotation.y == 0)
            {
                rb2d.velocity = new Vector2(dashSpeed, 0);
            }
            else
            {
                rb2d.velocity = new Vector2(-dashSpeed, 0);
            }
            --dashCnt;
        }
        LPressed = false;
    }

    private void RayCheck()
    {
        // 计算射线的起始点和方向
        Vector2 rayOriginLeft = leftFoot.position;
        Vector2 rayOriginRight = rightFoot.position;
        Vector2 rayDirection = Vector2.down;

        // 进行检测
        RaycastHit2D hitLeft = Physics2D.Raycast(rayOriginLeft, rayDirection, 0.05f, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(rayOriginRight, rayDirection, 0.05f, groundLayer);
        if (hitLeft.collider == null && hitRight.collider == null)
        {
            isGrounded = false;
            addSpeed = 0;
        }
        else
        {
            isGrounded = true;
            jumpCnt = 2;
            dashCnt = 1;
            if (hitLeft.collider != null)
            {
                HitOperation(hitLeft); // 根据检测物体的 tag 执行相应操作
            }
            else if (hitRight.collider != null)
            {
                HitOperation(hitRight);
            }
        }
        
        // 显示射线
        Debug.DrawLine(rayOriginLeft, rayOriginLeft + rayDirection * 0.05f, Color.red);
        Debug.DrawLine(rayOriginRight, rayOriginRight + rayDirection * 0.05f, Color.red);
    }

    private void HitOperation(RaycastHit2D hit)
    {
        switch (hit.transform.tag)
        {
            case "MovingPlatform":
                addSpeed = hit.rigidbody.velocity.x; break;
        }
    }

    private void FallCheck()
    {
        if (rb2d.velocity.y < -1e-5)
        {
            animator.SetBool("isFall", true);
        }
        else
        {
            animator.SetBool("isFall", false);
        }
    }

    private void DeadCheck()
    {
        animator.SetBool("isDead", isDead);
    }

    public void Die()
    {
        GameObject.Destroy(this.gameObject);
    }

    public void Attack()
    {
        dTime += Time.deltaTime;
        if (JPressed && dTime > shootTimeSpan)
        {
            dTime = 0;
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            // 给子弹添加速度
            Rigidbody2D rb2dBullet = newBullet.GetComponent<Rigidbody2D>();
            if (transform.rotation.y == 0)
            {
                rb2dBullet.velocity = new Vector2(shootSpeed, 0);
            }
            else
            {
                rb2dBullet.velocity = new Vector2(-shootSpeed, 0);
            }
        }
        JPressed = false;
    }

    public void FallDeadCheck()
    {
        if (transform.position.y < deadLine)
        {
            isDead = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            isDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            isDead = true;
        }
    }

    private void MyDebug()
    {
        // Debug.Log(rb2d.velocity.x);
    }
}
