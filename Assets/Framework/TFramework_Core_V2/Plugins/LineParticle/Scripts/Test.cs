using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var lineparticle = GameObject.Find("LineParticle").GetComponent<LineParticle>();
		lineparticle.SetPosition(0, new Vector3(1, 2, 3));
		var posiont_1 = lineparticle.GetPosition(1);
		posiont_1.x = 4f;
		posiont_1.y = 5f;
		posiont_1.z = 6f;
		lineparticle.SetPosition(1, posiont_1);
	}
}
