using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please Enter Start!");
            var _message = Console.ReadLine();
            //httpTest();
            http();
        }
        private static void httpTest()
        {
            MessageBody model = new MessageBody();
            model.Account = "System";
            model.Message = "您有一笔承租合同待审核";
            HttpHelper.Post("http://localhost:10086/api/Push/send", Encoding.UTF8.GetBytes(model.ToJson()));
        }

        private static void http() {

            List<string> userList = new List<string> { "System" };

            #region 要传递的数据
            MessageBody2 model = new MessageBody2();
            model.app_key = ConfigurationManager.AppSettings["app_key"].ToString();
            model.userName = ConfigurationManager.AppSettings["userName"].ToString();
            model.passWord = ConfigurationManager.AppSettings["passWord"].ToString();
            model.timestamp = Unixtimestamp.ConvertDateTimeInt(DateTime.Now).ToString();//生成时间戳
            model.Account = userList;
            model.Message = "测试测试测试测试测试";
            model.NoticeId = "";
            #endregion

            var jsondata = model.ToJson();

            StringBuilder signSB = new StringBuilder();//密文使用

            //添加业务参数
            signSB.Append(ConfigurationManager.AppSettings["secret_key"].ToString()); //密钥
            signSB.Append(jsondata);
            signSB.Append(ConfigurationManager.AppSettings["secret_key"].ToString());//密钥     

            string temp = Md5Helper.MD5(signSB.ToString() + jsondata, 32);//生成的密文

            ResultModel json = new ResultModel();
            json.Sign = temp;
            json.ResultData = jsondata;

            HttpHelper.Post(ConfigurationManager.AppSettings["SignalRBusSend"].ToString(), Encoding.UTF8.GetBytes(json.ToJson()));
        }
    }
}
