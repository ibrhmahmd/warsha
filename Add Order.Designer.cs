namespace warsha
{
    partial class Add_Order
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            back_btn = new Guna.UI2.WinForms.Guna2Button();
            guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            add_order_group = new Guna.UI2.WinForms.Guna2GroupBox();
            guna2Panel2.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel2
            // 
            guna2Panel2.BackColor = Color.Black;
            guna2Panel2.BorderRadius = 50;
            guna2Panel2.Controls.Add(back_btn);
            guna2Panel2.Controls.Add(guna2TextBox1);
            guna2Panel2.CustomizableEdges = customizableEdges5;
            guna2Panel2.Location = new Point(0, 0);
            guna2Panel2.Name = "guna2Panel2";
            guna2Panel2.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Panel2.Size = new Size(2500, 130);
            guna2Panel2.TabIndex = 10;
            // 
            // back_btn
            // 
            back_btn.BorderRadius = 20;
            back_btn.CustomizableEdges = customizableEdges1;
            back_btn.DisabledState.BorderColor = Color.DarkGray;
            back_btn.DisabledState.CustomBorderColor = Color.DarkGray;
            back_btn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            back_btn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            back_btn.FillColor = Color.White;
            back_btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            back_btn.ForeColor = Color.Black;
            back_btn.Location = new Point(2205, 30);
            back_btn.Name = "back_btn";
            back_btn.ShadowDecoration.CustomizableEdges = customizableEdges2;
            back_btn.Size = new Size(250, 70);
            back_btn.TabIndex = 0;
            back_btn.Text = "BACK";
            back_btn.Click += back_btn_Click;
            // 
            // guna2TextBox1
            // 
            guna2TextBox1.BorderColor = Color.White;
            guna2TextBox1.BorderRadius = 15;
            guna2TextBox1.CustomizableEdges = customizableEdges3;
            guna2TextBox1.DefaultText = "";
            guna2TextBox1.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            guna2TextBox1.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            guna2TextBox1.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox1.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox1.Font = new Font("Segoe UI", 9F);
            guna2TextBox1.ForeColor = Color.Black;
            guna2TextBox1.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox1.Location = new Point(10, 30);
            guna2TextBox1.Margin = new Padding(6, 7, 6, 7);
            guna2TextBox1.Name = "guna2TextBox1";
            guna2TextBox1.PasswordChar = '\0';
            guna2TextBox1.PlaceholderText = "  Search Here ";
            guna2TextBox1.SelectedText = "";
            guna2TextBox1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2TextBox1.Size = new Size(500, 70);
            guna2TextBox1.TabIndex = 0;
            // 
            // add_order_group
            // 
            add_order_group.BorderRadius = 25;
            add_order_group.CustomBorderColor = Color.White;
            add_order_group.CustomizableEdges = customizableEdges7;
            add_order_group.Font = new Font("Segoe UI", 9F);
            add_order_group.ForeColor = Color.FromArgb(125, 140, 150);
            add_order_group.Location = new Point(30, 200);
            add_order_group.Name = "add_order_group";
            add_order_group.ShadowDecoration.CustomizableEdges = customizableEdges8;
            add_order_group.Size = new Size(1300, 500);
            add_order_group.TabIndex = 12;
            add_order_group.Text = "ADD ORDER";
            // 
            // Add_Order
            // 
            AutoScaleDimensions = new SizeF(15F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2472, 1621);
            Controls.Add(add_order_group);
            Controls.Add(guna2Panel2);
            Name = "Add_Order";
            Text = "Add_Order";
            guna2Panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2Button back_btn;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        private Guna.UI2.WinForms.Guna2GroupBox add_order_group;
    }
}