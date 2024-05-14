using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_app.Models
{
    public class TravelAlarmService
    {
        public string resultCode { get; set; } // 결과 코드
        public string resultMsg { get; set; } // 결과 메시지
        public int numOfRows { get; set; } // 한 페이지 결과 수
        public int pageNo { get; set; } // 페이지 번호
        public int totalCount { get; set; } // 전체 결과 수
        public string country_eng_nm { get; set; } // 영문 국가명
        public string country_nm { get; set; } // 한글 국가명
        public string country_iso_alp2 { get; set; } // ISO 2자리코드
        public string continent_cd { get; set; } // 대륙 코드
        public string continent_eng_nm { get; set; } // 영문 대륙명
        public string continent_nm { get; set; } // 한글 대륙명
        public string dang_map_download_url { get; set; } // 위험 지도 경로
        public string flag_download_url { get; set; } // 국기 다운로드 경로
        public string map_download_url { get; set; } // 지도 다운로드 경로
        public int alarm_lvl { get; set; } // 경보단계
        public string region_ty { get; set; } // 지역유형
        public string remark {  get; set; }
        public DateTime written_dt { get; set; } // 작성일
        public int currentCount { get; set; } // 현재 결과 수

        public static readonly string INSERT_QUERY = @"INSERT INTO [dbo].[TravelAlarmService]
                                                                   ([resultCode]
                                                                   ,[resultMsg]
                                                                   ,[numOfRows]
                                                                   ,[pageNo]
                                                                   ,[totalCount]
                                                                   ,[country_eng_nm]
                                                                   ,[country_nm]
                                                                   ,[country_iso_alp2]
                                                                   ,[continent_cd]
                                                                   ,[continent_eng_nm]
                                                                   ,[continent_nm]
                                                                   ,[dang_map_download_url]
                                                                   ,[flag_download_url]
                                                                   ,[map_download_url]
                                                                   ,[alarm_lvl]
                                                                   ,[Region]
                                                                   ,[Remark]
                                                                   ,[written_dt]
                                                                   ,[currentCount])
                                                             VALUES
                                                                   (@Resultcode, 
                                                                   ,@ResultMsg, 
                                                                   ,@Numofrows, 
                                                                   ,@Pageno, 
                                                                   ,@Totalcount, 
                                                                   ,@Conen, 
                                                                   ,@Conkr, 
                                                                   ,@Con_ios, 
                                                                   ,@Con_cd, 
                                                                   ,@Con_ennm,
                                                                   ,@Con_nm, 
                                                                   ,@Dang_url, 
                                                                   ,@Flag_url, 
                                                                   ,@Map_url,
                                                                   ,@Alarm, 
                                                                   ,@Region,
                                                                   ,@Remark,
                                                                   ,@Written, 
                                                                   ,@Curcount)";

        public static readonly string SELECT_QUERY = @"SELECT [resultCode]
                                                                   ,[resultMsg]
                                                                   ,[numOfRows]
                                                                   ,[pageNo]
                                                                   ,[totalCount]
                                                                   ,[country_eng_nm]
                                                                   ,[country_nm]
                                                                   ,[country_iso_alp2]
                                                                   ,[continent_cd]
                                                                   ,[continent_eng_nm]
                                                                   ,[continent_nm]
                                                                   ,[dang_map_download_url]
                                                                   ,[flag_download_url]
                                                                   ,[map_download_url]
                                                                   ,[alarm_lvl]
                                                                   ,[Region]
                                                                   ,[Remark]
                                                                   ,[written_dt]
                                                                   ,[currentCount]
                                                          FROM [dbo].[TravelAlarmService]";

        //public static readonly string GETDATE_QUERY = @"SELECT CONVERT(CHAR(10), Written, 23) AS Save_Date
        //                                                  FROM [dbo].[TravelAlarmService]
        //                                                 GROUP BY CONVERT(CHAR(10), Written, 23)";
    }
}
