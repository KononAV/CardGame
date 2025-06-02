using System.Collections;
using UnityEngine;

public class SelectionMenuCard : MonoBehaviour, ICard
{
    [SerializeField] private Material _material;
    public Material Material
    {
        get { return _material; }
        set { _material = value; }
    }

    [SerializeField] private float rotationSpeed = 0.3f;

    private bool isDragging = false;
    private Vector2 lastPos;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

   

    void Update()
    {
        if (!isDragging) { transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * 5f, Space.World); }



#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
            TryStartDrag(Input.mousePosition);

        if (Input.GetMouseButton(0) && isDragging)
            Drag(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
            isDragging = false;


#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                TryStartDrag(touch.position);

            if (touch.phase == TouchPhase.Moved && isDragging)
                Drag(touch.position);

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                isDragging = false;

        }
#endif
    }

    private void TryStartDrag(in Vector2 screenPos)
    {
        //StopAllCoroutines();
        Ray ray = mainCam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                isDragging = true;
                lastPos = screenPos;
            }
        }
    }

    private void Drag(in Vector2 currentPos)
    {
        float deltaX = currentPos.x - lastPos.x;
        transform.Rotate(Vector3.up, -deltaX * rotationSpeed, Space.World);
        lastPos = currentPos;
        
    }


public void ChangeMaterial(int id, in Texture2D texture) =>
        GetComponent<Renderer>().material.SetTexture("_BaseMap", texture);
}
