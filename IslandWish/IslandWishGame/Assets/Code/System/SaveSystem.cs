using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void SavePlayer(Player player)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/player.coconut";
		FileStream stream = new FileStream(path, FileMode.Create);

		PlayerData data = new PlayerData(player);

		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static void SaveEnemies(List<EnemyBehavior> enemies, string level)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + string.Format("/{0}enemies.coconut", level);
		FileStream stream = new FileStream(path, FileMode.Create);

		EnemyData data = new EnemyData(enemies);

		formatter.Serialize(stream, data);
		stream.Close();
	}	
	
	public static void SaveCoconuts(List<CoconutData> coconuts)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + ("/coconuts.coconut");
		FileStream stream = new FileStream(path, FileMode.Create);

		CoconutSaveData data = new CoconutSaveData(coconuts);

		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static PlayerData LoadPlayer()
	{
		string path = Application.persistentDataPath + "/player.coconut";

		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			PlayerData data = formatter.Deserialize(stream) as PlayerData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return null;
		}
	}

	public static EnemyData LoadEnemies(string level)
	{
		string path = Application.persistentDataPath + string.Format("/{0}enemies.coconut", level);

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			EnemyData data = formatter.Deserialize(stream) as EnemyData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return null;
		}
	}	
	
	public static CoconutSaveData LoadCoconuts()
	{
		string path = Application.persistentDataPath + string.Format("/coconuts.coconut");

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			CoconutSaveData data = formatter.Deserialize(stream) as CoconutSaveData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return null;
		}
	}

	//just a testing thing for myself. don't even know if it'll work
	public static T LoadData<T>(string fileName) where T : Data
	{
		string path = Application.persistentDataPath + string.Format("/{0}.coconut", fileName);

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			T data = formatter.Deserialize(stream) as T;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return default(T);
		}
	}
}

public class Data
{

}