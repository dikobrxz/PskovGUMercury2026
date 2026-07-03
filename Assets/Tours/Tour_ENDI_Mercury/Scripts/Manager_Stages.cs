using UnityEngine;

namespace Tour_ENDI_TourStub1
{
    public class Manager_Stages : MonoBehaviour
    {    
        [Header("Триггеры этапов")]
        [SerializeField] private GameObject[] stageTriggers;
    
        public static Manager_Stages Instance;
        public delegate void StageChanged();
        public static event StageChanged OnStageChanged;
        public enum Stages {Start, Exploration, CraterQuest, SatelliteFound, PasswordStage, CoordinatesStage, Rescue, Quiz, End}
        public Stages _currentStage;
    
    
        private void Awake()
        {
            Instance = this;
        }
    
        void Start()
        {
            for (int i = 0; i < stageTriggers.Length; i++)
            {
                int index = i;
                var proxy = stageTriggers[i].GetComponent<StageTriggerProxy>();
    
                proxy.OnPlayerEnter = () => {OnTriggerReached(index);};
            }
    
            //_quiz.SetActive(false);
            //_screen.SetActive(false);
        }
    
        public void NextStage()
        {
            switch ((int)_currentStage)
            {
                case 0: Player_Teleport.Instance.Teleport(); break;
                case 6: Player_Teleport.Instance.Teleport(); break;
            }
    
            if ((int)_currentStage < 8)
            {
                _currentStage = (Stages)((int)++_currentStage);
            
                Manager_Audio.Instance.PlayStageVoice();
    
                OnStageChanged?.Invoke();
            }
        }
    
        public int GetIndexCurrentStage()
        {
            return (int)_currentStage;
        }
    
        private void OnTriggerReached(int index)
        {
            NextStage();
            stageTriggers[index].SetActive(false);
            Debug.Log($"Триггер {index} сработал и отключен.");
        }
    }
}
