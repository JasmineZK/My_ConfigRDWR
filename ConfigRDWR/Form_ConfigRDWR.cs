using System;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace ConfigRDWR
{
    public partial class Form_ConfigRDWR : Form
    {
        #region 常用量初始化

        #region 配置文件的哈希表
        Hashtable alarm = new Hashtable();
        Hashtable web = new Hashtable();
        Hashtable device = new Hashtable();
        #endregion

        #region textbox总数
        const int tbCountAll = 15 + 16 + 28;
        #endregion

        #region 指定文件读取路径
        string fileAlarm = @".\alarm_center_base.properties"; //指定Alarm文件读取路径
        string fileWeb = @".\web_base.properties"; //指定Web文件读取路径
        string fileDevice = @".\device_center_base.properties"; //指定Device文件读取路径
        #endregion

        #region 配置文件的三个Key数组
        string[] alarmName = new string[]{
                                     "cloud_version", "redis_ip", "redis_port", "local_listen_port", "local_listen_time_out",
                                     "rpc_server_port", "db_url", "db_user", "db_password", "web_server_enable", "web_server_ip",
                                     "web_server_port", "cloud_sms_enable", "serial_sms_enable", "serial_sms_com"
                                 };
        string[] webName = new string[]{
                                   "web_listen_port", "web_socket", "web_socket_port", "web_rpc_listen_port", "redis_ip", "redis_port",
                                   "db_url", "db_user", "db_password", "alarm_url", "alarm_pop_url", "alarm_server_ip",
                                   "alarm_server_port", "data_server_listen", "device_server_ip", "device_server_port"
                               };
        string[] deviceName = new string[]{
                                      "hk_send", "hk_false_data", "hk_ip", "hk_port", "hk_interval", "device_center_from_listen",
                                      "alarm_server_enable", "alarm_server_ip", "alarm_server_port","data_server_enable",
                                      "data_server_ip", "data_server_port", "water_1_enable", "water_1_send_seconds",
                                      "water_2_enable", "water_2_listen_port", "xhs_1_enable", "xhs_1_listen_port",
                                      "fire_1_enable", "fire_1_listen_port", "electric_1_enable", "electric_1_listen_port",
                                      "fire_2_enable", "fire_2_listen_port", "dynamic_monitor_1_enable", "db_url", "db_user", "db_password"
                                  };
        #endregion

        #region 存放配置的数组
        string[] arrAlarm = new string[2] { "0", "0" }; //用来存放
        string[] arrWeb = new string[2] { "0", "0" };   //配置文件
        string[] arrDevice = new string[2] { "0", "0" };//的键值对
        string sAlarm = "0"; //用来存放
        string sWeb = "0";   //读取到的
        string sDevice = "0";//配置文件
        int index = 0; //用来存放 "=" 的索引值

        #endregion

        #endregion

        /// <summary>
        /// 初始化默认构造函数
        /// </summary>
        public Form_ConfigRDWR()
        {
            InitializeComponent();

            #region 3个textBox数组
            TextBox[] TbAlarm = new TextBox[]{
                                tb_cloud_sms_enable, tb_cloud_version, tb_db_password, tb_db_url, tb_db_user, tb_local_listen_port,
                                tb_local_listen_time_out, tb_redis_ip, tb_redis_port, tb_rpc_server_port, tb_serial_sms_com,
                                tb_serial_sms_enable, tb_web_server_enable, tb_web_server_ip, tb_web_server_port
                            };
            TextBox[] TbWeb = new TextBox[]{
                                tb__alarm_pop_url, tb__alarm_server_ip, tb__alarm_server_port, tb__alarm_url, tb__data_server_listen,
                                tb__db_password, tb__db_url, tb__db_user, tb__device_server_ip, tb__device_server_port, tb__redis_ip,
                                tb__redis_port, tb__web_listen_port, tb__web_rpc_listen_port, tb__web_socket,tb__web_socket_port
                            };
            TextBox[] TbDevice = new TextBox[]{
                                    tbalarm_server_enable, tbalarm_server_ip, tbalarm_server_port, tbdata_server_enable, tbdata_server_ip,
                                    tbdata_server_port, tbdb_password, tbdb_url, tbdb_user, tbdevice_center_from_listen, tbdynamic_monitor_1_enable,
                                    tbelectric_1_enable, tbelectric_1_listen_port, tbfire_1_enable, tbfire_1_listen_port, tbfire_2_enable,
                                    tbfire_2_listen_port, tbhk_false_data, tbhk_interval, tbhk_ip, tbhk_port, tbhk_send, tbwater_1_enable, tbwater_1_send_seconds,
                                    tbwater_2_enable, tbwater_2_listen_port, tbxhs_1_enable, tbxhs_1_listen_port
                                };
            #endregion

            #region if-else块，判断exe程序根目录是否存在三个配置文件，不存在则创建并写入 Key=，存在则读取并显示出来

            #region if块

            if (File.Exists(@".\alarm_center_base.properties") && File.Exists(@".\web_base.properties") && File.Exists(@".\device_center_base.properties"))
            {
                //清空哈希表以重新使用
                alarm.Clear();
                web.Clear();
                device.Clear();

                #region 创建流对象
                FileStream fsAlarm = new FileStream(fileAlarm, FileMode.Open, FileAccess.Read); //新建Alarm文件流
                FileStream fsWeb = new FileStream(fileWeb, FileMode.Open, FileAccess.Read); //新建Web文件流
                FileStream fsDevice = new FileStream(fileDevice, FileMode.Open, FileAccess.Read); //新建Device文件流
                StreamReader srAlarm = new StreamReader(fsAlarm); //新建Alarm读取流对象
                StreamReader srWeb = new StreamReader(fsWeb); //新建Web读取流对象
                StreamReader srDevice = new StreamReader(fsDevice); //新建Device读取流对象
                #endregion

                #region try-catch块，捕获初始化读取异常

                #region try块
                try
                {
                    #region 将配置读取出来存放到哈希表
                    while ((sAlarm = srAlarm.ReadLine()) != null)
                    {
                        while (!sAlarm.StartsWith("#") && !sAlarm.StartsWith(" "))
                        {
                            sAlarm = sAlarm.Replace(" ", ""); //将字符串中的空格过滤掉
                            index = sAlarm.IndexOf('='); //取得等号的索引值
                            arrAlarm[0] = sAlarm.Substring(0, index); //等号左边的字符串
                            arrAlarm[1] = sAlarm.Substring(index + 1); //等号右边的字符串
                            alarm.Add(arrAlarm[0], arrAlarm[1]); //存入哈希表
                            break;
                        }
                    }

                    while ((sWeb = srWeb.ReadLine()) != null)
                    {
                        while (!sWeb.StartsWith("#") && !sWeb.StartsWith(" "))
                        {
                            sWeb = sWeb.Replace(" ", "");
                            index = sWeb.IndexOf('=');
                            arrWeb[0] = sWeb.Substring(0, index);
                            arrWeb[1] = sWeb.Substring(index + 1);
                            web.Add(arrWeb[0], arrWeb[1]);
                            break;
                        }
                    }

                    while ((sDevice = srDevice.ReadLine()) != null)
                    {
                        while (!sDevice.StartsWith("#") && !sDevice.StartsWith(" "))
                        {
                            sDevice = sDevice.Replace(" ", "");
                            index = sDevice.IndexOf('=');
                            arrDevice[0] = sDevice.Substring(0, index);
                            arrDevice[1] = sDevice.Substring(index + 1);
                            device.Add(arrDevice[0], arrDevice[1]);
                            break;
                        }
                    }
                    #endregion
                }

                #endregion

                #region catch块，捕获初始化异常

                catch (NullReferenceException ex)
                {
                    MessageBox.Show("初始化读取异常为：" + ex.Message);
                }

                #endregion

                #endregion

                #region 将读取到的配置显示到相应的textbox中
                for (int i = 0; i < alarmName.Length; i++)
                {
                    foreach (DictionaryEntry _alarm in alarm)
                    {
                        if (("tb_" + _alarm.Key.ToString()) == TbAlarm[i].Name)
                        {
                            TbAlarm[i].Text = _alarm.Value.ToString();
                        }
                    }
                }

                for (int j = 0; j < webName.Length; j++)
                {
                    foreach (DictionaryEntry _web in web)
                    {
                        if (("tb__" + _web.Key.ToString()) == TbWeb[j].Name)
                        {
                            TbWeb[j].Text = _web.Value.ToString();
                        }
                    }
                }

                for (int k = 0; k < deviceName.Length; k++)
                {
                    foreach (DictionaryEntry _device in device)
                    {
                        if (("tb" + _device.Key.ToString()) == TbDevice[k].Name)
                        {
                            TbDevice[k].Text = _device.Value.ToString();
                        }
                    }
                }
                #endregion

                #region 关闭流
                //关闭写入流
                srAlarm.Close();
                srWeb.Close();
                srDevice.Close();

                //关闭文件流
                fsAlarm.Close();
                fsWeb.Close();
                fsDevice.Close();
                #endregion
            }

            #endregion

            #region else块
            else
            {
                //清空哈希表以重新使用
                alarm.Clear();
                web.Clear();
                device.Clear();

                #region 创建保存配置文件的默认value为0的哈希表
                alarm.Add("cloud_version", "0");
                alarm.Add("redis_ip", "0");
                alarm.Add("redis_port", "0");
                alarm.Add("local_listen_port", "0");
                alarm.Add("local_listen_time_out", "0");
                alarm.Add("rpc_server_port", "0");
                alarm.Add("db_url", "0");
                alarm.Add("db_user", "0");
                alarm.Add("db_password", "0");
                alarm.Add("web_server_enable", "0");
                alarm.Add("web_server_ip", "0");
                alarm.Add("web_server_port", "0");
                alarm.Add("cloud_sms_enable", "0");
                alarm.Add("serial_sms_enable", "0");
                alarm.Add("serial_sms_com", "0");

                web.Add("web_listen_port", "0");
                web.Add("web_socket", "0");
                web.Add("web_socket_port", "0");
                web.Add("web_rpc_listen_port", "0");
                web.Add("redis_ip", "0");
                web.Add("redis_port", "0");
                web.Add("db_url", "0");
                web.Add("db_user", "0");
                web.Add("db_password", "0");
                web.Add("alarm_url", "0");
                web.Add("alarm_pop_url", "0");
                web.Add("alarm_server_ip", "0");
                web.Add("alarm_server_port", "0");
                web.Add("data_server_listen", "0");
                web.Add("device_server_ip", "0");
                web.Add("device_server_port", "0");

                device.Add("hk_send", "0");
                device.Add("hk_false_data", "0");
                device.Add("hk_ip", "0");
                device.Add("hk_port", "0");
                device.Add("hk_interval", "0");
                device.Add("device_center_from_listen", "0");
                device.Add("alarm_server_enable", "0");
                device.Add("alarm_server_ip", "0");
                device.Add("alarm_server_port", "0");
                device.Add("data_server_enable", "0");
                device.Add("data_server_ip", "0");
                device.Add("data_server_port", "0");
                device.Add("water_1_enable", "0");
                device.Add("water_1_send_seconds", "0");
                device.Add("water_2_enable", "0");
                device.Add("water_2_listen_port", "0");
                device.Add("xhs_1_enable", "0");
                device.Add("xhs_1_listen_port", "0");
                device.Add("fire_1_enable", "0");
                device.Add("fire_1_listen_port", "0");
                device.Add("electric_1_enable", "0");
                device.Add("electric_1_listen_port", "0");
                device.Add("fire_2_enable", "0");
                device.Add("fire_2_listen_port", "0");
                device.Add("dynamic_monitor_1_enable", "0");
                device.Add("db_url", "0");
                device.Add("db_user", "0");
                device.Add("db_password", "0");
                #endregion

                #region 创建流对象
                FileStream fsAlarm = new FileStream(fileAlarm, FileMode.Create, FileAccess.Write); //新建Alarm文件流
                FileStream fsWeb = new FileStream(fileWeb, FileMode.Create, FileAccess.Write); //新建Web文件流
                FileStream fsDevice = new FileStream(fileDevice, FileMode.Create, FileAccess.Write); //新建Device文件流
                StreamWriter swAlarm = new StreamWriter(fsAlarm); //新建Alarm写入流对象
                StreamWriter swWeb = new StreamWriter(fsWeb); //新建Web写入流对象
                StreamWriter swDevice = new StreamWriter(fsDevice); //新建Device写入流对象
                #endregion

                #region 配置文件写入

                #region Alarm配置文件写入
                for (int i = 0; i < alarmName.Length; i++)
                {
                    foreach (DictionaryEntry _alarm in alarm)
                    {
                        if (_alarm.Key.ToString() == alarmName[i])
                        {
                            switch (i)
                            {
                                case 0:
                                    swAlarm.WriteLine("#base information properties file");
                                    swAlarm.WriteLine("##########");
                                    swAlarm.WriteLine("#TYPE");
                                    break;
                                case 1:
                                    swAlarm.WriteLine("##########");
                                    swAlarm.WriteLine("#REDIS");
                                    break;
                                case 3:
                                    swAlarm.WriteLine("##########");
                                    swAlarm.WriteLine("#LISTEN DESKTOP CLIENT");
                                    break;
                                case 5:
                                    swAlarm.WriteLine("##########");
                                    swAlarm.WriteLine("#LISTEN DATA FROM DEVICE/WEB CENTER TO OPERATION");
                                    break;
                                case 6:
                                    swAlarm.WriteLine("##########");
                                    swAlarm.WriteLine("#DATABASE");
                                    break;
                                case 9:
                                    swAlarm.WriteLine("##########");
                                    swAlarm.WriteLine("#SEND ALARM DATA TO WEB CENTER");
                                    break;
                                case 12:
                                case 13:
                                    swAlarm.WriteLine("##########");
                                    break;
                            }
                            swAlarm.WriteLine(_alarm.Key + "=1"); //写入数据
                        }
                    }
                }
                swAlarm.WriteLine("##########");
                #endregion

                #region Web配置文件写入
                for (int j = 0; j < webName.Length; j++)
                {
                    foreach (DictionaryEntry _web in web)
                    {
                        if (_web.Key.ToString() == webName[j])
                        {
                            switch (j)
                            {
                                case 0:
                                    swWeb.WriteLine("##### web config file #####");
                                    swWeb.WriteLine("# WEB");
                                    break;
                                case 3:
                                    swWeb.WriteLine("##########");
                                    swWeb.WriteLine("# WEB RPC");
                                    break;
                                case 4:
                                    swWeb.WriteLine("##########");
                                    swWeb.WriteLine("# REDIS");
                                    break;
                                case 6:
                                    swWeb.WriteLine("##########");
                                    swWeb.WriteLine("#DATABASE");
                                    break;
                                case 9:
                                    swWeb.WriteLine("##########");
                                    swWeb.WriteLine("#ALARM OPERATION");
                                    break;
                                case 11:
                                    swWeb.WriteLine("##########");
                                    swWeb.WriteLine("# ALARM CENTER");
                                    break;
                                case 13:
                                    swWeb.WriteLine("##########");
                                    swWeb.WriteLine("# DATA CENTER");
                                    break;
                                case 14:
                                    swWeb.WriteLine("##########");
                                    swWeb.WriteLine("#DEVICE SERVER");
                                    break;                               
                            }
                            swWeb.WriteLine(_web.Key + "=1"); //写入数据
                        }
                    }
                }
                #endregion

                #region Device配置文件写入
                for (int k = 0; k < deviceName.Length; k++)
                {
                    foreach (DictionaryEntry _device in device)
                    {
                        if (_device.Key.ToString() == deviceName[k])
                        {
                            switch (k)
                            {
                                case 0:
                                case 5:
                                case 6:
                                case 9:
                                case 12:
                                case 14:
                                case 16:
                                case 18:
                                case 20:
                                case 22:
                                case 24:
                                    swDevice.WriteLine("###########");
                                    break;
                                case 25:
                                    swDevice.WriteLine("###########");
                                    swDevice.WriteLine("#DATABASE");
                                    break;
                            }
                            swDevice.WriteLine(_device.Key + "=1"); //写入数据
                        }
                    }
                }
                #endregion

                #endregion

                #region 清空缓冲
                swAlarm.Flush();
                swWeb.Flush();
                swDevice.Flush();
                #endregion

                #region 关闭流
                //关闭写入流
                swAlarm.Close();
                swWeb.Close();
                swDevice.Close();

                //关闭文件流
                fsAlarm.Close();
                fsWeb.Close();
                fsDevice.Close();
                #endregion
            }

            #endregion

            #endregion
        }

        /// <summary>
        /// 点击保存按钮，保存配置并回读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Save_Click(object sender, EventArgs e)
        {
            #region 3个textBox数组

            TextBox[] TbAlarm = new TextBox[]{
                                    tb_cloud_sms_enable, tb_cloud_version, tb_db_password, tb_db_url, tb_db_user, tb_local_listen_port,
                                    tb_local_listen_time_out, tb_redis_ip, tb_redis_port, tb_rpc_server_port, tb_serial_sms_com,
                                    tb_serial_sms_enable, tb_web_server_enable, tb_web_server_ip, tb_web_server_port
                                };
            TextBox[] TbWeb = new TextBox[]{
                                  tb__alarm_pop_url, tb__alarm_server_ip, tb__alarm_server_port, tb__alarm_url, tb__data_server_listen,
                                  tb__db_password, tb__db_url, tb__db_user, tb__device_server_ip, tb__device_server_port, tb__redis_ip,
                                  tb__redis_port, tb__web_listen_port, tb__web_rpc_listen_port, tb__web_socket,tb__web_socket_port
                              };
            TextBox[] TbDevice = new TextBox[]{
                                     tbalarm_server_enable, tbalarm_server_ip, tbalarm_server_port, tbdata_server_enable, tbdata_server_ip,
                                     tbdata_server_port, tbdb_password, tbdb_url, tbdb_user, tbdevice_center_from_listen, tbdynamic_monitor_1_enable,
                                     tbelectric_1_enable, tbelectric_1_listen_port, tbfire_1_enable, tbfire_1_listen_port, tbfire_2_enable,
                                     tbfire_2_listen_port, tbhk_false_data, tbhk_interval, tbhk_ip, tbhk_port, tbhk_send, tbwater_1_enable, tbwater_1_send_seconds,
                                     tbwater_2_enable, tbwater_2_listen_port, tbxhs_1_enable, tbxhs_1_listen_port
                                 };

            #endregion

            #region 检查是否 tbCountAll 个配置都填入textbox

            int tbCount = 0;
            for (int i = 0; i < alarmName.Length; i++)
            {
                if (TbAlarm[i].Text != "")
                {
                    tbCount++;
                }
            }

            for (int j = 0; j < webName.Length; j++)
            {
                if (TbWeb[j].Text != "")
                {
                    tbCount++;
                }
            }

            for (int k = 0; k < deviceName.Length; k++)
            {
                if (TbDevice[k].Text != "")
                {
                    tbCount++;
                }
            }

            #endregion

            #region if-else块，若所有配置文件都已填好，则可保存，并回读

            #region if块

            if (tbCount == tbCountAll) //一共tbCountAll个配置,tbcount为非空textbox的个数
            {
                //清空哈希表以重新使用
                alarm.Clear();
                web.Clear();
                device.Clear();

                #region 创建配置文件的哈希表

                alarm.Add("cloud_version", tb_cloud_version.Text);
                alarm.Add("redis_ip", tb_redis_ip.Text);
                alarm.Add("redis_port", tb_redis_port.Text);
                alarm.Add("local_listen_port", tb_local_listen_port.Text);
                alarm.Add("local_listen_time_out", tb_local_listen_time_out.Text);
                alarm.Add("rpc_server_port", tb_rpc_server_port.Text);
                alarm.Add("db_url", tb_db_url.Text);
                alarm.Add("db_user", tb_db_user.Text);
                alarm.Add("db_password", tb_db_password.Text);
                alarm.Add("web_server_enable", tb_web_server_enable.Text);
                alarm.Add("web_server_ip", tb_web_server_ip.Text);
                alarm.Add("web_server_port", tb_web_server_port.Text);
                alarm.Add("cloud_sms_enable", tb_cloud_sms_enable.Text);
                alarm.Add("serial_sms_enable", tb_serial_sms_enable.Text);
                alarm.Add("serial_sms_com", tb_serial_sms_com.Text);

                web.Add("web_listen_port", tb__web_listen_port.Text);
                web.Add("web_socket", tb__web_socket.Text);
                web.Add("web_socket_port", tb__web_socket_port.Text);
                web.Add("web_rpc_listen_port", tb__web_rpc_listen_port.Text);
                web.Add("redis_ip", tb__redis_ip.Text);
                web.Add("redis_port", tb__redis_port.Text);
                web.Add("db_url", tb__db_url.Text);
                web.Add("db_user", tb__db_user.Text);
                web.Add("db_password", tb__db_password.Text);
                web.Add("alarm_url", tb__alarm_url.Text);
                web.Add("alarm_pop_url", tb__alarm_pop_url.Text);
                web.Add("alarm_server_ip", tb__alarm_server_ip.Text);
                web.Add("alarm_server_port", tb__alarm_server_port.Text);
                web.Add("data_server_listen", tb__data_server_listen.Text);
                web.Add("device_server_ip", tb__device_server_ip.Text);
                web.Add("device_server_port", tb__device_server_port.Text);

                device.Add("hk_send", tbhk_send.Text);
                device.Add("hk_false_data", tbhk_false_data.Text);
                device.Add("hk_ip", tbhk_ip.Text);
                device.Add("hk_port", tbhk_port.Text);
                device.Add("hk_interval", tbhk_interval.Text);
                device.Add("device_center_from_listen", tbdevice_center_from_listen.Text);
                device.Add("alarm_server_enable", tbalarm_server_enable.Text);
                device.Add("alarm_server_ip", tbalarm_server_ip.Text);
                device.Add("alarm_server_port", tbalarm_server_port.Text);
                device.Add("data_server_enable", tbdata_server_enable.Text);
                device.Add("data_server_ip", tbdata_server_ip.Text);
                device.Add("data_server_port", tbdata_server_port.Text);
                device.Add("water_1_enable", tbwater_1_enable.Text);
                device.Add("water_1_send_seconds", tbwater_1_send_seconds.Text);
                device.Add("water_2_enable", tbwater_2_enable.Text);
                device.Add("water_2_listen_port", tbwater_2_listen_port.Text);
                device.Add("xhs_1_enable", tbxhs_1_enable.Text);
                device.Add("xhs_1_listen_port", tbxhs_1_listen_port.Text);
                device.Add("fire_1_enable", tbfire_1_enable.Text);
                device.Add("fire_1_listen_port", tbfire_1_listen_port.Text);
                device.Add("electric_1_enable", tbelectric_1_enable.Text);
                device.Add("electric_1_listen_port", tbelectric_1_listen_port.Text);
                device.Add("fire_2_enable", tbfire_2_enable.Text);
                device.Add("fire_2_listen_port", tbfire_2_listen_port.Text);
                device.Add("dynamic_monitor_1_enable", tbdynamic_monitor_1_enable.Text);
                device.Add("db_url", tbdb_url.Text);
                device.Add("db_user", tbdb_user.Text);
                device.Add("db_password", tbdb_password.Text);

                #endregion

                #region try-catch-finally块，保存并回读

                #region try块，保存

                try
                {
                    #region 创建文件流
                    FileStream fsAlarm = new FileStream(fileAlarm, FileMode.Create, FileAccess.Write); //新建Alarm文件流
                    FileStream fsWeb = new FileStream(fileWeb, FileMode.Create, FileAccess.Write); //新建Web文件流
                    FileStream fsDevice = new FileStream(fileDevice, FileMode.Create, FileAccess.Write); //新建Device文件流
                    StreamWriter swAlarm = new StreamWriter(fsAlarm); //新建Alarm写入流对象
                    StreamWriter swWeb = new StreamWriter(fsWeb); //新建Web写入流对象
                    StreamWriter swDevice = new StreamWriter(fsDevice); //新建Device写入流对象
                    #endregion

                    #region 配置文件写入

                    #region Alarm配置文件写入
                    for (int i = 0; i < alarmName.Length; i++)
                    {
                        foreach (DictionaryEntry _alarm in alarm)
                        {
                            if (_alarm.Key.ToString() == alarmName[i])
                            {
                                switch (i)
                                {
                                    case 0:
                                        swAlarm.WriteLine("#base information properties file");
                                        swAlarm.WriteLine("##########");
                                        swAlarm.WriteLine("#TYPE");
                                        break;
                                    case 1:
                                        swAlarm.WriteLine("##########");
                                        swAlarm.WriteLine("#REDIS");
                                        break;
                                    case 3:
                                        swAlarm.WriteLine("##########");
                                        swAlarm.WriteLine("#LISTEN DESKTOP CLIENT");
                                        break;
                                    case 5:
                                        swAlarm.WriteLine("##########");
                                        swAlarm.WriteLine("#LISTEN DATA FROM DEVICE/WEB CENTER TO OPERATION");
                                        break;
                                    case 6:
                                        swAlarm.WriteLine("##########");
                                        swAlarm.WriteLine("#DATABASE");
                                        break;
                                    case 9:
                                        swAlarm.WriteLine("##########");
                                        swAlarm.WriteLine("#SEND ALARM DATA TO WEB CENTER");
                                        break;
                                    case 12:
                                    case 13:
                                        swAlarm.WriteLine("##########");
                                        break;
                                }
                                swAlarm.WriteLine(_alarm.Key + "=" + _alarm.Value); //写入数据
                            }
                        }
                    }
                    swAlarm.WriteLine("##########");
                    #endregion

                    #region Web配置文件写入
                    for (int j = 0; j < webName.Length; j++)
                    {
                        foreach (DictionaryEntry _web in web)
                        {
                            if (_web.Key.ToString() == webName[j])
                            {
                                switch (j)
                                {
                                    case 0:
                                        swWeb.WriteLine("##### web config file #####");
                                        swWeb.WriteLine("# WEB");
                                        break;
                                    case 3:
                                        swWeb.WriteLine("##########");
                                        swWeb.WriteLine("# WEB RPC");
                                        break;
                                    case 4:
                                        swWeb.WriteLine("##########");
                                        swWeb.WriteLine("# REDIS");
                                        break;
                                    case 6:
                                        swWeb.WriteLine("##########");
                                        swWeb.WriteLine("#DATABASE");
                                        break;
                                    case 9:
                                        swWeb.WriteLine("##########");
                                        swWeb.WriteLine("#ALARM OPERATION");
                                        break;
                                    case 11:
                                        swWeb.WriteLine("##########");
                                        swWeb.WriteLine("# ALARM CENTER");
                                        break;
                                    case 13:
                                        swWeb.WriteLine("##########");
                                        swWeb.WriteLine("# DATA CENTER");
                                        break;
                                    case 14:
                                        swWeb.WriteLine("##########");
                                        swWeb.WriteLine("#DEVICE SERVER");
                                        break;
                                }
                                swWeb.WriteLine(_web.Key + "=" + _web.Value); //写入数据
                            }
                        }
                    }
                    #endregion

                    #region Device配置文件写入
                    for (int k = 0; k < deviceName.Length; k++)
                    {
                        foreach (DictionaryEntry _device in device)
                        {
                            if (_device.Key.ToString() == deviceName[k])
                            {
                                switch (k)
                                {
                                    case 0:
                                    case 5:
                                    case 6:
                                    case 9:
                                    case 12:
                                    case 14:
                                    case 16:
                                    case 18:
                                    case 20:
                                    case 22:
                                    case 24:
                                        swDevice.WriteLine("###########");
                                        break;
                                    case 25:
                                        swDevice.WriteLine("###########");
                                        swDevice.WriteLine("#DATABASE");
                                        break;
                                }
                                swDevice.WriteLine(_device.Key + "=" + _device.Value); //写入数据
                            }
                        }
                    }
                    #endregion
                    #endregion

                    MessageBox.Show("保存成功！");

                    #region 清空缓冲
                    swAlarm.Flush();
                    swWeb.Flush();
                    swDevice.Flush();
                    #endregion

                    #region 关闭流
                    //关闭写入流
                    swAlarm.Close();
                    swWeb.Close();
                    swDevice.Close();

                    //关闭文件流
                    fsAlarm.Close();
                    fsWeb.Close();
                    fsDevice.Close();
                    #endregion
                }

                #endregion

                #region catch块，捕获保存异常

                catch (Exception ex)
                {
                    MessageBox.Show("保存异常为：" + ex.Message); //若保存出错则弹框显示异常信息
                }

                #endregion

                #region finally块,回读配置

                finally
                {
                    //清空哈希表以重新使用
                    alarm.Clear();
                    web.Clear();
                    device.Clear();

                    #region 创建流
                    FileStream fsAlarm1 = new FileStream(fileAlarm, FileMode.OpenOrCreate, FileAccess.Read); //新建Alarm文件流
                    FileStream fsWeb1 = new FileStream(fileWeb, FileMode.OpenOrCreate, FileAccess.Read); //新建Web文件流
                    FileStream fsDevice1 = new FileStream(fileDevice, FileMode.OpenOrCreate, FileAccess.Read); //新建Device文件流
                    StreamReader srAlarm = new StreamReader(fsAlarm1); //新建Alarm读取流对象
                    StreamReader srWeb = new StreamReader(fsWeb1); //新建Web读取流对象
                    StreamReader srDevice = new StreamReader(fsDevice1); //新建Device读取流对象
                    #endregion

                    #region try-catch-finally块，回读

                    try
                    {
                        #region 将配置读取出来存入哈希表
                        while ((sAlarm = srAlarm.ReadLine()) != null)
                        {

                            //sAlarm = srAlarm.ReadLine();
                            while (!sAlarm.StartsWith("#") && !sAlarm.StartsWith(" "))
                            {
                                sAlarm = sAlarm.Replace(" ", ""); //将字符串中的空格过滤掉
                                index = sAlarm.IndexOf('='); //取得等号的索引值
                                arrAlarm[0] = sAlarm.Substring(0, index); //等号左边的字符串
                                arrAlarm[1] = sAlarm.Substring(index + 1); //等号右边的字符串
                                alarm.Add(arrAlarm[0], arrAlarm[1]); //存入哈希表
                                break;
                            }
                        }

                        while ((sWeb = srWeb.ReadLine()) != null)
                        {
                            //sWeb = srWeb.ReadLine();
                            while (!sWeb.StartsWith("#") && !sWeb.StartsWith(" "))
                            {
                                sWeb = sWeb.Replace(" ", "");
                                index = sWeb.IndexOf('=');
                                arrWeb[0] = sWeb.Substring(0, index);
                                arrWeb[1] = sWeb.Substring(index + 1);
                                web.Add(arrWeb[0], arrWeb[1]);
                                break;
                            }
                        }

                        while ((sDevice = srDevice.ReadLine()) != null)
                        {
                            //sDevice = srDevice.ReadLine();
                            while (!sDevice.StartsWith("#") && !sDevice.StartsWith(" "))
                            {
                                sDevice = sDevice.Replace(" ", "");
                                index = sDevice.IndexOf('=');
                                arrDevice[0] = sDevice.Substring(0, index);
                                arrDevice[1] = sDevice.Substring(index + 1);
                                device.Add(arrDevice[0], arrDevice[1]);
                                break;
                            }
                        }

                        #endregion

                        #region 将读取到的配置显示到相应的textbox中
                        for (int i = 0; i < TbAlarm.Length; i++)
                        {
                            foreach (DictionaryEntry _alarm in alarm)
                            {
                                if (("tb_" + _alarm.Key.ToString()) == TbAlarm[i].Name)
                                {
                                    TbAlarm[i].Text = _alarm.Value.ToString();
                                }
                            }
                        }

                        for (int i = 0; i < TbWeb.Length; i++)
                        {
                            foreach (DictionaryEntry _web in web)
                            {
                                if (("tb__" + _web.Key.ToString()) == TbWeb[i].Name)
                                {
                                    TbWeb[i].Text = _web.Value.ToString();
                                }
                            }
                        }

                        for (int i = 0; i < TbDevice.Length; i++)
                        {
                            foreach (DictionaryEntry _device in device)
                            {
                                if (("tb" + _device.Key.ToString()) == TbDevice[i].Name)
                                {
                                    TbDevice[i].Text = _device.Value.ToString();
                                }
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("回读异常为：" + ex.Message);
                    }

                    finally
                    {
                        #region 关闭流
                        //关闭读取流
                        srAlarm.Close();
                        srWeb.Close();
                        srDevice.Close();

                        //关闭文件流
                        fsAlarm1.Close();
                        fsWeb1.Close();
                        fsDevice1.Close();
                        #endregion
                    }
                }

                #endregion
            }
            #endregion

            #endregion

            #endregion

            #region else块

            else
            {
                MessageBox.Show("    请输入所有配置");
            }

            #endregion

            #endregion
        }
    }
}