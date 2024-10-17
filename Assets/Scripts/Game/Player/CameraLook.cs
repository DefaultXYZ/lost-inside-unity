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
            OnObjectLost();
            CurrentObservedObject = observedObject;
            if (CurrentObservedObject != null && CurrentObservedObject.TryGetComponent(out IObservable observable))
            {
                observable.OnLookReceived();
            }
        }
    }

    private void OnObjectLost()
    {
        if (CurrentObservedObject != null)
        {
            if (CurrentObservedObject.TryGetComponent(out IObservable observable))
            {
                observable.OnLookLost();
            }

            CurrentObservedObject = null;
        }
    }
}