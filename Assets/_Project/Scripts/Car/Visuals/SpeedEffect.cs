using UnityEngine;

namespace Content.Car.Visuals
{
    [RequireComponent(typeof(ParticleSystem))]
    public class SpeedEffect : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float minSpeed = 60f;
        
        [SerializeField] private float maxSpeed = 140f;

        [SerializeField] private float maxEmissionRate = 40f;

        private ParticleSystem _particleSystem;
        private ParticleSystem.EmissionModule _emissionModule;
        private Rigidbody _targetCar;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _emissionModule = _particleSystem.emission;
            _emissionModule.rateOverTime = 0f;
        }

        private void Update()
        {
            if (_targetCar == null || !_targetCar.gameObject.activeInHierarchy)
            {
                _emissionModule.rateOverTime = 0f;
                return;
            }

            float currentSpeedKmh = _targetCar.linearVelocity.magnitude * 3.6f;

            float speedFactor = Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeedKmh);

            _emissionModule.rateOverTime = Mathf.Lerp(0f, maxEmissionRate, speedFactor);
        }

        public void SetTargetCar(Rigidbody car)
        {
            _targetCar = car;
        }

        public void ClearTarget()
        {
            _targetCar = null;
        }
    }
}