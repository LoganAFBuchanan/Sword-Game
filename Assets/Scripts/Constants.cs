using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This file should store variables that are immutable
public static class Constants {

	public static float GRIDSIZE = 1f;
	public static float MOVEFACTOR = 1f;

	public static float MOVESPEED = 20f; //Move lerp speed of all (non-rotating) objects

	public static float MOVE_COOLDOWN = 0f;

	public static int PLAYER_STARTING_HEALTH = 3;
	public static int PLAYER_STARTING_DAMAGE = 1;

	public static int BASIC_ENEMY_HEALTH = 2000;
	public static int BASIC_ENEMY_DAMAGE = 1;

	public static int WALL_DAMAGE = 1;

	//MAP GENERATION VALUES
	public static int START_ROOM_SIZE = 1;
}
