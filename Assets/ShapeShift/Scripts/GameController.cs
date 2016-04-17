using UnityEngine;
using System.Collections;
using ShapeShift.Player;
using UnityEngine.SceneManagement;

namespace ShapeShift
{
    public class GameController : MonoBehaviour
    {
        public static readonly int LayerPlayer = 8;
        public static readonly int LayerLand = 9;
        public static readonly int LayerEnemy = 10;
        public static readonly int LayerBullet = 11;
        public static int Seed = 789;

        public PlayerController Player;

        public static float Score = 0;
        public static float Health = 100;

        public float MorphTime = 6f;
        public GameObject EndGamePanel;

        private IEnumerator MorphCoroutine;

        public void Start()
        {
            this.MorphCoroutine = MorphWait(Random.Range(this.MorphTime - 1f, this.MorphTime + 1f));
            StartCoroutine(this.MorphCoroutine);
        }

        public IEnumerator MorphWait(float seconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(seconds);
                this.Player.RandomMorph(Random.Range(0, 29));
            }
        }

        public void AddScore(float amount)
        {
            Score += amount;
        }

        public void Damage(float amount = 10f)
        {
            Health -= amount;
        }

        public void Heal(float amount = 10)
        {
            Health += amount;
        }

        public void ShowEndGame(bool success = false)
        {
            this.EndGamePanel.SetActive(true);
        }

        public void HideEndGame()
        {
            this.EndGamePanel.SetActive(false);
        }

        public void Restart()
        {
            GameController.Health = 100f;
            GameController.Score = 0f;
            Seed = (int)Random.Range(0, 500);
            this.HideEndGame();
            SceneManager.LoadScene("Level1");
        }

        public void SceneMainMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void SceneGame()
        {
            SceneManager.LoadScene("Level1");
        }
    }
}