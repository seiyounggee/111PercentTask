using UnityEngine;

public class UnscaledParticleSystem : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private float _previousTime;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _previousTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        // Calculate the delta time based on unscaled time
        float currentTime = Time.realtimeSinceStartup;
        float deltaTime = currentTime - _previousTime;
        _previousTime = currentTime;

        // Manually simulate the particle system with unscaled time
        _particleSystem.Simulate(deltaTime, true, false);
    }
}
