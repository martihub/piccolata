using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

/// <summary>
/// Ensures the correct animators are assigned to the characters.
/// </summary>
[InitializeOnLoad]
public class AnimatorAssigner
{
    private static string[] s_AnimatorGUIDS = new string[] { "00734c75e5484e24697dddaf47e8c152", "1c65957c39679034fb94019d52d6a984", "79f4dab00da40824fbd3697b6c773522",
                                                              "2d9ab56181c2ca34abcc6645243cf341", "e567772a993c11f448f9b69023c6cef6", "e58cef58c651b36498088253ec70c3ba",
                                                              "7d702f1c77d91684ab1774d5ce14a714"};
    private static Scene s_ActiveScene;

    /// <summary>
    /// Registers for the scene change callback.
    /// </summary>
    static AnimatorAssigner()
    {
        EditorApplication.update += Update;
    }

    /// <summary>
    /// The scene has been changed.
    /// </summary>
    private static void Update()
    {
        var scene = SceneManager.GetActiveScene();

        if (scene == s_ActiveScene) {
            return;
        }

        s_ActiveScene = scene;

        // Only the Behavior Designer sample scenes should be affected.
        if (!s_ActiveScene.path.Replace("\\", "/").Contains("Behavior Designer/Integrations/UltimateCharacterController/Demo")) {
            return;
        }

        // Find the animator controller reference.
        RuntimeAnimatorController animatorController = null;
        for (int i = 0; i < s_AnimatorGUIDS.Length; ++i) {
            var path = AssetDatabase.GUIDToAssetPath(s_AnimatorGUIDS[i]);
            if (!string.IsNullOrEmpty(path)) {
                animatorController = AssetDatabase.LoadAssetAtPath(path, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
                if (animatorController != null) {
                    break;
                }
            }
        }

        if (animatorController == null) {
            return;
        }

        var animators = Resources.FindObjectsOfTypeAll<Animator>();
        for (int i = 0; i < animators.Length; ++i) {
            // If the animator already has a controller then it can be skipped.
            if (animators[i].runtimeAnimatorController != null) {
                continue;
            }

            // The animator must exist within the scene.
            var path = AssetDatabase.GetAssetPath(animators[i]);
            if (!string.IsNullOrEmpty(path)) {
                continue;
            }

            // The animator needs a controller.
            animators[i].runtimeAnimatorController = animatorController;
            EditorUtility.SetDirty(animators[i]);
        }
    }
}
