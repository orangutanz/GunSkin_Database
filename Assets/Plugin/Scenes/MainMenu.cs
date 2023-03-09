using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;

public class MainMenu : MonoBehaviour
{
    //Info 
    private string dbName = "URI=file:GameDB.db";

    //Panels
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelLogin;
    [SerializeField] private GameObject panelRegister;

    //Login
    [SerializeField] private InputField loginInput;

    //Register 
    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField emailInput;
    [SerializeField] private InputField fNameInput;
    [SerializeField] private InputField lNameInput;


    // Start is called before the first frame update

    private void Awake()
    {
        panelMainMenu.SetActive(true);
        panelLogin.SetActive(false);
        panelRegister.SetActive(false);
    }

    void Start()
    {
        
    }
    //Main menu screen

    public void Login()
    {
        panelMainMenu.SetActive(false);
        panelLogin.SetActive(true);
    }

    public void Register()
    {
        panelMainMenu.SetActive(false);
        panelRegister.SetActive(true);
    }


    //Login Screen

    public void LoginPlayer()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                int noPlayers = 0;
                string loginName = "";
                command.CommandText = "select count( * ) from Player where Username = '" + loginInput.text + "' or Email = '" + loginInput.text + "' ;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        noPlayers = reader.GetInt32(0);

                    }
                    reader.Close();
                }
                command.CommandText = "select Username from Player where Username = '" + loginInput.text + "' or Email = '" + loginInput.text + "' ;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loginName += reader["Username"];
                    }
                    reader.Close();
                }

                if (noPlayers > 0)
                {
                    Debug.Log("Welcome " + loginName);
                    //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                }
                else
                {
                    Debug.Log("User Does not Exists");
                    
                }
                
            }
            connection.Close();
        }
        
    }

    //Register Screen

    public void RegisterPlayer()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select count( * ) from Player where Username = '" + loginInput.text + "';"; 
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int noPlayers = reader.GetInt32(0);
                        if (noPlayers > 0)
                        {
                            Debug.Log("User Already Exists");
                            reader.Close();
                        }
                        else
                        {
                            reader.Close();
                            command.CommandText = "insert into Player (Username, Email, FName, LName) values ('" + usernameInput.text + "','" + emailInput.text + "', '" + fNameInput.text + "' , '" + lNameInput.text + "');";
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            connection.Close();
        }
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
