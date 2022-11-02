Imports System.IO

Module FormInit

    Public curr_path = My.Computer.FileSystem.CurrentDirectory + "\"

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

    Public Sub Property_Folder_Init()
        Dim txt_path = My.Computer.FileSystem.CurrentDirectory + "\resources\texts"
        If Not System.IO.Directory.Exists(txt_path) Then
            System.IO.Directory.CreateDirectory(txt_path)
        End If

        Dim img_path = My.Computer.FileSystem.CurrentDirectory + "\resources\images"
        If Not System.IO.Directory.Exists(img_path) Then
            System.IO.Directory.CreateDirectory(img_path)
        End If

        Dim profile_path = My.Computer.FileSystem.CurrentDirectory + "\profiles"
        If Not System.IO.Directory.Exists(profile_path) Then
            System.IO.Directory.CreateDirectory(profile_path)
        End If

    End Sub

    Public Sub Render_profile_CheckedListBox()
        Dim mypath = curr_path + "profiles"
        Dim dirs() As String = IO.Directory.GetDirectories(mypath)
        For Each dir As String In dirs
            'Debug.WriteLine(dir)
            Form1.Profile_CheckedListBox.Items.Add(dir)
        Next

    End Sub

    Public Sub Render_TextFile_listbox()
        Dim mypath = curr_path + "resources\texts"
        Dim dirs() As String = IO.Directory.GetDirectories(mypath)

        For Each dir As String In dirs
            'Debug.WriteLine(dir)
            Dim files() As String = IO.Directory.GetFiles(dir)
            For Each file As String In files
                'Debug.WriteLine(file)
                If Path.GetExtension(file) = ".txt" Then
                    Form1.Text_File_CheckedListBox.Items.Add(file)
                End If

            Next

        Next

    End Sub

    Public Sub Render_img_listbox()
        Dim mypath = curr_path + "resources\images"
        If Not System.IO.Directory.Exists(mypath) Then
            System.IO.Directory.CreateDirectory(mypath)
        End If
        Dim dirs() As String = IO.Directory.GetDirectories(mypath)
        'image/*,image/heif,image/heic,video/*,video/mp4,video/x-m4v,video/x-matroska,.mkv
        Dim allowed_extentions As String() = {".jpg", ".jpeg", ".png", ".mp4", ".mk4"}

        For Each dir As String In dirs
            'Debug.WriteLine(dir)
            Dim files() As String = IO.Directory.GetFiles(dir)
            For Each file As String In files
                'Debug.WriteLine(file)
                If allowed_extentions.Contains(Path.GetExtension(file)) Then
                    Form1.img_CheckedListBox.Items.Add(file)
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
        Dim mypath = curr_path + "resources\texts"
        Dim dirs() As String = IO.Directory.GetDirectories(mypath)

        For Each dir As String In dirs
            Form1.TextFolder_ListBox.Items.Add(dir)
        Next

    End Sub

    Public Sub Render_ImageFolder_listbox()
        Dim mypath = curr_path + "resources\images"
        Dim dirs() As String = IO.Directory.GetDirectories(mypath)

        For Each dir As String In dirs
            Form1.ImageFolder_ListBox.Items.Add(dir)
        Next

    End Sub


    Public Sub Render_Groups_Listview()
        Form1.Groups_ListView.View = View.Details
        Form1.Groups_ListView.GridLines = True
        Form1.Groups_ListView.FullRowSelect = True
        Form1.Groups_ListView.Columns.Add("社團名稱", 200)
        Form1.Groups_ListView.Columns.Add("網址", 800)
    End Sub


End Module
