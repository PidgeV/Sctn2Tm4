using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherUIManager : MonoBehaviour
{
	[SerializeField] private GameObject mainMenu;
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject CreditsMenu;
	[SerializeField] private GameObject GameUI;

	public static OtherUIManager Instance;

	bool pause = false;

	private void Start()
	{
		Instance = this;
		GameUI.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ShowPauseMenu(pause);
		}
	}

	public void Play(bool hardMode)
	{
		// Start a game
		GameUI.SetActive(true);
		mainMenu.SetActive(false);

		StartCoroutine(GameManager.Instance.init(hardMode));
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void ShowPauseMenu(bool show)
	{
		pause = !pause;

		if (show)
		{
			pauseMenu.SetActive(true);
			Time.timeScale = 0;
		}
		else
		{
			pauseMenu.SetActive(false);
			Time.timeScale = 1;
		}
	}

	public void ShowCredits(bool show)
	{
		if (show)
		{
			CreditsMenu.SetActive(true);
			mainMenu.SetActive(false);
		}
		else
		{
			CreditsMenu.SetActive(false);
			mainMenu.SetActive(true);
		}
	}

	public void QuitGame()
	{
		mainMenu.SetActive(true);
		GameManager.Instance.EndGame();
	}
}
