using UnityEngine;

[RequireComponent (typeof(CircleCollider2D))]
public class LoomPeg : MonoBehaviour
{
    private CircleCollider2D col;

    [SerializeField] private float pegRadius;
    [SerializeField] private int index;

    public bool IsActive { get; private set; }
    public bool InUse { get; private set; }
    public int Index { get => index; }

    public void SetPegUse(bool _state) => InUse = _state;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        col.radius = pegRadius;
    }
}
