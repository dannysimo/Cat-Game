﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;
    private Animator animator;

    private bool attacking = false;

    private float timeToAttack = 0.25f;
    private float timer = 0f;

    
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>(); // 
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift) && !attacking)
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
                animator.SetBool("isAttacking", false); 
            }
        }
    }

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
        animator.SetTrigger("Attack"); 
        animator.SetBool("isAttacking", true); 
    }
}