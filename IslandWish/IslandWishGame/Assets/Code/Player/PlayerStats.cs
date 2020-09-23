using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
	[Header("Player Stats")]
	public int playerNumber;
	public int health;
	public float iFrameDuration;
	public float iFrameFlashDuration;
	public float pauseTime;
	public float pauseDuration;
	public float speed;
	[Header("Equipment Stats")]
	public int spearDamage, slingDamage;
	public int shieldMaxHealth = 100;
	public float shieldRechargeRate;
	public int slingMaxAmmo;
	public float slingSpeed, slingDuration;
}
