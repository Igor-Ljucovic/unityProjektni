using Cinemachine;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private Transform MainMenu;



    public void Start()
    {
       virtualCamera.Follow = MainMenu;
    }
}
