using UnityEngine;
using TMPro;

public class MemoryTracker : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI memoryText; // Drag your UI text here

    [Header("Tracking Settings")]
    public float distanceMultiplier = 0.1f; // Adjust to speed up or slow down the meter count
    
    private float totalRestored = 0f;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate exact physical distance moved each frame, regardless of axis or rotation changes
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        totalRestored += distanceMoved * distanceMultiplier;
        
        lastPosition = transform.position;

        // Update the UI text
        if (memoryText != null)
        {
            memoryText.text = $"Memory Restored: {Mathf.FloorToInt(totalRestored)}m";
        }
    }
}