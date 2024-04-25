using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public GameObject GameScreen;
    public GameObject WelcomeScreen;
    public GameObject QuestionScreen;
    public GameObject HelloCloud;
    public GameObject ConfirmScreen;


    public Text HelloText;
    public InputField NameInput;
    public GameScript GameManager;
    public NewControls newControls;

    private bool SpaceEnabled = false;
    private bool isConfirmEnd = true;
    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("Game").GetComponent<GameScript>();
        newControls = new NewControls();
        newControls.Enable();
        newControls.KeyboardMain.EndGame.performed += _ => { showConfirmScreen("end"); };
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && SpaceEnabled)
        {
            SpaceEnabled = false;
            HelloCloud.SetActive(false);
            GameManager.loadGame();
            QuestionScreen.SetActive(true);
            GameManager.Timer.isGameStarted = true;
        }

    }

    public void startGame()
    {
        string name = NameInput.text;
        WelcomeScreen.SetActive(false);
        GameScreen.SetActive(true);
        HelloCloud.SetActive(true);
        HelloText.text = $"Witaj {name}! Naciœnij spacjê, aby rozpocz¹æ grê";
        SpaceEnabled = true;
    }

    public void restartGame()
    {
        ConfirmScreen.SetActive(false);
        GameManager.EndGameScreen.SetActive(false);
        GameManager.restartGame();
    }

    public void goToWelcomeScreen()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SpaceEnabled = false;
    }

    public void hideFriendsAdviceSreen()
    {
        GameManager.CallingScreen.SetActive(false);
    }
    public void showConfirmScreen(string action)
    {
        ConfirmScreen.SetActive(true);
        Text displayedText = ConfirmScreen.GetComponentInChildren<Text>();
        if (action == "end")
        {
            isConfirmEnd = true;
            displayedText.text = $"Czy na pewno chcesz zakoñczyæ grê? Aktualna wygrana: {GameManager.currentPrize} z³";
        } else
        {
            isConfirmEnd = false;
            displayedText.text = "Czy na pewno chcesz zrestartowaæ grê?";
        }
    }
    public void confirmYes()
    {
        ConfirmScreen.SetActive(false);
        if (isConfirmEnd)
        {

            GameManager.gameOver(true);

        } else
        {
            
            restartGame();
        }
    }

    public void confirmNo()
    {
        ConfirmScreen.SetActive(false);
    }

}
