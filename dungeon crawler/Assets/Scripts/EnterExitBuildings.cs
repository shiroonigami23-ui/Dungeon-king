using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterExitBuildings : MonoBehaviour
{
    // CAMERAS
    public GameObject BSCam;
    public GameObject MainCam;

    // PLAYER
    public GameObject player;

    // POSITIONS
    public Transform BSPos;
    public Transform TownPos;

    // SCRIPT REFERENCES
    public CameraFollow cameraScript;

    // TRANSITIONS
    [Header("Transition Effects")]
    public Animator crossFade;

    // Start is called before the first frame update
    void Start()
    {
        BSCam.SetActive(false);
        MainCam.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Teleport
            StartCoroutine(EnterBuilding(true));
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            // Teleport
            StartCoroutine(EnterBuilding(false));
        }
    }

    IEnumerator EnterBuilding(bool exit)
    {
        crossFade.SetTrigger("End");

        yield return new WaitForSeconds(1);

        if (exit == true)
        {
            player.transform.position = BSPos.position;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
            BSCam.SetActive(true);
            MainCam.SetActive(false);
        }
        else
        {
            player.transform.position = TownPos.position;
            BSCam.SetActive(false);
            MainCam.SetActive(true);
            cameraScript.posToPlayer();
        }

        crossFade.SetTrigger("Start");
    }
}
