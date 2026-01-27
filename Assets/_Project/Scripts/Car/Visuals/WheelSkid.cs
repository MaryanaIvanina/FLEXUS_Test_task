using UnityEngine;

public class WheelSkid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WheelCollider targetWheel;
    [SerializeField] private TrailRenderer trailRenderer;

    [Header("Settings")]
    [SerializeField] private float slipThreshold = 0.4f;

    private void Update()
    {
        WheelHit hit;
        if (targetWheel.GetGroundHit(out hit))
        {
            float slip = Mathf.Abs(hit.sidewaysSlip);

            if (slip > slipThreshold)
            {
                trailRenderer.emitting = true;

                transform.position = hit.point + Vector3.up * 0.02f;
            }
            else trailRenderer.emitting = false;
        }
        else trailRenderer.emitting = false;
    }
}