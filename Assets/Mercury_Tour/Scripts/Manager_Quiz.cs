using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestionData
    {
        public string question;
        [Tooltip("Первый вариант всегда правильный")]
        public string[] answers;
    }

    [Header("Настройки UI")]
    [SerializeField] private TextMeshProUGUI questionTextUI;
    [SerializeField] private GameObject[] buttonParents;

    [Header("База вопросов")]
    [SerializeField] private List<QuestionData> questions;

    [Header("Аудио")]
    [SerializeField] private AudioClip clipEasterEgg;
    [SerializeField] private string triggerAnswer = "Я бывал"; 
    [SerializeField] private AudioClip clip5, clip3, clip0, clipFinal;

    private TextMeshProUGUI[] buttonLabels;
    private XRSimpleInteractable[] interactables;
    private int currentCorrectIndex;
    private int currentQuestion = 0;
    private int countCorrectAnswers = 0;
    private bool isFinalState = false; // Флаг: мы в финальном меню или нет

    void Awake()
    {
        buttonLabels = new TextMeshProUGUI[buttonParents.Length];
        interactables = new XRSimpleInteractable[buttonParents.Length];

        for (int i = 0; i < buttonParents.Length; i++)
        {
            int index = i;
            interactables[i] = buttonParents[i].GetComponentInChildren<XRSimpleInteractable>();
            buttonLabels[i] = buttonParents[i].GetComponentInChildren<TextMeshProUGUI>();

            interactables[i].selectEntered.AddListener((args) => OnButtonClick(index));
        }
    }

    void Start() => DisplayQuestion(0);

    public void DisplayQuestion(int questionIndex)
    {
        if (questionIndex >= questions.Count)
        {
            FinalDisplay();
            return;
        }

        QuestionData data = questions[questionIndex];
        questionTextUI.text = data.question;

        if (questionIndex == 4) 
        {
            buttonParents[2].SetActive(false);
            buttonParents[3].SetActive(false);
        }
        else
        {
            buttonParents[2].SetActive(true);
            buttonParents[3].SetActive(true);
        }

        string correctAnswerText = data.answers[0];
        List<string> shuffledAnswers = new List<string>(data.answers);

        for (int i = shuffledAnswers.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            string temp = shuffledAnswers[i];
            shuffledAnswers[i] = shuffledAnswers[rnd];
            shuffledAnswers[rnd] = temp;
        }

        for (int i = 0; i < buttonLabels.Length; i++)
        {
            if (i < shuffledAnswers.Count)
            {
                buttonLabels[i].text = shuffledAnswers[i];
                if (shuffledAnswers[i] == correctAnswerText) currentCorrectIndex = i;
            }
        }
    }

    private void OnButtonClick(int id)
    {
        if (isFinalState)
        {
            HandleFinalClick(id);
        }
        else
        {
            HandleAnswerClick(id);
        }
    }

    private void HandleAnswerClick(int id)
    {
        string pressedText = buttonLabels[id].text;

        if (pressedText == triggerAnswer)
        {
            Manager_Audio.Instance.PlayDirect(clipEasterEgg);

            countCorrectAnswers++; 
        }

        else if (id == currentCorrectIndex)
        {
            countCorrectAnswers++;
            Debug.Log("<color=green>ВЕРНО</color>");
        }

        currentQuestion++;

        if (currentQuestion < questions.Count)
        {
            DisplayQuestion(currentQuestion);
        }
        else
        {
            FinalDisplay();
        }
    }

    private void FinalDisplay()
    {
        isFinalState = true;

        if (countCorrectAnswers >= 5) Manager_Audio.Instance.PlayDirect(clip5);
        else if (countCorrectAnswers >= 3) Manager_Audio.Instance.PlayDirect(clip3);
        else Manager_Audio.Instance.PlayDirect(clip0);

        questionTextUI.text = $"Верных ответов: {countCorrectAnswers} из {questions.Count}.\nХотите повторить?";
        
        buttonParents[0].SetActive(true);
        buttonParents[1].SetActive(true);
        buttonParents[2].SetActive(false);
        buttonParents[3].SetActive(false);

        buttonLabels[0].text = "Нет";
        buttonLabels[1].text = "Да";
    }

    private void HandleFinalClick(int id)
    {
        if (id == 0)
        {
            Manager_Audio.Instance.PlayDirect(clipFinal);
            gameObject.SetActive(false);
        }
        else if (id == 1)
        {
            currentQuestion = 0;
            countCorrectAnswers = 0;
            isFinalState = false;
            DisplayQuestion(0);
        }
    }
}