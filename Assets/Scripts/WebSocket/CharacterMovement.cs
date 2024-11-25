using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterMovement : MonoBehaviour
{
    private Vector3 targetPosition; // Hedef pozisyon
    private Quaternion targetRotation; // Hedef rotasyon
    public float moveSpeed = 5f; // Hareket hýzý

    private void Start()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }


    private void Update()
    {
        // Sabit hýzda hedef pozisyona hareket et
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Rotasyonu anýnda uygula
        transform.rotation = targetRotation;
    }

    public void UpdateTarget(Vector3 newPosition)
    {
        // Z parametresine belirli bir deðer ekleyerek pozisyonu güncelle
        //newPosition.z += 1f; // Z eksenine 1 ekleme
        targetPosition = newPosition;

        // Yeni rotasyonu ayarla
        //targetRotation = Quaternion.Euler(newRotation);
    }
}
