/*
 * Created by SharpDevelop.
 * User: gohmi
 * Date: 21/04/2019
 * Time: 9:08 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Linq;
using System.Globalization;

namespace WriterHelper
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			loadWordNetDict();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			try {
			
			this.webBrowser1.Navigate("https://en.wiktionary.org/wiki/" + textBox1.Text.Replace(" ", "+"));
			this.webBrowser2.Navigate("https://en.wikipedia.org/wiki/" + textBox1.Text.Replace(" ", "+"));
			
			
			List<string> res = searchNounIndex(textBox1.Text);
			List<string> res2 = searchVerbIndex(textBox1.Text);
			List<string> res3 = searchAdvIndex(textBox1.Text);
			List<string> res4 = searchAdjIndex(textBox1.Text);
			
			richTextBox5.Text = "";
			
			foreach(string s in res) {
				richTextBox5.Text +=  "\n" + s;
			}
			
			foreach(string s in res2) {
				richTextBox5.Text +=  "\n" + s;
			}
			
			foreach(string s in res3) {
				richTextBox5.Text +=  "\n" + s;
			}
			
			foreach(string s in res4) {
				richTextBox5.Text +=  "\n" + s;
			}
			
			string[] tmp = richTextBox5.Text.Split('\n');
			richTextBox5.Text = "";
			
			foreach(string s in tmp) {
				if(s.Contains("Synonyms") != true) {
					richTextBox5.Text = richTextBox5.Text + "\n" + s;
				}
			}
			
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
			}
			
		}
		
		
		void Button2Click(object sender, EventArgs e)
		{
			
			try {
			
			if(comboBox2.Text == "All") {
				List<string> res = searchNounIndex(textBox2.Text);
				List<string> res2 = searchVerbIndex(textBox2.Text);
				List<string> res3 = searchAdvIndex(textBox2.Text);
				List<string> res4 = searchAdjIndex(textBox2.Text);
			
				richTextBox4.Text = "";
			
				foreach(string s in res) {
					richTextBox4.Text = richTextBox4.Text + "\n" + s;
				}
			
				foreach(string s in res2) {
					richTextBox4.Text = richTextBox4.Text + "\n" + s;
				}
			
				foreach(string s in res3) {
					richTextBox4.Text = richTextBox4.Text + "\n" + s;
				}
			
				foreach(string s in res4) {
					richTextBox4.Text = richTextBox4.Text + "\n" + s;
				}
			}
			
			if(comboBox2.Text == "Noun") {
				
				richTextBox4.Text = "";
				List<string> res = searchNounIndex(textBox2.Text);
				
				foreach(string s in res) {
					richTextBox4.Text = richTextBox4.Text + "\n" + s;
				}
			}
			
			if(comboBox2.Text == "Verb") {
				
				richTextBox4.Text = "";
				List<string> res2 = searchVerbIndex(textBox2.Text);
				
				foreach(string s in res2) {
					richTextBox4.Text = richTextBox4.Text + "\n" + s;
				}
			}
			
			if(comboBox2.Text == "Adjectives") {
				
				richTextBox4.Text = "";
				List<string> res4 = searchAdjIndex(textBox2.Text);
				
				foreach(string s in res4) {
					richTextBox4.Text = richTextBox4.Text + "\n" + s;
				}
			}
			
			if(comboBox2.Text == "Adverb") {
				
				richTextBox4.Text = "";
				List<string> res3 = searchAdvIndex(textBox2.Text);
				
				foreach(string s in res3) {
					richTextBox4.Text = richTextBox4.Text + "\n" + s;
				}
			}
				
				
			}
			
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
			}
			
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			try {
				
			
			if(comboBox1.Text.Equals("AES")) {
				string res = encryptStringAES(richTextBox1.Text, textBox3.Text);
				richTextBox2.Text = res;
			}
			
			if(comboBox1.Text.Equals("TripleDES")) {
				string res = encryptStringTripleDES(richTextBox1.Text, textBox3.Text);
				richTextBox2.Text = res;
			}
			
			if(comboBox1.Text.Equals("DES")) {
				string res = encryptStringDES(richTextBox1.Text, textBox3.Text);
				richTextBox2.Text = res;
			}
				
				
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
			}
			
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			try {
			if(comboBox1.Text.Equals("AES")) {
				string res = decryptStringAES(richTextBox2.Text, textBox3.Text);
				richTextBox1.Text = res;
			}
			
			if(comboBox1.Text.Equals("TripleDES")) {
				string res = decryptTripleDES(richTextBox2.Text, textBox3.Text);
				richTextBox1.Text = res;
			}
			
			if(comboBox1.Text.Equals("DES")) {
				string res = decryptStringDES(richTextBox2.Text, textBox3.Text);
				richTextBox1.Text = res;
			}
				
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
			}
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			try {
			
			DialogResult result = MessageBox.Show("This can takes some time. Do you want to continue?", "Text", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			
			if(result == DialogResult.Yes) {
			if(radioButton1.Checked) {
				string[] sentences = Regex.Split(richTextBox3.Text, @"(?<=[\.!\?])\s+");
				
				richTextBox3.Text = "";
				double g = 0;
				foreach(string sent in sentences) {
					
					string re = "";
					string re2 = "";
					if(checkBox1.Checked) {
						re = searchGoogle(sent);
					}
					if(checkBox2.Checked) {
						re2 = searchBing(sent);
					}
					
					richTextBox3.Text += "\n" + sent + " :: " + re + " :: " + re2;
					if(re.Equals("") != true || re2.Equals("") != true) g++;
					
					
				}
				
				richTextBox3.Text += "\n\n Plagiarism: " + g.ToString() + " \\ " + sentences.Length.ToString();;
			
			}
			
			if(radioButton2.Checked) {
				string[] paragraphs = richTextBox3.Text.Split(new String[] {"\r\n" }, System.StringSplitOptions.None);
				
				richTextBox3.Text = "";
				double g = 0;
				foreach(string par in paragraphs) {
					string re = "";
					string re2 = "";
					if(checkBox1.Checked) {
						re = searchGoogle(par);
					}
					if(checkBox2.Checked) {
						re2 = searchBing(par);
					}
					
					richTextBox3.Text += "\n" + par + " :: " + re + " :: " + re2;
					if(re.Equals("") != true || re2.Equals("") != true) g++;
					
					
					
				}
				
				richTextBox3.Text += "\n\n Plagiarism: " + g.ToString() + " \\ " + paragraphs.Length.ToString();;
			}
				
			}
			
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
			}
			
	
		}
		
		
		
		public List<string> searchNounIndex(string term) {
			
			try {
				
			
			List<string> l = new List<string>();
			
			
			foreach(string row in nounIndex) {
				string[] col = row.Split(' ');
				
				if(term.ToLower().Replace(" ", "_") == col[0]) {
					
					string indexes = getIndex(row);
					
					//l.Add(row);
					l.Add("Noun\n");
					l.Add(indexes);
					
					string[] tmp = indexes.Split(' ');
					foreach(string index in tmp) {
						l.Add(searchNounData(index));
					}
					
				}
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public string searchNounData(string index) {
			
			try {
			foreach(string row in nounData) {
				string[] col = row.Split(' ');
				
				if(col[0] == index) {
					
					//MessageBox.Show(row);
					string definition = row.Split('|')[1];
					string results = Regex.Replace(row.Split('|')[0], @"[^A-Za-z_ ]+", "");
					return "\nDefinition: " + definition + "\nSynonyms: " + results.Replace(" ", ",");
				}
			}
			
			
			return "";
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		
		public List<string> searchVerbIndex(string term) {
			
			try {
			List<string> l = new List<string>();
			
			
			foreach(string row in verbIndex) {
				string[] col = row.Split(' ');
				
				if(term.ToLower().Replace(" ", "_") == col[0]) {
					
					string indexes = getIndex(row);
					
					//l.Add(row);
					l.Add("Verb\n");
					l.Add(indexes);
					
					string[] tmp = indexes.Split(' ');
					foreach(string index in tmp) {
						l.Add(searchVerbData(index));
					}
					
				}
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public string searchVerbData(string index) {
			
			try {
			foreach(string row in verbData) {
				string[] col = row.Split(' ');
				
				if(col[0] == index) {
					
					//MessageBox.Show(row);
					string definition = row.Split('|')[1];
					string results = Regex.Replace(row.Split('|')[0], @"[^A-Za-z_ ]+", "");
					return "\nDefinition: " + definition + "\nSynonyms: " + results.Replace(" ", ",");
				}
			}
			
			
			return "";
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		
		public List<string> searchAdvIndex(string term) {
			
			try {
			List<string> l = new List<string>();
			
			
			foreach(string row in advIndex) {
				string[] col = row.Split(' ');
				
				if(term.ToLower().Replace(" ", "_") == col[0]) {
					
					string indexes = getIndex(row);
					
					//l.Add(row);
					l.Add("Adverb\n");
					l.Add(indexes);
					
					string[] tmp = indexes.Split(' ');
					foreach(string index in tmp) {
						l.Add(searchAdvData(index));
					}
					
				}
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public string searchAdvData(string index) {
			
			try {
			foreach(string row in advData) {
				string[] col = row.Split(' ');
				
				if(col[0] == index) {
					
					//MessageBox.Show(row);
					string definition = row.Split('|')[1];
					string results = Regex.Replace(row.Split('|')[0], @"[^A-Za-z_ ]+", "");
					return "\nDefinition: " + definition + "\nSynonyms: " + results.Replace(" ", ",");
				}
			}
			
			
			return "";
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		
		public List<string> searchAdjIndex(string term) {
			
			try {
			List<string> l = new List<string>();
			
			
			foreach(string row in adjIndex) {
				string[] col = row.Split(' ');
				
				if(term.ToLower().Replace(" ", "_") == col[0]) {
					
					string indexes = getIndex(row);
					
					//l.Add(row);
					l.Add("Adjective\n");
					l.Add(indexes);
					
					string[] tmp = indexes.Split(' ');
					foreach(string index in tmp) {
						l.Add(searchAdjData(index));
					}
					
				}
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public string searchAdjData(string index) {
			
			try {
			foreach(string row in adjData) {
				string[] col = row.Split(' ');
				
				if(col[0] == index) {
					
					//MessageBox.Show(row);
					string definition = row.Split('|')[1];
					string results = Regex.Replace(row.Split('|')[0], @"[^A-Za-z_ ]+", "");
					return "\nDefinition: " + definition + "\nSynonyms: " + results.Replace(" ", ",");
				}
			}
			
			
			return "";
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		
		public string getIndex(string row) {
			
			try {
			List<string> res = new List<string>();
			string[] col = row.Split(' ');
			
			string temp = "";
			int i = 0;
			foreach(string s in col) {
				
				if(s.Length == 8 && s.All(char.IsDigit)) 
				{
					if(i == 0)
						temp = s;
					if(i > 0) 
						temp += s + " ";
				}
				
				i++;
			}
			
			return temp;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		
		public List<string> nounIndex = new List<string>();
		public List<string> nounData = new List<string>();
		public List<string> verbIndex = new List<string>();
		public List<string> verbData = new List<string>();
		public List<string> adjIndex = new List<string>();
		public List<string> adjData = new List<string>();
		public List<string> advIndex = new List<string>();
		public List<string> advData = new List<string>();
		public void loadWordNetDict() {
			
			try {
			nounIndex = loadNounIndex();
			nounData = loadNounData();
			verbIndex = loadVerbIndex();
			verbData = loadVerbData();
			adjIndex = loadAdjIndex();
			adjData = loadAdjData();
			advIndex = loadadvIndex();
			advData = loadadvData();
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
			}
			
		}
		
		public List<string> loadNounIndex() {
			
			try {
			List<string> l = new List<string>();
			
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\dict\\index.noun");
			string line;
			while((line = sr.ReadLine()) != null) {
				
				l.Add(line);
				
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public List<string> loadNounData() {
			
			try {
			List<string> l = new List<string>();
			
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\dict\\data.noun");
			string line;
			while((line = sr.ReadLine()) != null) {
				
				l.Add(line);
				
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public List<string> loadVerbIndex() {
			
			try {
			List<string> l = new List<string>();
			
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\dict\\index.verb");
			string line;
			while((line = sr.ReadLine()) != null) {
				
				l.Add(line);
				
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public List<string> loadVerbData() {
			
			try {
			List<string> l = new List<string>();
			
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\dict\\data.verb");
			string line;
			while((line = sr.ReadLine()) != null) {
				
				l.Add(line);
				
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public List<string> loadAdjIndex() {
			
			try {
			List<string> l = new List<string>();
			
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\dict\\index.adj");
			string line;
			while((line = sr.ReadLine()) != null) {
				
				l.Add(line);
				
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public List<string> loadAdjData() {
			
			try {
			List<string> l = new List<string>();
			
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\dict\\data.adj");
			string line;
			while((line = sr.ReadLine()) != null) {
				
				l.Add(line);
				
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public List<string> loadadvIndex() {
			
			try {
			List<string> l = new List<string>();
			
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\dict\\index.adv");
			string line;
			while((line = sr.ReadLine()) != null) {
				
				l.Add(line);
				
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		public List<string> loadadvData() {
			
			try {
			List<string> l = new List<string>();
			
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\dict\\data.adv");
			string line;
			while((line = sr.ReadLine()) != null) {
				
				l.Add(line);
				
			}
			
			return l;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return null;
			}
		}
		
		
		
		
		
		public string searchGoogle(string sent) {
			
			try {
			string[] t = new string[2];
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Settings.ini");
			string line = "";
			int i = 0;
			while((line = sr.ReadLine()) != null) {
				t[i] = line;
				i++;
			}
			sr.Close();
			
			string h = getHTML("https://www.google.com/search?q=" + new string(sent.Replace(" ", "+").Where(c => !char.IsPunctuation(c)).ToArray()));
			
			if(h.Contains(t[0].Replace("[Google]", ""))) {
				return "https://www.google.com/search?q=" + sent.Replace(" ", "+");
			}
			
			return "";
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		
		
		public string searchBing(string sent) {
			
			try {
			string[] t = new string[2];
			StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\Settings.ini");
			string line = "";
			int i = 0;
			while((line = sr.ReadLine()) != null) {
				t[i] = line;
				i++;
			}
			sr.Close();
			
			string h = getHTML("https://www.bing.com/search?q=" + new string(sent.Replace(" ", "+").Where(c => !char.IsPunctuation(c)).ToArray()));
			
			if(h.Contains(t[1].Replace("[Bing]", ""))) {
				return "https://www.bing.com/search?q=" + sent.Replace(" ", "+");
			}
			
			return "";
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		public string getHTML(string url) {
			
			try {
			WebClient web = new WebClient();
			string html = web.DownloadString(url);
			
			return html;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
			
		}
		
		
		
		
		
		
		//Reference: https://tekeye.uk/visual_studio/encrypt-decrypt-c-sharp-string
		public string encryptStringAES(string plainText, string passPhrase) {
			
			try {
			int keysize = 256;
			string initVector = "qwertyuiopasdfgh";
			
			byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			
			PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
			byte[] keyBytes = password.GetBytes(keysize / 8);
			
			RijndaelManaged symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
			
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptostream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			cryptostream.Write(plainTextBytes, 0, plainTextBytes.Length);
			cryptostream.FlushFinalBlock();
			byte[] cipherTextBytes = memoryStream.ToArray();
			memoryStream.Close();
			cryptostream.Close();
			
			string result = Convert.ToBase64String(cipherTextBytes);
			
			return result;
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		public string decryptStringAES(string encryptedText, string passPhrase) {
			
			try {
			int keysize = 256;
			string initVector = "qwertyuiopasdfgh";
			
			byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
			byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
			
			PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
			byte[] keyBytes = password.GetBytes(keysize / 8);
			
			RijndaelManaged symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
			
			MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
			CryptoStream cryptostream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];
			int decryptedByteCount = cryptostream.Read(plainTextBytes, 0, plainTextBytes.Length);
			
			memoryStream.Close();
			cryptostream.Close();
			
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		//Reference: https://www.codeguru.com/csharp/csharp/cs_misc/security/triple-des-encryption-and-decryption-in-c.html
		public string encryptStringTripleDES(string plainText, string passPhrase) {
			
			try {
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			
			MD5CryptoServiceProvider md5Crypto = new MD5CryptoServiceProvider();
			byte[] key = md5Crypto.ComputeHash(UTF8Encoding.UTF8.GetBytes(passPhrase));
			md5Crypto.Clear();
			

			TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = key;
			tripleDES.Mode = CipherMode.ECB;
			tripleDES.Padding = PaddingMode.PKCS7;
			
			ICryptoTransform encryptor = tripleDES.CreateEncryptor();
			
			byte[] resultByte = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
			tripleDES.Clear();
			
			return Convert.ToBase64String(resultByte, 0, resultByte.Length);
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
			                                   
		}
		
		public string decryptTripleDES(string encryptedText, string passPhrase) {
			
			try {
			byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);
			
			MD5CryptoServiceProvider md5Crypto = new MD5CryptoServiceProvider();
			byte[] key = md5Crypto.ComputeHash(UTF8Encoding.UTF8.GetBytes(passPhrase));
			md5Crypto.Clear();
			

			TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
			tripleDES.Key = key;
			tripleDES.Mode = CipherMode.ECB;
			tripleDES.Padding = PaddingMode.PKCS7;
			
			ICryptoTransform decryptor = tripleDES.CreateDecryptor();
			
			byte[] resultByte = decryptor.TransformFinalBlock(encryptedTextBytes, 0, encryptedTextBytes.Length);
			tripleDES.Clear();
			
			return UTF8Encoding.UTF8.GetString(resultByte);
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		
		public string encryptStringDES(string plainText, string passPhrase) {
			try {
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			
			MD5CryptoServiceProvider md5Crypto = new MD5CryptoServiceProvider();
			byte[] key = md5Crypto.ComputeHash(UTF8Encoding.UTF8.GetBytes(passPhrase));
			md5Crypto.Clear();
			
			DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
			DES.Key = key;
			
			ICryptoTransform encryptor = DES.CreateEncryptor();
			
			byte[] resultByte = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
			DES.Clear();
			
			return Convert.ToBase64String(resultByte, 0, resultByte.Length);
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		public string decryptStringDES(string encryptedText, string passPhrase) {
			try {
			byte[] encryptedTextBytes = Encoding.UTF8.GetBytes(encryptedText);
			
			MD5CryptoServiceProvider md5Crypto = new MD5CryptoServiceProvider();
			byte[] key = md5Crypto.ComputeHash(UTF8Encoding.UTF8.GetBytes(passPhrase));
			md5Crypto.Clear();
			
			DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
			DES.Key = key;
			
			ICryptoTransform decryptor = DES.CreateDecryptor();
			
			byte[] resultByte = decryptor.TransformFinalBlock(encryptedTextBytes, 0, encryptedTextBytes.Length);
			DES.Clear();
			
			return UTF8Encoding.UTF8.GetString(resultByte);
			
			}
			
			catch(Exception ex) {
				MessageBox.Show(ex.ToString());
				return "";
			}
		}
		
		
		
		void OpenWriterHelperToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Show();
    			this.WindowState = FormWindowState.Normal;
    			this.ShowInTaskbar = true;
		}
		void DictionaryToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Show();
    			this.WindowState = FormWindowState.Normal;
    			this.ShowInTaskbar = true;
    			tabControl1.SelectedIndex = 0;
		}
		void ThesaurusToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Show();
    			this.WindowState = FormWindowState.Normal;
    			this.ShowInTaskbar = true;
    			tabControl1.SelectedIndex = 1;
		}
		void PLagiarismCheckerToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Show();
    			this.WindowState = FormWindowState.Normal;
    			this.ShowInTaskbar = true;
    			tabControl1.SelectedIndex = 2;
		}
		void EncryptionToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Show();
    			this.WindowState = FormWindowState.Normal;
    			this.ShowInTaskbar = true;
    			tabControl1.SelectedIndex = 3;
		}
		void CloseToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Close();
		}
		
		
		
		
		
		void MainFormResize(object sender, EventArgs e)
		{
			if(FormWindowState.Minimized == WindowState) {
				Hide();
				notifyIcon1.Visible = true;  
			}
		}
		void NotifyIcon1MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Show();
			WindowState = FormWindowState.Normal;
			notifyIcon1.Visible = false;  
		}
		
		
	}
}
