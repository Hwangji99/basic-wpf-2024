﻿using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_app.Models
{
    public class AnimalRescue
    {
        public string Sj { get; set; } // 구조정보
        public string Wrter { get; set; } // 작성자
        public DateTime WritngDe { get; set; } // 작성일
        public string Cn { get; set; } // 내용
        public string Ty3Date { get; set; } // 포획일시
        public string Ty3Place { get; set; } // 포획장소
        public string Ty3Kind { get; set; } // 동물종류
        public string Ty3Sex { get; set; } // 성별
        public string Ty3Process { get; set; } // 처리현황
        public string Ty3Ingye { get; set; } // 인계
        public string Ty3Insu {  get; set; } // 인수
        public string Ty3Picture { get; set; } // 동물사진

        public static readonly string INSERT_QUERY = @"INSERT INTO [dbo].[AnimalRescue]
                                                                       ([sj]
                                                                       ,[wrter]
                                                                       ,[writngDe]
                                                                       ,[cn]
                                                                       ,[ty3Date]
                                                                       ,[ty3Place]
                                                                       ,[ty3Kind]
                                                                       ,[ty3Sex]
                                                                       ,[ty3Process]
                                                                       ,[ty3Ingye]
                                                                       ,[ty3Insu]
                                                                       ,[ty3Picture])
                                                                 VALUES
                                                                       (@sj
                                                                       ,@wrter
                                                                       ,@writngDe
                                                                       ,@cn
                                                                       ,@ty3Date
                                                                       ,@ty3Place
                                                                       ,@ty3Kind
                                                                       ,@ty3Sex
                                                                       ,@ty3Process
                                                                       ,@ty3Ingye
                                                                       ,@ty3Insu
                                                                       ,@ty3Picture)";

        public static readonly string SELECT_QUERY = @"SELECT [sj]
                                                             ,[wrter]
                                                             ,[writngDe]
                                                             ,[cn]
                                                             ,[ty3Date]
                                                             ,[ty3Place]
                                                             ,[ty3Kind]
                                                             ,[ty3Sex]
                                                             ,[ty3Process]
                                                             ,[ty3Ingye]
                                                             ,[ty3Insu]
                                                             ,[ty3Picture]
                                                         FROM [dbo].[AnimalRescue]";

        public static readonly string CHECK_QUERY = @"SELECT COUNT(*) 
                                                        FROM AnimalRescue
                                                       WHERE cn = @cn";

        public static readonly string DELETE_QUERY = @"DELETE FROM [dbo].[AnimalRescue] WHERE cn = @cn";

        //public static readonly string GETDATE_QUERY = @"SELECT CONVERT(CHAR(10), Written, 23) AS Save_Date
        //                                                  FROM [dbo].[TravelAlarmService]
        //                                                 GROUP BY CONVERT(CHAR(10), Written, 23)";
    }
}
