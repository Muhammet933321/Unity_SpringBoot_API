using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterMovement : MonoBehaviour
{
    private Vector3 targetPosition; // Hedef pozisyon
    private Quaternion targetRotation; // Hedef rotasyon
    public float moveSpeed = 5f; // Hareket h�z�

    private void Start()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }


    private void Update()
    {
        // Sabit h�zda hedef pozisyona hareket et
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Rotasyonu an�nda uygula
        transform.rotation = targetRotation;
    }

    public void UpdateTarget(Vector3 newPosition)
    {
        // Z parametresine belirli bir de�er ekleyerek pozisyonu g�ncelle
        //newPosition.z += 1f; // Z eksenine 1 ekleme
        targetPosition = newPosition;

        // Yeni rotasyonu ayarla
        //targetRotation = Quaternion.Euler(newRotation);
    }
}
