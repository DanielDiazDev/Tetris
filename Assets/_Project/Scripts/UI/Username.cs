using System;
using _Project.Scripts.SOAP.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class Username : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private StringEvent saveNameEvent;
        [SerializeField] private Button saveButton;

        private void Awake()
        {
            saveButton.onClick.AddListener(Save);
        }

        private void Save()
        {
            if(string.IsNullOrEmpty(nameInputField.text)) return;
            saveNameEvent.Raise(nameInputField.text);
            gameObject.SetActive(false);
        }
    }
}