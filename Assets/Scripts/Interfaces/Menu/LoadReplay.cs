using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadReplay : MonoBehaviour {
   public bool init;
	// Use this for initialization
	void Start () {
	    init = false;
	    DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	public void onClick(){
	    SceneManager.LoadScene("Replay", LoadSceneMode.Single);
	}
	
    void Update(){
		if(Application.loadedLevelName == "Multiplayer_W1"){
			Destroy(gameObject);
		}
	}
}
