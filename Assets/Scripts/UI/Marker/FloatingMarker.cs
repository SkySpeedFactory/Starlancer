using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingMarker : MonoBehaviour
{
    // https://www.youtube.com/watch?v=a-J4ktx6y0Y
    [SerializeField] private WaypointItem waypoint;
    [SerializeField] private TMP_Text targetNameTextfield;
    private int MarkerDistance;

    void Awake()
    {
        RemoveMarkerTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (waypoint.Target != null)
        {
            float minX = waypoint.MarkerImage.GetPixelAdjustedRect().width * 0.25f;
            float maxX = Screen.width - minX;

            float minY = waypoint.MarkerImage.GetPixelAdjustedRect().height * 0.25f;
            float maxY = Screen.height - minY;

            Vector2 pos = Camera.main.WorldToScreenPoint(waypoint.Target.position + waypoint.MarkerOffset);

            if (Vector3.Dot((waypoint.Target.position - transform.position), transform.forward) < 0)
            {
                // Behind player
                if (pos.x < Screen.width * 0.5)
                {
                    pos.x = maxX;

                }
                else
                {
                    pos.x = minX;
                }
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            waypoint.MarkerImage.transform.position = pos;
            waypoint.MarkerImage.color = waypoint.MarkerColor;
            //waypoint.TargetName.text = ((int)Vector3.Distance(waypoint.Target.position, transform.position)).ToString();
            //MarkerDistance = (int)Vector3.Distance(waypoint.Target.position, transform.position);
            
        }
    }

    public void SetMarkerTarget(WaypointItem selectedTaget)
    {
        waypoint.Target = selectedTaget.Target;
        if (waypoint.Target != null)
        {
            waypoint.MarkerImage.enabled = true;
            //waypoint.MarkerImage = selectedTaget.MarkerImage;
            targetNameTextfield.text = selectedTaget.TargetName;
            targetNameTextfield.color = selectedTaget.MarkerColor;
            waypoint.MarkerColor = selectedTaget.MarkerColor;
        }
    }

    public void RemoveMarkerTarget()
    {
        waypoint.Target = null;
        //waypoint.MarkerImage = null;
        waypoint.MarkerImage.enabled = false;
        targetNameTextfield.text = "";
        targetNameTextfield.color = Color.white;
        waypoint.MarkerColor = Color.white;
    }
}
