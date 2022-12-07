Imports System
Imports System.Threading
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Edge
Imports OpenQA.Selenium.Firefox
Imports OpenQA.Selenium.Support.Extensions
Imports OpenQA.Selenium.Support.UI
Imports OpenQA.Selenium.Interactions
Imports WebDriverManager
Imports WebDriverManager.DriverConfigs
Imports WebDriverManager.DriverConfigs.Impl
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net.NetworkInformation
Imports System.IO.File
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks.Task
Imports WinFormsApp1.MyLogging
Imports WebDriverManager.Helpers


Public Class Form1

    'Const Version = "1.0.221027.1"


    'Dim chromeDriver As IWebDriver
    'Dim webDriverWait As WebDriverWait

    Dim rnd_num As New Random()

    'Public css_selector_config_obj As Newtonsoft.Json.Linq.JObject
    'Public m_css_selector_config_obj As Newtonsoft.Json.Linq.JObject

    Public used_chrome_profile As String = ""
    Public running_chrome_profile As String = ""
    'Dim webDriverWait As WebDriverWait


    Public used_lang = "zh-TW"


    Public Profile_Queue() As String

    'Dim act = New Actions(chromeDriver)
    Public act As Actions


    Public myWebDriver As New MyWebDriver()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Me.Text = "Main Form - " + Version
        'Default String : 
        Profile_TextBox.Text = FormInit.curr_path + "profiles"
        Block_Text_TextBox.Text = "分隔行"
        curr_url_ComboBox.Text = "https://www.facebook.com/"

        FormInit.Property_Folder_Init()
        FormInit.Render_Script_listview()
        FormInit.Render_Maching_condition_listview()
        FormInit.Render_img_listbox()
        FormInit.Render_TextFolder_listbox()
        FormInit.Render_ImageFolder_listbox()
        FormInit.Render_TextFile_listbox()
        FormInit.Render_profile_CheckedListBox()
        FormInit.Render_Lang_Packs_ComboBox()
        FormInit.Render_DevList_combobox()
        FormInit.Render_Groups_Listview()
        FormInit.Render_ProfileName_ComboBox_Item()
        FormInit.Render_My_Script_ComboBox()
        FormInit.Render_Keyword_TextFIle()
        FormInit.Render_URL_TextFIle()
        FormInit.Render_Current_URL_ComboBox()

    End Sub


    Private Sub Pause_script_btn_Click(sender As Object, e As EventArgs) Handles stop_script_btn.Click
        'script_running = False
        'loop_run = False
        Pause_Script = True
    End Sub

    Private Sub Continute_Script_btn_Click(sender As Object, e As EventArgs) Handles Continute_Script_btn.Click
        Pause_Script = False
    End Sub

    Private Sub Reset_Script_btn_Click(sender As Object, e As EventArgs) Handles Reset_Script_btn.Click
        Flag_start_script = False
        loop_run = False
        start_time = "0"
        end_time = "0"
        script_running = False
        Pause_Script = False
        Continue_time = ""
        Restore_ListViewItems_BackColor()
        For Each item As ListViewItem In script_ListView.Items
            item.SubItems.Item(3).Text = ""
            item.SubItems.Item(6).Text = ""
        Next
    End Sub

    Dim profile_index = 0
    Private Async Sub Run_script_controller()
        Dim i = 1

        While True
            For Each item As ListViewItem In script_ListView.Items
                item.SubItems.Item(6).Text = ""
            Next
            Await Run_script(i)


            If Not loop_run Then
                Exit While
            End If
            i += 1

        End While
        start_time = 0
        end_time = 0

    End Sub


    '# Main routing struct #'
    Private Async Function Run_script(i As Integer) As Task
        'Debug.WriteLine(logging.Get_NewLogFile_dir())

        Dim record_script = False
        Dim log_file_path As String = ""
        If Record_script_result_checkbox.Checked = True Then
            log_file_path = MyLogging.Get_NewLogFile_dir()
            record_script = True
        End If
        'Dim rnd_num As New Random()

        For Each item As ListViewItem In script_ListView.Items
            Restore_ListViewItems_BackColor()
            item.BackColor = Color.SteelBlue
            item.ForeColor = Color.White
            item.EnsureVisible()


            If Pause_Script = True Then ' Pause script
                While True
                    Await Delay_msec(1000)
                    If Pause_Script = False Then
                        Exit While
                    End If
                End While
            End If

            If script_running = False Then
                'Debug.WriteLine("running false")
                loop_run = False
                Exit Function
            End If

            '3 : browser type
            '4 : device type
            '5 : profile
            '6 : url
            '7 : action ... click sendkey or navigate
            '8 : parameter and content
            '9 : result
            If item.SubItems.Item(4).Text = "" Then
                Continue For
            End If

            Dim brower = item.SubItems.Item(1).Text
            Dim devicetype = item.SubItems.Item(2).Text
            Dim profile = item.SubItems.Item(3).Text
            Dim action = item.SubItems.Item(4).Text
            Dim content = item.SubItems.Item(5).Text
            Dim execute_result = item.SubItems.Item(6)
            Dim boolean_result As Boolean
            item.SubItems.Item(3).Text = used_chrome_profile
            Select Case action
                Case "開始"
                    Continue_time = content
                    Pause_Script = True
                    boolean_result = True
                Case "開啟"
                    If profile = "" Or profile <> running_chrome_profile Then
                        boolean_result = Await myWebDriver.Open_Browser_Task(brower, devicetype, content)
                    Else
                        boolean_result = False
                    End If
                Case "開啟:佇列"
                    loop_run = True
                    If i = 1 Then
                        Profile_Queue = content.Split(";")
                    End If
                    'Debug.WriteLine("Profile= : " + Profile_Queue(profile_index))
                    used_chrome_profile = Profile_Queue(profile_index).Split("\")(UBound(Profile_Queue(profile_index).Split("\")))
                    item.SubItems.Item(3).Text = used_chrome_profile
                    boolean_result = Await myWebDriver.Open_Browser_Task(brower, devicetype, Profile_Queue(profile_index))

                    'Debug.WriteLine("ProfleQ: " & Profile_Queue.Count)

                    If Profile_Queue.Count - 1 = profile_index Then

                        If CheckBox_loop_run.Checked = False Then
                            profile_index = 0
                            loop_run = False
                        Else
                            profile_index = 0
                        End If
                    Else
                        profile_index += 1
                    End If


                Case "關閉"
                    boolean_result = Quit_chromedriver()
                Case "登入"
                    Dim auth() As String = content.Split(";")
                    Dim account_passwd = content.Split(" ")
                    boolean_result = myWebDriver.Login_fb(account_passwd(0).Split(":")(1), account_passwd(1).Split(":")(1))
                    Await Delay_msec(1000)
                Case "前往"
                    If content.Contains(";"c) Then
                        content = content.Split(";")(1)
                    End If
                    boolean_result = Await myWebDriver.Navigate_GoToUrl_Task(content)

                Case "前往:隨機"

                    If content = "全部隨機" Then
                        Dim allURLTextFile = Navigation_URL_CheckedListBox.Items
                        Dim rnd = rnd_num.Next(0, allURLTextFile.Count)
                        boolean_result = Await myWebDriver.Navigate_GoToUrl_Task(File.ReadAllText(FormInit.URL_Navigation_path + allURLTextFile(rnd)))
                    Else
                        Dim URLTextFiles = content.Split(";")
                        Dim rnd = rnd_num.Next(0, URLTextFiles.Length)
                        'content_RichTextBox.Text = File.ReadAllText(TextFiles(rnd))
                        boolean_result = Await myWebDriver.Navigate_GoToUrl_Task(File.ReadAllText(FormInit.URL_Navigation_path + URLTextFiles(rnd)))
                    End If

                Case "等待"
                    Try

                        Dim sec = Get_random_sec_frome_content(content)
                        Dim counter = sec
                        For i = 1 To sec
                            item.SubItems.Item(6).Text = (counter.ToString())
                            Await Delay_msec(1000)
                            counter -= 1
                        Next
                        'Await Delay_msec(sec * 1000)
                        boolean_result = True
                    Catch ex As Exception
                        Debug.WriteLine(ex)
                        boolean_result = False
                    End Try
                Case "點擊"
                    boolean_result = Await myWebDriver.Click_element_by_feature_Task(content)
                Case "發送"
                    boolean_result = Await myWebDriver.Write_post_send_content_Task(content)
                Case "發送:隨機"
                    If content = "全部隨機" Then
                        Dim allTextFile = Text_File_CheckedListBox.Items
                        Dim rnd = rnd_num.Next(0, allTextFile.Count)
                        'Debug.WriteLine("TEXT : " + allTextFile(rnd))
                        boolean_result = Await myWebDriver.Write_post_send_content_Task(File.ReadAllText(text_folder_path + allTextFile(rnd)))
                    Else
                        Dim TextFiles = content.Split(";")
                        Dim rnd = rnd_num.Next(0, TextFiles.Length)
                        'content_RichTextBox.Text = File.ReadAllText(TextFiles(rnd))
                        boolean_result = Await myWebDriver.Write_post_send_content_Task(File.ReadAllText(text_folder_path + TextFiles(rnd)))
                    End If
                Case "發送上載:隨機配對"
                    boolean_result = Await Post_Random_Match_TextAndImage(content)
                Case "發送上載:隨機配對多圖"
                    boolean_result = Await Post_Random_Match_TextAndImageFolder(content)
                Case "清空"
                    boolean_result = Await myWebDriver.Clear_post_content_Task()
                Case "上載"
                    boolean_result = Await myWebDriver.Tring_to_upload_img_Task(content)
                Case "上載:隨機"
                    If content = "全部隨機" Then
                        Dim allImageFile = img_CheckedListBox.Items
                        Dim rnd = rnd_num.Next(0, allImageFile.Count)
                        boolean_result = Await myWebDriver.Tring_to_upload_img_Task(image_folder_path + allImageFile(rnd))
                    Else
                        Dim ImageFiles = content.Split(";")
                        Dim rnd = rnd_num.Next(0, ImageFiles.Length)
                        boolean_result = Await myWebDriver.Tring_to_upload_img_Task(image_folder_path + ImageFiles(rnd))
                    End If

                Case "回應:上載"
                    boolean_result = Await myWebDriver.Upload_reply_img_Task(image_folder_path + content)
                Case "回應:上載隨機"
                    If content = "全部隨機" Then
                        Dim allImageFile = img_CheckedListBox.Items
                        Dim rnd = rnd_num.Next(0, allImageFile.Count)
                        boolean_result = Await myWebDriver.Upload_reply_img_Task(image_folder_path + allImageFile(rnd))
                    Else
                        Dim ImageFiles = content.Split(";")
                        Dim rnd = rnd_num.Next(0, ImageFiles.Length)
                        boolean_result = Await myWebDriver.Upload_reply_img_Task(image_folder_path + ImageFiles(rnd))
                    End If
                Case "回應:內容"
                    boolean_result = Await myWebDriver.Send_reply_comment_Task(content)
                Case "回應:隨機"
                    If content = "全部隨機" Then
                        Dim allTextFile = Text_File_CheckedListBox.Items
                        Dim rnd = rnd_num.Next(0, allTextFile.Count)
                        'Debug.WriteLine("TEXT : " + allTextFile(rnd))
                        boolean_result = Await myWebDriver.Send_reply_comment_Task(File.ReadAllText(curr_path + "resources\texts\" + allTextFile(rnd)))
                    Else
                        Dim TextFiles = content.Split(";")
                        Dim rnd = rnd_num.Next(0, TextFiles.Length)
                        'content_RichTextBox.Text = File.ReadAllText(TextFiles(rnd))
                        boolean_result = Await myWebDriver.Send_reply_comment_Task(File.ReadAllText(curr_path + "resources\texts\" + TextFiles(rnd)))
                    End If
                Case "回應:隨機配對"
                    boolean_result = Await myWebDriver.Reply_Random_Match_TextAndImage(content)

                Case "回應:送出"

                    If myWebDriver.IsElementPresentByClass("uiScaledImageContainer") Then
                        boolean_result = Await myWebDriver.Submit_reply_comment_Task()
                    Else
                        boolean_result = Await myWebDriver.Submit_reply_comment_Task()
                    End If

                Case "回應:按讚"
                    boolean_result = Await myWebDriver.Click_reply_random_emoji_Task(content)
                Case "捲動頁面"
                    Dim Offset As String() = content.Split(";")
                    Dim y_offset = CInt(Offset(1).Split(":")(0))
                    Dim y_single_offset = CInt(Offset(1).Split(":")(1))

                    If y_single_offset > 0 Then
                        For scroll_offset As Integer = y_single_offset To y_offset Step y_single_offset
                            item.SubItems.Item(6).Text = CStr(scroll_offset)
                            boolean_result = ScrollPage_By_Offset(Offset(0), CStr(scroll_offset))
                            Await Delay(1000)
                        Next
                    Else
                        boolean_result = ScrollPage_By_Offset(Offset(0), CStr(y_offset))
                    End If
                Case "發送:按鍵"
                    boolean_result = SendBrowserKeyAction(content)
                Case "點擊:座標"
                    Dim offset() = content.Split(";")
                    boolean_result = myWebDriver.Click_by_Cursor_Offset(offset(0), offset(1))
                Case "聊天"
                    Dim Target = content.Split(";")(0)
                    Dim Text = content.Split(";")(1)
                    boolean_result = myWebDriver.Messager_Contact(Target, Text)

                Case "搜尋"
                    Dim param() = content.Split(";")
                    boolean_result = Await myWebDriver.Search_Keyword_Task(param(0), param(1))
                Case "搜尋:隨機"

                    Dim param() = content.Split("%20")

                    If param(1) = "全部隨機" Then
                        Dim allKeywordTextFile = Searching_Keyword_CheckedListBox.Items
                        Dim rnd = rnd_num.Next(0, allKeywordTextFile.Count)
                        boolean_result = Await myWebDriver.Search_Keyword_Task(param(0), File.ReadAllText(FormInit.keyword_Searching_path + allKeywordTextFile(rnd)))
                    Else
                        Dim KeywordTextFiles = param(1).Split(";")
                        Dim rnd = rnd_num.Next(0, KeywordTextFiles.Length)
                        'content_RichTextBox.Text = File.ReadAllText(TextFiles(rnd))
                        boolean_result = Await myWebDriver.Search_Keyword_Task(param(0), File.ReadAllText(FormInit.keyword_Searching_path + KeywordTextFiles(rnd)))
                    End If

                Case "關閉程式"
                    Try
                        Dim sec = Get_random_sec_frome_content(content)
                        Dim counter = sec
                        For i = 1 To sec
                            item.SubItems.Item(6).Text = (counter.ToString())
                            Await Delay_msec(1000)
                            counter -= 1
                        Next
                        'Await Delay_msec(sec * 1000)
                        boolean_result = True
                        Close()
                    Catch ex As Exception
                        Debug.WriteLine(ex)
                        boolean_result = False
                    End Try
                Case "關閉電腦"
                    Try
                        Dim sec = Get_random_sec_frome_content(content)
                        Dim counter = sec
                        For i = 1 To sec
                            item.SubItems.Item(6).Text = (counter.ToString())
                            Await Delay_msec(1000)
                            counter -= 1
                        Next
                        'Await Delay_msec(sec * 1000)
                        boolean_result = True
                        'shutdown system
                        Process.Start("shutdown", "-s -t 0")
                        Close()
                    Catch ex As Exception
                        Debug.WriteLine(ex)
                        boolean_result = False
                    End Try

            End Select
            If boolean_result = True Then ' record the result

                item.SubItems.Item(6).Text = ("成功")
            ElseIf boolean_result = False Then
                item.SubItems.Item(6).Text = ("失敗")
            End If
            If record_script Then
                MyLogging.Write_to_file(log_file_path, brower + "," + devicetype + "," + action + "," + content + "," + item.SubItems.Item(6).Text)
            End If

        Next
        Await Delay_msec(1000)
    End Function

    Public Shared Async Function Delay_msec(msec As Integer) As Task
        Await Task.Delay(msec)
    End Function

    Private Async Sub Open_browser_Button_Click(sender As Object, e As EventArgs) Handles open_browser_Button.Click


        If chrome_RadioButton.Checked = True Then

            If EmulatedDevice_ComboBox.SelectedItem IsNot Nothing Then
                myWebDriver.used_dev_model = EmulatedDevice_ComboBox.SelectedItem.ToString
            Else
                myWebDriver.used_dev_model = "PC"
            End If

            Dim myprofile = ""
            If Profile_TextBox.Text <> "" And Profile_Name_ComboBox.Text <> "" Then
                myprofile = Profile_TextBox.Text + "\" + Profile_Name_ComboBox.Text
            Else
                For Each itemSeleted In Profile_CheckedListBox.SelectedItems
                    'Debug.WriteLine(itemSeleted)
                    myprofile = itemSeleted
                Next
            End If



            'Open_Browser("Chrome", used_dev_model, myprofile)
            Await myWebDriver.Open_Browser_Task("Chrome", myWebDriver.used_dev_model, myprofile)

        ElseIf firefox_RadioButton.Checked = True Then
            Open_Firefox()
        ElseIf edge_RadioButton.Checked = True Then
            Open_Edge()
        End If



        If curr_url_ComboBox.Text <> "" Then
            Dim pattern As String
            pattern = "http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?"
            If Regex.IsMatch(curr_url_ComboBox.Text, pattern) Then
                Await myWebDriver.Navigate_GoToUrl_Task(curr_url_ComboBox.Text)
            Else
                MsgBox("網址格式錯誤")
            End If

        End If

    End Sub


    Private Async Sub Open_Random_Profile_btn_Click(sender As Object, e As EventArgs) Handles Open_Random_Profile_btn.Click
        If chrome_RadioButton.Checked = True Then

            If EmulatedDevice_ComboBox.SelectedItem IsNot Nothing Then
                myWebDriver.used_dev_model = EmulatedDevice_ComboBox.SelectedItem.ToString
            Else
                myWebDriver.used_dev_model = "PC"
            End If

            Dim myprofile = ""
            If Profile_TextBox.Text <> "" And Profile_Name_ComboBox.Text <> "" Then
                myprofile = Profile_TextBox.Text + "\" + Profile_Name_ComboBox.Text
            Else
                For Each itemSeleted In Profile_CheckedListBox.SelectedItems
                    'Debug.WriteLine(itemSeleted)
                    myprofile = itemSeleted
                Next
            End If

            Await myWebDriver.Open_Browser_Task("Chrome", myWebDriver.used_dev_model, "全部隨機")

            If curr_url_ComboBox.Text <> "" Then
                Dim pattern As String
                pattern = "http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?"
                If Regex.IsMatch(curr_url_ComboBox.Text, pattern) Then
                    Navigate_GoToUrl(curr_url_ComboBox.Text)
                Else
                    MsgBox("網址格式錯誤")
                End If

            End If

        ElseIf firefox_RadioButton.Checked = True Then
            Open_Firefox()
        ElseIf edge_RadioButton.Checked = True Then
            Open_Edge()
        End If

    End Sub

    Public Sub Open_Firefox()
        myWebDriver.used_browser = "Firefox"
        Dim driverManager = New DriverManager()
        driverManager.SetUpDriver(New FirefoxConfig())
        Dim firefoxDriver = New FirefoxDriver()
    End Sub

    Public Sub Open_Edge()
        myWebDriver.used_browser = "Edge"
        Dim driverManager = New DriverManager()
        driverManager.SetUpDriver(New EdgeConfig())
        Dim edgeDrvier = New EdgeDriver()
    End Sub

    Public Function Navigate_GoToUrl(url As String)
        Try
            myWebDriver.Navigate_GoToUrl(url)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Sub get_groupname_Button_Click(sender As Object, e As EventArgs) Handles get_groupname_Button.Click

        Dim group_name = myWebDriver.Get_current_group_name()
        If group_name <> "" Then
            group_name_TextBox.Text = group_name
        Else
            MsgBox("無法取得群組名稱")
        End If

    End Sub



    Private Sub Driver_close_Click(sender As Object, e As EventArgs) Handles driver_close_bnt.Click
        'driver_close_bnt.Enabled = False
        used_chrome_profile = ""
        running_chrome_profile = ""

        If Quit_chromedriver() = False Then
            MsgBox("未偵測到Chrome")
        End If

    End Sub

    Public Function Quit_chromedriver()
        Try

            used_chrome_profile = ""
            running_chrome_profile = ""
            myWebDriver.Quit_ChromeDriver()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Dim current_checked As Integer
    Private Sub reply_img_CheckedListBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        img_CheckedListBox.SetItemChecked(current_checked, False)
        For i = 0 To img_CheckedListBox.Items.Count - 1
            'We ask if this item is checked or not
            If img_CheckedListBox.GetItemChecked(i) Then
                current_checked = i
            End If
        Next

    End Sub


    Private Sub Click_by_location_test_btn_Click(sender As Object, e As EventArgs) Handles Click_by_location_test_btn.Click
        Dim cursor_x = CursorX_TextBox.Text
        Dim cursor_y = CursorY_TextBox.Text
        myWebDriver.Click_by_Cursor_Offset(cursor_x, cursor_y)
    End Sub

    Public Sub EventlogListview_AddNewItem(content)
        Dim curr_row = script_ListView.Items.Count
        script_ListView.Items.Insert(curr_row, curr_row + 1.ToString)

        Dim splittedLine() As String = content.Split(",")
        For Each log In splittedLine
            script_ListView.Items(curr_row).SubItems.Add(log)
        Next
    End Sub


    '####################### Function script for selenium executing ##############################################################



    '############# Main Routing ###################################################

    Dim Flag_start_script = False
    Dim loop_run = False
    Dim start_time As String = "0"
    Dim end_time As String = "0"
    Dim script_running = False
    Dim Pause_Script = False
    Dim Continue_time = ""


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim TimeNow = Date.Now.ToString("HH:mm:ss")
        'Debug.WriteLine("current : " + Date.Now.ToString("HH:mm:ss"))
        'Debug.WriteLine("start : " + start_time)
        'Debug.WriteLine("End : " + end_time)

        If end_time <> "0" Then
            Dim TimeEnd_Arr = end_time.Split(":")
            Dim TimeNow_Arr = TimeNow.Split(":")
            Dim hours_flag = CInt(TimeNow_Arr(0)) - CInt(TimeEnd_Arr(0))
            Dim minutes_flag = CInt(TimeNow_Arr(1)) - CInt(TimeEnd_Arr(1))
            Dim secs_flag = CInt(TimeNow_Arr(2)) - CInt(TimeEnd_Arr(2))

            'Debug.WriteLine(hours_flag & "," & minutes_flag & "," & secs_flag)
            If hours_flag >= 0 And minutes_flag >= 0 And secs_flag >= 0 Then
                script_running = False
            End If

        End If


        If Continue_time = TimeNow Then
            Pause_Script = False
        End If

        If start_time = TimeNow Then
            Flag_start_script = True
        End If

        'If end_time = TimeNow Then
        'script_running = False
        'End If

        If Flag_start_script Then
            script_running = True
            Flag_start_script = False
            Run_script_controller()
        End If

    End Sub

    Private Sub Run_script_btn_Click(sender As Object, e As EventArgs) Handles Run_script_btn.Click





        loop_run = CheckBox_loop_run.Checked
        'Debug.WriteLine("Loop Run : " & loop_run)

        If CheckBox_script_start.Checked = True Then
            Flag_start_script = False
            start_time = NumericUpDown_script_start_hour.Value.ToString.PadLeft(2, "0") + ":" + NumericUpDown_script_start_minute.Value.ToString.PadLeft(2, "0") + ":" + NumericUpDown_script_start_second.Value.ToString.PadLeft(2, "0")
            'Debug.WriteLine("start time : " + start_time)
        Else
            Flag_start_script = True
        End If

        If CheckBox_script_end.Checked = True Then
            end_time = NumericUpDown_script_end_hour.Value.ToString.PadLeft(2, "0") + ":" + NumericUpDown_script_end_minute.Value.ToString.PadLeft(2, "0") + ":" + NumericUpDown_script_end_second.Value.ToString.PadLeft(2, "0")
            'Debug.WriteLine("end time : " + end_time)
        End If

    End Sub

    Private Shared Function Get_random_sec_frome_content(content)
        Dim base_sec = content.Split("±")(0).Replace("秒", "")
        Dim rnd_sec = "0"
        If content.Contains("±"c) Then
            Debug.WriteLine("gen random")
            rnd_sec = content.Split("±")(1).Replace("秒", "")
        End If
        Dim rnd_num As New Random()
        Dim random_sec = rnd_num.Next(-CInt(rnd_sec), CInt(rnd_sec))
        Dim total_sec = CInt(base_sec) + random_sec
        If total_sec < 0 Then
            total_sec = 1
        End If
        Return total_sec
    End Function

    Private Function SendBrowserKeyAction(pkey As String)
        Dim key_list() As String = pkey.Split("+")

        Dim firstKey = Keys.Enter

        Select Case key_list(0)
            Case "ENTER"
                firstKey = Keys.Enter
            Case "ESC"
                firstKey = Keys.Escape
            Case "CTRL"
                firstKey = Keys.LeftControl
            Case "ALT"
                firstKey = Keys.LeftAlt

        End Select

        Try
            If key_list.Length > 1 Then

                act.KeyDown(firstKey).SendKeys(key_list(1)).KeyUp(firstKey).Perform()
            Else
                act.SendKeys(firstKey).Perform()
            End If

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Sub Restore_ListViewItems_BackColor()
        For Each item As ListViewItem In script_ListView.Items
            item.BackColor = Color.White
            item.ForeColor = Color.Black
        Next

    End Sub

    Private Async Function Post_Random_Match_TextAndImage(content) As Task(Of Boolean)
        'Debug.WriteLine(content)
        Dim AllConditions() As String = content.Split(";")

        Dim rnd = rnd_num.Next(0, AllConditions.Length)

        Dim TextFolder = AllConditions(rnd).Split("%20")(0)
        Dim ImageFolder = AllConditions(rnd).Split("%20")(1)

        Dim Txtfiles() As String = IO.Directory.GetFiles(TextFolder)
        Dim TextFile_ArrayList = New ArrayList()
        For Each file As String In Txtfiles
            'Debug.WriteLine(file)
            If Path.GetExtension(file) = ".txt" Then
                TextFile_ArrayList.Add(file)
            End If

        Next

        rnd = rnd_num.Next(0, TextFile_ArrayList.Count)

        If Await myWebDriver.Write_post_send_content_Task(File.ReadAllText(TextFile_ArrayList(rnd))) = False Then
            Return False
        End If


        Dim Imgfiles() As String = IO.Directory.GetFiles(ImageFolder)
        Dim ImgageFile_ArrayList = New ArrayList()
        For Each file As String In Imgfiles
            'Debug.WriteLine(file)
            ImgageFile_ArrayList.Add(file)
        Next

        rnd = rnd_num.Next(0, ImgageFile_ArrayList.Count)
        'Debug.WriteLine("Image : " + ImgageFile_ArrayList(rnd))

        If Await myWebDriver.Tring_to_upload_img_Task(ImgageFile_ArrayList(rnd)) = False Then
            Return False
        End If

        Return True

    End Function

    Private Async Function Post_Random_Match_TextAndImageFolder(content) As Task(Of Boolean)
        Dim AllConditions() As String = content.Split(";")

        Dim rnd = rnd_num.Next(0, AllConditions.Length)

        Dim TextFolder = AllConditions(rnd).Split("%20")(0)
        Dim ImageFolder = AllConditions(rnd).Split("%20")(1)

        Dim Txtfiles() As String = IO.Directory.GetFiles(TextFolder)
        Dim TextFile_ArrayList = New ArrayList()
        For Each file As String In Txtfiles
            'Debug.WriteLine(file)
            If Path.GetExtension(file) = ".txt" Then
                TextFile_ArrayList.Add(file)
            End If

        Next

        rnd = rnd_num.Next(0, TextFile_ArrayList.Count)

        If myWebDriver.Write_post_send_content(File.ReadAllText(TextFile_ArrayList(rnd))) = False Then
            Return False
        End If

        Dim Imgfiles() As String = IO.Directory.GetFiles(ImageFolder)
        Dim img_path_str As String = ""
        For Each file As String In Imgfiles
            If img_path_str = "" Then
                img_path_str = file
            Else
                img_path_str = img_path_str & vbLf & file
            End If
        Next

        If Await myWebDriver.Tring_to_upload_img_Task(img_path_str) = False Then
            Return False
        End If

        Return True
    End Function

    Private Sub Get_url_btn_Click(sender As Object, e As EventArgs) Handles Get_url_btn.Click
        Try
            curr_url_ComboBox.Text = myWebDriver.chromeDriver.Url
        Catch ex As Exception
            MsgBox("未偵測到Chrome")
        End Try
    End Sub

    Private Function ScrollPage_By_Offset(x As String, y As String)
        Try
            myWebDriver.chromeDriver.ExecuteJavaScript("window.scrollTo(" + x + ", " + y + ");")
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Private Sub Insert_Login_Button_Click(sender As Object, e As EventArgs) Handles Insert_login_Button.Click
        Insert_Login()
    End Sub

    Private Sub Insert_navigate_to_url_btn_Click(sender As Object, e As EventArgs) Handles Insert_navigate_to_url_btn.Click
        Insert_navigate_to_url()
    End Sub

    Private Sub Insert_delay_btn_Click(sender As Object, e As EventArgs) Handles Insert_delay_btn.Click
        Insert_delay()
    End Sub

    Private Sub Clear_script_btn_Click(sender As Object, e As EventArgs) Handles Clear_script_btn.Click
        myWebDriver.used_browser = ""
        myWebDriver.used_dev_model = "PC"
        myWebDriver.used_chrome_profile = ""
        myWebDriver.running_chrome_profile = ""
        script_ListView.Items.Clear()
    End Sub

    Private Sub Insert_open_browser_btn_Click(sender As Object, e As EventArgs) Handles Insert_open_browser_btn.Click
        Insert_open_browser()
    End Sub

    Private Sub Insert_click_leave_msg_btn_Click(sender As Object, e As EventArgs) Handles Insert_click_leave_msg_btn.Click
        Insert_to_script("點擊", "留個言吧")
    End Sub

    Private Sub Insert_send_content_btn_Click(sender As Object, e As EventArgs) Handles Insert_send_content_btn.Click
        Insert_to_script("發送", content_RichTextBox.Text)
    End Sub

    Private Sub Insert_clear_content_btn_Click(sender As Object, e As EventArgs) Handles Insert_clear_content_btn.Click
        Insert_to_script("清空", "內容")
    End Sub

    Private Sub Insert_click_img_video_btn_Click(sender As Object, e As EventArgs) Handles Insert_click_img_video_btn.Click
        Insert_click_img_video()
    End Sub

    Private Sub Insert_Upload_Random_Image_btn_Click(sender As Object, e As EventArgs) Handles Insert_Upload_Random_Image_btn.Click
        Insert_Upload_Random_Image()
    End Sub

    Private Sub Insert_submit_post_btn_Click(sender As Object, e As EventArgs) Handles Insert_submit_post_btn.Click
        Insert_to_script("點擊", "發佈")
    End Sub

    Private Sub Insert_close_driver_btn_Click(sender As Object, e As EventArgs) Handles Insert_close_driver_btn.Click
        Insert_to_script("關閉", "瀏覽器")
    End Sub

    Private Sub Insert_click_reply_btn_Click(sender As Object, e As EventArgs) Handles Insert_click_reply_btn.Click
        Insert_to_script("點擊", "回覆/留言")
    End Sub

    Private Sub Insert_reply_comment_btn_Click(sender As Object, e As EventArgs) Handles Insert_reply_comment_btn.Click
        Insert_to_script("回應:內容", content_RichTextBox.Text)
    End Sub

    Private Sub Insert_comment_upload_img_btn_Click(sender As Object, e As EventArgs) Handles Insert_comment_upload_img_btn.Click
        Insert_comment_upload_img()
    End Sub

    Private Sub Insert_submit_comment_btn_Click(sender As Object, e As EventArgs) Handles Insert_submit_comment_btn.Click
        Insert_to_script("回應:送出", "送出")
    End Sub

    Private Sub Insert_emoji_btn_Click(sender As Object, e As EventArgs) Handles Insert_emoji_btn.Click
        Insert_emoji()
    End Sub

    Private Sub Insert_empty_btn_Click(sender As Object, e As EventArgs) Handles Insert_empty_btn.Click
        Insert_to_script("", Block_Text_TextBox.Text)
    End Sub

    Private Sub Open_file_dialog_btn_Click(sender As Object, e As EventArgs) Handles Open_file_dialog_btn.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String

        fd.Title = "Open File Dialog"
        'fd.InitialDirectory = "C:\"
        fd.Filter = "txt files (*.txt)|"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
            Debug.WriteLine(strFileName)
            TextBox_script_file_path.Text = strFileName
        End If
    End Sub

    Private Sub load_script_btn_Click(sender As Object, e As EventArgs) Handles load_script_btn.Click
        Try
            script_ListView.Items.Clear()
            Dim log_lines = IO.File.ReadAllLines(TextBox_script_file_path.Text)
            Dim curr_row As Integer = 0
            Dim index = 1
            For Each line In log_lines
                Dim splittedLine() As String = line.Split(",")
                script_ListView.Items.Add(index.ToString)
                For Each cmd In splittedLine
                    script_ListView.Items(curr_row).SubItems.Add(cmd)
                Next
                curr_row += 1
                index += 1
            Next

        Catch ex As Exception
            MsgBox("載入失敗，檔案無效或不存在")
        End Try

    End Sub

    Private Sub SaveAs_script_btn_Click(sender As Object, e As EventArgs) Handles SaveAs_script_btn.Click
        FormComponentController.Save_Script_Content()

    End Sub

    Private Sub script_ListView_Click(sender As Object, e As EventArgs) Handles script_ListView.Click
        Dim browser = script_ListView.Items(script_ListView.FocusedItem.Index).SubItems(1).Text
        Dim devtype = script_ListView.Items(script_ListView.FocusedItem.Index).SubItems(2).Text
        Dim action = script_ListView.Items(script_ListView.FocusedItem.Index).SubItems(4).Text
        Dim content = script_ListView.Items(script_ListView.FocusedItem.Index).SubItems(5).Text

        fb_account_TextBox.Text = ""
        fb_password_TextBox.Text = ""
        group_name_TextBox.Text = ""
        curr_url_ComboBox.Text = ""


        Select Case browser
            Case "Chrome"
                chrome_RadioButton.Checked = True
            Case "Firefox"
                firefox_RadioButton.Checked = True
            Case "Edge"
                edge_RadioButton.Checked = True
        End Select


        Select Case action
            Case "前往"
                If content.Contains(";"c) Then
                    group_name_TextBox.Text = content.Split(";")(0)
                    content = content.Split(";")(1)
                Else
                    group_name_TextBox.Text = ""
                End If
                curr_url_ComboBox.Text = content

                'Case "開啟"
                'profile_path_TextBox.Text = content
            Case "登入"
                Dim account_passwd = content.Split(" ")
                fb_account_TextBox.Text = account_passwd(0).Split(":")(1)
                fb_password_TextBox.Text = account_passwd(1).Split(":")(1)
            Case "等待"
                Dim total_sec As Integer = 0
                If content.Contains("±"c) Then 'if have random second
                    total_sec = CInt(content.Split("±")(0))
                    wait_random_second_NumericUpDown.Value = CInt(content.Split("±")(1).Replace("秒", ""))

                Else
                    total_sec = CInt(content.Split("±")(0).Replace("秒", ""))
                End If

                wait_hour_NumericUpDown.Value = Int(total_sec / 3600)
                total_sec -= wait_hour_NumericUpDown.Value * 3600
                wait_minute_NumericUpDown.Value = Int(total_sec / 60)
                total_sec -= wait_minute_NumericUpDown.Value * 60
                wait_second_NumericUpDown.Value = total_sec

        End Select

    End Sub

    Private Sub Delete_selected_item_btn_Click(sender As Object, e As EventArgs) Handles Delete_selected_item_btn.Click
        FormComponentController.Delete_ScriptListView_selected_item()
    End Sub

    Private Sub Move_up_selected_item_btn_Click(sender As Object, e As EventArgs) Handles Move_up_selected_item_btn.Click
        FormComponentController.Move_up_ScriptListView_selected_item()
    End Sub

    Private Sub MoveDown_selected_item_btn_Click(sender As Object, e As EventArgs) Handles MoveDown_selected_item_btn.Click
        FormComponentController.MoveDown_ScriptListView_selected_item()
    End Sub

    Private Sub Text_File_CheckedListBox_Click(sender As Object, e As EventArgs) Handles Text_File_CheckedListBox.ItemCheck
        FormComponentController.Text_File_CheckedListBox_Click()

    End Sub

    Private Sub img_CheckedListBox_Click(sender As Object, e As EventArgs) Handles img_CheckedListBox.ItemCheck
        FormComponentController.Img_CheckedListBox_Click()
    End Sub

    Private Sub Insert_send_Random_content_TextFile_btn_Click(sender As Object, e As EventArgs) Handles Insert_send_Random_content_TextFile_btn.Click
        Insert_send_Random_content_TextFile()
    End Sub

    Private Sub Set_Matching_btn_Click(sender As Object, e As EventArgs) Handles Set_Matching_btn.Click

        If TextFolder_ListBox.SelectedIndex >= 0 And ImageFolder_ListBox.SelectedIndex >= 0 Then
            Match_Condition_ListView.Items.Add(TextFolder_ListBox.SelectedItem.ToString()).SubItems.Add(ImageFolder_ListBox.SelectedItem.ToString())
        Else
            MsgBox("必須選取資料夾")
        End If

    End Sub

    Private Sub Insert_random_matching_text_and_img_btn_Click(sender As Object, e As EventArgs) Handles Insert_random_matching_text_and_img_btn.Click
        Insert_random_matching_text_and_img()
    End Sub

    Private Sub Clear_Conditions_Listview_Click(sender As Object, e As EventArgs) Handles Clear_Conditions_Listview.Click
        Match_Condition_ListView.Items.Clear()
    End Sub

    Private Sub Insert_Reply_Random_TxtFile_btn_Click(sender As Object, e As EventArgs) Handles Insert_Reply_Random_TxtFile_btn.Click
        Insert_Reply_Random_TxtFile()
    End Sub

    Private Sub Insert_Reply_Random_Match_btn_Click(sender As Object, e As EventArgs) Handles Insert_Reply_Random_Match_btn.Click
        Insert_Reply_Random_Match()
    End Sub

    Private Sub Insert_Reply_Random_Image_btn_Click(sender As Object, e As EventArgs) Handles Insert_Reply_Random_Image_btn.Click
        Insert_Reply_Random_Image()
    End Sub

    Private Sub SaveAs_RichBox_Content_btn_Click(sender As Object, e As EventArgs) Handles SaveAs_RichBox_Content_btn.Click
        FormComponentController.Save_RichBox_Content()
    End Sub

    Private Sub Open_dir_in_explorer_btn_Click(sender As Object, e As EventArgs) Handles Open_dir_in_explorer_btn.Click
        FormComponentController.Reveal_Selected_In_File_Explorer()
    End Sub

    Private Sub Save_Account_Passwd_btn_Click(sender As Object, e As EventArgs) Handles Save_Account_Passwd_btn.Click
        FormComponentController.Save_Account_Info()
    End Sub

    Private Sub Selected_PictureBox_Click(sender As Object, e As EventArgs) Handles Selected_PictureBox.Click
        FormComponentController.Selected_PictureBox_Click()
    End Sub

    Private Sub Profile_CheckedListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Profile_CheckedListBox.SelectedIndexChanged
        FormComponentController.Profile_CheckedListBox_SelectedIndexChanged()
    End Sub

    Private Sub Insert_To_Queuing_Click(sender As Object, e As EventArgs) Handles Insert_To_Queuing.Click
        FormComponentController.Insert_To_Queuing()
    End Sub

    Private Sub Insert_Queue_To_script_Click(sender As Object, e As EventArgs) Handles Insert_Queue_To_script.Click
        ScriptInsertion.Insert_Queue_To_script()
    End Sub

    Private Sub Insert_random_matching_text_and_all_img_btn_Click(sender As Object, e As EventArgs) Handles Insert_random_matching_text_and_all_img_btn.Click
        ScriptInsertion.Insert_random_matching_text_and_all_img()
    End Sub

    Private Sub Clear_Profile_Queue_Click(sender As Object, e As EventArgs) Handles Clear_Profile_Queue.Click
        Profile_Queue_ListBox.Items.Clear()
    End Sub

    Private Sub Selected_All_Profile_btn_Click(sender As Object, e As EventArgs) Handles Selected_All_Profile_btn.Click
        FormComponentController.Set_Selected_All_Profile(True)
    End Sub
    Private Sub Uncheck_All_Profile_btn_Click(sender As Object, e As EventArgs) Handles Uncheck_All_Profile_btn.Click
        FormComponentController.Set_Selected_All_Profile(False)
    End Sub

    Private Sub Delete_Profile_From_Queue_btn_Click(sender As Object, e As EventArgs) Handles Delete_Profile_From_Queue_btn.Click
        FormComponentController.Delete_Profile_From_Queue()
    End Sub

    Private Sub Open_Folder_with_TextFile_textbox_btn_Click(sender As Object, e As EventArgs) Handles Open_Folder_with_TextFile_textbox_btn.Click
        FormComponentController.Open_Folder_with_TextFile_textbox()
    End Sub

    Private Sub Open_Folder_with_Image_textbox_btn_Click(sender As Object, e As EventArgs) Handles Open_Folder_with_Image_textbox_btn.Click
        FormComponentController.Open_Folder_with_Image_textbox()
    End Sub

    Private Sub Refresh_All_ListBox_btn_Click(sender As Object, e As EventArgs) Handles Refresh_All_ListBox_btn.Click
        FormComponentController.Refresh_All_ListBox()
    End Sub

    Private Sub Insert_ScrollBy_Offset_btn_Click(sender As Object, e As EventArgs) Handles Insert_ScrollBy_Offset_btn.Click
        Insert_to_script("捲動頁面", ScrollBy_X_Offset_NumericUpDown.Value & ";" & ScrollBy_Y_Offset_NumericUpDown.Value & ":" & ScrollBy_Y_SingleOffset_NumericUpDown.Value)
    End Sub

    Private Sub Insert_Messager_Contact_btn_Click(sender As Object, e As EventArgs) Handles Insert_Messager_Contact_btn.Click
        ScriptInsertion.Insert_Messager_Contact()
    End Sub

    Private Async Sub Get_Groups_List_btn_Click(sender As Object, e As EventArgs) Handles Get_Groups_List_btn.Click
        Groups_ListView.Items.Clear()
        myWebDriver.Navigate_GoToUrl("https://www.facebook.com/groups/feed/")
        Try 'if there are more groups, load the groups via button clicked
            myWebDriver.chromeDriver.FindElement(By.XPath("//span[contains(text(),'查看更多')]")).Click()
            Await Delay_msec(1000)
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try

        Dim scroll_x_value = 2000
        Dim pre_counter As Integer = 0


        While True
            scroll_x_value += 1000
            Dim my_counter As Integer = myWebDriver.chromeDriver.FindElements(By.CssSelector("div.x9f619.x1n2onr6.x1ja2u2z.x78zum5.x1iyjqo2.xs83m0k.xeuugli.x1qughib.x6s0dn4.x1a02dak.x1q0g3np.xdl72j9 > div > div > div > div:nth-child(1) > span")).Count
            If my_counter = pre_counter Then
                Exit While
            End If

            myWebDriver.chromeDriver.ExecuteJavaScript("document.getElementsByClassName(""xb57i2i x1q594ok x5lxg6s x78zum5 xdt5ytf x6ikm8r x1ja2u2z x1pq812k x1rohswg xfk6m8 x1yqm8si xjx87ck x1l7klhg x1iyjqo2 xs83m0k x2lwn1j xx8ngbg xwo3gff x1oyok0e x1odjw0f x1e4zzel x1n2onr6 xq1qtft"")[0].scroll(0," & scroll_x_value & ")")
            pre_counter = my_counter
            Await Delay_msec(1000)
        End While

        Dim group_name_classes = myWebDriver.chromeDriver.FindElements(By.CssSelector("div.x9f619.x1n2onr6.x1ja2u2z.x78zum5.x1iyjqo2.xs83m0k.xeuugli.x1qughib.x6s0dn4.x1a02dak.x1q0g3np.xdl72j9 > div > div > div > div:nth-child(1) > span > span > span"))
        Dim group_url_classes = myWebDriver.chromeDriver.FindElements(By.CssSelector("div.x9f619.x1n2onr6.x1ja2u2z.x78zum5.xdt5ytf.x2lah0s.x193iq5w.x1sxyh0.xurb0ha > a"))
        'Debug.WriteLine(group_name_classes.Count)
        For i As Integer = 0 To group_name_classes.Count - 1
            'Debug.WriteLine(group_name_classes.ElementAt(i))
            Groups_ListView.Items.Add(group_name_classes.ElementAt(i).GetAttribute("innerHTML"), 100)
            Groups_ListView.Items(i).SubItems.Add(group_url_classes.ElementAt(i).GetAttribute("href"))
        Next
    End Sub

    Private Sub Delete_Selected_Profile_Folder_btn_Click(sender As Object, e As EventArgs) Handles Delete_Selected_Profile_Folder_btn.Click
        FormComponentController.Delete_Selected_Profile_Folder()
    End Sub

    Private Sub Insert_Script_Start_TIme_btn_Click(sender As Object, e As EventArgs) Handles Insert_Script_Start_TIme_btn.Click
        start_time = NumericUpDown_script_start_hour.Value.ToString.PadLeft(2, "0") + ":" + NumericUpDown_script_start_minute.Value.ToString.PadLeft(2, "0") + ":" + NumericUpDown_script_start_second.Value.ToString.PadLeft(2, "0")
        Insert_to_script("開始", start_time)
    End Sub

    Private Sub Script_Config_ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Script_Config_ComboBox.SelectedIndexChanged
        FormComponentController.Script_Config_ComboBox_SelectedIndexChanged()
    End Sub

    Private Sub Delete_Conditions_Listview_item_btn_Click(sender As Object, e As EventArgs) Handles Delete_Conditions_Listview_item_btn.Click
        For i As Integer = Match_Condition_ListView.SelectedIndices.Count - 1 To 0 Step -1
            Match_Condition_ListView.Items.RemoveAt(Match_Condition_ListView.SelectedIndices(i))
        Next
    End Sub

    Private Sub Insert_Exit_Program_btn_Click(sender As Object, e As EventArgs) Handles Insert_Exit_Program_btn.Click
        ScriptInsertion.Insert_Exit_Program()
    End Sub

    Private Sub Insert_Shutdown_System_btn_Click(sender As Object, e As EventArgs) Handles Insert_Shutdown_System_btn.Click
        ScriptInsertion.Insert_Shutdown_System()
    End Sub

    Private Sub Insert_SendKeyClick_btn_Click(sender As Object, e As EventArgs) Handles Insert_SendKeyClick_btn.Click
        If KeyboardFirstKey_ComboBox.Text <> "" And KeyboardSecondKey_ComboBox.Text <> "" Then
            Insert_to_script("發送:按鍵", KeyboardFirstKey_ComboBox.Text + "+" + KeyboardSecondKey_ComboBox.Text)
        ElseIf KeyboardFirstKey_ComboBox.Text <> "" Then
            Insert_to_script("發送:按鍵", KeyboardFirstKey_ComboBox.Text)
        Else
            MsgBox("未選擇任何按鍵")
        End If

    End Sub

    Private Sub Insert_Mouse_Click_by_Offset_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Mouse_Click_by_Offset_Btn.Click

        If CursorX_TextBox.Text <> "" And CursorY_TextBox.Text <> "" Then
            Insert_to_script("點擊:座標", CursorX_TextBox.Text + ";" + CursorY_TextBox.Text)
        Else
            MsgBox("座標數據數值不能為空")
        End If


    End Sub

    Private Sub Save_Search_Keyword_btn_Click(sender As Object, e As EventArgs) Handles Save_Search_Keyword_btn.Click
        FormComponentController.Save_Search_Keyword_btn_Click()
    End Sub

    Private Sub Save_Navigation_URL_btn_Click(sender As Object, e As EventArgs) Handles Save_Navigation_URL_btn.Click
        FormComponentController.Save_Navigation_URL_btn_Click()
    End Sub

    Private Sub Searching_Keyword_CheckedListBox_Click(sender As Object, e As EventArgs) Handles Searching_Keyword_CheckedListBox.ItemCheck
        FormComponentController.Searching_Keyword_CheckedListBox_OnClick()
    End Sub

    Private Sub Navigation_URL_CheckedListBox_Click(sender As Object, e As EventArgs) Handles Navigation_URL_CheckedListBox.ItemCheck
        FormComponentController.Navigation_URL_CheckedListBox_OnClick()
    End Sub

    Private Sub Searching_Keyword_Text_SaveAs_btn_Click(sender As Object, e As EventArgs) Handles Searching_Keyword_Text_SaveAs_btn.Click
        FormComponentController.Searching_Keyword_Text_SaveAs()
    End Sub

    Private Sub Navigation_URL_Text_SaveAs_btn_Click(sender As Object, e As EventArgs) Handles Navigation_URL_Text_SaveAs_btn.Click
        FormComponentController.Navigation_URL_Text_SaveAs()
    End Sub

    Private Sub Reveal_Keyword_Folder_btn_Click(sender As Object, e As EventArgs) Handles Reveal_Keyword_Folder_btn.Click
        FormComponentController.Reveal_Keyword_Folder()
    End Sub

    Private Sub Reveal_Navigation_URL_Folder_btn_Click(sender As Object, e As EventArgs) Handles Reveal_Navigation_URL_Folder_btn.Click
        FormComponentController.Reveal_Navigation_URL_Folder()
    End Sub

    Private Sub Delete_Keyword_Folder_btn_Click(sender As Object, e As EventArgs) Handles Delete_Keyword_Folder_btn.Click
        FormComponentController.Delete_Keyword_Folder()
    End Sub

    Private Sub Delete_NavigationURL_Folder_btn_Click(sender As Object, e As EventArgs) Handles Delete_NavigationURL_Folder_btn.Click
        FormComponentController.Delete_Navigation_URL_File()
    End Sub

    Private Sub Insert_Searching_Keyword_btn_Click(sender As Object, e As EventArgs) Handles Insert_Searching_Keyword_btn.Click
        If Search_Engine_ComboBox.Text <> "" And Searching_keyword_Text_Textbox.Text <> "" Then
            Insert_to_script("搜尋", Search_Engine_ComboBox.Text + ";" + Searching_keyword_Text_Textbox.Text)
        Else
            MsgBox("搜尋引擎或內容為空")
        End If

    End Sub

    Private Sub Insert_Random_Searching_Keyword_btn_Click(sender As Object, e As EventArgs) Handles Insert_Random_Searching_Keyword_btn.Click
        ScriptInsertion.Insert_Random_Searching_Keyword()
    End Sub

    Private Sub Insert_Random_Navigation_URL_btn_Click(sender As Object, e As EventArgs) Handles Insert_Random_Navigation_URL_btn.Click
        ScriptInsertion.Insert_Random_Navigation_URL()
    End Sub


    Private Sub Refresh_Searching_Keyword_CheckedListBox_btn_Click(sender As Object, e As EventArgs) Handles Refresh_Searching_Keyword_CheckedListBox_btn.Click
        Searching_Keyword_CheckedListBox.Items.Clear()
        FormInit.Render_Keyword_TextFIle()
    End Sub

    Private Sub Refresh_Navigation_URL_CheckedListBox_btn_Click(sender As Object, e As EventArgs) Handles Refresh_Navigation_URL_CheckedListBox_btn.Click
        Navigation_URL_CheckedListBox.Items.Clear()
        FormInit.Render_URL_TextFIle()
    End Sub

    Private Sub Get_mGroups_List_btn_Click(sender As Object, e As EventArgs) Handles Get_mGroups_List_btn.Click
        Dim curr_row As Integer = 0
        Dim pre_counter As Integer = 0
        Groups_ListView.Items.Clear()
        '### Find other groups ###
        myWebDriver.chromeDriver.Navigate.GoToUrl("https://m.facebook.com/groups_browse/your_groups/")
        Thread.Sleep(3000)
        pre_counter = 0
        While True ' Scroll to the bottom
            Dim my_counter As Integer = myWebDriver.chromeDriver.FindElements(By.CssSelector("a > ._7hkf._3qn7._61-3._2fyi._3qng")).Count
            'Debug.WriteLine(my_counter)
            If my_counter = pre_counter Then
                Exit While
            End If
            myWebDriver.chromeDriver.ExecuteJavaScript("window.scrollTo(0, document.body.scrollHeight);")
            pre_counter = my_counter
            Thread.Sleep(1000)
        End While


        'Add to the listview
        Dim group_name_classes = myWebDriver.chromeDriver.FindElements(By.CssSelector(".x1jchvi3.x132q4wb.xzsf02u.x117nqv4"))
        Dim group_url_classes = myWebDriver.chromeDriver.FindElements(By.CssSelector("div._7hkf._3qn7._61-3._2fyi._3qng > a"))

        'Debug.WriteLine(group_url_classes.Count)
        For i As Integer = 0 To group_url_classes.Count - 1
            'Debug.WriteLine(group_classes.ElementAt(i).GetAttribute("href"))
            Groups_ListView.Items.Add(group_name_classes.ElementAt(i).GetAttribute("innerHTML"), 100)
            'Groups_ListView.Items.Add("NameGG", 100)
            Groups_ListView.Items(curr_row).SubItems.Add(group_url_classes.ElementAt(i).GetAttribute("href"))
            curr_row += 1
        Next
    End Sub

    Private Sub GroupList_Replace_String_Btn_Click(sender As Object, e As EventArgs) Handles GroupList_Replace_String_Btn.Click
        For Each item As ListViewItem In Groups_ListView.Items
            Debug.WriteLine(item.SubItems(1).Text)
            item.SubItems(1).Text = item.SubItems(1).Text.Replace(GroupList_Target_String_ComboBox.Text, GroupList_Replaced_String_ComboBox.Text)
        Next
    End Sub

    Private Sub Groups_ListView_ItemSelectionChanged(sender As Object, e As EventArgs) Handles Groups_ListView.ItemSelectionChanged
        For i As Integer = Groups_ListView.SelectedIndices.Count - 1 To 0 Step -1
            'Debug.WriteLine(Groups_ListView.SelectedItems.Item(0).SubItems(0).Text)
            GroupList_GroupName_Textbox.Text = Groups_ListView.SelectedItems.Item(0).SubItems(0).Text
            GroupList_GroupURL_Textbox.Text = Groups_ListView.SelectedItems.Item(0).SubItems(1).Text
        Next
    End Sub

    Private Sub Insert_GroupList_Navigate_ToURL_btn_Click(sender As Object, e As EventArgs) Handles Insert_GroupList_Navigate_ToURL_btn.Click
        ScriptInsertion.Insert_navigate_to_url_in_GroupList()
    End Sub

    Private Sub Reveal_Auto_Generated_Folder_Btn_Click(sender As Object, e As EventArgs) Handles Reveal_Auto_Generated_Folder_Btn.Click

        Dim mypath As String = FormInit.auto_generated_textfile_path
        If Not System.IO.Directory.Exists(mypath) Then
            System.IO.Directory.CreateDirectory(mypath)
        End If
        Process.Start("explorer.exe", mypath)
    End Sub

    Private Sub Auto_Generated_TextFile_Btn_Click(sender As Object, e As EventArgs) Handles Auto_Generated_TextFile_Btn.Click
        FormComponentController.Auto_Generated_TextFile()
    End Sub



    Private Async Sub Crawl_Post_Content_Btn_Click(sender As Object, e As EventArgs) Handles Crawl_Post_Content_Btn.Click

        Crawl_Post_Content_Btn.Enabled = False
        Crawl_Post_Content_Btn.Text = "抓取中..."

        Dim chromeDriverCrawler As IWebDriver

        Dim driverManager = New DriverManager()
        driverManager.SetUpDriver(New ChromeConfig(), VersionResolveStrategy.MatchingBrowser) 'automatically download a chromedriver.exe matching the version of the browser
        Dim serv As ChromeDriverService = ChromeDriverService.CreateDefaultService

        serv.HideCommandPromptWindow = True 'hide cmd

        Dim options = New Chrome.ChromeOptions()

        options.AddArguments("--disable-notifications", "--disable-popup-blocking")

        If Crawl_Headless_Mode_CheckBox.Checked = True Then
            options.AddArguments("--disable-notifications", "--disable-popup-blocking", "--headless", "--disable-gpu")
        Else
            options.AddArguments("--disable-notifications", "--disable-popup-blocking")
        End If

        chromeDriverCrawler = New ChromeDriver(serv, options)

        Try
            chromeDriverCrawler.Navigate.GoToUrl(Crawler_Post_URL_TextBox.Text)
            If chromeDriverCrawler.Url.Contains("?next=") Then
                Await Delay_msec(2000) ' waiting for redirection
                chromeDriverCrawler.Navigate.GoToUrl(Crawler_Post_URL_TextBox.Text)
            End If
            'next=
            Dim css_selector_str = "div[data-testid='post_message'] > div"

            If Crawler_Post_URL_TextBox.Text.Contains("m.facebook.com") Then
                css_selector_str = "div._5rgt._5nk5 > div > p"
            End If

            Crawler_Post_Content_RichTextBox.Text = chromeDriverCrawler.FindElement(By.CssSelector(css_selector_str)).GetAttribute("innerHTML")
            chromeDriverCrawler.Quit()
            Crawl_Post_Content_Btn.Enabled = True
            Crawl_Post_Content_Btn.Text = "抓取貼文內容"
        Catch ex As Exception
            Debug.WriteLine(ex)
            'chromeDriverCrawler.Quit()
            Crawler_Post_Content_RichTextBox.Text = "抓取失敗,網址錯誤或者其他錯誤"
            Crawl_Post_Content_Btn.Enabled = True
            Crawl_Post_Content_Btn.Text = "抓取貼文內容"
        End Try

    End Sub

    Private Sub Split_NewLine_By_Char_btn_Click(sender As Object, e As EventArgs) Handles Split_NewLine_By_Char_btn.Click
        If Auto_GenerateTextFile_RichTextBox.Text = "" Then
            Return
        End If
        Dim myRichBoxText() = Auto_GenerateTextFile_RichTextBox.Text.Split(Pattern_Str_ComboBox.Text)

        Auto_GenerateTextFile_RichTextBox.Clear()
        For Each line In myRichBoxText
            Auto_GenerateTextFile_RichTextBox.AppendText(line & vbCr)
        Next

    End Sub

    Private Sub Delete_Str_from_RTBox_btn_Click(sender As Object, e As EventArgs) Handles Delete_Str_from_RTBox_btn.Click
        Auto_GenerateTextFile_RichTextBox.Text = Auto_GenerateTextFile_RichTextBox.Text.Replace(Pattern_Str_ComboBox.Text, "")
    End Sub
End Class