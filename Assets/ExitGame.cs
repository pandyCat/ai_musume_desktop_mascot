using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Unity�G�f�B�^�[�ł̓���
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ���ۂ̃Q�[���I������
        Application.Quit();
#endif
    }
}
