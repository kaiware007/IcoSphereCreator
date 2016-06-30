using UnityEditor;
using UnityEngine;

public class IcoSphereWizard : ScriptableWizard {
	
	[MenuItem("Assets/Create/Ico Sphere")]
	private static void CreateWizard () {
		ScriptableWizard.DisplayWizard<IcoSphereWizard>("Create Ico Sphere");
	}
	
	[Range(1, 8)]
	public int level = 1;
	public float radius = 1f;
	
	private void OnWizardCreate () {
        Mesh mesh = IcoSphereCreator.Create(level, radius);

        string path = EditorUtility.SaveFilePanelInProject("Save Ico Sphere", "IcoSphere", "asset", "Specify where to save the mesh.");
        if (path.Length > 0)
        {
            MeshUtility.Optimize(mesh);
            AssetDatabase.CreateAsset(mesh, path);
            Selection.activeObject = mesh;
        }

    }
}