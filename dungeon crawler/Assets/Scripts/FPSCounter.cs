using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public int avgFrameRate;
    public Text MainFPS;
    private float current;

    public float lowestFrameRate;
    public Text LowFPS;
    private float timer;

    void Start()
    {
        lowestFrameRate = 5000;
        current = 5000;
        timer = 0;
    }

    void Update()
    {
        // Main FPS
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        MainFPS.text = avgFrameRate.ToString() + " FPS";


        // Low FPS
        timer += Time.deltaTime;

        if (timer > 5 && current < lowestFrameRate)
        {
            lowestFrameRate = current;
            LowFPS.text = "Lowest FPS: " + lowestFrameRate;
        }
    }
}
