using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects.UI.Test
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button _serverBtn;
        [SerializeField] private Button _hostBtn;
        [SerializeField] private Button _clientBtn;

        private void Awake()
        {
            _serverBtn.onClick.AddListener((() =>
            {
                NetworkManager.Singleton.StartServer();
                HideMenu();
            }));
            _hostBtn.onClick.AddListener((() =>
            {
                NetworkManager.Singleton.StartHost();
                HideMenu();
            }));
            _clientBtn.onClick.AddListener((() =>
            {
                NetworkManager.Singleton.StartClient();
                HideMenu();
            }));
        }

        private void HideMenu()
        {
            gameObject.SetActive(false);
        }

        public void ShowMenu()
        {
            gameObject.SetActive(true);
        }
    }
}