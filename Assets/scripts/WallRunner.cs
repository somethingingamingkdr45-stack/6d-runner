using UnityEngine;

public class WallRunner : MonoBehaviour
{
    [Header("Speed Progression")]
    public float currentSpeed = 150f;
    public float maxSpeed = 500f;
    public float distanceMilestone = 5f;
    public float speedBoostAmount = 2f;

    [Header("Steering")]
    public float tiltSensitivity = 150f;
    public float leftLimit = -150f;
    public float rightLimit = 150f;

    [Header("Magnet & Surface")]
    public float wallCheckDistance = 60f;
    public float rotationSpeed = 15f;
    public float floorOffset = 1f;
    public LayerMask groundLayer;

    [Header("Crash Effects")]
    public Animation playerAnimation;
    public AnimationClip crashClip;
    public AudioSource crashSound;
    
    [Header("Memory Score System")]
    public float memoryScore = 0f;
    public float memoryPenaltyOnHit = 500f; // How much memory is lost on impact
    public float speedPenaltyOnHit = 30f;   // Slows player down slightly on impact

    private float currentXOffset = 0f;
    private float totalDistanceTraveled = 0f;
    private int milestonesReached = 0;

    void Update()
    {
        // Constant Movement (Game Never Ends)
        float frameMove = currentSpeed * Time.deltaTime;
        totalDistanceTraveled += frameMove;
        
        // Add to the score smoothly while running
        memoryScore += frameMove * 0.1f; 

        int currentMilestone = Mathf.FloorToInt(totalDistanceTraveled / distanceMilestone);
        if (currentMilestone > milestonesReached)
        {
            int milestonesCrossed = currentMilestone - milestonesReached;
            milestonesReached = currentMilestone;
            currentSpeed = Mathf.Min(currentSpeed + (speedBoostAmount * milestonesCrossed), maxSpeed);
        }

        transform.position += transform.forward * frameMove;

        // Responsive Steering
        float horizontalInput = Input.acceleration.x;
        #if UNITY_EDITOR
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }
        #endif

        float sidewaysMove = horizontalInput * tiltSensitivity * Time.deltaTime;
        float nextOffset = Mathf.Clamp(currentXOffset + sidewaysMove, leftLimit, rightLimit);
        float stepMove = nextOffset - currentXOffset;
        currentXOffset = nextOffset;
        
        transform.position += transform.right * stepMove;

        HandleSurfaceTransitions();
    }

    private void HandleSurfaceTransitions()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + (transform.up * 5f);

        if (Physics.Raycast(rayOrigin, transform.forward, out hit, wallCheckDistance, groundLayer))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else if (Physics.Raycast(rayOrigin, -transform.up, out hit, 30f, groundLayer))
        {
            transform.position = hit.point + (transform.up * floorOffset);
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // 1. Play Effects
            if (crashSound != null) crashSound.Play();
            if (playerAnimation != null && crashClip != null)
            {
                playerAnimation.clip = crashClip;
                playerAnimation.Play();
            }

            // 2. Apply Penalties
            memoryScore -= memoryPenaltyOnHit;
            if (memoryScore < 0) memoryScore = 0; // Prevent negative memory
            
            // Slow down slightly on impact so it feels physical, but don't stop completely
            currentSpeed = Mathf.Max(currentSpeed - speedPenaltyOnHit, 100f); 

            // 3. Destroy the Rock Instantly (Solves the getting stuck and pile-up bugs!)
            Destroy(other.gameObject);
        }
    }
}