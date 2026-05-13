using UnityEngine;
using TMPro;

public class Manager_UI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _hint;
    //[SerializeField] private TextMeshProUGUI _coordinates;

    private void OnEnable()
    {
        Manager_Stages.OnStageChanged += UpdateUI;
    }

    private void OnDisable()
    {
        Manager_Stages.OnStageChanged -= UpdateUI;
    }

    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        int _currentStage = (int)Manager_Stages.Instance._currentStage;

        switch (_currentStage)
        {
            case 0: _hint.text = "Для начала миссии нажмите на штурвал корабля."; break;
            case 1: _hint.text = "Исследуйте местность. Двигайтесь по направлению к навигационным маякам."; break;
            case 2: _hint.text = "Остерегайтесь падения в кратеры"; break;
            case 3: _hint.text = "Исследуйте место падения спутника"; break;
            case 4: _hint.text = "Система заблокирована. Введите код доступа на панели терминала"; break;
            case 5: _hint.text = "Доступ разрешён. Введите точные координаты для отправки спасательного модуля"; break;
            case 6: _hint.text = "Координаты приняты. Найдите и нажмите кнопку эвакуации на спутнике"; break;
            case 7: _hint.text = "Миссия успешно завершена. Пройдите викторину"; break;
            case 8: _hint.text = "Удачи в следующем приключении"; break;
        }
        // _stage.text = "Этап: " + (Manager_Stages.Stages)_currentStage;

        // if (_currentStage == 5)
        // {
        //     _coordinates.text += "Координаты: " + Manager_Panel._correctAnswer;
        // }

        // if (_currentStage == 6)
        // {
        //     _coordinates.text = "";
        // }
    }
}
