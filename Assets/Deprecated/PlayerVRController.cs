/*using UnityEngine;
using Avatar = Alteruna.Avatar;
using System.Collections;
using TrackedPoseDriver = UnityEngine.SpatialTracking.TrackedPoseDriver;


public class PlayerVRController : MonoBehaviour
{
    private Avatar avatar;
    [SerializeField] private Transform head;
    [SerializeField] private Camera camera;
    [SerializeField] private int playerSelfLayer;

    private void Awake()
    {
        avatar = GetComponent<Avatar>();
    }

    private void Start()
    {
        if(avatar.IsMe)
        {
            head.gameObject.layer = playerSelfLayer;
            foreach(Transform child in head)
            {
                child.gameObject.layer = playerSelfLayer;
            }
        }
        else
        {
            camera.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(!avatar.IsMe)
        {
            return;
        }

        head.localPosition = camera.transform.localPosition;
        head.rotation = camera.transform.rotation;
    }
}*/