using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3f, -5f);
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (!target) return;

        // Smoothly position behind player relative to their current orientation
        Vector3 targetPos = target.position + (target.rotation * offset);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);

        // Match the player's tilt and wall-running angles
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * smoothSpeed);
    }
}