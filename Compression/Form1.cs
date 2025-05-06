using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Compression
{
    public partial class Form1 : Form
    {
        RLE rle;
        LZ77 lz77;
        LZ78 lz78;
        List<Tuple<int, char>> encodedDataLZ78;
        List<Tuple<int, int, char>> encodedDataLZ77;
        List<Tuple<char, int>> encodedRLE;
        string selectedFilePath;
        string inputFromFile;

        public Form1()
        {
            InitializeComponent();
        }

        private void clearForm()
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();
            richTextBox6.Clear();
            richTextBox7.Clear();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            clearForm();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Выберите файл";
            openFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Обработка выбранного файла
                selectedFilePath = openFileDialog1.FileName;
                try
                {
                    inputFromFile = File.ReadAllText(selectedFilePath);
                }
                catch
                {
                    MessageBox.Show("Error");
                }  
            }
            richTextBox1.Text = inputFromFile;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();
            if(radioButton1.Checked)
            {
                richTextBox2.Clear();
                if(!string.IsNullOrEmpty(richTextBox1.Text))
                {
                    string originalText = richTextBox1.Text;
                    rle = new RLE(originalText);
                    var (encodedRLE, encodingTime3, encodingMemory3) = CompressionBenchmark.MeasureEncoding(rle);
                    this.encodedRLE = encodedRLE;
                    StringBuilder sb = new StringBuilder();
                    foreach (var tuple in encodedRLE)
                    {
                        sb.AppendLine(tuple.ToString());
                    }

                    // Отключаем перерисовку RichTextBox
                    richTextBox2.SuspendLayout();
                    richTextBox2.Text = sb.ToString();
                    richTextBox2.ResumeLayout();
                    richTextBox4.Text = Math.Round(CompressionBenchmark.CalculateCompressionRatio(originalText, encodedRLE),2).ToString();
                    richTextBox5.Text = encodingMemory3.ToString();
                    richTextBox6.Text = encodingTime3.ToString();
                    richTextBox7.Text = Math.Round(CompressionBenchmark.CalculateBPC(originalText, encodedRLE),2).ToString();


                    //richTextBox4.Text = CompressionRatioRLE(originalText, encodedRLE).ToString();
                }
                else
                {
                    MessageBox.Show("Ошибка!", "Warning", MessageBoxButtons.OK);
                }
            }
            else if(radioButton2.Checked)
            {
                richTextBox2.Clear();
                if (!string.IsNullOrEmpty(richTextBox1.Text))
                {
                    string originalText = richTextBox1.Text;
                    lz77 = new LZ77(originalText);
                    var (encodedDataLZ77, encodingTime, encodingMemory) = CompressionBenchmark.MeasureEncoding(lz77);
                    this.encodedDataLZ77 = encodedDataLZ77;
                    // Создаем StringBuilder для накопления текста
                    StringBuilder sb = new StringBuilder();
                    foreach (var tuple in encodedDataLZ77)
                    {
                        sb.AppendLine(tuple.ToString());
                    }

                    // Отключаем перерисовку RichTextBox
                    richTextBox2.SuspendLayout();
                    richTextBox2.Text = sb.ToString();
                    richTextBox2.ResumeLayout();

                    richTextBox4.Text = Math.Round(CompressionBenchmark.CalculateCompressionRatio(originalText, encodedDataLZ77),2).ToString();
                    richTextBox5.Text = encodingMemory.ToString();
                    richTextBox6.Text = encodingTime.ToString();
                    richTextBox7.Text = Math.Round(CompressionBenchmark.CalculateBPC(originalText, encodedDataLZ77), 2).ToString();
                }
                else
                {
                    MessageBox.Show("Ошибка!, Исходного текста нет", "Warning", MessageBoxButtons.OK);
                }
            }
            else if (radioButton3.Checked)
            {
                richTextBox2.Clear();
                if (!string.IsNullOrEmpty(richTextBox1.Text))
                {
                    string originalText = richTextBox1.Text;
                    lz78 = new LZ78(originalText);
                    var (encodedDataLZ78, encodingTime2, encodingMemory2) = CompressionBenchmark.MeasureEncoding(lz78);
                    this.encodedDataLZ78 = encodedDataLZ78;
                    StringBuilder sb = new StringBuilder();
                    foreach (var tuple in encodedDataLZ78)
                    {
                        sb.AppendLine(tuple.ToString());
                    }

                    // Отключаем перерисовку RichTextBox
                    richTextBox2.SuspendLayout();
                    richTextBox2.Text = sb.ToString();
                    richTextBox2.ResumeLayout();

                    richTextBox4.Text = Math.Round(CompressionBenchmark.CalculateCompressionRatio(originalText, encodedDataLZ78),2).ToString();
                    richTextBox5.Text = encodingMemory2.ToString();
                    richTextBox6.Text = encodingTime2.ToString();
                    richTextBox7.Text = Math.Round(CompressionBenchmark.CalculateBPC(originalText, encodedDataLZ78), 2).ToString();
                }
                else
                {
                    MessageBox.Show("Ошибка!, Исходного текста нет", "Warning", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Выберите алгоритм!", "Warning", MessageBoxButtons.OK);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox7.Clear();
            if (radioButton1.Checked)
            {
                var (decompressed3, decodingTime3, decodingMemory3) = CompressionBenchmark.MeasureDecoding(rle, encodedRLE);
                richTextBox3.Text = decompressed3;
                richTextBox5.Text = decodingMemory3.ToString();
                richTextBox6.Text = Math.Round(Convert.ToDouble(decodingTime3),2).ToString();
            }
            else if(radioButton2.Checked)
            {
                if(encodedDataLZ77 != null)
                {
                    var (decompressed, decodingTime, decodingMemory) = CompressionBenchmark.MeasureDecoding(lz77, encodedDataLZ77);
                    richTextBox3.Text = decompressed;
                    richTextBox5.Text = decodingMemory.ToString();
                    richTextBox6.Text = Math.Round(Convert.ToDouble(decodingTime), 2).ToString();
                }
                else
                {
                    MessageBox.Show("Error LZ77!", "Warning", MessageBoxButtons.OK);
                }
            }
            else if(radioButton3.Checked)
            {
                if(encodedDataLZ78 != null)
                {
                    var (decompressed2, decodingTime2, decodingMemory2) = CompressionBenchmark.MeasureDecoding(lz78, encodedDataLZ78);
                    richTextBox3.Text = decompressed2;
                    richTextBox5.Text = decodingMemory2.ToString();
                    richTextBox6.Text = Math.Round(Convert.ToDouble(decodingTime2), 2).ToString();
                }
                else
                {
                    MessageBox.Show("Error LZ78!", "Warning", MessageBoxButtons.OK);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clearForm();
            encodedRLE?.Clear();
            encodedDataLZ77?.Clear();
            encodedDataLZ78?.Clear();
        }
    }
}
