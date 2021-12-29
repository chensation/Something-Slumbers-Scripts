// Floater v0.0.2
// by Donovan Keith
//
// [MIT License](https://opensource.org/licenses/MIT)

using UnityEngine;
using System.Collections;

// Makes objects float up & down while gently spinning.
public class Floater : MonoBehaviour
{
    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private float minimum = -1.0F;
    private float maximum = 1.0F;

    private float t = 0.5f;

    // Use this for initialization
    void Start()
    {
        maximum = transform.position.y + amplitude;
        minimum = transform.position.y - amplitude;
    }

    // Update is called once per frame
    void Update()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.unscaledDeltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(minimum, maximum, t), transform.position.z);

        t += frequency * Time.unscaledDeltaTime;

        if (t > 1.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }
    }
}