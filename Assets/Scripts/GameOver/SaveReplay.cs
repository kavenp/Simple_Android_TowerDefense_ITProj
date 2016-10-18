using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SaveReplay : MonoBehaviour {

    private string fileName;
	private string fileOut;
	public Text replaySaved;
	byte[] replay;
	// Use this for initialization
	void Start () {
	    #if UNITY_ANDROID
	    fileName = Application.persistentDataPath + "/tempReplay.dat";
		fileOut = Application.persistentDataPath + "/saveReplay.dat";
	    #else
	    fileName = "tempReplay.dat";
		fileOut = "saveReplay.dat";
	    #endif
	}
	
	// Update is called once per frame
	public void onClick(){
	    replay = System.IO.File.ReadAllBytes(fileName);
		var stream = new FileStream(fileOut, FileMode.Create);
		stream.Write(replay,0,replay.Length);
		replaySaved.text = "Replay Saved!";
	}
}
