
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class Belt : MonoBehaviour
{
    [Range(0.5f, 1f)]
    public float height;
    public static Belt instance;

    private Transform head = null;

    private void Start()
    {
        //head = SteamVR_Render.Top().head;
        head = Player.instance.hmdTransform;
        instance = this;
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

    public void ConfigureBelt(BeltPrefabs beltPrefab)
    {
        DeleteSlots();
        if (beltPrefab.spawnObject == null)
        {
            GetComponentInChildren<SlotSpawner>().gameObject.SetActive(false);
        }
        else
        {
            var gb = GetComponentInChildren<SlotSpawner>();
            gb.gameObject.SetActive(true);
            gb.ChangeSpawn(beltPrefab.spawnObject);
        }
        var slots = GetComponentsInChildren<Slot>();

        //if (beltPrefab.leftObject != null)
        //{
        //  slots[0].TryStore(beltPrefab.leftObject);
        //}
        if (beltPrefab.rightObject != null)
        {
            //deleteObj = 
            slots[1].TryStore(Instantiate(beltPrefab.rightObject));
        }

    }

    public void DeleteSlots()
    {
        var slots = GetComponentsInChildren<Slot>();
        slots[0].DestroyAttachedObject();
        slots[1].DestroyAttachedObject();
    }

    [System.Serializable]
    public struct BeltPrefabs
    {
        public GrabbableObject spawnObject;
        public GrabbableObject leftObject;
        public GrabbableObject rightObject;


        public BeltPrefabs(GrabbableObject spawnObject = null, GrabbableObject leftObject = null, GrabbableObject rightObject = null)
        {
            this.spawnObject = spawnObject;
            this.leftObject = leftObject;
            this.rightObject = rightObject;
        }
    }

}

