using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;


namespace comparison
{
    public partial class Form1 : Form
    {
        public static string selectedFileName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void EncryptAesManaged(string raw)
        {
            try
            {
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {

                    string securityKey = "1234567812345678";
                    byte[] k = Encoding.ASCII.GetBytes(securityKey);
                    // Encrypt string    
                    byte[] encrypted = Encrypt(raw, k, aes.IV);
                    // Print encrypted string    
                    // Console.WriteLine($ "Encrypted data: {System.Text.Encoding.UTF8.GetString(encrypted)}");
                    // Decrypt the bytes to a string.    
                    string text = System.Text.Encoding.UTF8.GetString(encrypted);
                    textBox2.Text = text;
                    //string decrypted = Decrypt(encrypted, aes.Key, aes.IV);
                    // Print decrypted string. It should be same as raw data    
                    // Console.WriteLine($ "Decrypted data: {decrypted}");
                }
            }
            catch (Exception exp)
            {
                textBox2.Text = exp.Message;

                //Console.WriteLine(exp.Message);
            }
            // Console.ReadKey();
        }
        static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                 selectedFileName = openFileDialog1.FileName;
                Stopwatch sw2 = new Stopwatch();
                sw2.Start();
                byte[] m = StreamFile(selectedFileName);
                textBox1.Text = Encoding.UTF8.GetString(m);
                sw2.Stop();
                string encodetime = sw2.Elapsed.ToString();


                Stopwatch sw = new Stopwatch();
                sw.Start();
                string da = Encoding.UTF8.GetString(m);
                EncryptAesManaged(da);
                sw.Stop();
                string aesdetime = sw.Elapsed.ToString();


                this.dataGridView1.Rows.Add(encodetime, aesdetime);
                //...
            }
        }
        private byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // Create a byte array of file stream length
            byte[] ImageData = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();
            return ImageData; //return the byte data
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Image img = System.Drawing.Image.FromFile(selectedFileName);

            pictureBox1.Image = img;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
