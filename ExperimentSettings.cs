using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExperimentTypeEnum { APC, TSC }
public enum MazeNameEnum { JP, VME, TP, A, B }
public class MazeSettings {
	public MazeNameEnum MazeName;
    public bool Reverse = false;
    public bool ReachedEnd = false;

    public bool Arrows = false;

	//public bool SingleIntro = false;
	//public bool MultiIntro = false;
	//public string StartLocationCM = null; // Refactor for CM-specific script?
	//public string LandmarkGoalCM = null;

    public string TrialName;
}

public class ExperimentSettings {
	public string ParticipantID;
	public string ExperimenterInitials;
	public DateTime Date;
	public ExperimentTypeEnum ExperimentType;
    public int Session = 0;
    public MazeSettings MazeSettings;
	public string FileDir;
	public string FileName;
    public IDictionary<string, int> TrialTracker = new Dictionary<string, int>();

    //singleton:	
    private static ExperimentSettings _instance;

	public static ExperimentSettings GetInstance() {
		if (_instance == null) _instance = new ExperimentSettings();
		return _instance;
	}
    
	private ExperimentSettings(){}

    public static void NameAndCountTrial() {
        _instance.MazeSettings.TrialName = _instance.MazeSettings.MazeName.ToString();
        if (ExperimentSettings.IsStudy() && !ExperimentSettings.IsPractice()) _instance.MazeSettings.TrialName += "_LearnT";

        else if (!ExperimentSettings.IsStudy() && !ExperimentSettings.IsPractice()) {
            if (_instance.MazeSettings.Reverse) _instance.MazeSettings.TrialName += "_FWD";
            else _instance.MazeSettings.TrialName += "_REV";
        }

        if (_instance.TrialTracker.ContainsKey(_instance.MazeSettings.TrialName)) _instance.TrialTracker[_instance.MazeSettings.TrialName] += 1;
        else _instance.TrialTracker[_instance.MazeSettings.TrialName] = 1;
    }

    public static bool IsPractice() {
        if (_instance.MazeSettings.MazeName == MazeNameEnum.JP ||
            _instance.MazeSettings.MazeName == MazeNameEnum.VME ) return true;
        else return false;
    }

    public static bool IsStudy() {
        if (
            
            _instance.MazeSettings.Arrows
            //_instance.MazeSettings.Pause ||
            //_instance.MazeSettings.Rotate ||
            //_instance.MazeSettings.SingleIntro ||
            //_instance.MazeSettings.MultiIntro 
       
            ) return true;
        else return false;
    }

    public static bool IsAPC() {
        if (_instance.ExperimentType == ExperimentTypeEnum.APC) return true;
        else return false;
    }

    public static bool IsTSC()
    {
        if (_instance.ExperimentType == ExperimentTypeEnum.TSC) return true;
        else return false;
    }
}



