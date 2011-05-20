namespace Wolfpack.AppStats.Demo
{
    partial class AppStatsDemoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppStatsDemoForm));
            this.uiFinalPanel = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.uiCountStatId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.uiCountTen = new System.Windows.Forms.Button();
            this.uiCount = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.uiTimerStatId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.uiTimerStartStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.uiVote = new System.Windows.Forms.Button();
            this.uiVoteC = new System.Windows.Forms.RadioButton();
            this.uiVoteB = new System.Windows.Forms.RadioButton();
            this.uiVoteA = new System.Windows.Forms.RadioButton();
            this.uiPublisherSelectPanel = new System.Windows.Forms.Panel();
            this.uiPublisherList = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.uiPublisherSelectNext = new System.Windows.Forms.Button();
            this.uiNsbPublisherOptions = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.uiDestinationQueue = new System.Windows.Forms.TextBox();
            this.uiFileQueuePublisherOptions = new System.Windows.Forms.Panel();
            this.uiDestinationFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.uiFinalPanel.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.uiPublisherSelectPanel.SuspendLayout();
            this.uiNsbPublisherOptions.SuspendLayout();
            this.uiFileQueuePublisherOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiFinalPanel
            // 
            this.uiFinalPanel.Controls.Add(this.groupBox3);
            this.uiFinalPanel.Controls.Add(this.groupBox2);
            this.uiFinalPanel.Controls.Add(this.groupBox1);
            this.uiFinalPanel.Location = new System.Drawing.Point(3, 259);
            this.uiFinalPanel.Name = "uiFinalPanel";
            this.uiFinalPanel.Size = new System.Drawing.Size(630, 249);
            this.uiFinalPanel.TabIndex = 15;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.uiCountStatId);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.uiCountTen);
            this.groupBox3.Controls.Add(this.uiCount);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(11, 115);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(311, 126);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Counts";
            // 
            // uiCountStatId
            // 
            this.uiCountStatId.Location = new System.Drawing.Point(76, 66);
            this.uiCountStatId.Name = "uiCountStatId";
            this.uiCountStatId.Size = new System.Drawing.Size(151, 20);
            this.uiCountStatId.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Statistic Id";
            // 
            // uiCountTen
            // 
            this.uiCountTen.Location = new System.Drawing.Point(152, 92);
            this.uiCountTen.Name = "uiCountTen";
            this.uiCountTen.Size = new System.Drawing.Size(75, 23);
            this.uiCountTen.TabIndex = 15;
            this.uiCountTen.Text = "Count (10)";
            this.uiCountTen.UseVisualStyleBackColor = true;
            this.uiCountTen.Click += new System.EventHandler(this.uiCountTen_Click);
            // 
            // uiCount
            // 
            this.uiCount.Location = new System.Drawing.Point(76, 92);
            this.uiCount.Name = "uiCount";
            this.uiCount.Size = new System.Drawing.Size(70, 23);
            this.uiCount.TabIndex = 14;
            this.uiCount.Text = "Count (1)";
            this.uiCount.UseVisualStyleBackColor = true;
            this.uiCount.Click += new System.EventHandler(this.uiCount_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(14, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(291, 40);
            this.label7.TabIndex = 13;
            this.label7.Text = "If you need to count something then use either the One() or Count(N) method";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uiTimerStatId);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.uiTimerStartStop);
            this.groupBox2.Location = new System.Drawing.Point(11, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(311, 104);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Timer";
            // 
            // uiTimerStatId
            // 
            this.uiTimerStatId.Location = new System.Drawing.Point(76, 75);
            this.uiTimerStatId.Name = "uiTimerStatId";
            this.uiTimerStatId.Size = new System.Drawing.Size(127, 20);
            this.uiTimerStatId.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Statistic Id";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(14, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(291, 48);
            this.label6.TabIndex = 12;
            this.label6.Text = "If you want to time something then use an AppStatTimer. This will record the numb" +
                "er of milliseconds against an operation";
            // 
            // uiTimerStartStop
            // 
            this.uiTimerStartStop.Location = new System.Drawing.Point(209, 73);
            this.uiTimerStartStop.Name = "uiTimerStartStop";
            this.uiTimerStartStop.Size = new System.Drawing.Size(75, 23);
            this.uiTimerStartStop.TabIndex = 9;
            this.uiTimerStartStop.Text = "Timer Start";
            this.uiTimerStartStop.UseVisualStyleBackColor = true;
            this.uiTimerStartStop.Click += new System.EventHandler(this.uiTimerStartStop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.uiVote);
            this.groupBox1.Controls.Add(this.uiVoteC);
            this.groupBox1.Controls.Add(this.uiVoteB);
            this.groupBox1.Controls.Add(this.uiVoteA);
            this.groupBox1.Location = new System.Drawing.Point(328, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 240);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vote Example";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Wolfpack is....";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(14, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(272, 82);
            this.label4.TabIndex = 10;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // uiVote
            // 
            this.uiVote.Location = new System.Drawing.Point(211, 206);
            this.uiVote.Name = "uiVote";
            this.uiVote.Size = new System.Drawing.Size(75, 23);
            this.uiVote.TabIndex = 3;
            this.uiVote.Text = "Vote!";
            this.uiVote.UseVisualStyleBackColor = true;
            this.uiVote.Click += new System.EventHandler(this.uiVote_Click);
            // 
            // uiVoteC
            // 
            this.uiVoteC.AutoSize = true;
            this.uiVoteC.Location = new System.Drawing.Point(33, 179);
            this.uiVoteC.Name = "uiVoteC";
            this.uiVoteC.Size = new System.Drawing.Size(109, 17);
            this.uiVoteC.TabIndex = 2;
            this.uiVoteC.Text = "Light years ahead";
            this.uiVoteC.UseVisualStyleBackColor = true;
            // 
            // uiVoteB
            // 
            this.uiVoteB.AutoSize = true;
            this.uiVoteB.Location = new System.Drawing.Point(33, 156);
            this.uiVoteB.Name = "uiVoteB";
            this.uiVoteB.Size = new System.Drawing.Size(131, 17);
            this.uiVoteB.TabIndex = 1;
            this.uiVoteB.Text = "What\'s Wolfpack?";
            this.uiVoteB.UseVisualStyleBackColor = true;
            // 
            // uiVoteA
            // 
            this.uiVoteA.AutoSize = true;
            this.uiVoteA.Checked = true;
            this.uiVoteA.Location = new System.Drawing.Point(33, 133);
            this.uiVoteA.Name = "uiVoteA";
            this.uiVoteA.Size = new System.Drawing.Size(54, 17);
            this.uiVoteA.TabIndex = 0;
            this.uiVoteA.TabStop = true;
            this.uiVoteA.Text = "Great!";
            this.uiVoteA.UseVisualStyleBackColor = true;
            // 
            // uiPublisherSelectPanel
            // 
            this.uiPublisherSelectPanel.Controls.Add(this.uiFileQueuePublisherOptions);
            this.uiPublisherSelectPanel.Controls.Add(this.uiNsbPublisherOptions);
            this.uiPublisherSelectPanel.Controls.Add(this.uiPublisherSelectNext);
            this.uiPublisherSelectPanel.Controls.Add(this.label9);
            this.uiPublisherSelectPanel.Controls.Add(this.uiPublisherList);
            this.uiPublisherSelectPanel.Location = new System.Drawing.Point(3, 2);
            this.uiPublisherSelectPanel.Name = "uiPublisherSelectPanel";
            this.uiPublisherSelectPanel.Size = new System.Drawing.Size(630, 249);
            this.uiPublisherSelectPanel.TabIndex = 16;
            // 
            // uiPublisherList
            // 
            this.uiPublisherList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiPublisherList.FormattingEnabled = true;
            this.uiPublisherList.Location = new System.Drawing.Point(126, 17);
            this.uiPublisherList.Name = "uiPublisherList";
            this.uiPublisherList.Size = new System.Drawing.Size(356, 21);
            this.uiPublisherList.TabIndex = 0;
            this.uiPublisherList.SelectedIndexChanged += new System.EventHandler(this.uiPublisherList_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Select Publisher....";
            // 
            // uiPublisherSelectNext
            // 
            this.uiPublisherSelectNext.Location = new System.Drawing.Point(539, 207);
            this.uiPublisherSelectNext.Name = "uiPublisherSelectNext";
            this.uiPublisherSelectNext.Size = new System.Drawing.Size(75, 23);
            this.uiPublisherSelectNext.TabIndex = 4;
            this.uiPublisherSelectNext.Text = "Next";
            this.uiPublisherSelectNext.UseVisualStyleBackColor = true;
            this.uiPublisherSelectNext.Click += new System.EventHandler(this.uiPublisherSelectNext_Click);
            // 
            // uiNsbPublisherOptions
            // 
            this.uiNsbPublisherOptions.Controls.Add(this.uiDestinationQueue);
            this.uiNsbPublisherOptions.Controls.Add(this.label2);
            this.uiNsbPublisherOptions.Location = new System.Drawing.Point(6, 53);
            this.uiNsbPublisherOptions.Name = "uiNsbPublisherOptions";
            this.uiNsbPublisherOptions.Size = new System.Drawing.Size(608, 51);
            this.uiNsbPublisherOptions.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Destination Queue";
            // 
            // uiDestinationQueue
            // 
            this.uiDestinationQueue.Location = new System.Drawing.Point(120, 13);
            this.uiDestinationQueue.Name = "uiDestinationQueue";
            this.uiDestinationQueue.Size = new System.Drawing.Size(356, 20);
            this.uiDestinationQueue.TabIndex = 2;
            // 
            // uiFileQueuePublisherOptions
            // 
            this.uiFileQueuePublisherOptions.Controls.Add(this.uiDestinationFolder);
            this.uiFileQueuePublisherOptions.Controls.Add(this.label1);
            this.uiFileQueuePublisherOptions.Location = new System.Drawing.Point(6, 114);
            this.uiFileQueuePublisherOptions.Name = "uiFileQueuePublisherOptions";
            this.uiFileQueuePublisherOptions.Size = new System.Drawing.Size(608, 51);
            this.uiFileQueuePublisherOptions.TabIndex = 19;
            // 
            // uiDestinationFolder
            // 
            this.uiDestinationFolder.Location = new System.Drawing.Point(120, 13);
            this.uiDestinationFolder.Name = "uiDestinationFolder";
            this.uiDestinationFolder.Size = new System.Drawing.Size(356, 20);
            this.uiDestinationFolder.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Destination Folder";
            // 
            // AppStatsDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 512);
            this.Controls.Add(this.uiPublisherSelectPanel);
            this.Controls.Add(this.uiFinalPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AppStatsDemoForm";
            this.Text = "Wolfpack AppStats Demo";
            this.uiFinalPanel.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.uiPublisherSelectPanel.ResumeLayout(false);
            this.uiPublisherSelectPanel.PerformLayout();
            this.uiNsbPublisherOptions.ResumeLayout(false);
            this.uiNsbPublisherOptions.PerformLayout();
            this.uiFileQueuePublisherOptions.ResumeLayout(false);
            this.uiFileQueuePublisherOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel uiFinalPanel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox uiCountStatId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button uiCountTen;
        private System.Windows.Forms.Button uiCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox uiTimerStatId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button uiTimerStartStop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button uiVote;
        private System.Windows.Forms.RadioButton uiVoteC;
        private System.Windows.Forms.RadioButton uiVoteB;
        private System.Windows.Forms.RadioButton uiVoteA;
        private System.Windows.Forms.Panel uiPublisherSelectPanel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox uiPublisherList;
        private System.Windows.Forms.Button uiPublisherSelectNext;
        private System.Windows.Forms.Panel uiNsbPublisherOptions;
        private System.Windows.Forms.TextBox uiDestinationQueue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel uiFileQueuePublisherOptions;
        private System.Windows.Forms.TextBox uiDestinationFolder;
        private System.Windows.Forms.Label label1;
    }
}

