using UnityEngine;

public class BeamBuilder : MonoBehaviour
{
    [SerializeField] private float maxWidth;
    [SerializeField] private Sprite[] startSprite;
    [SerializeField] private Material[] middleSprite;
    [SerializeField] private Sprite[] endSprite;
    [SerializeField] private LayerMask mask;
    private Transform end;
    private float startWidth;
    private float endWidth;
    private float middleWidth;
    private LineRenderer line;
    private MeshCollider collider;

    void Start()
    {
        collider = GetComponentInChildren<MeshCollider>();

        // Get transforms
        Transform start = transform.GetChild(0);
        end = transform.GetChild(2);

        // Get sprite renderer for each section
        SpriteRenderer startRenderer = start.GetComponent<SpriteRenderer>();
        SpriteRenderer endRenderer = end.GetComponent<SpriteRenderer>();

        line = GetComponentInChildren<LineRenderer>();

        // Set sprites
        if (startSprite.Length > 0)
        {
            startRenderer.sprite = startSprite[0];
        }
        line.material = middleSprite[0];
        if (endSprite.Length > 0)
        {
            endRenderer.sprite = endSprite[0];
        }

        // Set collider sizes
        start.GetComponent<BoxCollider2D>().size = startRenderer.size;
        end.GetComponent<BoxCollider2D>().size = endRenderer.size;

        // Set length of each section
        endWidth = endRenderer.size.x;
        startWidth = startRenderer.size.x;
        line.SetPosition(0, new Vector3(startWidth, 0, 0));

        UpdateStart();

        void UpdateStart() => start.localPosition = new Vector2(startWidth * 0.5f, 0);
    }

    void Update()
    {
        UpdateMiddle();
        if (middleSprite.Length > 1)
        {
            int randomNum = Random.Range(0, middleSprite.Length);
        }
        UpdateEnd();
    }

    public void GenerateMeshCollider()
    {
        if (collider == null)
        {
            collider = transform.GetChild(1).gameObject.AddComponent<MeshCollider>();
        }

        Mesh mesh = new();
        line.BakeMesh(mesh, true);
        collider.sharedMesh = mesh;
    }

    private void UpdateMiddle()
    {
        middleWidth = GetMiddleWidth();
        line.SetPosition(1, new Vector3(startWidth + middleWidth, 0, 0));
        if (middleWidth > 0)
        {
            GenerateMeshCollider();
        }

        float GetMiddleWidth()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxWidth - endWidth, mask);
            if (hit.collider == null)
            {
                return maxWidth - (endWidth + startWidth);
            }
            return hit.distance < startWidth ? 0 : hit.distance - startWidth;
        }
    }

    private void UpdateEnd()
    {
        end.localPosition = new Vector2(startWidth + middleWidth + (endWidth * 0.5f), 0);
    }
}
