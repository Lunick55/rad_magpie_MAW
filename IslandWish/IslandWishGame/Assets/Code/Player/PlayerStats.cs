using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
	public int playerNumber;
	public int health;
	public int spearDamage, slingDamage;
	public float speed;
	public float slingSpeed, slingDuration;
	public int slingMaxAmmo;
	public float iFrameDuration;
	public float iFrameFlashDuration;
}
