using UnityEngine.SceneManagement;

namespace Utils.Services
{
    public class SceneService
    {
        public void LoadScene(int buildIndex) => SceneManager.LoadScene(buildIndex);
        
        public int GetActiveSceneBuildIndex() => SceneManager.GetActiveScene().buildIndex;
    }
}