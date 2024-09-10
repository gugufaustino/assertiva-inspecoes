namespace Gerador
{
    partial class FormPrincipal
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
            this.labelClassName = new System.Windows.Forms.Label();
            this.textBoxClassName = new System.Windows.Forms.TextBox();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.labelPath = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.folderBrowserDialogFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.labelDomainContractRepository = new System.Windows.Forms.Label();
            this.labelDomainContractService = new System.Windows.Forms.Label();
            this.labelDomainEntity = new System.Windows.Forms.Label();
            this.labelRepositoryMappings = new System.Windows.Forms.Label();
            this.labelRepositoryRepositories = new System.Windows.Forms.Label();
            this.labelDomainFilter = new System.Windows.Forms.Label();
            this.labelSample = new System.Windows.Forms.Label();
            this.dataGridViewEntityAttributes = new System.Windows.Forms.DataGridView();
            this.Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PrimaryKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Identity = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Filter = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Required = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Order = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MaxLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxLengthValidation = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.labelEntityAttributes = new System.Windows.Forms.Label();
            this.labelServiceServices = new System.Windows.Forms.Label();
            this.buttonAddAttribute = new System.Windows.Forms.Button();
            this.buttonRemoveAttribute = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.progressBarLoad = new System.Windows.Forms.ProgressBar();
            this.labelLoad = new System.Windows.Forms.Label();
            this.labelTestService = new System.Windows.Forms.Label();
            this.labelTestRepository = new System.Windows.Forms.Label();
            this.labelTestAssert = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntityAttributes)).BeginInit();
            this.SuspendLayout();
            // 
            // labelClassName
            // 
            this.labelClassName.AutoSize = true;
            this.labelClassName.Location = new System.Drawing.Point(12, 87);
            this.labelClassName.Name = "labelClassName";
            this.labelClassName.Size = new System.Drawing.Size(71, 13);
            this.labelClassName.TabIndex = 0;
            this.labelClassName.Text = "Class name: *";
            // 
            // textBoxClassName
            // 
            this.textBoxClassName.Location = new System.Drawing.Point(15, 103);
            this.textBoxClassName.Name = "textBoxClassName";
            this.textBoxClassName.Size = new System.Drawing.Size(418, 20);
            this.textBoxClassName.TabIndex = 1;
            this.textBoxClassName.TextChanged += new System.EventHandler(this.textBoxClassName_TextChanged);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(15, 35);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(340, 20);
            this.textBoxPath.TabIndex = 3;
            this.textBoxPath.TextChanged += new System.EventHandler(this.textBoxPath_TextChanged);
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(12, 19);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(218, 13);
            this.labelPath.TabIndex = 2;
            this.labelPath.Text = "Root application path (mapped on your pc): *";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(361, 33);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(72, 23);
            this.buttonSearch.TabIndex = 4;
            this.buttonSearch.Text = "browse...";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // labelDomainContractRepository
            // 
            this.labelDomainContractRepository.AutoSize = true;
            this.labelDomainContractRepository.Location = new System.Drawing.Point(12, 136);
            this.labelDomainContractRepository.Name = "labelDomainContractRepository";
            this.labelDomainContractRepository.Size = new System.Drawing.Size(155, 13);
            this.labelDomainContractRepository.TabIndex = 5;
            this.labelDomainContractRepository.Text = "labelDomainContractRepository";
            // 
            // labelDomainContractService
            // 
            this.labelDomainContractService.AutoSize = true;
            this.labelDomainContractService.Location = new System.Drawing.Point(12, 159);
            this.labelDomainContractService.Name = "labelDomainContractService";
            this.labelDomainContractService.Size = new System.Drawing.Size(141, 13);
            this.labelDomainContractService.TabIndex = 6;
            this.labelDomainContractService.Text = "labelDomainContractService";
            // 
            // labelDomainEntity
            // 
            this.labelDomainEntity.AutoSize = true;
            this.labelDomainEntity.Location = new System.Drawing.Point(12, 183);
            this.labelDomainEntity.Name = "labelDomainEntity";
            this.labelDomainEntity.Size = new System.Drawing.Size(91, 13);
            this.labelDomainEntity.TabIndex = 7;
            this.labelDomainEntity.Text = "labelDomainEntity";
            // 
            // labelRepositoryMappings
            // 
            this.labelRepositoryMappings.AutoSize = true;
            this.labelRepositoryMappings.Location = new System.Drawing.Point(12, 252);
            this.labelRepositoryMappings.Name = "labelRepositoryMappings";
            this.labelRepositoryMappings.Size = new System.Drawing.Size(125, 13);
            this.labelRepositoryMappings.TabIndex = 10;
            this.labelRepositoryMappings.Text = "labelRepositoryMappings";
            // 
            // labelRepositoryRepositories
            // 
            this.labelRepositoryRepositories.AutoSize = true;
            this.labelRepositoryRepositories.Location = new System.Drawing.Point(12, 228);
            this.labelRepositoryRepositories.Name = "labelRepositoryRepositories";
            this.labelRepositoryRepositories.Size = new System.Drawing.Size(137, 13);
            this.labelRepositoryRepositories.TabIndex = 9;
            this.labelRepositoryRepositories.Text = "labelRepositoryRepositories";
            // 
            // labelDomainFilter
            // 
            this.labelDomainFilter.AutoSize = true;
            this.labelDomainFilter.Location = new System.Drawing.Point(12, 205);
            this.labelDomainFilter.Name = "labelDomainFilter";
            this.labelDomainFilter.Size = new System.Drawing.Size(87, 13);
            this.labelDomainFilter.TabIndex = 8;
            this.labelDomainFilter.Text = "labelDomainFilter";
            // 
            // labelSample
            // 
            this.labelSample.AutoSize = true;
            this.labelSample.Location = new System.Drawing.Point(12, 58);
            this.labelSample.Name = "labelSample";
            this.labelSample.Size = new System.Drawing.Size(182, 13);
            this.labelSample.TabIndex = 11;
            this.labelSample.Text = "Ex.: D:\\Programas\\Aplicacao\\master";
            // 
            // dataGridViewEntityAttributes
            // 
            this.dataGridViewEntityAttributes.AllowUserToAddRows = false;
            this.dataGridViewEntityAttributes.AllowUserToDeleteRows = false;
            this.dataGridViewEntityAttributes.AllowUserToResizeColumns = false;
            this.dataGridViewEntityAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEntityAttributes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Name,
            this.Alias,
            this.Type,
            this.PrimaryKey,
            this.Identity,
            this.Filter,
            this.Required,
            this.Order,
            this.MaxLength,
            this.MaxLengthValidation});
            this.dataGridViewEntityAttributes.Location = new System.Drawing.Point(453, 33);
            this.dataGridViewEntityAttributes.Name = "dataGridViewEntityAttributes";
            this.dataGridViewEntityAttributes.Size = new System.Drawing.Size(718, 435);
            this.dataGridViewEntityAttributes.TabIndex = 12;
            // 
            // Name
            // 
            this.Name.HeaderText = "DB Name";
            this.Name.Name = "Name";
            // 
            // Alias
            // 
            this.Alias.HeaderText = "Alias";
            this.Alias.Name = "Alias";
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Items.AddRange(new object[] {
            "",
            "string",
            "int",
            "short",
            "decimal",
            "double",
            "DateTime",
            "bool"});
            this.Type.Name = "Type";
            // 
            // PrimaryKey
            // 
            this.PrimaryKey.HeaderText = "Primary Key";
            this.PrimaryKey.Name = "PrimaryKey";
            // 
            // Identity
            // 
            this.Identity.HeaderText = "Identity";
            this.Identity.Name = "Identity";
            // 
            // Filter
            // 
            this.Filter.HeaderText = "Filter";
            this.Filter.Name = "Filter";
            // 
            // Required
            // 
            this.Required.HeaderText = "Required";
            this.Required.Name = "Required";
            // 
            // Order
            // 
            this.Order.HeaderText = "Order";
            this.Order.Name = "Order";
            // 
            // MaxLength
            // 
            this.MaxLength.HeaderText = "MaxLength";
            this.MaxLength.Name = "MaxLength";
            // 
            // MaxLengthValidation
            // 
            this.MaxLengthValidation.HeaderText = "Maxlength Validation";
            this.MaxLengthValidation.Items.AddRange(new object[] {
            "!=",
            ">"});
            this.MaxLengthValidation.Name = "MaxLengthValidation";
            this.MaxLengthValidation.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MaxLengthValidation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // labelEntityAttributes
            // 
            this.labelEntityAttributes.AutoSize = true;
            this.labelEntityAttributes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEntityAttributes.Location = new System.Drawing.Point(450, 9);
            this.labelEntityAttributes.Name = "labelEntityAttributes";
            this.labelEntityAttributes.Size = new System.Drawing.Size(96, 13);
            this.labelEntityAttributes.TabIndex = 13;
            this.labelEntityAttributes.Text = "Entity attributes";
            // 
            // labelServiceServices
            // 
            this.labelServiceServices.AutoSize = true;
            this.labelServiceServices.Location = new System.Drawing.Point(12, 275);
            this.labelServiceServices.Name = "labelServiceServices";
            this.labelServiceServices.Size = new System.Drawing.Size(106, 13);
            this.labelServiceServices.TabIndex = 14;
            this.labelServiceServices.Text = "labelServiceServices";
            // 
            // buttonAddAttribute
            // 
            this.buttonAddAttribute.Location = new System.Drawing.Point(930, 4);
            this.buttonAddAttribute.Name = "buttonAddAttribute";
            this.buttonAddAttribute.Size = new System.Drawing.Size(115, 23);
            this.buttonAddAttribute.TabIndex = 15;
            this.buttonAddAttribute.Text = "Add Attritube";
            this.buttonAddAttribute.UseVisualStyleBackColor = true;
            this.buttonAddAttribute.Click += new System.EventHandler(this.buttonAddAttribute_Click);
            // 
            // buttonRemoveAttribute
            // 
            this.buttonRemoveAttribute.Location = new System.Drawing.Point(1056, 4);
            this.buttonRemoveAttribute.Name = "buttonRemoveAttribute";
            this.buttonRemoveAttribute.Size = new System.Drawing.Size(115, 23);
            this.buttonRemoveAttribute.TabIndex = 16;
            this.buttonRemoveAttribute.Text = "Remove Attritube";
            this.buttonRemoveAttribute.UseVisualStyleBackColor = true;
            this.buttonRemoveAttribute.Click += new System.EventHandler(this.buttonRemoveAttribute_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(1056, 483);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(115, 23);
            this.buttonGenerate.TabIndex = 17;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // progressBarLoad
            // 
            this.progressBarLoad.Location = new System.Drawing.Point(453, 486);
            this.progressBarLoad.Maximum = 10;
            this.progressBarLoad.Name = "progressBarLoad";
            this.progressBarLoad.Size = new System.Drawing.Size(574, 23);
            this.progressBarLoad.TabIndex = 18;
            // 
            // labelLoad
            // 
            this.labelLoad.AutoSize = true;
            this.labelLoad.Location = new System.Drawing.Point(450, 471);
            this.labelLoad.Name = "labelLoad";
            this.labelLoad.Size = new System.Drawing.Size(27, 13);
            this.labelLoad.TabIndex = 19;
            this.labelLoad.Text = "load";
            // 
            // labelTestService
            // 
            this.labelTestService.AutoSize = true;
            this.labelTestService.Location = new System.Drawing.Point(12, 345);
            this.labelTestService.Name = "labelTestService";
            this.labelTestService.Size = new System.Drawing.Size(86, 13);
            this.labelTestService.TabIndex = 21;
            this.labelTestService.Text = "labelTestService";
            // 
            // labelTestRepository
            // 
            this.labelTestRepository.AutoSize = true;
            this.labelTestRepository.Location = new System.Drawing.Point(12, 322);
            this.labelTestRepository.Name = "labelTestRepository";
            this.labelTestRepository.Size = new System.Drawing.Size(100, 13);
            this.labelTestRepository.TabIndex = 20;
            this.labelTestRepository.Text = "labelTestRepository";
            // 
            // labelTestAssert
            // 
            this.labelTestAssert.AutoSize = true;
            this.labelTestAssert.Location = new System.Drawing.Point(12, 299);
            this.labelTestAssert.Name = "labelTestAssert";
            this.labelTestAssert.Size = new System.Drawing.Size(79, 13);
            this.labelTestAssert.TabIndex = 22;
            this.labelTestAssert.Text = "labelTestAssert";
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 515);
            this.Controls.Add(this.labelTestAssert);
            this.Controls.Add(this.labelTestService);
            this.Controls.Add(this.labelTestRepository);
            this.Controls.Add(this.labelLoad);
            this.Controls.Add(this.progressBarLoad);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.buttonRemoveAttribute);
            this.Controls.Add(this.buttonAddAttribute);
            this.Controls.Add(this.labelServiceServices);
            this.Controls.Add(this.labelEntityAttributes);
            this.Controls.Add(this.dataGridViewEntityAttributes);
            this.Controls.Add(this.labelSample);
            this.Controls.Add(this.labelRepositoryMappings);
            this.Controls.Add(this.labelRepositoryRepositories);
            this.Controls.Add(this.labelDomainFilter);
            this.Controls.Add(this.labelDomainEntity);
            this.Controls.Add(this.labelDomainContractService);
            this.Controls.Add(this.labelDomainContractRepository);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.labelPath);
            this.Controls.Add(this.textBoxClassName);
            this.Controls.Add(this.labelClassName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntityAttributes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelClassName;
        private System.Windows.Forms.TextBox textBoxClassName;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogFolder;
        private System.Windows.Forms.Label labelDomainContractRepository;
        private System.Windows.Forms.Label labelDomainContractService;
        private System.Windows.Forms.Label labelDomainEntity;
        private System.Windows.Forms.Label labelRepositoryMappings;
        private System.Windows.Forms.Label labelRepositoryRepositories;
        private System.Windows.Forms.Label labelDomainFilter;
        private System.Windows.Forms.Label labelSample;
        private System.Windows.Forms.DataGridView dataGridViewEntityAttributes;
        private System.Windows.Forms.Label labelEntityAttributes;
        private System.Windows.Forms.Label labelServiceServices;
        private System.Windows.Forms.Button buttonAddAttribute;
        private System.Windows.Forms.Button buttonRemoveAttribute;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.ProgressBar progressBarLoad;
        private System.Windows.Forms.Label labelLoad;
        private System.Windows.Forms.Label labelTestService;
        private System.Windows.Forms.Label labelTestRepository;
        private System.Windows.Forms.Label labelTestAssert;
        private System.Windows.Forms.DataGridViewTextBoxColumn Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alias;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PrimaryKey;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Identity;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Filter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Required;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Order;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxLength;
        private System.Windows.Forms.DataGridViewComboBoxColumn MaxLengthValidation;
    }
}