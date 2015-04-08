﻿using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maxmum;

		public Count(int min, int max) 
		{
			minimum = min;
			maxmum = max;
		}
	}

	public int columns = 0;
	public int rows = 0;
	//8 x 8 game board
	public Count wallCount = new Count(5, 9);
	public Count foodCount = new Count(1, 5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	private Transform boardHolder;
	//??? 

	private List<Vector3> gridPositions = new List<Vector3>();

	void InitialiseList() 
	{
		gridPositions.Clear();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	void BoardSetup () 
	{
		//outwall, floor, gameboard

		//board
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];

				//check outwall
				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				}

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity as GameObject);
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	Vector3 RandomPosition()
	{
		int RandomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [RandomIndex];
		gridPositions.RemoveAt (RandomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maxmum)
	{
		int objectCount = Random.Range (minimum, maxmum + 1);
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void setupScene (int level)
	{
		BoardSetup ();
		InitialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maxmum);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maxmum);
		int enemyCount = (intMathf.Log(level, 2f));
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}
}
