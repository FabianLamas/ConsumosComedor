using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace ConsumosComedor
{
    public partial class frmConsumos : Form
    {
        #region variables
        clsDAO objDAO = new clsDAO();
        clsDAO objDAOConsumos = new clsDAO();
        clsDAO objDAOPersonas = new clsDAO();
        clsDAO objDAOTurnos = new clsDAO();
        SerialPort sp = new SerialPort("COM" + ConsumosComedor.Properties.Settings.Default.Puerto);
        ThreadStart CnnThreadDelegate;
        Thread CnnThread;
        bool blnProcesando = false;
        bool blnSincronizando = false;
        bool blnConStatus = false;
        string NumeroTurno = "0";
        string NombreTurno = "Sin turno";
        string DASSCOTurno = "";
        DateTime dtmUltimaFichadas;
        #endregion

        #region methods
        public frmConsumos()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void frmConsumos_Load(object sender, EventArgs e)
        {
            //DataTable dt = objDAO.SqlCallLocal("SELECT Automatico FROM Consumos");
            DataTable dt = objDAO.SqlCall("SELECT Automatico FROM Consumos");
            if (dt == null)
                //objDAO.SqlExecLocal("ALTER TABLE Consumos ADD COLUMN Automatico BIT NULL");

                objDAO.SqlExecLocal("ALTER TABLE Consumos ADD Automatico BIT NULL");

            blnConStatus = objDAO.TestConnection();
            VerificarTurno();
            CargarConsumos();
            AbrirPuerto();
            tmrConsumos.Enabled = true;
            tmrSincronizacion.Enabled = true;
        }

        private void frmConsumos_Closing(object sender, FormClosingEventArgs e)
        {
            CerrarPuerto();
            tmrConsumos.Enabled = false;
            tmrSincronizacion.Enabled = false;
        }

        public void AbrirPuerto()
        {
            if (!sp.PortName.ToString().Equals("COM" + ConsumosComedor.Properties.Settings.Default.Puerto))
            {
                if (sp.IsOpen)
                    sp.Close();
                sp = new SerialPort("COM" + ConsumosComedor.Properties.Settings.Default.Puerto);
            }

            if (!sp.IsOpen)
            {
                try
                {
                    sp.Open();
                }
                catch
                {
                    frmConfig frmConfig = new frmConfig();
                    frmConfig.Owner = this;
                    frmConfig.Show();
                    frmConfig.Focus();
                }
            }
        }

        private void CerrarPuerto()
        {
            if (sp.IsOpen)
                sp.Close();
        }

        private void VerificarTurno()
        {
            try
            {
                //DataTable dt = objDAOTurnos.SqlCallLocal("SELECT Numero_turno, Nombre_turno, DASSCO FROM Turnos WHERE Uo = '" + ConsumosComedor.Properties.Settings.Default.Uo + "' AND CONVERT(nvarchar(5),getdate(),108) BETWEEN Hora_inicio AND Hora_final");
                DataTable dt = objDAOTurnos.SqlCall("SELECT Numero_turno, Nombre_turno, DASSCO FROM Turnos WHERE Uo = '" + ConsumosComedor.Properties.Settings.Default.Uo + "' AND CONVERT(nvarchar(5),getdate(),108) BETWEEN Hora_inicio AND Hora_final");

                if (!NumeroTurno.Equals(dt.Rows[0][0].ToString()))
                {
                    if (dt.Rows.Count > 0)
                    {
                        NumeroTurno = dt.Rows[0][0].ToString();
                        NombreTurno = dt.Rows[0][1].ToString();
                        DASSCOTurno = dt.Rows[0][2].ToString();
                    }
                    else
                    {
                        NumeroTurno = "0";
                        NombreTurno = "Sin turno";
                        DASSCOTurno = "";
                    }

                    imgFoto.ImageLocation = "";

                    CargarConsumos();
                }
            }
            catch (Exception ex)
            {
                NumeroTurno = "0";
                NombreTurno = "Sin turno";
                DASSCOTurno = "";
            }
            Text = "Comedor (" + NombreTurno + ")";
        }

        private void CargarConsumos()
        {
            NumeroTurno = "3";
            //DataTable dt = objDAO.SqlCallLocal("SELECT * FROM Consumos WHERE Turno = " + NumeroTurno + " AND Local = '" + ConsumosComedor.Properties.Settings.Default.Uo + "' AND Fecha = CONVERT(INT,CONVERT(NVARCHAR(8),GETDATE(),112)) ORDER BY Id desc");
            DataTable dt = objDAO.SqlCall("SELECT * FROM Consumos c, BCPERSO1 bc WHERE c.Numero_tarjeta = bc.ZAUSW AND Numero_turno = " + NumeroTurno + " AND Departamento = '" + ConsumosComedor.Properties.Settings.Default.Uo + "' AND Fecha_consumo = CONVERT(INT,CONVERT(NVARCHAR(8),GETDATE(),112)) ORDER BY Fecha_consumo desc");
            //DataTable dt = objDAO.SqlCall("SELECT * FROM Consumos WHERE Numero_turno = " + NumeroTurno + " AND Departamento = '" + ConsumosComedor.Properties.Settings.Default.Uo + "' AND Fecha_consumo = CONVERT(INT,CONVERT(NVARCHAR(8),GETDATE(),112)) ORDER BY Fecha_consumo desc");

            lstConsumos.ValueMember = "Numero_tarjeta";
            lstConsumos.DisplayMember = "ENAME";
            lstConsumos.DataSource = dt;
            if (lstConsumos.Items.Count > 0)
                lstConsumos.SelectedIndex = 0;
            else
                lstConsumos.SelectedIndex = -1;
        }

        private void GrabarConsumoTarjeta(string _Tarjeta)
        {
            //DataTable dt = objDAO.SqlCallLocal("SELECT * FROM HistPerso1 WHERE CONVERT(int,ZAUSW) = CONVERT(int," + _Tarjeta + ")");
            DataTable dt = objDAO.SqlCall("SELECT * FROM HistPerso1 WHERE CONVERT(int,ZAUSW) = CONVERT(int," + _Tarjeta + ")");

            if (blnConStatus && dt.Rows.Count == 0)
            {
                dt = objDAO.SqlCall("SELECT TOP 1 Tarjeta FROM [Accesos].[dbo].[Personas] WHERE Tarjeta = " + _Tarjeta);

                if (dt.Rows.Count == 0)
                    dt = objDAO.SqlCall("SELECT TOP 1 Tarjeta FROM [Accesos].[dbo].[Personas] WHERE Provisoria = " + _Tarjeta);

                if (dt.Rows.Count > 0)
                    //dt = objDAO.SqlCallLocal("SELECT * FROM HistPerso1 WHERE CONVERT(int,ZAUSW) = CONVERT(int," + dt.Rows[0][0].ToString() + ")");
                    dt = objDAO.SqlCall("SELECT * FROM HistPerso1 WHERE CONVERT(int,ZAUSW) = CONVERT(int," + dt.Rows[0][0].ToString() + ")");
            }

            if (dt.Rows.Count > 0)
            {
                //if (objDAO.SqlCallLocal("SELECT Legajo FROM Consumos WHERE Turno = " + NumeroTurno + " AND Fecha = CONVERT(INT,CONVERT(NVARCHAR(8),GETDATE(),112)) AND CONVERT(INT,Legajo) = CONVERT(INT," + dt.Rows[0][3].ToString() + ")").Rows.Count < 1)
                if (objDAO.SqlCall("SELECT Numero_tarjeta FROM Consumos WHERE Numero_turno = " + NumeroTurno + " AND Fecha_consumo = CONVERT(INT,CONVERT(NVARCHAR(8),GETDATE(),112)) AND CONVERT(INT,Numero_tarjeta) = CONVERT(INT," + dt.Rows[0][3].ToString() + ")").Rows.Count < 1)
                {
                    string EsVisita = (dt.Rows[0][2].ToString().Equals("08") ? "1" : "0");
                    //objDAO.SqlExecLocal("INSERT INTO Consumos (Fecha,Hora,Tarjeta,Legajo,Nombre,CeCo,EsVisita,Turno,Local,Automatico) VALUES (CONVERT(INT,CONVERT(NVARCHAR(8),GETDATE(),112)),CONVERT(NVARCHAR(5),GETDATE(),108),'" + dt.Rows[0][0].ToString() + "','" + dt.Rows[0][3].ToString() + "','" + dt.Rows[0][4].ToString() + "','CentroCosto'," + EsVisita + "," + NumeroTurno + ",'" + ConsumosComedor.Properties.Settings.Default.Uo + "',1)");
                    // borrar objDAO.SqlExecLocal("INSERT INTO Consumos (Fecha_consumo, Numero_turno, Numero_tarjeta, Numero_comida, DASSCO, Numero_pago, Hora, Departamento, Automatico, Numero_lote, Transferida) VALUES (CONVERT(INT,CONVERT(NVARCHAR(8),GETDATE(),112)),CONVERT(NVARCHAR(5),GETDATE(),108),'" + dt.Rows[0][0].ToString() + "','" + dt.Rows[0][3].ToString() + "','" + dt.Rows[0][4].ToString() + "','CentroCosto'," + EsVisita + "," + NumeroTurno + ",'" + ConsumosComedor.Properties.Settings.Default.Uo + "',1)");

                    //arreglar objDAO.SqlExec("INSERT INTO Consumos(Fecha_consumo, Numero_turno, Numero_tarjeta, Numero_comida, DASSCO, Numero_pago, Hora, Departamento, Automatico, Numero_Lote, Transferida) VALUES(CONVERT(INT, CONVERT(NVARCHAR(8), GETDATE(), 112), '" + NumeroTurno + "', '" + dt.Rows[0][0].ToString() + "', 1 , 'Z003', " + 0 + ", CONVERT(NVARCHAR(5), GETDATE(), 108), '" + ConsumosComedor.Properties.Settings.Default.Uo + "', 1, NULL, 3)"); 

                    objDAO.SqlExec("INSERT INTO dbo.Consumos(Fecha_consumo,Numero_turno,Numero_tarjeta,Numero_comida,DASSCO,Numero_pago,Hora,Departamento,Automatico,Numero_Lote,Transferida) VALUES ('20191127', 3, 747218, 1, 'Z004', 0, '01:08', 'COUY', 1, NULL, 3)");

                    CargarConsumos();
                }
                else
                {
                    MessageBox.Show("Ya existe un consumo para esta persona.");
                }
            }
            else
            {
                MessageBox.Show("No se encuentra a la persona. La tarjeta leída es " + _Tarjeta);
            }
        }

        public void GrabarConsumoManual(string _Legajo)
        {
            DataTable dt = objDAO.SqlCallLocal("SELECT * FROM HistPerso1 WHERE CONVERT(int,PERNR) = CONVERT(int," + _Legajo + ")");
            if (dt.Rows.Count > 0)
            {
                if (objDAO.SqlCallLocal("SELECT Legajo FROM Consumos WHERE Turno = " + NumeroTurno + " AND Fecha = CONVERT(INT,CONVERT(NVARCHAR(8),GETDATE(),112)) AND CONVERT(INT,Legajo) = CONVERT(INT," + dt.Rows[0][3].ToString() + ")").Rows.Count < 1)
                {
                    string EsVisita = (dt.Rows[0][2].ToString().Equals("08") ? "1" : "0");
                    objDAO.SqlExecLocal("INSERT INTO Consumos (Fecha,Hora,Tarjeta,Legajo,Nombre,CeCo,EsVisita,Turno,Local,Automatico) VALUES (CONVERT(INT,CONVERT(NVARCHAR(8),GETDATE(),112)),CONVERT(NVARCHAR(5),GETDATE(),108),'" + dt.Rows[0][0].ToString() + "'," + _Legajo + ",'" + dt.Rows[0][4].ToString() + "','CentroCosto'," + EsVisita + "," + NumeroTurno + ",'" + ConsumosComedor.Properties.Settings.Default.Uo + "',0)");
                    CargarConsumos();
                }
                else
                {
                    MessageBox.Show("Ya existe un consumo para esta persona.");
                }
            }
            else
            {
                MessageBox.Show("No se encuentra a la persona.");
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            frmManual frmManual = new frmManual();
            frmManual.Owner = this;
            frmManual.Show();
            frmManual.Focus();

            frmManual.Location = new Point(Location.X + Size.Width - (frmManual.Size.Width + 20), Location.Y + Size.Height - (frmManual.Size.Height + 18));
        }

        private void lstConsumos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtNombre.Text = lstConsumos.Text;
                string Legajo = ((DataRowView)lstConsumos.SelectedItem).Row[2].ToString();

                imgFoto.ImageLocation = objDAO.SqlCall("SELECT TOP 1 Valor FROM Configuracion WHERE Id='Fotos'").Rows[0][0].ToString() + Convert.ToInt32(Legajo).ToString() + ".jpg";
            }
            catch { }
        }
        #endregion

        #region timers
        private void tmrConsumos_Tick(object sender, EventArgs e)
        {
            try { VerificarTurno(); }
            catch (Exception ex) { }

            if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0)
                SyncPersonas();

            if (dtmUltimaFichadas < DateTime.Now.AddSeconds(-10))
            {
                imgFoto.ImageLocation = "";
            }
            if (!NumeroTurno.Equals("0") && sp.IsOpen)
            {
                btnManual.Enabled = true;
                //personasToolStripMenuItem.Enabled = false;
                //turnosToolStripMenuItem.Enabled = false;
                if (sp.BytesToRead >= 14)
                {
                    dtmUltimaFichadas = DateTime.Now;
                    string strLine = sp.ReadLine().Trim();

                    GrabarConsumoTarjeta(strLine.Substring(strLine.Length - 6));
                }
            }
            else
            {
                btnManual.Enabled = false;
                personasToolStripMenuItem.Enabled = true;
                turnosToolStripMenuItem.Enabled = true;
            }
        }

        private void tmrSincronizacion_Tick(object sender, EventArgs e)
        {
            SyncConsumos();
        }
        #endregion

        #region threads
        private void SyncConsumos()
        {
            CnnThreadDelegate = new ThreadStart(SyncConsumosThread);
            CnnThread = new Thread(CnnThreadDelegate);

            if (!blnSincronizando)
            {
                blnSincronizando = true;
                CnnThread.Start();
            }
        }

        private void SyncConsumosThread()
        {
            try
            {
                int ultimo = Convert.ToInt32(objDAOConsumos.SqlCallLocal("SELECT Ultimo FROM Sincronizado").Rows[0][0].ToString());

                if (objDAOConsumos.SqlCall("SELECT * FROM Sincronizar WHERE UO = '" + Properties.Settings.Default.Uo + "'").Rows.Count > 0)
                {
                    ultimo = 0;
                    objDAOConsumos.SqlExec("DELETE FROM Sincronizar WHERE UO = '" + Properties.Settings.Default.Uo + "'");
                }

                DataTable dt = objDAOConsumos.SqlCallLocal("SELECT c.Id,c.Fecha,c.Turno,1,t.DASSCO,c.Tarjeta,c.Hora,c.Local,c.Automatico FROM Consumos c INNER JOIN Turnos t ON t.Numero_turno = c.Turno AND t.Uo = c.Local  WHERE Id > " + ultimo.ToString());

                progressBar1.Maximum = dt.Rows.Count;
                progressBar1.Value = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    objDAOConsumos.SqlExec("SP_NUEVO_CONSUMO '" + dr[1].ToString() + "'," + dr[2].ToString() + "," + dr[3].ToString() + ",'" + dr[4].ToString() + "','" + dr[5].ToString() + "','" + dr[6].ToString() + "','" + dr[7].ToString() + "'," + dr[8].ToString() + "");

                    objDAOConsumos.SqlExecLocal("UPDATE Sincronizado SET Ultimo = " + dr[0].ToString());
                    progressBar1.Value += 1;
                }
                progressBar1.Value = 0;
            }
            catch (Exception ex) { }

            blnSincronizando = false;
        }

        private void SyncTurnos()
        {
            CnnThreadDelegate = new ThreadStart(SyncTurnosThread);
            CnnThread = new Thread(CnnThreadDelegate);

            if (!blnProcesando)
            {
                personasToolStripMenuItem.Enabled = false;
                turnosToolStripMenuItem.Enabled = false;
                blnProcesando = true;
                CnnThread.Start();
            }
        }

        private void SyncTurnosThread()
        {
            DataTable dt = objDAOTurnos.SqlCall("SELECT * FROM Turnos");

            progressBar1.Maximum = dt.Rows.Count;
            progressBar1.Value = 0;
            objDAOTurnos.SqlExecLocal("DELETE FROM Turnos");
            foreach (DataRow dr in dt.Rows)
            {
                objDAOTurnos.SqlExecLocal("INSERT INTO Turnos SELECT '" + dr[0].ToString() + "'," + dr[1].ToString() + ",'" + dr[2].ToString() + "','" + dr[3].ToString() + "','" + dr[4].ToString() + "','" + dr[5].ToString() + "'");
                progressBar1.Value += 1;
            }
            personasToolStripMenuItem.Enabled = true;
            turnosToolStripMenuItem.Enabled = true;
            blnProcesando = false;
            progressBar1.Value = 0;
        }

        private void SyncPersonas()
        {
            CnnThreadDelegate = new ThreadStart(SyncPersonasThread);
            CnnThread = new Thread(CnnThreadDelegate);

            if (!blnProcesando)
            {
                personasToolStripMenuItem.Enabled = false;
                turnosToolStripMenuItem.Enabled = false;
                blnProcesando = true;
                CnnThread.Start();
            }
        }

        private void SyncPersonasThread()
        {
            DataTable dt = objDAOPersonas.SqlCall("SELECT [ZAUSW],[ZAUVE],[ZANBE],[PERNR],[ENAME],[INFO1],[INFOA],[IMAIL],CONVERT(varchar(8),FECALTA,112) FECALTA,CONVERT(varchar(8),FECBAJA,112) FECBAJA FROM [Comedor].[dbo].[HistPerso1] WHERE PERNR IS NOT NULL AND FECBAJA IS NULL ORDER BY FECALTA DESC");

            //if (dt.Rows.Count > 0)
            //    objDAOPersonas.SqlExecLocal("DELETE FROM HistPerso1");
            int i = 0;
            personasToolStripMenuItem.Text = i.ToString() + "/" + dt.Rows.Count.ToString();
            foreach (DataRow dr in dt.Rows)
            {
                objDAOPersonas.SqlExecLocal("DELETE FROM HistPerso1 WHERE PERNR = '" + dr[3].ToString() + "'");
                objDAOPersonas.SqlExecLocal("INSERT INTO HistPerso1 SELECT '" + dr[0].ToString() + "','" + dr[1].ToString() + "','" + dr[2].ToString() + "','" + dr[3].ToString() + "','" + dr[4].ToString() + "','" + dr[5].ToString() + "','" + dr[6].ToString() + "','" + dr[7].ToString() + "','" + dr[8].ToString() + "','" + dr[9].ToString() + "'");
                i++;
                personasToolStripMenuItem.Text = i.ToString() + "/" + dt.Rows.Count.ToString();
            }
            personasToolStripMenuItem.Enabled = true;
            turnosToolStripMenuItem.Enabled = true;
            blnProcesando = false;
            personasToolStripMenuItem.Text = "Sincronizar personas";
        }
        #endregion

        #region menus
        private void reporteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cargaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void personasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objDAO.TestConnection())
            {
                personasToolStripMenuItem.Enabled = false;
                turnosToolStripMenuItem.Enabled = false;
                SyncPersonas();
            }
            personasToolStripMenuItem.Enabled = true;
            turnosToolStripMenuItem.Enabled = true;
        }

        private void turnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objDAO.TestConnection())
            {
                personasToolStripMenuItem.Enabled = false;
                turnosToolStripMenuItem.Enabled = false;
                SyncTurnos();
            }
            personasToolStripMenuItem.Enabled = true;
            turnosToolStripMenuItem.Enabled = true;
        }

        private void configuracionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConfig frmConfig = new frmConfig();
            frmConfig.Owner = this;
            frmConfig.Show();
            frmConfig.Focus();
        }
        #endregion
    }
}
