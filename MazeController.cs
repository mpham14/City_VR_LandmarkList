using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MazeController : MonoBehaviour
{

    public Text text;
    public GameObject player;
    public GameObject arrows;
    public GameObject startHall;
    public GameObject endHall;
    public GameObject overheadCam;
    public Transform startLoc;
    public Transform endLoc;
    public GameObject minimap;
    public GameObject landmarkList;
    static public bool hasEnded;
    public bool hasStarted;
    static public Material pathColor;
    static public float totalTime;
    public Sprite[] introImgs;
    public static GameObject subjectInstance;
    public static UnityStandardAssets.Characters.FirstPerson.FirstPersonController controller;
    private Vector3 lastPos;
    private Vector3 currentPos;
    private int picCounter;
    static private float totalDistance;
    static private float avgVelocity;
    static private List<string> path;
    public GameObject[] landmarks;
    static public ExperimentSettings _expInstance;
    private List<string> experimentInfo;
    public int durationPerPic;

    static public Transform cornerTransform;
    static public int onCorner;
    static public bool cornerEvent = true;
    

    void Start()
    {
        _expInstance = ExperimentSettings.GetInstance();
        InitMaze();
        totalDistance = 0;
        totalTime = 0;
        path = new List<string>();
        hasEnded = false;
        hasStarted = false;
        picCounter = 0;
        subjectInstance = GameObject.FindWithTag("Player");
        controller = subjectInstance.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        controller.enabled = false;
        lastPos = subjectInstance.transform.position;
        currentPos = subjectInstance.transform.position;
        GameObject _goTransform = new GameObject();
        cornerTransform = _goTransform.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !hasEnded)
        {
            MazeEnd();
            SceneManager.LoadScene(0);
        }

        else if (Input.GetKeyDown(KeyCode.E) && hasEnded)
            SceneManager.LoadScene(0);

        if (Input.GetKeyDown("space") && !hasStarted)
        {

            ExperimentSettings.NameAndCountTrial();
            StartCoroutine(WaitAndStart());
            InvokeRepeating("TrackPathEverySecond", 2.0f, 1.0f);
            controller.enabled = true;

            /*if (_expInstance.MazeSettings.MultiIntro) StartCoroutine(MultiPicIntro());
			else if (_expInstance.MazeSettings.SingleIntro)
                StartCoroutine(SinglePicIntro());
            else {
                StartCoroutine(WaitAndStart());
                InvokeRepeating("TrackPathEverySecond", 2.0f, 1.0f);
                controller.enabled = true;
            }*/
        }

        // Trial has begun, do the following:
        if (hasStarted && !hasEnded)
        {
            currentPos = subjectInstance.transform.position;
            totalDistance += Vector3.Distance(currentPos, lastPos);
            totalTime += Time.deltaTime;
            lastPos = currentPos;
        }
    }

    private void LateUpdate()
    {

    }

    public void InitMaze()
    {

        if (_expInstance.MazeSettings.MazeName == MazeNameEnum.JP)
        {
            Instantiate(player, startLoc.transform);
        }

        else if (_expInstance.MazeSettings.MazeName == MazeNameEnum.VME)
        {
            Instantiate(player, startLoc.transform);
            endHall.AddComponent<End>();
            text.text = "Please navigate to the end of the hall as quickly as possible\r\n\r\n Hit the spacebar to begin.";
        }

        // Load AP, SPT, TP conditions
        else if ((ExperimentSettings.IsAPC() || ExperimentSettings.IsTSC()) && !ExperimentSettings.IsPractice())
        {

			if (_expInstance.MazeSettings.Arrows) {
				text.text = "Please follow the arrows.\r\n\r\nHit the spacebar to begin.";
				arrows.SetActive (true);
			}
			else
            {
                text.text = "Please recreate the route.\r\n\r\nHit the spacebar to begin.";
                arrows.SetActive(false);
            }

            if (_expInstance.MazeSettings.Reverse)
            {
                Instantiate(player, endLoc.transform);
                startHall.AddComponent<End>();
            }
            else  // was an else, might come back to this
            {
                Instantiate(player, startLoc.transform);
                endHall.AddComponent<End>();
            }
        }
    }

    IEnumerator WaitAndStart()
    {
        text.text = "+";
        yield return new WaitForSeconds(1);
        Image img = GameObject.Find("Panel").GetComponent<Image>();
        img.color = UnityEngine.Color.clear;

        if (ExperimentSettings.IsAPC() && ExperimentSettings.IsStudy())
        {
            RawImage rawimg = minimap.GetComponent<RawImage>();
            rawimg.color = Color.white;
        }

        if (ExperimentSettings.IsTSC() && ExperimentSettings.IsStudy()) {
            landmarkList.SetActive(true);
        }

        hasStarted = true;
        text.text = "";
        controller.enabled = true;
    }

    IEnumerator WaitToCheckDistance()
    {
        yield return new WaitForSeconds(2f);

    }

    public void TrackPathEverySecond()
    {
        int second = Mathf.RoundToInt(totalTime);
        path.Add(second.ToString() + ": " + subjectInstance.transform.position);
    }

    /*IEnumerator PauseWithOptions(int seconds, bool enableCtrlAfter = true, bool grabAfter = false, bool rotateAfter = false, string textAfter = "")
    {
        controller.enabled = false;
        yield return new WaitForSeconds(seconds);
        controller.enabled = enableCtrlAfter;
        text.text = textAfter;
    }*/

    /*IEnumerator SinglePicIntro()
    {
        text.text = "";
        Image img = GameObject.Find("Panel").GetComponent<Image>();
        img.color = UnityEngine.Color.white;
		if (_expInstance.MazeSettings.MazeName == MazeNameEnum.TP) picCounter = 2;
		else picCounter = 8; // magic number for montage pic
        img.sprite = introImgs[picCounter];
		yield return new WaitForSeconds(introImgs.Length * durationPerPic);
        img.sprite = null;
        img.color = UnityEngine.Color.black;
        StartCoroutine(WaitAndStart());
    }

    IEnumerator MultiPicIntro()
    {
        text.text = "";
        Image img = GameObject.Find("Panel").GetComponent<Image>();
		while (picCounter <= introImgs.Length - 2)
        {
            img.color = UnityEngine.Color.white;
            img.sprite = introImgs[picCounter];
            yield return new WaitForSeconds(durationPerPic);
            picCounter++;
        }
        img.color = UnityEngine.Color.black;
        StartCoroutine(WaitAndStart());
    }*/

    static public void MazeEnd()
    {
        hasEnded = true;
        controller.enabled = false;
        Text text = GameObject.Find("Text").GetComponent<Text>();
        text.text = "You have reached the end.";
        TrailRenderer trail = GameObject.Find("Trail").GetComponent<TrailRenderer>();
        Material mat = Resources.Load("Yellow") as Material;
        trail.material = mat;
        avgVelocity = totalDistance / totalTime;
        List<string> experimentInfo = GetExperimentInfo();
        List<string> experimentHeader = GetExperimentHeader();

        if (!File.Exists(_expInstance.FileName + "_data.txt"))
        {
            System.IO.File.WriteAllLines(_expInstance.FileName + "_data.txt", experimentHeader.ToArray());
            System.IO.File.AppendAllText(_expInstance.FileName + "_data.txt", "\r\n");
            System.IO.File.WriteAllLines(_expInstance.FileName + "_pathdata.txt", experimentHeader.ToArray());
            System.IO.File.AppendAllText(_expInstance.FileName + "_pathdata.txt", "\r\n");
            foreach (string line in experimentInfo)
            {
                System.IO.File.AppendAllText(_expInstance.FileName + "_data.txt", line + "\r\n");
                System.IO.File.AppendAllText(_expInstance.FileName + "_pathdata.txt", line + "\r\n");
            }
        }
        else
            foreach (string line in experimentInfo)
            {
                System.IO.File.AppendAllText(_expInstance.FileName + "_data.txt", line + "\r\n");
                System.IO.File.AppendAllText(_expInstance.FileName + "_pathdata.txt", line + "\r\n");
            }

        foreach (string line in path)
            System.IO.File.AppendAllText(_expInstance.FileName + "_pathdata.txt", line + "\r\n");

        TakePhoto();

        _expInstance.MazeSettings = new MazeSettings(); // hoping this line resets class defaults

        // It doesn't, can reset manually - but there's got to be a way to do that
        /*_expInstance.MazeSettings.Arrows = false;
		_expInstance.MazeSettings.Rotate = false;
		_expInstance.MazeSettings.Pause = false;
		_expInstance.MazeSettings.Reverse = false;
		_expInstance.MazeSettings.SingleIntro = false;
		_expInstance.MazeSettings.MultiIntro = false;*/

    }

    static private List<string> GetExperimentHeader()
    {
        List<string> experimentHeader = new List<string>();

        ExperimentSettings _expInstance = ExperimentSettings.GetInstance();
        experimentHeader.Add("Participant ID: " + _expInstance.ParticipantID);
        experimentHeader.Add("Experimenter Initials: " + _expInstance.ExperimenterInitials);
        experimentHeader.Add("Date: " + _expInstance.Date);
        experimentHeader.Add("Experiment Type: " + _expInstance.ExperimentType);
        return experimentHeader;
    }

    static private List<string> GetExperimentInfo()
    {
        List<string> experimentInfo = new List<string>();
        ExperimentSettings _expInstance = ExperimentSettings.GetInstance();


        experimentInfo.Add(_expInstance.MazeSettings.TrialName + _expInstance.TrialTracker[_expInstance.MazeSettings.TrialName].ToString());
        experimentInfo.Add("Maze: " + _expInstance.MazeSettings.MazeName.ToString());
        experimentInfo.Add("Trial Number: " + _expInstance.TrialTracker[_expInstance.MazeSettings.TrialName].ToString());
        experimentInfo.Add("Experiment Type: " + _expInstance.ExperimentType);


        if (!ExperimentSettings.IsPractice())
        {
            experimentInfo.Add("Arrows: " + _expInstance.MazeSettings.Arrows.ToString());
        }

        experimentInfo.Add("Distance: " + totalDistance);
        experimentInfo.Add("Time: " + totalTime);
        experimentInfo.Add("Avg. Velocity: " + avgVelocity);
        if (_expInstance.MazeSettings.ReachedEnd && _expInstance.MazeSettings.MazeName != MazeNameEnum.JP) experimentInfo.Add("Reached End: " + _expInstance.MazeSettings.ReachedEnd.ToString());
        else if (!_expInstance.MazeSettings.ReachedEnd && _expInstance.MazeSettings.MazeName != MazeNameEnum.JP) experimentInfo.Add("Reached End: False, manual exit");
        experimentInfo.Add("\r\n");
        
        return experimentInfo;
    }

    static private void TakePhoto()
    {
        ExperimentSettings _expInstance = ExperimentSettings.GetInstance();
        Camera cam = GameObject.Find("Overhead Cam").GetComponent<Camera>();
        RenderTexture currentRT = RenderTexture.active;
        var rTex = new RenderTexture(cam.pixelHeight, cam.pixelHeight, 16);
        cam.targetTexture = rTex;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D tex = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        tex.Apply(false);
        RenderTexture.active = currentRT;
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);
        string imageFileName = NameImageFile();
        System.IO.File.WriteAllBytes(imageFileName, bytes);
    }

    static private string NameImageFile()
    {
        ExperimentSettings _expInstance = ExperimentSettings.GetInstance();
        string imageFileName = _expInstance.FileName + "_" + _expInstance.MazeSettings.TrialName + _expInstance.TrialTracker[_expInstance.MazeSettings.TrialName].ToString() + "_";

        if (ExperimentSettings.IsAPC() ||
            ExperimentSettings.IsTSC())
        {
            if (_expInstance.MazeSettings.Arrows) imageFileName += "Arrows_";
        }

        imageFileName += "_path.png";

        return imageFileName;
    }
}