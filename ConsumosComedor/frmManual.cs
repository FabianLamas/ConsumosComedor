using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConsumosComedor
{
    public partial class frmManual : Form
    {
        public frmManual()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!txtLegajo.Text.Trim().Equals(""))
            {
                frmConsumos frmConsumos = (frmConsumos)Owner;
                frmConsumos.GrabarConsumoManual(txtLegajo.Text);
            }
            txtLegajo.Text = "";
            Hide();
        }
    }
}
