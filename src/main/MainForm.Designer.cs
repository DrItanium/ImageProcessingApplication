namespace ImageProcessingApplication 
{
    partial class MainForm
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
#if !MONO_NET
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
#endif
						this.source = new ImageProcessingApplication.ImageDisplayPanel();
						this.result= new ImageProcessingApplication.ImageDisplayPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultImageOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bothSourceAndResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transposeImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
						this.saveResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
						this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.reloadMenuStripItem1 = new System.Windows.Forms.ToolStripMenuItem();
						this.desaturateMenuStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
#if !MONO_NET
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
#endif

#if !MONO_NET 
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
#endif
            this.SuspendLayout();
            //
            // reloadMenuStripItem1
            // 
            this.reloadMenuStripItem1.Name = "reloadMenuStripItem1";
            this.reloadMenuStripItem1.Size = new System.Drawing.Size(42, 20);
            this.reloadMenuStripItem1.Text = "Reload Plugins";
            this.reloadMenuStripItem1.Click += new System.EventHandler(this.onReloadClicked);
						//
						// source
						//
						this.source.Location = new System.Drawing.Point(0,45);
						this.source.Size = new System.Drawing.Size(500, 718);

						//
						// result 
						//
						this.result.Location = new System.Drawing.Point(512,45);
						this.result.Size = new System.Drawing.Size(500, 718);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.filtersToolStripMenuItem,
            this.reloadMenuStripItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1024, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.toolStripSeparator1,
						this.saveResultToolStripMenuItem,
            this.quitToolStripMenuItem,
            this.quitToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resultImageOnlyToolStripMenuItem,
            this.bothSourceAndResultToolStripMenuItem});
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // resultImageOnlyToolStripMenuItem
            // 
            this.resultImageOnlyToolStripMenuItem.Name = "resultImageOnlyToolStripMenuItem";
            this.resultImageOnlyToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.resultImageOnlyToolStripMenuItem.Text = "Result Image Only";
            this.resultImageOnlyToolStripMenuItem.Click += new System.EventHandler(this.resultImageOnlyToolStripMenuItem_Click);
            // 
            // bothSourceAndResultToolStripMenuItem
            // 
            this.bothSourceAndResultToolStripMenuItem.Name = "bothSourceAndResultToolStripMenuItem";
            this.bothSourceAndResultToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.bothSourceAndResultToolStripMenuItem.Text = "Both Source and Result";
            this.bothSourceAndResultToolStripMenuItem.Click += new System.EventHandler(this.bothSourceAndResultToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(111, 22);
            this.toolStripSeparator1.Text = "Open";
            this.toolStripSeparator1.Click += new System.EventHandler(this.toolStripSeparator1_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(108, 6);
            // 
            // quitToolStripMenuItem1
            // 
            this.quitToolStripMenuItem1.Name = "quitToolStripMenuItem1";
            this.quitToolStripMenuItem1.Size = new System.Drawing.Size(111, 22);
            this.quitToolStripMenuItem1.Text = "Quit";
            this.quitToolStripMenuItem1.Click += new System.EventHandler(this.quitToolStripMenuItem1_Click);
						//
						// saveResultToolStripMenuItem
						// 
            this.saveResultToolStripMenuItem.Name = "saveResultToolStripMenuItem";
            this.saveResultToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.saveResultToolStripMenuItem.Text = "Save";
            this.saveResultToolStripMenuItem.Click += new System.EventHandler(this.SaveResultantImage);
            // 
            // filtersToolStripMenuItem
            // 
            this.filtersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.transposeImagesToolStripMenuItem,
						this.desaturateMenuStripItem,
            this.toolStripSeparator2});
            this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
            this.filtersToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.filtersToolStripMenuItem.Text = "Filters";
						//
						// desaturateMenuStripItem
						//
            this.desaturateMenuStripItem.Name = "desaturateMenuStripItem";
            this.desaturateMenuStripItem.Size = new System.Drawing.Size(183, 22);
            this.desaturateMenuStripItem.Text = "Desaturate Image";
            this.desaturateMenuStripItem.Click += new System.EventHandler(this.Desaturate);
            // 
            // transposeImagesToolStripMenuItem
            // 
            this.transposeImagesToolStripMenuItem.Name = "transposeImagesToolStripMenuItem";
            this.transposeImagesToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.transposeImagesToolStripMenuItem.Text = "Transpose Images";
            this.transposeImagesToolStripMenuItem.Click += new System.EventHandler(this.transposeImagesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(180, 6);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveFile);
            // 
            // Form1
            // 
           // this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           // this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.menuStrip1);
						this.Controls.Add(this.source);
						this.Controls.Add(this.result);
#if !MONO_NET
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
#endif
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Let\'s Apply Some Filters!";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
#if !MONO_NET
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
#endif
#if !MONO_NET 
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
#endif
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
				private ImageProcessingApplication.ImageDisplayPanel source, result;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
				private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem resultImageOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bothSourceAndResultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transposeImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
				private System.Windows.Forms.ToolStripMenuItem saveResultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadMenuStripItem1;
				private System.Windows.Forms.ToolStripMenuItem desaturateMenuStripItem;
    }
}

