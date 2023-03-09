using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
public class LoadMaterials : MonoBehaviour
{
    private string dbName = "URI=file:GameDB.db";

    struct Mat
    {
        public string matName;
        public string matFileName;
    }

    [SerializeField] private Object[] objects;
    private List<Material> materials = new List<Material>();
    private List<Mat> materialsData = new List<Mat>();
    // Start is called before the first frame update

    void Awake()
    {
        AddAllMaterials();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddAllMaterials()
    {
        objects = Resources.LoadAll("Materials", typeof(Material));

        foreach(Material mat in objects)
        {
            materials.Add(mat);
        }

        foreach(var mat in materials)
        {
            Mat newMat = new Mat();
            newMat.matName = mat.name;
            newMat.matFileName = (mat.name + ".mat");
        }
        materials.Clear();
    }

    public void AddMaterialToDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            for (int i = 0; i < materialsData.Count; ++i)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into Material(MaterialName, MaterialFileName) values ( '" + materialsData[i].matName + "', " + materialsData[i].matFileName + ");";

                    Debug.Log(materialsData[i].matFileName);
                    command.ExecuteNonQuery();
                }
            }
            materialsData.Clear();
            connection.Close();

        }
    }
}
