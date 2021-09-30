using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace WeatherSvr
{
    public partial class Form1 : Form
    {
        Thread thread1;
        int sec = 120;

        [DllImport("kernel32.dll")]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        public string ReadValue(string section, string key)
        {
            StringBuilder tmp = new StringBuilder(255);

            GetPrivateProfileString(section, key, string.Empty, tmp, 255, ".\\WeatherSvr.ini");

            return tmp.ToString();
        }

        public Form1()
        {
            InitializeComponent();

            thread1 = new Thread(new ThreadStart(Start));
            thread1.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;

            if (thread1 != null)
            {
                thread1.Abort();
                thread1 = null;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            thread1 = new Thread(new ThreadStart(Start));
            thread1.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label1.Text = (sec--) + "초 후 업데이트..";
        }

        private void Start()
        {
            GetData(GetUrl());
        }

        private string GetUrl()
        {
            // 공공데이터포털 (data.go.kr)
            // 동네예보 조회서비스 (활용기간: 2020-05-11 ~ 2022-05-11)
            string url = "";

            // End Point (초단기실황조회)
            url = ReadValue("URL", "ENDPOINT");
            // 인증키 
            url += "?serviceKey=" + ReadValue("URL", "KEY");
            // 예보지점 X 좌표 (서울 중구)
            url += "&nx=" + ReadValue("URL", "X");
            // 예보지점 Y 좌표 (서울 중구)
            url += "&ny=" + ReadValue("URL", "Y");

            string baseDate = DateTime.Now.ToString("yyyyMMdd");
            string baseTime;

            string timeHours = DateTime.Now.ToString("HH");
            int timeMinutes = Convert.ToInt32(DateTime.Now.ToString("mm"));

            if (45 < timeMinutes)
            {
                baseTime = DateTime.Now.ToString("HH") + "00";
            }
            else
            {
                if (timeHours == "00")
                {
                    baseTime = "2300";
                    baseDate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                }
                else
                {
                    baseTime = DateTime.Now.AddHours(-1).ToString("HH") + "00";
                }
            }

            // 발표일자
            url += "&base_date=" + baseDate;
            // 발표시각
            url += "&base_time=" + baseTime;

            SaveLog(Environment.NewLine + "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] ");
            SaveLog("Base Date: " + baseDate + " / ");
            SaveLog("Base Time: " + baseTime + " / ");

            return url;
        }

        private void GetData(string url)
        {
            string xml = "";
            string data = "";

            try
            {
                using (WebClient client = new NoKeepAliveWebClient())
                {
                    xml = Encoding.UTF8.GetString(client.DownloadData(url));
                }

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xml);

                XmlNodeList xnList = xDoc.SelectNodes("/response/body/items/item");
                foreach (XmlNode xn in xnList)
                {
                    if (xn["category"].InnerText == "T1H")
                    {
                        data = xn["obsrValue"].InnerText + "℃";
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveLog(Environment.NewLine + ex.ToString() + Environment.NewLine);
            }

            SaveData(data);
            SaveLog(data);
        }

        private void SaveData(string data)
        {
            string dataFilePath = ReadValue("PATH", "DATA_FILE");
            string dataDirPath = Path.GetDirectoryName(dataFilePath);

            if (!Directory.Exists(dataDirPath))
                Directory.CreateDirectory(dataDirPath);

            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                fs = new FileStream(dataFilePath, FileMode.Create);

                sw = new StreamWriter(fs);
                sw.Write(data);

                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                if (sw != null)
                    sw.Close();

                if (fs != null)
                    fs.Close();
            }

            label2.Text = "(최종 업데이트: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " / " + data + ")";
            sec = 120;
        }

        public void SaveLog(string msg)
        {
            string logDirPath = ReadValue("PATH", "LOG_DIR");

            if (!Directory.Exists(logDirPath))
                Directory.CreateDirectory(logDirPath);

            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                fs = new FileStream(logDirPath + "\\WeatherSvr_log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);

                sw = new StreamWriter(fs);
                sw.Write(msg);

                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                if (sw != null)
                    sw.Close();

                if (fs != null)
                    fs.Close();
            }
        }
    }
}
