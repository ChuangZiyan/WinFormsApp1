Public Class Form2

    Private Sub form2_clear_log_btn_Click_1(sender As Object, e As EventArgs) Handles form2_clear_log_btn.Click
        form2_logs_RichTextBox.Text = ""
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub test_success_log_btn_Click(sender As Object, e As EventArgs) Handles test_success_log_btn.Click
        Dim content = "test success log"
        'Form2.form2_logs_RichTextBox.SelectedText = "Fail"
        form2_logs_RichTextBox.SelectionColor = Color.Black
        form2_logs_RichTextBox.AppendText("[ " + Date.Now.ToString("yyyy/MM/dd HH:mm:ss") + " ] - Success: " + content & vbCrLf)
        Form1.log_to_file("[ " + Date.Now.ToString("yyyy/MM/dd HH:mm:ss") + " ] - Success: " + content)
    End Sub

    Private Sub test_fail_log_btn_Click(sender As Object, e As EventArgs) Handles test_fail_log_btn.Click
        Dim content = "test fail log"
        'Form2.form2_logs_RichTextBox.SelectedText = "Fail"
        form2_logs_RichTextBox.SelectionColor = Color.Red
        form2_logs_RichTextBox.AppendText("[ " + Date.Now.ToString("yyyy/MM/dd HH:mm:ss") + " ] - Fail: " + content & vbCrLf)
        Form1.log_to_file("[ " + Date.Now.ToString("yyyy/MM/dd HH:mm:ss") + " ] - Fail: " + content)
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