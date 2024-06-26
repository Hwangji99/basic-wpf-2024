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
using ex10_MovieFinder2024.Models;
using System.Windows.Media.Imaging;
using Microsoft.Data.SqlClient;
using CefSharp.DevTools.Page;
using System.Data;

namespace Project_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        bool isFavorite = false; // 즐겨찾기인지, API로 검색한건지/ True = openAPI, True = 즐겨찾기 보기

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            // await this.ShowMessageAsync("검색", "검색을 시작합니다!!");
            if (string.IsNullOrEmpty(TxtAnimalName.Text))
            {
                await this.ShowMessageAsync("검색", "검색할 동물명을 입력하세요.");
                return;
            }

            SearchMovie(TxtAnimalName.Text);
            isFavorite = false; // 검색은 즐겨찾기 보기 아님
            ImgPoster.Source = new BitmapImage(new Uri("/No_Picture.png", UriKind.RelativeOrAbsolute));
        }

        private async void SearchMovie(string movieName)
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

        private void TxtMovieName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearch_Click(sender, e); // 검색 버튼클릭 이벤트핸들러 실행
            }
        }

        private async void GrdResult_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            // 재검색하면 데이터그리드 결과가 바뀌면서 이 이벤트가 다시 발생
            try
            {
                var animal = GrdResult.SelectedItem as AnimalRescue;
                var ty3Picture = animal.Ty3Picture

                // await this.ShowMessageAsync("포스터", poster_path);
                if (string.IsNullOrEmpty(ty3Picture))
                {
                    ImgPoster.Source = new BitmapImage(new Uri("/No_Picture.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    var base_url = "http://apis.data.go.kr/6260000/BusanPetAnimalInfoService/getPetAnimalInfo?serviceKey=z6BhxEUBu1diXG%2FWmiJHqqj5SbvVqzr%2BikJSCKelgBVoiUaOonc3nsfQn5S9bQAfr9NIkJSf5qPojF%2BPYaR8qg%3D%3D&numOfRows=10&pageNo=1&resultType=json";
                    ImgPoster.Source = new BitmapImage(new Uri($"{base_url}{ty3Picture}", UriKind.Absolute));
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
            }
        }

        // 즐겨찾기 조회
        private async void BtnViewFavorite_Click(object sender, RoutedEventArgs e)
        {
            //await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 확인합니다.");
            this.DataContext = null; // 데이터그리드에 보낸 데이터를 모두 삭제
            TxtAnimalName.Text = string.Empty;

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
                    isFavorite = true; // 즐겨찾기 DB에서
                    StsResult.Content = $"즐겨찾기 {NCAnimals.Count}건 조회 완료";
                    ImgPoster.Source = new BitmapImage(new Uri("/No_Picture.png", UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"즐겨찾기 조회 오류 {ex.Message}");
            }
        }

        // 즐겨찾기 삭제
        private async void BtnDelFavorite_Click(object sender, RoutedEventArgs e)
        {
            // await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 삭제합니다.");
            if (isFavorite == false)
            {
                await this.ShowMessageAsync("삭제", "즐겨찾기한 영화가 아닙니다.");
                return;
            }

            if (GrdResult.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("삭제", "삭제할 영화를 선택하세요.");
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
                        SqlCommand cmd = new SqlCommand(Models.MovieItem.DELETE_QUERY, conn);
                        cmd.Parameters.AddWithValue("@Id", item.Id);

                        delRes += cmd.ExecuteNonQuery();
                    }

                    if (delRes == GrdResult.SelectedItems.Count)
                    {
                        await this.ShowMessageAsync("삭제", $"즐겨찾기 {delRes}건 삭제");
                    }
                    else
                    {
                        await this.ShowMessageAsync("삭제", $"즐겨찾기 {GrdResult.SelectedItems.Count}건중 {delRes}건 삭제");
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"즐겨찾기 삭제 오류 {ex.Message}");
            }

            BtnViewFavorite_Click(sender, e); // 즐겨찾기 보기 재실행
        }

        // 즐겨찾기 추가
        private async void BtnAddFavorite_Click(object sender, RoutedEventArgs e)
        {
            //await this.ShowMessageAsync("즐겨찾기", "즐겨찾기 추가합니다.");
            if (GrdResult.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("즐겨찾기", "즐겨찾기에 추가 할 영화를 선택하세요 (복수선택 가능)");
                return;
            }

            if (isFavorite == true) // 즐겨찾기 보기한 뒤 영화를 다시 즐겨찾기하려고 할 때 막음
            {
                await this.ShowMessageAsync("즐겨찾기", "이미 즐겨찾기한 영화입니다.");
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

                        cmd.Parameters.AddWithValue("@ty3Ingye", item.ty3Ingye);
                        cmd.Parameters.AddWithValue("@writngDe", item.writngDe);
                        cmd.Parameters.AddWithValue("@ty3Place", item.ty3Place);
                        cmd.Parameters.AddWithValue("@ty3Sex", item.ty3Sex);
                        cmd.Parameters.AddWithValue("@cn", item.cn);
                        cmd.Parameters.AddWithValue("@ty3Date", item.ty3Date);
                        cmd.Parameters.AddWithValue("@wrter", item.wrter);
                        cmd.Parameters.AddWithValue("@ty3Insu", item.ty3Insu);
                        cmd.Parameters.AddWithValue("@ty3Picture", item.ty3Picture);
                        cmd.Parameters.AddWithValue("@ty3Kind", item.ty3Kind);
                        cmd.Parameters.AddWithValue("@sj", item.sj);
                        cmd.Parameters.AddWithValue("@ty3Process", item.ty3Process);

                        insRes += cmd.ExecuteNonQuery(); // 데이터 하나마다 INSERT 쿼리 실행
                    }
                }
                if (insRes == addanimals.Count)
                {
                    await this.ShowMessageAsync("즐겨찾기", $"즐겨찾기 {insRes}건 저장성공!");
                }
                else
                {
                    await this.ShowMessageAsync("즐겨찾기", $"즐겨찾기 {addanimals.Count}건중 {insRes}건 저장성공!");
                }
            }
            catch (Exception ex)
            {

                await this.ShowMessageAsync("오류", $"즐겨찾기 오류 {ex.Message}");
            }

            BtnViewFavorite_Click(sender, e); //저장 후 정자된 즐겨찾기 바로보기
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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TxtAnimalName.Focus();
        }

        // 데이터그리드 더블클릭시 발생 이벤트핸들러
        private async void GrdResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine(GrdResult.SelectedItem);
            //var curItem = GrdResult.SelectedItem as MovieItem;

            //await this.ShowMessageAsync($"{curItem.Title} ({curItem.Overview})", curItem.Overview);
        }
    }
}