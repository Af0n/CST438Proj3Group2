using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public Gradient colors;
    [Header("Unity Set Up")]
    public DayNight dayNight;
    private Light2D illum;

    private void Awake() {
        illum = GetComponent<Light2D>();
    }

    private void Update() {
        illum.color = colors.Evaluate(dayNight.PercentDay);
    }
}
