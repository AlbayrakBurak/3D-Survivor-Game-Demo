using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public FloatingJoystick floatingJoystick;
    public float speed;
    public float turnSpeed;
    public Animator anim;
    public bool isDeath;
    public Image attackRangeIndicator; // Dairesel gösterge
    public float attackRange = 10f; // Saldırı menzili yarıçapı

    public GameObject stonePrefab; // Taşın prefabı
    public Transform stoneSpawnPoint; // Taşın çıkacağı konum
    public float stoneSpeed = 10f; // Taşın hızı
    public float stoneCooldown = 0.5f; // Taş atma aralığı
    private float lastStoneThrowTime = 0f; // Son taş atma zamanı
    private Transform targetEnemy; // Hedeflenen düşman
    private bool isTargeted = false; // Hedeflenen düşman bayrağı
    
    public int score = 0; // Oyuncunun skorunu tutacak değişken
    public TMP_Text scoreText;
     private int currentHealth = 100; // Oyuncunun mevcut canı
    public TMP_Text healthText; // Canı gösteren metin

    public GameObject Panel;
    
    private void Update (){
          UpdateAttackRangeIndicator();
                  healthText.text = "Health: " + currentHealth.ToString();


    }


    
    
    void FixedUpdate()
    {
        if(!isDeath){
        if(Input.GetButton("Fire1")){
            
            JoystickMovement();
        }
         else {
             anim.SetBool("isRun",false);
         }

        if (Time.time - lastStoneThrowTime >= stoneCooldown)
            {
                FindAndThrowStone();
            }
        }
        else if (isDeath){
            anim.SetBool("isDeath",true);
        }
    }

    private void JoystickMovement(){
        anim.SetBool("isRun",true);
        float horizontal=floatingJoystick.Horizontal;
        float vertical=floatingJoystick.Vertical;

        Vector3 pos=new Vector3(horizontal*speed*Time.deltaTime,0,vertical*speed*Time.deltaTime);
        transform.position+=pos;

        Vector3 direction=Vector3.forward*vertical+Vector3.right*horizontal;
        transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction), turnSpeed*Time.deltaTime); 

    }

       private void UpdateAttackRangeIndicator()
    {
        attackRangeIndicator.rectTransform.sizeDelta = new Vector2(attackRange * 2, attackRange * 2);
        attackRangeIndicator.transform.position = transform.position;
        
    }


     private void FindAndThrowStone()
    {
       if (!isTargeted) // Sadece hedeflenmemiş düşmanlara taş fırlat
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange - 2);
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    targetEnemy = collider.transform;
                    isTargeted = true; // Hedeflenen düşman bayrağını true yap
                    ThrowStoneAtEnemy(targetEnemy); // Hedef düşmanı taş atma fonksiyonuna gönder
                    break;
                }
            }
        }
    }

private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyCube"))
        {
            // Düşmana çarpıldığında canı azalt
             TakeDamage(10); // 10 hasar al
        }
        if(collision.gameObject.CompareTag("Enemy")){
            Die();
        }
    }


    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Canı kontrol et ve ölüm durumunu ele al
        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthText();
    }
    private void Die()
    {
        anim.SetTrigger("isDie");
        isDeath = true;

        
        Invoke("ActivatePanel", 3f);
    }
    private void ActivatePanel()
{   
    gameObject.SetActive(false);
    Panel.SetActive(true);
}

    private void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHealth.ToString();
    }

   private void ThrowStoneAtEnemy(Transform enemy)
{
    lastStoneThrowTime = Time.time;
  

    Vector3 directionToEnemy = (enemy.position - stoneSpawnPoint.position).normalized;
    GameObject stone = Instantiate(stonePrefab, stoneSpawnPoint.position, Quaternion.identity);

    Rigidbody stoneRb = stone.GetComponent<Rigidbody>();
    if (stoneRb == null)
    {
        stoneRb = stone.AddComponent<Rigidbody>();
    }

    stoneRb.velocity = directionToEnemy * stoneSpeed;

    StartCoroutine(FollowStoneToEnemy(stone.transform));
    score += 1;
    scoreText.text = "Score: " + score.ToString();
}

private IEnumerator FollowStoneToEnemy(Transform stone)
{
    Collider stoneCollider = stone.GetComponent<Collider>();
    Collider enemyCollider = targetEnemy.GetComponent<Collider>();

    Vector3 initialPosition = stone.position;
    float startTime = Time.time;


    Animator enemyAnimator = targetEnemy.GetComponent<Animator>();
    if (enemyAnimator != null)
    {
        enemyAnimator.SetTrigger("isDie"); // Düşmanın ölüm animasyonunu tetikle
    }

    while (stone != null && targetEnemy != null)
    {
        float elapsedTime = Time.time - startTime;
        float journeyLength = Vector3.Distance(initialPosition, targetEnemy.position);
        float normalizedDistance = elapsedTime * stoneSpeed / journeyLength;

        Vector3 lerpedPosition = Vector3.Lerp(initialPosition, targetEnemy.position, normalizedDistance);
        stone.position = lerpedPosition;

        if (normalizedDistance >= 1.0f)
        {
            float impactAnimationDuration = GetImpactAnimationDuration(enemyAnimator);
            yield return new WaitForSeconds(impactAnimationDuration-1); // Ölüm animasyonunun süresini bekle
            Destroy(stone.gameObject);
            Destroy(targetEnemy.gameObject, 0.5f); 
          
             yield return new WaitForSeconds(stoneCooldown);
              isTargeted=false;

            FindAndThrowStone();

            

            break;
        }

        yield return null;
    }
}

private float GetImpactAnimationDuration(Animator animator)
{
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    return stateInfo.length; // Animasyonun süresini al
}
   
}