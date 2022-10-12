Imports System.IO
Imports WinFormsApp1.Form1


Public Class FormInit


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

    Public Sub Render_profile_combobox()
        Dim mypath = "C:\selenium_file\my_profile"
        Dim dirs() As String = IO.Directory.GetDirectories(mypath)
        For Each dir As String In dirs
            'Debug.WriteLine(dir)
            Form1.Chrome_Profile_ComboBox.Items.Add(dir)
        Next

    End Sub


    Public Sub Render_TextFile_listbox()
        Dim mypath = "C:\selenium_file\inventory\text"
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
        Dim files() As String = IO.Directory.GetFiles("C:\selenium_file\my_img") ' your img folder

        For Each file As String In files
            'Debug.WriteLine(file)
            Form1.img_CheckedListBox.Items.Add(file)
            Form1.reply_img_CheckedListBox.Items.Add(file)
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
        Dim mypath = "C:\selenium_file\inventory\text"
        Dim dirs() As String = IO.Directory.GetDirectories(mypath)

        For Each dir As String In dirs
            Form1.TextFolder_CheckedListBox.Items.Add(dir)
        Next

    End Sub

    Public Sub Render_ImageFolder_listbox()
        Dim mypath = "C:\selenium_file\inventory\image"
        Dim dirs() As String = IO.Directory.GetDirectories(mypath)

        For Each dir As String In dirs
            Form1.ImageFolder_CheckedListBox.Items.Add(dir)
        Next

    End Sub


End Class
