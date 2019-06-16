using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private Button buttonClicked;
    public bool quit; /*!< Script utilisé pour la fin de jeu pour quitter ou revenir au menu, permet d'utiliser un seul scritp pour les deux actions */
    // Start is called before the first frame update
    void Start()
    {
        buttonClicked = GetComponent<Button>();
        if(quit){
            buttonClicked.onClick.AddListener(QuitGame);
        }
        else{
            buttonClicked.onClick.AddListener(GoToMenu);
        }
    }

    private void QuitGame(){
        Debug.Log("Quit App");
        Application.Quit();
    }

    private void GoToMenu(){
        // SceneManager.LoadScene(0);
        PhotonNetwork.LeaveRoom();
    }

    void OnLeftRoom(){
        SceneManager.LoadScene(0);
    }
}
