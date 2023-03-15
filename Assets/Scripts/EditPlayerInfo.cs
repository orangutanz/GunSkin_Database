using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System;
using Mono.Data.Sqlite;

public class EditPlayerInfo : MonoBehaviour
{

    private string dbName = "URI=file:GameDB.db";

    [SerializeField] private InputField userInput;
    private bool isInput = false;


    [SerializeField] private InputField fNameInput;
    [SerializeField] private InputField lNameInput;
    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField emailInput;
    [SerializeField] private InputField DoBInput;

    private string fName;
    private string lName;
    private string username;
    private string email;

    private int ID;
    // Start is called before the first frame update


    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DisplayInfo()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                int noPlayers = 0;
                
                command.CommandText = "select count( * ), FName, LName, Username, Email  from Player where Username = '" + userInput.text + "' or Email = '" + userInput.text + "' ;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        noPlayers = reader.GetInt32(0);
                        if (noPlayers > 0)
                        {
                            fNameInput.text += reader["FName"];
                            fName += reader["FName"];
                            lNameInput.text += reader["LName"];
                            lName += reader["LName"];
                            usernameInput.text += reader["Email"];
                            username += reader["Email"];
                            emailInput.text += reader["Username"];
                            email += reader["Username"];
                        }
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
    }


    public void ChangeInfo()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "select count( * ), ID from Player where Username = '" + userInput.text + "' or Email = '" + userInput.text + "' ;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int noPlayers = reader.GetInt32(0);
                        if (noPlayers == 0)
                        {
                            Debug.Log("User doesn't Exists");
                            reader.Close();
                        }
                        else
                        {
                            string s ="";
                            s += reader["ID"];
                            ID  = int.Parse(s);
                            reader.Close();
                            command.CommandText = "update Player set FName = '"+ fNameInput.text+ "',LName = '" + lNameInput.text + "',Username = '" + usernameInput.text + "', Email = '" + emailInput.text + "'  where ID = " + ID +";";
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
