using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image nomalImage;
    public List<Sprite> nomalImages;
    void Start()
    {

    }
    public void SetImage(bool setFirtIamge)
    {
        nomalImage.GetComponent<Image>().sprite = setFirtIamge ? nomalImages[1] : nomalImages[0];
    }

}
