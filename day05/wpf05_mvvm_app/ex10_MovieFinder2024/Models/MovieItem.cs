// TMDB API로 넘어온 검색 결과를 담는 객체, List<MovieItem>

namespace ex10_MovieFinder2024.Models
{
    public class MovieItem
    {
        public bool Adult { get; set; }
        public int Id { get; set; } // TMDB Key
        public string Original_Language { get; set; }
        public string Original_Title { get; set;}
        public string Overview { get; set; }
        public double Popularity { get; set; }
        public string Poster_Path { get; set; }
        public string Release_Date { get; set; }
        public string Title { get; set; }
        public double Vote_Average { get; set; }
        public int Vote_Count { get; set; }
    }
}
