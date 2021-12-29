using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public enum EquipmentType
    {
        Radio,
        BBGun,
        FishingRod,
        NotApplicable
    }

    public EquipmentType Type;
    private InteractiveObject _myInteract;
    
    public GameObject Camera;
    private InteractiveObject _cameraInteract;
    private CameraEquipment _cameraEquip;

    // Start is called before the first frame update
    void Start()
    {
        _myInteract = GetComponent<InteractiveObject>();
        _cameraInteract = Camera.GetComponent<InteractiveObject>();
        _cameraEquip = Camera.GetComponent<CameraEquipment>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (_myInteract.Interactive)
        {
            Select();
        }
    }

    private void OnMouseUpAsButton()
    {
        if (!_myInteract.Selected)
        {
            DeSelect();
        }
    }

    public void Select()
    {
        _cameraInteract.Type = InteractiveObject.ObjectType.CanBeCombined;
        _cameraInteract.ObjectToCombineWith = _myInteract;
        _cameraInteract.CombinedAction.RemoveAllListeners();
        _cameraInteract.CombinedAction.AddListener(Equip);
        _cameraInteract.CombinedAction.AddListener(_cameraEquip.PeekThrough);
    }

    public void DeSelect()
    {
        _cameraInteract.Type = InteractiveObject.ObjectType.PromptTextOnClick;
        _cameraInteract.ObjectToCombineWith = null;
        _cameraInteract.CombinedAction.RemoveAllListeners();

    }
    public void Equip()
    {
        switch (Type)
        {
            case EquipmentType.Radio:
                HandManager.HoldRadio();
                break;
            case EquipmentType.BBGun:
                HandManager.HoldBBGun();
                break;
            default:
                break;
        }
    }
}
