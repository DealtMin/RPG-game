using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemySaveData
{
    public string Type; 
    public Vector3 Position;
    public Quaternion Rotation;
    public float CurrentHp;
    public int WeaponIndex=-1;
}
[System.Serializable]
public class ProjectileSaveData
{
    public string Type;
    public Vector3 Position;
    public Vector3 Direction;
}
[System.Serializable]
public class PlayerData
{
    
    public float Hp;
    public float MaxHp;
    public Vector3 Position;
    public Quaternion Rotation;

    public int Score;
    public int KillCount;
    public bool BossSpawned;

    
    public List<EnemySaveData> Enemies = new List<EnemySaveData>();
    public List<ProjectileSaveData> Projectiles = new List<ProjectileSaveData>();
}
