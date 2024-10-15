using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    public AudioClip explosionSound; // Âm thanh phát nổ

    public void Die()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
       
        PlayExplosionSound();
    }

    void PlayExplosionSound()
    {
        if (explosionSound != null)
        {
            
            GameObject tempGameObject = new GameObject("TempAudio");
            
            AudioSource audioSource = tempGameObject.AddComponent<AudioSource>(); // Thêm AudioSource vào GameObject tạm thời
            audioSource.clip = explosionSound; // Gắn AudioClip
            audioSource.Play(); // Phát âm thanh

            Destroy(tempGameObject, explosionSound.length); // Destroy GameObject tạm thời sau khi âm thanh phát xong
        }
    }

}
