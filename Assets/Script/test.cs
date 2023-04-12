using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using TMPro;

public class test : MonoBehaviour
{
    [SerializeField] string host;
    [SerializeField] string dataBase;
    [SerializeField] string userName;
    [SerializeField] string password;
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] TextMeshProUGUI LogLogin;
    [SerializeField] TMP_InputField ifLogin;
    [SerializeField] TMP_InputField ifPassword;
    MySqlConnection conn;
    string constr;

    void ConnectBDD()
    {
        constr = "server=" + host + ";DATABASE=" + dataBase + ";User ID=" + userName + ";Password=" + password + ";Pooling=true;Charset=utf8;";
        try
        {
            conn = new MySqlConnection(constr);
            conn.Open();
        }
        catch (System.Exception ex)
        {
            txt.text = ex.Message;
            throw;
        }
    }

    void CloseBDD()
    {
        Debug.Log("ShutDown Connexion");
        if (conn != null && conn.State.ToString() != "Closed")
        {
            conn.Close();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (conn == null)
            return;
        txt.text = conn.State.ToString();
    }

    private void OnApplicationQuit()
    {
        CloseBDD();
    }

    public void Register()
    {
        ConnectBDD();
        if (conn == null)
            return;

        //verification si le compte existe deja
        bool exist = false;
        MySqlCommand verifCmd = new MySqlCommand("SELECT Pseudo FROM Users WHERE Pseudo ='"+ifLogin.text+ "'", conn);
        MySqlDataReader myReader = verifCmd.ExecuteReader();

        while (myReader.Read())
        {
            if (myReader["Pseudo"].ToString() != "")
            {
                LogLogin.text = "User Pseudo Exist";
                exist = true;
            }
        }
        myReader.Close();
        if(!exist)
        {
            string command = "INSERT INTO Users VALUE (default,'" + ifLogin.text + "','" + ifPassword.text + "');";
            MySqlCommand cmd = new MySqlCommand(command, conn);
            try
            {
                cmd.ExecuteReader();
                LogLogin.text = "Register Succesfull";
            }
            catch (System.Exception ex)
            {
                txt.text = ex.Message;
                throw;
            }
            cmd.Dispose();
        }
       
        CloseBDD();
    }

    public void Login()
    {
        ConnectBDD();

        string pass = null;

        try
        {
            MySqlCommand cmdSql = new MySqlCommand("SELECT * FROM Users WHERE Pseudo = '" + ifLogin.text + "'", conn);
            MySqlDataReader myReader = cmdSql.ExecuteReader();
            while (myReader.Read())
            {
                Debug.Log("inWhile");
                if (myReader["Pseudo"].ToString() != "")
                {
                    pass = myReader["password"].ToString();

                    if (pass == ifPassword.text)
                    {
                        LogLogin.text = "connected";
                        Debug.Log("connected");
                    }
                    else
                    {
                        LogLogin.text = "Invalid Pseudo/password";
                        Debug.Log("invalid");

                    }
                }
            }
            myReader.Close();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
            throw;
        }

       

        CloseBDD(); 
    }

}
