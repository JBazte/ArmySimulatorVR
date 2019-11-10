
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class Belt : MonoBehaviour
{
    [Range(0.5f, 1f)]
    public float height;

    private Transform head = null;

    private void Start()
    {
        //head = SteamVR_Render.Top().head;
        head = Player.instance.hmdTransform;
        // if (head == null)

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
        Vector3 adjustableHeight = head.position;
        adjustableHeight.y = Mathf.Lerp(0f, adjustableHeight.y, height);
        transform.position = adjustableHeight;
    }

    private void RotateWithHead()
    {
        Vector3 adjustableRotation = head.localEulerAngles;
        adjustableRotation.x = 0;
        adjustableRotation.z = 0;

        transform.localEulerAngles = adjustableRotation;
    }
}
