using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
	public string enemyName;
	public int health, attack, defense;
	public float attackRange, moveSpeed;

}
