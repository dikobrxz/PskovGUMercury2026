using UnityEngine;
using System.Collections.Generic;

namespace Tour_ENDI_TourStub1
{
    public class Manager_UI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private List<GameObject> _hints;
    
        private void Start()
        {
            if (_hints != null)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    _hints.Add(transform.GetChild(i).gameObject);
                }
            }
            UpdateUI();
        }
    
        private void OnEnable()
        {
            Manager_Stages.OnStageChanged += UpdateUI;
        }
    
        private void OnDisable()
        {
            Manager_Stages.OnStageChanged -= UpdateUI;
        }
    
        private void UpdateUI()
        {
            int _currentStage = (int)Manager_Stages.Instance._currentStage;
            foreach (GameObject hint in _hints) hint.SetActive(false);
            switch (_currentStage)
            {
                case 0: _hints[0].SetActive(true); break;//_hint.text = "Для начала миссии нажмите на штурвал корабля."; break;
                case 1: _hints[1].SetActive(true); break;//_hint.text = "Исследуйте местность. Двигайтесь по направлению к навигационным маякам."; break;
                case 2: _hints[2].SetActive(true); break;//_hint.text = "Остерегайтесь падения в кратеры"; break;
                case 3: _hints[3].SetActive(true); break; //_hint.text = "Исследуйте место падения спутника"; break;
                case 4: _hints[4].SetActive(true); break; //_hint.text = "Система заблокирована. Введите код доступа на панели терминала"; break;
                case 5: _hints[5].SetActive(true); break; //_hint.text = "Доступ разрешён. Введите точные координаты для отправки спасательного модуля"; break;
                case 6: _hints[6].SetActive(true); break; //_hint.text = "Координаты приняты. Найдите и нажмите кнопку эвакуации на спутнике"; break;
                case 7: _hints[7].SetActive(true); break; //_hint.text = "Миссия успешно завершена. Пройдите викторину"; break;
                case 8: _hints[8].SetActive(true); break; //_hint.text = "Удачи в следующем приключении"; break;
            }
        }
    }
}
