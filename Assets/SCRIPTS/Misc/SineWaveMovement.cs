using UnityEngine;

public class SineWaveMovement : MonoBehaviour
{
    public float amplitude = 1f; // Height of the sine wave
    public float frequency = 1f; // Speed of the oscillation

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, startPosition.y + yOffset, startPosition.z);
    }
}

