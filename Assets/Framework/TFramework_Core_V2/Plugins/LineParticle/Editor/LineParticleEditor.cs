using UnityEngine;
using UnityEditor;
using System.Collections;

public class LineParticleEditor : EditorWindow
{

	[MenuItem ("LineParticle/Create Line", false, 1)]
	public static void CreateLineParticle ()
	{
		var lineGo = new GameObject ("LineParticle");
		var lineCtrl = lineGo.AddComponent<LineParticle> ();
		Selection.activeObject = lineGo;
	}
}
