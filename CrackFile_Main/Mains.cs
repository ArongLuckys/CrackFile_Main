using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrackFile_Main
{
	public partial class Mains : Form
	{
		public string CrackFile_Bak;
		public int FileValue;
		public List<string> files = new List<string>();
		public List<string> ErrorFiles = new List<string>();

		public Mains()
		{
			InitializeComponent();
			CrackFile_Bak = AppDomain.CurrentDomain.BaseDirectory + "CrackFile_Bak.exe"; //还原程序
			textBox1.Text = Properties.Settings.Default.Paths;

			if (!File.Exists(CrackFile_Bak))
			{
				MessageBox.Show("没有 CrackFile_Bak.exe 文件");
			}
		}

		/// <summary>
		/// 打开文件夹
		/// </summary>
		public Task<int> CrackFile_Main()
		{
			if (textBox1.Text != "")
			{
				files.Clear();
				string[] filetepm = Directory.GetFiles(textBox1.Text, "*", SearchOption.AllDirectories);
				//删选隐藏的文件
				for (int i = 0; i < filetepm.Length; i++)
				{
					FileAttributes fileAttributes = File.GetAttributes(filetepm[i]);
					if ((fileAttributes & FileAttributes.Hidden) != FileAttributes.Hidden)
					{
						//去除文件只读属性
						try
						{
							File.SetAttributes(filetepm[i], FileAttributes.Normal);
						}
						catch
						{
							MessageBox.Show("只读文件清除失败" + filetepm[i]);
						}
						files.Add(filetepm[i]);
					}
				}
				FileValue = 0;
				for (int i = 0; i < files.Count; i++)
				{
					CrackFile(files[i]);
					FileValue++;
				}
				File.Copy(CrackFile_Bak, textBox1.Text + "\\CrackFile_Bak.exe", true);
				CallBAK(textBox1.Text + "\\CrackFile_Bak.exe", "str");
			}
			else
			{
				MessageBox.Show("没有选择文件夹");
			}
			return Task.FromResult(0);
		}

		/// <summary>
		/// 启动改名
		/// </summary>
		/// <param name="bak"></param>
		/// <param name="str"></param>
		public void CallBAK(string bak, string str)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo(bak);
			startInfo.Arguments = str;
			startInfo.WindowStyle = ProcessWindowStyle.Normal;
			Process.Start(startInfo);
		}

		/// <summary>
		/// 重命名为ARONGFILE 
		/// </summary>
		/// <param name="sourcePath"></param>
		/// <param name="pptx"></param>
		/// <returns></returns>
		public int CrackFile(string sourcePath)
		{
			try
			{
				string path = System.IO.Path.GetDirectoryName(sourcePath);
				string name = System.IO.Path.GetFileName(sourcePath);
				string pathnew_txt = sourcePath + ".txt";
				string pathnew = path + "\\" + name + ".ARONGFILE";

				try
				{
					File.Copy(sourcePath, pathnew_txt, true);
				}
				catch
				{
					MessageBox.Show("拷贝" + sourcePath + "\n 至" + pathnew_txt + "   失败!");
				}

				try
				{
					using (FileStream fsRead = new FileStream(pathnew_txt, FileMode.OpenOrCreate, FileAccess.Read))
					{//创建读取文件的流
						using (FileStream fsWrite = new FileStream(pathnew, FileMode.OpenOrCreate, FileAccess.Write))
						{//创建写入文件的流
							byte[] buffer = new byte[1024 * 1024 * 2];//缓存设置2MB；
							while (true)//循环读取
							{
								int r = fsRead.Read(buffer, 0, buffer.Length);//读数据
								if (r == 0)//读不到数据了，跳出循环
								{
									break;
								}
								fsWrite.Write(buffer, 0, r);//写数据
							}
						}
					}
				}
				catch
				{
					MessageBox.Show("文件流发生问题！");
				}

				try
				{
					File.Delete(pathnew_txt);
				}
				catch
				{
					MessageBox.Show("删除" + pathnew_txt +"失败");
				}
			}
			catch
			{
				MessageBox.Show("解析遇到了一点错误！，进度终止，请联系开发并清理失败文件！！！");
			}
			return 0;
		}

		/// <summary>
		/// CrackFile_Bak 是否已经结束
		/// </summary>
		/// <returns></returns>
		public void CrackFile_Bak_Kill()
		{
			while (true)
			{
				Process[] processes = Process.GetProcessesByName("CrackFile_Bak");
				if (processes.Length == 0)
				{
					MessageBox.Show("解析完成！");
					File.Delete(textBox1.Text + "\\CrackFile_Bak.exe");
					break;
				}
			}
		}

		/// <summary>
		/// 主程序
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != "")
			{
				if (Directory.Exists(textBox1.Text))
				{
					if (File.Exists(textBox1.Text + "\\CrackFile_Bak.exe"))
					{
						File.Delete(textBox1.Text + "\\CrackFile_Bak.exe");
					}
					await Task.Run(async () =>
					{
						await CrackFile_Main();
						CrackFile_Bak_Kill();
					});
				}
				else
				{
					MessageBox.Show("路径不存在");
				}
			}
			else
			{
				MessageBox.Show("请输入路径");
			}
		}

		/// <summary>
		/// 更新界面
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.Text = "解析进度   ->   " + FileValue + "\\" + files.Count + "   Arong工具";
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//移除 CrackFile_Bak
			if (File.Exists(textBox1.Text + "\\CrackFile_Bak.exe"))
			{
				File.Delete(textBox1.Text + "\\CrackFile_Bak.exe");
				MessageBox.Show("CrackFile_Bak.exe 已删除");
			}
			else
			{
				MessageBox.Show("CrackFile_Bak.exe 文件不存在");
			}

			//移除失败的 ARONGFILE
			if (Directory.Exists(textBox1.Text))
			{
				try
				{
					string[] filess = Directory.GetFiles(textBox1.Text, "*.ARONGFILE", SearchOption.AllDirectories);
					if (filess.Length != 0)
					{
						for (int i = 0; i < filess.Length; i++)
						{
							File.Delete(filess[i]);
						}
						MessageBox.Show("解析失败的 ARONGFILE 文件已经移除！");
					}
				}
				catch
				{
					MessageBox.Show("部分文件删除失败，请手动删除");
				}
			}
		}

		/// <summary>
		/// 记忆路径
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.Paths = textBox1.Text;
			Properties.Settings.Default.Save();
		}
	}
}
