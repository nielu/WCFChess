namespace WCFChess
{
    partial class GameSessionSelect
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
            this.avaliableSessionsView = new System.Windows.Forms.ListView();
            this.username_column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.guid_column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.connectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.playLocalButton = new System.Windows.Forms.Button();
            this.playRemoteButton = new System.Windows.Forms.Button();
            this.hostButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nickTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // avaliableSessionsView
            // 
            this.avaliableSessionsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.guid_column,
            this.username_column});
            this.avaliableSessionsView.Location = new System.Drawing.Point(12, 34);
            this.avaliableSessionsView.MultiSelect = false;
            this.avaliableSessionsView.Name = "avaliableSessionsView";
            this.avaliableSessionsView.Size = new System.Drawing.Size(270, 186);
            this.avaliableSessionsView.TabIndex = 0;
            this.avaliableSessionsView.UseCompatibleStateImageBehavior = false;
            this.avaliableSessionsView.View = System.Windows.Forms.View.Details;
            // 
            // username_column
            // 
            this.username_column.Text = "Player name";
            this.username_column.Width = 126;
            // 
            // guid_column
            // 
            this.guid_column.Text = "GUID";
            this.guid_column.Width = 141;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(288, 34);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Available sessions:";
            // 
            // playLocalButton
            // 
            this.playLocalButton.Location = new System.Drawing.Point(288, 139);
            this.playLocalButton.Name = "playLocalButton";
            this.playLocalButton.Size = new System.Drawing.Size(75, 23);
            this.playLocalButton.TabIndex = 3;
            this.playLocalButton.Text = "Local";
            this.playLocalButton.UseVisualStyleBackColor = true;
            this.playLocalButton.Click += new System.EventHandler(this.playLocalButton_Click);
            // 
            // playRemoteButton
            // 
            this.playRemoteButton.Enabled = false;
            this.playRemoteButton.Location = new System.Drawing.Point(288, 197);
            this.playRemoteButton.Name = "playRemoteButton";
            this.playRemoteButton.Size = new System.Drawing.Size(75, 23);
            this.playRemoteButton.TabIndex = 4;
            this.playRemoteButton.Text = "Remote";
            this.playRemoteButton.UseVisualStyleBackColor = true;
            this.playRemoteButton.Click += new System.EventHandler(this.playRemoteButton_Click);
            // 
            // hostButton
            // 
            this.hostButton.Enabled = false;
            this.hostButton.Location = new System.Drawing.Point(288, 168);
            this.hostButton.Name = "hostButton";
            this.hostButton.Size = new System.Drawing.Size(75, 23);
            this.hostButton.TabIndex = 5;
            this.hostButton.Text = "Host";
            this.hostButton.UseVisualStyleBackColor = true;
            this.hostButton.Click += new System.EventHandler(this.hostButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Nick";
            // 
            // nickTextBox
            // 
            this.nickTextBox.Location = new System.Drawing.Point(288, 9);
            this.nickTextBox.Name = "nickTextBox";
            this.nickTextBox.Size = new System.Drawing.Size(75, 20);
            this.nickTextBox.TabIndex = 7;
            this.nickTextBox.Text = "Default";
            // 
            // GameSessionSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 226);
            this.Controls.Add(this.nickTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hostButton);
            this.Controls.Add(this.playRemoteButton);
            this.Controls.Add(this.playLocalButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.avaliableSessionsView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GameSessionSelect";
            this.Text = "GameSessionSelect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView avaliableSessionsView;
        private System.Windows.Forms.ColumnHeader username_column;
        private System.Windows.Forms.ColumnHeader guid_column;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button playLocalButton;
        private System.Windows.Forms.Button playRemoteButton;
        private System.Windows.Forms.Button hostButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nickTextBox;
    }
}