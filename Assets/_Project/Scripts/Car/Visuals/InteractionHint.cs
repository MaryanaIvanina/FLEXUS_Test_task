using UnityEngine;
using Zenject;

namespace Content.Car.Visuals
{
    public class InteractionHint : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject uiElement;
        [SerializeField] private float showDistance = 4.0f; 
        [SerializeField] private CarController carController;

        private GameInteractionManager _gameManager;

        [Inject]
        public void Construct(GameInteractionManager gameManger)
        {
            _gameManager = gameManger;
        }

        private void Start()
        {
            uiElement.SetActive(false);

            if (carController == null)
                carController = GetComponentInParent<CarController>();
        }

        private void Update()
        {
            var player = _gameManager.Player;

            if (player == null || !player.gameObject.activeInHierarchy || carController.enabled)
            {
                if (uiElement.activeSelf) uiElement.SetActive(false);
                return;
            }

            float distance = Vector3.Distance(transform.position, player.transform.position);

            bool shouldShow = distance <= showDistance;

            if (uiElement.activeSelf != shouldShow)
            {
                uiElement.SetActive(shouldShow);
            }
        }
    }
}