using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This file should store variables that are immutable
public static class Constants {

	public static float GRIDSIZE = 1f;
	public static float MOVEFACTOR = 1f;

	public static float MOVESPEED = 15f; //Move lerp speed of all (non-rotating) objects

	public static float MOVE_COOLDOWN = 0f;

	public static int PLAYER_STARTING_HEALTH = 3;
	public static int PLAYER_STARTING_DAMAGE = 1;

	public static int BASIC_ENEMY_HEALTH = 3;
	public static int BASIC_ENEMY_DAMAGE = 1;
	public static int BASIC_ENEMY_SPAWNCHANCE = 100;


	public static int DRAGON_ENEMY_HEALTH = 5;
	public static int DRAGON_ENEMY_DAMAGE = 2;
	public static int DRAGON_ENEMY_RANGE = 3;
	public static int DRAGON_ENEMY_SPAWNCHANCE = 15;

	public static int WALL_DAMAGE = 1;

	//MAP GENERATION VALUES
	public static int START_ROOM_SIZE = 2;
	public static int END_ROOM_SIZE = 1;

	public static float MID_GOLD_CHANCE = 0.15f;
	public static float HIGH_GOLD_CHANCE = 0.05f;


	public static float VAMPIRIC_SINGLE_CHANCE = 0.10f;
	public static float VAMPIRIC_DOUBLE_CHANCE = 0.25f;
}
