using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CornerCollision : MonoBehaviour
{

    public int cornerNum;
    public GameObject cornerArrows;

    private void OnTriggerEnter(Collider other)
    {
        ExperimentSettings _expInstance = ExperimentSettings.GetInstance();
        if (ExperimentSettings.IsTSC())
        {
            Debug.Log(gameObject.transform.position);
            MazeController.cornerTransform = gameObject.transform;
            gameObject.SetActive(false);
            //Debug.Log(gameObject.ToString());
            MazeController.onCorner = cornerNum;
            GameObject landmarkList = GameObject.Find("LandmarkList");
            Transform landmarkNameTransform = landmarkList.transform.GetChild(cornerNum);
            if (cornerNum > 0)
            {
                Transform lastLandmarkNameTransform = landmarkList.transform.GetChild(cornerNum - 1);
                Text lastLandmarkNameText = lastLandmarkNameTransform.GetComponent<Text>();
                lastLandmarkNameText.fontSize = 14;
                lastLandmarkNameText.fontStyle = FontStyle.Normal;
                lastLandmarkNameText.color = Color.white;
            }
            Text landmarkNameText = landmarkNameTransform.GetComponent<Text>();
            landmarkNameText.fontSize = 20;
            landmarkNameText.fontStyle = FontStyle.Bold;
            landmarkNameText.color = Color.red;
        }
        if (ExperimentSettings.IsAPC())
        {
            cornerArrows.SetActive(true);
            GameObject previousTurn = GameObject.Find("Turn" + (cornerNum - 1).ToString() + "Arrows");
            if (previousTurn != null) previousTurn.SetActive(false);
            else
            {
                GameObject item = GameObject.Find("Turn" + (cornerNum + 1).ToString() + "Arrows");
                if (item != null) { item.SetActive(false); };
            }
        }
    }


}
