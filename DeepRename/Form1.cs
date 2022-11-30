using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeepRename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DialogResult result  = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text.ToString()))
            {
                button2.Enabled = true;
            } else
            {
                button2.Enabled = false;
                
            }
        }


        public void CopyAndRenameAllFIleAndFolders(string sourcePath, string findText, string replaceText)
        {
            // Get all files and folders in source path
            string[] files = Directory.GetFiles(sourcePath);
            string[] folders = Directory.GetDirectories(sourcePath);

            // Rename all files
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string newFileName = fileName.Replace(findText, replaceText);
                string newFilePath = Path.Combine(sourcePath, newFileName);
                File.Move(file, newFilePath);

                // Read content of newFilePath, replace them
                string content = File.ReadAllText(newFilePath);
                content = content.Replace(findText, replaceText);
                File.WriteAllText(newFilePath, content);
            }

            // Rename all folders
            foreach (string folder in folders)
            {
                string folderName = Path.GetFileName(folder);
                string newFolderName = folderName.Replace(findText, replaceText);
                string newFolderPath = Path.Combine(sourcePath, newFolderName);
                if (folder != newFolderPath)
                {
                    Directory.Move(folder, newFolderPath);
                }
                
                // Recursive call
                CopyAndRenameAllFIleAndFolders(newFolderPath, findText, replaceText);
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị cho ô Tìm");
                return;
            }

            string findText = textBox2.Text;
            string replaceText = textBox3.Text;

            // Deep scan folder, then rename file and folder
            CopyAndRenameAllFIleAndFolders(textBox1.Text, findText, replaceText);
        }

    }
}
