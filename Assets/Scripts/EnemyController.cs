using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float attackRange = 5.0f;
    public GameObject stonePrefab;
    public Transform throwPoint;
    public Transform player;
    public float  stonePower= 10f; 


    private Animator animator;

    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player=GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        float playerRange = Vector3.Distance(transform.position, player.position);
        if (playerRange > attackRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            transform.forward = direction;
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            animator.SetBool("isRun", true);

            if (isAttacking)
            {
                isAttacking = false;
                animator.SetBool("EnemyAttack", false);
            }
        }
        else
        {
            if (!isAttacking)
            {
                isAttacking = true;
                animator.SetBool("isRun", false);
                animator.SetBool("EnemyAttack", true);
                Attack();
            }
        }
    }

    void Attack(){
        GameObject stone = Instantiate(stonePrefab, throwPoint.position, throwPoint.rotation);
        
    }
}

    // This method is called from the attack animation using Animation Events
   

