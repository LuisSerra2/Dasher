using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameManager
{
    Idle,
    Playing,
    Dead
}

public interface IGameStateController
{
    public abstract void Idle();
    public abstract void Playing();
    public abstract void Dead();
}

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GameManager gameManager = GameManager.Idle;
    private List<IGameStateController> gameStateControllers = new List<IGameStateController>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameStateControllers.AddRange(FindObjectsOfType<MonoBehaviour>().OfType<IGameStateController>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ChangeState(GameManager.Playing);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeState(GameManager.Dead);
        }

        foreach (var controller in gameStateControllers)
        {
            switch (gameManager)
            {
                case GameManager.Idle:
                    controller.Idle();
                    break;
                case GameManager.Playing:
                    controller.Playing();
                    break;
                case GameManager.Dead:
                    controller.Dead();
                    break;

            }
        }

    }
    public void ChangeState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void RegisterController(IGameStateController controller)
    {
        gameStateControllers.Add(controller);
    }
}
