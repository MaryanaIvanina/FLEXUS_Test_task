using UnityEngine;
using System.Collections.Generic;

namespace Content.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerFootsteps : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private AudioSource audioSource;

        [Header("Settings")]
        [SerializeField] private List<AudioClip> stepSounds; 
        [SerializeField] private float walkInterval = 0.5f;
        [SerializeField] private float runInterval = 0.3f; 

        private float _timer;

        private void Reset()
        {
            characterController = GetComponentInParent<CharacterController>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!characterController.isGrounded || characterController.velocity.magnitude < 0.2f)
            {
                _timer = 0;
                return;
            }

            bool isRunning = characterController.velocity.magnitude > 6f;
            float currentInterval = isRunning ? runInterval : walkInterval;

            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                PlayStep();
                _timer = currentInterval;
            }
        }

        private void PlayStep()
        {
            if (stepSounds.Count == 0) return;

            AudioClip clip = stepSounds[Random.Range(0, stepSounds.Count)];

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.volume = Random.Range(0.8f, 1.0f);

            audioSource.PlayOneShot(clip);
        }
    }
}