using UnityEngine;

public class BeamBuilder : MonoBehaviour
{
    [SerializeField] private Sprite[] startSprite;
    [SerializeField] private Material[] middleSprite;
    [SerializeField] private Sprite[] endSprite;
    [SerializeField] private LayerMask mask;
    [SerializeField] private bool stopOnCollision;
    private Transform end;
    private float startLength;
    private float endLength;
    private float middleLength;
    private LineRenderer line;
    private MeshCollider collider;
    public float Length { get; set; }

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
        if (middleSprite.Length > 0)
        {
            line.material = middleSprite[0];
        }
        if (endSprite.Length > 0)
        {
            endRenderer.sprite = endSprite[0];
        }

        // Set collider sizes
        start.GetComponent<BoxCollider2D>().size = startRenderer.size;
        end.GetComponent<BoxCollider2D>().size = endRenderer.size;

        // Set length of each section
        endLength = endRenderer.size.x;
        startLength = startRenderer.size.x;
        line.SetPosition(0, new Vector3(startLength, 0, 0));

        UpdateStart();

        // Not stopping on collision will make a fixed length beam
        if (!stopOnCollision)
        {
            middleLength = Length - (endLength + startLength);
            line.SetPosition(1, new Vector3(startLength, middleLength, 0));
            UpdateEnd();
        }

        void UpdateStart() => start.localPosition = new Vector2(startLength * 0.5f, 0);
    }

    void Update()
    {
        if (!stopOnCollision)
        {
            return;
        }
        UpdateMiddle();
        UpdateEnd();
    }

    public void GenerateMeshCollider()
    {
        if (collider == null)
        {
            collider = transform.GetChild(1).gameObject.AddComponent<MeshCollider>();
        }

        Mesh mesh = new();
        line.BakeMesh(mesh);
        collider.sharedMesh = mesh;
    }

    private void UpdateMiddle()
    {
        middleLength = GetMiddleWidth();
        line.SetPosition(1, new Vector3(startLength + middleLength, 0, 0));
        if (middleLength > 0)
        {
            GenerateMeshCollider();
        }

        float GetMiddleWidth()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Length - endLength, mask);
            if (hit.collider == null)
            {
                return Length - (endLength + startLength);
            }
            return hit.distance < startLength ? 0 : hit.distance - startLength;
        }
    }

    private void UpdateEnd()
    {
        end.localPosition = new Vector2(startLength + middleLength + (endLength * 0.5f), 0);
    }
}
