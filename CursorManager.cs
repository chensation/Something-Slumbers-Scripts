using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public static Texture2D NormalCursor;
    private AssetStore _assetStore;
    
    private NPCManager _npcManager;
    private Radio _radio;

    private bool _inCameraScene;

    void Start()
    {
        _assetStore = GameObject.FindGameObjectWithTag("GameController").GetComponent<AssetStore>();
        _inCameraScene = GameManager.CurrentScene.Equals(GameManager.Scene.Camera);

        if (!_inCameraScene)
        {
            SetCursor(_assetStore.NormalCursor);
            NormalCursor = _assetStore.NormalCursor;
        }
        else
        {
            if (HandManager.Currently.Equals(HandManager.state.HoldingRadio))
            {
                _radio = GameObject.FindGameObjectWithTag("RadioUI").GetComponent<Radio>();
                _npcManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<NPCManager>();

                SetCursor(_assetStore.RadioCursor1);
                NormalCursor = _assetStore.RadioCursor1;

            }
            else if (HandManager.Currently.Equals(HandManager.state.HoldingBBGun)){
                SetCursor(_assetStore.BBGunCursor);
                NormalCursor = _assetStore.BBGunCursor;
            }

            else
            {
                SetCursor(_assetStore.CameraCursor);
                NormalCursor = _assetStore.CameraCursor;
            }

        }


    }

    private void Update()
    {
        if (_inCameraScene)
        {
            if (HandManager.Currently.Equals(HandManager.state.HoldingRadio))
            {
                ChangeRadioCursor();
            }
            else if (DialogueManager.InDialogue)
            {
                SetCursor(_assetStore.NormalCursor);
                NormalCursor = _assetStore.NormalCursor;
            }
        }
        if (CombineManager.InCombineMode)
        {
            SetCursor(_assetStore.CombineCursor);
        }

    }

    public void ChangeRadioCursor()
    {
        if (_npcManager.ClosestNPC != null)
        {
            var npcFrequency = _npcManager.ClosestNPC.RadioFrequency;
            var npcRadioTolerance = _npcManager.ClosestNPC.Tolerance;
            if (_radio.Frequency < npcFrequency + npcRadioTolerance && _radio.Frequency > npcFrequency - npcRadioTolerance)
            {
                if (NormalCursor != _assetStore.RadioCursor3)
                {
                    SetCursor(_assetStore.RadioCursor3);
                    NormalCursor = _assetStore.RadioCursor3;
                }

            }
            else if (_radio.Frequency < npcFrequency + npcRadioTolerance * 5 && _radio.Frequency > npcFrequency - npcRadioTolerance * 5)
            {
                if (NormalCursor != _assetStore.RadioCursor2)
                {
                    SetCursor(_assetStore.RadioCursor2);
                    NormalCursor = _assetStore.RadioCursor2;
                }
            }
            else
            {
                if (NormalCursor != _assetStore.RadioCursor1)
                {
                    SetCursor(_assetStore.RadioCursor1);
                    NormalCursor = _assetStore.RadioCursor1;
                }
            }
        }   
        
        
    }

    public static void SetCursor(Texture2D cursor)
    {
        Vector2 hotSpot = new Vector2(cursor.width / 2f, cursor.height / 2f);
        Cursor.SetCursor(cursor, hotSpot, CursorMode.ForceSoftware);
    }
}
