using UnityEngine;

public class Bumerang : MonoBehaviour
{
   public float speed = 5f; // Taşın hızı

    private Transform target; // Hedef düşman

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
