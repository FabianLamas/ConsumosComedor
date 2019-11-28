using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.ProviderBase;
using System.Data.OleDb;
using System.Runtime.InteropServices;

/// <summary>
/// Summary description for clsDAO
/// </summary>
public class clsDAO
{
    SqlConnection cnn;
    SqlCeConnection cnnLocal;
    bool blnStatus = true;

    public string strNombreConexion
    {
        get
        {
            return ConsumosComedor.Properties.Settings.Default.CnnComedor;
        }
    }

    public string strNombreConexionLocal
    {
        get
        {
            return ConsumosComedor.Properties.Settings.Default.CnnLocal;
        }
    }

    public bool TestConnection()
    {
        cnn = new SqlConnection(strNombreConexion);

        SqlCommand sqlComm = new SqlCommand("SELECT 1");
        sqlComm.Connection = cnn;
        sqlComm.CommandTimeout = 30;

        try
        {
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = sqlComm;
            DataTable tmpDT = new DataTable();
            da.Fill(tmpDT);
            cnn.Close();
            blnStatus = true;
        }
        catch (Exception ex)
        {
            blnStatus = false;
        }

        return blnStatus;
    }

    public System.Data.DataTable SqlCall(string strValue)
    {
        try
        {
            cnn = new SqlConnection(strNombreConexion);
            SqlCommand sqlComm = new SqlCommand(strValue);
            sqlComm.Connection = cnn;
            sqlComm.CommandTimeout = 30000;
            cnn.Open();
            //sqlComm.ExecuteScalar();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = sqlComm;
            DataTable tmpDT = new DataTable();
            da.Fill(tmpDT);

            cnn.Close();
            System.Diagnostics.Debug.WriteLine("------------------------------------------------------------");
            System.Diagnostics.Debug.WriteLine(tmpDT);
            return tmpDT;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void SqlExec(string strValue)
    {
        try
        {
            cnn = new SqlConnection(strNombreConexion);
            SqlCommand sqlComm = new SqlCommand(strValue);
            sqlComm.Connection = cnn;
            sqlComm.CommandTimeout = 30000;
            cnn.Open();
            sqlComm.ExecuteScalar();
            sqlComm = null;
            cnn.Close();
        }
        catch (Exception ex)
        {
            cnn.Close();
            throw ex;
        }

    }

    public System.Data.DataTable SqlCallLocal(string strValue)
    {
        try
        {
            cnnLocal = new SqlCeConnection(strNombreConexionLocal);
            SqlCeCommand sqlComm = new SqlCeCommand(strValue, cnnLocal);
            if (cnnLocal.State.ToString().Equals("Open"))
                cnnLocal.Close();
            cnnLocal.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter(sqlComm);
            DataTable tmpDT = new DataTable();
            da.Fill(tmpDT);
            cnnLocal.Close();
            return tmpDT;
        }
        catch (Exception ex)
        {
            return null;
            //throw ex;
        }
    }

    public void SqlExecLocal(string strValue)
    {
        try
        {
            cnnLocal = new SqlCeConnection(strNombreConexionLocal/*"Data Source = KOFARALCBD03;Initial Catalog = Comedor; user id = ComedorDesktop; password=C0m3d0r;");*/);
            SqlCeCommand sqlComm = new SqlCeCommand(strValue);
            sqlComm.Connection = cnnLocal;
            //sqlComm.CommandTimeout = 30000;
            if (cnnLocal.State == ConnectionState.Open)
                cnnLocal.Close();
            cnnLocal.Open();
            sqlComm.ExecuteNonQuery();
            cnnLocal.Close();
        }
        catch (Exception ex)
        {
            //cnnLocal.Close();
            //throw ex;
        }

    }
}
