Public Class Form2
    Dim log_path = My.Computer.FileSystem.CurrentDirectory + "\logs"


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
        eventlog_ListView.Items.Clear()
        Dim log_lines = IO.File.ReadAllLines(log_path + "\selenium_log_temp.txt").Reverse()
        Dim curr_row As Integer = 0
        Dim index As Integer = IO.File.ReadAllLines(log_path + "\selenium_log_temp.txt").Length
        For Each line In log_lines
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

    Private Sub Show_err_log_btn_Click(sender As Object, e As EventArgs) Handles show_err_log_btn.Click
        eventlog_ListView.Items.Clear()
        Filter_and_update_eventlog("Error", 2)
    End Sub

    Private Sub show_all_log_btn_Click(sender As Object, e As EventArgs) Handles show_all_log_btn.Click
        Update_eventlog_listview()
    End Sub

    Private Sub eventlog_search_btn_Click(sender As Object, e As EventArgs) Handles eventlog_search_btn.Click
        Dim search_str As String = eventlog_search_TextBox.Text
        Filter_and_update_eventlog(search_str, 3)

    End Sub


    Private Sub Filter_and_update_eventlog(str As String, item As Integer) ' 2:level   3:content
        eventlog_ListView.Items.Clear()
        Dim log_lines = IO.File.ReadAllLines(log_path + "\selenium_log_temp.txt").Reverse()
        Dim curr_row As Integer = 0
        Dim index As Integer = IO.File.ReadAllLines(log_path + "\selenium_log_temp.txt").Length
        For Each line In log_lines
            Dim splittedLine() As String = line.Split(",")
            If Not splittedLine(item).Contains(str) Then
                Continue For
            End If
            eventlog_ListView.Items.Add(index.ToString)
            For Each log In splittedLine
                eventlog_ListView.Items(curr_row).SubItems.Add(log)
            Next
            curr_row += 1
            index -= 1
        Next

    End Sub
End Class