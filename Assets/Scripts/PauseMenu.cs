using System.Collections;
using System.Collections.Generic;
using System.Data;
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
						//materialList.Add(new KeyValuePair<string,string> (matName,matFile));
					}
					reader.Close();
				}
			}
			connection.Close();
		}
		PopulateDropDownMenu();
	}
	
	private void PopulateDropDownMenu()
	{
		dropdownMenu.ClearOptions();		
		dropdownMenu.AddOptions(matNames);
	}

	public void SkinSelect(TMP_Dropdown selected)
	{
		int index = selected.value;
		//tempMaterial = Resources.Load("Materials/" + matFileNames[index].ToString(), typeof(Material)) as Material;
		tempMaterial = Resources.Load("Materials/" + matFileNames[index], typeof(Material)) as Material;
		
		if(ARMesh.gameObject.gameObject.activeInHierarchy)
		{
			Debug.Log("ARMesh change to " + matFileNames[index].ToString());
			Material[] tempARMesh = ARMesh.materials;
			tempARMesh[1] = tempMaterial;
			tempARMesh[3] = tempMaterial;
			tempARMesh[5] = tempMaterial;
			ARMesh.materials = tempARMesh;
		}
		if(HandGunMesh.gameObject.gameObject.activeInHierarchy)
		{
			Debug.Log("HandGunMesh chage to" + matFileNames[index].ToString());			
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

	public void SaveAssualtGunSkin()
	{
		
	}

	public void SaveHandGunSkin()
	{

	}	
	
	public void DropdownSample(int idx)
	{
		
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
