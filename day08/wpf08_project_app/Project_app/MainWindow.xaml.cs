using Project_app.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;
using Project_app.Models;

namespace Project_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            InitComboDateFromDB();
        }

        private void InitComboDateFromDB()
        {
            
        }

        private async void BtnReqRealtime_Click(object sender, RoutedEventArgs e)
        {
            string openApiUri = "http://apis.data.go.kr/1262000/TravelAlarmService2/getTravelAlarmList2?serviceKey=z6BhxEUBu1diXG%2FWmiJHqqj5SbvVqzr%2BikJSCKelgBVoiUaOonc3nsfQn5S9bQAfr9NIkJSf5qPojF%2BPYaR8qg%3D%3D";
            string result = string.Empty;

            // WebRequest, WebResponse 객체
            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApiUri);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();

                // await this.ShowMessageAsync("결과", result);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"OpenAPI 조회오류 {ex.Message}");
            }
            var jsonResult = JObject.Parse(result);
            var status = Convert.ToInt32(jsonResult["currentCount"]);

            if (status > 0)
            {
                var data = jsonResult["data"];
                var jsonArray = data as JArray; // json자체에서 []안에 들어간 배열데이터만 Array 변환가능

                var travelalarmservice = new List<TravelAlarmService>();
                foreach (var item in jsonArray)
                {
                    travelalarmservice.Add(new TravelAlarmService()
                    {
                        alarm_lvl = Convert.ToInt32(item["alarm_lvl"]),
                        continent_cd = Convert.ToString(item["continent_cd"]),
                        continent_eng_nm = Convert.ToString(item["continent_eng_nm"]),
                        continent_nm = Convert.ToString(item["continent_nm"]),
                        country_eng_nm = Convert.ToString(item["country_eng_nm"]),
                        country_iso_alp2 = Convert.ToString(item["country_iso_alp2"]),
                        country_nm = Convert.ToString(item["country_nm"]),
                        dang_map_download_url = Convert.ToString(item["dang_map_download_url"]),
                        flag_download_url = Convert.ToString(item["flag_download_url"]),
                        map_download_url = Convert.ToString(item["map_download_url"]),
                        region_ty = Convert.ToString(item["region_ty"]),
                        remark = Convert.ToString(item["remark"]),
                        written_dt = Convert.ToDateTime(item["written_dt"])
                    });
                }

                this.DataContext = travelalarmservice;
                StsResult.Content = $"OpenAPI {travelalarmservice.Count}건 조회완료!";
            }
        }

        private void CboReqDate_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void GrdResult_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}