Imports System.IO
Imports System.IO.File
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports WinFormsApp1.JsonProfileInfo
Module FormComponentController

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
        End If
        Rearrange_scriptlistview_number()
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
        End If
        Rearrange_scriptlistview_number()
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

        If Form1.Text_File_CheckedListBox.SelectedIndex >= 0 Then
            Current_selected_Item = Form1.Text_File_CheckedListBox.SelectedItem.ToString()
            Selected_Counter += 1
        End If

        'Get Image CheckListBox Selected Item
        If Form1.img_CheckedListBox.SelectedIndex >= 0 Then
            Current_selected_Item = Form1.img_CheckedListBox.SelectedItem.ToString()
            Selected_Counter += 1
        End If

        If Form1.TextFolder_ListBox.SelectedIndex >= 0 Then
            Current_selected_Item = Form1.TextFolder_ListBox.SelectedItem.ToString()
            Selected_Counter += 1
        End If

        If Form1.ImageFolder_ListBox.SelectedIndex >= 0 Then
            Current_selected_Item = Form1.ImageFolder_ListBox.SelectedItem.ToString()
            Selected_Counter += 1
        End If



        If Form1.Match_Condition_ListView.SelectedItems.Count > 0 Then
            Current_selected_Item = Form1.Match_Condition_ListView.SelectedItems(0).Text + "%20" + Form1.Match_Condition_ListView.SelectedItems(0).SubItems(1).Text
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
        If Form1.Text_File_CheckedListBox.SelectedIndex >= 0 Then
            Form1.Text_File_CheckedListBox.SetSelected(Form1.Text_File_CheckedListBox.SelectedIndex, False)
        End If

        If Form1.img_CheckedListBox.SelectedIndex >= 0 Then
            Form1.img_CheckedListBox.SetSelected(Form1.img_CheckedListBox.SelectedIndex, False)
        End If

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
            Profile_Path = itemSeleted
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

    Public Sub Profile_CheckedListBox_SelectedIndexChanged()

        Dim myfile = ""
        For Each itemSelected In Form1.Profile_CheckedListBox.SelectedItems
            'Debug.WriteLine(itemSeleted)
            myfile = itemSelected + "\ProfileInfo.txt"
            Form1.Profile_TextBox.Text = itemSelected

        Next

        Debug.WriteLine(myfile)
        If My.Computer.FileSystem.FileExists(myfile) Then
            Dim JsonString As String = System.IO.File.ReadAllText(myfile)
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

            If Directory.Exists(itemSelected) Then
                Dim result As DialogResult = MessageBox.Show("確定要刪除此資料夾?", "確認訊息", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then
                    Debug.WriteLine("Delete : " + itemSelected)
                    'Delete a Directory
                    Directory.Delete(itemSelected, True)
                    Form1.Profile_CheckedListBox.Items.Clear()
                    Render_profile_CheckedListBox()
                    Exit Sub
                End If

            End If
        Next
    End Sub

End Module
