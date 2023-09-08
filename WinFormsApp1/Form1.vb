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
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports AngleSharp.Text
Imports System.Diagnostics.Metrics
Imports System.Xml
Imports System.Collections.Specialized
Imports System.Reflection.Metadata
Imports System.Buffers
Imports ICSharpCode.SharpZipLib.Zip
Imports System.Text.Json

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

    'Dim act = New Actions(chromeDriver)e
    Public act As Actions


    Public myWebDriver As New MyWebDriver()
    Public keyboardMouseController As New KeyboardAndMouseController()

    ' ######### Keyboard hook #############




    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Me.Text = "Main Form - " + Version
        'Default String : 
        Profile_TextBox.Text = FormInit.curr_path + "profiles/available"
        Block_Text_TextBox.Text = "分隔行"
        curr_url_ComboBox.Text = "https://www.facebook.com/"
        FormInit.FormInit_Render_All()
        Filter_Available_Profile_CheckBox.Checked = True
    End Sub


    'Overload MsgBox
    Public Sub MsgBox(msg As String)
        MessageBox.Show(msg, Me.Text, MessageBoxButtons.OK)
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
        Profile_Queue = {}
        Restore_ListViewItems_BackColor()
        For Each item As ListViewItem In script_ListView.Items
            item.SubItems.Item(3).Text = ""
            item.SubItems.Item(6).Text = ""
        Next
    End Sub

    Dim profile_index = 0
    Dim selected_matched_folder As String
    Dim matched_type As String

    Private Async Sub Run_script_controller()
        Dim i = 1

        While True
            For Each item As ListViewItem In script_ListView.Items
                item.SubItems.Item(6).Text = ""
            Next

            Await Run_script(i)
            If loop_run = False Then
                script_running = False
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

        'Debug.WriteLine("**************  Run : " & i & " *********************************")
        Dim record_script = False
        Dim log_file_path As String = ""
        If Record_script_result_checkbox.Checked = True Then
            log_file_path = MyLogging.Get_NewLogFile_dir()
            record_script = True
        End If
        'Dim rnd_num As New Random()
        Dim curr_line = 0
        Dim ignore_counter = 0
        For Each item As ListViewItem In script_ListView.Items

            curr_line += 1


            ' Only Run Selected row script
            If seleted_script_item_index <> 0 Then

                If curr_line <> seleted_script_item_index Then
                    Continue For
                End If

            End If

            ' run script from input start to end
            If Script_Start_Row_Index <> 0 Then

                If curr_line < Script_Start_Row_Index Then
                    Continue For
                ElseIf curr_line > Script_End_Row_Index Then
                    Continue For
                End If
            End If



            Restore_ListViewItems_BackColor()
            item.BackColor = Color.SteelBlue
            item.ForeColor = Color.White
            item.EnsureVisible()



            If Pause_Script = True Then ' Pause script
                While True
                    Await Delay_msec(1000)
                    'Debug.WriteLine("Pause_Script : " & Pause_Script)
                    If Pause_Script = False Then
                        Debug.WriteLine("Go ")
                        Exit While
                    End If

                    If Flag_start_script Then
                        Return
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


            If ignore_counter > 0 Then
                ignore_counter -= 1
                execute_result.Text = "略過"
                Await Delay_msec(1000)
                Continue For
            End If



            Select Case action
                Case "開始"
                    Continue_time = content
                    Pause_Script = True
                    boolean_result = True
                    'Debug.WriteLine(Continue_time)
                Case "開啟"

                    myWebDriver.headless_mode = Headless_Mode_Checkbox.Checked

                    If profile = "" Or profile <> running_chrome_profile Then

                        If content.Contains(";"c) Then
                            Dim allProfile = content.Split(";")
                            'Debug.WriteLine("count : " & allProfile.Count)
                            Dim rnd = rnd_num.Next(0, allProfile.Count)
                            content = allProfile(rnd)
                        End If

                        'Debug.WriteLine(content)
                        'boolean_result = True
                        'Return
                        used_chrome_profile = content.Split("\")(UBound(content.Split("\")))
                        item.SubItems.Item(3).Text = used_chrome_profile
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

                    'boolean_result = True

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

                Case "開啟:資料夾佇列"

                    loop_run = True
                    If i = 1 Then
                        Dim ProfileArrayList As ArrayList = New ArrayList()
                        Dim dirs() As String = IO.Directory.GetDirectories(FormInit.profile_path + "available")
                        For Each dir As String In dirs
                            ProfileArrayList.Add(dir)
                        Next
                        'ProfileArrayList.CopyTo(Profile_Queue)
                        Profile_Queue = DirectCast(ProfileArrayList.ToArray(GetType(String)), String())

                        If content = "隨機" Then
                            FormComponentController.RandomizeArray(Profile_Queue)
                        End If

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

                    Dim wrapper As New Simple3Des("password")
                    Dim plainText As String = wrapper.DecryptData(account_passwd(1).Split(":")(1))

                    'Debug.WriteLine(plainText)

                    boolean_result = myWebDriver.Login_fb(account_passwd(0).Split(":")(1), plainText)
                    'boolean_result = True
                    Await Delay_msec(1000)
                Case "前往"
                    If content.Contains(";"c) Then
                        content = content.Split(";")(1)
                    End If
                    boolean_result = Await myWebDriver.Navigate_GoToUrl_Task(content)

                Case "前往:隨機"

                    Dim URLTextFiles = content.Split(";")
                    Dim rnd = rnd_num.Next(0, URLTextFiles.Length)
                    'content_RichTextBox.Text = File.ReadAllText(TextFiles(rnd))
                    boolean_result = Await myWebDriver.Navigate_GoToUrl_Task(File.ReadAllText(FormInit.URL_Navigation_path + URLTextFiles(rnd)))

                Case "前往社團"
                    'Debug.WriteLine("Running Profile : " + myWebDriver.running_chrome_profile_path)
                    Dim groupListFilePath = FormInit.profile_path + content.Split(";")(0) + "\GroupList.txt"
                    If content.Split(";")(0) = "不指定" Then
                        groupListFilePath = myWebDriver.running_chrome_profile_path + "\GroupList.txt"
                        Debug.WriteLine(groupListFilePath)
                    End If

                    If My.Computer.FileSystem.FileExists(groupListFilePath) Then

                        Dim GroupsFileText = File.ReadAllLines(groupListFilePath)
                        Dim rnd = rnd_num.Next(0, GroupsFileText.Count)

                        Dim line_counter = 0
                        For Each line In GroupsFileText

                            If line_counter = rnd Then
                                Dim myGroupURL = line.Split("&nbsp")(1)
                                'Debug.WriteLine("GOTO " + line)
                                If Await myWebDriver.Navigate_GoToUrl_Task(myGroupURL) Then
                                    boolean_result = True
                                Else
                                    boolean_result = False
                                End If
                                Exit For
                            End If
                            line_counter += 1
                        Next
                    Else
                        boolean_result = False
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
                Case "點擊:留言"
                    boolean_result = Await myWebDriver.Click_reply_Task(CInt(content))
                Case "點擊:文字"
                    Try
                        Dim mycontent = content.Split(";")
                        boolean_result = Await myWebDriver.Click_By_Text_Task(mycontent(0), mycontent(1))
                    Catch ex As Exception
                        boolean_result = False
                    End Try

                Case "分享網址"
                    boolean_result = Await myWebDriver.Write_post_send_content_Task(content)
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
                    Try
                        boolean_result = Await Post_Random_Match_TextAndImage(content)
                    Catch ex As Exception
                        boolean_result = False
                    End Try

                Case "發送上載:隨機配對多圖"
                    Try
                        boolean_result = Await Post_Random_Match_TextAndImageFolder(content)
                    Catch ex As Exception
                        boolean_result = False
                    End Try

                Case "清空"
                    boolean_result = Await myWebDriver.Clear_post_content_Task()
                Case "上載"
                    Try
                        boolean_result = Await myWebDriver.Tring_to_upload_img_Task(content)
                        Debug.WriteLine(content)
                    Catch ex As Exception
                        boolean_result = False
                    End Try

                Case "上載:隨機"
                    Try
                        If content = "全部隨機" Then
                            Dim allImageFile = img_CheckedListBox.Items
                            Dim rnd = rnd_num.Next(0, allImageFile.Count)
                            boolean_result = Await myWebDriver.Tring_to_upload_img_Task(image_folder_path + allImageFile(rnd))
                        Else
                            Dim ImageFiles = content.Split(";")
                            Dim rnd = rnd_num.Next(0, ImageFiles.Length)
                            boolean_result = Await myWebDriver.Tring_to_upload_img_Task(image_folder_path + ImageFiles(rnd))
                        End If
                    Catch ex As Exception
                        boolean_result = False
                    End Try

                Case "插入背景"
                    boolean_result = Await myWebDriver.Insert_Post_Background_Task()

                Case "回應:上載"
                    Try
                        boolean_result = Await myWebDriver.Upload_reply_img_Task(image_folder_path + content)
                    Catch ex As Exception
                        boolean_result = False
                    End Try

                Case "回應:上載隨機"
                    Try
                        If content = "全部隨機" Then
                            Dim allImageFile = img_CheckedListBox.Items
                            Dim rnd = rnd_num.Next(0, allImageFile.Count)
                            boolean_result = Await myWebDriver.Upload_reply_img_Task(image_folder_path + allImageFile(rnd))
                        Else
                            Dim ImageFiles = content.Split(";")
                            Dim rnd = rnd_num.Next(0, ImageFiles.Length)
                            boolean_result = Await myWebDriver.Upload_reply_img_Task(image_folder_path + ImageFiles(rnd))
                        End If

                    Catch ex As Exception
                        boolean_result = False
                    End Try


                Case "限時:上載"
                    Try
                        boolean_result = Await myWebDriver.Upload_Story_img_Task(content)
                    Catch ex As Exception
                        boolean_result = False
                    End Try

                Case "限時:隨機上載"
                    Try
                        If content = "全部隨機" Then
                            Dim allImageFile = img_CheckedListBox.Items
                            Dim rnd = rnd_num.Next(0, allImageFile.Count)
                            boolean_result = Await myWebDriver.Upload_Story_img_Task(image_folder_path + allImageFile(rnd))
                        Else
                            Dim ImageFiles = content.Split(";")
                            Dim rnd = rnd_num.Next(0, ImageFiles.Length)
                            boolean_result = Await myWebDriver.Upload_Story_img_Task(image_folder_path + ImageFiles(rnd))
                        End If
                    Catch ex As Exception
                        boolean_result = False
                    End Try


                Case "回應:內容"
                    Try
                        boolean_result = Await myWebDriver.Send_reply_comment_Task(content)
                    Catch ex As Exception
                        boolean_result = False
                    End Try

                Case "回應:隨機"
                    Try
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
                    Catch ex As Exception
                        boolean_result = False
                    End Try

                Case "回應:隨機配對"
                    boolean_result = Await myWebDriver.Reply_Random_Match_TextAndImage(content)

                Case "回應:送出"

                    If myWebDriver.IsUploadImage Then

                        'div.x1iyjqo2.xurb0ha.x1sxyh0.xh8yej3 > div.xzpqnlu.x1hyvwdk.xjm9jq1.x6ikm8r.x10wlt62.x10l6tqk.x1i1rx1s

                        'x1iyjqo2 x1emribx x1xmf6yo x1e56ztr

                        For i = 0 To 15
                            Await Delay_msec(2000)

                            If Await myWebDriver.IsElementPresentByClass_Task(".x1iyjqo2.x1emribx.x1xmf6yo.x1e56ztr") = False Then
                                Debug.WriteLine("NOT !!!!")
                                boolean_result = Await myWebDriver.Submit_reply_comment_Task()
                                Exit For
                            End If

                        Next

                    Else
                        boolean_result = Await myWebDriver.Submit_reply_comment_Task()
                    End If

                    myWebDriver.IsUploadImage = False

                Case "回應:按讚"
                    boolean_result = Await myWebDriver.Click_reply_random_emoji_Task(content)
                Case "捲動頁面"
                    Dim Offset As String() = content.Split(";")
                    Dim y_offset = CInt(Offset(1).Split(":")(0))
                    Dim y_single_offset = CInt(Offset(1).Split(":")(1))
                    Dim y_single_delay_offset = CInt(Offset(1).Split(":")(2))

                    'Debug.WriteLine(y_single_delay_offset)
                    If y_single_offset > 0 Then
                        For scroll_offset As Integer = y_single_offset To y_offset Step y_single_offset
                            boolean_result = ScrollPage_By_Offset(Offset(0), CStr(scroll_offset))
                            For sec = y_single_delay_offset To 0 Step -1
                                Debug.WriteLine(y_single_delay_offset)
                                item.SubItems.Item(6).Text = CStr(scroll_offset) + ":" + CStr(sec)
                                Await Delay(1000)
                            Next

                        Next
                    Else
                        boolean_result = ScrollPage_By_Offset(Offset(0), CStr(y_offset))
                    End If
                Case "發送:按鍵"
                    boolean_result = myWebDriver.SendBrowserKeyAction(content)
                Case "點擊:座標"
                    Dim offset() = content.Split(";")
                    boolean_result = myWebDriver.Click_by_Cursor_Offset(offset(0), offset(1))
                Case "聊天"
                    Dim images = content.Split(";")(0)
                    Dim Text = content.Split(";")(1)
                    boolean_result = Await myWebDriver.Messager_Contact_Task(images, Text)
                Case "聊天:送出"
                    boolean_result = Await myWebDriver.Messager_Submit_Content_Task()
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
                        Close()
                        Process.Start("shutdown", "-s -t 0")
                    Catch ex As Exception
                        Debug.WriteLine(ex)
                        boolean_result = False
                    End Try
                Case "系統點擊:左鍵"
                    Dim position = content.Split(";")(1)
                    Dim counter = CInt(content.Split(";")(0))
                    For i = counter To 1 Step -1
                        item.SubItems.Item(6).Text = (i.ToString())
                        Await Delay_msec(1000)
                    Next
                    boolean_result = keyboardMouseController.System_Mouse_OnClick_By_Position("left", CInt(position.Split(":")(0)), CInt(position.Split(":")(1)))
                Case "系統點擊:右鍵"
                    Dim position = content.Split(";")(1)
                    Dim counter = CInt(content.Split(";")(0))
                    For i = counter To 1 Step -1
                        item.SubItems.Item(6).Text = (i.ToString())
                        Await Delay_msec(1000)
                    Next
                    boolean_result = keyboardMouseController.System_Mouse_OnClick_By_Position("right", CInt(position.Split(":")(0)), CInt(position.Split(":")(1)))
                Case "系統發送:按鍵"
                    boolean_result = System_SendKey(content.Split("+")(0), content.Split("+")(1))

                Case "複製:檔案內容"
                    Try
                        Dim txtFileContent = IO.File.ReadAllText(content)
                        'Clipboard.SetImage(myimage)
                        Clipboard.SetText(txtFileContent)
                        boolean_result = True

                    Catch ex As Exception
                        Debug.WriteLine(ex)
                        boolean_result = False
                    End Try

                Case "複製:圖片位置"
                    Try
                        Dim imageFilePath_StringCollection = New StringCollection()
                        For Each file In content.Split(";")
                            'Dim myimage = Bitmap.FromFile(file)
                            imageFilePath_StringCollection.Add(file)
                        Next
                        Clipboard.SetFileDropList(imageFilePath_StringCollection)
                        'Clipboard.SetImage()
                        boolean_result = True
                    Catch ex As Exception
                        Debug.WriteLine(ex)
                        boolean_result = False
                    End Try
                Case "複製:隨機資料夾"
                    Try
                        matched_type = "single"
                        Dim dirs() As String = IO.Directory.GetDirectories("resources\matched_resource")
                        Dim rnd = rnd_num.Next(0, dirs.Length)
                        selected_matched_folder = dirs(rnd)

                        If content = "文字和圖片" Then
                            'Debug.WriteLine("text and image")
                            matched_type = "single"
                        ElseIf content = "文字和全部圖片" Then
                            'Debug.WriteLine("text and all image")
                            matched_type = "all"
                        End If
                        boolean_result = True
                    Catch ex As Exception
                        boolean_result = False
                    End Try
                Case "貼上:隨機資料夾"
                    Clipboard.Clear()
                    Try
                        Debug.WriteLine("Seleted dir :" + selected_matched_folder)
                        Debug.WriteLine("Type :" + matched_type)

                        Dim files() As String = IO.Directory.GetFiles(selected_matched_folder)
                        Dim textFile As ArrayList = New ArrayList()
                        Dim imageFile As ArrayList = New ArrayList()

                        For Each file As String In files
                            'Debug.WriteLine(file)
                            Dim extension = Path.GetExtension(file)
                            If extension = ".txt" Then
                                textFile.Add(file)
                            ElseIf extension = ".png" Or extension = ".jpg" Or extension = ".jpeg" Then
                                imageFile.Add(file)
                            End If
                        Next

                        Dim txtrnd = rnd_num.Next(0, textFile.Count)
                        Clipboard.SetText(ReadAllText(textFile(Rnd)))
                        System_SendKey("CTRL", "v")

                        If matched_type = "single" Then
                            Dim imgrnd = rnd_num.Next(0, imageFile.Count)
                            Dim image_file = Bitmap.FromFile(imageFile(imgrnd))
                            Clipboard.SetImage(image_file)
                        ElseIf matched_type = "all" Then
                            Dim imageFilePath_StringCollection = New StringCollection()
                            For Each img In imageFile
                                'Dim myimage = Bitmap.FromFile(file)
                                'Debug.WriteLine(My.Computer.FileSystem.CurrentDirectory + "\" + img)
                                imageFilePath_StringCollection.Add(My.Computer.FileSystem.CurrentDirectory + "\" + img)
                            Next
                            Clipboard.SetFileDropList(imageFilePath_StringCollection)

                        Else
                            MsgBox("Error! No Type Seleted")
                        End If
                        System_SendKey("CTRL", "v")
                        boolean_result = True
                    Catch ex As Exception
                        Debug.WriteLine(ex)
                        boolean_result = False
                    End Try


                Case "更改密碼"
                    boolean_result = Await myWebDriver.Change_Fb_Password_Task()
                Case "檢查"
                    Try
                        Dim last_line_result = script_ListView.Items(curr_line - 2).SubItems.Item(6).Text
                        If last_line_result = "失敗" Then
                            ignore_counter = CInt(content)
                        End If
                        boolean_result = True
                    Catch ex As Exception
                        boolean_result = False
                    End Try
                Case "拍賣:按拍賣"
                    boolean_result = Await myWebDriver.Click_Sale_Product_Task()

                Case "拍賣:上載"
                    boolean_result = Await myWebDriver.Upload_Product_Profile_Task(content)
                Case "拍賣:發布"
                    boolean_result = Await myWebDriver.Click_Post_Product_Task()

                Case "m拍賣:上載"
                    Try
                        Dim my_list = content.Split(";")
                        Debug.WriteLine("URL : " + my_list(0))
                        Dim pruduct_folders = my_list(1).Split("/")
                        Dim rnd = rnd_num.Next(0, pruduct_folders.Length - 1)
                        Await myWebDriver.Navigate_GoToUrl_Task(my_list(0))
                        boolean_result = Await myWebDriver.Upload_Product_Profile_To_Group_Task(pruduct_folders(rnd))
                    Catch ex As Exception
                        boolean_result = False
                    End Try
                Case "m拍賣:按發布"
                    boolean_result = Await myWebDriver.m_Click_Post_Product_Task()
                Case "拍賣:IPhone上載"
                    Try
                        Dim my_list = content.Split(";")
                        Debug.WriteLine("URL : " + my_list(0))
                        Dim pruduct_folders = my_list(1).Split("/")
                        Dim rnd = rnd_num.Next(0, pruduct_folders.Length - 1)
                        Await myWebDriver.Navigate_GoToUrl_Task(my_list(0))
                        boolean_result = Await myWebDriver.Upload_Product_Profile_To_Group_IPhone_Task(pruduct_folders(rnd))
                    Catch ex As Exception
                        boolean_result = False
                    End Try

            End Select
            If boolean_result = True Then 'record the result

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
                    Debug.WriteLine(itemSeleted)
                    myprofile = itemSeleted
                Next
            End If



            'Open_Browser("Chrome", used_dev_model, myprofile)
            'Debug.WriteLine("profile : " + myprofile)
            'Exit Sub
            Await myWebDriver.Open_Browser_Task("Chrome", myWebDriver.used_dev_model, myprofile)
            Render_profile_CheckedListBox()

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

            Dim allProfileItem = Profile_CheckedListBox.Items
            Debug.WriteLine("count : " & allProfileItem.Count)
            Dim rnd = rnd_num.Next(0, allProfileItem.Count)

            Await myWebDriver.Open_Browser_Task("Chrome", myWebDriver.used_dev_model, FormInit.profile_path + allProfileItem(rnd))

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
            'Dismiss_alert()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Sub Dismiss_alert()
        Try

            Dim alert As IAlert = myWebDriver.chromeDriver.SwitchTo().Alert()

            Dim alertText As String = alert.Text

            'alert.Accept() ' 或者
            alert.Dismiss()
        Catch ex As NoAlertPresentException
            ' 如果没有警报框弹出，继续测试
        End Try
    End Sub


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


    Dim RunCountdown = False



    Dim seleted_script_item_index = 0

    Dim Script_Start_Row_Index = 0
    Dim Script_End_Row_Index = 0



    Private Sub Countdown_Toggle_Button_Click(sender As Object, e As EventArgs) Handles Countdown_Toggle_Button.Click

        If Exit_Program_Counter_CheckBox.Checked Or PowerOff_PC_Counter_CheckBox.Checked = True Then

            If RunCountdown = True Then

                Countdown_Toggle_Button.Text = "開始"
                RunCountdown = False
                Exit_Program_Counter_CheckBox.Enabled = True
                PowerOff_PC_Counter_CheckBox.Enabled = True
                myHoursCounter_NumericUpDown.Enabled = True
                myMinutesCounter_NumericUpDown.Enabled = True
                mySecondsCounter_NumericUpDown.Enabled = True

            ElseIf RunCountdown = False Then

                Countdown_Toggle_Button.Text = "取消"
                RunCountdown = True
                Exit_Program_Counter_CheckBox.Enabled = False
                PowerOff_PC_Counter_CheckBox.Enabled = False
                myHoursCounter_NumericUpDown.Enabled = False
                myMinutesCounter_NumericUpDown.Enabled = False
                mySecondsCounter_NumericUpDown.Enabled = False

            End If


        Else
            MsgBox("需勾選關閉程式或者關閉電腦")
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        'Dim MousePosition As Point
        Dim MousePosition = Cursor.Position

        Cursor_X_Position_Label.Text = MousePosition.X
        Cursor_Y_Position_Label.Text = MousePosition.Y
        'Debug.WriteLine(MousePosition.X)
        Dim TimeNow = Date.Now.ToString("HH:mm:ss")
        'Debug.WriteLine("current : " + Date.Now.ToString("HH:mm:ss"))
        'Debug.WriteLine("Ctime : " + Continue_time)
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


        ' Countdown
        If RunCountdown Then
            Dim total_secs = myHoursCounter_NumericUpDown.Value * 3600 + myMinutesCounter_NumericUpDown.Value * 60 + mySecondsCounter_NumericUpDown.Value


            'Exit Program or Shutdown PC
            If total_secs = 0 Then
                If Exit_Program_Counter_CheckBox.Checked = True Then
                    'Debug.WriteLine("離開程式")
                    Close()
                ElseIf PowerOff_PC_Counter_CheckBox.Checked = True Then
                    Close()
                    Process.Start("shutdown", "-s -t 0")
                End If
                Return
            End If

            total_secs -= 1

            myHoursCounter_NumericUpDown.Value = Int(total_secs / 3600)
            total_secs = total_secs Mod 3600
            myMinutesCounter_NumericUpDown.Value = Int(total_secs / 60)
            mySecondsCounter_NumericUpDown.Value = total_secs Mod 60
        End If


    End Sub

    Private Sub Run_script_btn_Click(sender As Object, e As EventArgs) Handles Run_script_btn.Click
        script_ListView.SelectedItems.Clear()
        loop_run = CheckBox_loop_run.Checked
        seleted_script_item_index = 0
        Script_Start_Row_Index = 0
        Script_End_Row_Index = 0
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


    Private Sub Run_Selected_Script_Btn_Click(sender As Object, e As EventArgs) Handles Run_Selected_Script_Btn.Click
        For i As Integer = script_ListView.SelectedIndices.Count - 1 To 0 Step -1
            If script_ListView.SelectedItems.Count > 0 Then
                Dim selectedItem = script_ListView.SelectedItems.Item(0).Text
                Debug.WriteLine(selectedItem)
                seleted_script_item_index = CInt(selectedItem)
                Flag_start_script = True
            End If
        Next
    End Sub


    Private Sub Run_Script_From_Input_Row_To_Row_Btn_Click(sender As Object, e As EventArgs) Handles Run_Script_From_Input_Row_To_Row_Btn.Click
        Script_Start_Row_Index = Start_Script_Row_NumericUpDown.Value
        Script_End_Row_Index = End_Script_Row_NumericUpDown.Value

        If Script_Start_Row_Index >= Script_End_Row_Index Then
            MsgBox("開始行數不得小於等於結束行數")
        ElseIf Script_Start_Row_Index = 0 Or Script_End_Row_Index = 0 Then
            MsgBox("行數不得為0")
        Else
            Flag_start_script = True
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
        Insert_to_script("點擊:留言", comment_row_Numer.Value)
    End Sub

    Private Sub Insert_Click_reply_Top_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Click_reply_Top_Btn.Click
        Insert_to_script("點擊", "回覆上")
    End Sub

    Private Sub Insert_Click_reply_Bottom_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Click_reply_Bottom_Btn.Click
        Insert_to_script("點擊", "回覆下")
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


    Public Function System_SendKey(firstKey, secondKey)

        Try
            Select Case firstKey
                Case "ENTER"
                    My.Computer.Keyboard.SendKeys("~")
                Case "ESC"
                    My.Computer.Keyboard.SendKeys("{ESC}")
                Case "CTRL"
                    My.Computer.Keyboard.SendKeys("^" + secondKey)
                Case "ALT"
                    My.Computer.Keyboard.SendKeys("%" + secondKey)
                Case Else
                    My.Computer.Keyboard.SendKeys(secondKey)

            End Select
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

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
                    script_ListView.Items(curr_row).SubItems.Add(cmd.Replace("&vbLf", vbLf))
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

        'fb_account_TextBox.Text = ""
        'fb_password_TextBox.Text = ""
        group_name_TextBox.Text = ""
        'curr_url_ComboBox.Text = ""


        Select Case browser
            Case "Chrome"
                chrome_RadioButton.Checked = True
            Case "Firefox"
                firefox_RadioButton.Checked = True
            Case "Edge"
                edge_RadioButton.Checked = True
        End Select


        Select Case action
            'Case "前往"
            'If content.Contains(";"c) Then
            'group_name_TextBox.Text = content.Split(";")(0)
            '   content = content.Split(";")(1)
            'Else
            '     group_name_TextBox.Text = ""
            'end If
            '  curr_url_ComboBox.Text = content

            'Case "開啟"
            'profile_path_TextBox.Text = content
            'Case "登入"
            'Dim account_passwd = content.Split(" ")
            '  fb_account_TextBox.Text = account_passwd(0).Split(":")(1)
            'fb_password_TextBox.Text = account_passwd(1).Split(":")(1)
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

    Private Sub Move_Script_ListView_Item_To_Index_Btn_Click(sender As Object, e As EventArgs) Handles Move_Script_ListView_Item_To_Index_Btn.Click
        FormComponentController.Move_Script_ListView_Item_To_Index(Target_Index_Script_ListView_NummericUpDown.Value)
    End Sub

    Private Sub Text_File_CheckedListBox_Click(sender As Object, e As EventArgs) Handles Text_File_CheckedListBox.SelectedIndexChanged
        FormComponentController.Text_File_CheckedListBox_Click()

    End Sub

    Private Sub img_CheckedListBox_Click(sender As Object, e As EventArgs) Handles img_CheckedListBox.SelectedIndexChanged
        Try
            FormComponentController.Img_CheckedListBox_Click()
        Catch ex As Exception
            MsgBox("File not exist")
        End Try

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
        Insert_to_script("捲動頁面", ScrollBy_X_Offset_NumericUpDown.Value & ";" & ScrollBy_Y_Offset_NumericUpDown.Value & ":" & ScrollBy_Y_SingleOffset_NumericUpDown.Value & ":" & ScrollBy_Y_SingleDelayOffset_NumericUpDown.Value)
    End Sub


    Private Async Sub Get_Groups_List_btn_Click(sender As Object, e As EventArgs) Handles Get_Groups_List_btn.Click


        If Await myWebDriver.Navigate_GoToUrl_Task("https://www.facebook.com/groups/feed/") = False Then
            MsgBox("未偵測到Chrome")
            Exit Sub
        End If

        Groups_ListView.Items.Clear()

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
            Await Delay_msec(4000)
        End While

        Dim group_name_classes = myWebDriver.chromeDriver.FindElements(By.CssSelector("div.x9f619.x1n2onr6.x1ja2u2z.x78zum5.x1iyjqo2.xs83m0k.xeuugli.x1qughib.x6s0dn4.x1a02dak.x1q0g3np.xdl72j9 > div > div > div > div:nth-child(1) > span > span > span"))
        Dim group_url_classes = myWebDriver.chromeDriver.FindElements(By.CssSelector("div.x9f619.x1n2onr6.x1ja2u2z.x78zum5.xdt5ytf.x2lah0s.x193iq5w.x1sxyh0.xurb0ha > a"))
        'Debug.WriteLine(group_name_classes.Count)
        For i As Integer = 0 To group_name_classes.Count - 1
            'Debug.WriteLine(group_name_classes.ElementAt(i))
            Groups_ListView.Items.Add(group_name_classes.ElementAt(i).GetAttribute("innerHTML"), 100)
            Groups_ListView.Items(i).SubItems.Add(group_url_classes.ElementAt(i).GetAttribute("href"))
            'FB_Groups_CheckedListBox
        Next
    End Sub

    Private Async Sub Get_mGroups_List_btn_Click(sender As Object, e As EventArgs) Handles Get_mGroups_List_btn.Click

        If Await myWebDriver.Navigate_GoToUrl_Task("https://m.facebook.com/groups_browse/your_groups/") = False Then
            MsgBox("未偵測到Chrome")
            Exit Sub
        End If

        Dim curr_row As Integer = 0
        Dim pre_counter As Integer = 0
        Groups_ListView.Items.Clear()
        '### Find other groups ###

        Await Delay_msec(3000)
        pre_counter = 0
        While True ' Scroll to the bottom
            Dim my_counter As Integer = myWebDriver.chromeDriver.FindElements(By.CssSelector("a > ._7hkf._3qn7._61-3._2fyi._3qng")).Count
            'Debug.WriteLine(my_counter)
            If my_counter = pre_counter Then
                Exit While
            End If
            myWebDriver.chromeDriver.ExecuteJavaScript("window.scrollTo(0, document.body.scrollHeight);")
            pre_counter = my_counter
            Await Delay_msec(4000)
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



    Private Sub Delete_Selected_Profile_Folder_btn_Click(sender As Object, e As EventArgs) Handles Delete_Selected_Profile_Folder_btn.Click
        FormComponentController.Delete_Selected_Profile_Folder()
    End Sub

    Private Sub Insert_Script_Start_TIme_btn_Click(sender As Object, e As EventArgs) Handles Insert_Script_Start_TIme_btn.Click
        Dim mytime = NumericUpDown_script_start_hour.Value.ToString.PadLeft(2, "0") + ":" + NumericUpDown_script_start_minute.Value.ToString.PadLeft(2, "0") + ":" + NumericUpDown_script_start_second.Value.ToString.PadLeft(2, "0")
        Insert_to_script("開始", mytime)
    End Sub

    Private Sub Script_Config_ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Script_Config_ComboBox.SelectedIndexChanged
        FormComponentController.Load_Script_File(Script_Config_ComboBox.Text)
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

    Private Sub Searching_Keyword_CheckedListBox_Click(sender As Object, e As EventArgs)
        FormComponentController.Searching_Keyword_CheckedListBox_OnClick()
    End Sub


    Private Sub Searching_Keyword_Text_SaveAs_btn_Click(sender As Object, e As EventArgs) Handles Searching_Keyword_Text_SaveAs_btn.Click
        FormComponentController.Searching_Keyword_Text_SaveAs()
    End Sub

    Private Sub Reveal_Keyword_Folder_btn_Click(sender As Object, e As EventArgs) Handles Reveal_Keyword_Folder_btn.Click
        FormComponentController.Reveal_Keyword_Folder()
    End Sub


    Private Sub Delete_Keyword_Folder_btn_Click(sender As Object, e As EventArgs) Handles Delete_Keyword_Folder_btn.Click
        FormComponentController.Delete_Keyword_Folder()
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


    Private Sub Refresh_Searching_Keyword_CheckedListBox_btn_Click(sender As Object, e As EventArgs) Handles Refresh_Searching_Keyword_CheckedListBox_btn.Click
        Searching_Keyword_CheckedListBox.Items.Clear()
        SearchingKeyword_folder_Textbox.Clear()
        Keyword_TextFileName_TextBox.Clear()
        Searching_keyword_Text_Textbox.Clear()
        FormInit.Render_Keyword_TextFIle()
    End Sub




    Private Sub GroupList_Replace_String_Btn_Click(sender As Object, e As EventArgs) Handles GroupList_Replace_String_Btn.Click
        For Each item As ListViewItem In Groups_ListView.Items
            Debug.WriteLine(item.SubItems(1).Text)
            item.SubItems(1).Text = item.SubItems(1).Text.Replace(GroupList_Target_String_ComboBox.Text, GroupList_Replaced_String_ComboBox.Text)
        Next
    End Sub

    Private Sub Groups_ListView_ItemSelectionChanged(sender As Object, e As EventArgs)
        For i As Integer = Groups_ListView.SelectedIndices.Count - 1 To 0 Step -1
            'Debug.WriteLine(Groups_ListView.SelectedItems.Item(0).SubItems(0).Text)
            GroupList_GroupName_Textbox.Text = Groups_ListView.SelectedItems.Item(0).SubItems(0).Text
            GroupList_GroupURL_Textbox.Text = Groups_ListView.SelectedItems.Item(0).SubItems(1).Text
        Next
    End Sub

    Private Sub Modify_Selected_GroupList_Item_Btn_Click(sender As Object, e As EventArgs) Handles Modify_Selected_GroupList_Item_Btn.Click
        For i As Integer = Groups_ListView.SelectedIndices.Count - 1 To 0 Step -1
            Groups_ListView.SelectedItems.Item(0).SubItems(0).Text = GroupList_GroupName_Textbox.Text
            Groups_ListView.SelectedItems.Item(0).SubItems(1).Text = GroupList_GroupURL_Textbox.Text
        Next
    End Sub

    Private Sub Add_Item_To_GroupList_Btn_Click(sender As Object, e As EventArgs) Handles Add_Item_To_GroupList_Btn.Click
        Groups_ListView.Items.Add(GroupList_GroupName_Textbox.Text, 100)
        Groups_ListView.Items(Groups_ListView.Items.Count - 1).SubItems.Add(GroupList_GroupURL_Textbox.Text)
    End Sub

    Private Async Sub Navigate_To_Selected_Group_Btn_Click(sender As Object, e As EventArgs) Handles Navigate_To_Selected_Group_Btn.Click

        For i As Integer = Groups_ListView.SelectedIndices.Count - 1 To 0 Step -1
            Dim url = Groups_ListView.SelectedItems.Item(0).SubItems(1).Text
            If Await myWebDriver.Navigate_GoToUrl_Task(url) = False Then
                MsgBox("未偵測到瀏覽器，或者網址錯誤")
            End If
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

            If chromeDriverCrawler.Url.Contains("?next=") Or chromeDriverCrawler.Url.Contains("permalink.php") Then
                Await Delay_msec(2000) ' waiting for redirection
                chromeDriverCrawler.Navigate.GoToUrl(Crawler_Post_URL_TextBox.Text)
            End If
            'next=
            Dim css_selector_str = "div[data-testid='post_message'] > div"

            If Crawler_Post_URL_TextBox.Text.Contains("m.facebook.com") Then
                css_selector_str = "div.story_body_container > div > div"
            End If

            Dim source_HTML = chromeDriverCrawler.FindElement(By.CssSelector(css_selector_str)).GetAttribute("innerHTML").Trim()

            Dim regex As New Text.RegularExpressions.Regex("<.*?>", RegexOptions.Singleline)
            Dim result As String = regex.Replace(source_HTML, String.Empty)
            'Dim result As String = regex.Replace(source_HTML, vbCrLf)


            Crawler_Post_Content_RichTextBox.Text = result

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

    Private Sub Exit_Program_Counter_CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles Exit_Program_Counter_CheckBox.CheckedChanged
        If Exit_Program_Counter_CheckBox.Checked = True Then
            PowerOff_PC_Counter_CheckBox.Checked = False
        End If

    End Sub

    Private Sub PowerOff_PC_Counter_CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles PowerOff_PC_Counter_CheckBox.CheckedChanged
        If PowerOff_PC_Counter_CheckBox.Checked = True Then
            Exit_Program_Counter_CheckBox.Checked = False
        End If
    End Sub

    Private Sub Insert_Story_Image_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Story_Image_Btn.Click
        ScriptInsertion.Insert_story_img_video()
    End Sub

    Private Sub Insert_Random_Story_Image_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Random_Story_Image_Btn.Click
        ScriptInsertion.Insert_Story_Random_Image()
    End Sub

    Private Async Sub Block_User_By_Page_Click(sender As Object, e As EventArgs) Handles Block_User_By_Page.Click
        Block_User_By_Page.Enabled = False
        Block_User_By_Page.Text = "執行中"
        Await myWebDriver.Block_User_By_Page_Task()

        Block_User_By_Page.Enabled = True
        Block_User_By_Page.Text = "封鎖"

    End Sub


    Private Sub Test_System_Mouse_Position_Click_btn_Click(sender As Object, e As EventArgs) Handles Test_System_Mouse_Position_Click_btn.Click
        If IsNumeric(Cursor_X_Position_TextBox.Text) And IsNumeric(Cursor_Y_Position_TextBox.Text) Then
            keyboardMouseController.Test_System_Mouse_Position_OnClick()
        Else
            MsgBox("非法數值")
        End If

    End Sub

    Private Sub Insert_Mouse_Left_Click_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Mouse_Left_Click_Btn.Click
        If IsNumeric(Cursor_X_Position_TextBox.Text) And IsNumeric(Cursor_Y_Position_TextBox.Text) Then
            Insert_to_script("系統點擊:左鍵", Mouse_OnClick_Delay_Sec_Numeric.Value & ";" + Cursor_X_Position_TextBox.Text + ":" + Cursor_Y_Position_TextBox.Text)
        Else
            MsgBox("非法數值")
        End If

    End Sub

    Private Sub Insert_Mouse_Right_Click_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Mouse_Right_Click_Btn.Click
        If IsNumeric(Cursor_X_Position_TextBox.Text) And IsNumeric(Cursor_Y_Position_TextBox.Text) Then
            Insert_to_script("系統點擊:右鍵", Mouse_OnClick_Delay_Sec_Numeric.Value & ";" + Cursor_X_Position_TextBox.Text + ":" + Cursor_Y_Position_TextBox.Text)
        Else
            MsgBox("非法數值")
        End If

    End Sub

    Private Sub Select_TextFile_For_Copy_Dialog_Btn_Click(sender As Object, e As EventArgs) Handles Select_TextFile_For_Copy_Dialog_Btn.Click
        FormComponentController.Select_TextFile_For_Copy_Dialog()
    End Sub

    Private Sub Select_ImageFile_For_Copy_Dialog_Btn_Click(sender As Object, e As EventArgs) Handles Select_ImageFile_For_Copy_Dialog_Btn.Click
        FormComponentController.Select_ImageFile_For_Copy_Dialog()
    End Sub

    Private Sub Insert_Copy_TextFileContent_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Copy_TextFileContent_Btn.Click
        ScriptInsertion.Insert_Copy_TextFileContent()
    End Sub

    Private Sub Insert_Copy_ImageFile_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Copy_ImageFile_Btn.Click
        ScriptInsertion.Insert_Copy_ImageFile()
    End Sub

    Private Sub Insert_SystemKeyboardClick_btn_Click(sender As Object, e As EventArgs) Handles Insert_SystemKeyboardClick_btn.Click
        If SystemKeyboardFirstKey_ComboBox.Text <> "" Or SystemKeyboardSecondKey_ComboBox.Text <> "" Then
            Insert_to_script("系統發送:按鍵", SystemKeyboardFirstKey_ComboBox.Text + "+" + SystemKeyboardSecondKey_ComboBox.Text)
        End If
    End Sub

    Private Sub Reveal_password_Textbox_Btn_Click(sender As Object, e As EventArgs) Handles Reveal_password_Textbox_Btn.Click

        Debug.WriteLine(fb_password_TextBox.PasswordChar)

        If fb_password_TextBox.PasswordChar = "*" Then
            Reveal_password_Textbox_Btn.Text = "隱藏"
            fb_password_TextBox.PasswordChar = ""
        ElseIf fb_password_TextBox.PasswordChar = vbNullChar Then
            Reveal_password_Textbox_Btn.Text = "顯示"
            fb_password_TextBox.PasswordChar = "*"
        End If

    End Sub

    Private Sub Modify_Selected_ListView_Url_Btn_Click(sender As Object, e As EventArgs) Handles Modify_Selected_ListView_Url_Btn.Click
        Modify_ScriptListView_Selected_URL_item()
    End Sub

    Private Sub Modify_Selected_ListView_AccountAndPasswd_Btn_Click(sender As Object, e As EventArgs) Handles Modify_Selected_ListView_AccountAndPasswd_Btn.Click
        Modify_ScriptListView_Selected_AccountAndPassword_item()
    End Sub

    Private Sub Move_Profile_To_Selected_Foler_btn_Click(sender As Object, e As EventArgs) Handles Move_Profile_To_Selected_Foler_btn.Click
        Dim targetDir = FormInit.profile_path + Profile_Target_Dir_Name_ComboBox.Text
        Dim allowedDir As String() = {"available", "ban", "useless"}

        'Debug.WriteLine(Array.IndexOf(allowedDir, Profile_Target_Dir_Name_ComboBox.Text))

        'Exit Sub

        If Array.IndexOf(allowedDir, Profile_Target_Dir_Name_ComboBox.Text) = -1 Then
            MsgBox("無效資料夾")
            Exit Sub
        End If

        Dim myfilePath = ""
        Dim myFileName = ""
        For Each itemSelected In Profile_CheckedListBox.SelectedItems
            'Debug.WriteLine(itemSelected)
            myfilePath = FormInit.profile_path + itemSelected
            myFileName = itemSelected.Split("\")(1)

            If itemSelected.Split("\")(0) = Profile_Target_Dir_Name_ComboBox.Text Then
                MsgBox("目的資料夾與來源相同")
                Exit Sub
            End If

        Next

        If myfilePath = "" Then
            MsgBox("未選擇Proflie")
            Exit Sub
        End If

        Try
            Debug.WriteLine("From: " + myfilePath)
            Debug.WriteLine("To : " + targetDir)
            Debug.WriteLine(myFileName)
            My.Computer.FileSystem.MoveDirectory(myfilePath, targetDir + "\" + myFileName)
            FormInit.Render_profile_CheckedListBox()
        Catch ex As Exception
            Debug.WriteLine(ex)
            MsgBox("移動失敗，相同檔名或者其他錯誤")
        End Try



    End Sub

    Private Sub Filter_Available_Profile_CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles Filter_Available_Profile_CheckBox.CheckedChanged
        FormInit.Render_profile_CheckedListBox()
    End Sub

    Private Sub Filter_Ban_Profile_CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles Filter_Ban_Profile_CheckBox.CheckedChanged
        FormInit.Render_profile_CheckedListBox()
    End Sub

    Private Sub Filter_Useless_Profile_CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles Filter_Useless_Profile_CheckBox.CheckedChanged
        FormInit.Render_profile_CheckedListBox()
    End Sub

    Private Sub SelectAll_Folder_with_TextFile_textbox_btn_Click(sender As Object, e As EventArgs) Handles SelectAll_Folder_with_TextFile_textbox_btn.Click
        If TextFileFolder_TextBox.Text <> "" Then
            FormComponentController.Set_Folder_Checked_with_TextFile_textbox(TextFileFolder_TextBox.Text)
        Else
            FormComponentController.Set_TextFile_Item_Checked(True)
        End If

    End Sub

    Private Sub DeselectAll_Folder_with_TextFile_textbox_btn_Click(sender As Object, e As EventArgs) Handles DeselectAll_Folder_with_TextFile_textbox_btn.Click
        FormComponentController.Set_TextFile_Item_Checked(False)
    End Sub

    Private Sub Select_All_Folder_with_Image_textbox_btn_Click(sender As Object, e As EventArgs) Handles Select_All_Folder_with_Image_textbox_btn.Click

        If ImageFolder_TextBox.Text <> "" Then
            FormComponentController.Set_Folder_Checked_with_ImageFile_textbox(ImageFolder_TextBox.Text)
        Else
            FormComponentController.Set_ImageFile_Item_Checked(True)
        End If


    End Sub

    Private Sub Deselect_All_Image_Folder_btn_Click(sender As Object, e As EventArgs) Handles Deselect_All_Image_Folder_btn.Click
        FormComponentController.Set_ImageFile_Item_Checked(False)
    End Sub


    Private Sub script_ListView_Right_Click(sender As Object, ByVal e As MouseEventArgs) Handles script_ListView.MouseClick
        'Debug.WriteLine("Click")
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Set_Item_Index_To_NummericUpDown()
        End If
    End Sub

    Private Sub Save_Group_List_In_Profile_Button_Click(sender As Object, e As EventArgs) Handles Save_Group_List_In_Profile_Button.Click
        FormComponentController.Save_Groups_List_In_Profile(True)
    End Sub

    Private Sub Insert_GroupList_RandomGroup_Navigate_ToURL_btn_Click(sender As Object, e As EventArgs) Handles Insert_GroupList_RandomGroup_Navigate_ToURL_btn.Click
        Dim Profile = ""

        For Each itemSeleted In Profile_CheckedListBox.SelectedItems
            'Debug.WriteLine(itemSeleted)
            Profile = itemSeleted
        Next

        If Profile = "" Then
            MsgBox("未選擇任何Profile")
            Exit Sub
        End If
        Insert_to_script("前往社團", Profile + ";全部隨機")
    End Sub

    Private Sub Reveal_GroupListFile_In_FileExplorer_Click(sender As Object, e As EventArgs) Handles Reveal_GroupListFile_In_FileExplorer.Click
        Dim Profile = ""

        For Each itemSeleted In Profile_CheckedListBox.SelectedItems
            'Debug.WriteLine(itemSeleted)
            Profile = itemSeleted
        Next

        If Profile = "" Then
            MsgBox("未選擇任何Profile")
            Exit Sub
        End If

        Dim grouplist_file_path = FormInit.profile_path + Profile + "\GroupList.txt"
        Debug.WriteLine(grouplist_file_path)
        If System.IO.File.Exists(grouplist_file_path) Then
            Process.Start("explorer.exe", grouplist_file_path)
        Else
            MsgBox("社團列表不存在")
        End If


    End Sub

    Private Sub Refresh_GroupList_Btn_Click(sender As Object, e As EventArgs) Handles Refresh_GroupList_Btn.Click
        FormComponentController.Profile_CheckedListBox_SelectedIndexChanged()
    End Sub

    Private Sub Remove_Selected_Group_Btn_Click(sender As Object, e As EventArgs) Handles Remove_Selected_Group_Btn.Click
        For i As Integer = Groups_ListView.SelectedIndices.Count - 1 To 0 Step -1
            Groups_ListView.Items.RemoveAt(Groups_ListView.SelectedIndices(i))
        Next
        Save_Groups_List_In_Profile(False)
    End Sub

    Private Sub Delete_All_Facebook_GroupList_Btn_Click(sender As Object, e As EventArgs) Handles Delete_All_Facebook_GroupList_Btn.Click


        Dim Profile = ""

        For Each itemSeleted In Profile_CheckedListBox.SelectedItems
            'Debug.WriteLine(itemSeleted)
            Profile = itemSeleted
        Next

        If Profile = "" Then
            MsgBox("未選擇任何Profile")
            Exit Sub
        End If

        Dim grouplist_file_path = FormInit.profile_path + Profile + "\GroupList.txt"
        'Debug.WriteLine(grouplist_file_path)
        If System.IO.File.Exists(grouplist_file_path) Then

            Dim result As DialogResult = MessageBox.Show("確定要刪除此社團列表?", "確認訊息", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then
                My.Computer.FileSystem.DeleteFile(grouplist_file_path)
                Groups_ListView.Items.Clear()
            End If

        Else
            MsgBox("社團列表不存在")
        End If

    End Sub

    Private Sub Insert_Available_Queue_To_script_Click(sender As Object, e As EventArgs) Handles Insert_Available_Queue_To_script.Click
        myWebDriver.used_browser = "Chrome"
        Insert_to_script("開啟:資料夾佇列", "依序")
    End Sub

    Private Sub Insert_Available_RndQueue_To_script_Click(sender As Object, e As EventArgs) Handles Insert_Available_RndQueue_To_script.Click
        myWebDriver.used_browser = "Chrome"
        Insert_to_script("開啟:資料夾佇列", "隨機")
    End Sub

    Private Sub Insert_Click_by_Text_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Click_by_Text_Btn.Click
        Insert_to_script("點擊:文字", TextElementType_ComboBox.Text + ";" + TargetClickText_TextBox.Text)
    End Sub

    Private Sub Insert_Auto_GroupList_RandomGroup_Navigate_ToURL_btn_Click(sender As Object, e As EventArgs) Handles Insert_Auto_GroupList_RandomGroup_Navigate_ToURL_btn.Click
        Insert_to_script("前往社團", "不指定;全部隨機")
    End Sub

    Private Sub Generate_Random_Password_String_Btn_Click(sender As Object, e As EventArgs) Handles Generate_Random_Password_String_Btn.Click
        Dim generated_password_str = Generate_Random_Password(Pasword_Length_NumericUpDown.Value)
        Generated_Password_TextBox.Text = generated_password_str
    End Sub

    Private Sub Copy_Generated_Password_Btn_Click(sender As Object, e As EventArgs) Handles Copy_Generated_Password_Btn.Click
        Clipboard.SetText(Generated_Password_TextBox.Text)
    End Sub

    Private Sub Insert_Change_FB_Password_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Change_FB_Password_Btn.Click
        Insert_to_script("更改密碼", "")
    End Sub

    Private Sub Reveal_Profile_Info_Btn_Click(sender As Object, e As EventArgs) Handles Reveal_Profile_Info_Btn.Click
        Dim Profile = ""

        For Each itemSeleted In Profile_CheckedListBox.SelectedItems
            'Debug.WriteLine(itemSeleted)
            Profile = itemSeleted
        Next

        If Profile = "" Then
            MsgBox("未選擇任何Profile")
            Exit Sub
        End If

        Dim profile_info_path = FormInit.profile_path + Profile + "\ProfileInfo.txt"

        If System.IO.File.Exists(profile_info_path) Then
            Process.Start("explorer.exe", profile_info_path)
        Else
            MsgBox("ProfileInfo.txt 不存在")
        End If


    End Sub

    Private Sub Refresh_myScript_ComboBox_Btn_Click(sender As Object, e As EventArgs) Handles Refresh_myScript_ComboBox_Btn.Click
        Script_Config_ComboBox.Items.Clear()
        FormInit.Render_My_Script_ComboBox()
        MsgBox("已更新常用腳本")
    End Sub

    Private Sub Selection_Item_ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Selection_Item_ComboBox.SelectedIndexChanged

        SearchingKeyword_folder_Textbox.Clear()
        Keyword_TextFileName_TextBox.Clear()
        Searching_keyword_Text_Textbox.Clear()
        If Selection_Item_ComboBox.Text = "前往" Then
            FormComponentController.curr_selected_feature = "前往"


            Search_Engine_ComboBox.Visible = False
            Insert_Searching_Keyword_btn.Visible = False
            Insert_Random_Searching_Keyword_btn.Visible = False

            Insert_Random_Navigation_URL_btn.Visible = True

        ElseIf Selection_Item_ComboBox.Text = "搜尋" Then
            FormComponentController.curr_selected_feature = "搜尋"
            Insert_Random_Navigation_URL_btn.Visible = False

            Search_Engine_ComboBox.Visible = True
            Insert_Searching_Keyword_btn.Visible = True
            Insert_Random_Searching_Keyword_btn.Visible = True

        End If
        FormInit.Render_Keyword_TextFIle()
    End Sub

    Private Sub Insert_Random_Navigation_URL_btn_Click(sender As Object, e As EventArgs) Handles Insert_Random_Navigation_URL_btn.Click
        ScriptInsertion.Insert_Random_Navigation_URL()
    End Sub

    Private Sub Insert_Matched_Random_Folder_Txt_Img_btn_Click(sender As Object, e As EventArgs) Handles Insert_Matched_Random_Folder_Txt_Img_btn.Click
        Insert_to_script("複製:隨機資料夾", "文字和圖片")
    End Sub

    Private Sub Insert_Matched_Random_Folder_Txt_AllImg_btn_Click(sender As Object, e As EventArgs) Handles Insert_Matched_Random_Folder_Txt_AllImg_btn.Click
        Insert_to_script("複製:隨機資料夾", "文字和全部圖片")
    End Sub

    Private Sub Insert_Paste_Match_TextAndImg_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Paste_Match_TextAndImg_Btn.Click
        Insert_to_script("貼上:隨機資料夾", "")
    End Sub

    Private Sub Insert_Random_Post_Background_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Random_Post_Background_Btn.Click
        Insert_to_script("插入背景", "隨機")
    End Sub

    Private Sub Set_All_Window_Size_Btn_Click(sender As Object, e As EventArgs) Handles Set_All_Window_Size_Btn.Click
        Set_All_Window_Size(Window_Width_NumericUpDown.Value, Window_Height_NumericUpDown.Value)
    End Sub


    Private Sub Overlap_All_Window_To_Location_Btn_Click(sender As Object, e As EventArgs) Handles Overlap_All_Window_To_Location_Btn.Click
        Overlap_All_Window(Overlap_Location_X_NumericUpDown.Value, Overlap_Location_Y_NumericUpDown.Value)
    End Sub

    Private Sub Perfom_Window_Layout_Btn_Click(sender As Object, e As EventArgs) Handles Perfom_Window_Layout_Btn.Click
        perform_Window_Layout()
    End Sub


    Private Sub Update_Window_Hwnd_Btn_Click(sender As Object, e As EventArgs) Handles Update_Window_Hwnd_Btn.Click
        Update_Window_Hwnd_ListView()
    End Sub

    Private Sub Set_Selected_Hwnd_Btn_Click(sender As Object, e As EventArgs) Handles Set_Selected_Hwnd_Btn.Click

        For i As Integer = Window_Hwnd_ListView.Items.Count - 1 To 0 Step -1
            'Debug.WriteLine(Groups_ListView.SelectedItems.Item(0).SubItems(0).Text)
            Window_Hwnd_ListView.Items(i).SubItems(2).Text = ""
        Next

        For i As Integer = Window_Hwnd_ListView.SelectedIndices.Count - 1 To 0 Step -1
            'Debug.WriteLine(Groups_ListView.SelectedItems.Item(0).SubItems(0).Text)
            WindowControllerModule.MasterHwnd = CInt(Window_Hwnd_ListView.SelectedItems.Item(0).SubItems(0).Text)
            Window_Hwnd_ListView.SelectedItems.Item(0).SubItems(2).Text = "主控端"
        Next
    End Sub

    Private Sub Start_Windows_Action_Sync_btn_Click(sender As Object, e As EventArgs) Handles Start_Windows_Action_Sync_btn.Click
        'Debug.WriteLine("Hwnd : " & WindowControllerModule.MasterHwnd)
        If WindowControllerModule.sync_flag = True Then
            WindowControllerModule.sync_flag = False
            Start_Windows_Action_Sync_btn.Text = "開始同步"
            Start_Windows_Action_Sync_btn.BackColor = Color.White
        ElseIf WindowControllerModule.sync_flag = False Then
            WindowControllerModule.sync_flag = True
            Start_Windows_Action_Sync_btn.Text = "停止同步"
            Start_Windows_Action_Sync_btn.BackColor = Color.OrangeRed
        End If

        'Debug.WriteLine("FLAG:" & WindowControllerModule.sync_flag)
    End Sub

    Private Sub Insert_share_url_btn_Click(sender As Object, e As EventArgs) Handles Insert_share_url_btn.Click
        Insert_to_script("分享網址", curr_url_ComboBox.Text)
    End Sub

    Private Sub Add_Script_To_Queue_Btn_Click(sender As Object, e As EventArgs) Handles Add_Script_To_Queue_Btn.Click

        If Script_File_ListBox.SelectedItems.Count > 0 Then
            For Each selectedItem As Object In Script_File_ListBox.SelectedItems
                Debug.WriteLine(selectedItem.ToString())

                Dim myTime = Script_Start_Time_DateTimePicker.Value

                'time check
                While True
                    If script_time_check(myTime.ToString("HH:mm:ss")) Then
                        Exit While
                    Else
                        myTime = myTime.AddMinutes(1) ' if same time add 1 min
                    End If

                End While

                Dim curr_row = Script_File_Queue_ListView.Items.Count
                Script_File_Queue_ListView.Items.Insert(curr_row, selectedItem.ToString())

                Script_File_Queue_ListView.Items(curr_row).SubItems.Add(myTime.ToString("HH:mm:ss"))

                If LoopRun_SingleScript_Queue_CheckBox.Checked = True Then
                    Script_File_Queue_ListView.Items(curr_row).SubItems.Add("是")
                Else
                    Script_File_Queue_ListView.Items(curr_row).SubItems.Add("否")
                End If

            Next
        End If
    End Sub

    Public Function script_time_check(mytime)
        For i = 0 To Script_File_Queue_ListView.Items.Count - 1

            Dim time_1 As New TimeSpan(Hour(mytime), Minute(mytime), Second(mytime))
            'Debug.WriteLine("######" + mytime)


            Dim scripttime = Script_File_Queue_ListView.Items(i).SubItems(1).Text
            Debug.WriteLine("scripttime" + scripttime)

            Dim my_script_start_time As New TimeSpan(Hour(scripttime), Minute(scripttime), Second(scripttime))

            If Math.Abs(CInt(time_1.TotalSeconds) - CInt(my_script_start_time.TotalSeconds)) < 60 Then
                Debug.WriteLine(Math.Abs(CInt(time_1.TotalSeconds) - CInt(my_script_start_time.TotalSeconds)))
                Return False
            End If

        Next

        Return True

    End Function


    Public Function TimeCheck(myTime)
        Dim TimeNow = Date.Now.ToString("HH:mm:ss")


        Dim curr_time As New TimeSpan(Hour(TimeNow), Minute(TimeNow), Second(TimeNow))
        'Debug.WriteLine("sec:" & CInt(curr_time.TotalSeconds))

        Dim my_script_start_time As New TimeSpan(Hour(myTime), Minute(myTime), Second(myTime))
        'Debug.WriteLine("SCsec:" & CInt(my_script_start_time.TotalSeconds))

        If Math.Abs(CInt(my_script_start_time.TotalSeconds) - CInt(curr_time.TotalSeconds)) < 2 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Async Function Run_script_Queue() As Task(Of Boolean)
        'Restore_ScriptFileQueueListView_BackColor()


        For i = 0 To Script_File_Queue_ListView.Items.Count - 1

            Dim currentScriptTime = Script_File_Queue_ListView.Items(i).SubItems(1).Text
            Dim TimeNow = Date.Now.ToString("HH:mm:ss")

            'Debug.WriteLine("script : " + Script_File_Queue_ListView.Items(i).Text)
            'Debug.WriteLine("time : " + currentScriptTime)


            If TimeCheck(currentScriptTime) Then 'if still running, interrupt current script and start next script


                If script_running = True Then
                    script_running = False
                    Await Delay_msec(2000)
                End If

                Try
                    myWebDriver.chromeDriver.Close()
                Catch ex As Exception
                    Debug.WriteLine(ex)
                End Try


                Restore_ScriptFileQueueListView_BackColor()
                Dim first_item = Script_File_Queue_ListView.Items(i)
                first_item.BackColor = Color.SteelBlue
                first_item.ForeColor = Color.White
                first_item.EnsureVisible()

                Load_Script_File(Script_File_Queue_ListView.Items(i).Text)
                If Script_File_Queue_ListView.Items(i).SubItems(2).Text = "是" Then
                    loop_run = True
                Else
                    loop_run = False
                End If
                Flag_start_script = True
                Await Delay_msec(2000)
            End If

        Next

        Debug.WriteLine("EOF")
        Return True

    End Function


    Private Async Sub Start_Script_Queue_btn_Click(sender As Object, e As EventArgs) Handles Start_Script_Queue_btn.Click
        'Dim script_start_time = Script_File_Queue_ListView.Items(0).SubItems(1).Text
        script_ListView.Items.Clear()
        While True
            Await Run_script_Queue()
            Await Delay_msec(1000)
        End While

    End Sub

    Private Sub Restore_ScriptFileQueueListView_BackColor()
        For Each item As ListViewItem In Script_File_Queue_ListView.Items
            item.BackColor = Color.White
            item.ForeColor = Color.Black
        Next

    End Sub

    Private Sub Script_File_ListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Script_File_ListBox.SelectedIndexChanged
        'Debug.WriteLine(Script_File_ListBox.SelectedItem.ToString)
        Load_Script_File(Script_File_ListBox.SelectedItem.ToString)
    End Sub

    Private Sub Save_script_queue_file_btn_Click(sender As Object, e As EventArgs) Handles Save_script_queue_file_btn.Click
        Save_Script_Queue_File_Content()
        script_queue_ComboBox.Items.Clear()
        FormInit.Render_MyScript_Queue_ComboBox()
    End Sub

    Private Sub Refresh_script_queue_btn_Click(sender As Object, e As EventArgs) Handles Refresh_script_queue_btn.Click
        script_queue_ComboBox.Items.Clear()
        FormInit.Render_MyScript_Queue_ComboBox()

    End Sub

    Private Sub script_queue_ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles script_queue_ComboBox.SelectedIndexChanged
        Load_Script_Queue_File(script_queue_ComboBox.Text)
    End Sub

    Private Sub Insert_Check_Last_script_btn_Click(sender As Object, e As EventArgs) Handles Insert_Check_Last_script_btn.Click
        Insert_to_script("檢查", Ignore_line_count_NumericUpDown1.Value)
    End Sub

    Private Sub Delete_selected_image_Btn_Click(sender As Object, e As EventArgs) Handles Delete_selected_image_Btn.Click
        Dim filePath As String = ""
        For Each itemSeleted In img_CheckedListBox.SelectedItems
            filePath = FormInit.image_folder_path + itemSeleted
            'Debug.WriteLine(filePath)
        Next

        If File.Exists(filePath) Then
            Dim result As DialogResult = MessageBox.Show("確定要刪除圖片嗎?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                File.Delete(filePath)
                Selected_PictureBox.Image = Nothing
                img_CheckedListBox.Items.Clear()
                FormInit.Render_img_listbox()
            End If
        Else
            MsgBox("找不到指定的檔案。")
        End If


    End Sub

    Private Sub Save_Product_Profile_To_File_Button_Click(sender As Object, e As EventArgs) Handles Save_Product_Profile_To_File_Button.Click

        Try
            Dim Product_Profile As New With {
            .ProductName = Pruduct_Name_TextBox.Text,
            .ProductPrice = Product_Price_TextBox.Text,
            .ProductCondition = Product_Condition_ComboBox.SelectedIndex.ToString(),
            .ProductLocated = Product_Located_TextBox.Text,
            .ProdcutDescription = Product_Description_RichTextBox.Text
            }

            MsgBox(Product_Condition_ComboBox.Text)

            Dim jsonStr As String = System.Text.Json.JsonSerializer.Serialize(Product_Profile)
            Dim selectedText = ""
            For Each selectedItem As Object In Product_List_CheckedListBox.SelectedItems
                selectedText = selectedItem.ToString()
                'MessageBox.Show("Selected item: " & selectedText)
            Next

            If selectedText = "" Then
                MsgBox("未選取要儲存的資料夾")
                Exit Sub
            End If


            Dim FilePath As String = FormInit.product_dir_path + selectedText + "\ProductProfile.json"
            WriteAllText(FilePath, jsonStr)
            MsgBox("儲存商品成功")
        Catch ex As Exception
            MsgBox("儲存商品失敗")
        End Try

    End Sub


    Public Class PruductProfileDataType
        Public Property ProductName As String
        Public Property ProductPrice As String
        Public Property ProductCondition As Integer
        Public Property ProductLocated As String
        Public Property ProdcutDescription As String
    End Class

    Private Sub Product_List_CheckedListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Product_List_CheckedListBox.SelectedIndexChanged
        Dim Product_Profile_Path As String = ""
        For Each itemSeleted In Product_List_CheckedListBox.SelectedItems
            Product_Profile_Path = FormInit.product_dir_path + itemSeleted + "\ProductProfile.json"
        Next

        If File.Exists(Product_Profile_Path) Then
            Dim jsonString = File.ReadAllText(Product_Profile_Path)

            Dim jsonData As PruductProfileDataType = System.Text.Json.JsonSerializer.Deserialize(Of PruductProfileDataType)(jsonString)

            Pruduct_Name_TextBox.Text = jsonData.ProductName
            Product_Price_TextBox.Text = jsonData.ProductPrice
            Product_Condition_ComboBox.SelectedIndex = jsonData.ProductCondition
            Product_Located_TextBox.Text = jsonData.ProductLocated
            Product_Description_RichTextBox.Text = jsonData.ProdcutDescription
        Else
            'MsgBox("File not exists")
            Pruduct_Name_TextBox.Clear()
            Product_Price_TextBox.Clear()
            Product_Located_TextBox.Clear()
            Product_Description_RichTextBox.Clear()
        End If


    End Sub

    Private Sub Reveal_Product_Dir_Button_Click(sender As Object, e As EventArgs) Handles Reveal_Product_Dir_Button.Click
        Dim Product_Profile_Path As String = ""
        For Each itemSeleted In Product_List_CheckedListBox.SelectedItems
            Product_Profile_Path = FormInit.product_dir_path + itemSeleted
        Next

        If Product_Profile_Path = "" Then
            MsgBox("未選取資料夾")
        End If

        If System.IO.Directory.Exists(Product_Profile_Path) Then
            Process.Start("explorer.exe", Product_Profile_Path)
        End If

    End Sub

    Private Sub Delete_Selected_Product_Folder_Button_Click(sender As Object, e As EventArgs) Handles Delete_Selected_Product_Folder_Button.Click
        For Each itemSelected In Product_List_CheckedListBox.SelectedItems
            Dim Product_Dir_path = FormInit.product_dir_path + itemSelected
            If Directory.Exists(Product_Dir_path) Then
                Dim result As DialogResult = MessageBox.Show("確定要刪除此資料夾?", "確認訊息", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then
                    Debug.WriteLine("Delete : " + Product_Dir_path)
                    'Delete a Directory
                    Directory.Delete(Product_Dir_path, True)
                    Product_List_CheckedListBox.Items.Clear()
                    Render_Product_List_CheckedListBox()
                    Exit Sub
                End If

            End If
        Next
    End Sub

    Private Sub Check_All_Product_Item_Btn_Click(sender As Object, e As EventArgs) Handles Check_All_Product_Item_Btn.Click
        For i As Integer = 0 To Product_List_CheckedListBox.Items.Count - 1
            Product_List_CheckedListBox.SetItemChecked(i, True)
        Next
    End Sub

    Private Sub UnCheck_All_Product_Item_Btn_Click(sender As Object, e As EventArgs) Handles UnCheck_All_Product_Item_Btn.Click
        For i As Integer = 0 To Product_List_CheckedListBox.Items.Count - 1
            Product_List_CheckedListBox.SetItemChecked(i, False)
        Next
    End Sub

    Private Sub Insert_Sale_Product_To_Script_Btn_Click(sender As Object, e As EventArgs) Handles Insert_m_Upload_Product_Profile_To_Script_Btn.Click


        Dim product_folders As String = ""
        For Each checkedItem As Object In Product_List_CheckedListBox.CheckedItems
            Dim checkedText As String = checkedItem.ToString()
            Debug.WriteLine("Checked item: " & checkedText)
            product_folders += checkedText + "/"
        Next

        If Fb_Sale_Group_Url_ComboBox.Text = "" Then
            MsgBox("未選擇網址")
            Exit Sub
        End If

        If product_folders = "" Then
            MsgBox("未選擇商品")
            Exit Sub
        End If

        Insert_to_script("m拍賣:上載", Fb_Sale_Group_Url_ComboBox.Text + ";" + product_folders)
    End Sub

    Private Sub Insert_Post_Product_btn_Click(sender As Object, e As EventArgs) Handles Insert_m_Post_Product_btn.Click
        Insert_to_script("m拍賣:按發布", "")
    End Sub

    Private Sub Insert_IPhone_Upload_Product_Profile_To_Script_Btn_Click(sender As Object, e As EventArgs) Handles Insert_IPhone_Upload_Product_Profile_To_Script_Btn.Click
        Dim product_folders As String = ""
        For Each checkedItem As Object In Product_List_CheckedListBox.CheckedItems
            Dim checkedText As String = checkedItem.ToString()
            Debug.WriteLine("Checked item: " & checkedText)
            product_folders += checkedText + "/"
        Next

        If Fb_Sale_Group_Url_ComboBox.Text = "" Then
            MsgBox("未選擇網址")
            Exit Sub
        End If

        If product_folders = "" Then
            MsgBox("未選擇商品")
            Exit Sub
        End If

        Insert_to_script("拍賣:IPhone上載", Fb_Sale_Group_Url_ComboBox.Text + ";" + product_folders)
    End Sub

    Private Sub Insert_IPhone_Post_Product_btn_Click(sender As Object, e As EventArgs) Handles Insert_IPhone_Post_Product_btn.Click
        Insert_to_script("拍賣:IPhone按發布", "")
    End Sub

    Private Sub Insert_Messager_Contact_btn_Click(sender As Object, e As EventArgs) Handles Insert_Messager_Contact_btn.Click
        ScriptInsertion.Insert_Messager_Contact()
    End Sub

    Private Sub Insert_Messager_Submit_Content_Btn_Click(sender As Object, e As EventArgs) Handles Insert_Messager_Submit_Content_Btn.Click
        Insert_to_script("聊天:送出", "")
    End Sub


    Private Async Sub Submit_Message_Button_Click(sender As Object, e As EventArgs) Handles Submit_Message_Button.Click
        Dim task_result = Await myWebDriver.Messager_Submit_Content_Task()
        If task_result = False Then
            MsgBox("聊天室送出失敗")
        End If
    End Sub

    Private Async Sub Upload_Message_Button_Click(sender As Object, e As EventArgs) Handles Upload_Message_Button.Click
        If content_RichTextBox.Text = "" Then
            MsgBox("內容不能為空")
        Else
            Dim img_path_str As String = ""

            'get selected img path into string 
            If img_CheckedListBox.CheckedItems.Count <> 0 Then
                For i = 0 To img_CheckedListBox.Items.Count - 1
                    'img_upload_input.SendKeys(img_CheckedListBox.Items(i).ToString)

                    If img_CheckedListBox.GetItemChecked(i) Then
                        'Debug.WriteLine("###################")
                        'Debug.WriteLine(Form1.img_CheckedListBox.Items(i).ToString)
                        If img_path_str = "" Then
                            img_path_str = image_folder_path + img_CheckedListBox.Items(i).ToString
                        Else
                            img_path_str = img_path_str & vbLf & image_folder_path + img_CheckedListBox.Items(i).ToString
                        End If
                    End If

                Next

            End If

            Dim task_result = Await myWebDriver.Messager_Contact_Task(img_path_str, content_RichTextBox.Text)

            If task_result = False Then
                MsgBox("聊天室上傳失敗")
            End If
        End If
    End Sub

    Private Sub Insert_Click_Sale_button_Click(sender As Object, e As EventArgs) Handles Insert_Click_Sale_button.Click
        Insert_to_script("拍賣:按拍賣", "")
    End Sub

    Private Sub Insert_Upload_Product_Profile_Button_Click(sender As Object, e As EventArgs) Handles Insert_Upload_Product_Profile_Button.Click

        Dim product_folders As String = ""
        For Each checkedItem As Object In Product_List_CheckedListBox.CheckedItems
            Dim checkedText As String = checkedItem.ToString()
            Debug.WriteLine("Checked item: " & checkedText)
            product_folders += checkedText + "/"
        Next

        If product_folders = "" Then
            MsgBox("未選擇商品")
            Exit Sub
        End If

        Insert_to_script("拍賣:上載", product_folders)
    End Sub

    Private Sub Insert_Post_Product_Button_Click(sender As Object, e As EventArgs) Handles Insert_Post_Product_Button.Click
        Insert_to_script("拍賣:發布", "")
    End Sub
End Class