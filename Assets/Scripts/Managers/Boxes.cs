using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    [SerializeField] private Transform _boxesParent;
    [SerializeField] private MoneyBoxUI _moneyBoxPrefab;

    private List<MoneyBox> _boxes;

    public void InitializeBoxes()
    {
        if (_boxes == null)
        {
            _boxes = new List<MoneyBox>();
            return;
        }

        foreach (var box in _boxes)
        {
            var newBox = Instantiate(_moneyBoxPrefab, _boxesParent);
            newBox.InitializeVisualization(box);
        }
    }

   public void AddNewBox(MoneyBox box)
    {
        _boxes.Add(box);

        var newBox = Instantiate(_moneyBoxPrefab, _boxesParent);
        newBox.InitializeVisualization(box);
    }
}
