using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using ResidentEvilClone;
using System;

public class ElectricLock : MonoBehaviour, IInteractable, IComponentSavable
{

    [SerializeField] private GameObject _camera;
    [SerializeField] private float _countdown = 1f;
    [SerializeField] private float _deactivate = 2f;
    [SerializeField] private AudioClip _unlockSound;
    [SerializeField] LockableDoor lockedDoor;

    [SerializeField] UnityEvent unlockedLoad;

    public bool _isLocked = true;


    public void Interact()
    {
        if (_isLocked)
        {
            StartCoroutine(Unlock());
        }
    }

    private IEnumerator Unlock()
    {
        StartUnlock();

        yield return new WaitForSecondsRealtime(_countdown);

        Unlocking();

        yield return new WaitForSecondsRealtime(_deactivate);

        FinishUnlock();
    }

    private void FinishUnlock()
    {
        _camera.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = true;
        lockedDoor.Locked = false;
    }

    private void Unlocking()
    {
        unlockedLoad?.Invoke();
        SoundManagement.Instance.PlaySound(_unlockSound);
    }

    private void StartUnlock()
    {
        _isLocked = false;

        Cursor.visible = false;
        _camera.SetActive(true);
        Time.timeScale = 0;
    }

    public string GetSavableData()
    {
        print(_isLocked);
        return _isLocked.ToString();
    }

    public void SetFromSaveData(string savedData)
    {
        _isLocked = Convert.ToBoolean(savedData);
        if (!_isLocked)
        {
            unlockedLoad?.Invoke();
        }
    }
}