using System.Linq;
using UnityEngine;

public class BeamBuilder : MonoBehaviour
{
    [SerializeField] private float maxWidth;
    [SerializeField] private Sprite[] startSprite;
    [SerializeField] private Sprite[] middleSprite;
    [SerializeField] private Sprite[] endSprite;
    [SerializeField] private LayerMask mask;
    private Transform middle;
    private Transform end;
    private SpriteRenderer middleRenderer;
    private BoxCollider2D middleCollider;
    private float startWidth;
    private float endWidth;
    private float middleHeight;
    private float middleWidth;

    void Start()
    {
        // Get transforms
        Transform start = transform.GetChild(0);
        middle = transform.GetChild(1);
        end = transform.GetChild(2);

        // Get sprite renderer for each section
        SpriteRenderer startRenderer = start.GetComponent<SpriteRenderer>();
        middleRenderer = middle.GetComponent<SpriteRenderer>();
        SpriteRenderer endRenderer = end.GetComponent<SpriteRenderer>();

        // Set sprites
        if (startSprite.Length > 0)
        {
            startRenderer.sprite = startSprite[0];
        }
        middleRenderer.sprite = middleSprite[0];
        if (endSprite.Length > 0)
        {
            endRenderer.sprite = endSprite[0];
        }

        // Set collider sizes
        start.GetComponent<BoxCollider2D>().size = startRenderer.size;
        middleCollider = middle.GetComponent<BoxCollider2D>();
        middleCollider.size = middleRenderer.size;
        end.GetComponent<BoxCollider2D>().size = endRenderer.size;

        // Set length of each section
        middleHeight = middleRenderer.size.y;
        endWidth = endRenderer.size.x;
        startWidth = startRenderer.size.x;

        UpdateStart();
        UpdateMiddle();
        UpdateEnd();

        void UpdateStart() => start.localPosition = new Vector2(startWidth * 0.5f, 0);
    }

    void Update()
    {
        UpdateMiddle();
        if (middleSprite.Length > 1)
        {
            int randomNum = Random.Range(0, middleSprite.Length);
            middleRenderer.sprite = middleSprite[randomNum];
        }
        UpdateEnd();
    }

    private void UpdateMiddle()
    {
        middleWidth = GetMiddleWidth();
        middleRenderer.size = new Vector2(middleWidth, middleHeight);
        middleCollider.size = middleRenderer.size;
        middle.localPosition = new Vector2((middleWidth * 0.5f) + startWidth, 0);

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
