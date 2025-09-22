using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LoomController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputReader input;
    private LineRenderer lineRenderer;

    [Header("Peg Parameters")]
    [SerializeField] List<LoomPeg> selectedPegs;
    [SerializeField] float pegDetectRadius;
    [SerializeField] LayerMask pegLayer;

    [Header("Input")]
    private Vector2 pointerPosition;
    private bool isDragging;

    //Events
    static public event Action<string> InputCompleted;

    private void OnEnable()
    {
        input.PositionEvent += HandlePosition;
        input.ClickEvent += HandleClick;
    }

    private void OnDisable()
    {
        input.PositionEvent -= HandlePosition;
        input.ClickEvent -= HandleClick;
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        isDragging = false;
    }

    private void Update()
    {
        if (isDragging) DetectPegs();
    }

    private void DetectPegs()
    {
        UpdateMouseLine();

        Collider2D _col = Physics2D.OverlapCircle(pointerPosition, pegDetectRadius, pegLayer);

        if(_col == null) return;
        if (!_col.TryGetComponent<LoomPeg>(out var _peg))
            throw new Exception("An item on the peg layer doesnt have the LoomPeg class");
        if (_peg.InUse == true) return;

        selectedPegs.Add(_peg);
        _peg.SetPegUse(true);
        UpdateLoomLine();
    }

    private void UpdateLoomLine()
    {
        lineRenderer.positionCount = selectedPegs.Count + 1;
        for (int i = 0; i < selectedPegs.Count; i++)
        {
            var p = selectedPegs[i];
            lineRenderer.SetPosition(i, p.transform.position);
        }
    }

    private void UpdateMouseLine()
    {
        if (lineRenderer.positionCount == 0) return;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, pointerPosition);
    }

    private void HandlePosition(Vector2 _position)
    {
        pointerPosition = _position;
    }

    private void HandleClick(bool _isClicking)
    {
        if (!_isClicking) OnDragEnd();
        else if (!isDragging) OnDragStart();
    }

    private void OnDragStart()
    {
        isDragging = true;
    }

    private void OnDragEnd()
    {
        isDragging = false;
        string _input = "";

        foreach (var _peg in selectedPegs)
        {
            _peg.SetPegUse(false);
            _input += _peg.Index.ToString();
        }

        InputCompleted?.Invoke(_input);
        selectedPegs.Clear();
        lineRenderer.positionCount = 0;
    }
}
