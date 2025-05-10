using UnityEngine;
using UnityEngine.XR.Management;

public class XRRigInitializer : MonoBehaviour
{
    private GameObject xrRig;

    void Start()
    {
        xrRig = GameObject.FindWithTag("XRRig");
        InitializeXRRig();
    }

    void InitializeXRRig()
    {
        // Reset XR Rig position and rotation to match new scene
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
    }
}
