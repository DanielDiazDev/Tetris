using System;
using _Project.Scripts.SOAP.Events;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private IntEvent setLevelEvent;
    private void OnEnable()
    {
        setLevelEvent.Subscribe(SetLevel);
    }

    private void OnDestroy()
    {
        setLevelEvent.Unsubscribe(SetLevel);
    }

    private void Awake()
    {
        SetLevel(1);
    }

    private void SetLevel(int level)
    {
        levelText.text = level.ToString();
    }
}