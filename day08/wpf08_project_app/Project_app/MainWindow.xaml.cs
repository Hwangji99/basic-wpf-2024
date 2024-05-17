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
using System.Diagnostics;
using System.Windows.Media.Imaging;

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
            string openApiUri = "http://apis.data.go.kr/6260000/BusanPetAnimalInfoService/getPetAnimalInfo?serviceKey=z6BhxEUBu1diXG%2FWmiJHqqj5SbvVqzr%2BikJSCKelgBVoiUaOonc3nsfQn5S9bQAfr9NIkJSf5qPojF%2BPYaR8qg%3D%3D&numOfRows=10&pageNo=1&resultType=json";
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
            var resultCode = Convert.ToInt32(jsonResult["getPetAnimalInfo"]["header"]["resultCode"]);

            if (resultCode == 00)
            {
                var data = jsonResult["getPetAnimalInfo"]["body"]["items"]["item"];
                var jsonArray = data as JArray; // json자체에서 []안에 들어간 배열데이터만 Array 변환가능

                var animalRescues = new List<AnimalRescue>();
                foreach (var item in jsonArray)
                {
                    animalRescues.Add(new AnimalRescue()
                    {
                        Ty3Ingye = Convert.ToString(item["ty3Ingye"]),
                        WritngDe = Convert.ToDateTime(item["writngDe"]),
                        Ty3Place = Convert.ToString(item["ty3Place"]),
                        Ty3Sex = Convert.ToString(item["ty3Sex"]),
                        Cn = Convert.ToString(item["cn"]),
                        Ty3Date = Convert.ToString(item["ty3Date"]),
                        Wrter = Convert.ToString(item["wrter"]),
                        Ty3Insu = Convert.ToString(item["ty3Insu"]),
                        Ty3Picture = Convert.ToString(item["ty3Picture"]),
                        Ty3Kind = Convert.ToString(item["ty3Kind"]),
                        Sj = Convert.ToString(item["sj"]),
                        Ty3Process = Convert.ToString(item["ty3Process"]),
                    });
                }

                this.DataContext = animalRescues;
                StsResult.Content = $"OpenAPI {animalRescues.Count}건 조회완료!";
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