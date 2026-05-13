using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.XR.CoreUtils;

using OldPoseDriver = UnityEngine.SpatialTracking.TrackedPoseDriver;
using NewPoseDriver = UnityEngine.InputSystem.XR.TrackedPoseDriver;

public class Player_Teleport : MonoBehaviour
{
    [SerializeField] private Image _fade;
    [SerializeField] public float _duration = 1f;
    [Header("Settings teleport points")]
    [SerializeField] private Transform _SpawnPointMercury;
    [SerializeField] private Transform _SpawnPointShip;
    [SerializeField] private XROrigin _xrOrigin;
    [SerializeField] private CharacterController _controller;
    
    [Header("Tracked Pose Drivers")]
    [SerializeField] private Component[] _allPoseDrivers; 
    
    public static Player_Teleport Instance;
    
    void Awake()
    {
        Instance = this;
        
        if (_xrOrigin == null) 
            _xrOrigin = GetComponent<XROrigin>();
        if (_controller == null) 
            _controller = GetComponent<CharacterController>();
        
        if (_allPoseDrivers == null || _allPoseDrivers.Length == 0)
        {
            FindAllPoseDrivers();
        }
    }
    
    void Start()
    {
        if(_fade != null) 
            _fade.canvasRenderer.SetAlpha(0f);
    }
    
    public void Teleport()
    {
        if (Manager_Stages.Instance._currentStage < Manager_Stages.Stages.Rescue)
        {
            StartCoroutine(DoTeleport(_SpawnPointMercury));
        }
        else
        {    
            StartCoroutine(DoTeleport(_SpawnPointShip));
            if(Manager_Stages.Instance._quiz != null)
                Manager_Stages.Instance._quiz.SetActive(true);
        }
    }
    
    public IEnumerator DoTeleport(Transform point)
    {
        Debug.Log("Телепортация началась!");
        
        Transform locomotion = transform.Find("Locomotion");
        if (locomotion != null) locomotion.gameObject.SetActive(false);
        
        if (_fade != null)
        {
            _fade.CrossFadeAlpha(1f, _duration, false);
            yield return new WaitForSeconds(_duration);
        }
        
        foreach (var driver in _allPoseDrivers)
        {
            if (driver != null)
            {
                if (driver is OldPoseDriver oldDrvr) oldDrvr.enabled = false;
                if (driver is NewPoseDriver newDrvr) newDrvr.enabled = false;
            }
        }
        
        if (_controller != null) _controller.enabled = false;
        
        if (_xrOrigin != null)
        {
            _xrOrigin.MoveCameraToWorldLocation(point.position);
            _xrOrigin.MatchOriginUpCameraForward(point.up, point.forward);
            
            if (_xrOrigin.CameraFloorOffsetObject != null)
            {
                _xrOrigin.CameraFloorOffsetObject.transform.localPosition = new Vector3(0, 1.3f, 0);
            }
        }
        else
        {
            transform.position = point.position;
            transform.rotation = point.rotation;
        }
        
        if (_controller != null) _controller.enabled = true;
        
        foreach (var driver in _allPoseDrivers)
        {
            if (driver != null)
            {
                if (driver is OldPoseDriver oldDrvr) oldDrvr.enabled = true;
                if (driver is NewPoseDriver newDrvr) newDrvr.enabled = true;
            }
        }

        yield return new WaitForSeconds(_duration);

        if (locomotion != null) locomotion.gameObject.SetActive(true);
        
        if (_fade != null)
        {
            _fade.CrossFadeAlpha(0f, _duration, false);
        }
        
        Debug.Log($"Телепортация завершена успешно!");
    }
    
    [ContextMenu("Find All Pose Drivers")]
    public void FindAllPoseDrivers()
    {
        var oldDrivers = GetComponentsInChildren<OldPoseDriver>(true);
        var newDrivers = GetComponentsInChildren<NewPoseDriver>(true);

        var allFound = new System.Collections.Generic.List<Component>();
        allFound.AddRange(oldDrivers);
        allFound.AddRange(newDrivers);

        _allPoseDrivers = allFound.ToArray();
        
        Debug.Log($"<color=cyan>[PoseFinder]</color> Найдено всего драйверов: {_allPoseDrivers.Length}");
        foreach (var driver in _allPoseDrivers)
        {
            Debug.Log($"- На объекте: <b>{driver.gameObject.name}</b> (Тип: {driver.GetType().Name})");
        }
    }
}