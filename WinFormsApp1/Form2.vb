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
    End Sub

    Private Sub test_fail_log_btn_Click(sender As Object, e As EventArgs) Handles test_fail_log_btn.Click
        Dim content = "test fail log"
        'Form2.form2_logs_RichTextBox.SelectedText = "Fail"
        form2_logs_RichTextBox.SelectionColor = Color.Red
        form2_logs_RichTextBox.AppendText("[ " + Date.Now.ToString("yyyy/MM/dd HH:mm:ss") + " ] - Fail: " + content & vbCrLf)
    End Sub
End Class