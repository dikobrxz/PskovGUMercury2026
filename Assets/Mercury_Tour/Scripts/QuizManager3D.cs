using UnityEngine;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class QuestionData {
    public string questionText;
    [Tooltip("Правильный ответ первый в списке")]
    public string[] answers; 
}

public class QuizManager3D : MonoBehaviour
{
    [Header("Настройки UI")]
    public TMP_Text questionLabel;
    
    [Header("3D Кнопки")]
    public GameObject[] cubeButtons;
    public Transform[] spawnPoints;

    [Header("Вопросы")]
    public QuestionData[] questions;
    
    private int currentIdx = 0;
    private string currentCorrectAnswer;

    void Start() {
        if (questions.Length > 0) ShowQuestion();
    }

    void ShowQuestion() {
        if (currentIdx >= questions.Length) {
            FinishQuiz();
            return;
        }

        var q = questions[currentIdx];
        questionLabel.text = q.questionText;
        currentCorrectAnswer = q.answers[0];

        for (int i = 0; i < cubeButtons.Length; i++) {
            if (i < q.answers.Length) {
                cubeButtons[i].SetActive(true);

                cubeButtons[i].GetComponentInChildren<TMP_Text>().text = q.answers[i];
            } else {
                cubeButtons[i].SetActive(false);
            }
        }

        ShuffleCubes();
    }

    void ShuffleCubes() {
        List<Transform> availablePoints = new List<Transform>(spawnPoints);
        foreach (GameObject cube in cubeButtons) {
            if (cube.activeSelf) {
                int randomIndex = Random.Range(0, availablePoints.Count);
                cube.transform.position = availablePoints[randomIndex].position;
                availablePoints.RemoveAt(randomIndex);
            }
        }
    }

    public void OnCubeClicked(GameObject clickedCube) {
        string clickedText = clickedCube.GetComponentInChildren<TMP_Text>().text;

        if (clickedText == currentCorrectAnswer) {
            Debug.Log("Верно!");
            currentIdx++;
            ShowQuestion();
        } else {
            Debug.Log("Неверно!");
        }
    }

    void FinishQuiz() {
        questionLabel.text = "Викторина пройдена!";
        foreach (var cube in cubeButtons) cube.SetActive(false);
        Manager_Audio.Instance.PlayStageVoice();
    }
}