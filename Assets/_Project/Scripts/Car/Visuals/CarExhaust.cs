using UnityEngine;
using Zenject;

namespace Content.Car.Visuals
{
    public class CarExhaust : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ParticleSystem exhaustParticles;

        [Header("Settings")]
        [SerializeField] private float idleRate = 10f; 
        [SerializeField] private float movingRate = 50f; 
        [SerializeField] private float idleSize = 0.3f;   
        [SerializeField] private float movingSize = 0.6f; 

        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Reset()
        {
            exhaustParticles = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (exhaustParticles == null) return;

            bool isGasPressed = Mathf.Abs(_inputService.MoveInput.y) > 0.1f;

            float targetRate = isGasPressed ? movingRate : idleRate;
            float targetSize = isGasPressed ? movingSize : idleSize;

            var emission = exhaustParticles.emission;
            emission.rateOverTime = Mathf.Lerp(emission.rateOverTime.constant, targetRate, Time.deltaTime * 5f);

            var main = exhaustParticles.main;
            main.startSize = Mathf.Lerp(main.startSize.constant, targetSize, Time.deltaTime * 5f);

            float targetSpeed = isGasPressed ? 4f : 1.5f;
            main.startSpeed = Mathf.Lerp(main.startSpeed.constant, targetSpeed, Time.deltaTime * 2f);
        }
    }
}