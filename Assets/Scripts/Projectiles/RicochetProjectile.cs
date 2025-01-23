using UnityEngine;

public class RicochetProjectile : Projectile
{
    [Header("Ricochet Settings")]
    public int maxBounces = 2;
    public float bounceSpeedMultiplier = 0.8f;
    public bool maintainHeight = true;
    public float heightOffset = 1f;
    public LayerMask collisionLayers = -1; // All layers by default
    
    private int _bounceCount = 0;
    private Vector3 _currentDirection;

    protected override void Start()
    {
        base.Start();
        _currentDirection = transform.forward;
    }

    protected override void Update()
    {
        // Calculate the next position
        Vector3 nextPosition = transform.position + (_currentDirection * speed * Time.deltaTime);
        
        // Cast a ray to detect collisions
        RaycastHit hit;
        float moveDistance = speed * Time.deltaTime;
        if (Physics.Raycast(transform.position, _currentDirection, out hit, moveDistance, collisionLayers))
        {
            HandleCollision(hit);
        }
        else
        {
            // No collision, move normally
            transform.position = nextPosition;
        }

        // Update rotation to match direction
        transform.forward = _currentDirection;
    }

    private void HandleCollision(RaycastHit hit)
    {
        if (_bounceCount >= maxBounces)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate reflection
        Vector3 incomingVector = _currentDirection;
        Vector3 normalVector = hit.normal;
        Vector3 reflectDirection = Vector3.Reflect(incomingVector, normalVector);

        // Maintain height if desired
        if (maintainHeight)
        {
            reflectDirection.y = 0;
            transform.position = new Vector3(transform.position.x, heightOffset, transform.position.z);
        }

        // Set new direction and position
        _currentDirection = reflectDirection.normalized;
        transform.position = hit.point + (_currentDirection * 0.1f); // Slight offset to prevent sticking
        
        // Reduce speed after bounce
        speed *= bounceSpeedMultiplier;
        
        _bounceCount++;
    }
}