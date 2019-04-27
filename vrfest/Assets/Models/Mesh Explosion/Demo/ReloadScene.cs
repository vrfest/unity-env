using UnityEngine;

public class ReloadScene : MonoBehaviour {
	
#if MESH_EXPLOSION_DEMO_BUILD
	// The scenes have to be set up correctly in the project settings for this to work:
	
	string[] sceneNames = new string[] { "Simulated Gravity", "Full Physics", "Basic Variations" };
	
	void OnGUI() {
		GUILayout.Label("Test scenes:");
		var n = sceneNames.Length;
		for (var i = 0; i < n; ++i) {
			var buttonLabel = sceneNames[i];
			if (i == Application.loadedLevel) buttonLabel += " (Reload)";
			if (GUILayout.Button(buttonLabel)) {
				Application.LoadLevel(i);
			}
		}
	}
#else
	void OnGUI() {
		if (GUILayout.Button("Reload")) Reload();
	}
#endif
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			Reload();
		}
	}
	
	void Reload() {
#if UNITY_5_4_OR_NEWER
		UnityEngine.SceneManagement.SceneManager.LoadScene(
			UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
#else
		Application.LoadLevel(Application.loadedLevel);
#endif
	}
	
}
