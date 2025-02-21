using UnityEngine;
using UnityEngine.InputSystem;

public class SliceStaker : MonoBehaviour
{
    public LayerMask SliceLayer;
    public GameObject SelectedSlice;
    public void CheckSliceHit(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit obj, 100, SliceLayer))
            {
                if(obj.collider.gameObject.name != "Bread")
                SelectedSlice = obj.collider.gameObject;
            }
        }
    }

    private Vector2 GetNearestCardinal(Vector2 vector)
    {
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        float roundedAngle = Mathf.Round(angle / 90f) * 90f;
        float rad = roundedAngle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    private GameObject Raycast4Slice(Vector2 direction)
    {
        GameObject slcFound = null;
        if(Physics.Raycast(SelectedSlice.transform.position, new Vector3(direction.x, 0, direction.y), out RaycastHit hitInfo, SelectedSlice.transform.localScale.z, SliceLayer))
        {
            slcFound = hitInfo.collider.gameObject;
        }
        return slcFound;
    }

    private void SetStack(GameObject foundSlice)
    {
        if(foundSlice.transform.parent != null)
        {
            SelectedSlice.transform.parent = foundSlice.transform.parent;
        }
        else
        {
            GameObject parent = Instantiate(new GameObject());
            parent.name = "Stack";
            SelectedSlice.transform.parent = parent.transform;
            foundSlice.transform.parent = parent.transform;
        }
    }

    private bool isDragging;
    private Vector2 startPosition;
    [SerializeField] private float dragThreshold = 5f;
    public void OnDrag(InputAction.CallbackContext context)
    {
        if(context.action.name != "Drag" || SelectedSlice == null)
        return;

        if(context.phase == InputActionPhase.Started)
        {
            startPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            isDragging = true;
        }

        else if(context.phase == InputActionPhase.Canceled)
        {
            if(isDragging)
            {
                Vector2 endPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 delta = endPosition - startPosition;
                
                if(delta.magnitude >= dragThreshold)
                {
                    Vector2 swipeDirection = GetNearestCardinal(delta);
                    //Debug.Log("Direzione cardinale: " + swipeDirection);
                    //Debug.Log($"Selected : {Raycast4Slice(swipeDirection)} at position : {Raycast4Slice(swipeDirection).transform.position}");
                    GameObject hitObj = Raycast4Slice(swipeDirection);
                    if(hitObj != null)
                    {
                        Transform trnf = hitObj.transform;
                        SelectedSlice.transform.position = new Vector3(trnf.position.x, trnf.position.y + trnf.localScale.y, trnf.position.z);
                        //SetStack(hitObj);
                    }
                }

                isDragging = false;
            }
        }

        //Debug.Log($"Drag status: {isDragging}");
    }

}
