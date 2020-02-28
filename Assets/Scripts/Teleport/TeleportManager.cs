using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportManager : MonoBehaviour
{
    // Player
    private GameObject player;
    private Transform playerTransform;


    // Cam (player)
    private GameObject cam;

    // Fading
    private Image fadeImg;
    public float FadeRate;
    public float targetAlpha;

    // SFX
    public AudioManager AudioManager;
    public AudioClip TeleportStartClip;
    public AudioClip TeleportEndClip;

    // Building 
    BuildingManager BuildingManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        playerTransform = player.transform;
        AudioManager = AudioManager.Instance;
        BuildingManager = BuildingManager.Instance;

        // Init fade image
        fadeImg = GameObject.FindGameObjectWithTag("FadeImage").GetComponent<Image>();
        if (fadeImg == null)
        {
            Debug.LogError("Error: No image on " + fadeImg.name);
        }
        targetAlpha = fadeImg.color.a;
        
        // Star game fade
        StartCoroutine(Fade.FadeTo(0.0f, 1.0f, fadeImg));
    }
    
    void Update()
    {
        GetTeleportDest();
    }

    private void GetTeleportDest()
    {
        if (Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Fire3"))
        {
            if(Physics.Raycast(playerTransform.position, cam.transform.forward, out RaycastHit hit))
            {
                for (int i = 1; i <= 6; i++)
                {
                    if (hit.collider.gameObject.name == ("Building_" + i))
                    {
                        StartCoroutine(TeleportTo(hit));
                    }
                    else
                    {
                        if (hit.collider.gameObject.name == "HighriseBuilding")
                        {
                            StartCoroutine(TeleportTo(hit));
                        }
                    }
                }
            }
        }
    }

    private IEnumerator TeleportTo(RaycastHit hit)
    {
        // Fade out, play teleport start SFX
        AudioManager.TeleportStart.Play();
        yield return StartCoroutine(Fade.FadeTo(1.0f, 0.5f, fadeImg));

        // Fade in, play teleport start SFX
        playerTransform.position = new Vector3(hit.collider.bounds.center.x, hit.collider.bounds.max.y + 12, hit.collider.bounds.center.z);
        AudioManager.TeleportEnd.Play();
        yield return StartCoroutine(Fade.FadeTo(0.0f, 0.5f, fadeImg));

        //yield return StartCoroutine(SlerpCamera());

        // Work around for player camera weird rotation after teleport.
        playerTransform.eulerAngles = new Vector3(0, playerTransform.rotation.eulerAngles.y, 0);

        // Reset forces acting on player
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        yield break;
    }

    public IEnumerator SlerpCamera()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        Quaternion initialRotation = cam.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(BuildingManager.GetMainBuilding().GetComponent<MeshRenderer>().bounds.center - cam.transform.position);
        for(float t = 0.0f; t < 1.0f; t += Time.deltaTime)
        {
            if(initialRotation != Quaternion.identity && targetRotation != Quaternion.identity)
                playerTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            playerTransform.eulerAngles = new Vector3(0, playerTransform.rotation.eulerAngles.y, 0);
            yield return null;
        }
        yield return null;
    }
}
