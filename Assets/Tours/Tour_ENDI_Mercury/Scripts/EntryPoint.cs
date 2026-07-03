using System;
using System.Collections;
using TourverseToolkit.Runtime;
using UnityEngine;

namespace Tour_ENDI_TourStub1
{
    public class EntryPoint : MonoBehaviour
    {
        private PlayerCamera playerCamera;

        private void Start()
        {
            playerCamera = FindFirstObjectByType<PlayerCamera>();
            StartTour();
        }


        private void StartTour()
        {
            TourController.TourStart(playerCamera);
            StartCoroutine(CallbackAfter30Secs(CallCheckPoint));
        }

        private void CallCheckPoint()
        {
            TourController.CheckPoint(1);
        }

        private IEnumerator CallbackAfter30Secs(Action callback)
        {

            yield return new WaitForSeconds(30f);
            callback?.Invoke();
            Debug.Log("Checkpoint reached");
        }

    }
}