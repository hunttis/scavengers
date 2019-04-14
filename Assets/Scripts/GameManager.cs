using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  public float turnDelay = .1f;
  public static GameManager instance = null;
  public BoardManager boardScript;
  public int playerFoodPoints = 100;
  [HideInInspector] public bool playersTurn = true;

  private List<Enemy> enemies;
  private bool enemiesMoving;
  private int level = 3;

  void Awake() {
    if (instance == null) {
      instance = this;
    } else if (instance != this) {
      Destroy(gameObject);
    }

    DontDestroyOnLoad(gameObject);
    boardScript = GetComponent<BoardManager>();
    enemies = new List<Enemy>();
    InitGame();
  }

  void InitGame() {
    enemies.Clear();
    boardScript.SetupScene(level);
  }

  public void GameOver() {
    enabled = false;
  }

  IEnumerator MoveEnemies() {
    enemiesMoving = true;
    yield return new WaitForSeconds(turnDelay);
    if (enemies.Count == 0) {
      yield return new WaitForSeconds(turnDelay);
    }

    for (int i = 0; i < enemies.Count; i++) {
      enemies[i].MoveEnemy();
      yield return new WaitForSeconds(enemies[i].moveTime);
    }

    playersTurn = true;
    enemiesMoving = false;
  }

  public void AddEnemyToList(Enemy script) {
    enemies.Add(script);
  }

  void Update() {
    if (playersTurn || enemiesMoving) {
      return;
    }

    StartCoroutine(MoveEnemies());
  }
}
