using UnityEngine;

public class StageTriggerProxy : MonoBehaviour
{
    public System.Action OnPlayerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke();
        }
    }
}
