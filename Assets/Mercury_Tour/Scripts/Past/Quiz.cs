using UnityEngine;
using UnityEngine.UI;
using TMPro;

    [System.Serializable]
    public class QuestionData2 {
    public string questionText;
    public string[] answers; // Правильный ответ всегда под индексом 0
    }

public class Quiz : MonoBehaviour
{
    public TMP_Text _questionLabel;
    public Button[] _answerButtons;
    public QuestionData[] _allQuestions;

    private int currentIdx = 0;

    void Start()
    {
        ShowQuestion();
    }

    void ShowQuestion() {
        if (currentIdx >= _allQuestions.Length) {
            Debug.Log("Викторина окончена!");
            Manager_Audio.Instance.PlayStageVoice();
            return;
        }

        var q = _allQuestions[currentIdx];
        _questionLabel.text = q.questionText;

        for (int i = 0; i < _answerButtons.Length; i++) {
            if (i < q.answers.Length) {
                _answerButtons[i].gameObject.SetActive(true);
                _answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.answers[i];
                
                string val = q.answers[i];
                _answerButtons[i].onClick.RemoveAllListeners();
                _answerButtons[i].onClick.AddListener(() => CheckAnswer(val, q.answers[0]));
            }
            else
            {
                _answerButtons[i].gameObject.SetActive(false);
            }
        }
        
        RandomizeButtons();
    }

    void RandomizeButtons() {
        foreach (var btn in _answerButtons) {
            btn.transform.SetSiblingIndex(Random.Range(0, _answerButtons.Length));
        }
    }

    void CheckAnswer(string clicked, string correct) {
        if (clicked == correct) {
            currentIdx++;
            ShowQuestion();
        } else {
            Debug.Log("Неверно!");
        }
    }
}