using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private WindowLevelUp _prefabWindowLevelUp;

    public uint PlayerLevel { get; private set; }

    private void Awake()
    {
        PlayerLevel = (uint)PlayerPrefs.GetInt("PlayerLevel", 1);
    }

    public void LevelUp()
    {
        PlayerLevel++;
        PlayerPrefs.SetInt("PlayerLevel", (int)PlayerLevel);
        Debug.Log($"PlayerLevelManager: LevelUp: Level increased to {PlayerLevel}");
        
        WindowLevelUp windowLevelUp = Instantiate(_prefabWindowLevelUp, _canvas.transform);
        windowLevelUp.SetLevel(PlayerLevel);
        windowLevelUp.ShowWindow();
    }
}

