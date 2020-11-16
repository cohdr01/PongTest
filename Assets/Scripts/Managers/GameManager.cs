using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Private members
    private bool gameEnded = false;
    private Ball _ball;
    GameObject theBall;
    private static int _leftPlayerScore;
    private static int _rightPlayerScore;
    #endregion

    public GUISkin layout;

    #region Editor exposed properties
    [SerializeField] private int _scoreToWin = 3;
    [SerializeField] private int _matchWaitSeconds = 3;
    #endregion

    // Singleton
    public static GameManager Instance { get; private set; }

    private void Start()
    {
        // Singleton init
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        // Find the ball
        theBall = GameObject.FindGameObjectWithTag("Ball"); // FindObjectOfType<Ball>();
        if (theBall == null)
        {
            Debug.LogError("theBall not found!");
            Application.Quit();
            return;
        }


        // Basic init
        UI.Instance.UpdatePlayersScores(_leftPlayerScore, _rightPlayerScore);

        theBall.SendMessage("Reset", 0.5f, SendMessageOptions.RequireReceiver);

        _ball = FindObjectOfType<Ball>();
        if (_ball == null)
        {
            Debug.LogError("_ball not found!");
            //Application.Quit();
            //return;
        }

        _ball.EnteredEndZone += BallOnEnteredEndZone;

        StartCoroutine(StartNewMatch());
    }

    /// <summary>
    /// Starts a new match
    /// </summary>
    private IEnumerator StartNewMatch()
    {
        _ball.Reset();

        // Sanity
        if (_matchWaitSeconds <= 0)
        {
            _matchWaitSeconds = 3;
        }

        // Wait before starting new match (Countdown)
        // TODO: Using this coroutine, show a countdown message for _matchWaitSeconds seconds (Use UI.Instance.ChangeMainMessage)

        // Start
        _ball.GiveRandomVelocity();

        // TODO: Remove this yield break
        yield break;
    }

    /// <summary>
    /// Restarts the entire game
    /// </summary>
    private void StartNewGame()
    {
        Application.LoadLevel(0);
    }

    public static void Score(string wallID)
    {
        if (wallID == "RightWall")
        {
            _leftPlayerScore++;
        }
        else
        {
            _rightPlayerScore++;
        }

        UI.Instance.UpdatePlayersScores(_leftPlayerScore, _rightPlayerScore);
    }

    /// <summary>
    /// Event listener to handle goal
    /// </summary>
    /// <param name="endZoneType">The goal side that the ball entered</param>
    private void BallOnEnteredEndZone(EndZone.EndZoneType endZoneType)
    {
        StartCoroutine(ShowGoalMessageAndHandleGoal(endZoneType == EndZone.EndZoneType.Left ? Player.PlayerType.Right : Player.PlayerType.Left));
    }

    /// <summary>
    /// Shows a message for the goal
    /// </summary>
    /// <param name="endZoneType">The end zone that was</param>
    /// <returns></returns>
    private IEnumerator ShowGoalMessageAndHandleGoal(Player.PlayerType scoringPlayer)
    {
        // TODO: Increase the correct player's score

        // Update score
        UI.Instance.UpdatePlayersScores(_leftPlayerScore, _rightPlayerScore);


        // Show message / Handle game victory
        // TODO: Handle victory condition (_scoreToWin)
        bool isGameOver = false;
        if (isGameOver)
        {
            // TODO: Show message which player has won
            // TODO: Wait 3 seconds before starting a new game
            StartNewGame();
        }
        else
        {
            // TODO: Show message which player has scored
            // TODO: Wait 3 seconds before starting a new match
            StartCoroutine(StartNewMatch());
        }

        // TODO: Remove this "Yield break" after you've implemented the above
        yield break;
    }


    void OnGUI()
    {
        GUI.skin = layout;
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + _leftPlayerScore);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + _rightPlayerScore);

        if (GUI.Button(new Rect(Screen.width / 2 - 60, 20, 120, 53), "RESTART"))
        {
            _leftPlayerScore = 0;
            _rightPlayerScore = 0;
            theBall.SendMessage("Reset", 0.5f, SendMessageOptions.RequireReceiver);
        }

        if (_leftPlayerScore == _scoreToWin)
        {
            GUI.Label(new Rect(Screen.width / 2 - 200, 200, 2000, 1000), "LEFT PLAYER HAS WON!");
            theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
            gameEnded = true;

        }
        else if (_rightPlayerScore == _scoreToWin)
        {
            GUI.Label(new Rect(Screen.width / 2 - 200, 200, 2000, 1000), "RIGHT PLAYER HAS WON!");
            theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
            gameEnded = true;
        }

        if (gameEnded)
        {
            // TODO: Wait for 3 seconds 
            //_leftPlayerScore = 0;
            //_rightPlayerScore = 0;
            //theBall.SendMessage("Reset", 0.5f, SendMessageOptions.RequireReceiver);
            gameEnded = false;

        }
    }
}