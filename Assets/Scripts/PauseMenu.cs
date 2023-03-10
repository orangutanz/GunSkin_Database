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
	private TMP_Dropdown dropdown;
	private bool isPaused = false;
	public bool GetIsPaused(){return isPaused;}
	
    private string dbName = "URI=file:GameDB.db";
	
	private List<KeyValuePair<string,string>> materialList = new List<KeyValuePair<string,string>> (); 


	public void Start()
	{		
        using (var connection = new SqliteConnection(dbName))
        {
			connection.Open();
			
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "Select * from Material";
			
                using (var reader = command.ExecuteReader())
                {
					string DBMat = "";
					while(reader.Read())
					{

					}
				}
			}



			connection.Close();
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
