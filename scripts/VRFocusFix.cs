using UnityEngine;

public class VRFocusFix : MonoBehaviour
{
    void Start()
    {
        OVRManager.display.RecenterPose(); // Ensures the headset is properly centered
    }
}