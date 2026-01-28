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
        [SerializeField] private AudioSource audioSource;

        [Header("Clips")]
        [SerializeField] private AudioClip ignitionSound;
        [SerializeField] private AudioClip engineLoopSound;

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
            audioSource = GetComponent<AudioSource>();
            carRigidbody = GetComponentInParent<Rigidbody>();
        }

        public void StartEngine()
        {
            if (_isEngineRunning || _isIgnitionInProgress) return;
            PlayIgnitionSequence();
        }

        public void StopEngine()
        {
            _isEngineRunning = false;
            _isIgnitionInProgress = false;
            audioSource.Stop();
        }

        private async void PlayIgnitionSequence()
        {
            _isIgnitionInProgress = true;

            if (ignitionSound != null)
            {
                audioSource.loop = false;
                audioSource.clip = ignitionSound;
                audioSource.pitch = 1f;
                audioSource.volume = 1f; 
                audioSource.Play();

                int waitTime = (int)(ignitionSound.length * 1000);
                await Task.Delay(waitTime);
            }

            if (!_isIgnitionInProgress) return;

            _isEngineRunning = true;
            _isIgnitionInProgress = false;

            audioSource.clip = engineLoopSound;
            audioSource.loop = true;
            audioSource.volume = 0f;
            audioSource.Play();
        }

        private void Update()
        {
            if (!_isEngineRunning) return;

            bool isGasPressed = Mathf.Abs(_inputService.MoveInput.y) > 0.1f;

            float targetVolume = isGasPressed ? 1.0f : 0.0f;

            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * fadeSpeed);


            float currentSpeed = carRigidbody.linearVelocity.magnitude * 3.6f;

            float inputBoost = Mathf.Abs(_inputService.MoveInput.y) * 0.3f;

            float targetPitch = Mathf.Lerp(minPitch, maxPitch, currentSpeed / maxSpeed) + inputBoost;
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * 5f);
        }
    }
}