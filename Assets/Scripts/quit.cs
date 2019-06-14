using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class quit : MonoBehaviour, IPointerUpHandler {
  public void OnPointerUp (PointerEventData eventData) {
    Application.Quit();
  }
}