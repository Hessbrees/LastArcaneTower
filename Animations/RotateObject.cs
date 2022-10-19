using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] protected float rotationSpeed = 1f;

    protected virtual void Update() => transform.Rotate(0, 0, rotationSpeed, Space.Self);
}
