# local-temperature-server

## 1. 기능
- 웹서비스 URL 파라미터 생성 및 호출 (특정 지역, 현재 시간)
- 결과 파싱해서 txt 파일로 저장 (기온 데이터)
- 클라이언트 프로그램 : local-temperature-client

## 2. 웹서비스 예시

* URL

    http://apis.data.go.kr/1360000/VilageFcstInfoService/getUltraSrtNcst?serviceKey=인증키&nx=60&ny=127&base_date=20210930&base_time=1500

* Result (XML)

    ```
    <response>
        <header>
            <resultCode>00</resultCode>
            <resultMsg>NORMAL_SERVICE</resultMsg>
        </header>
        <body>
            <dataType>XML</dataType>
            <items>
                <item>
                    <baseDate>20210930</baseDate>
                    <baseTime>1500</baseTime>
                    <category>PTY</category>
                    <nx>60</nx>
                    <ny>127</ny>
                    <obsrValue>0</obsrValue>
                </item>
                <item>
                    <baseDate>20210930</baseDate>
                    <baseTime>1500</baseTime>
                    <category>REH</category>
                    <nx>60</nx>
                    <ny>127</ny>
                    <obsrValue>63</obsrValue>
                </item>
                <item>
                    <baseDate>20210930</baseDate>
                    <baseTime>1500</baseTime>
                    <category>RN1</category>
                    <nx>60</nx>
                    <ny>127</ny>
                    <obsrValue>0</obsrValue>
                </item>
                <item>
                    <baseDate>20210930</baseDate>
                    <baseTime>1500</baseTime>
                    <category>T1H</category>
                    <nx>60</nx>
                    <ny>127</ny>
                    <obsrValue>25.8</obsrValue>
                </item>
                <item>
                    <baseDate>20210930</baseDate>
                    <baseTime>1500</baseTime>
                    <category>UUU</category>
                    <nx>60</nx>
                    <ny>127</ny>
                    <obsrValue>1.8</obsrValue>
                </item>
                <item>
                    <baseDate>20210930</baseDate>
                    <baseTime>1500</baseTime>
                    <category>VEC</category>
                    <nx>60</nx>
                    <ny>127</ny>
                    <obsrValue>275</obsrValue>
                </item>
                <item>
                    <baseDate>20210930</baseDate>
                    <baseTime>1500</baseTime>
                    <category>VVV</category>
                    <nx>60</nx>
                    <ny>127</ny>
                    <obsrValue>-0.1</obsrValue>
                </item>
                <item>
                    <baseDate>20210930</baseDate>
                    <baseTime>1500</baseTime>
                    <category>WSD</category>
                    <nx>60</nx>
                    <ny>127</ny>
                    <obsrValue>1.8</obsrValue>
                </item>
            </items>
            <numOfRows>10</numOfRows>
            <pageNo>1</pageNo>
            <totalCount>8</totalCount>
        </body>
    </response>
    ```

## 3. 화면 예시

![제목 없음-1](https://user-images.githubusercontent.com/14077108/135413910-20f6989f-5015-4a8b-b1e4-c11623546678.jpg)
