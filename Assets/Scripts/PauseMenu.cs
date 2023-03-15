using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using Mono.Data.Sqlite;
using TMPro;


public class PauseMenu : MonoBehaviour
{
	[SerializeField] 
	private GameObject pauseMenu;	
	[SerializeField] 
	private TMP_Dropdown dropdownMenu;	
	[SerializeField] 
	private SkinnedMeshRenderer  ARMesh;		
	[SerializeField] 
	private SkinnedMeshRenderer  HandGunMesh;	
	
    private string dbName = "URI=file:GameDB.db";
	private bool isPaused = false;
	public bool GetIsPaused(){return isPaused;}

	private List<string> matNames = new List<string>();
	private List<string> matFileNames = new List<string>();
	//private List<KeyValuePair<string,string>> materialList = new List<KeyValuePair<string,string>> (); 
	Material tempMaterial;

	public void Start()
	{	
		InitialSetUp();
	}
	
	
	private void InitialSetUp()
	{	
        using (var connection = new SqliteConnection(dbName))
        {
			connection.Open();
			
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "Select * from Material;";
			
                using (var reader = command.ExecuteReader())
                {
					while(reader.Read())
					{
						string matName = "";
						string matFile = "";
						matName += reader["MaterialName"];
						matFile += reader["MaterialFileName"];
						matNames.Add(matName);
						matFileNames.Add(matFile);
					}
					reader.Close();					
				}
				dropdownMenu.ClearOptions();		
				dropdownMenu.AddOptions(matNames);
				Debug.Log("InitialSetUp is done");
			}
			connection.Close();			
		}
	}

	public void LoadARSkin()
	{
		using (var connection = new SqliteConnection(dbName))
		{
			connection.Open();

			using (var command = connection.CreateCommand())
			{
				//AR skin
				command.CommandText = "Select BodyM, BoltM, DetailsM, GripM, GripFrontM, StockM from AssaultRifleSkin where AssultRifleSkinID = " + 1 + ";";
				using (var reader = command.ExecuteReader())
				{
					if(reader.Read())
					{
						Material[] tempARMesh = ARMesh.materials;
						string info = "";
						info += reader["BodyM"];
						int skinID = -1;
						skinID = Int32.Parse(info);
						if (skinID > 0)//SkinIDs start at 1
						{
							tempMaterial = Resources.Load("Materials/" + matFileNames[skinID - 1], typeof(Material)) as Material;
							if(tempMaterial)
							{
								tempARMesh[0] = tempMaterial;
							}
						}
						info = "";
						info += reader["BoltM"];
						skinID = -1;
						skinID = Int32.Parse(info);
						if (skinID > 0)//SkinIDs start at 1
						{
							tempMaterial = Resources.Load("Materials/" + matFileNames[skinID - 1], typeof(Material)) as Material;
							if (tempMaterial)
							{
								tempARMesh[1] = tempMaterial;
							}
						}
						info = "";
						info += reader["DetailsM"];
						skinID = -1;
						skinID = Int32.Parse(info);
						if (skinID > 0)//SkinIDs start at 1
						{
							tempMaterial = Resources.Load("Materials/" + matFileNames[skinID - 1], typeof(Material)) as Material;
							if (tempMaterial)
							{
								tempARMesh[2] = tempMaterial;
							}
						}
						info = "";
						info += reader["GripM"];
						skinID = -1;
						skinID = Int32.Parse(info);
						if (skinID > 0)//SkinIDs start at 1
						{
							tempMaterial = Resources.Load("Materials/" + matFileNames[skinID - 1], typeof(Material)) as Material;
							if (tempMaterial)
							{
								tempARMesh[3] = tempMaterial;
							}
						}
						info = "";
						info += reader["GripFrontM"];
						skinID = -1;
						skinID = Int32.Parse(info);
						if (skinID > 0)//SkinIDs start at 1
						{
							tempMaterial = Resources.Load("Materials/" + matFileNames[skinID - 1], typeof(Material)) as Material;
							if (tempMaterial)
							{
								tempARMesh[4] = tempMaterial;
							}
						}
						info = "";
						info += reader["StockM"];
						skinID = -1;
						skinID = Int32.Parse(info);
						if (skinID > 0)//SkinIDs start at 1
						{
							tempMaterial = Resources.Load("Materials/" + matFileNames[skinID - 1], typeof(Material)) as Material;
							if (tempMaterial)
							{
								tempARMesh[5] = tempMaterial;
							}
						}
						ARMesh.materials = tempARMesh;
					}
					reader.Close();
				}
			}
			connection.Close();
		}

	}

	public void SkinSelect(TMP_Dropdown selected)
	{
		SkinSelectByIndex(selected.value);
	}

	private void SkinSelectByIndex(int index)
	{
		tempMaterial = Resources.Load("Materials/" + matFileNames[index], typeof(Material)) as Material;

		Debug.Log("LoadAR skin index " + index);

		if (ARMesh.gameObject.gameObject.activeInHierarchy)
		{
			Material[] tempARMesh = ARMesh.materials;
			tempARMesh[1] = tempMaterial;
			tempARMesh[3] = tempMaterial;
			tempARMesh[5] = tempMaterial;
			ARMesh.materials = tempARMesh;
		}
		if(HandGunMesh.gameObject.gameObject.activeInHierarchy)
		{
			Material[] tempGunMesh = HandGunMesh.materials;
			tempGunMesh[2] = tempMaterial;
			tempGunMesh[3] = tempMaterial;
			tempGunMesh[4] = tempMaterial;
			HandGunMesh.materials = tempGunMesh;
		}
	}

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		pauseMenu.SetActive(true);
		Cursor.visible = true;	
        Cursor.lockState = CursorLockMode.None;
		isPaused = true;
	}

	public void ResumeGame()
	{
		Time.timeScale = 1.0f;
		pauseMenu.SetActive(false);
		Cursor.visible = false;	
		Cursor.lockState = CursorLockMode.Locked;
		isPaused = false;

	}

	public void SaveGunSkins()
	{

		string matString = "";
		int findIdx = -1;
		foreach(var M in ARMesh.materials)
        {
			for(int i =0;i< matFileNames.Count;++i)
            {
				if(matFileNames[i] == M.name)
                {
					findIdx = i;
                }
            }
        }

		using (var connection = new SqliteConnection(dbName))
		{
			using (var command = connection.CreateCommand())
			{
				//command.CommandText = "select count( * ), ID from Player where Username = '" + userInput.text + "' or Email = '" + userInput.text + "' ;";
				using (IDataReader reader = command.ExecuteReader())
				{

				}
			}
		}
	}

	public void ExitToMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			if(isPaused)
			{
				ResumeGame();
			}
			else
			{
				PauseGame();
			}
		}
	}	
}
