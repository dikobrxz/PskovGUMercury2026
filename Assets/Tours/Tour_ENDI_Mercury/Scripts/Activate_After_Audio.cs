using UnityEngine;

namespace Tour_ENDI_TourStub1
{
    public class Activate_After_Audio : MonoBehaviour
    {
        public int targetAudioToFinish;
        public GameObject objectToActive;
        void OnEnable()
        {
            Manager_Audio.OnAudioFinished += CheckAudio;
        }
        void OnDisable()
        {
            Manager_Audio.OnAudioFinished -= CheckAudio;
        }
        void Start()
        {
            objectToActive.SetActive(false);
        }
        private void CheckAudio(int finishedAudio)
        {
            if (finishedAudio == targetAudioToFinish)
            {
                objectToActive.SetActive(true);
    
                Manager_Audio.OnAudioFinished -= CheckAudio;
                this.enabled = false;
            }
        }
    }
}
