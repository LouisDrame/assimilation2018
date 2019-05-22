using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class textColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

  public Text theText;

  public void OnPointerEnter (PointerEventData eventData) {
    theText.color =  new Color32(205,0,85,255); //Or however you do your color
    Debug.Log("Enter");
  }

  public void OnPointerExit (PointerEventData eventData) {
    theText.color = new Color32(208,54,126,255); //Or however you do your color
    Debug.Log("Exit");
  }

  public void OnPointerDown (PointerEventData eventData) {
    theText.color = new Color32(110,18,115,255); //Or however you do your color
  }

  public void OnPointerUp (PointerEventData eventData) {
    theText.color = new Color32(205,0,85,255); //Or however you do your color
  }
}