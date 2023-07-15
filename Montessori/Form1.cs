using System.IO;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace Montessori
{
    public partial class Form1 : Form
    {

        List<string> files1 = new List<string>();
        List<string> files2 = new List<string>();
        List<string> files3 = new List<string>();
        List<string> files4 = new List<string>();
        List<string> files5 = new List<string>();
        List<string> files6 = new List<string>();
        List<string> filesNames1 = new List<string>();
        List<string> filesNames2 = new List<string>();
        List<string> filesNames3 = new List<string>();
        List<string> filesNames4 = new List<string>();
        List<string> filesNames5 = new List<string>();
        List<string> filesNames6 = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void FindAndReplace(Word.Application wordApp, object ToFindText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCard = false;
            object matchSoundLike = false;
            object matchAllForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;    
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            
            wordApp.Selection.Find.Execute(
                                            ref ToFindText,
                                            ref matchCase,
                                            ref matchWholeWord,
                                            ref matchWildCard,
                                            ref matchSoundLike,
                                            ref matchAllForms,
                                            ref forward,
                                            ref wrap,
                                            ref format,
                                            ref replaceWithText,
                                            ref replace,
                                            ref matchKashida,
                                            ref matchDiactitics,
                                            ref matchAlefHamza,
                                            ref matchControl                                            
                                           );

        }

        private List<string> GetName(List<string> fileNames)
        {
            List<string> names = new List<string>(); 

            for (int i = 0; i < fileNames.Count; i++)
            {
                string fileName = fileNames[i];
                string name = "";

                int j = 0;
                while (!fileName[j].Equals('2'))
                {
                    if (fileName[j] == '_')
                    {
                        name = name + " ";
                    }
                    else
                    {
                        name = name + fileName[j];
                    }
                    
                    j++;
                }         
                names.Add(name);
            }
            return names;
        }

        private void MarkAGrade(List<string> umiejetnosci, Word.Table table, int semestr, int howmuch, int start)
        {
            for (int i = 0; i < howmuch; i++)
            {
                if (umiejetnosci[i + start] == "OPANOWANE W STOPNIU ROZSZERZONYM")
                {
                    table.Cell(i * 7 + 3, semestr).Range.Text = "X";
                }
                else if (umiejetnosci[i + start] == "OPANOWANE W STOPNIU PE³NYM")
                {
                    table.Cell(i * 7 + 4, semestr).Range.Text = "X";
                }
                else if (umiejetnosci[i + start] == "WYMAGA NIEWIELU æWICZEñ")
                {
                    table.Cell(i * 7 + 5, semestr).Range.Text = "X";
                }
                else if (umiejetnosci[i + start] == "WYMAGA WIELU æWICZEñ")
                {
                    table.Cell(i * 7 + 6, semestr).Range.Text = "X";
                }
                else if (umiejetnosci[i + start] == "ZAPREZENTOWANE/W TRAKCIE æWICZEñ")
                {
                    table.Cell(i * 7 + 7, semestr).Range.Text = "X";
                }
                else if (umiejetnosci[i + start] == "ZALEG³Oœæ")
                {
                    table.Cell(i * 7 + 8, semestr).Range.Text = "X";
                }
                else
                {
                    table.Cell(i * 7 + 9, semestr).Range.Text = "X";
                }
            }
        }

        private void CreateWordDocument(object filename, object SaveAs, int semestr, string name) 
        {

            //object templatePath = Path.GetDirectoryName(Application.ExecutablePath).Trim() +"\\Template.docx";


            object templatePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Template.docx";
            semestr++;

            Excel.Application excel = new Excel.Application();
            Excel.Workbook wb;
            Excel.Worksheet ws;

            wb = excel.Workbooks.Open((string)filename);
            ws = wb.Worksheets[1];

            List<string> umiejetnosci = new List<string>();
            List<string> opis = new List<string>();

            //petla tworzaca tablice z wartosciami wszystkich umiejetnosci
            for (int i = 0; i < 83; i++)
            {
                umiejetnosci.Add(ws.Cells[i+2, 5].Value2);
                opis.Add(ws.Cells[i + 2, 4].Value2);
            }
            

            Word.Application wordApp = new Word.Application();
            object missing = Missing.Value;
            Word.Document myWordDoc = null;


            object readOnly = false;
            object isVisible = false;
            wordApp.Visible = false;


            if (File.Exists((string)SaveAs)){


                myWordDoc = wordApp.Documents.Open(ref SaveAs, ref missing, ref readOnly,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing, ref missing);


            }
            else 
            {
                myWordDoc = wordApp.Documents.Open(ref templatePath, ref missing, ref readOnly,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing, ref missing);
            }



                

                

            myWordDoc.Activate();

            // find and replace 

            this.FindAndReplace(wordApp, "<name>", name);
                
            //informatyczna 
            Word.Table table = myWordDoc.Tables[6]; 
            MarkAGrade(umiejetnosci, table, semestr, 4, 0);
            //matematyczna
            table = myWordDoc.Tables[1]; 
            MarkAGrade(umiejetnosci, table, semestr, 26, 4);
            //plastyczna
            table = myWordDoc.Tables[5];
            MarkAGrade(umiejetnosci, table, semestr, 6, 30);
            //polonistyczna
            table = myWordDoc.Tables[2];
            MarkAGrade(umiejetnosci, table, semestr, 17, 36);
            //przyrodnicza
            table = myWordDoc.Tables[3];
            MarkAGrade(umiejetnosci, table, semestr, 11, 53);
            //angielski
            table = myWordDoc.Tables[4];
            MarkAGrade(umiejetnosci, table, semestr, 11, 64);
            //wf
            table = myWordDoc.Tables[7];
            MarkAGrade(umiejetnosci, table, semestr, 3, 75);
                



            myWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing
                , ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing
                );

            myWordDoc.Close();
            wordApp.Quit();
            wb.Close();
            excel.Quit();

            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            
            ofd.Title = "Import umiejêtnoœci";
            ofd.Multiselect = true;
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in ofd.SafeFileNames)
                {
                    checkedListBox1.Items.Add(item, true);
                    filesNames1.Add(item);
                   
                }
                foreach (var item in ofd.FileNames)
                {
                    files1.Add(item);
                }
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Akcja spowoduje usuniêcie z listy, wszystkich obecnie zaimportowanych plików.", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);
      
            if (dialogResult == DialogResult.Yes)
            {
                checkedListBox1.Items.Clear();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("Czy chcesz usun¹æ z listy zaznaczone pliki?", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult1 == DialogResult.Yes)
            {
                for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
                {
                    // clb is the name of the CheckedListBox control
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        checkedListBox1.Items.Remove(checkedListBox1.Items[i]);
                    }
                }
            }
            else if (dialogResult1 == DialogResult.No)
            {
                //do something else
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                
                if(string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Brak œcie¿ki zapisu");
                }
                else
                {
                    List<int> indexes = new List<int>();
                    string path = textBox1.Text.ToString();

                    //semestr 1
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        indexes.Add(checkedListBox1.CheckedIndices[i]);
                    }
                    foreach (int index in indexes)
                    {
                       
                        CreateWordDocument(files1[index], path + @"\" + GetName(filesNames1)[index] + @".docx", 1, GetName(filesNames1)[index]);
                    }
                    indexes.Clear();

                    //2
                    for (int i = 0; i < checkedListBox2.Items.Count; i++)
                    {
                        indexes.Add(checkedListBox2.CheckedIndices[i]);
                    }
                    foreach (int index in indexes)
                    {
                        CreateWordDocument(files2[index], path + @"\" + GetName(filesNames2)[index] + @".docx", 2, GetName(filesNames2)[index]);
                    }
                    indexes.Clear();

                    //3
                    for (int i = 0; i < checkedListBox3.Items.Count; i++)
                    {
                        indexes.Add(checkedListBox3.CheckedIndices[i]);
                    }
                    foreach (int index in indexes)
                    {
                        CreateWordDocument(files3[index], path + @"\" + GetName(filesNames3)[index] + @".docx", 3, GetName(filesNames3)[index]);
                    }
                    indexes.Clear();

                    //4
                    for (int i = 0; i < checkedListBox4.Items.Count; i++)
                    {
                        indexes.Add(checkedListBox4.CheckedIndices[i]);
                    }
                    foreach (int index in indexes)
                    {
                        CreateWordDocument(files4[index], path + @"\" + GetName(filesNames4)[index] + @".docx", 4, GetName(filesNames4)[index]);
                    }
                    indexes.Clear();


                    //5 
                    for (int i = 0; i < checkedListBox5.Items.Count; i++)
                    {
                        indexes.Add(checkedListBox5.CheckedIndices[i]);
                    }
                    foreach (int index in indexes)
                    {
                        CreateWordDocument(files5[index], path + @"\" + GetName(filesNames5)[index] + @".docx", 5, GetName(filesNames5)[index]);
                    }
                    indexes.Clear();

                    //6
                    for (int i = 0; i < checkedListBox6.Items.Count; i++)
                    {
                        indexes.Add(checkedListBox6.CheckedIndices[i]);
                    }
                    foreach (int index in indexes)
                    {
                        CreateWordDocument(files6[index], path + @"\" + GetName(filesNames6)[index] + @".docx", 6, GetName(filesNames6)[index]);
                    }
                    indexes.Clear();
                }

                

            }
            if(checkBox2.Checked)
            {

            }
            if (checkBox3.Checked)
            {

            }
            
        }
        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.SelectedPath;
                textBox1.Text = path;   
            }



        }

        #region nieuzywane 
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        #endregion

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Import umiejêtnoœci";
            ofd.Multiselect = true;
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in ofd.SafeFileNames)
                {
                    checkedListBox2.Items.Add(item, true);
                    filesNames2.Add(item);

                }
                foreach (var item in ofd.FileNames)
                {
                    files2.Add(item);
                }

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("Czy chcesz usun¹æ z listy zaznaczone pliki?", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult1 == DialogResult.Yes)
            {
                for (int i = checkedListBox2.Items.Count - 1; i >= 0; i--)
                {
                    // clb is the name of the CheckedListBox control
                    if (checkedListBox2.GetItemChecked(i))
                    {
                        checkedListBox2.Items.Remove(checkedListBox2.Items[i]);
                    }
                }
            }
            else if (dialogResult1 == DialogResult.No)
            {
                //do something else
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("Czy chcesz usun¹æ z listy zaznaczone pliki?", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult1 == DialogResult.Yes)
            {
                for (int i = checkedListBox3.Items.Count - 1; i >= 0; i--)
                {
                    // clb is the name of the CheckedListBox control
                    if (checkedListBox3.GetItemChecked(i))
                    {
                        checkedListBox3.Items.Remove(checkedListBox3.Items[i]);
                    }
                }
            }
            else if (dialogResult1 == DialogResult.No)
            {
                //do something else
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("Czy chcesz usun¹æ z listy zaznaczone pliki?", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult1 == DialogResult.Yes)
            {
                for (int i = checkedListBox4.Items.Count - 1; i >= 0; i--)
                {
                    // clb is the name of the CheckedListBox control
                    if (checkedListBox4.GetItemChecked(i))
                    {
                        checkedListBox4.Items.Remove(checkedListBox4.Items[i]);
                    }
                }
            }
            else if (dialogResult1 == DialogResult.No)
            {
                //do something else
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("Czy chcesz usun¹æ z listy zaznaczone pliki?", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult1 == DialogResult.Yes)
            {
                for (int i = checkedListBox5.Items.Count - 1; i >= 0; i--)
                {
                    // clb is the name of the CheckedListBox control
                    if (checkedListBox5.GetItemChecked(i))
                    {
                        checkedListBox5.Items.Remove(checkedListBox5.Items[i]);
                    }
                }
            }
            else if (dialogResult1 == DialogResult.No)
            {
                //do something else
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Import umiejêtnoœci";
            ofd.Multiselect = true;
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in ofd.SafeFileNames)
                {
                    checkedListBox5.Items.Add(item, true);
                    filesNames5.Add(item);

                }
                foreach (var item in ofd.FileNames)
                {
                    files5.Add(item);
                }

            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Import umiejêtnoœci";
            ofd.Multiselect = true;
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in ofd.SafeFileNames)
                {
                    checkedListBox3.Items.Add(item, true);
                    filesNames3.Add(item);

                }
                foreach (var item in ofd.FileNames)
                {
                    files3.Add(item);
                }

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Import umiejêtnoœci";
            ofd.Multiselect = true;
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in ofd.SafeFileNames)
                {
                    checkedListBox4.Items.Add(item, true);
                    filesNames4.Add(item);

                }
                foreach (var item in ofd.FileNames)
                {
                    files4.Add(item);
                }

            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Import umiejêtnoœci";
            ofd.Multiselect = true;
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in ofd.SafeFileNames)
                {
                    checkedListBox6.Items.Add(item, true);
                    filesNames6.Add(item);

                }
                foreach (var item in ofd.FileNames)
                {
                    files6.Add(item);
                }

            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult1 = MessageBox.Show("Czy chcesz usun¹æ z listy zaznaczone pliki?", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult1 == DialogResult.Yes)
            {
                for (int i = checkedListBox6.Items.Count - 1; i >= 0; i--)
                {
                    // clb is the name of the CheckedListBox control
                    if (checkedListBox6.GetItemChecked(i))
                    {
                        checkedListBox6.Items.Remove(checkedListBox6.Items[i]);
                    }
                }
            }
            else if (dialogResult1 == DialogResult.No)
            {
                //do something else
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Akcja spowoduje usuniêcie z listy, wszystkich obecnie zaimportowanych plików.", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                checkedListBox2.Items.Clear();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Akcja spowoduje usuniêcie z listy, wszystkich obecnie zaimportowanych plików.", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                checkedListBox3.Items.Clear();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Akcja spowoduje usuniêcie z listy, wszystkich obecnie zaimportowanych plików.", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                checkedListBox4.Items.Clear();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Akcja spowoduje usuniêcie z listy, wszystkich obecnie zaimportowanych plików.", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                checkedListBox5.Items.Clear();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Akcja spowoduje usuniêcie z listy, wszystkich obecnie zaimportowanych plików.", "OSTRZE¯ENIE", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                checkedListBox6.Items.Clear();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }
    }
}