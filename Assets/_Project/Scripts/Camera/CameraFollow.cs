using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new(0, 5, -10); 
    [SerializeField] private float smoothSpeed = 0.01f;

    private Transform _target;

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desiredPosition = _target.position + _target.TransformDirection(offset);

        Vector3 targetPos = _target.position - _target.forward * Mathf.Abs(offset.z) + Vector3.up * offset.y;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(_target);
    }
}