using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject pauseMenu;
    //Script permettant d'afficher ou cacher le menu passé en variable publique "pauseMenu"
    //L'affichage s'active/se désactive avec Echap
    //Utilisé pour ouvrir un menu en jeu

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}
