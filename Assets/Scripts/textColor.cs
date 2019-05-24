using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class textColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

  public Text theText;

  private void Start() {
    theText.color = new Color32(205,0,85,255); 
  }

  public void OnPointerEnter (PointerEventData eventData) {
    theText.color =  new Color32(205,0,85,255); 
  }

  public void OnPointerExit (PointerEventData eventData) {
    theText.color = new Color32(208,54,126,255); 
  }

  public void OnPointerDown (PointerEventData eventData) {
    theText.color = new Color32(110,18,115,255); 
  }

  public void OnPointerUp (PointerEventData eventData) {
    theText.color = new Color32(205,0,85,255); 
  }
}