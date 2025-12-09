using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    public float AngleY = 90f;
    public GameObject target;
    private float targetValue = 0.0f;
    private float currentValue = 0.0f;
    private float easing = 0.05f;

    void Start()
    {
        
    }

    void Update()
    {
        currentValue = currentValue+(targetValue - currentValue) * easing;
        target.transform.rotation = Quaternion.identity;
        target.transform.Rotate(0, currentValue, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Target: " + target);
        targetValue = AngleY;
        currentValue = 0.0f;
        Debug.Log("Enter Trigger");
        Debug.Log("Target Value: " + targetValue);
    }

    void OnTriggerExit(Collider other)
    {
        currentValue = AngleY;
        targetValue = 0.0f;
        Debug.Log("Exit Trigger");
    }
}
