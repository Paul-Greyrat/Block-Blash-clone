using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image hooverImage;
    public Image activeImage;
    public Image nomalImage;
    public List<Sprite> nomalImages;
    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool Squareoccupied { get; set; }


    void Start()
    {
        Selected = false;
        Squareoccupied = false;
    }

    public bool CanWeUseThisSquare()
    {
        return hooverImage.gameObject.activeSelf;
    }
    public void PlaceShapeOnBroad()
    {
        ActivateSquare();   
    }
    public void ActivateSquare()
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        Squareoccupied = true;
    }

    public void SetImage(bool setFirtIamge)
    {
        nomalImage.GetComponent<Image>().sprite = setFirtIamge ? nomalImages[1] : nomalImages[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Squareoccupied == false)
        {
            Selected = true;
            hooverImage.gameObject.SetActive(true);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        if (Squareoccupied == false)
        {
            hooverImage.gameObject.SetActive(true);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Squareoccupied == false)
        {
            Selected = false;
            hooverImage.gameObject.SetActive(false);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UnSetOccupied();
        }
    }

}
