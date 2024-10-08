﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Diagnostics;
using System.Web;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Project_app.Models;
using System.Windows.Media.Imaging;
using Microsoft.Data.SqlClient;
using CefSharp.DevTools.Page;
using System.Data;
using System.Net.Http;

namespace Project_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        bool coldCase = false; // 즐겨찾기인지, API로 검색한건지/ True = openAPI, True = 즐겨찾기 보기

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TxtAnimalKind.Focus();
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrEmpty(TxtAnimalKind.Text))
            //{
            //    await this.ShowMessageAsync("조회", "조회할 동물종을 입력하세요.");
            //    return;
            //}

            SearchAnimal(TxtAnimalKind.Text);
            coldCase = false; // 검색은 즐겨찾기 보기 아님
            ImgPoster.Source = new BitmapImage(new Uri("/No_Picture.png", UriKind.RelativeOrAbsolute));
        }

        private async void SearchAnimal(string animalKind)
        {
            string openApiUri = "http://apis.data.go.kr/6260000/BusanPetAnimalInfoService/getPetAnimalInfo?serviceKey=z6BhxEUBu1diXG%2FWmiJHqqj5SbvVqzr%2BikJSCKelgBVoiUaOonc3nsfQn5S9bQAfr9NIkJSf5qPojF%2BPYaR8qg%3D%3D&numOfRows=10&pageNo=1&resultType=json";
            string result = string.Empty;

            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApiUri);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();

                // JSON 결과 출력 (디버깅용)
                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"OpenAPI 조회오류 {ex.Message}");
                return;
            }

            JObject jsonResult = JObject.Parse(result);

            // 전체 JSON 결과를 확인하고 구조를 확인합니다.
            Debug.WriteLine(jsonResult.ToString());

            // Null 체크 및 JSON 구조에 따른 수정
            if (jsonResult["response"]?["header"]?["resultCode"] == null)
            {
                await this.ShowMessageAsync("오류", "OpenAPI 응답 형식 오류");
                return;
            }

            var resultCode = Convert.ToInt32(jsonResult["response"]["header"]["resultCode"]);

            if (resultCode == 00)
            {
                var data = jsonResult["response"]["body"]["items"]["item"];
                var jsonArray = data as JArray;

                var animalRescues = new List<AnimalRescue>();
                foreach (var item in jsonArray)
                {
                    var ty3Kind = Convert.ToString(item["ty3Kind"]);
                    if (string.IsNullOrEmpty(animalKind) || (!string.IsNullOrEmpty(ty3Kind) && ty3Kind.Contains(animalKind, StringComparison.OrdinalIgnoreCase)))
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
                            Ty3Kind = ty3Kind,
                            Sj = Convert.ToString(item["sj"]),
                            Ty3Process = Convert.ToString(item["ty3Process"]),
                        });
                    }
                }

                if (animalRescues.Count > 0)
                {
                    this.DataContext = animalRescues;
                }
                else
                {
                    await this.ShowMessageAsync("검색 결과", "해당 종에 대한 검색 결과가 없습니다.");
                }
            }
            else
            {
                await this.ShowMessageAsync("오류", $"OpenAPI 응답 오류: {resultCode}");
            }
        }


        private void TxtAnimalKind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearch_Click(sender, e); // 검색 버튼클릭 이벤트핸들러 실행
            }
        }

        // HttpClient 객체를 클래스 수준에서 정의
        private readonly HttpClient _httpClient = new HttpClient();

        private async void GrdResult_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            // 재검색하면 데이터그리드 결과가 바뀌면서 이 이벤트가 다시 발생
            try
            {
                var animal = GrdResult.SelectedItem as AnimalRescue;

                if (animal == null)
                {
                    return; // animal이 null인 경우 아무 작업도 하지 않음
                }

                var ty3Picture = animal.Ty3Picture;

                if (string.IsNullOrEmpty(ty3Picture))
                {
                    ImgPoster.Source = new BitmapImage(new Uri("/No_Picture.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    // 사진 URL에서 이미지 스트림 가져오기
                    var imageStream = await _httpClient.GetStreamAsync(ty3Picture);

                    // BitmapImage 객체 생성 및 설정
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = imageStream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                    // 이미지를 Image 컨트롤에 표시
                    ImgPoster.Source = bitmapImage;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
            }
        }

        // 클래스가 IDisposable 인터페이스를 구현하여 HttpClient 객체를 정리
        public class YourClassName : IDisposable
        {
            private readonly HttpClient _httpClient = new HttpClient();

            public void Dispose()
            {
                _httpClient?.Dispose();
            }
        }


        //private async void BtnWatchTrailer_Click(object sender, RoutedEventArgs e)
        //{
        //    if (GrdResult.SelectedItems.Count == 0)
        //    {
        //        await this.ShowMessageAsync("예고편 보기", "영화를 선택하세요.");
        //        return;
        //    }

        //    if (GrdResult.SelectedItems.Count > 1)
        //    {
        //        await this.ShowMessageAsync("예고편 보기", "영화를 하나만 선택하세요.");
        //        return;
        //    }

        //    var movieName = (GrdResult.SelectedItem as MovieItem).Title;

        //    var trailerWindow = new TrailerWindow(movieName);
        //    trailerWindow.Owner = this;
        //    trailerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        //    trailerWindow.ShowDialog();
        //}


        // 데이터그리드 더블클릭시 발생 이벤트핸들러
        private async void GrdResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var curItem = GrdResult.SelectedItem as AnimalRescue;

            //var mapWindow = new MapWindow(curItem.Coordy, curItem.Coordx);
            //mapWindow.Owner = this;
            //mapWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //mapWindow.ShowDialog();
        }

        private async void BtnAddColdcase_Click(object sender, RoutedEventArgs e)
        {
            //await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 추가합니다.");
            if (GrdResult.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("미인수건", "미인수건에 추가할 데이터를 선택하세요 (복수선택 가능)");
                return;
            }

            if (coldCase == true) 
            {
                await this.ShowMessageAsync("미인수건", "이미 추가한 데이터입니다.");
                return;
            }

            var addanimals = new List<AnimalRescue>();
            foreach (AnimalRescue item in GrdResult.SelectedItems)
            {
                addanimals.Add(item);
            }

            Debug.WriteLine(addanimals.Count);
            try
            {
                var insRes = 0;
                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    foreach (AnimalRescue item in addanimals)
                    {
                        // 저장되기 전에 이미 저장된 데이터인지 확인 후
                        SqlCommand chkCmd = new SqlCommand(AnimalRescue.CHECK_QUERY, conn);
                        chkCmd.Parameters.AddWithValue("@Cn", item.Cn);
                        var cnt = Convert.ToInt32(chkCmd.ExecuteNonQuery()); // COUNT(*) 등의 1row, 1coloumn값을 리턴할 때

                        if (cnt > 1) continue; // 이미 데이터가 있으면 패스

                        SqlCommand cmd = new SqlCommand(Models.AnimalRescue.INSERT_QUERY, conn);

                        cmd.Parameters.AddWithValue("@ty3Ingye", item.Ty3Ingye);
                        cmd.Parameters.AddWithValue("@writngDe", item.WritngDe);
                        cmd.Parameters.AddWithValue("@ty3Place", item.Ty3Place);
                        cmd.Parameters.AddWithValue("@ty3Sex", item.Ty3Sex);
                        cmd.Parameters.AddWithValue("@cn", item.Cn);
                        cmd.Parameters.AddWithValue("@ty3Date", item.Ty3Date);
                        cmd.Parameters.AddWithValue("@wrter", item.Wrter);
                        cmd.Parameters.AddWithValue("@ty3Insu", item.Ty3Insu);
                        cmd.Parameters.AddWithValue("@ty3Picture", item.Ty3Picture);
                        cmd.Parameters.AddWithValue("@ty3Kind", item.Ty3Kind);
                        cmd.Parameters.AddWithValue("@sj", item.Sj);
                        cmd.Parameters.AddWithValue("@ty3Process", item.Ty3Process);

                        insRes += cmd.ExecuteNonQuery(); // 데이터 하나마다 INSERT 쿼리 실행
                    }
                }
                if (insRes == addanimals.Count)
                {
                    await this.ShowMessageAsync("미인수건", $"미인수건 {insRes}건 저장성공!");
                }
                else
                {
                    await this.ShowMessageAsync("미인수건", $"미인수건 {addanimals.Count}건중 {insRes}건 저장성공!");
                }
            }
            catch (Exception ex)
            {

                await this.ShowMessageAsync("오류", $"미인수건 오류 {ex.Message}");
            }

            BtnViewColdcase_Click(sender, e); //저장 후 정자된 즐겨찾기 바로보기
        }

        private async void BtnViewColdcase_Click(object sender, RoutedEventArgs e)
        {
            //await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 확인합니다.");
            this.DataContext = null; // 데이터그리드에 보낸 데이터를 모두 삭제
            TxtAnimalKind.Text = string.Empty;

            List<AnimalRescue> NCAnimals = new List<AnimalRescue>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    // var : 내부에서 사용하는 동적 선언
                    var cmd = new SqlCommand(Models.AnimalRescue.SELECT_QUERY, conn);
                    var adapter = new SqlDataAdapter(cmd);
                    var dSet = new DataSet();
                    adapter.Fill(dSet, "AnimalRescue");

                    foreach (DataRow row in dSet.Tables["AnimalRescue"].Rows)
                    {
                        var animalRescue = new AnimalRescue()
                        {
                            Ty3Ingye = Convert.ToString(row["ty3Ingye"]),
                            WritngDe = Convert.ToDateTime(row["writngDe"]),
                            Ty3Place = Convert.ToString(row["ty3Place"]),
                            Ty3Sex = Convert.ToString(row["ty3Sex"]),
                            Cn = Convert.ToString(row["cn"]),
                            Ty3Date = Convert.ToString(row["ty3Date"]),
                            Wrter = Convert.ToString(row["wrter"]),
                            Ty3Insu = Convert.ToString(row["ty3Insu"]),
                            Ty3Picture = Convert.ToString(row["ty3Picture"]),
                            Ty3Kind = Convert.ToString(row["ty3Kind"]),
                            Sj = Convert.ToString(row["sj"]),
                            Ty3Process = Convert.ToString(row["ty3Process"])

                        };

                        NCAnimals.Add(animalRescue);
                    }
                    this.DataContext = NCAnimals;
                    coldCase = true; // 즐겨찾기 DB에서
                    //StsResult.Content = $"즐겨찾기 {NCAnimals.Count}건 조회 완료";
                    ImgPoster.Source = new BitmapImage(new Uri("/No_Picture.png", UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"미인수건 조회 오류 {ex.Message}");
            }
        }

        private async void BtnDelColdcase_Click(object sender, RoutedEventArgs e)
        {
            // await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 삭제합니다.");
            if (coldCase == false)
            {
                await this.ShowMessageAsync("삭제", "추가한 사건이 아닙니다.");
                return;
            }

            if (GrdResult.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("삭제", "삭제할 사건을 선택하세요.");
                return;
            }

            // 삭제시작!
            try
            {
                using (SqlConnection conn = new SqlConnection(Helpers.Common.CONNSTRING))
                {
                    conn.Open();

                    var delRes = 0;
                    foreach (AnimalRescue item in GrdResult.SelectedItems)
                    {
                        SqlCommand cmd = new SqlCommand(Models.AnimalRescue.DELETE_QUERY, conn);
                        cmd.Parameters.AddWithValue("@cn", item.Cn);

                        delRes += cmd.ExecuteNonQuery();
                    }

                    if (delRes == GrdResult.SelectedItems.Count)
                    {
                        await this.ShowMessageAsync("삭제", $"미인수건 {delRes}건 삭제");
                    }
                    else
                    {
                        await this.ShowMessageAsync("삭제", $"미인수건 {GrdResult.SelectedItems.Count}건중 {delRes}건 삭제");
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"미인수건 삭제 오류 {ex.Message}");
            }

            BtnViewColdcase_Click(sender, e); // 즐겨찾기 보기 재실행
        }
    }


}