using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum StartText {}

public class UIListener : MonoBehaviour {

	/*private UnityAction listener;
	[SerializeField] private Text text;

	void Awake(){
		listener = new UnityAction (Arrows);
		ExperimentSettings _expInstance = ExperimentSettings.GetInstance ();
	}

	void OnEnable () {
		EventManager.StartListening ("Arrows", listener);
	}

	void OnDisable() {
		EventManager.StopListening ("Arrows", listener);
	}

	void Arrows() {
		text.text = "Please follow the arrows.\r\n\r\n Hit the spacebar to begin.";
	}*/
}