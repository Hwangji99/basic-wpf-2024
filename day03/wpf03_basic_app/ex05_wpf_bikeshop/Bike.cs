﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ex05_wpf_bikeshop
{
    // Notifier를 상속받으면 AutoProperty { get; set; }은 사용할 수 없음
    public class Bike : Notifier
    {
        private double speed; // 멤버변수
        private Color color;

        public double Speed
        {
            get { return speed; } // 속성
            set { 
                speed = value;
                // 속성값이 변경되는 것을 알려주려면 이 작업이 반드시 필요함
                OnPropertyChanged(nameof(Speed)); // "Speed"
            }
        }
        // System.Window.Media 컬러를 써야한다 (System Drawing은 아님)
        public Color Color
        {
            get { return color; }
            set { 
                color = value;
                OnPropertyChanged(nameof(Color));
            }
            
        }
    }
}
