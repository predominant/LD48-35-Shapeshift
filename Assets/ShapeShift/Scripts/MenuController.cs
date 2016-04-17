using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ShapeShift
{
    public class MenuController : MonoBehaviour
    {

        public void Play()
        {
            SceneManager.LoadScene("Level1");
        }

        public void Menu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void Instructions()
        {
            SceneManager.LoadScene("Instructions");
        }

        public void Doge()
        {
            SceneManager.LoadScene("Doge");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}