using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator animator;
    private bool isActivated;

    private void Start()
    {
        isActivated = false;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SetAnimator();
    }

    private void SetAnimator()
    {
        animator.SetBool("isTouch", isActivated);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SetActivated(true);
        }
    }

    public void SetActivated(bool flag)
    {
        isActivated = flag;
    }

    public bool GetActivated()
    {
        return isActivated;
    }
}
