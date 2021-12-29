using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnUsedContainer : MonoBehaviour
{
    public float OpenDuration = 5f;
    private bool _isOpen = false;
    private Animator _animator;
    private Coroutine _lastCoroutine = null;


    // Start is called before the first frame update
    void Start()
    {

        _animator = GetComponent<Animator>();
    }

    public void MouseEnterEvent()
    {

        Open();
        StopCoroutine(_lastCoroutine);
    }

    public void MouseExitEvent()
    {
        Close();
    }

    public void Open()
    {

        if (_isOpen)
        {
            StopCoroutine(_lastCoroutine);
        }
        else
        {
            _animator.SetTrigger("Open");
        }

        _isOpen = true;
        _lastCoroutine = StartCoroutine(WaitThenClose());
    }

    IEnumerator WaitThenClose()
    {
        yield return new WaitForSeconds(OpenDuration);
        Close();
    }
    public void Close()
    {
        _animator.SetTrigger("Close");
        _isOpen = false;
    }
}
