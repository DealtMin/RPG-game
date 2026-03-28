using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnemySaveData
{
    public string Type; 
    public Vector3 Position;
    public float CurrentHp;
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

    
    public List<EnemySaveData> Enemies = new List<EnemySaveData>();
    public List<ProjectileSaveData> Projectiles = new List<ProjectileSaveData>();
}
