using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Takip edilecek hedef (oyuncu)
    public float smoothSpeed = 0.125f; // Kamera hareketinin yumuşatma hızı
    public Vector3 offset; // Kamera ile hedef arasındaki mesafe

 
    private void LateUpdate()
    {
        
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        
        transform.LookAt(target); // Hedefe bakması için kamerayı hedefe yönlendir
    }
 
}
