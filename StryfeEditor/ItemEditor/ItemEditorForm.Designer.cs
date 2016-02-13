namespace StryfeEditor.ItemEditor
{
    partial class ItemEditorForm
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
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.price = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.modAttribute = new System.Windows.Forms.ComboBox();
            this.modType = new System.Windows.Forms.ComboBox();
            this.modNewButton = new System.Windows.Forms.Button();
            this.modList = new System.Windows.Forms.ListBox();
            this.modDelButton = new System.Windows.Forms.Button();
            this.modEditButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.modValue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.script = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.itemList = new System.Windows.Forms.ListBox();
            this.changeTextureButton = new System.Windows.Forms.Button();
            this.tileSize = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.gidLabel = new System.Windows.Forms.Label();
            this.newButton = new System.Windows.Forms.Button();
            this.delButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.type = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(81, 12);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(193, 20);
            this.name.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Type:";
            // 
            // price
            // 
            this.price.Location = new System.Drawing.Point(163, 67);
            this.price.Name = "price";
            this.price.Size = new System.Drawing.Size(111, 20);
            this.price.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Price:";
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(81, 94);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(193, 20);
            this.description.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Description:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.modAttribute);
            this.groupBox1.Controls.Add(this.modType);
            this.groupBox1.Controls.Add(this.modNewButton);
            this.groupBox1.Controls.Add(this.modList);
            this.groupBox1.Controls.Add(this.modDelButton);
            this.groupBox1.Controls.Add(this.modEditButton);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.modValue);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 179);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 130);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modifiers";
            // 
            // modAttribute
            // 
            this.modAttribute.FormattingEnabled = true;
            this.modAttribute.Items.AddRange(new object[] {
            "Level",
            "Experience",
            "Vitality",
            "Wisdom",
            "Endurance",
            "Strenght",
            "Dexterity",
            "Intelligence",
            "Faith",
            "Luck",
            "HP",
            "MP",
            "Stamina",
            "PhysicalDamage",
            "MagicalDamage",
            "CriticalChance",
            "PhysicalDefense",
            "MagicalDefense"});
            this.modAttribute.Location = new System.Drawing.Point(69, 19);
            this.modAttribute.Name = "modAttribute";
            this.modAttribute.Size = new System.Drawing.Size(82, 21);
            this.modAttribute.TabIndex = 13;
            // 
            // modType
            // 
            this.modType.FormattingEnabled = true;
            this.modType.Items.AddRange(new object[] {
            "Current",
            "Equipment",
            "Temporary",
            "Permanent"});
            this.modType.Location = new System.Drawing.Point(69, 43);
            this.modType.Name = "modType";
            this.modType.Size = new System.Drawing.Size(82, 21);
            this.modType.TabIndex = 12;
            // 
            // modNewButton
            // 
            this.modNewButton.Location = new System.Drawing.Point(6, 97);
            this.modNewButton.Name = "modNewButton";
            this.modNewButton.Size = new System.Drawing.Size(57, 23);
            this.modNewButton.TabIndex = 11;
            this.modNewButton.Text = "New";
            this.modNewButton.UseVisualStyleBackColor = true;
            this.modNewButton.Click += new System.EventHandler(this.modNewButton_Click);
            // 
            // modList
            // 
            this.modList.FormattingEnabled = true;
            this.modList.Location = new System.Drawing.Point(163, 19);
            this.modList.Name = "modList";
            this.modList.Size = new System.Drawing.Size(93, 95);
            this.modList.TabIndex = 10;
            this.modList.SelectedIndexChanged += new System.EventHandler(this.modList_SelectedIndexChanged);
            // 
            // modDelButton
            // 
            this.modDelButton.Location = new System.Drawing.Point(123, 97);
            this.modDelButton.Name = "modDelButton";
            this.modDelButton.Size = new System.Drawing.Size(34, 23);
            this.modDelButton.TabIndex = 9;
            this.modDelButton.Text = "Del";
            this.modDelButton.UseVisualStyleBackColor = true;
            // 
            // modEditButton
            // 
            this.modEditButton.Location = new System.Drawing.Point(69, 97);
            this.modEditButton.Name = "modEditButton";
            this.modEditButton.Size = new System.Drawing.Size(48, 23);
            this.modEditButton.TabIndex = 8;
            this.modEditButton.Text = "Save";
            this.modEditButton.UseVisualStyleBackColor = true;
            this.modEditButton.Click += new System.EventHandler(this.modEditButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Value:";
            // 
            // modValue
            // 
            this.modValue.Location = new System.Drawing.Point(91, 71);
            this.modValue.Name = "modValue";
            this.modValue.Size = new System.Drawing.Size(60, 20);
            this.modValue.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Attribute:";
            // 
            // script
            // 
            this.script.Location = new System.Drawing.Point(227, 121);
            this.script.Name = "script";
            this.script.Size = new System.Drawing.Size(47, 20);
            this.script.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Script:";
            // 
            // itemList
            // 
            this.itemList.FormattingEnabled = true;
            this.itemList.Location = new System.Drawing.Point(280, 12);
            this.itemList.Name = "itemList";
            this.itemList.Size = new System.Drawing.Size(151, 251);
            this.itemList.Sorted = true;
            this.itemList.TabIndex = 13;
            this.itemList.SelectedIndexChanged += new System.EventHandler(this.itemList_SelectedIndexChanged);
            // 
            // changeTextureButton
            // 
            this.changeTextureButton.Location = new System.Drawing.Point(15, 148);
            this.changeTextureButton.Name = "changeTextureButton";
            this.changeTextureButton.Size = new System.Drawing.Size(92, 23);
            this.changeTextureButton.TabIndex = 14;
            this.changeTextureButton.Text = "Change texture";
            this.changeTextureButton.UseVisualStyleBackColor = true;
            this.changeTextureButton.Click += new System.EventHandler(this.changeTextureButton_Click);
            // 
            // tileSize
            // 
            this.tileSize.Location = new System.Drawing.Point(227, 150);
            this.tileSize.Name = "tileSize";
            this.tileSize.Size = new System.Drawing.Size(47, 20);
            this.tileSize.TabIndex = 15;
            this.tileSize.TextChanged += new System.EventHandler(this.tileSize_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(173, 153);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Tile size:";
            // 
            // gidLabel
            // 
            this.gidLabel.AutoSize = true;
            this.gidLabel.Location = new System.Drawing.Point(113, 153);
            this.gidLabel.Name = "gidLabel";
            this.gidLabel.Size = new System.Drawing.Size(33, 13);
            this.gidLabel.TabIndex = 17;
            this.gidLabel.Text = "gid: 0";
            // 
            // newButton
            // 
            this.newButton.Location = new System.Drawing.Point(279, 276);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(54, 23);
            this.newButton.TabIndex = 16;
            this.newButton.Text = "New";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // delButton
            // 
            this.delButton.Location = new System.Drawing.Point(396, 276);
            this.delButton.Name = "delButton";
            this.delButton.Size = new System.Drawing.Size(34, 23);
            this.delButton.TabIndex = 15;
            this.delButton.Text = "Del";
            this.delButton.UseVisualStyleBackColor = true;
            this.delButton.Click += new System.EventHandler(this.delButton_Click);
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(339, 276);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(51, 23);
            this.editButton.TabIndex = 14;
            this.editButton.Text = "Save";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // type
            // 
            this.type.FormattingEnabled = true;
            this.type.Items.AddRange(new object[] {
            "Armor",
            "Misc",
            "Quest",
            "Usable",
            "Weapon"});
            this.type.Location = new System.Drawing.Point(153, 38);
            this.type.Name = "type";
            this.type.Size = new System.Drawing.Size(121, 21);
            this.type.Sorted = true;
            this.type.TabIndex = 18;
            // 
            // ItemEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 321);
            this.Controls.Add(this.type);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.delButton);
            this.Controls.Add(this.gidLabel);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tileSize);
            this.Controls.Add(this.changeTextureButton);
            this.Controls.Add(this.itemList);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.script);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.description);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.price);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.name);
            this.Name = "ItemEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ItemEditorForm_FormClosed);
            this.Shown += new System.EventHandler(this.ItemEditorForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox price;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox modValue;
        private System.Windows.Forms.Button modDelButton;
        private System.Windows.Forms.Button modEditButton;
        private System.Windows.Forms.TextBox script;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox modList;
        private System.Windows.Forms.ListBox itemList;
        private System.Windows.Forms.Button changeTextureButton;
        private System.Windows.Forms.TextBox tileSize;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label gidLabel;
        private System.Windows.Forms.Button modNewButton;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.Button delButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.ComboBox type;
        private System.Windows.Forms.ComboBox modType;
        private System.Windows.Forms.ComboBox modAttribute;
    }
}