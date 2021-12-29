using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAlbum : MonoBehaviour
{
    public const int NumPhotos = 4;

    public static int PhotoIndex = 0;
    public static string[] Photos = new string[NumPhotos];
    public static bool AlbumOpen = false;

    public GameObject AlbumUI;
    public RawImage[] _photoImages = new RawImage[NumPhotos];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AlbumOpen && (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonDown(1))) {
            CloseAlbum();
        }
    }

    public static void SavePhoto(string path)
    {
        Photos[PhotoIndex] = path;
        PhotoIndex = (PhotoIndex+1) % NumPhotos;
    }

    public void OpenAlbum()
    {

        for(int i = 0; i< NumPhotos; i++)
        {
            var tempColor = _photoImages[i].color;

            if (!string.IsNullOrEmpty(Photos[i]))
            {
                byte[] byteArray = File.ReadAllBytes(Photos[i]);
                Texture2D tex = new Texture2D(2, 2);

                var test = ImageConversion.LoadImage(tex, byteArray);

                _photoImages[i].texture = tex;

                tempColor.a = 1;
            }
            else
            {
                tempColor.a = 0;
            }

            _photoImages[i].color = tempColor;
        }

        AlbumUI.SetActive(true);
        AlbumOpen = true;
        GameManager.AllowExitGame = false;
        InteractiveObject.UniversalInteractable = false;

    }

    public void CloseAlbum()
    {
        AlbumUI.SetActive(false);
        AlbumOpen = false;
        GameManager.AllowExitGame = true;
        InteractiveObject.UniversalInteractable = true;

    }
}
