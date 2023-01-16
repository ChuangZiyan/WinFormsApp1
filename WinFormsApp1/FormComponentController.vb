Imports System.Buffers
Imports System.IO
Imports System.IO.File
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports WinFormsApp1.JsonProfileInfo
Module FormComponentController

    Public curr_selected_feature = "搜尋"


    Public Sub Delete_ScriptListView_selected_item()
        For i As Integer = Form1.script_ListView.SelectedIndices.Count - 1 To 0 Step -1
            Form1.script_ListView.Items.RemoveAt(Form1.script_ListView.SelectedIndices(i))
        Next
        Rearrange_scriptlistview_number()
    End Sub

    Public Sub Move_up_ScriptListView_selected_item()
        If Form1.script_ListView.SelectedIndices.Count > 0 Then
            For i = 0 To Form1.script_ListView.SelectedIndices.Count - 1
                Dim index = Form1.script_ListView.SelectedIndices(i)
                If index > 0 Then
                    If Form1.script_ListView.SelectedIndices.Contains(index - 1) Then
                        Continue For
                    End If
                    Dim temp As ListViewItem = Form1.script_ListView.Items(index)
                    Form1.script_ListView.Items.RemoveAt(index)
                    Form1.script_ListView.Items.Insert(index - 1, temp)
                    Form1.script_ListView.Items(index - 1).Focused = True
                End If
            Next
            Rearrange_scriptlistview_number()
        End If

    End Sub

    Public Sub MoveDown_ScriptListView_selected_item()
        If Form1.script_ListView.SelectedIndices.Count > 0 Then
            For i = Form1.script_ListView.SelectedIndices.Count - 1 To 0 Step -1
                Dim index = Form1.script_ListView.SelectedIndices(i)
                If index < Form1.script_ListView.Items.Count - 1 Then
                    If Form1.script_ListView.SelectedIndices.Contains(index + 1) Then
                        Continue For
                    End If
                    Dim temp As ListViewItem = Form1.script_ListView.Items(index)
                    Form1.script_ListView.Items.RemoveAt(index)
                    Form1.script_ListView.Items.Insert(index + 1, temp)
                    Form1.script_ListView.Items(index + 1).Focused = True
                End If
            Next
            Rearrange_scriptlistview_number()
        End If

    End Sub

    Public Sub Move_Script_ListView_Item_To_Index(target_idx)

        If Form1.script_ListView.SelectedIndices.Count > 0 Then
            For i = 0 To Form1.script_ListView.SelectedIndices.Count - 1
                Dim index = Form1.script_ListView.SelectedIndices(i)
                If index >= 0 Then
                    'If Form1.script_ListView.SelectedIndices.Contains(target_idx) Then
                    'Continue For
                    'End If

                    If target_idx > Form1.script_ListView.Items.Count Then
                        target_idx = Form1.script_ListView.Items.Count
                        Debug.WriteLine(Form1.script_ListView.Items.Count)
                    End If

                    Dim temp As ListViewItem = Form1.script_ListView.Items(index)
                    Form1.script_ListView.Items.RemoveAt(index)
                    Form1.script_ListView.Items.Insert(target_idx - 1, temp)
                    'Debug.WriteLine("idx : " & index)
                    'Form1.script_ListView.Items(target_idx - 1).Focused = True
                End If
            Next
            Rearrange_scriptlistview_number()
        End If
    End Sub


    Public Sub Set_Item_Index_To_NummericUpDown()
        If Form1.script_ListView.SelectedIndices.Count > 0 Then
            For i = 0 To Form1.script_ListView.SelectedIndices.Count - 1
                Dim index = Form1.script_ListView.SelectedIndices(i)
                If index >= 0 Then
                    'Debug.WriteLine(Form1.script_ListView.Items.Item(index).Text)
                    Form1.Target_Index_Script_ListView_NummericUpDown.Value = CInt(Form1.script_ListView.Items.Item(index).Text)
                End If
            Next

        End If
    End Sub


    Public Sub Rearrange_scriptlistview_number()
        Dim index = 1
        For Each item As ListViewItem In Form1.script_ListView.Items
            item.SubItems.Item(0).Text = index.ToString()
            index += 1
        Next

    End Sub

    Public Sub Save_Script_Content()
        Dim script_txt = ""
        Dim cmd_arrlist As ArrayList = New ArrayList()
        For Each item As ListViewItem In Form1.script_ListView.Items

            Dim tmp_str = ""
            For i = 1 To item.SubItems.Count - 1
                cmd_arrlist.Add(item.SubItems.Item(i).Text)
            Next
            cmd_arrlist(5) = ""
            tmp_str = String.Join(",", cmd_arrlist.ToArray())
            cmd_arrlist.Clear()
            script_txt += tmp_str & vbCrLf

        Next

        Form1.SaveFileDialog1.Filter = "txt files (*.txt)|"
        Form1.SaveFileDialog1.DefaultExt = "txt"
        Form1.SaveFileDialog1.FilterIndex = 2
        Form1.SaveFileDialog1.RestoreDirectory = True

        If Form1.SaveFileDialog1.ShowDialog = DialogResult.OK Then
            'Debug.WriteLine(script_txt)
            WriteAllText(Form1.SaveFileDialog1.FileName, script_txt)
        End If
    End Sub

    Public Sub Save_RichBox_Content()
        Form1.SaveFileDialog1.Filter = "txt files (*.txt)|"
        Form1.SaveFileDialog1.DefaultExt = "txt"
        Form1.SaveFileDialog1.FilterIndex = 2
        Form1.SaveFileDialog1.RestoreDirectory = True

        If Form1.SaveFileDialog1.ShowDialog = DialogResult.OK Then
            'Debug.WriteLine(script_txt)
            WriteAllText(Form1.SaveFileDialog1.FileName, Form1.content_RichTextBox.Text)
        End If
    End Sub

    Public Sub Reveal_Selected_In_File_Explorer()
        Dim Current_selected_Item As String = ""
        Dim Selected_Counter As Integer = 0

        'Get TextFile CheckListBox Selected Item

        If Form1.TextFolder_ListBox.SelectedIndex >= 0 Then
            Current_selected_Item = text_folder_path + Form1.TextFolder_ListBox.SelectedItem.ToString()
            Selected_Counter += 1
        End If

        If Form1.ImageFolder_ListBox.SelectedIndex >= 0 Then
            Current_selected_Item = image_folder_path + Form1.ImageFolder_ListBox.SelectedItem.ToString()
            Selected_Counter += 1
        End If



        If Form1.Match_Condition_ListView.SelectedItems.Count > 0 Then
            Current_selected_Item = text_folder_path + Form1.Match_Condition_ListView.SelectedItems(0).Text + "%20" + image_folder_path + Form1.Match_Condition_ListView.SelectedItems(0).SubItems(1).Text
            Selected_Counter += 1
        End If

        'Debug.WriteLine("Seleted Counter : " & Selected_Counter)

        If Selected_Counter = 1 Then
            Debug.WriteLine("Seleted : " + Current_selected_Item)
            If Current_selected_Item.Contains("%20") Then
                Dim paths() = Current_selected_Item.Split("%20")
                'Debug.WriteLine(paths(0))
                'Debug.WriteLine(paths(1))
                Process.Start("explorer.exe", paths(0))
                Process.Start("explorer.exe", paths(1))
            Else
                If Path.GetExtension(Current_selected_Item) <> "" Then
                    Dim fi As New IO.FileInfo(Current_selected_Item)
                    Current_selected_Item = fi.DirectoryName
                End If
                Process.Start("explorer.exe", Current_selected_Item)
            End If


        ElseIf Selected_Counter = 0 Then
            MsgBox("未選取選取任何目錄")
        Else
            MsgBox("不能選擇多於一個")
            Deselect_All_Item()
        End If

    End Sub

    Private Sub Deselect_All_Item()

        If Form1.TextFolder_ListBox.SelectedIndex >= 0 Then
            Form1.TextFolder_ListBox.SetSelected(Form1.TextFolder_ListBox.SelectedIndex, False)
        End If

        If Form1.ImageFolder_ListBox.SelectedIndex >= 0 Then
            Form1.ImageFolder_ListBox.SetSelected(Form1.ImageFolder_ListBox.SelectedIndex, False)
        End If

        If Form1.Match_Condition_ListView.Items.Count > 0 Then
            Form1.Match_Condition_ListView.SelectedItems.Clear()

        End If


    End Sub

    Public Sub Save_Account_Info()
        Dim Profile_Path = ""

        For Each itemSeleted In Form1.Profile_CheckedListBox.SelectedItems
            Debug.WriteLine(itemSeleted)
            Profile_Path = FormInit.profile_path + itemSeleted
        Next

        Debug.WriteLine(Profile_Path)
        If Profile_Path = "" Then
            MsgBox("未選擇任何Profile")
            Exit Sub
        End If

        Dim jsonObject = New JsonProfileInfo()
        jsonObject.Account = Form1.fb_account_TextBox.Text
        jsonObject.Password = Form1.fb_password_TextBox.Text
        jsonObject.LanguagePack = Form1.Lang_Packs_ComboBox.Text
        jsonObject.Remark = Form1.Remark_TextBox.Text

        Dim jsonString = JsonConvert.SerializeObject(jsonObject)

        'Debug.WriteLine(jsonString)
        Try

            Dim myfile As System.IO.StreamWriter
            myfile = My.Computer.FileSystem.OpenTextFileWriter(Profile_Path + "\ProfileInfo.txt", False) 'True : append   'False : overwrite
            myfile.WriteLine(jsonString)
            myfile.Close()

        Catch ex As Exception
            Debug.WriteLine(ex)
            MsgBox("儲存失敗，路徑錯誤或者其他錯誤")
        End Try

        MsgBox("已儲存到 : " + Profile_Path + "\ProfileInfo.txt")

    End Sub


    Public Sub Save_Groups_List_In_Profile(messagebox)
        Dim Profile_Path = ""

        For Each itemSeleted In Form1.Profile_CheckedListBox.SelectedItems
            'Debug.WriteLine(itemSeleted)
            Profile_Path = FormInit.profile_path + itemSeleted
        Next

        'Debug.WriteLine(Profile_Path)
        If Profile_Path = "" Then
            MsgBox("未選擇任何Profile")
            Exit Sub
        End If

        'Debug.WriteLine(jsonString)
        Try

            Dim myfile As System.IO.StreamWriter
            myfile = My.Computer.FileSystem.OpenTextFileWriter(Profile_Path + "\GroupList.txt", False) 'True : append   'False : overwrite

            For Each groupItem As ListViewItem In Form1.Groups_ListView.Items

                Dim LVRow As String = ""
                For Each LVCell As ListViewItem.ListViewSubItem In groupItem.SubItems

                    LVRow &= LVCell.Text & "&nbsp"
                Next
                myfile.WriteLine(LVRow)
            Next

            myfile.Close()

        Catch ex As Exception
            Debug.WriteLine(ex)
            MsgBox("儲存失敗，路徑錯誤或者其他錯誤")
        End Try

        If messagebox Then
            MsgBox("已儲存到 : " + Profile_Path + "\GroupList.txt")
        End If


    End Sub

    Public Sub Profile_CheckedListBox_SelectedIndexChanged()

        Dim myprofile = ""
        For Each itemSelected In Form1.Profile_CheckedListBox.SelectedItems
            'Debug.WriteLine(itemSelected)
            myprofile = "profiles\" + itemSelected
            Form1.Profile_TextBox.Text = FormInit.profile_path + itemSelected.Split("\")(0)
            Form1.Profile_Name_ComboBox.Text = itemSelected.Split("\")(1)
        Next

        'Debug.WriteLine("myfile " + myprofile)
        If My.Computer.FileSystem.FileExists(myprofile + "\ProfileInfo.txt") Then
            Dim JsonString As String = System.IO.File.ReadAllText(myprofile + "\ProfileInfo.txt")
            Dim Profile_JsonObject As Newtonsoft.Json.Linq.JObject
            Profile_JsonObject = JsonConvert.DeserializeObject(JsonString)

            Form1.fb_account_TextBox.Text = Profile_JsonObject.Item("Account")
            Form1.fb_password_TextBox.Text = Profile_JsonObject.Item("Password")
            Form1.Lang_Packs_ComboBox.SelectedIndex = Form1.Lang_Packs_ComboBox.FindStringExact(Profile_JsonObject.Item("LanguagePack"))
            Form1.Remark_TextBox.Text = Profile_JsonObject.Item("Remark")
        Else
            Form1.fb_account_TextBox.Text = ""
            Form1.fb_password_TextBox.Text = ""
            Form1.Lang_Packs_ComboBox.SelectedIndex = -1
            Form1.Remark_TextBox.Text = ""
        End If


        'Render Facebook Group List
        Form1.Groups_ListView.Items.Clear()
        'Debug.WriteLine(myprofile + "\GroupList.csv")
        If My.Computer.FileSystem.FileExists(myprofile + "\GroupList.txt") Then
            Dim reader As StreamReader = My.Computer.FileSystem.OpenTextFileReader(myprofile + "\GroupList.txt")
            Dim line As String
            Dim i = 0
            Do
                line = reader.ReadLine
                If line = "" Then
                    Continue Do
                End If
                'Debug.WriteLine(line)
                Dim myItem = line.Split("&nbsp")
                Form1.Groups_ListView.Items.Add(myItem(0), 100)
                Form1.Groups_ListView.Items(i).SubItems.Add(myItem(1))
                i += 1
            Loop Until line Is Nothing

            reader.Close()

        End If

    End Sub

    Public Sub Text_File_CheckedListBox_Click()
        Dim Txt_file_path As String = ""
        For Each itemSeleted In Form1.Text_File_CheckedListBox.SelectedItems
            Txt_file_path = itemSeleted
        Next
        Form1.content_RichTextBox.Text = File.ReadAllText(curr_path + "resources\texts\" + Txt_file_path)

        Dim temp_arr = Txt_file_path.Split("\")
        Form1.TextFileFolder_TextBox.Text = temp_arr(temp_arr.Length - 2)

    End Sub

    Public Sub Img_CheckedListBox_Click()
        Dim Img_file_path As String = ""
        Dim allowed_video_extentions As String() = {".mp4", ".mk4"}
        For Each itemSeleted In Form1.img_CheckedListBox.SelectedItems
            'Debug.WriteLine(itemSeleted)
            Img_file_path = itemSeleted
        Next

        Dim temp_arr = Img_file_path.Split("\")
        Form1.ImageFolder_TextBox.Text = temp_arr(temp_arr.Length - 2)

        If allowed_video_extentions.Contains(Path.GetExtension(image_folder_path + Img_file_path)) Then ' if video
            'Debug.WriteLine("it's video")
            Form1.Selected_PictureBox.Cursor = Cursors.Hand
            Form1.Selected_PictureBox.Image = Image.FromFile(My.Computer.FileSystem.CurrentDirectory + "\images\PlayVideo.jpg")
        Else
            Form1.Selected_PictureBox.Cursor = Cursors.Default
            Form1.Selected_PictureBox.Image = Image.FromFile(image_folder_path + Img_file_path) ' image 
        End If

    End Sub

    Public Sub Selected_PictureBox_Click()
        Dim allowed_video_extentions As String() = {".mp4", ".mk4"}
        For Each itemSeleted In Form1.img_CheckedListBox.SelectedItems
            If allowed_video_extentions.Contains(Path.GetExtension(itemSeleted)) Then ' if video
                Dim result As DialogResult = MessageBox.Show("是否播放影片?", "確認訊息", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then
                    Process.Start("explorer.exe", image_folder_path + itemSeleted)
                End If
            End If
        Next
    End Sub

    Public Sub Insert_To_Queuing()

        For Each itemChecked In Form1.Profile_CheckedListBox.CheckedItems
            'Debug.WriteLine(itemChecked)
            Form1.Profile_Queue_ListBox.Items.Add(itemChecked)
        Next

    End Sub

    Public Sub Set_Selected_All_Profile(checked As Boolean)
        For i As Integer = 0 To Form1.Profile_CheckedListBox.Items.Count - 1
            Form1.Profile_CheckedListBox.SetItemChecked(i, checked)
        Next
    End Sub

    Public Sub Delete_Profile_From_Queue()
        Form1.Profile_Queue_ListBox.Items.Remove(Form1.Profile_Queue_ListBox.SelectedItem)
    End Sub

    Public Sub Refresh_All_ListBox()
        Form1.img_CheckedListBox.Items.Clear()
        Form1.TextFolder_ListBox.Items.Clear()
        Form1.ImageFolder_ListBox.Items.Clear()
        Form1.Text_File_CheckedListBox.Items.Clear()


        FormInit.Render_img_listbox()
        FormInit.Render_TextFolder_listbox()
        FormInit.Render_ImageFolder_listbox()
        FormInit.Render_TextFile_listbox()

    End Sub

    Public Sub Open_Folder_with_TextFile_textbox()
        If Form1.TextFileFolder_TextBox.Text = "" Then
            MsgBox("資料夾名稱不可為空白")
            Exit Sub
        End If

        Dim mypath As String = FormInit.curr_path + "resources\texts\" + Form1.TextFileFolder_TextBox.Text
        If Not System.IO.Directory.Exists(mypath) Then
            System.IO.Directory.CreateDirectory(mypath)
        End If
        Process.Start("explorer.exe", mypath)
    End Sub

    Public Sub Open_Folder_with_Image_textbox()
        If Form1.ImageFolder_TextBox.Text = "" Then
            MsgBox("資料夾名稱不可為空白")
            Exit Sub
        End If

        Dim mypath As String = FormInit.curr_path + "resources\images\" + Form1.ImageFolder_TextBox.Text
        If Not System.IO.Directory.Exists(mypath) Then
            System.IO.Directory.CreateDirectory(mypath)
        End If
        Process.Start("explorer.exe", mypath)
    End Sub

    Public Sub Delete_Selected_Profile_Folder()

        For Each itemSelected In Form1.Profile_CheckedListBox.SelectedItems
            Dim profile_path = FormInit.profile_path + itemSelected
            If Directory.Exists(profile_path) Then
                Dim result As DialogResult = MessageBox.Show("確定要刪除此資料夾?", "確認訊息", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then
                    Debug.WriteLine("Delete : " + profile_path)
                    'Delete a Directory
                    Directory.Delete(profile_path, True)
                    Form1.Profile_CheckedListBox.Items.Clear()
                    Render_profile_CheckedListBox()
                    Exit Sub
                End If

            End If
        Next
    End Sub

    Public Sub Script_Config_ComboBox_SelectedIndexChanged()

        Try
            Form1.script_ListView.Items.Clear()
            Dim log_lines = IO.File.ReadAllLines(save_script_folder_path + Form1.Script_Config_ComboBox.Text)
            Dim curr_row As Integer = 0
            Dim index = 1
            For Each line In log_lines
                Dim splittedLine() As String = line.Split(",")
                Form1.script_ListView.Items.Add(index.ToString)
                For Each cmd In splittedLine
                    Form1.script_ListView.Items(curr_row).SubItems.Add(cmd)
                Next
                curr_row += 1
                index += 1
            Next

        Catch ex As Exception
            MsgBox("載入失敗，檔案無效或不存在")
        End Try
    End Sub

    Public Sub Save_Search_Keyword_btn_Click()

        If Form1.SearchingKeyword_folder_Textbox.Text <> "" Then

            Dim keyword_folder_path = FormInit.keyword_Searching_path + "/" + Form1.SearchingKeyword_folder_Textbox.Text

            If curr_selected_feature = "搜尋" Then
                keyword_folder_path = FormInit.keyword_Searching_path + "/" + Form1.SearchingKeyword_folder_Textbox.Text
            ElseIf curr_selected_feature = "前往" Then
                keyword_folder_path = FormInit.URL_Navigation_path + "/" + Form1.SearchingKeyword_folder_Textbox.Text
            End If

            Dim keyword_file_name As String

            If Form1.Keyword_TextFileName_TextBox.Text = "" Then ' auto generate file name
                keyword_file_name = Date.Now.ToString("yyyyMMddHHmmss")
                keyword_file_name = "keyword_" + keyword_file_name + ".txt"
            Else
                keyword_file_name = Form1.Keyword_TextFileName_TextBox.Text + ".txt"
            End If

            If Not System.IO.Directory.Exists(keyword_folder_path) Then
                System.IO.Directory.CreateDirectory(keyword_folder_path)
            End If
            WriteAllText(keyword_folder_path + "/" + keyword_file_name, Form1.Searching_keyword_Text_Textbox.Text)
            Form1.Searching_Keyword_CheckedListBox.Items.Clear()

            Form1.SearchingKeyword_folder_Textbox.Clear()
            Form1.Keyword_TextFileName_TextBox.Clear()
            Form1.Searching_keyword_Text_Textbox.Clear()
            FormInit.Render_Keyword_TextFIle()

        Else
            MsgBox("資料夾名稱不得為空")
        End If

    End Sub

    Public Sub Save_Navigation_URL_btn_Click()

        If Form1.Navigation_URL_Dir_TextBox.Text <> "" Then

            Dim navigation_url_folder_path = FormInit.URL_Navigation_path + "/" + Form1.Navigation_URL_Dir_TextBox.Text
            Dim url_file_name As String

            If Form1.Navigation_URL_FIleName_TextBox.Text = "" Then ' auto generate file name
                url_file_name = Date.Now.ToString("yyyyMMddHHmmss")
                url_file_name = "url_" + url_file_name + ".txt"
            Else
                url_file_name = Form1.Navigation_URL_FIleName_TextBox.Text + ".txt"
            End If

            If Not System.IO.Directory.Exists(navigation_url_folder_path) Then
                System.IO.Directory.CreateDirectory(navigation_url_folder_path)
            End If
            WriteAllText(navigation_url_folder_path + "/" + url_file_name, Form1.Navigation_URL_FileText_TextBox.Text)
            Form1.Navigation_URL_CheckedListBox.Items.Clear()
            FormInit.Render_URL_TextFIle()

        Else
            MsgBox("資料夾名稱不得為空")
        End If

    End Sub


    Public Sub Searching_Keyword_CheckedListBox_OnClick()
        Dim myFolderPath = ""

        If curr_selected_feature = "搜尋" Then
            myFolderPath = FormInit.keyword_Searching_path
        ElseIf curr_selected_feature = "前往" Then
            myFolderPath = FormInit.URL_Navigation_path
        End If


        Dim Txt_file_path As String = ""
        For Each itemSeleted In Form1.Searching_Keyword_CheckedListBox.SelectedItems
            Txt_file_path = itemSeleted
        Next
        Form1.Searching_keyword_Text_Textbox.Text = File.ReadAllText(myFolderPath + Txt_file_path)
        Dim temp_arr = Txt_file_path.Split("\")
        Form1.Keyword_TextFileName_TextBox.Text = Path.GetFileNameWithoutExtension(Txt_file_path)
        Form1.SearchingKeyword_folder_Textbox.Text = temp_arr(temp_arr.Length - 2)


    End Sub

    Public Sub Navigation_URL_CheckedListBox_OnClick()

        Dim Txt_file_path As String = ""
        For Each itemSeleted In Form1.Navigation_URL_CheckedListBox.SelectedItems
            Txt_file_path = itemSeleted
        Next
        Form1.Navigation_URL_FileText_TextBox.Text = File.ReadAllText(FormInit.URL_Navigation_path + Txt_file_path)
        Dim temp_arr = Txt_file_path.Split("\")
        Form1.Navigation_URL_FIleName_TextBox.Text = Path.GetFileNameWithoutExtension(Txt_file_path)
        Form1.Navigation_URL_Dir_TextBox.Text = temp_arr(temp_arr.Length - 2)
    End Sub

    Public Sub Searching_Keyword_Text_SaveAs()
        Dim keyword_txt = Form1.Searching_keyword_Text_Textbox.Text
        Form1.SaveFileDialog1.Filter = "txt files (*.txt)|"
        Form1.SaveFileDialog1.DefaultExt = "txt"
        Form1.SaveFileDialog1.FilterIndex = 2
        Form1.SaveFileDialog1.RestoreDirectory = True

        If Form1.SaveFileDialog1.ShowDialog = DialogResult.OK Then
            'Debug.WriteLine(script_txt)
            WriteAllText(Form1.SaveFileDialog1.FileName, keyword_txt)
            Form1.Searching_Keyword_CheckedListBox.Items.Clear()
            FormInit.Render_Keyword_TextFIle()
        End If
    End Sub

    Public Sub Navigation_URL_Text_SaveAs()
        Dim url_txt = Form1.Navigation_URL_FileText_TextBox.Text
        Form1.SaveFileDialog1.Filter = "txt files (*.txt)|"
        Form1.SaveFileDialog1.DefaultExt = "txt"
        Form1.SaveFileDialog1.FilterIndex = 2
        Form1.SaveFileDialog1.RestoreDirectory = True

        If Form1.SaveFileDialog1.ShowDialog = DialogResult.OK Then
            'Debug.WriteLine(script_txt)
            WriteAllText(Form1.SaveFileDialog1.FileName, url_txt)
            Form1.Navigation_URL_CheckedListBox.Items.Clear()
            FormInit.Render_URL_TextFIle()
        End If
    End Sub


    Public Sub Reveal_Keyword_Folder()
        If Form1.SearchingKeyword_folder_Textbox.Text = "" Then
            MsgBox("資料夾名稱不可為空白")
            Exit Sub
        End If

        Dim mypath As String = FormInit.keyword_Searching_path + Form1.SearchingKeyword_folder_Textbox.Text

        If curr_selected_feature = "搜尋" Then
            mypath = FormInit.keyword_Searching_path + Form1.SearchingKeyword_folder_Textbox.Text
        ElseIf curr_selected_feature = "前往" Then
            mypath = FormInit.URL_Navigation_path + Form1.SearchingKeyword_folder_Textbox.Text
        End If


        If Not System.IO.Directory.Exists(mypath) Then
            System.IO.Directory.CreateDirectory(mypath)
        End If
        Process.Start("explorer.exe", mypath)
    End Sub

    Public Sub Reveal_Navigation_URL_Folder()
        If Form1.Navigation_URL_Dir_TextBox.Text = "" Then
            MsgBox("資料夾名稱不可為空白")
            Exit Sub
        End If

        Dim mypath As String = FormInit.URL_Navigation_path + Form1.Navigation_URL_Dir_TextBox.Text
        If Not System.IO.Directory.Exists(mypath) Then
            System.IO.Directory.CreateDirectory(mypath)
        End If
        Process.Start("explorer.exe", mypath)
    End Sub

    Public Sub Delete_Keyword_Folder()
        For Each itemSelected In Form1.Searching_Keyword_CheckedListBox.SelectedItems

            Dim keyword_file = FormInit.keyword_Searching_path + itemSelected

            If curr_selected_feature = "搜尋" Then
                keyword_file = FormInit.keyword_Searching_path + itemSelected
            ElseIf curr_selected_feature = "前往" Then
                keyword_file = FormInit.URL_Navigation_path + itemSelected
            End If




            If File.Exists(keyword_file) Then
                Dim result As DialogResult = MessageBox.Show("確定要刪除此檔案?", "確認訊息", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then
                    File.Delete(keyword_file)
                    Form1.Searching_Keyword_CheckedListBox.Items.Clear()
                    FormInit.Render_Keyword_TextFIle()
                    Exit Sub
                End If

            End If
        Next
    End Sub

    Public Sub Delete_Navigation_URL_File()
        For Each itemSelected In Form1.Navigation_URL_CheckedListBox.SelectedItems

            Dim url_file = FormInit.URL_Navigation_path + itemSelected
            If File.Exists(url_file) Then
                Dim result As DialogResult = MessageBox.Show("確定要刪除此檔案?", "確認訊息", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then
                    File.Delete(url_file)
                    Form1.Navigation_URL_CheckedListBox.Items.Clear()
                    FormInit.Render_URL_TextFIle()
                    Exit Sub
                End If

            End If
        Next
    End Sub

    Public Sub Auto_Generated_TextFile()
        Dim Texts() As String = Form1.Auto_GenerateTextFile_RichTextBox.Text.Split(vbLf)
        Dim file_count = 1
        For Each txt In Texts
            Debug.WriteLine("txt = " + txt)
            If txt <> "" Then
                WriteAllText(FormInit.auto_generated_textfile_path + CStr(file_count) + ".txt", txt)
                file_count += 1
            End If

        Next
    End Sub


    Public Sub Select_TextFile_For_Copy_Dialog()
        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String

        fd.Title = "Open File Dialog"
        'fd.InitialDirectory = "C:\"
        fd.Filter = "Text Files (.txt)|*.txt"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        'Dim allowed_extension = New String() {".txt"}

        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName

            If Path.GetExtension(strFileName) = ".txt" Then
                Form1.Target_TextFile_Path_For_Copy_TextBox.Text = strFileName
            Else
                MsgBox("無效的文字檔案")
            End If

        End If

    End Sub

    Public Sub Select_ImageFile_For_Copy_Dialog()
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Open File Dialog"
        'fd.InitialDirectory = "C:\"
        fd.Filter = "Image Files|*.jpeg;*.jpg;*.png;"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        fd.Multiselect = True

        'Dim allowed_extension = New String() {".jpg", ".jpeg", ".png"}

        If fd.ShowDialog() = DialogResult.OK Then
            Dim formatted_filePath = ""
            For Each fileName In fd.FileNames
                'Debug.WriteLine(fileName)
                formatted_filePath += fileName + ";"
            Next
            formatted_filePath = formatted_filePath.Trim(";")
            'Debug.WriteLine(formatted_filePath)
            Form1.Target_ImageFile_Path_For_Copy_TextBox.Text = formatted_filePath
        End If
    End Sub


    Public Sub Modify_ScriptListView_Selected_URL_item()

        Try

            If Form1.script_ListView.SelectedItems(0).SubItems.Item(4).Text <> "前往" Then
                MsgBox("僅能修改前往網址")
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("未選取任何項目")
            Exit Sub
        End Try

        Dim pattern As String
        Dim content As String = ""
        pattern = "http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?"
        If Regex.IsMatch(Form1.curr_url_ComboBox.Text, pattern) Then

            If Form1.group_name_TextBox.Text <> "" Then
                content = Form1.group_name_TextBox.Text + ";" + Form1.curr_url_ComboBox.Text
            Else
                content = Form1.curr_url_ComboBox.Text
            End If

            Form1.script_ListView.SelectedItems(0).SubItems.Item(5).Text = content
        Else
            MsgBox("網址格式錯誤")
        End If

    End Sub

    Public Sub Modify_ScriptListView_Selected_AccountAndPassword_item()

        Try

            If Form1.script_ListView.SelectedItems(0).SubItems.Item(4).Text <> "登入" Then
                MsgBox("僅能修改帳號密碼")
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("未選取任何項目")
            Exit Sub
        End Try

        Dim content = Form1.script_ListView.SelectedItems(0).SubItems.Item(5).Text
        Dim fb_email = content.Split(" ")(0).Split(":")(1)
        Dim fb_passwd = content.Split(" ")(1).Split(":")(1)

        If Form1.fb_account_TextBox.Text <> "" Then
            fb_email = Form1.fb_account_TextBox.Text
        End If

        If Form1.fb_password_TextBox.Text <> "" Then
            Dim plainText = fb_passwd
            Dim wrapper As New Simple3Des("password")
            Dim cipherText As String = wrapper.EncryptData(plainText)
            fb_passwd = cipherText
        End If

        Form1.script_ListView.SelectedItems(0).SubItems.Item(5).Text = "帳號:" + fb_email + " 密碼:" + fb_passwd

    End Sub


    Public Sub Set_Folder_Checked_with_TextFile_textbox(folder_name)

        For i As Integer = 0 To Form1.Text_File_CheckedListBox.Items.Count - 1
            Dim fd_name = Form1.Text_File_CheckedListBox.Items(i).ToString.Split("\"c)(0)
            If fd_name = folder_name Then
                Form1.Text_File_CheckedListBox.SetItemChecked(i, True)
            End If
        Next

    End Sub

    Public Sub Set_TextFile_Item_Checked(checked)

        For i As Integer = 0 To Form1.Text_File_CheckedListBox.Items.Count - 1
            Form1.Text_File_CheckedListBox.SetItemChecked(i, checked)
        Next

    End Sub


    Public Sub Set_Folder_Checked_with_ImageFile_textbox(folder_name)

        For i As Integer = 0 To Form1.img_CheckedListBox.Items.Count - 1
            Dim fd_name = Form1.img_CheckedListBox.Items(i).ToString.Split("\"c)(0)
            If fd_name = folder_name Then
                Form1.img_CheckedListBox.SetItemChecked(i, True)
            End If
        Next

    End Sub

    Public Sub Set_ImageFile_Item_Checked(checked)

        For i As Integer = 0 To Form1.img_CheckedListBox.Items.Count - 1
            Form1.img_CheckedListBox.SetItemChecked(i, checked)
        Next

    End Sub

    Public Sub RandomizeArray(ByVal items() As String)
        Dim max_index As Integer = items.Length - 1
        Dim rnd As New Random
        For i As Integer = 0 To max_index - 1
            ' Pick an item for position i.
            Dim j As Integer = rnd.Next(i, max_index + 1)
            ' Swap them.
            Dim temp As String = items(i)
            items(i) = items(j)
            items(j) = temp
        Next i
    End Sub


    Public Function Generate_Random_Password(pw_length As Integer)
        Dim allowed_Char_String As String
        Dim generated_Password As String
        allowed_Char_String = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        generated_Password = ""
        For i = 1 To pw_length
            Randomize()
            generated_Password &= Mid(allowed_Char_String, Int(Rnd() * Len(allowed_Char_String) + 1), 1)
        Next

        Return generated_Password
    End Function

End Module
