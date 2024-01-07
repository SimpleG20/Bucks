using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    [SerializeField] private Transform _boxesParent;
    [SerializeField] private MoneyBoxUI _moneyBoxPrefab;

    private List<MoneyBox> _boxes;

    public void UpdateBoxes()
    {
        _boxes = User.Instance.BoxesList;

        _boxesParent.DeleteChildren();

        foreach (var box in _boxes)
        {
            var newBox = Instantiate(_moneyBoxPrefab, _boxesParent);
            newBox.InitializeVisualization(box);
        }
    }
}
