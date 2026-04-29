using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.Collections;

public class Player_Teleport : MonoBehaviour
{
    [SerializeField] private Image _fade;
    [SerializeField] public float _duration;
    [Header("Settings telepor points")]
    [SerializeField] private Transform _SpawnPointMercury;
    [SerializeField] private Transform _SpawnPointShip;

    public static Player_Teleport Instance;
    private CharacterController _controller;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _fade.canvasRenderer.SetAlpha(0.0f);
    }

    public void Teleport()
    {
        if ((int)Manager_Stages.Instance._currentStage < 7)
        {
            StartCoroutine(DoTeleport(_SpawnPointMercury));
            Debug.Log("Телепортация на Меркурий завершена!");
        }
        else
        {    
            StartCoroutine(DoTeleport(_SpawnPointShip));
            Manager_Stages.Instance._quiz.SetActive(true);
            Debug.Log("Телепортация на корабль завершена!");
        }
    }

    public IEnumerator DoTeleport(Transform point)
    {
        Debug.Log("Кнопка нажата!");

        _fade.CrossFadeAlpha(1.0f, _duration, false);
        yield return new WaitForSeconds(_duration);

        _controller = GetComponent<CharacterController>();
        _controller.enabled = false;

        transform.position = point.position;
        transform.rotation = point.rotation;

        _controller.enabled = true;

        yield return new WaitForSeconds(_duration);

        _fade.CrossFadeAlpha(0.0f, _duration, false);
        yield return new WaitForSeconds(_duration);

        Debug.Log("Телепортация завершена!");
    }
}