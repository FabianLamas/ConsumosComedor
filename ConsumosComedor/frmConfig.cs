﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConsumosComedor
{
    public partial class frmConfig : Form
    {
        clsDAO objDAO = new clsDAO();

        public frmConfig()
        {
            InitializeComponent();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            CargarConfiguracion();
        }

        private void CargarConfiguracion()
        {
            txtPuerto.Text = ConsumosComedor.Properties.Settings.Default.Puerto;

            DataTable dt = objDAO.SqlCallLocal("SELECT DISTINCT Uo FROM Turnos");
            cmbLocal.DataSource = dt;
            cmbLocal.DisplayMember = "Uo";
            cmbLocal.ValueMember = "Uo";
            if (!ConsumosComedor.Properties.Settings.Default.Uo.Equals(""))
            {
                cmbLocal.SelectedIndex = cmbLocal.FindStringExact(ConsumosComedor.Properties.Settings.Default.Uo);
            }
        }

        private void GuardarConfiguracion()
        {

            ConsumosComedor.Properties.Settings.Default.Puerto = txtPuerto.Text;
            ConsumosComedor.Properties.Settings.Default.Uo = cmbLocal.Text;

            ConsumosComedor.Properties.Settings.Default.Save();

            frmConsumos frmConsumos = (frmConsumos)Owner;
            frmConsumos.AbrirPuerto();
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            GuardarConfiguracion();
        }
    }
}
