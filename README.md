# WPF 윈폼 개발학습
IoT 개발자 WPF 학습 리포지토리

## 1일차(2024-04-29)
- WPF(Window Presentation ) 기본학습
    - Winforms 확장한 WPF
        - 이전의 Winforms는 이미지 비트맵방식(2D)
        - WPF 이미지 벡터방식
        - XAML 화면 디자인 - 안드로이드 개발 시 Java XML로 한 화면 디자인과 PyQt로 한 디자인과 동일

    - XAML(한국은 엑스에이엠엘, 미국은 재믈이라 부름)
        - 여는 태그 <Window>, 닫는 태그 </Window>
        - 합치면 <Window /> - Window 태그 안에 다른객체가 없단 뜻
        - 여는 태그와 닫는 태그 사이에 다른 태그(객체)를 넣어서 디자인

    - WPF 기본 사용법
        - Winforms와는 다르게 코딩으로 디자인을 함 

    - 레이아웃
        - 1. Grid - WPF에서 가장 많이 쓰는 대표적인 레이아웃(중요!)
        - 2. Canvas - 미술에서 캔버스와 유사
        - 3. StackPanel - 스택으로 컨트롤을 쌓는 레이아웃
        - 4. DockPanel - 컨트롤을 방향에 따라서 도킹시키는 레이아웃
        - 5. Margin - 여백기능, 앵커링 같이함 (중요!)

## 2일차(2024-04-30)
- WPF 기본학습
    - 데이터 바인딩 - 데이터 소스(DB, 엑셀, txt, 클라우드에 보관된 데이터원본)에 데이터를 쉽게 가져다쓰기 위해 데이터 핸들링 방법
        - xaml : {Binding Path = 속성, ElementName=객체, Mode=(One|TowWay), StringFormat={}{0:#,#}}
        - dataContext : 데이터를 담아서 전달하는 이름
        - 전통적인 윈폼 코드비하인드에서 데이터를 처리하는 것을 지양 - 디자인, 개발 부분 분리

## 3일차(2024-05-02)
- WPF에서 중요한 개념(윈폼과 차이점)
    - 1. 데이터바인딩 - 바인딩 키워드로 코드와 분리
    - 2. 옵저버 패턴 - 값이 변경된 사실을 사용자에게 공지 OnPropertyChanged 이벤트
    - 3. 디자인 리소스 - 각 컨트롤마다 디자인(X), 리소스로 디자인 공유
          - 각 화면당 Resources - 자기 화면에만 적용되는 디자인
          - App.xaml Resources - 애플리케이션 전체에 적용되는 디자인
          - 리소스사전 - 공유할 내용이 많을 때 파일로 따로 지정

- WPF MVVM
    - MVC(Model View Controller 패턴)
        - 웹개발(Spring, ASP.NET MVC, dJango, etc...) 현재도 사용되고 있음
        - Model : Data 입출력 처리를 담당
        - View : 디스플레이 화면 담당, 순수 xaml로만 구성
        - Controller : View를 제어, Model 처리 중앙에 관장

    - MVVM(Model View ViewModel)
        - Model : Data 입출력(DB side), 뷰에 제공할 데이터...
        - View : 화면, 순수 xaml로만 구성
        - ViewModel : 뷰에 대한 메서드, 액션, INotifyPropertyChanged를 구현

        ![MVVM패턴](https://github.com/Hwangji99/basic-wpf-2024/blob/main/images/wpf001.png)

    - 권장 구현방법
        - ViewModel 생성, 알림 속성 구현, 
        - View에 ViewModel을 데이터바인딩
        - Model DB 작업은 독립적으로 구현

    - MVVM 구현 도와주는 프레임워크
        - 0. ~~MVVMlight.Toolkit~~ - 3rd Party 개발. 2009년부터 시작 2014년도 이후 더이상 개발이나 지원이 없음
        - 1. **Caliburn.Micro** - 3rd Party 개발. MVVM이 아주 간단. 강력. 중소형 프로젝트에 적합. 디버깅이 조금 어려움
        - 2. Avalonia 
            - 3rd Party 개발
            - 크로스플랫폼
            - 디자인은 최고
        - 3. Prism - Microsoft 개발. 무지막지하게 어렵다.

- Caliburn.Micro
    - 1. 프로젝트 생성 후 MainWindow.xaml 삭제
    - 2. Models, Views, ViewModels 폴더(네임스페이스) 생성
    - 3. 종속성 NuGet 패키지 Caliburn.Micro 설치
    - 4. 루트 폴더에 Bootstrapper.cs 클래스 생성
    - 5. App.xaml에서 StartupUri 삭제
    - 6. App.xaml에 Bootstrapper 클래스를 리소스 사전에 등록
    - 7. App.xaml.cs에 App() 생성자 추가
    - 8. ViewModels 폴더에 MainViewModel.cs 클래스 생성
    - 9. Bootstrapper.cs에 OnStartup()에 내용을 변경
    - 10. Views 폴더에 MainView.xaml

    - 작업(3명) 분리
        - DB 개발자 - DBMS 테이블 생성, Models에 클래스 작업
        - Xaml 디자이너 - Views 폴더에 있는 xaml 파일을 디자인작업

## 4일차(2024-05-03)
- Caliburn.Micro
    - 작업 분리
        - Xaml디자이너 - xaml 파일만 디자인
        - ViewModel개발자 - Model에 있는 DB 관련 정보와 View와 연계 전체 구현 작업

    - Caliburn.Micro 특징
        - Xaml 디자인 시 {Binding ...} 잘 사용하지 않음
        - 대신 x:Name을 사용

    - MVVM 특징
        - 예외발생 시 예외메시지 표시없이 프로그램 종료
        - F5로 디버깅(디버깅해서 오류 찾기)
        - ViewModel에서 디버깅 시작
        - View.xaml에선 바인딩, 버튼클릭 이름(ViewModel 속성, 메서드) 지정 주의
        - Model내 속성 DB 테이블 컬럼 이름 일치, CRUD 쿼리문 오타 주의
        - ViewModel 부분
            - 변수, 속성으로 분리
            - 속성이 Model내의 속성과 이름이 일치 
            - List 사용불가 -> BindableCollection으로 변경
            - 메서드와 이름이 동일한 Can... 프로퍼티 지정, 버튼 활성/비활성화
            - 모든 속성에 NotifyOfPropertyChange() 메서드 존대(값 변경 알림)



        ![실행화면](https://github.com/Hwangji99/basic-wpf-2024/blob/main/images/wpf002.png)
    


## 5일차(2024-05-07)
- MahApps.Metro(https://mahapps.com/)
    - Metro(Modern UI) 디자인 접목

    ![실행화면](https://github.com/Hwangji99/basic-wpf-2024/blob/main/images/wpf003.png)

    ![저장화면](https://github.com/Hwangji99/basic-wpf-2024/blob/main/images/wpf004.png)

- Movie API 연동 앱, MovieFinder 2024
    - 좋아하는 영화 즐겨찾기 앱
    - SQLCerver를 이용함
    - DB(SQL Server) 연동
    - MahApps.Metro UI
    - CefSharp WebBrowser 패키지
    - Google.Apis 패키지
    - Newtonsoft. Json 패키지
    - MVVM은 사용 안함
    - OpenAPI 두가지 사용
    - [TMDB][https://www.themoviedb.org/]() OpenAPI 활용
        - 회원가입 후 API 신청
    - [Youtube API](https://console.cloud.google.com/) 활용
        - 새 프로젝트 생성
        - API 및 서비스, 라이브러리 선택
        - YouTube Data API v3 선택, 사용버튼 클릭
        - 사용자 인증정보 만들기 클릭
            1. 사용자 데이터 라디오버튼 클릭 -> 다음
            2. OAuth 동의화면, 기본내용 입력 후 다음
            3. 범위는 저장 후 계속
            4. OAuth Client ID, 앱 유형을 데스크톱 앱, 이름 입력 후 만들기 클릭

## 6일차(2024-05-08)
- MovieFinder 2024 계속
    - 즐겨찾기 후 다시 선택 즐겨찾기 막아야함
    - 즐겨찾기 삭제 구현
    - 즐겨찾기 일부만 저장기능 추가
    - 그리드뷰 영화를 더블클릭하면 영화소개 팝업

## 7일차(2024-05-09)
- MovieFinder 2024 완료
  

https://github.com/Hwangji99/basic-wpf-2024/assets/158007430/eb0e7001-5514-42ba-8113-de31b9867852


- 데이터포털 API로 연동앱 예제
    - CefSharp 사용 시 플랫폼 대상 AnyCPU에서 x64로 변경 필수
    - MahApps.Metro UI, IconPacks
    - Newtonsoft.Json
    - 개인 프로젝트 참조 소스

## 8일차(2024-05-13)
- WPF 개인 토이 프로젝트
    - 부산광역시 119에 구조된 반려동물 찾기 서비스
        - 활요한 Open API : 부산광역시_119 구조 반려동물 정보
        (https://www.data.go.kr/tcs/dss/selectApiDataDetailView.do?publicDataPk=15034086)

        - 성능
            1. 조회 클릭 시
                - 아무것도 입력이 되지않은 상태에서 조회 시 모든 자료가 뜸(구현)
                - 조회하고자 하는 종을 입력 후 조회 시 구조된 반려 동물들의 정보가 뜸(구현)

            2. 자료를 클릭 시 반려동물의 사진이 뜸(구현)

            3. 자료를 클릭 후 미인수건 버튼을 누르면 미인수건으로 따로 저장됨(구현)
                - 중복 추가 안됨

            4. 미인수건 보기 버튼 클릭 시 추가한 미인수건이 나옴
                - 삭제버튼을 누르면 해당 자료가 삭제됨

- 실행화면

https://github.com/Hwangji99/basic-wpf-2024/assets/158007430/7ef4195c-5372-4fa4-9d1f-7fb369b5fd69
