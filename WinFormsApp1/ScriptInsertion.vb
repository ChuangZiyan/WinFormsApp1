Imports System.IO
Imports System.Text.RegularExpressions

Module ScriptInsertion

    Public Sub Insert_to_script(action As String, content As String)

        If Form1.EmulatedDevice_ComboBox.SelectedItem IsNot Nothing Then
            Form1.myWebDriver.used_dev_model = Form1.EmulatedDevice_ComboBox.SelectedItem.ToString
        Else
            Form1.myWebDriver.used_dev_model = "PC"
        End If

        Dim myline As String

        If action = "" Then
            myline = content + ",,,,,,"
        Else
            myline = Form1.myWebDriver.used_browser + "," + Form1.myWebDriver.used_dev_model + "," + Form1.used_chrome_profile + "," + action + "," + content + ","
        End If

        Form1.EventlogListview_AddNewItem(myline)
        Form1.script_ListView.Items(Form1.script_ListView.Items.Count - 1).EnsureVisible()
    End Sub


    Public Sub Insert_Login()
        Dim fb_email = Form1.fb_account_TextBox.Text
        Dim fb_passwd = Form1.fb_password_TextBox.Text

        Dim plainText = fb_passwd
        Dim wrapper As New Simple3Des("password")
        Dim cipherText As String = wrapper.EncryptData(plainText)

        If fb_email = "" OrElse fb_passwd = "" Then
            MsgBox("帳號與密碼不可為空")
        Else
            Insert_to_script("登入", "帳號:" + fb_email + " 密碼:" + cipherText)
        End If
    End Sub

    Public Sub Insert_navigate_to_url()
        Dim pattern As String
        pattern = "http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?"
        If Regex.IsMatch(Form1.curr_url_ComboBox.Text, pattern) Then
            Dim content As String
            If Form1.group_name_TextBox.Text <> "" Then
                content = Form1.group_name_TextBox.Text + ";" + Form1.curr_url_ComboBox.Text
            Else
                content = Form1.curr_url_ComboBox.Text
            End If

            Insert_to_script("前往", content)
        Else
            MsgBox("網址格式錯誤")
        End If
    End Sub

    Public Sub Insert_delay()
        'Dim rnd_num As New Random()
        Dim hour = Form1.wait_hour_NumericUpDown.Value
        Dim minute = Form1.wait_minute_NumericUpDown.Value
        Dim second = Form1.wait_second_NumericUpDown.Value

        'Dim random_sec = rnd_num.Next(-wait_random_second_NumericUpDown.Value, wait_random_second_NumericUpDown.Value)

        Dim total_second = hour * 3600 + minute * 60 + second
        If Form1.wait_random_second_NumericUpDown.Value > 0 Then
            Insert_to_script("等待", total_second.ToString() + "±" + Form1.wait_random_second_NumericUpDown.Value.ToString() + "秒")
        ElseIf total_second > 1 Then
            Insert_to_script("等待", total_second.ToString() + "秒")
        Else
            Insert_to_script("等待", "1秒")
        End If
    End Sub

    Public Sub Insert_Exit_Program()
        'Dim rnd_num As New Random()
        Dim hour = Form1.wait_hour_NumericUpDown.Value
        Dim minute = Form1.wait_minute_NumericUpDown.Value
        Dim second = Form1.wait_second_NumericUpDown.Value

        'Dim random_sec = rnd_num.Next(-wait_random_second_NumericUpDown.Value, wait_random_second_NumericUpDown.Value)

        Dim total_second = hour * 3600 + minute * 60 + second
        If Form1.wait_random_second_NumericUpDown.Value > 0 Then
            Insert_to_script("關閉程式", total_second.ToString() + "±" + Form1.wait_random_second_NumericUpDown.Value.ToString() + "秒")
        ElseIf total_second > 1 Then
            Insert_to_script("關閉程式", total_second.ToString() + "秒")
        Else
            Insert_to_script("關閉程式", "1秒")
        End If
    End Sub


    Public Sub Insert_Shutdown_System()
        'Dim rnd_num As New Random()
        Dim hour = Form1.wait_hour_NumericUpDown.Value
        Dim minute = Form1.wait_minute_NumericUpDown.Value
        Dim second = Form1.wait_second_NumericUpDown.Value

        'Dim random_sec = rnd_num.Next(-wait_random_second_NumericUpDown.Value, wait_random_second_NumericUpDown.Value)

        Dim total_second = hour * 3600 + minute * 60 + second
        If Form1.wait_random_second_NumericUpDown.Value > 0 Then
            Insert_to_script("關閉電腦", total_second.ToString() + "±" + Form1.wait_random_second_NumericUpDown.Value.ToString() + "秒")
        ElseIf total_second > 1 Then
            Insert_to_script("關閉電腦", total_second.ToString() + "秒")
        Else
            Insert_to_script("關閉電腦", "1秒")
        End If
    End Sub

    Public Sub Insert_open_browser()
        Dim myprofile As String = ""
        Dim profile_dir = FormInit.profile_path

        If Form1.Profile_TextBox.Text <> "" And Form1.Profile_Name_ComboBox.Text <> "" Then
            myprofile = Form1.Profile_TextBox.Text + "\" + Form1.Profile_Name_ComboBox.Text
        Else
            For Each itemChecked In Form1.Profile_CheckedListBox.CheckedItems
                'Debug.WriteLine(itemChecked)
                myprofile += profile_dir + itemChecked + ";"
            Next
            myprofile = myprofile.Trim(";"c)

        End If

        If Form1.chrome_RadioButton.Checked = True Then
            Form1.myWebDriver.used_browser = "Chrome"
            Dim profile_nickname = ""

            If myprofile.Contains(";"c) Then
                Dim profile_arr As String() = myprofile.Split(";"c)
                For Each nickname In profile_arr
                    profile_nickname += nickname.Split("\")(UBound(nickname.Split("\"))) + " "
                Next
                profile_nickname.Trim(" ")
            Else
                profile_nickname = myprofile.Split("\")(UBound(myprofile.Split("\"))).Trim(";")
            End If

            Form1.used_chrome_profile = profile_nickname

        ElseIf Form1.firefox_RadioButton.Checked = True Then
            Form1.myWebDriver.used_browser = "Firefox"
        ElseIf Form1.edge_RadioButton.Checked = True Then
            Form1.myWebDriver.used_browser = "Edge"
        End If


        If myprofile = "" Then
            Insert_to_script("開啟", "全部隨機")
        Else
            Insert_to_script("開啟", myprofile.TrimEnd(";"))
        End If
    End Sub

    Public Sub Insert_click_img_video()
        Dim img_path_str As String = ""

        'get selected img path into string 
        If Form1.img_CheckedListBox.CheckedItems.Count <> 0 Then
            For i = 0 To Form1.img_CheckedListBox.Items.Count - 1
                'img_upload_input.SendKeys(img_CheckedListBox.Items(i).ToString)

                If Form1.img_CheckedListBox.GetItemChecked(i) Then
                    'Debug.WriteLine("###################")
                    'Debug.WriteLine(Form1.img_CheckedListBox.Items(i).ToString)
                    If img_path_str = "" Then
                        img_path_str = image_folder_path + Form1.img_CheckedListBox.Items(i).ToString
                    Else
                        img_path_str = img_path_str & vbLf & image_folder_path + Form1.img_CheckedListBox.Items(i).ToString
                    End If
                End If

            Next
            Insert_to_script("上載", img_path_str)
        Else
            MsgBox("未勾選任何檔案")
        End If
    End Sub

    Public Sub Insert_comment_upload_img()
        Dim img_path_str As String = ""

        'get selected img path into string 
        If Form1.img_CheckedListBox.CheckedItems.Count = 1 Then

            For i = 0 To Form1.img_CheckedListBox.Items.Count - 1
                'We ask if this item is checked or not
                If Form1.img_CheckedListBox.GetItemChecked(i) Then
                    img_path_str = Form1.img_CheckedListBox.Items(i).ToString()
                End If
            Next

            Insert_to_script("回應:上載", img_path_str)
        ElseIf Form1.img_CheckedListBox.CheckedItems.Count > 1 Then
            MsgBox("回覆最多只能一張圖")
        Else
            MsgBox("未勾選任何檔案")
        End If
    End Sub

    Public Sub Insert_emoji()
        Dim Emoji_list As String = ""

        If Form1.Emoji_like_CheckBox.Checked Then
            Emoji_list += "讚好 "
        End If

        If Form1.Emoji_love_CheckBox.Checked Then
            Emoji_list += "愛心 "
        End If

        If Form1.Emoji_wow_CheckBox.Checked Then
            Emoji_list += "驚訝 "
        End If

        If Form1.Emoji_haha_CheckBox.Checked Then
            Emoji_list += "哈哈 "
        End If

        If Form1.Emoji_sad_CheckBox.Checked Then
            Emoji_list += "難過 "
        End If

        If Form1.Emoji_care_CheckBox.Checked Then
            Emoji_list += "加油 "
        End If

        If Form1.Emoji_angry_CheckBox.Checked Then
            Emoji_list += "生氣 "
        End If

        If Emoji_list = "" Then
            MsgBox("未勾選任何心情")
        Else
            Insert_to_script("回應:按讚", Emoji_list)
        End If
    End Sub


    Public Sub Insert_send_Random_content_TextFile()
        Dim Txt_file_path As String = ""
        For Each itemChecked In Form1.Text_File_CheckedListBox.CheckedItems
            'Debug.WriteLine(itemChecked)
            Txt_file_path += itemChecked + ";"
        Next

        If Txt_file_path = "" Then
            Insert_to_script("發送:隨機", "全部隨機")
        Else
            Insert_to_script("發送:隨機", Txt_file_path.TrimEnd(";"))
        End If
    End Sub

    Public Sub Insert_Upload_Random_Image()
        Dim Image_file_path As String = ""
        For Each itemChecked In Form1.img_CheckedListBox.CheckedItems
            'Debug.WriteLine(itemChecked)
            Image_file_path += itemChecked + ";"
        Next

        If Image_file_path = "" Then
            Insert_to_script("上載:隨機", "全部隨機")
        Else
            Insert_to_script("上載:隨機", Image_file_path.TrimEnd(";"))
        End If
    End Sub


    Public Sub Insert_Reply_Random_TxtFile()
        Dim Txt_file_path As String = ""
        For Each itemChecked In Form1.Text_File_CheckedListBox.CheckedItems
            'Debug.WriteLine(itemChecked)
            Txt_file_path += itemChecked + ";"
        Next

        If Txt_file_path = "" Then
            Insert_to_script("回應:隨機", "全部隨機")
        Else
            Insert_to_script("回應:隨機", Txt_file_path.TrimEnd(";"))
        End If
    End Sub

    Public Sub Insert_Reply_Random_Image()
        Dim Image_file_path As String = ""
        For Each itemChecked In Form1.img_CheckedListBox.CheckedItems
            'Debug.WriteLine(itemChecked)
            Image_file_path += itemChecked + ";"
        Next
        If Image_file_path = "" Then
            Insert_to_script("回應:上載隨機", "全部隨機")
        Else
            Insert_to_script("回應:上載隨機", Image_file_path.TrimEnd(";"))
        End If
    End Sub

    Public Sub Insert_Reply_Random_Match()
        Dim Content As String = ""
        For Each item As ListViewItem In Form1.Match_Condition_ListView.Items
            'MsgBox(item.SubItems(0).Text & vbCrLf & item.SubItems(1).Text)
            Content += item.SubItems(0).Text + "%20" + item.SubItems(1).Text + ";"
        Next
        'MsgBox(Content)
        If Content = "" Then
            MsgBox("無任何配對條件")
        Else
            Insert_to_script("回應:隨機配對", Content.TrimEnd(";"c))
        End If
    End Sub

    Public Sub Insert_random_matching_text_and_img()
        Dim Content As String = ""
        For Each item As ListViewItem In Form1.Match_Condition_ListView.Items
            'MsgBox(item.SubItems(0).Text & vbCrLf & item.SubItems(1).Text)
            Content += text_folder_path + item.SubItems(0).Text + "%20" + image_folder_path + item.SubItems(1).Text + ";"
        Next
        'MsgBox(Content)
        If Content = "" Then
            MsgBox("無任何配對條件")
        Else
            Insert_to_script("發送上載:隨機配對", Content.TrimEnd(";"c))
        End If
    End Sub

    Public Sub Insert_Queue_To_script()
        Dim Content As String = ""
        Dim profile_dir = FormInit.profile_path
        For Each item In Form1.Profile_Queue_ListBox.Items
            'MsgBox(item.SubItems(0).Text & vbCrLf & item.SubItems(1).Text)
            Content += profile_dir + item + ";"
        Next

        If Content = "" Then
            MsgBox("佇列為空")
        Else
            Form1.myWebDriver.used_browser = "Chrome"
            Insert_to_script("開啟:佇列", Content.Trim(";"c))
        End If

    End Sub

    Public Sub Insert_random_matching_text_and_all_img()
        Dim Content As String = ""
        For Each item As ListViewItem In Form1.Match_Condition_ListView.Items
            'MsgBox(item.SubItems(0).Text & vbCrLf & item.SubItems(1).Text)
            Content += text_folder_path + item.SubItems(0).Text + "%20" + image_folder_path + item.SubItems(1).Text + ";"
        Next
        'MsgBox(Content)
        If Content = "" Then
            MsgBox("無任何配對條件")
        Else
            Insert_to_script("發送上載:隨機配對多圖", Content.TrimEnd(";"c))
        End If
    End Sub

    Public Sub Insert_Messager_Contact()


        Dim Txt_file_path As String = ""
        For Each itemChecked In Form1.Text_File_CheckedListBox.CheckedItems
            'Debug.WriteLine(itemChecked)
            Txt_file_path += itemChecked + ";"
        Next


        Dim img_path_str As String = ""

        'get selected img path into string 
        If Form1.img_CheckedListBox.CheckedItems.Count <> 0 Then
            For i = 0 To Form1.img_CheckedListBox.Items.Count - 1
                'img_upload_input.SendKeys(img_CheckedListBox.Items(i).ToString)

                If Form1.img_CheckedListBox.GetItemChecked(i) Then
                    'Debug.WriteLine("###################")
                    'Debug.WriteLine(Form1.img_CheckedListBox.Items(i).ToString)
                    If img_path_str = "" Then
                        img_path_str = image_folder_path + Form1.img_CheckedListBox.Items(i).ToString
                    Else
                        img_path_str = img_path_str & vbLf & image_folder_path + Form1.img_CheckedListBox.Items(i).ToString
                    End If
                End If

            Next

        End If

        Insert_to_script("聊天", img_path_str + "%20" + Txt_file_path.Trim(";"c))
    End Sub


    Public Sub Insert_Messager_Contact_Listbox_Ids()


        Dim Txt_file_path As String = ""
        For Each itemChecked In Form1.Text_File_CheckedListBox.CheckedItems
            'Debug.WriteLine(itemChecked)
            Txt_file_path += itemChecked + ";"
        Next


        Dim img_path_str As String = ""

        'get selected img path into string 
        If Form1.img_CheckedListBox.CheckedItems.Count <> 0 Then
            For i = 0 To Form1.img_CheckedListBox.Items.Count - 1
                'img_upload_input.SendKeys(img_CheckedListBox.Items(i).ToString)

                If Form1.img_CheckedListBox.GetItemChecked(i) Then
                    'Debug.WriteLine("###################")
                    'Debug.WriteLine(Form1.img_CheckedListBox.Items(i).ToString)
                    If img_path_str = "" Then
                        img_path_str = image_folder_path + Form1.img_CheckedListBox.Items(i).ToString
                    Else
                        img_path_str = img_path_str & vbLf & image_folder_path + Form1.img_CheckedListBox.Items(i).ToString
                    End If
                End If

            Next

        End If

        Insert_to_script("聊天:列表", img_path_str + "%20" + Txt_file_path.Trim(";"c) & "%20" & Form1.Send_Message_To_All_Id_delay_sec_NumericUpDown.Value)
    End Sub



    Public Sub Insert_navigate_to_url_in_GroupList()

        Dim pattern As String
        pattern = "http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?"
        If Regex.IsMatch(Form1.GroupList_GroupURL_Textbox.Text, pattern) Then
            Dim content As String
            If Form1.GroupList_GroupName_Textbox.Text <> "" Then
                content = Form1.GroupList_GroupName_Textbox.Text + ";" + Form1.GroupList_GroupURL_Textbox.Text
            Else
                content = Form1.curr_url_ComboBox.Text
            End If

            Insert_to_script("前往", content)
        Else
            MsgBox("網址格式錯誤")
        End If

    End Sub

    Public Sub Insert_Random_Searching_Keyword()
        If Form1.Search_Engine_ComboBox.Text = "" Then
            MsgBox("搜尋引擎為空")
            Exit Sub
        End If

        Dim Txt_file_path As String = ""
        For Each itemChecked In Form1.Searching_Keyword_CheckedListBox.CheckedItems
            'Debug.WriteLine(itemChecked)
            Txt_file_path += itemChecked + ";"
        Next

        If Txt_file_path = "" Then
            Insert_to_script("搜尋:隨機", Form1.Search_Engine_ComboBox.Text + "%20全部隨機")
        Else
            Insert_to_script("搜尋:隨機", Form1.Search_Engine_ComboBox.Text + "%20" + Txt_file_path.TrimEnd(";"))
        End If
    End Sub

    Public Sub Insert_Random_Navigation_URL()
        Dim Txt_file_path As String = ""
        For Each itemChecked In Form1.Searching_Keyword_CheckedListBox.CheckedItems
            'Debug.WriteLine(itemChecked)
            Txt_file_path += itemChecked + ";"
        Next

        If Txt_file_path = "" Then
            Insert_to_script("前往:隨機", "全部隨機")
        Else
            Insert_to_script("前往:隨機", Txt_file_path.TrimEnd(";"))
        End If
    End Sub

    Public Sub Insert_story_img_video()
        Dim img_path_str As String = ""

        'get selected img path into string 
        If Form1.img_CheckedListBox.CheckedItems.Count <> 0 Then
            For i = 0 To Form1.img_CheckedListBox.CheckedItems.Count - 1
                'img_upload_input.SendKeys(img_CheckedListBox.Items(i).ToString)
                Debug.WriteLine(Form1.img_CheckedListBox.Items(i).ToString)
                If img_path_str = "" Then
                    img_path_str = image_folder_path + Form1.img_CheckedListBox.Items(i).ToString
                Else
                    img_path_str = img_path_str & vbLf & image_folder_path + Form1.img_CheckedListBox.Items(i).ToString
                End If
            Next
            Insert_to_script("限時:上載", img_path_str)
        Else
            MsgBox("未勾選任何檔案")
        End If
    End Sub


    Public Sub Insert_Story_Random_Image()
        Dim Image_file_path As String = ""
        For Each itemChecked In Form1.img_CheckedListBox.CheckedItems
            'Debug.WriteLine(itemChecked)
            Image_file_path += itemChecked + ";"
        Next

        If Image_file_path = "" Then
            Insert_to_script("限時:隨機上載", "全部隨機")
        Else
            Insert_to_script("限時:隨機上載", Image_file_path.TrimEnd(";"))
        End If
    End Sub



    Public Sub Insert_Copy_TextFileContent()
        Dim myFilePath = Form1.Target_TextFile_Path_For_Copy_TextBox.Text
        'Dim allowed_extension = New String() {".txt", ".jpg", ".jpeg", ".png"}

        Dim myFile_extenion = Path.GetExtension(myFilePath)

        If myFile_extenion = ".txt" Then
            Insert_to_script("複製:檔案內容", myFilePath)
        Else
            MsgBox("無效的文字檔案或者路徑")
        End If

    End Sub

    Public Sub Insert_Copy_ImageFile()
        Dim myFilePaths = Form1.Target_ImageFile_Path_For_Copy_TextBox.Text
        Dim allowed_extension = New String() {".jpg", ".jpeg", ".png"}

        If myFilePaths = "" Then
            MsgBox("參數中中存在無效的圖片檔案或者無效路徑")
            Exit Sub
        End If


        For Each filePath In myFilePaths.Split(";")
            Dim myFile_extenion = Path.GetExtension(filePath)
            If Not allowed_extension.Contains(myFile_extenion) Then
                MsgBox("參數中中存在無效的圖片檔案或者無效路徑")
                Exit Sub
            End If
        Next

        Insert_to_script("複製:圖片位置", myFilePaths)
    End Sub




End Module
