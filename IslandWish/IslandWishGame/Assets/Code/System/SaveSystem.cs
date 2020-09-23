using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void SavePlayer(Player player)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/player" + player.stats.playerNumber + ".coconut";
		FileStream stream = new FileStream(path, FileMode.Create);

		PlayerData data = new PlayerData(player);

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
	public static void SaveLevel(LevelManager level)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + ("/level.level");
		FileStream stream = new FileStream(path, FileMode.Create);

		LevelManager.LevelData data = new LevelManager.LevelData(level);

		formatter.Serialize(stream, data);
		stream.Close();
	}	
	public static void SaveProgress(string currentLevelName)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + ("/progress.progress");
		FileStream stream = new FileStream(path, FileMode.Create);

		ProgressData data = new ProgressData(currentLevelName);

		formatter.Serialize(stream, data);
		stream.Close();
	}


	public static PlayerData LoadPlayer(int playerIndex)
	{
		string path = Application.persistentDataPath + "/player" + playerIndex + ".coconut";

		if (File.Exists(path))
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
	public static LevelManager.LevelData LoadLevel()
	{
		string path = Application.persistentDataPath + ("/level.level"); ;

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			LevelManager.LevelData data = formatter.Deserialize(stream) as LevelManager.LevelData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogError("File not found at: " + path);
			return null;
		}
	}
	public static ProgressData LoadProgress()
	{
		string path = Application.persistentDataPath + ("/progress.progress");

		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			ProgressData data = formatter.Deserialize(stream) as ProgressData;
			stream.Close();

			return data;
		}
		else
		{
			Debug.LogWarning("File not found at: " + path);
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