using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private const float MaxDistance = 5f;

    public GameObject CurrentObservedObject { get; private set; }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, MaxDistance))
        {
            OnLookAtObject(hit.collider.gameObject);
        }
        else
        {
            OnObjectLost();
        }
    }

    private void OnLookAtObject(GameObject observedObject)
    {
        if (!Equals(CurrentObservedObject, observedObject))
        {
            CurrentObservedObject = observedObject;
            Debug.Log(CurrentObservedObject.name);
        }
    }

    private void OnObjectLost()
    {
        if (CurrentObservedObject)
        {
            CurrentObservedObject = null;
        }
    }
}
