using UnityEngine;
using System.Threading.Tasks;
using Zenject;

namespace Content.Car.Visuals
{
    [RequireComponent(typeof(AudioSource))]
    public class CarAudio : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody carRigidbody;
        [SerializeField] private AudioSource engineSource;
        [SerializeField] private CarController carController;

        [Header("Clips - Engine")]
        [SerializeField] private AudioClip ignitionSound;
        [SerializeField] private AudioClip engineLoopSound;

        [Header("Clips - Doors")]
        [SerializeField] private AudioClip doorOpenCloseSound;

        [Header("Pitch Settings")]
        [SerializeField] private float minPitch = 0.8f;
        [SerializeField] private float maxPitch = 2.2f;
        [SerializeField] private float maxSpeed = 100f;

        [Header("Volume Settings")]
        [SerializeField] private float fadeSpeed = 5.0f;

        private IInputService _inputService;
        private bool _isEngineRunning = false;
        private bool _isIgnitionInProgress = false;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Reset()
        {
            engineSource = GetComponent<AudioSource>();
            carRigidbody = GetComponentInParent<Rigidbody>();
        }

        public void StartEngine()
        {
            if (_isEngineRunning || _isIgnitionInProgress) return;
            PlayEntrySequence();
        }

        public void StopEngine()
        {
            _isEngineRunning = false;
            _isIgnitionInProgress = false;
            engineSource.Stop();
        }

        private async void PlayEntrySequence()
        {
            _isIgnitionInProgress = true;

            if (doorOpenCloseSound != null)
            {
                engineSource.PlayOneShot(doorOpenCloseSound);
                await Task.Delay(1000);
            }

            if (!_isIgnitionInProgress) return;

            if (ignitionSound != null)
            {
                engineSource.loop = false;
                engineSource.clip = ignitionSound;
                engineSource.pitch = 1f;
                engineSource.volume = 1f;
                engineSource.Play();

                int waitTime = (int)(ignitionSound.length * 1000);
                await Task.Delay(waitTime);
            }

            if (!_isIgnitionInProgress) return;

            _isEngineRunning = true;
            _isIgnitionInProgress = false;

            engineSource.clip = engineLoopSound;
            engineSource.loop = true;
            engineSource.volume = 0f;
            engineSource.Play();

            carController.SetCanMove(true);
        }

        private void Update()
        {
            if (!_isEngineRunning) return;

            bool isGasPressed = Mathf.Abs(_inputService.MoveInput.y) > 0.1f;
            float targetVolume = isGasPressed ? 1.0f : 0.2f; 

            engineSource.volume = Mathf.Lerp(engineSource.volume, targetVolume, Time.deltaTime * fadeSpeed);

            float currentSpeed = carRigidbody.linearVelocity.magnitude * 3.6f;
            float inputBoost = Mathf.Abs(_inputService.MoveInput.y) * 0.3f;

            float targetPitch = Mathf.Lerp(minPitch, maxPitch, currentSpeed / maxSpeed) + inputBoost;
            engineSource.pitch = Mathf.Lerp(engineSource.pitch, targetPitch, Time.deltaTime * 5f);
        }
    }
}