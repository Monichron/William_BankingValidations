using BankingValidations2._3.NetCash;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingValidations2._3
{
    class Validation
    {
        public string Doc = @"C:\Users\William\notes\ErrorLog.txt";
        public CreateSqlConnection _SQL { get; set; }
        DataTable _BankingDetailsTable;
        public NIWS_PartnerClient _Client { get; set; }
        public ServiceInfo _ServiceInfo { get; set; }
        public ValidateServiceKeyRequest _ValidateServiceKeyRequest { get; set; }
        public ServiceInfoList _ServiceInfoList { get; set; }
        public ServiceInfoResponseList _ServiceInfoResponseList { get; set; }
        public Validation()
        {
            string fileName = ConfigurationManager.AppSettings["DocumentPath"];
            //Doc = @fileName;
            // Check if file already exists. If yes, delete it.     
            if (File.Exists(Doc) == false)
            {
                using (StreamWriter sw = File.CreateText(Doc))
                {
                }
            }
            using (StreamWriter sw = File.AppendText(Doc))
            {
                sw.WriteLine("****************************************************************************************************************");
                sw.WriteLine("New file created: {0}", DateTime.Now.ToString());
            }

            _ServiceInfo = new ServiceInfo();
            _ServiceInfoList = new ServiceInfoList();
            _ServiceInfoResponseList = new ServiceInfoResponseList();
            //class creates a sql connection then fills a table from sql
            CreateSqlConnection SQL = new CreateSqlConnection();
            _SQL = SQL;
            _BankingDetailsTable = _SQL.FetchTable();
            GetServiceInfo("5", "ba3c25ae-4d78-4d15-b3b3-a4a1394c155c");
            ValidateServiceKey();
        }

        protected ServiceInfoList GetServiceInfo(string pServiceID, string pServiceKey)
        {
            _ServiceInfo.ServiceId = pServiceID;
            _ServiceInfo.ServiceKey = pServiceKey;
            _ServiceInfoList.Add(_ServiceInfo);
            return _ServiceInfoList;
        }
        protected void ValidateServiceKey()
        {
            _Client = new NIWS_PartnerClient();
            _ValidateServiceKeyRequest = new ValidateServiceKeyRequest();
            //---------------------------------------
            string SoftwareVendorKey = "24ade73c-98cf-47b3-99be-cc7b867b3080";
            _ValidateServiceKeyRequest.SoftwareVendorKey = SoftwareVendorKey;
            _ValidateServiceKeyRequest.ServiceInfoList = _ServiceInfoList;
            _ValidateServiceKeyRequest.MerchantAccount = "51894408351";
            //---------------------------------------
            var Request = _Client.ValidateServiceKey(_ValidateServiceKeyRequest);
            using (StreamWriter sw = File.AppendText(Doc))
            {
                sw.WriteLine($"Service Key Status:{Request.ServiceInfo[0].ServiceStatus}");

            }
            if (Request.AccountStatus == "001")
            {
                _ServiceInfoResponseList = Request.ServiceInfo;
                foreach (var s in _ServiceInfoResponseList)
                {
                    string service = s.ServiceId;
                    switch (service)
                    {
                        case "2": //do something
                            break;
                        case "3": //do something
                            break;
                        case "5":
                            try
                            {
                                NetCashValidationService.NIWS_ValidationClient client = new NetCashValidationService.NIWS_ValidationClient();
                                try
                                {
                                    foreach (DataRow item in _BankingDetailsTable.Rows)
                                    {
                                        var holder = client.ValidateBankAccount("ba3c25ae-4d78-4d15-b3b3-a4a1394c155c", item.ItemArray[1].ToString(), item.ItemArray[2].ToString(), item.ItemArray[3].ToString());
                                        using (StreamWriter sw = File.AppendText(Doc))
                                        {
                                            sw.WriteLine($"PRACTICE NUMBER: {item.ItemArray[0].ToString()} Responce {holder}");
                                        }
                                        _SQL.LogResponce(item.ItemArray[0].ToString(),holder);

                                    }

                                }
                                catch (Exception e)
                                {
                                    using (StreamWriter sw = File.AppendText(Doc))
                                    {
                                        sw.WriteLine($"VALIDATION ERROR:{e.Message}");
                                    }
                                   
                                    continue;
                                }

                            }
                            catch (Exception e)
                            {

                                using (StreamWriter sw = File.AppendText(Doc))
                                {
                                    sw.WriteLine($"CLIENT ERROR:{e.Message}");
                                }
                            }
                            break;
                        case "10": //do something
                            break;
                        case "14": //do something
                            break;
                        default: //do something
                            break;
                    }
                }
            }
            else
            {
                //output if account not active
            }
            //Close client
            _Client.Close();
        }


        //    public string CreateEntity()
        //    {
        //        List<string> BankDetails = new List<string>();
        //        if (_BankingDetailsTable !=)
        //        {
        //            foreach (DataRow row in _BankingDetailsTable.Rows)
        //            {
        //                BankDetails[0] = row["AccountNumber"].ToString();
        //                BankDetails[1] = row["BranchNumber"].ToString();
        //                BankDetails[0] = row[""].ToString();
        //                BankDetails[0] = row["Account"].ToString();
        //                BankDetails[0] = row["Account"].ToString();
        //                BankDetails[0] = row["Account"].ToString();

        //            }
        //        }

        //    }
        //}
        public class holder
        {

        }
    }
}
