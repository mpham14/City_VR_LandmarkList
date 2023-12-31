using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour {

    [SerializeField] private InputField ParticipantIDInput;
	[SerializeField] private Dropdown ExperimentTypeDropdown;
    [SerializeField] private Dropdown SessionDropdown;
    [SerializeField] private InputField ExperimenterInitialsInput;
	[SerializeField] private Dropdown StartLocationCMDropdown;
	[SerializeField] private Dropdown LandmarkGoalCMDropdown;
	[SerializeField] private GameObject FirstPanel;
	[SerializeField] private GameObject APCPanel;
	[SerializeField] private GameObject TSCPanel;

	public static ExperimentSettings _expInstance;

    private void Start()
    {
		_expInstance = ExperimentSettings.GetInstance ();
		_expInstance.MazeSettings = new MazeSettings ();

		if (!string.IsNullOrEmpty(_expInstance.ExperimenterInitials))  // if this already exists then we're coming out of a maze
			OpenSubmenu ();
		else {
			PopulateDropdown ();
		}
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Escape)) 
			Application.Quit();
        if (Input.GetKey(KeyCode.E) && FirstPanel.activeSelf == false)
			SceneManager.LoadScene(0);
    }

	public void PopulateDropdown() {
		string[] ExperimentTypeEnumNames = Enum.GetNames(typeof(ExperimentTypeEnum));
		List<string> ExperimentTypes = new List<string> (ExperimentTypeEnumNames);
		ExperimentTypeDropdown.AddOptions (ExperimentTypes);
	}

	public void EnterExperimentInfo() {
		_expInstance.ParticipantID = ParticipantIDInput.text;
		_expInstance.ExperimenterInitials = ExperimenterInitialsInput.text;
		_expInstance.ExperimentType = (ExperimentTypeEnum)ExperimentTypeDropdown.value;
        _expInstance.Session = (int)SessionDropdown.value + 1;
        _expInstance.Date = DateTime.Now;
		SetDir (_expInstance);
		OpenSubmenu();
    }

	void OpenSubmenu() {
		// Enable the cursor because it's disabled coming out of maze scenes
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		// QUESTION: Could this be simplified? Still have to line up the right menu w/ the experiment type selected.
		FirstPanel.SetActive (false);
        if (ExperimentSettings.IsAPC()) APCPanel.SetActive(true);
        else if (ExperimentSettings.IsTSC()) TSCPanel.SetActive(true);
	}

	public void SetDir(ExperimentSettings _expInstance)
    {
		string currentDir = System.IO.Directory.GetCurrentDirectory ();
        string newDir = "star" + _expInstance.ParticipantID +
            "_" + _expInstance.ExperimentType +
            "_Sess" + _expInstance.Session.ToString();
        _expInstance.FileDir = currentDir + "\\" + newDir + "_" + DateTime.Now.ToString("ddHHmm");
		System.IO.Directory.CreateDirectory(_expInstance.FileDir);
        _expInstance.FileName = _expInstance.FileDir + "\\" + newDir;
    }
    
    /*public void EnableTPStudyConditions() {
        if (_expInstance.ExperimentType == ExperimentTypeEnum.ExpTP_SEQ) _expInstance.MazeSettings.MultiIntro = true;
        else if (_expInstance.ExperimentType == ExperimentTypeEnum.ExpTP_SIM) _expInstance.MazeSettings.SingleIntro = true;
    }*/
    
    public void EnableArrows () { _expInstance.MazeSettings.Arrows = true; }
	public void EnableReverse () { _expInstance.MazeSettings.Reverse = true; }

	public void LoadMazeJoystickPractice () {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.JP;
        SceneManager.LoadScene(1);
    }

	public void LoadMazeVisuomotorExpertise () {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.VME;
		SceneManager.LoadScene(2);
    }

	public void LoadMazeTaskPractice () {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.TP;
		SceneManager.LoadScene(3);
	}

	public void LoadMazeA() {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.A;
		SceneManager.LoadScene(4);
	}

	public void LoadMazeB() {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.B;
		SceneManager.LoadScene(5);
	}
}