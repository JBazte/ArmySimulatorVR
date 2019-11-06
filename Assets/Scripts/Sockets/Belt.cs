
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class Belt : MonoBehaviour
{
    [Range(0.5f, 0.75f)]
    public float height;

    private Transform head = null;

    private void Start()
    {
        head = SteamVR_Render.Top().head;

    }

    private void Update()
    {
        if (head)
        {
            PositionUnderHead();
            RotateWithHead();
        }
    }

    private void PositionUnderHead()
    {
        Vector3 adjustableHeight = head.localPosition;
        adjustableHeight.y = Mathf.Lerp(0f, adjustableHeight.y, height);
    }

    private void RotateWithHead()
    {
        Vector3 adjustableRotation = head.localEulerAngles;
        adjustableRotation.x = 0;
        adjustableRotation.z = 0;

        transform.localEulerAngles = adjustableRotation;
    }
}
