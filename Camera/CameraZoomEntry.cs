using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomEntry : MonoBehaviour
{
    public static bool FirstTime = true;
    public static bool ZoomReady = false;

    private float _leftBoundary;
    private float _rightBoundary;
    private ButtonPromptManager _buttonPromptManager;

    public Transform LeftBoundaryReference;
    public Transform RightBoundaryReference;

    public InteractiveTextContainer ThoughtBubble;

    private Texture2D _highlightCursor;

    private MoveCamera _player;

    private bool _highLighted;

    private OnHoverHint _hint;

    // Start is called before the first frame update
    void Start()
    {
        _buttonPromptManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ButtonPromptManager>();
        AssetStore assetStore = GameObject.FindGameObjectWithTag("GameController").GetComponent<AssetStore>();
        _highlightCursor = assetStore.ZoomCursor;

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveCamera>();

        
        _leftBoundary = LeftBoundaryReference.position.x;
        _rightBoundary = RightBoundaryReference.position.x;

        if (FirstTime)
        {
            _hint = gameObject.AddComponent<OnHoverHint>() as OnHoverHint;
            _hint.HintText = "I think I can zoom in here...";
            _hint.ThoughtBubble = ThoughtBubble;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0) && _highLighted) //left click
        {
            if (FirstTime)
            {
                FirstTime = false;
                _hint.CloseHint();
            }
            ZoomReady = false;
            _player.MoveToDestination(transform.position, (_leftBoundary, _rightBoundary));
            CursorManager.SetCursor(CursorManager.NormalCursor);
        }

        if (!FirstTime && _hint)
        {
            Destroy(_hint);
        }
    }

    private void OnMouseOver()
    {
        if (!_highLighted && !MoveCamera.Zooming && !MoveCamera.Panning && !MovementManager.CamLocked)
        {
            CursorManager.SetCursor(_highlightCursor);
            _highLighted = true;
            ZoomReady = true;
            _buttonPromptManager.ChangeInteractText("Zoom In");
        }
    }

    private void OnMouseExit()
    {
        CursorManager.SetCursor(CursorManager.NormalCursor);
        _highLighted = false;
        ZoomReady = false;
        _buttonPromptManager.ResetInteractText();
    }

    public (float, float) GetBoundary()
    {
        return (_leftBoundary, _rightBoundary);
    }
}
