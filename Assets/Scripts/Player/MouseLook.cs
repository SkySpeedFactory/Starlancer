using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private Vector2 mouseLook;
    private Vector2 smoothV;

    private readonly float sensitivity = 2.0F;
    private readonly float smoothing = 2.0F;

    private readonly float minRot = -70.0F;
    private readonly float maxRot = +70.0F;

    private float turnSpeed = 600f;

    GameObject character;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.Rotate(new Vector3(0, 0, 0));
    }

    void Update()
    {
        if (character != null)
        {
            LookDirection();
        }
    }

    private void LookDirection()
    {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1F / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1F / smoothing);
        mouseLook += smoothV;

        character.transform.localRotation = Quaternion.Euler(mouseLook.y, mouseLook.x, character.transform.rotation.eulerAngles.z);
    }

    public void SetCharacter(GameObject ship)
    {
        character = ship;
    }
}
