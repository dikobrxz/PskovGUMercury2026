using UnityEngine;

namespace Tour_ENDI_TourStub1
{
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
}
