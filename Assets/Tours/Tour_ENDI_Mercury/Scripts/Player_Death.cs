using UnityEngine;

namespace Tour_ENDI_TourStub1
{
    public class Player_Death : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Триггер коллайдеров");
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player_Teleport>().Teleport();
            }
        }
    }
}
