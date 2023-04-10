Imports System.IO

Module FormInit

    Public curr_path = My.Computer.FileSystem.CurrentDirectory + "\"

    Public profile_path = curr_path + "profiles\"
    Public text_folder_path = curr_path + "resources\texts\"
    Public image_folder_path = curr_path + "resources\images\"
    Public save_script_folder_path = curr_path + "myscript\"
    Public keyword_Searching_path = curr_path + "keyword\"
    Public URL_Navigation_path = curr_path + "url\"
    Public configs_dir = curr_path + "configs\"
    Public auto_generated_textfile_path = curr_path + "auto_generated\"


    Public Sub FormInit_Render_All()

        Property_Folder_Init()
        Render_Script_listview()
        Render_Maching_condition_listview()
        Render_img_listbox()
        Render_TextFolder_listbox()
        Render_ImageFolder_listbox()
        Render_TextFile_listbox()
        Render_profile_CheckedListBox()
        Render_Lang_Packs_ComboBox()
        Render_DevList_combobox()
        Render_Groups_Listview()
        Render_ProfileName_ComboBox_Item()
        Render_My_Script_ComboBox()
        'Render_Keyword_TextFIle()
        'Render_URL_TextFIle()
        Render_Current_URL_ComboBox()
        Render_Block_Lang_Packs_ComboBox()

        Render_Window_Hwnd_Listview()



        Form1.Selection_Item_ComboBox.Text = "搜尋"
    End Sub



    Public Sub Render_Script_listview()
        Form1.script_ListView.View = View.Details
        Form1.script_ListView.GridLines = True
        Form1.script_ListView.FullRowSelect = True
        Form1.script_ListView.Columns.Add("#", 30)
        Form1.script_ListView.Columns.Add("瀏覽器", 80)
        Form1.script_ListView.Columns.Add("設備", 120)
        Form1.script_ListView.Columns.Add("名稱", 100)
        Form1.script_ListView.Columns.Add("執行動作", 100)
        Form1.script_ListView.Columns.Add("內容", 280)
        Form1.script_ListView.Columns.Add("執行結果", 75)

    End Sub

    Public Sub Render_Maching_condition_listview()
        Form1.Match_Condition_ListView.View = View.Details
        Form1.Match_Condition_ListView.GridLines = True
        Form1.Match_Condition_ListView.FullRowSelect = True
        Form1.Match_Condition_ListView.Columns.Add("文字檔案夾", 300)
        Form1.Match_Condition_ListView.Columns.Add("相片檔案夾", 300)


    End Sub

    Public Sub Render_Lang_Packs_ComboBox()

        Dim files() As String = IO.Directory.GetFiles(curr_path + "langpacks")
        For Each file As String In files
            'Debug.WriteLine(file)
            If Path.GetExtension(file) = ".json" Then
                Dim lang = Path.GetFileNameWithoutExtension(file)
                Form1.Lang_Packs_ComboBox.Items.Add(lang)
            End If
        Next
    End Sub

    Public Sub Render_Block_Lang_Packs_ComboBox()

        Dim files() As String = IO.Directory.GetFiles(curr_path + "langpacks")
        For Each file As String In files
            'Debug.WriteLine(file)
            If Path.GetExtension(file) = ".json" Then
                Dim lang = Path.GetFileNameWithoutExtension(file)
                Form1.Block_User_Lang_ComboBox.Items.Add(lang)
            End If
        Next
    End Sub

    Public Sub Property_Folder_Init()
        Dim txt_path = My.Computer.FileSystem.CurrentDirectory + "\resources\texts"
        If Not System.IO.Directory.Exists(txt_path) Then
            System.IO.Directory.CreateDirectory(txt_path)
        End If

        Dim img_path = My.Computer.FileSystem.CurrentDirectory + "\resources\images"
        If Not System.IO.Directory.Exists(img_path) Then
            System.IO.Directory.CreateDirectory(img_path)
        End If

        Dim matched_path = My.Computer.FileSystem.CurrentDirectory + "\resources\matched_resource\"
        If Not System.IO.Directory.Exists(matched_path) Then
            System.IO.Directory.CreateDirectory(matched_path)
        End If

        If Not System.IO.Directory.Exists(profile_path) Then
            System.IO.Directory.CreateDirectory(profile_path)
        End If

        Dim profileChildFolder = {"available", "ban", "useless"}
        For Each childDir In profileChildFolder
            If Not System.IO.Directory.Exists(profile_path + childDir) Then
                System.IO.Directory.CreateDirectory(profile_path + childDir)
            End If
        Next


        If Not System.IO.Directory.Exists(save_script_folder_path) Then
            System.IO.Directory.CreateDirectory(save_script_folder_path)
        End If

        If Not System.IO.Directory.Exists(keyword_Searching_path) Then
            System.IO.Directory.CreateDirectory(keyword_Searching_path)
        End If

        If Not System.IO.Directory.Exists(URL_Navigation_path) Then
            System.IO.Directory.CreateDirectory(URL_Navigation_path)
        End If

        If Not System.IO.Directory.Exists(configs_dir) Then
            System.IO.Directory.CreateDirectory(configs_dir)
        End If

        If Not System.IO.Directory.Exists(auto_generated_textfile_path) Then
            System.IO.Directory.CreateDirectory(auto_generated_textfile_path)
        End If



    End Sub

    Public Sub Render_profile_CheckedListBox()
        Form1.Profile_CheckedListBox.Items.Clear()
        Dim myFilterList As ArrayList = New ArrayList()

        'Dim myFolder = {"available", "ban", "useless"}

        If Form1.Filter_Available_Profile_CheckBox.Checked Then
            myFilterList.Add("available")
        End If

        If Form1.Filter_Ban_Profile_CheckBox.Checked Then
            myFilterList.Add("ban")
        End If

        If Form1.Filter_Useless_Profile_CheckBox.Checked Then
            myFilterList.Add("useless")
        End If

        For Each folder In myFilterList
            'Dim mypath = curr_path + "profiles\" + folder
            Dim dirs() As String = IO.Directory.GetDirectories(profile_path + folder)
            For Each dir As String In dirs
                'Debug.WriteLine(dir)
                Dim split As String() = dir.Split("\")
                Dim profile_name As String = split(split.Length - 2) + "\" + split(split.Length - 1)
                Form1.Profile_CheckedListBox.Items.Add(profile_name)
            Next
        Next



    End Sub

    Public Sub Render_TextFile_listbox()
        Dim dirs() As String = IO.Directory.GetDirectories(text_folder_path)

        For Each dir As String In dirs
            'Debug.WriteLine(dir)
            Dim files() As String = IO.Directory.GetFiles(dir)
            For Each file As String In files
                'Debug.WriteLine(file)
                If Path.GetExtension(file) = ".txt" Then
                    'Debug.WriteLine("file : " + file)
                    Dim split As String() = file.Split("\")
                    Dim parentFolder As String = split(split.Length - 2)
                    'Debug.WriteLine(parentFolder)
                    Form1.Text_File_CheckedListBox.Items.Add(parentFolder + "\" + Path.GetFileName(file))
                End If

            Next

        Next

    End Sub

    Public Sub Render_img_listbox()
        If Not System.IO.Directory.Exists(image_folder_path) Then
            System.IO.Directory.CreateDirectory(image_folder_path)
        End If
        Dim dirs() As String = IO.Directory.GetDirectories(image_folder_path)
        'image/*,image/heif,image/heic,video/*,video/mp4,video/x-m4v,video/x-matroska,.mkv
        Dim allowed_extentions As String() = {".jpg", ".jpeg", ".png", ".mp4", ".mk4"}

        For Each dir As String In dirs
            'Debug.WriteLine(dir)
            Dim files() As String = IO.Directory.GetFiles(dir)
            For Each file As String In files
                'Debug.WriteLine(file)
                If allowed_extentions.Contains(Path.GetExtension(file)) Then
                    Dim split As String() = file.Split("\")
                    Dim parentFolder As String = split(split.Length - 2)
                    Form1.img_CheckedListBox.Items.Add(parentFolder + "\" + Path.GetFileName(file))
                End If

            Next

        Next

    End Sub

    Public Sub Render_DevList_combobox()
        Dim EmulatedDevices() As String = {"iPhone SE", "iPhone XR", "Pixel 5", "Samsung Galaxy S8+", "Samsung Galaxy S20 Ultra", "iPad Air",
            "iPad Mini", "Surface Pro 7", "Surface Duo", "Galaxy Fold", "Nest Hub", "Nest Hub Max"}

        For Each dev In EmulatedDevices
            Form1.EmulatedDevice_ComboBox.Items.Add(dev)
        Next
        'DeviceType_ComboBox
    End Sub

    Public Sub Render_TextFolder_listbox()
        Dim dirs() As String = IO.Directory.GetDirectories(text_folder_path)

        For Each dir As String In dirs
            Dim split As String() = dir.Split("\")
            Dim parentFolder As String = split(split.Length - 1)
            Form1.TextFolder_ListBox.Items.Add(parentFolder)
        Next

    End Sub

    Public Sub Render_ImageFolder_listbox()
        Dim dirs() As String = IO.Directory.GetDirectories(image_folder_path)

        For Each dir As String In dirs
            Dim split As String() = dir.Split("\")
            Dim parentFolder As String = split(split.Length - 1)
            Form1.ImageFolder_ListBox.Items.Add(parentFolder)
        Next

    End Sub

    Public Sub Render_Groups_Listview()
        Form1.Groups_ListView.View = View.Details
        Form1.Groups_ListView.GridLines = True
        Form1.Groups_ListView.FullRowSelect = True
        Form1.Groups_ListView.Columns.Add("社團名稱", 300)
        Form1.Groups_ListView.Columns.Add("網址", 800)
    End Sub

    Public Sub Render_Window_Hwnd_Listview()
        Form1.Window_Hwnd_ListView.View = View.Details
        Form1.Window_Hwnd_ListView.GridLines = True
        Form1.Window_Hwnd_ListView.FullRowSelect = True
        Form1.Window_Hwnd_ListView.Columns.Add("Hwnd", 100)
        Form1.Window_Hwnd_ListView.Columns.Add("窗口名稱", 310)
        Form1.Window_Hwnd_ListView.Columns.Add("狀態", 80)
    End Sub

    Public Sub Render_ProfileName_ComboBox_Item()
        For i As Integer = 1 To 100
            Form1.Profile_Name_ComboBox.Items.Add(i.ToString().PadLeft(3, "0"))
        Next
    End Sub

    Public Sub Render_My_Script_ComboBox()

        Dim files() As String = IO.Directory.GetFiles(save_script_folder_path)
        For Each file As String In files
            'Debug.WriteLine(file)
            If Path.GetExtension(file) = ".txt" Then
                'Debug.WriteLine("file : " + file)
                'Dim split As String() = file.Split("\")
                'Dim parentFolder As String = split(split.Length - 2)
                'Debug.WriteLine(parentFolder)
                Form1.Script_Config_ComboBox.Items.Add(Path.GetFileName(file))
            End If

        Next



        'Script_Config_ComboBox
    End Sub




    Public Sub Render_Keyword_TextFIle()
        Dim myPath = keyword_Searching_path


        If curr_selected_feature = "搜尋" Then
            myPath = keyword_Searching_path
        ElseIf curr_selected_feature = "前往" Then
            myPath = URL_Navigation_path
        End If


        If Not System.IO.Directory.Exists(myPath) Then
            System.IO.Directory.CreateDirectory(myPath)
        End If
        Form1.Searching_Keyword_CheckedListBox.Items.Clear()
        Dim dirs() As String = IO.Directory.GetDirectories(myPath)
        'image/*,image/heif,image/heic,video/*,video/mp4,video/x-m4v,video/x-matroska,.mkv

        For Each dir As String In dirs
            'Debug.WriteLine(dir)
            Dim files() As String = IO.Directory.GetFiles(dir)
            For Each file As String In files
                'Debug.WriteLine(file)

                Dim split As String() = file.Split("\")
                Dim parentFolder As String = split(split.Length - 2)
                Form1.Searching_Keyword_CheckedListBox.Items.Add(parentFolder + "\" + Path.GetFileName(file))

            Next

        Next

    End Sub

    Public Sub Render_URL_TextFIle()
        If Not System.IO.Directory.Exists(URL_Navigation_path) Then
            System.IO.Directory.CreateDirectory(URL_Navigation_path)
        End If
        Form1.Searching_Keyword_CheckedListBox.Items.Clear()
        Dim dirs() As String = IO.Directory.GetDirectories(URL_Navigation_path)
        'image/*,image/heif,image/heic,video/*,video/mp4,video/x-m4v,video/x-matroska,.mkv

        For Each dir As String In dirs
            'Debug.WriteLine(dir)
            Dim files() As String = IO.Directory.GetFiles(dir)
            For Each file As String In files
                'Debug.WriteLine(file)

                Dim split As String() = file.Split("\")
                Dim parentFolder As String = split(split.Length - 2)
                Form1.Searching_Keyword_CheckedListBox.Items.Add(parentFolder + "\" + Path.GetFileName(file))

            Next

        Next

    End Sub


    Public Sub Render_Current_URL_ComboBox()
        Dim URLs = New String() {"https://www.facebook.com/", "https://www.facebook.com/me/", "https://www.youtube.com/"} 'edit your URL here
        For Each url In URLs
            Form1.curr_url_ComboBox.Items.Add(url)
        Next
    End Sub



End Module
