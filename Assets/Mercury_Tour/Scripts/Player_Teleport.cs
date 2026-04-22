using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Player_Teleport : MonoBehaviour
{
    private CharacterController _controller;
    [Header("Settings")]
    [SerializeField] private Transform _SpawnPointMercury;
    [SerializeField] private Transform _SpawnPointShip;
    public void Teleport()
    {
        if ((int)Manager_Stages.Instance._currentStage < 6)
        {
            DoTeleport(_SpawnPointMercury);
            Debug.Log("Телепортация на Меркурий завершена!");
        }
        else
        {    
            DoTeleport(_SpawnPointShip);
            Debug.Log("Телепортация на корабль завершена!");
        }
    }

    private void DoTeleport(Transform point)
    {
        Debug.Log("Кнопка нажата!");
        if ( point == null)
        {
            Debug.LogError("Не назначена точка телепортации!");
            return;
        }

        _controller = GetComponent<CharacterController>();
        _controller.enabled = false;

        transform.position = point.position;
        transform.rotation = point.rotation;

        _controller.enabled = true;

        Debug.Log("Телепортация завершена!");
    }
}