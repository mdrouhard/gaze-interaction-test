using UnityEngine;
using System.Collections;
using System.IO;

public class BookmarkSaver : MonoBehaviour {

	public string outputFileName = "";

	private StreamWriter sw;
	private string outputPath;

	// Use this for initialization
	void Start () {
		if (File.Exists (outputFileName)) {
			Debug.Log (outputFileName+" exists -- doing something else");
			return;
		}

		outputPath = "Assets/Resources/" + outputFileName;
		sw = new StreamWriter (outputPath);

//		var sr = File.CreateText (File);
//		sr.WriteLine ("File created.");
//		sr.Close ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("j")) {
			AddBookmark ();
			Debug.Log ("Adding bookmark");
		}
	}

	public void AddBookmark() {
		sw.WriteLine ("Adding a Line");
	}

	void OnApplicationQuit() {
		sw.Close ();
	}
}
