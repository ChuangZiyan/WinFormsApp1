Public Class Form2
    Dim log_path = My.Computer.FileSystem.CurrentDirectory + "\logs"

    Private Sub form2_clear_log_btn_Click_1(sender As Object, e As EventArgs) Handles form2_clear_log_btn.Click
        form2_logs_RichTextBox.Text = ""
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles Me.Load
        eventlog_ListView.View = View.Details
        eventlog_ListView.GridLines = True
        eventlog_ListView.FullRowSelect = True
        eventlog_ListView.Columns.Add("ID", 50)
        eventlog_ListView.Columns.Add("Date", 100)
        eventlog_ListView.Columns.Add("Time", 100)
        eventlog_ListView.Columns.Add("Level", 100)
        eventlog_ListView.Columns.Add("Content", 800)


        If Not System.IO.Directory.Exists(log_path) Then
            System.IO.Directory.CreateDirectory(log_path)
        End If

        If Not My.Computer.FileSystem.FileExists(log_path + "\selenium_log_temp.txt") Then
            Dim log_file_temp As System.IO.StreamWriter
            log_file_temp = My.Computer.FileSystem.OpenTextFileWriter(log_path + "\selenium_log_temp.txt", True)
            log_file_temp.Write("")
            log_file_temp.Close()
        End If

        Update_eventlog_listview()

    End Sub


    Public Sub Update_eventlog_listview()

        Dim log_lines = IO.File.ReadAllLines(log_path + "\selenium_log_temp.txt").Reverse()
        Dim curr_row As Integer = 0
        Dim index As Integer = IO.File.ReadAllLines(log_path + "\selenium_log_temp.txt").Count
        For Each line In log_lines
            Debug.WriteLine(line)
            Dim splittedLine() As String = line.Split(",")
            eventlog_ListView.Items.Add(index.ToString)
            For Each log In splittedLine
                eventlog_ListView.Items(curr_row).SubItems.Add(log)
            Next
            curr_row += 1
            index -= 1
        Next

    End Sub

    Public Sub EventlogListview_AddNewItem(content)
        Dim curr_row = eventlog_ListView.Items.Count + 1
        eventlog_ListView.Items.Insert(0, curr_row.ToString)

        Dim splittedLine() As String = content.Split(",")
        For Each log In splittedLine
            eventlog_ListView.Items(0).SubItems.Add(log)
        Next
    End Sub

    Private Sub test_success_log_btn_Click(sender As Object, e As EventArgs) Handles test_success_log_btn.Click
        Dim content = "test success log"
        Form1.Write_log(content)
    End Sub

    Private Sub test_fail_log_btn_Click(sender As Object, e As EventArgs) Handles test_fail_log_btn.Click
        Dim content = "test fail log"
        Form1.Write_err_log(content)
    End Sub

    Private Sub form2_save_log_btn_Click(sender As Object, e As EventArgs) Handles form2_save_log_btn.Click
        'MsgBox(My.Computer.FileSystem.CurrentDirectory)
        Dim thisDate As String = Date.Today.ToString("dd-MM-yyyy")
        'Exit Sub
        Dim log_path = My.Computer.FileSystem.CurrentDirectory + "\logs\" + thisDate
        If Not System.IO.Directory.Exists(log_path) Then
            System.IO.Directory.CreateDirectory(log_path)
        End If
        Dim log_file As System.IO.StreamWriter
        log_file = My.Computer.FileSystem.OpenTextFileWriter(log_path + "\selenium.log", True)
        log_file.WriteLine(form2_logs_RichTextBox.Text)
        log_file.Close()
        MsgBox("Save log file to " + log_path)
    End Sub
End Class