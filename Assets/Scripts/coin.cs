using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
   
   public float speed=1f;
    public Vector3 rotateAxis;

    private void Update()
    {
        transform.Rotate(rotateAxis*speed*Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            coinControl coinControlScript = FindObjectOfType<coinControl>();
            if (coinControlScript != null)
            {
                coinControlScript.CollectGold();
            }

            Destroy(gameObject);
        }
    }
    
    
}
