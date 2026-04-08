using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Player_Teleport : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _SpawnPointMercury;
    [SerializeField] private Transform _SpawnPointShip;
    [SerializeField] private float _delay = 10f;
    public void Teleport()
    {
        StartCoroutine(TeleportSequence());
    }

    private IEnumerator TeleportSequence()
    {
        DoTeleport(_SpawnPointMercury);
        Debug.Log("Телепортация на Меркурий завершена!");

        yield return new WaitForSeconds(_delay);

        DoTeleport(_SpawnPointShip);
        Debug.Log("Телепортация на корабль завершена!");
    }

    private void DoTeleport(Transform point)
    {
        Debug.Log("Кнопка нажата!");
        if ( point == null)
        {
            Debug.LogError("Не назначена точка телепортации!");
            return;
        }

        transform.position = point.position;
        transform.rotation = point.rotation;

        Debug.Log("Телепортация завершена!");
    }
}