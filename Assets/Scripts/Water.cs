using UnityEngine;

public class Water : MonoBehaviour
{
    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        _audioSource.pitch = Random.Range(0.5f, 1.5f);
        _audioSource.Play();
    }
}
