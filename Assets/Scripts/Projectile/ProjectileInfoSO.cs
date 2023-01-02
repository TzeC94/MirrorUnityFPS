using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Info SO", menuName = "Projectile/Projectile Info SO")]
public class ProjectileInfoSO : ScriptableObject
{
    public enum ProjectileType { raycast, spawn }

    public ProjectileType type = ProjectileType.raycast;

    [Header("For Raycast Type")]
    public float distance;
    public LayerMask layerHit;

    [Header("For Projectile Spawn Type")]
    public GameObject projectilePrefab;

    [Header("Damage")]
    public ProjectileDamageSO damageSO;
}