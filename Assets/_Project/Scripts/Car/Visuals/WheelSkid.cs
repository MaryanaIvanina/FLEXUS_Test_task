using UnityEngine;

namespace Content.Car.Visuals
{
    public class WheelSkid : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private WheelCollider targetWheel;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private AudioSource audioSource;

        [Header("Settings")]
        [SerializeField] private float slipThreshold = 0.8f;
        [SerializeField] private float volumeSpeed = 10f;

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            audioSource.loop = true;
            audioSource.volume = 0f; 
            audioSource.Play();      
        }

        private void Update()
        {
            if (!targetWheel.GetGroundHit(out WheelHit hit))
            {
                trailRenderer.emitting = false;
                audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * volumeSpeed);
                return;
            }

            float currentSlip = Mathf.Abs(hit.sidewaysSlip) + Mathf.Abs(hit.forwardSlip);

            if (currentSlip > slipThreshold)
            {
                trailRenderer.emitting = true;
                transform.position = hit.point + Vector3.up * 0.02f;

                audioSource.volume = Mathf.Lerp(audioSource.volume, 0.5f, Time.deltaTime * volumeSpeed);
            }
            else
            {
                trailRenderer.emitting = false;
                audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * volumeSpeed);
            }
        }
    }
}