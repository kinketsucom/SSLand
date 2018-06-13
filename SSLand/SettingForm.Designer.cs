namespace SSLand
{
    partial class SettingForm
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
            this.photosizeNumericDown = new System.Windows.Forms.NumericUpDown();
            this.autoscrollCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.useBrandCheckBox = new System.Windows.Forms.CheckBox();
            this.brandComboBox = new System.Windows.Forms.ComboBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.brandAddButton = new System.Windows.Forms.Button();
            this.brandDelButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.vpassPasswordTextbox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cardLastNameTextBox = new System.Windows.Forms.TextBox();
            this.cardFirstNameTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.securityCodeTextBox = new System.Windows.Forms.TextBox();
            this.expireYearTextBox = new System.Windows.Forms.TextBox();
            this.expireMonthTextBox = new System.Windows.Forms.TextBox();
            this.cardNumberTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.showPromptBeforeBuyCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.photosizeNumericDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // photosizeNumericDown
            // 
            this.photosizeNumericDown.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.photosizeNumericDown.Location = new System.Drawing.Point(87, 243);
            this.photosizeNumericDown.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.photosizeNumericDown.Minimum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.photosizeNumericDown.Name = "photosizeNumericDown";
            this.photosizeNumericDown.Size = new System.Drawing.Size(79, 25);
            this.photosizeNumericDown.TabIndex = 16;
            this.photosizeNumericDown.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            // 
            // autoscrollCheckBox
            // 
            this.autoscrollCheckBox.AutoSize = true;
            this.autoscrollCheckBox.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoscrollCheckBox.Location = new System.Drawing.Point(12, 12);
            this.autoscrollCheckBox.Name = "autoscrollCheckBox";
            this.autoscrollCheckBox.Size = new System.Drawing.Size(288, 24);
            this.autoscrollCheckBox.TabIndex = 15;
            this.autoscrollCheckBox.Text = "新しい商品があれば自動でスクロールを移動";
            this.autoscrollCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(8, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "写真サイズ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(8, 568);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 20);
            this.label3.TabIndex = 19;
            this.label3.Text = "※設定は閉じると自動保存されます";
            // 
            // useBrandCheckBox
            // 
            this.useBrandCheckBox.AutoSize = true;
            this.useBrandCheckBox.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.useBrandCheckBox.Location = new System.Drawing.Point(12, 64);
            this.useBrandCheckBox.Name = "useBrandCheckBox";
            this.useBrandCheckBox.Size = new System.Drawing.Size(145, 24);
            this.useBrandCheckBox.TabIndex = 25;
            this.useBrandCheckBox.Text = "ブランドを指定する";
            this.useBrandCheckBox.UseVisualStyleBackColor = true;
            this.useBrandCheckBox.CheckedChanged += new System.EventHandler(this.useBrandCheckBox_CheckedChanged);
            // 
            // brandComboBox
            // 
            this.brandComboBox.FormattingEnabled = true;
            this.brandComboBox.Location = new System.Drawing.Point(12, 94);
            this.brandComboBox.Name = "brandComboBox";
            this.brandComboBox.Size = new System.Drawing.Size(419, 20);
            this.brandComboBox.TabIndex = 26;
            this.brandComboBox.Text = "未指定";
            this.brandComboBox.Visible = false;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 119);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(419, 112);
            this.listBox1.TabIndex = 27;
            // 
            // brandAddButton
            // 
            this.brandAddButton.Location = new System.Drawing.Point(437, 94);
            this.brandAddButton.Margin = new System.Windows.Forms.Padding(2);
            this.brandAddButton.Name = "brandAddButton";
            this.brandAddButton.Size = new System.Drawing.Size(84, 19);
            this.brandAddButton.TabIndex = 28;
            this.brandAddButton.Text = "指定する";
            this.brandAddButton.UseVisualStyleBackColor = true;
            this.brandAddButton.Visible = false;
            this.brandAddButton.Click += new System.EventHandler(this.brandAddButton_Click);
            // 
            // brandDelButton
            // 
            this.brandDelButton.Location = new System.Drawing.Point(436, 186);
            this.brandDelButton.Margin = new System.Windows.Forms.Padding(2);
            this.brandDelButton.Name = "brandDelButton";
            this.brandDelButton.Size = new System.Drawing.Size(84, 44);
            this.brandDelButton.TabIndex = 29;
            this.brandDelButton.Text = "選択ブランドを削除する";
            this.brandDelButton.UseVisualStyleBackColor = true;
            this.brandDelButton.Click += new System.EventHandler(this.brandDelButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.vpassPasswordTextbox);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cardLastNameTextBox);
            this.groupBox1.Controls.Add(this.cardFirstNameTextBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.securityCodeTextBox);
            this.groupBox1.Controls.Add(this.expireYearTextBox);
            this.groupBox1.Controls.Add(this.expireMonthTextBox);
            this.groupBox1.Controls.Add(this.cardNumberTextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(12, 274);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(508, 291);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "支払い方法";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(3, 220);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(176, 20);
            this.label11.TabIndex = 37;
            this.label11.Text = "VPassパーソナルメッセージ";
            // 
            // vpassPasswordTextbox
            // 
            this.vpassPasswordTextbox.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.vpassPasswordTextbox.Location = new System.Drawing.Point(183, 218);
            this.vpassPasswordTextbox.Name = "vpassPasswordTextbox";
            this.vpassPasswordTextbox.Size = new System.Drawing.Size(155, 19);
            this.vpassPasswordTextbox.TabIndex = 36;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(344, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 20);
            this.label10.TabIndex = 31;
            this.label10.Text = "VISAのみ対応";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(67, 198);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 20);
            this.label9.TabIndex = 35;
            this.label9.Text = "英字名義（名）";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(71, 172);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 20);
            this.label8.TabIndex = 34;
            this.label8.Text = "英字名義（性）";
            // 
            // cardLastNameTextBox
            // 
            this.cardLastNameTextBox.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cardLastNameTextBox.Location = new System.Drawing.Point(183, 169);
            this.cardLastNameTextBox.Name = "cardLastNameTextBox";
            this.cardLastNameTextBox.Size = new System.Drawing.Size(155, 19);
            this.cardLastNameTextBox.TabIndex = 33;
            // 
            // cardFirstNameTextBox
            // 
            this.cardFirstNameTextBox.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cardFirstNameTextBox.Location = new System.Drawing.Point(183, 193);
            this.cardFirstNameTextBox.Name = "cardFirstNameTextBox";
            this.cardFirstNameTextBox.Size = new System.Drawing.Size(155, 19);
            this.cardFirstNameTextBox.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(-1, 248);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(503, 40);
            this.label7.TabIndex = 31;
            this.label7.Text = "現在の仕様ではアカウントに既に住所が登録されている必要があります。\r\n複数の住所がアカウントに登録されている場合、デフォルトのものが使用されます。";
            // 
            // securityCodeTextBox
            // 
            this.securityCodeTextBox.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.securityCodeTextBox.Location = new System.Drawing.Point(183, 145);
            this.securityCodeTextBox.Name = "securityCodeTextBox";
            this.securityCodeTextBox.Size = new System.Drawing.Size(155, 19);
            this.securityCodeTextBox.TabIndex = 20;
            // 
            // expireYearTextBox
            // 
            this.expireYearTextBox.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.expireYearTextBox.Location = new System.Drawing.Point(183, 116);
            this.expireYearTextBox.Name = "expireYearTextBox";
            this.expireYearTextBox.Size = new System.Drawing.Size(155, 19);
            this.expireYearTextBox.TabIndex = 19;
            // 
            // expireMonthTextBox
            // 
            this.expireMonthTextBox.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.expireMonthTextBox.Location = new System.Drawing.Point(183, 86);
            this.expireMonthTextBox.Name = "expireMonthTextBox";
            this.expireMonthTextBox.Size = new System.Drawing.Size(155, 19);
            this.expireMonthTextBox.TabIndex = 18;
            // 
            // cardNumberTextBox
            // 
            this.cardNumberTextBox.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cardNumberTextBox.Location = new System.Drawing.Point(183, 56);
            this.cardNumberTextBox.Name = "cardNumberTextBox";
            this.cardNumberTextBox.Size = new System.Drawing.Size(155, 19);
            this.cardNumberTextBox.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(41, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "セキュリティコード";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(81, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "有効期限(年)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(81, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "有効期限(月)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(93, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "カード番号";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(157, 18);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(66, 24);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "代引き";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(7, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(131, 24);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "クレジットカード";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // showPromptBeforeBuyCheckbox
            // 
            this.showPromptBeforeBuyCheckbox.AutoSize = true;
            this.showPromptBeforeBuyCheckbox.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.showPromptBeforeBuyCheckbox.Location = new System.Drawing.Point(12, 36);
            this.showPromptBeforeBuyCheckbox.Name = "showPromptBeforeBuyCheckbox";
            this.showPromptBeforeBuyCheckbox.Size = new System.Drawing.Size(236, 24);
            this.showPromptBeforeBuyCheckbox.TabIndex = 31;
            this.showPromptBeforeBuyCheckbox.Text = "購入前に確認ウインドウを表示する";
            this.showPromptBeforeBuyCheckbox.UseVisualStyleBackColor = true;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 597);
            this.Controls.Add(this.showPromptBeforeBuyCheckbox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.brandDelButton);
            this.Controls.Add(this.brandAddButton);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.brandComboBox);
            this.Controls.Add(this.useBrandCheckBox);
            this.Controls.Add(this.photosizeNumericDown);
            this.Controls.Add(this.autoscrollCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SettingForm";
            this.Text = "設定画面";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.photosizeNumericDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown photosizeNumericDown;
        private System.Windows.Forms.CheckBox autoscrollCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox useBrandCheckBox;
        private System.Windows.Forms.ComboBox brandComboBox;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button brandAddButton;
        private System.Windows.Forms.Button brandDelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox securityCodeTextBox;
        private System.Windows.Forms.TextBox expireYearTextBox;
        private System.Windows.Forms.TextBox expireMonthTextBox;
        private System.Windows.Forms.TextBox cardNumberTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox cardLastNameTextBox;
        private System.Windows.Forms.TextBox cardFirstNameTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox vpassPasswordTextbox;
        private System.Windows.Forms.CheckBox showPromptBeforeBuyCheckbox;
    }
}