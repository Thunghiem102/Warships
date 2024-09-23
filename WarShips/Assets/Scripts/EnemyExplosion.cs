using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    public AudioClip explosionSound; // Âm thanh phát nổ

    public void Die()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Debug.Log("OnDestroy called for " + gameObject.name);
        PlayExplosionSound();
    }

    void PlayExplosionSound()
    {
        if (explosionSound != null)
        {
            Debug.Log("Playing explosion sound for " + gameObject.name);
            GameObject tempGameObject = new GameObject("TempAudio");
            
            AudioSource audioSource = tempGameObject.AddComponent<AudioSource>(); // Thêm AudioSource vào GameObject tạm thời
            audioSource.clip = explosionSound; // Gắn AudioClip
            audioSource.Play(); // Phát âm thanh

            Destroy(tempGameObject, explosionSound.length); // Destroy GameObject tạm thời sau khi âm thanh phát xong
        }
        else
        {
            Debug.LogError("Explosion sound is missing on " + gameObject.name);
        }
    }

}
