using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class GameMenuVisibilityTest
{
    private GameScreen gameScreen;
    private PauseMenu pauseMenu;
    private WinMenu winMenu;
    private GameOverMenu gameOverMenu;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Load the desired scene for testing
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

        yield return null; // Wait for a frame to let any potential Start methods execute
    }

    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator PauseMenuNotOpenWhenGameWon()
    {
        gameScreen = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<GameScreen>();
        pauseMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<PauseMenu>();
        winMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<WinMenu>();

        gameScreen.CloseAllGameMenus();
        gameScreen.SetIsGameOver(false);
        pauseMenu.SetIsGamePaused(false);

        winMenu.WinGame(true);

        pauseMenu.TogglePausedGame();

        Assert.IsTrue(!pauseMenu.GetIsGamePaused(), "Game isn't paused when game won");

        yield return null;
    }

    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator PauseMenuNotOpenWhenGameLost()
    {
        gameScreen = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<GameScreen>();
        pauseMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<PauseMenu>();
        gameOverMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<GameOverMenu>();

        gameScreen.CloseAllGameMenus();
        gameScreen.SetIsGameOver(false);
        pauseMenu.SetIsGamePaused(false);

        gameOverMenu.GameOver(true);

        pauseMenu.TogglePausedGame();

        Assert.IsTrue(!pauseMenu.GetIsGamePaused(), "Game isn't paused when game over");

        yield return null;
    }

    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator WinMenuNotOpenWhenPaused()
    {
        gameScreen = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<GameScreen>();
        pauseMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<PauseMenu>();
        winMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<WinMenu>();

        gameScreen.CloseAllGameMenus();
        gameScreen.SetIsGameOver(false);
        pauseMenu.SetIsGamePaused(false);

        pauseMenu.TogglePausedGame();

        winMenu.WinGame(true);

        Assert.IsTrue(!gameScreen.GetIsGameOver(), "Win menu not open when game is paused");

        yield return null;
    }

    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator GameOverMenuNotOpenWhenPaused()
    {
        gameScreen = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<GameScreen>();
        pauseMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<PauseMenu>();
        gameOverMenu = GameObject.FindGameObjectWithTag("GameUIDocument").GetComponent<GameOverMenu>();

        gameScreen.CloseAllGameMenus();
        gameScreen.SetIsGameOver(false);
        pauseMenu.SetIsGamePaused(false);

        pauseMenu.TogglePausedGame();

        gameOverMenu.GameOver(true);

        Assert.IsTrue(!gameScreen.GetIsGameOver(), "Game over menu not open when game is paused");

        yield return null;
    }
}