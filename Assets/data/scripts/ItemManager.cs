using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Transform handPosition;
    public Button actionButton;
    Rigidbody itemRigidbody;
    GameObject itemInHand;

    void Start()
    {
        actionButton.gameObject.SetActive(false);
        actionButton.onClick.AddListener(ThrowItem);
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && itemInHand == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Item"))
                MoveToHand(hit.collider.gameObject);
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0 && itemInHand == null)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Item"))
                MoveToHand(hit.collider.gameObject);
        }
    }

    void MoveToHand(GameObject item)
    {
        if (itemInHand != null) return;

        itemInHand = item;
        itemRigidbody = item.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
            itemRigidbody.isKinematic = true;
        item.transform.position = handPosition.position;
        item.transform.SetParent(handPosition);

        actionButton.gameObject.SetActive(true);
    }

    public void ThrowItem()
    {
        if (itemInHand != null)
        {
            itemRigidbody.isKinematic = false;
            itemInHand.transform.SetParent(null);
            itemRigidbody.AddForce(transform.forward * 10f, ForceMode.Impulse);
            itemInHand = null;
            actionButton.gameObject.SetActive(false);
        }
    }
}
