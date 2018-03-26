namespace _2.DownloadManager
{
	partial class DownloadManager
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
			this.urlTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.downloadButton = new System.Windows.Forms.Button();
			this.downloadsTable = new System.Windows.Forms.DataGridView();
			this.Url = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Cancel = new System.Windows.Forms.DataGridViewButtonColumn();
			((System.ComponentModel.ISupportInitialize)(this.downloadsTable)).BeginInit();
			this.SuspendLayout();
			// 
			// urlTextBox
			// 
			this.urlTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.urlTextBox.Location = new System.Drawing.Point(41, 11);
			this.urlTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.urlTextBox.Name = "urlTextBox";
			this.urlTextBox.Size = new System.Drawing.Size(578, 20);
			this.urlTextBox.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(9, 11);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(29, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "URL";
			// 
			// downloadButton
			// 
			this.downloadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.downloadButton.Location = new System.Drawing.Point(621, 11);
			this.downloadButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.downloadButton.Name = "downloadButton";
			this.downloadButton.Size = new System.Drawing.Size(100, 20);
			this.downloadButton.TabIndex = 2;
			this.downloadButton.Text = "Download";
			this.downloadButton.UseVisualStyleBackColor = true;
			this.downloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
			// 
			// downloadsTable
			// 
			this.downloadsTable.AllowUserToAddRows = false;
			this.downloadsTable.AllowUserToDeleteRows = false;
			this.downloadsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.downloadsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Url,
            this.Status,
            this.Cancel});
			this.downloadsTable.Location = new System.Drawing.Point(11, 42);
			this.downloadsTable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.downloadsTable.Name = "downloadsTable";
			this.downloadsTable.ReadOnly = true;
			this.downloadsTable.RowTemplate.Height = 28;
			this.downloadsTable.Size = new System.Drawing.Size(721, 388);
			this.downloadsTable.TabIndex = 3;
			this.downloadsTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.downloadsTable_CellContentClick);
			// 
			// Url
			// 
			this.Url.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Url.HeaderText = "Url";
			this.Url.Name = "Url";
			this.Url.ReadOnly = true;
			this.Url.Width = 45;
			// 
			// Status
			// 
			this.Status.HeaderText = "Status";
			this.Status.Name = "Status";
			this.Status.ReadOnly = true;
			// 
			// Cancel
			// 
			this.Cancel.HeaderText = "Action";
			this.Cancel.Name = "Cancel";
			this.Cancel.ReadOnly = true;
			// 
			// DownloadManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(744, 463);
			this.Controls.Add(this.downloadsTable);
			this.Controls.Add(this.downloadButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.urlTextBox);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "DownloadManager";
			this.Text = "Download Manager";
			((System.ComponentModel.ISupportInitialize)(this.downloadsTable)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox urlTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button downloadButton;
		private System.Windows.Forms.DataGridView downloadsTable;
		private System.Windows.Forms.DataGridViewTextBoxColumn Url;
		private System.Windows.Forms.DataGridViewTextBoxColumn Status;
		private System.Windows.Forms.DataGridViewButtonColumn Cancel;
	}
}

