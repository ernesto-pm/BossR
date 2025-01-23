using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Base Projectile Settings")]
    public float speed = 20f;
    public float lifetime = 3f;
    
    protected virtual void Start()
    {
        Destroy(gameObject, lifetime);
    }

    protected virtual void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}