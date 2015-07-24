using UnityEngine;
using System.Collections;
using System.IO;
using Boomlagoon.JSON;

public class BookmarkWriter : MonoBehaviour {

	public string outputFileName = "";

	private StreamWriter sw;
	private string outputPath;
	private JSONObject jsonOutput;
	private JSONArray bookmarks;

	// Use this for initialization
	void Start () {
		outputPath = "Assets/Resources/" + outputFileName;
		if (File.Exists (outputPath)) {
			Debug.Log (outputPath + " exists -- doing something else");
			return;
		}

		bookmarks = new JSONArray ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("b")) {
			AddBookmark ();
		}
	}

	// add bookmark at 
	void AddBookmark() {
		JSONObject newBookmark = CreateJSONBookmark (this.gameObject);
		bookmarks.Add (newBookmark);
	}

	// write bookmark file before application close
	void OnApplicationQuit() {
		WriteJSONToFile ();
	}

	// create bookmark composed of position and rotation objects
	private JSONObject CreateJSONBookmark(GameObject gameObject) {
		JSONObject bookmark = new JSONObject ();

		// Add position
		JSONObject position = new JSONObject();
		position.Add ("x", gameObject.transform.position.x);
		position.Add ("y", gameObject.transform.position.y);
		position.Add ("z", gameObject.transform.position.z);
		bookmark.Add ("position", position);

		// Add rotation
		JSONObject rotation = new JSONObject();
		rotation.Add ("x", gameObject.transform.rotation.x);
		rotation.Add ("y", gameObject.transform.rotation.y);
		rotation.Add ("z", gameObject.transform.rotation.z);
		rotation.Add ("w", gameObject.transform.rotation.w);
		bookmark.Add ("rotation", rotation);

		return bookmark;
	}

	// save bookmarks in JSON format to output file in Resources directory
	private void WriteJSONToFile() {
		if (bookmarks.Length != 0) {
			jsonOutput = new JSONObject ();
			jsonOutput.Add ("bookmarks", bookmarks);

			sw = new StreamWriter (outputPath);
			sw.Write (jsonOutput.ToString ());
			sw.Close ();
		}
	}
}