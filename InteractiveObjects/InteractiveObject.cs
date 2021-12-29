using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{

    public enum ObjectType
    {
        PromptTextOnClick,
        CanBeCombined
    }

    public bool Interactive = true;
    public static bool UniversalInteractable = true;

    public bool PlayDefaultClickSound = true;
    public AudioClip CustomOnClickSound;

    public ObjectType Type = ObjectType.PromptTextOnClick;

    public string Text;

    public InteractiveObject ObjectToCombineWith;
    public static CombineManager CombineManager;
    public UnityEvent CombinedAction;

    [System.NonSerialized]
    public bool Selected = false;

    [System.NonSerialized]
    public bool Hovered = false;

    public UnityEvent OnClick;

    private AssetStore _assetStore;

    private Material _highlightMat;
    private Material _unhighlightMat;

    private Color _highlightColor;
    private Color _selectedColor;


    private Renderer _myRenderer;
    public InteractiveTextContainer ThoughtContainer;

    private AudioManager _audioManager;


    // Start is called before the first frame update
    void Start()
    {
        _myRenderer = GetComponent<Renderer>();
        _unhighlightMat = _myRenderer.material;

        _assetStore = GameObject.FindGameObjectWithTag("GameController").GetComponent<AssetStore>();
        _audioManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioManager>();
        _highlightMat = _assetStore.HighlightMaterial;
        _highlightColor = _highlightMat.color;


        float h,s,v;
        Color.RGBToHSV(_highlightColor, out h, out s, out v);

        h = h*100;
        h = (h + 25) % 100;
        h = h / 100;
        _selectedColor = Color.HSVToRGB(h, s, v);


        if (ThoughtContainer == null)
        {
            GameObject.FindGameObjectWithTag("ThoughtBubble")?.TryGetComponent<InteractiveTextContainer>(out ThoughtContainer);
        }
        
        if(CombineManager == null)
        {
            CombineManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<CombineManager>();
        }

    }

    private void Update()
    {
        if(!UniversalInteractable)
        {
            Deselect();
        }
    }


    private void OnMouseEnter()
    {
        MouseEnterFunc();
    }

    public void MouseEnterFunc()
    {
        if (Interactive && UniversalInteractable)
        {
            if (CombineManager.InCombineMode)
            {
                _myRenderer.material = _highlightMat;
            }
            else
            {
                _myRenderer.material = _highlightMat;
            }
            Hovered = true;
        }
    }

    private void OnMouseExit()
    {
        MouseExitFunc();
    }

    public void MouseExitFunc()
    {
        if (Interactive && UniversalInteractable)
        {
            if (CombineManager.InCombineMode)
            {
                if (!Selected)
                {
                    _myRenderer.material = _unhighlightMat;

                }
            }
            else
            {
                _myRenderer.material = _unhighlightMat;
                _highlightMat.color = _highlightColor;
            }

        }
        Hovered = false;
    }

    private void OnMouseDown()
    {
        MouseDownFunc();
        if (Interactive && UniversalInteractable)
        {
            if (PlayDefaultClickSound)
            {
                _audioManager.PlayButtonClickSound();
            }
            else if(CustomOnClickSound != null)
            {
                _audioManager.PlaySound(CustomOnClickSound);
            }
        }
    }

    public void MouseDownFunc()
    {
        if (Interactive && UniversalInteractable)
        {
            if (CombineManager.InCombineMode)
            {
                _myRenderer.material = _highlightMat;

            }
            else
            {
                _highlightMat.color = _selectedColor;
                _myRenderer.material = _highlightMat;

            }
        }
    }

    private void OnMouseUpAsButton()
    {
        MouseUpFunc();
    }

    public void MouseUpFunc()
    {
        if (Interactive && UniversalInteractable)
        {
            if (CombineManager.InCombineMode)
            {
                Selected = !Selected;
                if (Selected)
                {
                    CombineManager.NumOfSelected++;
                }
                else
                {
                    CombineManager.NumOfSelected--;

                }
                CombineManager.CheckSelected(this);


            }
            else
            {
                if (Type.Equals(ObjectType.PromptTextOnClick))
                {
                    if(!String.IsNullOrWhiteSpace(Text))
                        ThoughtContainer.ChangeText(Text);
                    _highlightMat.color = _highlightColor;
                    OnClick.Invoke();
                }

                else if (Type.Equals(ObjectType.CanBeCombined))
                {
                    Selected = true;
                    CombineManager.InCombineMode = true;
                    CombineManager.NumOfSelected++;
                    CombineManager.CheckSelected(this);

                }
            }

        }
    }

    public void Deselect()
    {
        
        Selected = false;
        Hovered = false;
        _myRenderer.material = _unhighlightMat;
        _highlightMat.color = _highlightColor;
    }
}
