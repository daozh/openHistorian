﻿namespace MigrationUtility
{
    partial class MigrationUtility
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MigrationUtility));
            this.labelDestinationFilesLocation = new System.Windows.Forms.Label();
            this.textBoxDestinationFiles = new System.Windows.Forms.TextBox();
            this.buttonOpenDestinationFilesLocation = new System.Windows.Forms.Button();
            this.groupBoxSourceFiles = new System.Windows.Forms.GroupBox();
            this.checkBoxIgnoreDuplicateKeys = new System.Windows.Forms.CheckBox();
            this.buttonOpenSourceOffloadedFilesLocation = new System.Windows.Forms.Button();
            this.labelOffloadedFileLocation = new System.Windows.Forms.Label();
            this.textBoxSourceOffloadedFiles = new System.Windows.Forms.TextBox();
            this.buttonOpenSourceFilesLocation = new System.Windows.Forms.Button();
            this.labelSourceFilesLocation = new System.Windows.Forms.Label();
            this.textBoxSourceFiles = new System.Windows.Forms.TextBox();
            this.labelDuplicatesIgnored = new System.Windows.Forms.Label();
            this.groupBoxDestinationOptions = new System.Windows.Forms.GroupBox();
            this.radioButtonCompareArchives = new System.Windows.Forms.RadioButton();
            this.radioButtonFastMigration = new System.Windows.Forms.RadioButton();
            this.radioButtonLiveMigration = new System.Windows.Forms.RadioButton();
            this.labelGigabytes = new System.Windows.Forms.Label();
            this.labelTargetFileSize = new System.Windows.Forms.Label();
            this.textBoxTargetFileSize = new System.Windows.Forms.TextBox();
            this.labelDirectoryNamingMode = new System.Windows.Forms.Label();
            this.comboBoxDirectoryNamingMode = new System.Windows.Forms.ComboBox();
            this.labelInstanceName = new System.Windows.Forms.Label();
            this.textBoxInstanceName = new System.Windows.Forms.TextBox();
            this.labelThreads = new System.Windows.Forms.Label();
            this.labelMaxParallelism = new System.Windows.Forms.Label();
            this.textBoxMaxThreads = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxMessages = new System.Windows.Forms.GroupBox();
            this.textBoxMessageOutput = new System.Windows.Forms.TextBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxSourceFiles.SuspendLayout();
            this.groupBoxDestinationOptions.SuspendLayout();
            this.groupBoxMessages.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelDestinationFilesLocation
            // 
            this.labelDestinationFilesLocation.AutoSize = true;
            this.labelDestinationFilesLocation.Location = new System.Drawing.Point(15, 33);
            this.labelDestinationFilesLocation.Name = "labelDestinationFilesLocation";
            this.labelDestinationFilesLocation.Size = new System.Drawing.Size(127, 13);
            this.labelDestinationFilesLocation.TabIndex = 0;
            this.labelDestinationFilesLocation.Text = "&Destination files location:";
            this.labelDestinationFilesLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDestinationFiles
            // 
            this.textBoxDestinationFiles.Location = new System.Drawing.Point(148, 30);
            this.textBoxDestinationFiles.Name = "textBoxDestinationFiles";
            this.textBoxDestinationFiles.Size = new System.Drawing.Size(281, 21);
            this.textBoxDestinationFiles.TabIndex = 1;
            this.textBoxDestinationFiles.TextChanged += new System.EventHandler(this.textBoxDestinationFiles_TextChanged);
            // 
            // buttonOpenDestinationFilesLocation
            // 
            this.buttonOpenDestinationFilesLocation.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOpenDestinationFilesLocation.Location = new System.Drawing.Point(429, 29);
            this.buttonOpenDestinationFilesLocation.Name = "buttonOpenDestinationFilesLocation";
            this.buttonOpenDestinationFilesLocation.Size = new System.Drawing.Size(28, 23);
            this.buttonOpenDestinationFilesLocation.TabIndex = 2;
            this.buttonOpenDestinationFilesLocation.Text = "...";
            this.buttonOpenDestinationFilesLocation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonOpenDestinationFilesLocation.UseVisualStyleBackColor = true;
            this.buttonOpenDestinationFilesLocation.Click += new System.EventHandler(this.buttonOpenDestinationFilesLocation_Click);
            // 
            // groupBoxSourceFiles
            // 
            this.groupBoxSourceFiles.Controls.Add(this.checkBoxIgnoreDuplicateKeys);
            this.groupBoxSourceFiles.Controls.Add(this.buttonOpenSourceOffloadedFilesLocation);
            this.groupBoxSourceFiles.Controls.Add(this.labelOffloadedFileLocation);
            this.groupBoxSourceFiles.Controls.Add(this.textBoxSourceOffloadedFiles);
            this.groupBoxSourceFiles.Controls.Add(this.buttonOpenSourceFilesLocation);
            this.groupBoxSourceFiles.Controls.Add(this.labelSourceFilesLocation);
            this.groupBoxSourceFiles.Controls.Add(this.textBoxSourceFiles);
            this.groupBoxSourceFiles.Controls.Add(this.labelDuplicatesIgnored);
            this.groupBoxSourceFiles.Location = new System.Drawing.Point(15, 10);
            this.groupBoxSourceFiles.Name = "groupBoxSourceFiles";
            this.groupBoxSourceFiles.Size = new System.Drawing.Size(470, 122);
            this.groupBoxSourceFiles.TabIndex = 0;
            this.groupBoxSourceFiles.TabStop = false;
            this.groupBoxSourceFiles.Text = "openHistorian 1.0 / DatAWare Source Files:";
            // 
            // checkBoxIgnoreDuplicateKeys
            // 
            this.checkBoxIgnoreDuplicateKeys.AutoSize = true;
            this.checkBoxIgnoreDuplicateKeys.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxIgnoreDuplicateKeys.Location = new System.Drawing.Point(24, 86);
            this.checkBoxIgnoreDuplicateKeys.Name = "checkBoxIgnoreDuplicateKeys";
            this.checkBoxIgnoreDuplicateKeys.Size = new System.Drawing.Size(139, 17);
            this.checkBoxIgnoreDuplicateKeys.TabIndex = 6;
            this.checkBoxIgnoreDuplicateKeys.Text = "I&gnore duplicates keys?";
            this.checkBoxIgnoreDuplicateKeys.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreDuplicateKeys.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreDuplicateKeys_CheckedChanged);
            // 
            // buttonOpenSourceOffloadedFilesLocation
            // 
            this.buttonOpenSourceOffloadedFilesLocation.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOpenSourceOffloadedFilesLocation.Location = new System.Drawing.Point(429, 55);
            this.buttonOpenSourceOffloadedFilesLocation.Name = "buttonOpenSourceOffloadedFilesLocation";
            this.buttonOpenSourceOffloadedFilesLocation.Size = new System.Drawing.Size(28, 23);
            this.buttonOpenSourceOffloadedFilesLocation.TabIndex = 5;
            this.buttonOpenSourceOffloadedFilesLocation.Text = "...";
            this.buttonOpenSourceOffloadedFilesLocation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonOpenSourceOffloadedFilesLocation.UseVisualStyleBackColor = true;
            this.buttonOpenSourceOffloadedFilesLocation.Click += new System.EventHandler(this.buttonOpenSourceOffloadedFilesLocation_Click);
            // 
            // labelOffloadedFileLocation
            // 
            this.labelOffloadedFileLocation.AutoSize = true;
            this.labelOffloadedFileLocation.Location = new System.Drawing.Point(25, 59);
            this.labelOffloadedFileLocation.Name = "labelOffloadedFileLocation";
            this.labelOffloadedFileLocation.Size = new System.Drawing.Size(121, 13);
            this.labelOffloadedFileLocation.TabIndex = 3;
            this.labelOffloadedFileLocation.Text = "&Offloaded files location:";
            this.labelOffloadedFileLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSourceOffloadedFiles
            // 
            this.textBoxSourceOffloadedFiles.Location = new System.Drawing.Point(148, 56);
            this.textBoxSourceOffloadedFiles.Name = "textBoxSourceOffloadedFiles";
            this.textBoxSourceOffloadedFiles.Size = new System.Drawing.Size(281, 21);
            this.textBoxSourceOffloadedFiles.TabIndex = 4;
            // 
            // buttonOpenSourceFilesLocation
            // 
            this.buttonOpenSourceFilesLocation.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOpenSourceFilesLocation.Location = new System.Drawing.Point(429, 29);
            this.buttonOpenSourceFilesLocation.Name = "buttonOpenSourceFilesLocation";
            this.buttonOpenSourceFilesLocation.Size = new System.Drawing.Size(28, 23);
            this.buttonOpenSourceFilesLocation.TabIndex = 2;
            this.buttonOpenSourceFilesLocation.Text = "...";
            this.buttonOpenSourceFilesLocation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonOpenSourceFilesLocation.UseVisualStyleBackColor = true;
            this.buttonOpenSourceFilesLocation.Click += new System.EventHandler(this.buttonOpenSourceFilesLocation_Click);
            // 
            // labelSourceFilesLocation
            // 
            this.labelSourceFilesLocation.AutoSize = true;
            this.labelSourceFilesLocation.Location = new System.Drawing.Point(40, 33);
            this.labelSourceFilesLocation.Name = "labelSourceFilesLocation";
            this.labelSourceFilesLocation.Size = new System.Drawing.Size(106, 13);
            this.labelSourceFilesLocation.TabIndex = 0;
            this.labelSourceFilesLocation.Text = "&Source files location:";
            this.labelSourceFilesLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSourceFiles
            // 
            this.textBoxSourceFiles.Location = new System.Drawing.Point(148, 30);
            this.textBoxSourceFiles.Name = "textBoxSourceFiles";
            this.textBoxSourceFiles.Size = new System.Drawing.Size(281, 21);
            this.textBoxSourceFiles.TabIndex = 1;
            this.textBoxSourceFiles.TextChanged += new System.EventHandler(this.textBoxSourceFiles_TextChanged);
            // 
            // labelDuplicatesIgnored
            // 
            this.labelDuplicatesIgnored.AutoSize = true;
            this.labelDuplicatesIgnored.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDuplicatesIgnored.Location = new System.Drawing.Point(168, 87);
            this.labelDuplicatesIgnored.Name = "labelDuplicatesIgnored";
            this.labelDuplicatesIgnored.Size = new System.Drawing.Size(285, 13);
            this.labelDuplicatesIgnored.TabIndex = 7;
            this.labelDuplicatesIgnored.Text = "Any duplicated timestamp / point ID tuples will be ignored.";
            this.labelDuplicatesIgnored.Visible = false;
            // 
            // groupBoxDestinationOptions
            // 
            this.groupBoxDestinationOptions.Controls.Add(this.radioButtonCompareArchives);
            this.groupBoxDestinationOptions.Controls.Add(this.radioButtonFastMigration);
            this.groupBoxDestinationOptions.Controls.Add(this.radioButtonLiveMigration);
            this.groupBoxDestinationOptions.Controls.Add(this.labelGigabytes);
            this.groupBoxDestinationOptions.Controls.Add(this.labelTargetFileSize);
            this.groupBoxDestinationOptions.Controls.Add(this.textBoxTargetFileSize);
            this.groupBoxDestinationOptions.Controls.Add(this.labelDirectoryNamingMode);
            this.groupBoxDestinationOptions.Controls.Add(this.comboBoxDirectoryNamingMode);
            this.groupBoxDestinationOptions.Controls.Add(this.labelInstanceName);
            this.groupBoxDestinationOptions.Controls.Add(this.textBoxInstanceName);
            this.groupBoxDestinationOptions.Controls.Add(this.textBoxDestinationFiles);
            this.groupBoxDestinationOptions.Controls.Add(this.labelDestinationFilesLocation);
            this.groupBoxDestinationOptions.Controls.Add(this.buttonOpenDestinationFilesLocation);
            this.groupBoxDestinationOptions.Controls.Add(this.labelThreads);
            this.groupBoxDestinationOptions.Controls.Add(this.labelMaxParallelism);
            this.groupBoxDestinationOptions.Controls.Add(this.textBoxMaxThreads);
            this.groupBoxDestinationOptions.Location = new System.Drawing.Point(15, 138);
            this.groupBoxDestinationOptions.Name = "groupBoxDestinationOptions";
            this.groupBoxDestinationOptions.Size = new System.Drawing.Size(470, 153);
            this.groupBoxDestinationOptions.TabIndex = 1;
            this.groupBoxDestinationOptions.TabStop = false;
            this.groupBoxDestinationOptions.Text = "openHistorian 2.0 Destination Options:";
            // 
            // radioButtonCompareArchives
            // 
            this.radioButtonCompareArchives.AutoSize = true;
            this.radioButtonCompareArchives.Location = new System.Drawing.Point(291, 116);
            this.radioButtonCompareArchives.Name = "radioButtonCompareArchives";
            this.radioButtonCompareArchives.Size = new System.Drawing.Size(112, 17);
            this.radioButtonCompareArchives.TabIndex = 15;
            this.radioButtonCompareArchives.Text = "&Compare Archives";
            this.radioButtonCompareArchives.UseVisualStyleBackColor = true;
            this.radioButtonCompareArchives.CheckedChanged += new System.EventHandler(this.radioButtonCompareArchives_CheckedChanged);
            // 
            // radioButtonFastMigration
            // 
            this.radioButtonFastMigration.AutoSize = true;
            this.radioButtonFastMigration.Location = new System.Drawing.Point(291, 89);
            this.radioButtonFastMigration.Name = "radioButtonFastMigration";
            this.radioButtonFastMigration.Size = new System.Drawing.Size(152, 17);
            this.radioButtonFastMigration.TabIndex = 14;
            this.radioButtonFastMigration.Text = "&Fast Migration (file-by-file)";
            this.radioButtonFastMigration.UseVisualStyleBackColor = true;
            this.radioButtonFastMigration.CheckedChanged += new System.EventHandler(this.radioButtonFastMigration_CheckedChanged);
            // 
            // radioButtonLiveMigration
            // 
            this.radioButtonLiveMigration.AutoSize = true;
            this.radioButtonLiveMigration.Location = new System.Drawing.Point(291, 62);
            this.radioButtonLiveMigration.Name = "radioButtonLiveMigration";
            this.radioButtonLiveMigration.Size = new System.Drawing.Size(166, 17);
            this.radioButtonLiveMigration.TabIndex = 13;
            this.radioButtonLiveMigration.Text = "&Live Migration (roll-over safe)";
            this.radioButtonLiveMigration.UseVisualStyleBackColor = true;
            this.radioButtonLiveMigration.CheckedChanged += new System.EventHandler(this.radioButtonLiveMigration_CheckedChanged);
            // 
            // labelGigabytes
            // 
            this.labelGigabytes.AutoSize = true;
            this.labelGigabytes.Location = new System.Drawing.Point(214, 118);
            this.labelGigabytes.Name = "labelGigabytes";
            this.labelGigabytes.Size = new System.Drawing.Size(55, 13);
            this.labelGigabytes.TabIndex = 9;
            this.labelGigabytes.Text = "Gigabytes";
            this.labelGigabytes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTargetFileSize
            // 
            this.labelTargetFileSize.AutoSize = true;
            this.labelTargetFileSize.Location = new System.Drawing.Point(61, 119);
            this.labelTargetFileSize.Name = "labelTargetFileSize";
            this.labelTargetFileSize.Size = new System.Drawing.Size(81, 13);
            this.labelTargetFileSize.TabIndex = 7;
            this.labelTargetFileSize.Text = "&Target file size:";
            this.labelTargetFileSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTargetFileSize
            // 
            this.textBoxTargetFileSize.Location = new System.Drawing.Point(148, 115);
            this.textBoxTargetFileSize.Name = "textBoxTargetFileSize";
            this.textBoxTargetFileSize.Size = new System.Drawing.Size(60, 21);
            this.textBoxTargetFileSize.TabIndex = 8;
            this.textBoxTargetFileSize.Text = "1.5";
            // 
            // labelDirectoryNamingMode
            // 
            this.labelDirectoryNamingMode.AutoSize = true;
            this.labelDirectoryNamingMode.Location = new System.Drawing.Point(21, 89);
            this.labelDirectoryNamingMode.Name = "labelDirectoryNamingMode";
            this.labelDirectoryNamingMode.Size = new System.Drawing.Size(121, 13);
            this.labelDirectoryNamingMode.TabIndex = 5;
            this.labelDirectoryNamingMode.Text = "Directory &naming mode:";
            this.labelDirectoryNamingMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxDirectoryNamingMode
            // 
            this.comboBoxDirectoryNamingMode.FormattingEnabled = true;
            this.comboBoxDirectoryNamingMode.Location = new System.Drawing.Point(148, 86);
            this.comboBoxDirectoryNamingMode.Name = "comboBoxDirectoryNamingMode";
            this.comboBoxDirectoryNamingMode.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDirectoryNamingMode.TabIndex = 6;
            // 
            // labelInstanceName
            // 
            this.labelInstanceName.AutoSize = true;
            this.labelInstanceName.Location = new System.Drawing.Point(11, 61);
            this.labelInstanceName.Name = "labelInstanceName";
            this.labelInstanceName.Size = new System.Drawing.Size(131, 13);
            this.labelInstanceName.TabIndex = 3;
            this.labelInstanceName.Text = "&Instance name (optional):";
            this.labelInstanceName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxInstanceName
            // 
            this.textBoxInstanceName.Location = new System.Drawing.Point(148, 58);
            this.textBoxInstanceName.Name = "textBoxInstanceName";
            this.textBoxInstanceName.Size = new System.Drawing.Size(121, 21);
            this.textBoxInstanceName.TabIndex = 4;
            // 
            // labelThreads
            // 
            this.labelThreads.AutoSize = true;
            this.labelThreads.Location = new System.Drawing.Point(214, 119);
            this.labelThreads.Name = "labelThreads";
            this.labelThreads.Size = new System.Drawing.Size(46, 13);
            this.labelThreads.TabIndex = 12;
            this.labelThreads.Text = "Threads";
            this.labelThreads.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelThreads.Visible = false;
            // 
            // labelMaxParallelism
            // 
            this.labelMaxParallelism.AutoSize = true;
            this.labelMaxParallelism.Location = new System.Drawing.Point(59, 119);
            this.labelMaxParallelism.Name = "labelMaxParallelism";
            this.labelMaxParallelism.Size = new System.Drawing.Size(83, 13);
            this.labelMaxParallelism.TabIndex = 10;
            this.labelMaxParallelism.Text = "&Max parallelism:";
            this.labelMaxParallelism.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelMaxParallelism.Visible = false;
            // 
            // textBoxMaxThreads
            // 
            this.textBoxMaxThreads.Location = new System.Drawing.Point(148, 115);
            this.textBoxMaxThreads.Name = "textBoxMaxThreads";
            this.textBoxMaxThreads.Size = new System.Drawing.Size(60, 21);
            this.textBoxMaxThreads.TabIndex = 11;
            this.textBoxMaxThreads.Text = "2";
            this.toolTip.SetToolTip(this.textBoxMaxThreads, "For fast SSD drives, increased thread count may improve performance");
            this.textBoxMaxThreads.Visible = false;
            // 
            // groupBoxMessages
            // 
            this.groupBoxMessages.Controls.Add(this.textBoxMessageOutput);
            this.groupBoxMessages.Location = new System.Drawing.Point(15, 297);
            this.groupBoxMessages.Name = "groupBoxMessages";
            this.groupBoxMessages.Size = new System.Drawing.Size(470, 204);
            this.groupBoxMessages.TabIndex = 2;
            this.groupBoxMessages.TabStop = false;
            this.groupBoxMessages.Text = "Messages";
            // 
            // textBoxMessageOutput
            // 
            this.textBoxMessageOutput.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxMessageOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessageOutput.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageOutput.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxMessageOutput.Location = new System.Drawing.Point(3, 17);
            this.textBoxMessageOutput.Multiline = true;
            this.textBoxMessageOutput.Name = "textBoxMessageOutput";
            this.textBoxMessageOutput.ReadOnly = true;
            this.textBoxMessageOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessageOutput.Size = new System.Drawing.Size(464, 184);
            this.textBoxMessageOutput.TabIndex = 0;
            this.textBoxMessageOutput.TabStop = false;
            // 
            // buttonGo
            // 
            this.buttonGo.Enabled = false;
            this.buttonGo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGo.Location = new System.Drawing.Point(410, 507);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 3;
            this.buttonGo.Text = "&Go!";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(18, 507);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(386, 23);
            this.progressBar.TabIndex = 4;
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.ReshowDelay = 40;
            // 
            // MigrationUtility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 537);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.groupBoxMessages);
            this.Controls.Add(this.groupBoxDestinationOptions);
            this.Controls.Add(this.groupBoxSourceFiles);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MigrationUtility";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Historian Migration Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MigrationUtility_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MigrationUtility_FormClosed);
            this.Load += new System.EventHandler(this.MigrationUtility_Load);
            this.groupBoxSourceFiles.ResumeLayout(false);
            this.groupBoxSourceFiles.PerformLayout();
            this.groupBoxDestinationOptions.ResumeLayout(false);
            this.groupBoxDestinationOptions.PerformLayout();
            this.groupBoxMessages.ResumeLayout(false);
            this.groupBoxMessages.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelDestinationFilesLocation;
        private System.Windows.Forms.TextBox textBoxDestinationFiles;
        private System.Windows.Forms.Button buttonOpenDestinationFilesLocation;
        private System.Windows.Forms.GroupBox groupBoxSourceFiles;
        private System.Windows.Forms.Button buttonOpenSourceOffloadedFilesLocation;
        private System.Windows.Forms.Label labelOffloadedFileLocation;
        private System.Windows.Forms.TextBox textBoxSourceOffloadedFiles;
        private System.Windows.Forms.Button buttonOpenSourceFilesLocation;
        private System.Windows.Forms.Label labelSourceFilesLocation;
        private System.Windows.Forms.TextBox textBoxSourceFiles;
        private System.Windows.Forms.GroupBox groupBoxDestinationOptions;
        private System.Windows.Forms.Label labelDirectoryNamingMode;
        private System.Windows.Forms.ComboBox comboBoxDirectoryNamingMode;
        private System.Windows.Forms.Label labelInstanceName;
        private System.Windows.Forms.TextBox textBoxInstanceName;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label labelGigabytes;
        private System.Windows.Forms.Label labelTargetFileSize;
        private System.Windows.Forms.TextBox textBoxTargetFileSize;
        private System.Windows.Forms.GroupBox groupBoxMessages;
        private System.Windows.Forms.TextBox textBoxMessageOutput;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.RadioButton radioButtonFastMigration;
        private System.Windows.Forms.RadioButton radioButtonLiveMigration;
        private System.Windows.Forms.RadioButton radioButtonCompareArchives;
        private System.Windows.Forms.Label labelDuplicatesIgnored;
        private System.Windows.Forms.CheckBox checkBoxIgnoreDuplicateKeys;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelThreads;
        private System.Windows.Forms.Label labelMaxParallelism;
        private System.Windows.Forms.TextBox textBoxMaxThreads;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

