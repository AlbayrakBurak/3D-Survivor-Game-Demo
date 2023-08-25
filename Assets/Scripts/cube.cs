using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    public float speed = 5f; // Taşın hızı
    public Transform playerTransform; // Oyuncunun transformu
    public float maxDistance = 10f; // Taşın en fazla ne kadar uzaklığa gidebileceği
    private float currentDistance = 0f; // Taşın gittiği mesafe

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    private void Update()
    {
         transform.Translate(Vector3.forward*Time.deltaTime*speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {   
                Destroy(gameObject);
                playerController.TakeDamage(10);
            }
        }
    }
}
