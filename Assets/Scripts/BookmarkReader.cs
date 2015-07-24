using UnityEngine;
using System.Collections;
using System.IO;
using Boomlagoon.JSON;

public class BookmarkReader : MonoBehaviour {

	public string inputFileName = "";
	
	private StreamWriter sw;
	private string inputPath;
	private JSONObject jsonInput;
	private JSONArray bookmarks;

	// Use this for initialization
	void Start () {
		inputPath = "Assets/Resources/" + inputFileName;
		if (!File.Exists (inputPath)) {
			Debug.Log (inputPath + " does not exist.");
			return;
		}

		ReadJSONBookmarks ();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// read in locations in order and create array of locations
	void ReadJSONBookmarks() {	
		bookmarks = new JSONArray ();
	}
}
