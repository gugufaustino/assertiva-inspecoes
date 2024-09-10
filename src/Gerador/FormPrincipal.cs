using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Gerador
{
    public partial class FormPrincipal : Form
    {
        #region properties

        private const string TypeString = "string";
        private const string TypeInt = "int";
        private const string TypeShort = "short";
        private const string TypeDecimal = "decimal";
        private const string TypeDouble = "double";
        private const string TypeDateTime = "DateTime";
        private const string TypeBool = "bool";

        private const string MessageFilesCreated = "All files were created successfully. May you must to include these files in your project and add configurations (DBSet<T>, TMap) on context.";
        private const string MessageRequiredFields = "Please, fill in the required fields.";
        private string configLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\config.txt";

        #endregion

        public FormPrincipal()
        {
            InitializeComponent();
        }

        #region Events

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            ClearSugestionLabels();
            HideLoad();
            LoadInitialConfig();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            folderBrowserDialogFolder.ShowDialog(this);
            textBoxPath.Text = folderBrowserDialogFolder.SelectedPath;
            FileStream fs1 = new FileStream(configLocation, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs1);
            writer.Write(textBoxPath.Text);
            writer.Close();
        }

        private void textBoxClassName_TextChanged(object sender, EventArgs e)
        {
            WriteSugestionLabels();
        }

        private void buttonRemoveAttribute_Click(object sender, EventArgs e)
        {
            if (dataGridViewEntityAttributes.SelectedRows.Count > 0)
                dataGridViewEntityAttributes.Rows.Remove(dataGridViewEntityAttributes.SelectedRows[0]);
        }

        private void buttonAddAttribute_Click(object sender, EventArgs e)
        {
            dataGridViewEntityAttributes.Rows.Add();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxClassName.Text) || !String.IsNullOrEmpty(textBoxPath.Text))
            {
                ShowLoad();
                generateAllFiles();
            }
            else
            {
                MessageBox.Show(MessageRequiredFields);
            }
        }

        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {
            WriteSugestionLabels();
        }

        #endregion

        #region Auxiliaries

        public delegate void RefreshProgressCallBack(ProgressBar progressBar, int value);
        private void RefreshProgress(ProgressBar progressBar, int value)
        {
            if (InvokeRequired)
            {
                RefreshProgressCallBack refreshProgress = RefreshProgress;
                Invoke(refreshProgress, value);
            }
            else
            {
                progressBar.Value = value;
            }
        }

        private void HideLoad()
        {
            progressBarLoad.Value = 0;
            labelLoad.Visible = false;
            progressBarLoad.Visible = false;
        }

        private void ShowLoad()
        {
            labelLoad.Visible = true;
            progressBarLoad.Visible = true;
        }

        private void ClearSugestionLabels()
        {
            labelDomainContractRepository.Text = String.Empty;
            labelDomainContractService.Text = String.Empty;
            labelDomainEntity.Text = String.Empty;
            labelDomainFilter.Text = String.Empty;
            labelRepositoryRepositories.Text = String.Empty;
            labelRepositoryMappings.Text = String.Empty;
            labelServiceServices.Text = String.Empty;
            labelTestRepository.Text = String.Empty;
            labelTestService.Text = String.Empty;
            labelTestAssert.Text = String.Empty;
        }

        private void WriteSugestionLabels()
        {
            if (string.IsNullOrEmpty(textBoxClassName.Text) || string.IsNullOrEmpty(textBoxPath.Text))
                ClearSugestionLabels();
            else
            {
                labelDomainContractRepository.Text = String.Format(@"...\Differencial.Domain\Contracts\Repositories\I{0}Repository.cs", textBoxClassName.Text);
                labelDomainContractService.Text = String.Format(@"...\Differencial.Domain\Contracts\Services\I{0}Service.cs", textBoxClassName.Text);

                labelDomainEntity.Text = String.Format(@"...\Differencial.Domain\Entities\{0}.cs", textBoxClassName.Text);

                labelDomainFilter.Text = String.Format(@"...\Differencial.Domain\Filters\I{0}Repository.cs", textBoxClassName.Text);

                labelRepositoryRepositories.Text = String.Format(@"...\Differencial.Repository\Mappings\{0}Map.cs", textBoxClassName.Text);
                labelRepositoryMappings.Text = String.Format(@"...\Differencial.Repository\Repositories\{0}Repository.cs", textBoxClassName.Text);

                labelRepositoryMappings.Text = String.Format(@"...\Differencial.Repository\Repositories\{0}Repository.cs", textBoxClassName.Text);

                labelServiceServices.Text = String.Format(@"...\Differencial.Service\Services\{0}Services.cs", textBoxClassName.Text);

                labelTestRepository.Text = String.Format(@"...\Differencial.Test\Repository\{0}RepositoryTest.cs", textBoxClassName.Text);
                labelTestService.Text = String.Format(@"...\Differencial.Test\Service\{0}ServiceTest.cs", textBoxClassName.Text);
                labelTestAssert.Text = String.Format(@"...\Differencial.Test\Assert\{0}Assert.cs", textBoxClassName.Text);
            }
        }

        private void VerifyDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void LoadInitialConfig()
        {
            if (File.Exists(configLocation))
            {
                FileStream fs2 = new FileStream(configLocation, FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader reader = new StreamReader(fs2);
                textBoxPath.Text = reader.ReadToEnd();
                reader.Close();
            }
        }

        #region Generate

        public void generateAllFiles()
        {
            labelLoad.Text = "Generating '" + labelDomainContractRepository.Text + "'...";
            generateDomainContractRepository();
            RefreshProgress(progressBarLoad, 1);

            labelLoad.Text = "Generating '" + labelDomainContractService.Text + "'...";
            generateDomainContractService();
            RefreshProgress(progressBarLoad, 2);

            labelLoad.Text = "Generating '" + labelDomainEntity.Text + "'...";
            generateDomainEntity();
            RefreshProgress(progressBarLoad, 3);

            labelLoad.Text = "Generating '" + labelDomainFilter.Text + "'...";
            generateDomainFilter();
            RefreshProgress(progressBarLoad, 4);

            labelLoad.Text = "Generating '" + labelRepositoryRepositories.Text + "'...";
            generateRepositoryRepositories();
            RefreshProgress(progressBarLoad, 5);

            labelLoad.Text = "Generating '" + labelRepositoryMappings.Text + "'...";
            generateRepositoryMappings();
            RefreshProgress(progressBarLoad, 6);

            labelLoad.Text = "Generating '" + labelServiceServices.Text + "'...";
            generateServiceServices();
            RefreshProgress(progressBarLoad, 7);

            labelLoad.Text = "Generating '" + labelTestAssert.Text + "'...";
            generateTestAssert();
            RefreshProgress(progressBarLoad, 8);

            labelLoad.Text = "Generating '" + labelTestRepository.Text + "'...";
            generateTestRepository();
            RefreshProgress(progressBarLoad, 9);

            labelLoad.Text = "Generating '" + labelTestService.Text + "'...";
            generateTestService();
            RefreshProgress(progressBarLoad, 10);

            HideLoad();
            MessageBox.Show(MessageFilesCreated);
        }

        private void generateDomainContractRepository()
        {
            string file = String.Format(@"{0}\Differencial.Domain\Contracts\Repositories\", textBoxPath.Text);
            VerifyDirectory(file);
            file = String.Format(@"{0}\Differencial.Domain\Contracts\Repositories\I{1}Repository.cs", textBoxPath.Text, textBoxClassName.Text);

            string content = "using Differencial.Domain.Entities;" + Environment.NewLine;

            content += "using Differencial.Domain.Filters;" + Environment.NewLine;
            content += Environment.NewLine;
            content += "namespace Differencial.Domain.Contracts.Repositories" + Environment.NewLine;
            content += @"{" + Environment.NewLine;

            content += "\tpublic interface I" + textBoxClassName.Text + "Repository : IRepository<" + textBoxClassName.Text + ", " + textBoxClassName.Text + "Filter>" + Environment.NewLine;
            content += "\t{" + Environment.NewLine;
            content += "\t}" + Environment.NewLine;

            content += @"}";

            File.WriteAllText(file, content);
        }

        private void generateDomainContractService()
        {
            string file = String.Format(@"{0}\Differencial.Domain\Contracts\Services\", textBoxPath.Text);
            VerifyDirectory(file);
            file = String.Format(@"{0}\Differencial.Domain\Contracts\Services\I{1}Service.cs", textBoxPath.Text, textBoxClassName.Text);

            string content = "using Differencial.Domain.Entities;" + Environment.NewLine;

            content += "using Differencial.Domain.Filters;" + Environment.NewLine;
            content += "using System.Collections.Generic;" + Environment.NewLine;

            content += Environment.NewLine;
            content += "namespace Differencial.Domain.Contracts.Services" + Environment.NewLine;
            content += @"{" + Environment.NewLine;

            content += "\tpublic interface I" + textBoxClassName.Text + "Service" + Environment.NewLine;
            content += "\t{" + Environment.NewLine;

            content += "\t\tIEnumerable<" + textBoxClassName.Text + "> Listar(" + textBoxClassName.Text + "Filter filtro);" + Environment.NewLine;
            content += Environment.NewLine;
            content += "\t\tvoid Salvar(int codigoUsuarioLogado, " + textBoxClassName.Text + " entidade);" + Environment.NewLine;
            content += Environment.NewLine;
            content += "\t\tvoid Excluir(int codigoUsuarioLogado, int id);" + Environment.NewLine;

            content += "\t}" + Environment.NewLine;

            content += @"}";

            File.WriteAllText(file, content);
        }

        private void generateDomainEntity()
        {
            string file = String.Format(@"{0}\Differencial.Domain\Entities\", textBoxPath.Text);

            VerifyDirectory(file);

            file = String.Format(@"{0}\Differencial.Domain\Entities\{1}.cs", textBoxPath.Text, textBoxClassName.Text);

            string content = "using Differencial.Domain.Contracts.Entities;" + Environment.NewLine;

            content += "using Differencial.Domain.Resources;" + Environment.NewLine;
            content += "using Differencial.Domain.Util.ExtensionMethods;" + Environment.NewLine;
            content += "using Differencial.Domain.Validation;" + Environment.NewLine;
            content += "using System.ComponentModel.DataAnnotations.Schema;" + Environment.NewLine;
            content += "using System;" + Environment.NewLine;

            content += Environment.NewLine;
            //namespace

            content += "namespace Differencial.Domain.Entities" + Environment.NewLine;

            content += @"{" + Environment.NewLine;

            //class
            content += "\tpublic class " + textBoxClassName.Text + " : IEntity" + Environment.NewLine;

            content += "\t{" + Environment.NewLine;

            bool? isRequired = false;
            object type = "";
            object name = "";
            object alias = "";
            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                name = dataGridViewEntityAttributes.Rows[i].Cells["Name"].Value;
                alias = dataGridViewEntityAttributes.Rows[i].Cells["Alias"].Value;
                //[Column("Name")]
                type = dataGridViewEntityAttributes.Rows[i].Cells["Type"].Value;
                if ((name != null && !String.IsNullOrEmpty(name.ToString())) &&
                    (type != null && !String.IsNullOrEmpty(type.ToString())))
                {
                    isRequired = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["Required"].Value;

                    content += "\t\t[Column(\"" + name + "\")]" + Environment.NewLine;

                    content += "\t\tpublic ";
                    if (type.ToString() != TypeString)
                    {
                        if (isRequired.HasValue && isRequired.Value)
                            content += type + " ";
                        else
                            content += type + "? ";
                    }
                    else
                        content += TypeString + " ";

                    content += alias + " { get; set; }" + Environment.NewLine + Environment.NewLine;
                }
            }

            content += Environment.NewLine;

            //método validate
            content += "\t\t// Valida os dados da entidade" + Environment.NewLine;
            content += "\t\tpublic void Validate()" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;

            content += "\t\t\tvar validationResultsManager = new ValidationResultsManager();" + Environment.NewLine;
            content += Environment.NewLine;
            content += "\t\t\t// Required" + Environment.NewLine;

            object maxLength = "";
            object maxLengthValidation = "";
            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                name = dataGridViewEntityAttributes.Rows[i].Cells["Alias"].Value;
                type = dataGridViewEntityAttributes.Rows[i].Cells["Type"].Value;
                maxLength = dataGridViewEntityAttributes.Rows[i].Cells["MaxLength"].Value;
                maxLengthValidation = dataGridViewEntityAttributes.Rows[i].Cells["MaxLengthValidation"].Value;

                isRequired = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["Required"].Value;

                if (isRequired.HasValue && isRequired.Value)
                {
                    if (type.ToString() == TypeString)
                    {
                        content += "\t\t\tif(";
                        if (name.ToString().ToLower().Contains("email"))
                            content += name + ".IsNullOrEmpty() == false || (" + name + ".IsValidEmailAddress() == false || " + name + ".Length > 250)";
                        else
                            content += name + ".IsNullOrEmpty() || " + name + ".Length " + maxLengthValidation + " " + maxLength;
                    }
                    else if (type.ToString() == TypeInt)
                    {
                        if (name.ToString().ToLower().Equals("id") == false)
                        {
                            content += "\t\t\tif(";
                            content += name + " == 0";
                        }
                        else
                            continue;
                    }
                    else if (type.ToString() == TypeDateTime)
                    {
                        content += "\t\t\tif(";
                        content += name + ".IsValid() == false";
                    }
                    else
                    {
                        continue;
                    }

                    content += ")" + Environment.NewLine;
                    content += "\t\t\t\t";
                    content += "validationResultsManager.AddValidationResultNotValid(MensagensValidacao." + name + "Invalido);";
                    content += Environment.NewLine;
                }
            }

            content += Environment.NewLine;
            content += "\t\t\t// Optional" + Environment.NewLine;

            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                name = dataGridViewEntityAttributes.Rows[i].Cells["Alias"].Value;
                type = dataGridViewEntityAttributes.Rows[i].Cells["Type"].Value;
                maxLength = dataGridViewEntityAttributes.Rows[i].Cells["MaxLength"].Value;
                maxLengthValidation = dataGridViewEntityAttributes.Rows[i].Cells["MaxLengthValidation"].Value;

                isRequired = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["Required"].Value;

                if (isRequired == null || !isRequired.Value)
                {
                    if (type.ToString() == TypeString)
                    {
                        content += "\t\t\tif(";
                        if (name.ToString().ToLower().Contains("email"))
                            content += name + ".IsNullOrEmpty() == false && (" + name + ".IsValidEmailAddress() == false || " + name + ".Length > " + maxLength + ")";
                        else
                            content += name + ".IsNullOrEmpty() == false && " + name + ".Length > " + maxLength;
                        //content += name + ".IsNullOrEmpty() || " + name + ".Length " + maxLengthValidation + " " + maxLength;

                        content += ")" + Environment.NewLine;
                        content += "\t\t\t\t";
                        content += "validationResultsManager.AddValidationResultNotValid(MensagensValidacao." + name + "Invalido);";
                        content += Environment.NewLine;
                    }
                    else if (type.ToString() == TypeDateTime)
                    {
                        content += "\t\t\tif(";
                        content += name + ".HasValue && " + name + ".Value.IsValid() == false";
                        content += ")" + Environment.NewLine;
                        content += "\t\t\t\t";
                        content += "validationResultsManager.AddValidationResultNotValid(MensagensValidacao." + name + "Invalido);";
                        content += Environment.NewLine;
                    }
                }
            }

            content += "\t\t\tif (validationResultsManager.HasError)" + Environment.NewLine;
            content += "\t\t\t\tvalidationResultsManager.ThrowBusinessValidationError();" + Environment.NewLine;
            //fim - método validate
            content += "\t\t}" + Environment.NewLine;

            content += Environment.NewLine;

            //método clone
            content += "\t\t// Clona os dados da entidade" + Environment.NewLine;
            content += "\t\tpublic object Clone()" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;

            content += "\t\t\tvar entidade = new " + textBoxClassName.Text + "();" + Environment.NewLine;
            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                name = dataGridViewEntityAttributes.Rows[i].Cells["Alias"].Value;
                if (name.ToString() != "Id")
                    content += "\t\t\tentidade." + name + " = this." + name + ";" + Environment.NewLine;
            }

            content += "\t\t\treturn entidade;" + Environment.NewLine;
            content += "\t\t}" + Environment.NewLine;


            //fim - class
            content += "\t}" + Environment.NewLine;
            content += Environment.NewLine;

            //enum
            content += "\tpublic enum CampoOrdenacao" + textBoxClassName.Text + Environment.NewLine;
            content += "\t{" + Environment.NewLine;

            bool? isOrder = false;
            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                name = dataGridViewEntityAttributes.Rows[i].Cells["Alias"].Value;
                type = dataGridViewEntityAttributes.Rows[i].Cells["Type"].Value;
                if ((name != null && !String.IsNullOrEmpty(name.ToString())) &&
                    (type != null && !String.IsNullOrEmpty(type.ToString())))
                {
                    isOrder = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["Order"].Value;

                    if (isOrder.HasValue && isOrder.Value)
                    {
                        content += "\t\t" + name + "," + Environment.NewLine;
                    }
                }
            }

            content += "\t}" + Environment.NewLine;
            //fim - enum

            //namespace
            content += "}";

            File.WriteAllText(file, content);
        }

        private void generateDomainFilter()
        {
            string file = String.Format(@"{0}\Differencial.Domain\Filters\", textBoxPath.Text);
            VerifyDirectory(file);
            file = String.Format(@"{0}\Differencial.Domain\Filters\{1}Filter.cs", textBoxPath.Text, textBoxClassName.Text);

            string content = "using Differencial.Domain.Entities;" + Environment.NewLine;
            content += "using System;" + Environment.NewLine;            

            content += Environment.NewLine;

            content += "namespace Differencial.Domain.Filters" + Environment.NewLine;
            content += @"{" + Environment.NewLine;

            content += "\tpublic class " + textBoxClassName.Text + "Filter: Filter" + Environment.NewLine;
            content += "\t{" + Environment.NewLine;

            //content += Environment.NewLine;
            //content += "\t\tpublic " + textBoxClassName.Text + "Filter()" + Environment.NewLine;
            //content += "\t\t\t: base()" + Environment.NewLine;
            //content += "\t\t{" + Environment.NewLine;
            //content += "\t\t}" + Environment.NewLine;
            content += Environment.NewLine;

            bool? isFilter = false;
            object type = "";
            object name = "";
            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                isFilter = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["Filter"].Value;
                name = dataGridViewEntityAttributes.Rows[i].Cells["Alias"].Value;
                type = dataGridViewEntityAttributes.Rows[i].Cells["Type"].Value;

                if (isFilter.HasValue && isFilter.Value)
                {
                    content += "\t\tpublic ";
                    if (type.ToString() != TypeString)
                        content += type + "? ";
                    else
                        content += TypeString + " ";

                    content += name + " { get; set; }" + Environment.NewLine;
                }
            }
            content += Environment.NewLine;

            content += "\t\tpublic CampoOrdenacao" + textBoxClassName.Text + " CampoOrdenacao { get; set; }" + Environment.NewLine;

            content += "\t}" + Environment.NewLine;

            content += @"}";

            File.WriteAllText(file, content);
        }

        private void generateRepositoryRepositories()
        {
            string file = String.Format(@"{0}\Differencial.Repository\Repositories\", textBoxPath.Text);
            VerifyDirectory(file);
            file = String.Format(@"{0}\Differencial.Repository\Repositories\{1}Repository.cs", textBoxPath.Text, textBoxClassName.Text);

            string content = "";
            content += "using Differencial.Domain.Contracts.Repositories;" + Environment.NewLine;

            content += "using Differencial.Domain.Entities;" + Environment.NewLine;

            content += "using Differencial.Domain.Filters;" + Environment.NewLine;
            content += "using Differencial.Domain.Util.ExtensionMethods;" + Environment.NewLine;
            content += "using Differencial.Repository.Context;" + Environment.NewLine;
            content += "using System.Collections.Generic;" + Environment.NewLine;
            content += "using System.Linq;" + Environment.NewLine;
            content += "using System.Linq.Dynamic;" + Environment.NewLine;

            content += Environment.NewLine;

            //namespace
            content += "namespace Differencial.Repository.Repositories" + Environment.NewLine;
            content += @"{" + Environment.NewLine;

            //class
            content += "\tpublic class " + textBoxClassName.Text + "Repository : Repository<" + textBoxClassName.Text + ", " + textBoxClassName.Text + "Filter>, I" + textBoxClassName.Text + "Repository" + Environment.NewLine;
            content += "\t{" + Environment.NewLine;

            //constructor
            content += "\t\tpublic " + textBoxClassName.Text + "Repository(IDbContextFactory dbContextFactory)" + Environment.NewLine;
            content += "\t\t\t: base(dbContextFactory)" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;
            //fim - constructor
            content += "\t\t}" + Environment.NewLine;
            content += Environment.NewLine;

            //método Where
            content += "\t\tpublic override IEnumerable<" + textBoxClassName.Text + "> Where(" + textBoxClassName.Text + "Filter filter)" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;

            string nameInitiallizing = textBoxClassName.Text.Substring(0, 1).ToLower() + textBoxClassName.Text.Substring(1, textBoxClassName.Text.Length - 1);

            content += "\t\t\tvar query = from " + nameInitiallizing + " in _db." + textBoxClassName.Text + Environment.NewLine;
            content += "\t\t\t\t\t\tselect " + nameInitiallizing + ";" + Environment.NewLine;

            content += Environment.NewLine;
            content += "\t\t\tthis.AplicarFiltro(ref query, filter);" + Environment.NewLine;
            content += Environment.NewLine;

            content += "\t\t\treturn query.ToList();" + Environment.NewLine;
            //fim - método Where
            content += "\t\t}" + Environment.NewLine;
            content += Environment.NewLine;

            //método AplicarFiltro
            content += "\t\tprivate void AplicarFiltro(ref IQueryable<" + textBoxClassName.Text + "> query, " + textBoxClassName.Text + "Filter filter)" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;

            content += "\t\t\t// Ordenação" + Environment.NewLine;
            content += "\t\t\tstring order = string.Format(\"{0} {1}\", filter.CampoOrdenacao.ToString(), filter.Order.ToString());" + Environment.NewLine;
            content += "\t\t\tquery = query.OrderBy(order);" + Environment.NewLine;
            content += Environment.NewLine;

            bool? isFilter = false;
            object type = "";
            object name = "";
            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                isFilter = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["Filter"].Value;
                name = dataGridViewEntityAttributes.Rows[i].Cells["Alias"].Value;
                type = dataGridViewEntityAttributes.Rows[i].Cells["Type"].Value;

                if (isFilter.HasValue && isFilter.Value)
                {
                    var comparer = String.Empty;
                    content += "\t\t\tif (filter." + name;
                    if (type.ToString() != TypeString)
                    {
                        content += ".HasValue";
                        comparer = "filter." + name + " == x." + name;
                    }
                    else
                    {
                        content += ".IsNullOrEmpty() == false";
                        //comparer = "filter." + name + ".Contains(x." + name + ")";
                        comparer = "x." + name + ".Contains(filter." + name + ")";
                    }

                    content += ")" + Environment.NewLine;

                    content += "\t\t\t\tquery = query.Where(x => " + comparer + ");" + Environment.NewLine;
                    content += Environment.NewLine;
                }
            }

            content += "\t\t\t// Filtro" + Environment.NewLine;
            content += "\t\t\tbase.ApplyBasicFilter(ref query, ref filter);" + Environment.NewLine;

            //fim - método AplicarFiltro
            content += "\t\t}" + Environment.NewLine;

            //fim - class
            content += "\t}" + Environment.NewLine;

            //fim - namespace
            content += @"}";

            File.WriteAllText(file, content);
        }

        private void generateRepositoryMappings()
        {
            string file = String.Format(@"{0}\Differencial.Repository\Mappings\", textBoxPath.Text);
            VerifyDirectory(file);
            file = String.Format(@"{0}\Differencial.Repository\Mappings\{1}Map.cs", textBoxPath.Text, textBoxClassName.Text);

            string content = "";
            content += "using Differencial.Domain.Entities;" + Environment.NewLine;
            content += "using System.ComponentModel.DataAnnotations.Schema;" + Environment.NewLine;
            content += "using System.Data.Entity.ModelConfiguration;" + Environment.NewLine;
            content += Environment.NewLine;

            content += "namespace Differencial.Repository.Mappings" + Environment.NewLine;
            content += @"{" + Environment.NewLine;

            content += "\tpublic class " + textBoxClassName.Text + "Map : EntityTypeConfiguration<" + textBoxClassName.Text + ">" + Environment.NewLine;
            content += "\t{" + Environment.NewLine;

            content += "\t\tpublic " + textBoxClassName.Text + "Map()" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;
            //content += Environment.NewLine;

            object type = "";
            object name = "";
            bool? isPrimaryKey = false;
            bool? isIdentity = false;

            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                isPrimaryKey = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["PrimaryKey"].Value;
                isIdentity = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["Identity"].Value;
                name = dataGridViewEntityAttributes.Rows[i].Cells["Alias"].Value;
                type = dataGridViewEntityAttributes.Rows[i].Cells["Type"].Value;

                if (isPrimaryKey.HasValue && isPrimaryKey.Value)
                {
                    content += "\t\t\tHasKey(x => x." + name + ");" + Environment.NewLine;
                    content += Environment.NewLine;

                    if (isIdentity.HasValue && isIdentity.Value)
                    {
                        content += "\t\t\tProperty(t => t." + name + ").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);" + Environment.NewLine;
                        content += Environment.NewLine;
                    }

                    break;
                }
            }

            object maxLength = "";
            bool? isRequired = false;

            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                isPrimaryKey = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["PrimaryKey"].Value;
                name = dataGridViewEntityAttributes.Rows[i].Cells["Name"].Value;
                type = dataGridViewEntityAttributes.Rows[i].Cells["Type"].Value;
                maxLength = dataGridViewEntityAttributes.Rows[i].Cells["MaxLength"].Value;
                isRequired = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["Required"].Value;

                if (isPrimaryKey == null || !isPrimaryKey.Value)
                {
                    if (isRequired.HasValue && isRequired.Value)
                    {
                        content += "\t\t\tProperty(t => t." + name + ").IsRequired()";

                        int outResult;
                        if (maxLength != null && Int32.TryParse(maxLength.ToString(), out outResult))
                        {
                            if (outResult > 0)
                                content += ".HasMaxLength(" + outResult + ")";
                        }
                        content += ";" + Environment.NewLine;
                    }
                }
            }

            content += Environment.NewLine;

            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                isPrimaryKey = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["PrimaryKey"].Value;
                name = dataGridViewEntityAttributes.Rows[i].Cells["Name"].Value;
                type = dataGridViewEntityAttributes.Rows[i].Cells["Type"].Value;
                maxLength = dataGridViewEntityAttributes.Rows[i].Cells["MaxLength"].Value;
                isRequired = (bool?)dataGridViewEntityAttributes.Rows[i].Cells["Required"].Value;

                if (isPrimaryKey == null || !isPrimaryKey.Value)
                {
                    if (isRequired == null || !isRequired.Value)
                    {
                        content += "\t\t\tProperty(t => t." + name + ")";
                        int outResult;
                        if (maxLength != null && Int32.TryParse(maxLength.ToString(), out outResult))
                        {
                            if (outResult > 0)
                                content += ".HasMaxLength(" + outResult + ")";
                        }
                        content += ";" + Environment.NewLine;
                    }
                }
            }

            content += "\t\t}" + Environment.NewLine;

            content += "\t}" + Environment.NewLine;

            content += @"}";

            File.WriteAllText(file, content);
        }

        private void generateServiceServices()
        {
            string file = String.Format(@"{0}\Differencial.Service\Services\", textBoxPath.Text);
            VerifyDirectory(file);
            file = String.Format(@"{0}\Differencial.Service\Services\{1}Service.cs", textBoxPath.Text, textBoxClassName.Text);

            string content = "";
            content += "using Differencial.Domain.Contracts.Repositories;" + Environment.NewLine;
            content += "using Differencial.Domain.Contracts.Services;" + Environment.NewLine;

            content += "using Differencial.Domain.Entities;" + Environment.NewLine;

            content += "using Differencial.Domain.Filters;" + Environment.NewLine;
            content += "using Differencial.Domain.UOW;" + Environment.NewLine;
            content += "using System.Collections.Generic;" + Environment.NewLine;

            content += Environment.NewLine;
            content += "namespace Differencial.Service.Services" + Environment.NewLine;
            content += @"{" + Environment.NewLine;

            content += "\tpublic class " + textBoxClassName.Text + "Service : Service, I" + textBoxClassName.Text + "Service" + Environment.NewLine;
            content += "\t{" + Environment.NewLine;

            content += "\t\tI" + textBoxClassName.Text + "Repository";

            string nameInitiallizing = textBoxClassName.Text.Substring(0, 1).ToLower() + textBoxClassName.Text.Substring(1, textBoxClassName.Text.Length - 1);

            content += " _" + nameInitiallizing + "Repositorio;" + Environment.NewLine;
            content += Environment.NewLine;

            content += "\t\tpublic " + textBoxClassName.Text + "Service(";
            content += "IUnitOfWork uow, ";
            content += "I" + textBoxClassName.Text + "Repository " + nameInitiallizing + "Repositorio";
            content += ")" + Environment.NewLine;
            content += "\t\t\t: base(uow)" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;
            content += "\t\t\t_" + nameInitiallizing + "Repositorio = ";
            content += nameInitiallizing + "Repositorio;" + Environment.NewLine;
            content += "\t\t}" + Environment.NewLine;
            content += Environment.NewLine;

            //método listar
            content += "\t\tpublic IEnumerable<" + textBoxClassName.Text + "> ";
            content += "Listar(" + textBoxClassName.Text + "Filter filtro)" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;

            content += "\t\t\treturn TryCatch(() =>" + Environment.NewLine;
            content += "\t\t\t{" + Environment.NewLine;
            content += "\t\t\t\treturn _" + nameInitiallizing + "Repositorio.Where(filtro);" + Environment.NewLine;
            content += "\t\t\t});" + Environment.NewLine;
            //fim - método listar
            content += "\t\t}" + Environment.NewLine;

            content += Environment.NewLine;

            //método salvar
            content += "\t\tpublic void Salvar(int codigoUsuarioLogado, " + textBoxClassName.Text + " entidade)" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;

            content += "\t\t\tTryCatch(() =>" + Environment.NewLine;
            content += "\t\t\t{" + Environment.NewLine;
            content += "\t\t\t\tentidade.Validate();" + Environment.NewLine;
            content += Environment.NewLine;
            content += "\t\t\t\tif (entidade.Id == 0)" + Environment.NewLine;
            content += "\t\t\t\t\t_" + nameInitiallizing + "Repositorio.Add(codigoUsuarioLogado, entidade);" + Environment.NewLine;

            content += "\t\t\t\telse" + Environment.NewLine;
            content += "\t\t\t\t\t_" + nameInitiallizing + "Repositorio.Update(codigoUsuarioLogado, entidade);" + Environment.NewLine;

            content += "\t\t\t});" + Environment.NewLine;

            //fim - método salvar
            content += "\t\t}" + Environment.NewLine;
            content += Environment.NewLine;

            //método excluir
            content += "\t\tpublic void Excluir(int codigoUsuarioLogado, int id)" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;

            content += "\t\t\tTryCatch(() =>" + Environment.NewLine;
            content += "\t\t\t{" + Environment.NewLine;
            content += "\t\t\t\t_" + nameInitiallizing + "Repositorio.Delete(codigoUsuarioLogado, id);" + Environment.NewLine;
            content += "\t\t\t});" + Environment.NewLine;

            //fim - método excluir
            content += "\t\t}" + Environment.NewLine;

            content += "\t}" + Environment.NewLine;

            content += @"}";

            File.WriteAllText(file, content);
        }

        private void generateTestAssert()
        {
            string file = String.Format(@"{0}\Differencial.Test\Asserts\", textBoxPath.Text);
            VerifyDirectory(file);
            file = String.Format(@"{0}\Differencial.Test\Asserts\{1}Assert.cs", textBoxPath.Text, textBoxClassName.Text);

            string content = "";
            content += "using Microsoft.VisualStudio.TestTools.UnitTesting;" + Environment.NewLine;
            content += "using Differencial.Domain.Entities;" + Environment.NewLine;

            content += Environment.NewLine;

            //namespace
            content += "namespace Differencial.Test.Asserts" + Environment.NewLine;
            content += "{" + Environment.NewLine;

            //class
            content += "\tpublic static class " + textBoxClassName.Text + "Assert" + Environment.NewLine;
            content += "\t{" + Environment.NewLine;

            //metodo assert
            content += "\t\tpublic static void Asserts(" + textBoxClassName.Text + " expected, " + textBoxClassName.Text + " actual)" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;

            object name = "";
            for (int i = 0; i < dataGridViewEntityAttributes.Rows.Count; i++)
            {
                name = dataGridViewEntityAttributes.Rows[i].Cells["Name"].Value;
                content += "\t\t\tAssert.AreEqual(expected." + name + ", actual." + name + ");" + Environment.NewLine;
            }

            //metodo assert - fim
            content += "\t\t}" + Environment.NewLine;
            content += Environment.NewLine;

            //class - fim
            content += "\t}" + Environment.NewLine;

            //namespace - fim
            content += "}" + Environment.NewLine;

            File.WriteAllText(file, content);
        }

        private void generateTestRepository()
        {
            string file = String.Format(@"{0}\Differencial.Test\Repository\", textBoxPath.Text);
            VerifyDirectory(file);
            file = String.Format(@"{0}\Differencial.Test\Repository\{1}RepositoryTest.cs", textBoxPath.Text, textBoxClassName.Text);

            string content = "";
            content += "using Microsoft.VisualStudio.TestTools.UnitTesting;" + Environment.NewLine;
            content += "using Differencial.Domain.Contracts.Repositories;" + Environment.NewLine;
            //content += "using Differencial.Domain.Entities;" + Environment.NewLine;
            //content += "using Differencial.Domain.Filters;" + Environment.NewLine;
            content += "using Differencial.Test.Asserts;" + Environment.NewLine;
            content += "using System.Linq;" + Environment.NewLine;
            content += Environment.NewLine;

            //namespace
            content += "namespace Differencial.Test.Repository" + Environment.NewLine;
            content += "{" + Environment.NewLine;

            //class
            content += "\t[TestClass]" + Environment.NewLine;
            content += "\tpublic class " + textBoxClassName.Text + "RepositoryTest : BaseTest" + Environment.NewLine;
            content += "\t{" + Environment.NewLine;

            //metodo teste listar
            content += "\t\t[TestMethod]" + Environment.NewLine;
            content += "\t\tpublic void Repo_Listar_" + textBoxClassName.Text + "()" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;

            content += "\t\t\t//Arrange" + Environment.NewLine;
            content += "\t\t\tvar repo = _ioc.GetInstance<I" + textBoxClassName.Text + "Repository>();" + Environment.NewLine;
            content += Environment.NewLine;

            content += "\t\t\t//Act" + Environment.NewLine;
            content += "\t\t\tvar lista" + textBoxClassName.Text + " = repo.All();" + Environment.NewLine;
            content += Environment.NewLine;

            content += "\t\t\t//Assert" + Environment.NewLine;
            content += "\t\t\t//" + textBoxClassName.Text + "Assert.Asserts();" + Environment.NewLine;

            //metodo teste listar - fim
            content += "\t\t}" + Environment.NewLine;
            content += Environment.NewLine;

            //class - fim
            content += "\t}" + Environment.NewLine;

            //namespace - fim
            content += "}" + Environment.NewLine;

            File.WriteAllText(file, content);
        }

        private void generateTestService()
        {
            string file = String.Format(@"{0}\Differencial.Test\Service\", textBoxPath.Text);
            VerifyDirectory(file);
            file = String.Format(@"{0}\Differencial.Test\Service\{1}ServiceTest.cs", textBoxPath.Text, textBoxClassName.Text);

            string content = "";
            content += "using Microsoft.VisualStudio.TestTools.UnitTesting;" + Environment.NewLine;
            content += "using Differencial.Domain.Contracts.Services;" + Environment.NewLine;

            content += "using Differencial.Domain.Entities;" + Environment.NewLine;

            content += "using Differencial.Domain.Filters;" + Environment.NewLine;
            content += "using Differencial.Test.Asserts;" + Environment.NewLine;
            content += "using System.Linq;" + Environment.NewLine;
            content += Environment.NewLine;

            //namespace
            content += "namespace Differencial.Test.Service" + Environment.NewLine;
            content += "{" + Environment.NewLine;

            //class
            content += "\t[TestClass]" + Environment.NewLine;
            content += "\tpublic class " + textBoxClassName.Text + "ServiceTest : BaseTest" + Environment.NewLine;
            content += "\t{" + Environment.NewLine;

            //metodo teste listar
            content += "\t\t[TestMethod]" + Environment.NewLine;
            content += "\t\tpublic void Serv_Listar_" + textBoxClassName.Text + "()" + Environment.NewLine;
            content += "\t\t{" + Environment.NewLine;

            content += "\t\t\t//Arrange" + Environment.NewLine;
            content += "\t\t\tvar serv = _ioc.GetInstance<I" + textBoxClassName.Text + "Service>();" + Environment.NewLine;
            content += Environment.NewLine;

            content += "\t\t\t//Act" + Environment.NewLine;
            content += "\t\t\tvar lista" + textBoxClassName.Text + " = serv.Listar(new " + textBoxClassName.Text + "Filter());" + Environment.NewLine;
            content += Environment.NewLine;

            content += "\t\t\t//Assert" + Environment.NewLine;
            content += "\t\t\t//" + textBoxClassName.Text + "Assert.Asserts();" + Environment.NewLine;

            //metodo teste listar - fim
            content += "\t\t}" + Environment.NewLine;
            content += Environment.NewLine;

            //class - fim
            content += "\t}" + Environment.NewLine;

            //namespace - fim
            content += "}" + Environment.NewLine;

            File.WriteAllText(file, content);
        }

        #endregion

        #endregion
    }
}
