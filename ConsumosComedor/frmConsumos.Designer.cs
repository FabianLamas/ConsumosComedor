namespace ConsumosComedor
{
    partial class frmConsumos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConsumos));
            this.imgFoto = new System.Windows.Forms.PictureBox();
            this.lstConsumos = new System.Windows.Forms.ListBox();
            this.tmrConsumos = new System.Windows.Forms.Timer(this.components);
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.btnManual = new System.Windows.Forms.Button();
            this.tmrSincronizacion = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.reporteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.configuracionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.imgFoto)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgFoto
            // 
            this.imgFoto.ErrorImage = ((System.Drawing.Image)(resources.GetObject("imgFoto.ErrorImage")));
            this.imgFoto.Location = new System.Drawing.Point(380, 38);
            this.imgFoto.Name = "imgFoto";
            this.imgFoto.Size = new System.Drawing.Size(274, 292);
            this.imgFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgFoto.TabIndex = 0;
            this.imgFoto.TabStop = false;
            // 
            // lstConsumos
            // 
            this.lstConsumos.FormattingEnabled = true;
            this.lstConsumos.Location = new System.Drawing.Point(13, 38);
            this.lstConsumos.Name = "lstConsumos";
            this.lstConsumos.Size = new System.Drawing.Size(358, 355);
            this.lstConsumos.TabIndex = 1;
            this.lstConsumos.SelectedIndexChanged += new System.EventHandler(this.lstConsumos_SelectedIndexChanged);
            // 
            // tmrConsumos
            // 
            this.tmrConsumos.Interval = 1000;
            this.tmrConsumos.Tick += new System.EventHandler(this.tmrConsumos_Tick);
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(377, 343);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(47, 13);
            this.lblNombre.TabIndex = 3;
            this.lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.Enabled = false;
            this.txtNombre.Location = new System.Drawing.Point(424, 340);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(229, 20);
            this.txtNombre.TabIndex = 5;
            // 
            // btnManual
            // 
            this.btnManual.Location = new System.Drawing.Point(466, 366);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(120, 26);
            this.btnManual.TabIndex = 7;
            this.btnManual.Text = "Carga &manual";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // tmrSincronizacion
            // 
            this.tmrSincronizacion.Interval = 5000;
            this.tmrSincronizacion.Tick += new System.EventHandler(this.tmrSincronizacion_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reporteToolStripMenuItem,
            this.cargaToolStripMenuItem,
            this.personasToolStripMenuItem,
            this.turnosToolStripMenuItem,
            this.configuracionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(668, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // reporteToolStripMenuItem
            // 
            this.reporteToolStripMenuItem.Name = "reporteToolStripMenuItem";
            this.reporteToolStripMenuItem.Size = new System.Drawing.Size(134, 20);
            this.reporteToolStripMenuItem.Text = "&Reporte de consumos";
            this.reporteToolStripMenuItem.Click += new System.EventHandler(this.reporteToolStripMenuItem_Click);
            // 
            // cargaToolStripMenuItem
            // 
            this.cargaToolStripMenuItem.Name = "cargaToolStripMenuItem";
            this.cargaToolStripMenuItem.Size = new System.Drawing.Size(113, 20);
            this.cargaToolStripMenuItem.Text = "&Carga por archivo";
            this.cargaToolStripMenuItem.Click += new System.EventHandler(this.cargaToolStripMenuItem_Click);
            // 
            // personasToolStripMenuItem
            // 
            this.personasToolStripMenuItem.Name = "personasToolStripMenuItem";
            this.personasToolStripMenuItem.Size = new System.Drawing.Size(127, 20);
            this.personasToolStripMenuItem.Text = "Sincronizar personas";
            this.personasToolStripMenuItem.Click += new System.EventHandler(this.personasToolStripMenuItem_Click);
            // 
            // turnosToolStripMenuItem
            // 
            this.turnosToolStripMenuItem.Name = "turnosToolStripMenuItem";
            this.turnosToolStripMenuItem.Size = new System.Drawing.Size(114, 20);
            this.turnosToolStripMenuItem.Text = "Sincronizar turnos";
            this.turnosToolStripMenuItem.Click += new System.EventHandler(this.turnosToolStripMenuItem_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(0, 394);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(668, 10);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 11;
            // 
            // configuracionToolStripMenuItem
            // 
            this.configuracionToolStripMenuItem.Name = "configuracionToolStripMenuItem";
            this.configuracionToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.configuracionToolStripMenuItem.Text = "Configuración";
            this.configuracionToolStripMenuItem.Click += new System.EventHandler(this.configuracionToolStripMenuItem_Click);
            // 
            // frmConsumos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 404);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnManual);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lstConsumos);
            this.Controls.Add(this.imgFoto);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(684, 442);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(684, 442);
            this.Name = "frmConsumos";
            this.Text = "Comedor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConsumos_Closing);
            this.Load += new System.EventHandler(this.frmConsumos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgFoto)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imgFoto;
        private System.Windows.Forms.ListBox lstConsumos;
        private System.Windows.Forms.Timer tmrConsumos;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.Timer tmrSincronizacion;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem reporteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cargaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem personasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnosToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolStripMenuItem configuracionToolStripMenuItem;
    }
}

