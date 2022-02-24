using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class JsonAgentVM
    {
        public string Adress { get; set; }
        public string AgentFullName { get; set; }
        public string AgncFldCod { get; set; }
        public string AgncFldCod_int { get; set; }
        public string AgncType { get; set; }
        public DateTime? BgnDte { get; set; }
        public long? CmpCod { get; set; }
        public string CtyName { get; set; }
        public long? LglCod { get; set; }
        public string LglName { get; set; }
        public string PrvnNam { get; set; }
        public long? StsCod { get; set; }
        public string Tel { get; set; }
        public string mapAgnc { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Zoom { get; set; }
        public long? AgncFldOfcCod { get; set; }

        public static string getCompanyTitle (string agentCode)
        {
            string tempAllCompany = "[{\"text\":\"بيمه دانا\",\"value\":\"1\"},{\"text\":\"بيمه ايران\",\"value\":\"2\"},{\"text\":\"بيمه آسيا\",\"value\":\"3\"},{\"text\":\"بيمه دي\",\"value\":\"4\"},{\"text\":\"بيمه ميهن\",\"value\":\"5\"},{\"text\":\"بيمه البرز\",\"value\":\"6\"},{\"text\":\"بيمه معلم\",\"value\":\"7\"},{\"text\":\"بيمه پارسيان\",\"value\":\"8\"},{\"text\":\"بيمه كارآفرين\",\"value\":\"9\"},{\"text\":\"بيمه سينا\",\"value\":\"10\"},{\"text\":\"بيمه رازي\",\"value\":\"11\"},{\"text\":\"بيمه توسعه\",\"value\":\"12\"},{\"text\":\"بيمه ملت\",\"value\":\"13\"},{\"text\":\"بيمه سامان\",\"value\":\"15\"},{\"text\":\"بيمه نوين\",\"value\":\"16\"},{\"text\":\"بيمه پاسارگاد\",\"value\":\"17\"},{\"text\":\"بيمه كوثر\",\"value\":\"18\"},{\"text\":\"بيمه ما\",\"value\":\"19\"},{\"text\":\"بيمه آرمان\",\"value\":\"21\"},{\"text\":\"دانا(شركت هاي ادغامي)\",\"value\":\"22\"},{\"text\":\"بيمه حافظ\",\"value\":\"23\"},{\"text\":\"بيمه اميد\",\"value\":\"24\"},{\"text\":\"بيمه ايران معين\",\"value\":\"25\"},{\"text\":\"بيمه متقابل كيش\",\"value\":\"26\"},{\"text\":\"بيمه زندگي باران\",\"value\":\"27\"},{\"text\":\"بيمه اتكايي ايرانيان\",\"value\":\"28\"},{\"text\":\"بيمه تعاون\",\"value\":\"29\"},{\"text\":\"بيمه اتكايي امين\",\"value\":\"30\"},{\"text\":\"بيمه آسماري\",\"value\":\"31\"},{\"text\":\"بيمه متقابل اطمينان متحد قشم\",\"value\":\"32\"},{\"text\":\"بيمه سرمد\",\"value\":\"33\"},{\"text\":\"بيمه تجارت نو\",\"value\":\"34\"},{\"text\":\"بيمه زندگي خاورميانه\",\"value\":\"35\"},{\"text\":\"اتکایی بیمه مرکزی ج.ا.ا\",\"value\":\"50\"},{\"text\":\"حساب اتكايي ويژه\",\"value\":\"51\"},{\"text\":\"بيمه حكمت صبا\",\"value\":\"52\"},{\"text\":\"بیمه اتکایی سامان\",\"value\":\"53\"},{\"text\":\"بیمه فردا\",\"value\":\"54\"},{\"text\":\"بيمه آواي پارس\",\"value\":\"55\"},{\"text\":\"بيمه تهران رواک\",\"value\":\"56\"},{\"text\":\"بیمه پردیس\",\"value\":\"57\"},{\"text\":\"بیمه زندگی کاریزما\",\"value\":\"58\"}]";
            var allCompanyObj = JsonConvert.DeserializeObject<List<companyTemp>>(tempAllCompany);
            var foundItem = allCompanyObj.Where(t => t.value == agentCode).FirstOrDefault();
            if(foundItem != null)
                return foundItem.text;

            return "";
        }

        public class companyTemp
        {
            public string text { get; set; }
            public string value { get; set; }
        }
    }
}
