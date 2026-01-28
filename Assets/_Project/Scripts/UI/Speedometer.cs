using UnityEngine;
using TMPro;

namespace Content.UI
{
    public class Speedometer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI speedText;

        private Rigidbody _targetCar;

        private void Awake()
        {
            Hide();
        }

        private void Update()
        {
            if (_targetCar == null) return;

            float speed = _targetCar.linearVelocity.magnitude * 3.6f;
            speedText.text = $"{Mathf.RoundToInt(speed)} km/h";
        }

        public void Show(Rigidbody carRigidbody)
        {
            _targetCar = carRigidbody;
            gameObject.SetActive(true); 
        }

        public void Hide()
        {
            _targetCar = null;
            gameObject.SetActive(false);
        }
    }
}