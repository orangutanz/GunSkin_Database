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
	private TMP_Dropdown[] dropdownMenus;	
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
				foreach (var i in dropdownMenus)
				{
					i.ClearOptions();
					i.AddOptions(matNames);
				}
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
				command.CommandText = "Select BodyM, BoltM, DetailsM, GripM, GripFrontM, StockM from AssaultRifleSkin where AssultRifleSkinID = " + StaticPlayer.ARSkinID + ";";
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

				//Handgun Skin
			}
			connection.Close();
		}

	}

	public void SkinSelect_Part_1(TMP_Dropdown selected)
	{
		SkinSelectByIndex(selected.value, 0);
	}

	public void SkinSelect_Part_2(TMP_Dropdown selected)
	{
		SkinSelectByIndex(selected.value, 1);
	}


	public void SkinSelect_Part_3(TMP_Dropdown selected)
	{
		SkinSelectByIndex(selected.value, 2);
	}


	public void SkinSelect_Part_4(TMP_Dropdown selected)
	{
		SkinSelectByIndex(selected.value, 3);
	}

	public void SkinSelect_Part_5(TMP_Dropdown selected)
	{
		SkinSelectByIndex(selected.value, 4);
	}

	public void SkinSelect_Part_6(TMP_Dropdown selected)
	{
		SkinSelectByIndex(selected.value, 5);
	}




	private void SkinSelectByIndex(int MatIndex, int PartIndex)
	{
		tempMaterial = Resources.Load("Materials/" + matFileNames[MatIndex], typeof(Material)) as Material;

		if (ARMesh.gameObject.gameObject.activeInHierarchy)
		{
			Material[] tempARMesh = ARMesh.materials;
			tempARMesh[PartIndex] = tempMaterial;
			ARMesh.materials = tempARMesh;
		}
		else if(HandGunMesh.gameObject.gameObject.activeInHierarchy)
		{
			Material[] tempGunMesh = HandGunMesh.materials;
			tempGunMesh[PartIndex] = tempMaterial;
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

		

		using (var connection = new SqliteConnection(dbName))
		{
			using (var command = connection.CreateCommand())
			{

				connection.Open();
				int playerID = StaticPlayer.PlayerID;
				int ARSkinID = StaticPlayer.ARSkinID;
				//int playerID = 1;
				//int ARSkinID = 2;


				//Save AR Skin
				if (ARMesh.gameObject.gameObject.activeInHierarchy)
				{
					int[] ARMatIDs = new int[6];
					int ARPartIdx = 0;
					foreach (var M in ARMesh.materials)
					{
						ARMatIDs[ARPartIdx] = -1;
						for (int i = 0; i < matFileNames.Count; ++i)
						{
							if (M.name.Contains(matFileNames[i]))
							{
								Debug.Log("Find index " + i + " fileName: " + matFileNames[i]);
								ARMatIDs[ARPartIdx] = i +1;
								break;
							}
						}
						ARPartIdx++;
					}
					if(ARMatIDs[0] > -1)
					{
						command.CommandText = "update AssaultRifleSkin set BodyM = '" + ARMatIDs[0] + "'  where AssultRifleSkinID = " + ARSkinID + ";";
						command.ExecuteNonQuery();
					}
					if (ARMatIDs[1] > -1)
					{
						command.CommandText = "update AssaultRifleSkin set BoltM = '" + ARMatIDs[1] + "'  where AssultRifleSkinID = " + ARSkinID + ";";
						command.ExecuteNonQuery();
					}
					if (ARMatIDs[2] > -1)
					{
						command.CommandText = "update AssaultRifleSkin set DetailsM = '" + ARMatIDs[2] + "'  where AssultRifleSkinID = " + ARSkinID + ";";
						command.ExecuteNonQuery();
					}
					if (ARMatIDs[3] > -1)
					{
						command.CommandText = "update AssaultRifleSkin set GripM = '" + ARMatIDs[3] + "'  where AssultRifleSkinID = " + ARSkinID + ";";
						command.ExecuteNonQuery();
					}
					if (ARMatIDs[4] > -1)
					{
						command.CommandText = "update AssaultRifleSkin set GripFrontM = '" + ARMatIDs[4] + "'  where AssultRifleSkinID = " + ARSkinID + ";";
						command.ExecuteNonQuery();
					}
					if (ARMatIDs[5] > -1)
					{
						command.CommandText = "update AssaultRifleSkin set StockM = '" + ARMatIDs[5] + "'  where AssultRifleSkinID = " + ARSkinID + ";";
						command.ExecuteNonQuery();
					}

				}
				//Save Handgun Skin
				else if (HandGunMesh.gameObject.gameObject.activeInHierarchy)
				{
					foreach (var M in ARMesh.materials)
					{
						for (int i = 0; i < matFileNames.Count; ++i)
						{
							if (M.name.Contains(matFileNames[i]))
							{
								Debug.Log("Find index " + i + " fileName: " + matFileNames[i]);
							}
						}
					}
				}
				//command.CommandText = "select count( * ), ID from Player where Username = '" + userInput.text + "' or Email = '" + userInput.text + "' ;";

				connection.Close();
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
