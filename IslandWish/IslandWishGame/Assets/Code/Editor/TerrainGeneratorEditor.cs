using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainGeneratorEditor : EditorWindow
{
	List<GameObject> floorCubes = new List<GameObject>(), towerCubes;

	Transform floorParent;

	int floorX, floorY, maxTowerHeight;

	[MenuItem("Scooby/Terrain Generator")]
	public static void ShowWindow()
	{
		GetWindow(typeof(TerrainGeneratorEditor));
	}

	void OnGUI()
	{
		CreateObjectListDisplay(floorCubes, "Floor Objects", "Floor");
		CreateObjectListDisplay(towerCubes, "Tower Objects", "Tower");

		floorX = EditorGUILayout.IntField("Floor X", floorX);
		floorY = EditorGUILayout.IntField("Floor Y", floorY);
		maxTowerHeight = EditorGUILayout.IntField("Max Tower Height", maxTowerHeight);

		if (GUILayout.Button("Create Floor"))
		{
			CreateFloor();
		}
		floorParent = (Transform)EditorGUILayout.ObjectField("FloorParent", floorParent, typeof(Transform), true);
		if (GUILayout.Button("Update Floor"))
		{
			UpdateFloor();
		}
		if (GUILayout.Button("Create Towers"))
		{
			CreateTowers();
		}

	}

	void CreateFloor()
	{
		if(floorCubes.Count > 0)
		{
			for (int i = 0; i < floorX; i++)
			{
				for (int j = 0; j < floorY; j++)
				{
					int randFloorIndex = Random.Range(0, floorCubes.Count);

					Vector3 newFloorPos = new Vector3(i, 0, j);

					Instantiate(floorCubes[randFloorIndex], newFloorPos, Quaternion.identity);
				}
			}
		}
	}

	void UpdateFloor()
	{
		if (floorParent != null)
		{
			for (int i = 0; i < floorParent.childCount; i++)
			{
				Transform floorChild = floorParent.GetChild(i);

				if (!floorChild.gameObject.GetComponent<BoxCollider>())
				{
					floorChild.gameObject.AddComponent<BoxCollider>();
				}
			}
		}
	}

	void CreateTowers()
	{
		if (towerCubes.Count > 0)
		{
			for (int i = 0; i < floorX; i++)
			{
				for (int j = 0; j < floorY; j++)
				{
					int randTowerIndex = Random.Range(0, towerCubes.Count);
					int randTowerHeight = Random.Range(0, maxTowerHeight);

					for (int k = 0; k < randTowerHeight; k++)
					{
						Vector3 newTowerPos = new Vector3(i, k+1, j);

						Instantiate(floorCubes[randTowerIndex], newTowerPos, Quaternion.identity);
					}
				}
			}
		}
	}

	void CreateObjectListDisplay<T>(List<T> objectList, string listName, string indexName) where T : Object
	{
		//The delayed int field means it waits for the user to finish putting in the number
		//rether than just accepting input as it comes, it waits for the "enter" key
		if (objectList != null)
		{
			int objectCount = objectList.Count;
			objectCount = Mathf.Max(0, EditorGUILayout.DelayedIntField(listName, objectCount));
			EditorGUI.indentLevel++;

			//My way of updating the UI to display the number of array fields accurately
			while (objectCount < objectList.Count)
			{
				objectList.RemoveAt(objectList.Count - 1);
			}
			while (objectCount > objectList.Count)
			{
				objectList.Add(null);
			}

			//Actual audio fields
			for (int i = 0; i < objectList.Count; i++)
			{
				objectList[i] = (T)EditorGUILayout.ObjectField(string.Format("{0}: {1}", indexName, i), objectList[i], typeof(T), false);
			}
			EditorGUI.indentLevel--;
		}
	}
}
